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
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Collections.Concurrent;

namespace WinUPnPFun
{
    public partial class HolePunch : Form
    {
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bw2 = new BackgroundWorker();
        BlockingCollection<Form1.Target> Targets = new BlockingCollection<Form1.Target>();
        int selectedTarget = -1;
        string workerAction = "";
        string logText = "";
        List<ComboboxItem> myComboBox = new List<ComboboxItem>();
        List<Object[]> tableData = new List<Object[]>();


        public HolePunch(BlockingCollection<Form1.Target> existingTargets)
        {
            Targets = existingTargets;
            InitializeComponent();
        }

        private void find_port_map_device()
        {
            if (!bw.IsBusy)
            {
                loadingImage.Visible = true;
                bw.RunWorkerAsync();
            }
        }

        private void HolePunch_Load(object sender, EventArgs e)
        {
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            bw2.WorkerSupportsCancellation = true;
            bw2.WorkerReportsProgress = true;
            bw2.DoWork += new DoWorkEventHandler(bw2_DoWork);
            bw2.ProgressChanged += new ProgressChangedEventHandler(bw2_ProgressChanged);
            bw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw2_RunWorkerCompleted);

            find_port_map_device();
        }

        private void list_port_mapping_entries(Form1.Target myTarget)
        {
            tableData.Clear();

            System.Net.IPAddress[] DeviceIPs = myTarget.targetService.Device.RootHostAddresses;
            System.Net.IPAddress DeviceIP = DeviceIPs[0];
            String TargetIP = DeviceIP.ToString();
            tableData.Add(new object[] { String.Format("[{0} - {1}]", TargetIP, myTarget.targetService.Device.FriendlyName) });
            try
            {
                for (int i = 0; i < 100; i++)
                {

                    object[] inObj = new object[] { i };

                    object[] outObj = myTarget.targetService.InvokeAction("GetGenericPortMappingEntry", inObj);

                    tableData.Add(outObj);

                    
                }
            }
            catch
            {
            }
        }

        public string GetServiceUrl(string resp)
        {
            XmlDocument desc = new XmlDocument();
            try
            {
                desc.Load(WebRequest.Create(resp).GetResponse().GetResponseStream());

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
                nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");

                XmlNode node = desc.SelectSingleNode("//tns:service/tns:controlURL/text()", nsMgr);

                Uri baseURI = new Uri(resp);
                Uri coURL = new Uri(baseURI, node.Value);

                return coURL.AbsoluteUri;
            }
            catch (Exception err)
            {
                portMapDeviceLog.Text = err.ToString();

                return "";
            }

        }

