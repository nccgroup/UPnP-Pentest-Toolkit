/*

Released as open source by NCC Group Plc - http://www.nccgroup.com/

Developed by David.Middlehurst (@dtmsecurity), david dot middlehurst at nccgroup dot com

https://github.com/nccgroup/UPnP-Pentest-Toolkit

Released under AGPL see LICENSE for more information

This tool is a proof of concept and is intended to be used for research purposes in a trusted environment.

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagedUPnP;
using ManagedUPnP.Descriptions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Collections;
using System.Collections.Concurrent;

namespace WinUPnPFun
{

    public partial class Form1 : Form
    {

        Discovery mdDiscovery;


        // Background Worker used for updating treeView
        BackgroundWorker bw = new BackgroundWorker();
        // Background Worker used for single device enum
        BackgroundWorker deviceBw = new BackgroundWorker();

        // Stores devices identified and the Target objects that are subsequently generated
        BlockingCollection<ManagedUPnP.Device> QuickDevices = new BlockingCollection<Device>();
        BlockingCollection<ManagedUPnP.Device> Devices = new BlockingCollection<Device>();
        BlockingCollection<String> alreadyHaveDevice = new BlockingCollection<string>();
        BlockingCollection<Target> Targets = new BlockingCollection<Target>();
        BlockingCollection<String> devicesWithError = new BlockingCollection<string>();
        // Caches Control URLs
        Dictionary<string, string> ControlURLs = new Dictionary<string, string>();
        // treeView 
        BlockingCollection<TreeNode> treeData = new BlockingCollection<TreeNode>();
        List<String> RootNodes = new List<String>();
        // Old treeView index
        int tid = 0;
        // Trigger for the treeView to be updated
        bool treeUpdateRequired = false;
        // Discovery Stats
        int deviceFoundCount = 0;
        int deviceDoneCount = 0;
        int deviceTimedOut = 0;
        int deviceWantToEnum = 0;
        int targetFoundCount = 0;
        int targetDoneCount = 0;
        string timedOut = "";
        string enumTextVar = "Enumerating devices";
        bool scanningForDevices = true;
        // Log Viewer
        LogViewer logViewer;

        // Enumeration Mode
        bool enumAll = false;

        // Current ProgressBar Percentage
        int currentProgressBar = 0;

        // Define what a Target looks like
        public class Target
        {
            public int targetId { get; set; }
            public Service targetService { get; set; }
            public ServiceDescription targetServiceDesc { get; set; }
            public ActionDescription actionDesc { get; set; }
            public string soapRequest { get; set; }
            public string soapResponse { get; set; }
            public List<String> dataTypes { get; set; }
            public string controlURL { get; set; }
            public string documentURL { get; set; }
            public string friendlyName { get; set; }
            public string ServiceTypeIdentifier { get; set; }
            public bool notEnum { get; set; }
            public Device thisDevice { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            deviceBw.WorkerSupportsCancellation = true;
            deviceBw.WorkerReportsProgress = true;
            deviceBw.DoWork += new DoWorkEventHandler(deviceBw_DoWork);
            deviceBw.ProgressChanged += new ProgressChangedEventHandler(deviceBw_ProgressChanged);
            deviceBw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(deviceBw_RunWorkerCompleted);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logViewer = new LogViewer();

            Thread thread = new Thread(startEnum);
            thread.IsBackground = true;
            thread.Start();
        }


        private void startEnum()
        {
            ManagedUPnP.Logging.LogLines += new LogLinesEventHandler(ManagedUPnPLog);
            ManagedUPnP.Logging.Enabled = true;

            mdDiscovery = new Discovery(null,AddressFamilyFlags.IPv4,true);

            mdDiscovery.DeviceAdded += new DeviceAddedEventHandler(mdDiscovery_DeviceAdded);
            mdDiscovery.SearchComplete += new SearchCompleteEventHandler(mdDiscovery_SearchComplete);
            mdDiscovery.Start();
        }

        void mdDiscovery_SearchComplete(object sender, SearchCompleteEventArgs e)
        {
            enumTextVar = "Devices Enumerated";
            scanningForDevices = false;
        }

        void mdDiscovery_DeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            QuickDevices.Add((ManagedUPnP.Device)e.Device);
            Devices.Add((ManagedUPnP.Device)e.Device);
            treeUpdateRequired = true;
        }

        private void ManagedUPnPLog(object sender, LogLinesEventArgs a)
        {
            string lsDateTime = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff] ");
            string lsLineStart = lsDateTime + new String(' ', a.Indent * 4);
            logViewer.log = lsLineStart + a.Lines.Replace("\r\n", "\r\n" + lsLineStart) + "\r\n";
        }

        private BlockingCollection<Target> getServiceInfo(Service nService)
        {

                BlockingCollection<Target> nextTargetsToAdd = new BlockingCollection<Target>();

                try
                {
                    ServiceDescription nDesc = nService.Description();
                    ActionsDescription nActions = nDesc.Actions;

                    foreach (ActionDescription nAction in nActions.Values)
                    {
                        string controlURL = "";
                        if (ControlURLs.ContainsKey(String.Concat(nService.Device.DocumentURL, nService.ServiceTypeIdentifier)))
                        {
                            controlURL = ControlURLs[String.Concat(nService.Device.DocumentURL, nService.ServiceTypeIdentifier)];
                        }
                        else
                        {
                            controlURL = GetServiceUrl(nService.Device.DocumentURL, nService.ServiceTypeIdentifier);
                            ControlURLs.Add(String.Concat(nService.Device.DocumentURL, nService.ServiceTypeIdentifier), controlURL);
                        }

                        Target thisTarget = new Target();
                        thisTarget.targetService = nService;
                        thisTarget.targetServiceDesc = nDesc;
                        thisTarget.actionDesc = nAction;
                        thisTarget.targetId = tid;
                        thisTarget.controlURL = controlURL;
                        thisTarget.documentURL = nService.Device.DocumentURL;
                        thisTarget.friendlyName = nService.Device.FriendlyName;
                        thisTarget.ServiceTypeIdentifier = nService.ServiceTypeIdentifier;
                        thisTarget.notEnum = false;

                        nextTargetsToAdd.Add(thisTarget);
                        tid++;                      
                    }
                }
                catch (System.Net.WebException ex)
                {
                    nextTargetsToAdd = new BlockingCollection<Target>();
                }
                return nextTargetsToAdd;
        }




        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            treeData = new BlockingCollection<TreeNode>();
            RootNodes.Clear();
            
            int i = -1;
            int j = 0;

            targetDoneCount = Targets.Count();

            foreach (Target myTarget in Targets)
            {

                if (!RootNodes.Contains(myTarget.documentURL))
                {
                    i++;
                    j = 0;

                    TreeNode newNode = new TreeNode();
                    newNode.Name = myTarget.targetId.ToString();

                    Font boldFont = new Font(treeView1.Font, FontStyle.Bold);
                    newNode.NodeFont = boldFont;

                    newNode.Text = myTarget.friendlyName;
                    newNode.Text = newNode.Text;

                    newNode.ToolTipText = "Device Description:\r\n" + myTarget.friendlyName +
                         "\r\nDocument URL:\r\n" + myTarget.documentURL +
                        "\r\nControl URL:\r\n" + myTarget.controlURL + "\r\n";
                    newNode.Tag = myTarget;

                    treeData.Add(newNode);

                    RootNodes.Add(myTarget.documentURL);
                }
                else
                {
                    i = RootNodes.IndexOf(myTarget.documentURL);
                    j = treeData.ElementAt(i).GetNodeCount(false);
                }

                string makeSOAPRequest = "<?xml version=\"1.0\"?>\r\n" +
                                         "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
                                         "<s:Body>\r\n" +
                                         "<m:" + myTarget.actionDesc.Name + " xmlns:m=\"" + myTarget.ServiceTypeIdentifier + "\">\r\n";

                string makeSOAPResponse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                          "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
                                          "<s:Body>\r\n" +
                                          "<u:" + myTarget.actionDesc.Name + " xmlns:u=\"" + myTarget.ServiceTypeIdentifier + "\">";

                List<String> collectDataType = new List<string>();


                bool alreadyHasAction = false;

                foreach (TreeNode checkNode in treeData.ElementAt(i).Nodes)
                {
                    if (checkNode.Text == myTarget.actionDesc.Name)
                    {
                        alreadyHasAction = true;
                    }
                }

                if (!alreadyHasAction)
                {
                    treeData.ElementAt(i).Nodes.Add(myTarget.targetId.ToString(), myTarget.actionDesc.Name);
                    treeData.ElementAt(i).Nodes[j].Tag = myTarget;

                    foreach (ArgumentDescription nArg in myTarget.actionDesc.Arguments.Values)
                    {
                        if (nArg.Direction == "in")
                        {
                            TreeNode argumentNode = treeData.ElementAt(i).Nodes[j].Nodes.Add(myTarget.targetId.ToString(), String.Format("->{0}", nArg.Name));
                            argumentNode.Tag = myTarget;

                            argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Data Type: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType));
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Default Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue));
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues.Count > 0)
                            {
                                int av = 1;
                                foreach (string allowedValue in myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues)
                                {
                                    argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Allowed Value {0}: {1}", av.ToString(), allowedValue));
                                    av++;
                                }
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Minimum != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Minimum Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Minimum));
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Maximum Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum));
                            }

                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue != "")
                            {
                                makeSOAPRequest = makeSOAPRequest + "<" + nArg.Name + ">" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue + "</" + nArg.Name + ">\r\n";
                            }
                            else if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum != "")
                            {
                                makeSOAPRequest = makeSOAPRequest + "<" + nArg.Name + ">" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum + "</" + nArg.Name + ">\r\n";
                            }
                            else if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues.Count > 0)
                            {
                                makeSOAPRequest = makeSOAPRequest + "<" + nArg.Name + ">" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues[0] + "</" + nArg.Name + ">\r\n";
                            }
                            else
                            {
                                makeSOAPRequest = makeSOAPRequest + "<" + nArg.Name + ">" + "?" + "</" + nArg.Name + ">\r\n";
                            }

                        }
                        else
                        {
                            TreeNode argumentNode = treeData.ElementAt(i).Nodes[j].Nodes.Add(myTarget.targetId.ToString(), String.Format("<-{0}", nArg.Name));

                            argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Data Type: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType));
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Default Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue));
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues.Count > 0)
                            {
                                int av = 1;
                                foreach (string allowedValue in myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues)
                                {
                                    argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Allowed Value {0}: {1}", av.ToString(), allowedValue));
                                    av++;
                                }
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Minimum != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Minimum Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Minimum));
                            }
                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum != "")
                            {
                                argumentNode.Nodes.Add(myTarget.targetId.ToString(), String.Format("Maximum Value: {0}", myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum));
                            }

                            if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue != "")
                            {
                                makeSOAPResponse = makeSOAPResponse + "<" + nArg.Name + "><DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE>" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DefaultValue + "<DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE></" + nArg.Name + ">\r\n";
                            }
                            else if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum != "")
                            {
                                makeSOAPResponse = makeSOAPResponse + "<" + nArg.Name + "><DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE>" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedRange.Maximum + "<DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE></" + nArg.Name + ">\r\n";
                            }
                            else if (myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues.Count > 0)
                            {
                                makeSOAPResponse = makeSOAPResponse + "<" + nArg.Name + "><DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE>" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].AllowedValues[0] + "<DEFAULT-FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE></" + nArg.Name + ">\r\n";
                            }
                            else
                            {
                                makeSOAPResponse = makeSOAPResponse + "<" + nArg.Name + ">" + "<FUZZ-" + myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType + "-HERE>" + "</" + nArg.Name + ">\r\n";
                            }

                            collectDataType.Add(myTarget.targetServiceDesc.StateVariables[nArg.RelatedStateVariable].DataType);

                        }
                    }
                    j++;
                }

                makeSOAPRequest = makeSOAPRequest + "</m:" + myTarget.actionDesc.Name + ">\r\n" +
                                                    "</s:Body>\r\n" +
                                                    "</s:Envelope>\r\n";

                makeSOAPResponse = makeSOAPResponse + "</u:" + myTarget.actionDesc.Name + ">\r\n" +
                                                    "</s:Body>\r\n" +
                                                    "</s:Envelope>\r\n";

                myTarget.soapRequest = makeSOAPRequest;
                myTarget.soapResponse = makeSOAPResponse;
                myTarget.dataTypes = collectDataType;

            }

            foreach (Device quickListDevice in Devices)
            {
                if (!RootNodes.Contains(quickListDevice.DocumentURL))
                {
                    TreeNode newNode = new TreeNode();
                    newNode.Text = quickListDevice.FriendlyName;
                    newNode.ToolTipText = "Device Description:\r\n" + quickListDevice.FriendlyName +
                        "\r\nDocument URL:\r\n" + quickListDevice.DocumentURL + "\r\n";

                    Target deviceTarget = new Target();

                    deviceTarget.friendlyName = quickListDevice.FriendlyName;
                    deviceTarget.controlURL = "";
                    deviceTarget.documentURL = quickListDevice.DocumentURL;
                    deviceTarget.soapRequest = "";
                    deviceTarget.soapResponse = "";
                    deviceTarget.notEnum = true;
                    deviceTarget.thisDevice = quickListDevice;
                    newNode.Tag = deviceTarget;

                    if (devicesWithError.Contains(quickListDevice.DocumentURL))
                    {
                        newNode.ForeColor = System.Drawing.Color.Red;
                        newNode.Text = newNode.Text + " (error)";
                    }


                    treeData.Add(newNode);

                    RootNodes.Add(quickListDevice.DocumentURL);
                }
            }
            deviceFoundCount = RootNodes.Count();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            treeView1.Visible = false;

            treeView1.Nodes.Clear();
            treeView1.BeginUpdate();

            foreach (TreeNode newNode in treeData)
            {
                treeView1.Nodes.Add(newNode);
            }

            treeView1.EndUpdate();

            treeView1.TreeViewNodeSorter = new NodeSorter();
            treeView1.Sort();

            treeUpdateRequired = false;

            treeView1.Visible = true;

            //Fix Bold Text Issue
            for (int fn = 0; fn < treeView1.Nodes.Count; fn++)
            {
                treeView1.Nodes[fn].Text += "";
            }

            saveResultsToolStripMenuItem.Enabled = true;
        }

        public class NodeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                return string.Compare(tx.ToolTipText, ty.ToolTipText);
            }
        }


        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        public string GetServiceUrl(string resp, string serviceType)
        {
            XmlDocument desc = new XmlDocument();
            try
            {
                WebRequest r = HttpWebRequest.Create(resp);
                r.Timeout = 10000;
                WebResponse wres = r.GetResponse();
                Stream ress = wres.GetResponseStream();
                desc.Load(ress);

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
                nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");

                XmlNode node = desc.SelectSingleNode("//tns:service[tns:serviceType=\"" + serviceType + "\"]/tns:controlURL/text()", nsMgr);

                Uri baseURI = new Uri(resp);
                Uri coURL = new Uri(baseURI, node.Value);

                return coURL.AbsoluteUri;
            }
            catch (Exception)
            {
                return "";
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
                if (e.Node.Level > 0)
                {
                    Target tmp;
                    
                    if (e.Node.Level > 1)
                    {
                        if (e.Node.Parent.Tag != null)
                        {
                            tmp = (Target)e.Node.Parent.Tag;
                        }
                        else
                        {
                            tmp = (Target)e.Node.Parent.Parent.Tag;
                        }
                    }
                    else
                    {
                        tmp = (Target)e.Node.Tag;
                    }



                            textBox1.Text = tmp.documentURL;
                            textBox2.Text = tmp.controlURL;
                            textBox3.Text = tmp.actionDesc.Name;
                            if (tmp.soapRequest != null)
                            {
                                textBox4.Text = tmp.soapRequest;
                                HighlightRTF(textBox4);
                            }
                            textBox5.Text = tmp.friendlyName;
                }
                else
                {
                        Target tmp = (Target)e.Node.Tag;

                        if (tmp.notEnum)
                        {
                            if (!deviceBw.IsBusy)
                            {
                                button7.Text = "Enumerating " + tmp.friendlyName;
                                button7.Enabled = false;
                                deviceWantToEnum++;
                                deviceBw.RunWorkerAsync(tmp.thisDevice);
                            }
                        }

                        textBox1.Text = tmp.documentURL;
                        textBox2.Text = tmp.controlURL;
                        textBox3.Text = "No action selected";
                        if (tmp.soapRequest != null)
                        {
                            textBox4.Text = tmp.soapRequest;
                            HighlightRTF(textBox4);
                        }
                        textBox5.Text = tmp.friendlyName;
                    
                }
        }
     

        private void button2_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
            {
                MessageBox.Show("Please wait for enumeration to complete.",
                               "Please wait",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (treeView1.GetNodeCount(false) > 0)
                {
                    if ((treeView1.SelectedNode != null)&&(treeView1.SelectedNode.Level > 0))
                    {
                        Target tmp = (Target)treeView1.SelectedNode.Tag;

                        if ((tmp.actionDesc.Name == "Browse") || (tmp.actionDesc.Name == "SetVolume"))
                        {
                            WindowsMedia windowsMedia = new WindowsMedia(tmp, tmp.controlURL,tmp.actionDesc.Name);
                            windowsMedia.Show();
                        }
                        else
                        {
                            MessageBox.Show("Select either a 'Browse' or 'SetVolume' action for a media player target.",
                              "Selected action was not 'Browse' or 'SetVolume'",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Exclamation,
                              MessageBoxDefaultButton.Button1);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Select either a 'Browse' or 'SetVolume' action for a media player target.",
                                         "No target selected",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button1);
                    }

                }
                else
                {
                    MessageBox.Show("Please enumerate a device before using this tool.",
                                                   "No devices enumerated yet",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                }
            }
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {

                string elevation = "This feature requires to create web servers and does so using the HTTPListener class. " +
                         "To listen externally this either requires admin privileges or you have to add each URL using netsh." +
                         "You can either click 'Yes' to elevate this application (Only use in a trusted environment) or click 'No' and you can click through the individual elevation prompts that netsh requires to add each URL." +
                         "When you close the application you will then have to click through the elevation prompts for removing the URLs.";

                DialogResult dialogResult = MessageBox.Show(elevation, "Not running as Administrator. Do you wish to elevate?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Application.ExecutablePath;
                    proc.Verb = "runas";

                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        return;
                    }

                    Application.Exit();
                }
           
            }
            ComeGetIt comegetit = new ComeGetIt();
            comegetit.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MSearchSpoof msearchspoof = new MSearchSpoof();
            msearchspoof.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (bw.IsBusy)
            {
                MessageBox.Show("Please wait for enumeration to complete.",
                               "Please wait",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (Targets.Count > 0)
                {
                    HolePunch holepunch = new HolePunch(Targets);
                    holepunch.Show();
                }
                else
                {
                    MessageBox.Show("There are no targets yet.",
                                                   "No targets",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

                if (treeView1.GetNodeCount(false) > 0)
                {
                    if (treeView1.SelectedNode != null)
                    {
                        if (treeView1.SelectedNode.Level > 0)
                        {
                            Target tmp;

                            if (treeView1.SelectedNode.Level > 1)
                            {
                                if (treeView1.SelectedNode.Parent.Tag != null)
                                {
                                    tmp = (Target)treeView1.SelectedNode.Parent.Tag;
                                }
                                else
                                {
                                    tmp = (Target)treeView1.SelectedNode.Parent.Parent.Tag;
                                }
                            }
                            else
                            {
                                tmp = (Target)treeView1.SelectedNode.Tag;
                            }


                            if (tmp.soapRequest != null)
                            {

                                string tmpAction = tmp.actionDesc.Name;
                                string tmpCoUrl = tmp.controlURL;
                                string tmpSoap = tmp.soapRequest;
                                string tmpSvcIdent = tmp.targetService.ServiceTypeIdentifier;
                                RequestSender requestsender = new RequestSender(tmpAction, tmpCoUrl, tmpSoap, tmpSvcIdent);
                                requestsender.Show();
                            }


                        }
                        else
                        {
                            MessageBox.Show("No action selected.",
                                                   "Select the action of a target first.",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                        }

                    }
                    else
                    {
                        MessageBox.Show("No action selected.",
                                                   "Select the action of a target first.",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show("There are no targets yet.",
                                                   "No targets",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string nodeContents(TreeNodeCollection nodeCollection,string depth)
        {
            string outputText = "";
            string myDepth = depth;

            foreach (TreeNode node in nodeCollection)
            {

                if (node.Nodes.Count > 0)
                {
                    myDepth = myDepth + "\t";
                    outputText = outputText + myDepth + node.Text + "\r\n" + nodeContents(node.Nodes, myDepth);
                }
                else
                {
                    outputText = outputText + myDepth + "\t" + node.Text + "\r\n";
                }
                myDepth = depth;
            }
            return outputText;
        }

        private void saveResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outputText = "";
            string sep = "---------------------------------------------\r\n";

            foreach (TreeNode node in treeView1.Nodes)
            {
                outputText = outputText + "Device URL:\r\n" + node.Text + "\r\n";
                if (node.ToolTipText != "")
                {
                    outputText = outputText + node.ToolTipText + "\r\nActions:\r\n";
                }
                outputText = outputText + nodeContents(node.Nodes, "") + "\r\n" + sep +"\r\n";
            }

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        System.IO.StreamWriter resultsFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                        string resultsHeader = String.Format("{0}UPnP Pentest Toolkit\r\nDTM, NCC Group\r\nResults Saved: {1}\r\n{0}\r\n", sep, DateTime.Now.ToUniversalTime().ToString("r"));
                        resultsFile.WriteLine(resultsHeader);
                        resultsFile.WriteLine(outputText);
                        resultsFile.Close();
                    }
                }
            }
            catch
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if ((deviceDoneCount > 0)||(deviceTimedOut > 0))
            {
                enumStatus.Text = String.Format("{0}/{1} devices enumerated{2}", deviceDoneCount, deviceFoundCount, timedOut);
            }
            else
            {
                enumStatus.Text = enumTextVar;
            }

            if (treeUpdateRequired || (targetDoneCount != targetFoundCount))
            {
                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }
            }

            if (!scanningForDevices)
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
            }

            if (deviceBw.IsBusy)
            {
                if (deviceWantToEnum > 0)
                {
                    int percent = (int)Math.Ceiling(Decimal.Divide((deviceDoneCount + deviceTimedOut), deviceWantToEnum) * (decimal)100);
                    if ((percent >= 0) && (percent <= 100))
                    {
                        if (currentProgressBar != percent)
                        {
                            progressBar1.Value = percent;

                        }
                        currentProgressBar = percent;
                    }
                }
            }
            else
            {
                progressBar1.Value = 100;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (bw.IsBusy)
            {
                MessageBox.Show("Please wait for enumeration to complete.",
                               "Please wait",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (Targets.Count > 0)
                {
                   Learn learn = new Learn(Targets);
                   learn.Show();
                }
                else
                {
                    MessageBox.Show("Please enumerate devices for at least one target",
                                                   "No devices enumerated",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Exclamation,
                                                   MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            logViewer.Show();
        }

        private void enumAllDevices(Device myDevice, int beforeEnum)
        {
            if (!alreadyHaveDevice.Contains(myDevice.UniqueDeviceName))
            {
                int newServicesAdded = 0;
                Services myServices = myDevice.Services;

                foreach (Service myService in myServices)
                {
                    BlockingCollection<Target> nextTargetsToAdd = new BlockingCollection<Target>();
                    nextTargetsToAdd = getServiceInfo(myService);
                    if (nextTargetsToAdd.Count > 0)
                    {
                        foreach (Target nextTargetToAdd in nextTargetsToAdd)
                        {
                            Targets.Add(nextTargetToAdd);
                            targetFoundCount++;
                        }
                        newServicesAdded++;

                    }
                }
                if ((beforeEnum == deviceDoneCount) && (newServicesAdded > 0))
                {
                    deviceDoneCount++;
                    treeUpdateRequired = true;
                }

                if (beforeEnum == deviceDoneCount)
                {
                    deviceTimedOut++;
                    timedOut = String.Format(" ({0} error)", deviceTimedOut);
                    devicesWithError.Add(myDevice.DocumentURL);
                    treeUpdateRequired = true;
                }



                if (myDevice.HasChildren)
                {
                    foreach (Device childDevice in myDevice.Children)
                    {
                        enumAllDevices(childDevice,beforeEnum);
                    }
                }

            }
            enumTextVar = "Enumeration complete";
            alreadyHaveDevice.Add(myDevice.UniqueDeviceName);
        }


        private void deviceBw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (enumAll)
            {
                enumTextVar = "Enumerating services";

                foreach (Device myDevice in Devices)
                {
                    enumAllDevices(myDevice,deviceDoneCount);
                }
                enumAll = false;
            }
            else
            {
                Device myDevice = (Device)e.Argument;
                enumAllDevices(myDevice,deviceDoneCount);
            }

        }
        private void deviceBw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { }
        private void deviceBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button7.Text = "Enumerate All Devices";
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            deviceWantToEnum = deviceFoundCount;
            enumAll = true;
            if (!deviceBw.IsBusy)
            {
                button7.Text = "Enumerating All Devices";
                button7.Enabled = false;
                deviceBw.RunWorkerAsync();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {

                string elevation = "This feature requires to create web servers and does so using the HTTPListener class. " +
                         "To listen externally this either requires admin privileges or you have to add each URL using netsh." +
                         "You can either click 'Yes' to elevate this application (Only use in a trusted environment)";

                DialogResult dialogResult = MessageBox.Show(elevation, "Not running as Administrator. Do you wish to elevate?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Application.ExecutablePath;
                    proc.Verb = "runas";

                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        return;
                    }

                    Application.Exit();
                }

            }
            else
            {
                WinEnum winEnum = new WinEnum();
                winEnum.Show();
            }
        }

        public class HighlightColors
        {
            public static Color HC_NODE = Color.Firebrick;
            public static Color HC_STRING = Color.Blue;
            public static Color HC_ATTRIBUTE = Color.Red;
            public static Color HC_COMMENT = Color.GreenYellow;
            public static Color HC_INNERTEXT = Color.Black;
        }

        // http://blogs.msdn.com/b/cobold/archive/2011/01/31/xml-highlight-in-richtextbox.aspx
        public static void HighlightRTF(RichTextBox rtb)
        {
            int k = 0;

            string str = rtb.Text;

            int st, en;
            int lasten = -1;
            while (k < str.Length)
            {
                st = str.IndexOf('<', k);

                if (st < 0)
                    break;

                if (lasten > 0)
                {
                    rtb.Select(lasten + 1, st - lasten - 1);
                    rtb.SelectionColor = HighlightColors.HC_INNERTEXT;
                }

                en = str.IndexOf('>', st + 1);
                if (en < 0)
                    break;

                k = en + 1;
                lasten = en;

                if (str[st + 1] == '!')
                {
                    rtb.Select(st + 1, en - st - 1);
                    rtb.SelectionColor = HighlightColors.HC_COMMENT;
                    continue;

                }
                String nodeText = str.Substring(st + 1, en - st - 1);


                bool inString = false;

                int lastSt = -1;
                int state = 0;
                /* 0 = before node name
                 * 1 = in node name
                   2 = after node name
                   3 = in attribute
                   4 = in string
                   */
                int startNodeName = 0, startAtt = 0;
                for (int i = 0; i < nodeText.Length; ++i)
                {
                    if (nodeText[i] == '"')
                        inString = !inString;

                    if (inString && nodeText[i] == '"')
                        lastSt = i;
                    else
                        if (nodeText[i] == '"')
                        {
                            rtb.Select(lastSt + st + 2, i - lastSt - 1);
                            rtb.SelectionColor = HighlightColors.HC_STRING;
                        }

                    switch (state)
                    {
                        case 0:
                            if (!Char.IsWhiteSpace(nodeText, i))
                            {
                                startNodeName = i;
                                state = 1;
                            }
                            break;
                        case 1:
                            if (Char.IsWhiteSpace(nodeText, i))
                            {
                                rtb.Select(startNodeName + st, i - startNodeName + 1);
                                rtb.SelectionColor = HighlightColors.HC_NODE;
                                state = 2;
                            }
                            break;
                        case 2:
                            if (!Char.IsWhiteSpace(nodeText, i))
                            {
                                startAtt = i;
                                state = 3;
                            }
                            break;

                        case 3:
                            if (Char.IsWhiteSpace(nodeText, i) || nodeText[i] == '=')
                            {
                                rtb.Select(startAtt + st, i - startAtt + 1);
                                rtb.SelectionColor = HighlightColors.HC_ATTRIBUTE;
                                state = 4;
                            }
                            break;
                        case 4:
                            if (nodeText[i] == '"' && !inString)
                                state = 2;
                            break;


                    }

                }
                if (state == 1)
                {
                    rtb.Select(st + 1, nodeText.Length);
                    rtb.SelectionColor = HighlightColors.HC_NODE;
                }

            }
        }


        void requestCutAction(object sender, EventArgs e)
        {
            textBox4.Cut();
        }

        void requestCopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox4.SelectedText);
        }

        void requestPasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textBox4.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
            HighlightRTF(textBox4);
        }

        private void textBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(requestCutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(requestCopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(requestPasteAction);
                contextMenu.MenuItems.Add(menuItem);

                textBox4.ContextMenu = contextMenu;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }
    }
}
