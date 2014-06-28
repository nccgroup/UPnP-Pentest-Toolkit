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
using System.Text.RegularExpressions;

namespace WinUPnPFun
{
    public partial class WindowsMedia : Form
    {

        String coURL;
        Form1.Target myTarget;
        int thisValue = 0;
        int addThis = 1;
        List<String> containers = new List<String>();
        List<String> containers2 = new List<String>();
        List<String> titles = new List<String>();
        List<String> titlesPreview = new List<String>();
        BackgroundWorker bw = new BackgroundWorker();
        String actionTypeVar = "";

        public WindowsMedia(Form1.Target target, String controlURL, String actionType)
        {
            coURL = controlURL;
            myTarget = target;
            actionTypeVar = actionType;

            InitializeComponent();

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            find_content(false, null, true);
            titlesPreview = titles;
            worker.ReportProgress(33);
            find_content(true, containers, true);
            titlesPreview = titles;
            worker.ReportProgress(66);
            find_content(true, containers2, false);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            treeView1.Nodes.Clear();
            String[] uniqueTitles = titles.Distinct().ToArray();

            treeView1.BeginUpdate();
            foreach (string title in uniqueTitles)
            {

                treeView1.Nodes.Add(title);

            }
            treeView1.EndUpdate();
            loadingImage.Visible = false;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            treeView1.Nodes.Clear();
            String[] uniqueTitles = titlesPreview.Distinct().ToArray();

            treeView1.BeginUpdate();
            foreach (string title in uniqueTitles)
            {

                treeView1.Nodes.Add(title);

            }
            treeView1.EndUpdate();
            loadingImage.Visible = false;
        }

        public static XmlDocument sendWindowsMediaRenderingControl(string url, string soap, string function)
        {
            string req = "<?xml version=\"1.0\"?>" +
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
            "<s:Body>" +
            soap +
            "</s:Body>" +
            "</s:Envelope>";
            WebRequest r = HttpWebRequest.Create(url);
            r.Timeout = 10000;
            r.Method = "POST";
            byte[] b = Encoding.UTF8.GetBytes(req);
            r.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:RenderingControl:1#" + function + "\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";
            r.ContentLength = b.Length;
            r.GetRequestStream().Write(b, 0, b.Length);
            XmlDocument resp = new XmlDocument();
            WebResponse wres = r.GetResponse();
            Stream ress = wres.GetResponseStream();
            resp.Load(ress);
            return resp;
        }

        public static XmlDocument setVolume(string url, int volume)
        {
            string soap = "<m:SetVolume xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" +
                    "<InstanceID>0</InstanceID>" +
                    "<Channel>Master</Channel>" +
                    "<DesiredVolume>"+volume.ToString()+"</DesiredVolume>" +
                    "</m:SetVolume>";
            string function = "SetVolume";
            XmlDocument response;
            try
            {
                response = sendWindowsMediaRenderingControl(url, soap, function);
            }
            catch
            {
                response = new XmlDocument();
            }
            return response;
        }

        private void WindowsMedia_Load(object sender, EventArgs e)
        {
            label1.Text = myTarget.targetService.Device.FriendlyName;

            if (actionTypeVar == "Browse")
            {
                trackBar1.Enabled = false;
                checkBox1.Enabled = false;
                textBox1.Enabled = false;
            }
            else
            {
                button1.Enabled = false;
                treeView1.Enabled = false;
            }
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            string resultText = setVolume(coURL, trackBar1.Value).OuterXml;
            
            if(resultText != ""){
                textBox1.Text = resultText;
            }
            else
            {
                textBox1.Text = "Error";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer1.Start();

            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thisValue == 100)
            {
                addThis = -1;
            }
            else if (thisValue == 0)
            {
                addThis = 1;
            }

            trackBar1.Value = thisValue;
            thisValue = thisValue + addThis;
            string resultText = setVolume(coURL, trackBar1.Value).OuterXml;

            if (resultText != "")
            {
                textBox1.Text = resultText;
            }
            else
            {
                textBox1.Text = "Error";
            }
        }

        private void find_content(bool use_list, List<String> myContainers, bool store)
        {
            if (use_list == false)
            {
                for (int i = 0; i < 5; i++)
                {
                    string response_string = "";
                    try
                    {
                        object[] test = myTarget.targetService.InvokeAction("Browse", new object[] { i, "BrowseDirectChildren", "*", 0, 0, "" });
                        for (int k = 0; k < test.Count(); k++)
                        {
                            String nR = test[k].ToString();
                            response_string = response_string + nR;
                            
                        }

                            string pattern = @"container id=\""([^\""]+)\""";
                            MatchCollection matches1 = Regex.Matches(response_string, pattern);
                            foreach (Match match in matches1)
                            {
                                if (match.Success)
                                {
                                    containers.Add(match.Groups[1].Value);
                                }
                            }

                        pattern = "<dc:title>([^<]+)</dc:title>";
                            MatchCollection matches2 = Regex.Matches(response_string, pattern);
                            foreach (Match match in matches2)
                            {
                                if (match.Success)
                                {
                                    titles.Add(match.Groups[1].Value);
                                }
                            }
                      
                    }
                    catch
                    {

                    }

                }
            }
            else
            {
                foreach (string container in myContainers)
                {
                    string response_string = "";
                    try
                    {
                        object[] test = myTarget.targetService.InvokeAction("Browse", new object[] { container, "BrowseDirectChildren", "*", 0, 0, "" });
                        for (int k = 0; k < test.Count(); k++)
                        {
                            String nR = test[k].ToString();
                            response_string = response_string + nR;
                        }
                        string pattern = "";

                        if (store == true)
                        {
                            pattern = "container id=\"([^\"]+)\"";
                            MatchCollection matches3 = Regex.Matches(response_string, pattern);
                            foreach (Match match in matches3)
                            {
                                if (match.Success)
                                {

                                    containers2.Add(match.Groups[1].Value);

                                }
                            }
                        }

                        pattern = "<dc:title>([^<]+)</dc:title>";
                        MatchCollection matches4 = Regex.Matches(response_string, pattern);
                        foreach (Match match in matches4)
                        {
                            if (match.Success)
                            {
                                titles.Add(match.Groups[1].Value);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                        
            if (!bw.IsBusy)
            {
                loadingImage.Visible = true;
                bw.RunWorkerAsync();
            }
           
        }


    }
}