        public static XmlDocument addPortMapping(string url, string serviceIdentifier, string NewRemoteHost, string NewExternalPort, string NewProtocol, string NewInternalPort, string NewInternalClient, string NewEnabled, string NewPortMappingDescription, string NewLeaseDuration)
        {
            string req = "<?xml version=\"1.0\"?>"+
                         "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">"+
                         "<s:Body>"+
                         "<m:AddPortMapping xmlns:m=\""+serviceIdentifier+"\">"+
                         "<NewRemoteHost>"+NewRemoteHost+"</NewRemoteHost>"+
                         "<NewExternalPort>"+NewExternalPort+"</NewExternalPort>"+
                         "<NewProtocol>"+NewProtocol+"</NewProtocol>"+
                         "<NewInternalPort>"+NewInternalPort+"</NewInternalPort>"+
                         "<NewInternalClient>"+NewInternalClient+"</NewInternalClient>"+
                         "<NewEnabled>"+NewEnabled+"</NewEnabled>"+
                         "<NewPortMappingDescription>"+NewPortMappingDescription+"</NewPortMappingDescription>"+
                         "<NewLeaseDuration>"+NewLeaseDuration+"</NewLeaseDuration>"+
                         "</m:AddPortMapping>"+
                         "</s:Body>"+
                         "</s:Envelope>";

            WebRequest r = HttpWebRequest.Create(url);
            r.Timeout = 10000;
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\""+serviceIdentifier+"#AddPortMapping\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }

        public static XmlDocument deletePortMapping(string url, string serviceIdentifier, string NewRemoteHost, string NewExternalPort, string NewProtocol)
        {
            string req = "<?xml version=\"1.0\"?>" +
                         "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                         "<s:Body>" +
                         "<m:DeletePortMapping xmlns:m=\"" + serviceIdentifier + "\">" +
                         "<NewRemoteHost>" + NewRemoteHost + "</NewRemoteHost>" +
                         "<NewExternalPort>" + NewExternalPort + "</NewExternalPort>" +
                         "<NewProtocol>" + NewProtocol + "</NewProtocol>" +
                         "</m:DeletePortMapping>" +
                         "</s:Body>" +
                         "</s:Envelope>";

            WebRequest r = HttpWebRequest.Create(url);
            r.Timeout = 10000;
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\"" + serviceIdentifier + "#DeletePortMapping\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }


        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            int portMappingDevicesFound = 0;
            myComboBox.Clear();
            foreach (Form1.Target myTarget in Targets)
            {
                bool foundPortMappingDevice = false;
                
                if(myTarget.actionDesc.Name == "AddPortMapping")
                {
                    logText = String.Format("{0} seems to allow portmapping. It has an 'AddPortMapping' method.\r\n{1}",myTarget.targetService.Device.FriendlyName,logText);
                    foundPortMappingDevice = true;
                }

                if (myTarget.actionDesc.Name == "GetGenericPortMappingEntry")
                {
                    logText = String.Format("{0} seems to allow portmapping. It has an 'GetGenericPortMappingEntry' method.\r\n{1}", myTarget.targetService.Device.FriendlyName, logText);
                    foundPortMappingDevice = true;
                }


                if (foundPortMappingDevice)
                {

                    logText = String.Format("Attempting to list port mapping entries.\r\n{0}", logText);
                    list_port_mapping_entries(myTarget);
                    System.Net.IPAddress[] DeviceIPs = myTarget.targetService.Device.RootHostAddresses;
                    System.Net.IPAddress DeviceIP = DeviceIPs[0];
                    String TargetIP = DeviceIP.ToString();

                    ComboboxItem item = new ComboboxItem();
                    item.Text = String.Format("[{0} - {1}]", TargetIP, myTarget.targetService.Device.FriendlyName);
                    item.Value = i;

                    myComboBox.Add(item);
                    portMappingDevicesFound++;
                }


                i++;
            }
            if (portMappingDevicesFound == 0)
            {
                logText = String.Format("No devices capable of port mapping were found.\r\n");
            }
   
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            portMappingDevices.Items.Clear();
            loadingImage.Visible = false;

            portMapDeviceLog.Text = logText;

            foreach (ComboboxItem myItem in myComboBox)
            {
                portMappingDevices.Items.Add(myItem);
            }

            foreach (Object[] outObj in tableData)
            {
                portMappingTable.Rows.Add(outObj);
            }


            if (portMappingDevices.Items.Count > 0)
            {
                portMappingDevices.SelectedIndex = 0;
            }

        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { }

        private void portMapDeviceLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void portMappingDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem selectedItem = portMappingDevices.SelectedItem as ComboboxItem;
            selectedTarget = selectedItem.Value;
        }

        private void portMappingTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                try
                {
                    remoteHostEntry.Text = "";
                    externalPortEntry.Text = "";
                    protocolEntry.Text = "";
                    internalPortEntry.Text = "";
                    internalClientEntry.Text = "";
                    enabledEntry.Text = "";
                    portMappingDescEntry.Text = "";
                    leaseDurationEntry.Text = "";


                    remoteHostEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                    externalPortEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[1].Value.ToString();
                    protocolEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[2].Value.ToString();
                    internalPortEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[3].Value.ToString();
                    internalClientEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[4].Value.ToString();
                    enabledEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[5].Value.ToString();
                    portMappingDescEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[6].Value.ToString();
                    leaseDurationEntry.Text = portMappingTable.Rows[e.RowIndex].Cells[7].Value.ToString();
                }
                catch
                {

                }
        }

        private void bw2_DoWork(object sender, DoWorkEventArgs e) {

            if (selectedTarget != -1)
            {
                if (workerAction == "add")
                {
                    try
                    {
                        Form1.Target myTarget = Targets.ElementAt(selectedTarget);

                        string coURL = GetServiceUrl(myTarget.targetService.Device.DocumentURL);
                        string serviceIdent = myTarget.targetService.ServiceTypeIdentifier;

                        addPortMapping(coURL, serviceIdent, remoteHostEntry.Text, externalPortEntry.Text, protocolEntry.Text, internalPortEntry.Text, internalClientEntry.Text, enabledEntry.Text, portMappingDescEntry.Text, leaseDurationEntry.Text);
                    }
                    catch
                    {

                    }
                }

                if (workerAction == "del")
                {
                    try
                    {
                        Form1.Target myTarget = Targets.ElementAt(selectedTarget);

                        string coURL = GetServiceUrl(myTarget.targetService.Device.DocumentURL);
                        string serviceIdent = myTarget.targetService.ServiceTypeIdentifier;

                        deletePortMapping(coURL, serviceIdent, remoteHostEntry.Text, externalPortEntry.Text, protocolEntry.Text);
                    }
                    catch
                    {

                    }
                }
            }
        }
        private void bw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bw.IsBusy)
            {
                loadingImage.Visible = false;

                if (workerAction == "add")
                {
                    portMapDeviceLog.Text = "Added Port Mapping\r\nAttempting to refresh port mapping entries.\r\n";
                }
                if (workerAction == "del")
                {
                    portMapDeviceLog.Text = "Deleted Port Mapping\r\nAttempting to refresh port mapping entries.\r\n";
                }
            }
            portMappingTable.Rows.Clear();
            find_port_map_device();
        }
        private void bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { }

        private void addButton_Click(object sender, EventArgs e)
        {
            workerAction = "add";
            if (!bw2.IsBusy)
            {
                logText = "";
                portMapDeviceLog.Text = "";
                loadingImage.Visible = true;
                bw2.RunWorkerAsync();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            workerAction = "del";
            if (!bw2.IsBusy)
            {
                logText = "";
                portMapDeviceLog.Text = "";
                loadingImage.Visible = true;
                bw2.RunWorkerAsync();
            }
        }

 
    }
}
