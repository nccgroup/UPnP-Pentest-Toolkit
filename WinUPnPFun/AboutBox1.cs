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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WinUPnPFun
{
    partial class AboutBox1 : Form
    {
        BackgroundWorker bw = new BackgroundWorker();
        string currentVersion = "1.1";
        string latestVersion = "";

        public AboutBox1()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (!bw.IsBusy)
            {
                button1.Text = "Please wait";
                bw.RunWorkerAsync();
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string url = "https://dtm.co.uk/upnp_pentest_toolkit/version.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader objReader = new StreamReader(resStream);
                latestVersion = objReader.ReadToEnd().Trim();
            }
            catch
            {
                latestVersion = "";
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Text = "Check Latest Version";
            if (latestVersion == "")
            {
                MessageBox.Show("Could not make a HTTP request to get latest version. No internet connectivity?",
                               "Could not check latest version",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button1);
            }else{
                if (latestVersion == currentVersion)
                {
                    button1.Text = "You are up to date";
                    MessageBox.Show(String.Format("The latest version is: {0}",latestVersion),"You are up to date");
                }
                else
                {
                    button1.Text = String.Format("Please update to version {0}", latestVersion);

                    string messageContent = String.Format("The latest version is: {0}\r\nPlease update to the latest version for new features and bug fixes!", latestVersion);
                    MessageBox.Show(messageContent,
                                   "There is a newer version available",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Exclamation,
                                   MessageBoxDefaultButton.Button1);

                    button1.Text = String.Format("Please update to version {0}", latestVersion);
                }
            }

        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { }

        private void AboutBox1_Load(object sender, EventArgs e)
        {
            labelVersion.Text = String.Format("Version: {0}", currentVersion);
        }
    }
}
