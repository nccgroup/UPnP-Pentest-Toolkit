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
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WinUPnPFun
{
    public partial class RequestSender : Form
    {
        BackgroundWorker bw = new BackgroundWorker();
        XmlDocument resp = new XmlDocument();
        string myAction;
        string myUrl;
        string mySoapRequest;
        string myServiceIdentifier;
        string responseText = "";

        public RequestSender(string actionIn, string url, string soapRequest, string serviceIdentifier)
        {
            myAction = actionIn;
            myUrl = url;
            mySoapRequest = soapRequest;
            myServiceIdentifier = serviceIdentifier;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadingImage.Visible = true;
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
            else
            {
                bw.CancelAsync();
                loadingImage.Visible = false;
                resp = new XmlDocument();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebRequest r = HttpWebRequest.Create(controlURL.Text);
                r.Timeout = 10000;
                r.Method = "POST";
                byte[] b = Encoding.UTF8.GetBytes(requestEdit.Text);
                r.Headers.Add("SOAPACTION", "\"" + serviceIdent.Text.Trim() + "#" + actionName.Text.Trim() + "\"");
                r.ContentType = "text/xml; charset=\"utf-8\"";
                r.ContentLength = b.Length;
                r.GetRequestStream().Write(b, 0, b.Length);
                XmlDocument resp = new XmlDocument();
                WebResponse wres = r.GetResponse();
                Stream ress = wres.GetResponseStream();
                resp.Load(ress);
                responseText = resp.OuterXml;
            }
            catch(Exception err) {
                   if (err is WebException && ((WebException)err).Status==WebExceptionStatus.ProtocolError)
                   {
                      WebResponse errResp = ((WebException)err).Response;
                      using(Stream respStream = errResp.GetResponseStream())
                      {
                          StreamReader streamReader = new StreamReader(respStream, true);
                          try
                          {
                              responseText = streamReader.ReadToEnd();
                          }
                          finally
                          {
                              streamReader.Close();
                          }
                      }
                   }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingImage.Visible = false;
            response.Text = responseText;
            HighlightRTF(response);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { }

        private void RequestSender_Load(object sender, EventArgs e)
        {
            actionName.Text = myAction;
            controlURL.Text = myUrl;
            requestEdit.Text = mySoapRequest;
            HighlightRTF(requestEdit);
            requestEdit.Select(0, 0);
            serviceIdent.Text = myServiceIdentifier;
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
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
                int startNodeName=0, startAtt =0;
                for (int i = 0; i < nodeText.Length; ++i)
                {
                    if (nodeText[i] == '"')
                        inString = !inString;
                    
                    if(inString && nodeText[i] == '"')
                        lastSt = i;
                    else
                        if (nodeText[i] == '"')
                        {
                            rtb.Select(lastSt + st +2, i-lastSt - 1);
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
                                rtb.Select(startNodeName + st,i - startNodeName + 1);
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
                                rtb.Select(startAtt + st, i- startAtt+1);
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
                    rtb.Select(st+1, nodeText.Length);
                    rtb.SelectionColor = HighlightColors.HC_NODE;
                }

            }
        }

        private void requestEdit_MouseDown(object sender, MouseEventArgs e)
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

                requestEdit.ContextMenu = contextMenu;
            }
        }

        void requestCutAction(object sender, EventArgs e)
        {
            requestEdit.Cut();
        }

        void requestCopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(requestEdit.SelectedText);
        }

        void requestPasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                requestEdit.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
            HighlightRTF(requestEdit);
        }

        private void response_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(responseCutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(responseCopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(responsePasteAction);
                contextMenu.MenuItems.Add(menuItem);

                response.ContextMenu = contextMenu;
            }
        }
        void responseCutAction(object sender, EventArgs e)
        {
            response.Cut();
        }

        void responseCopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(response.SelectedText);
        }

        void responsePasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                response.Text
                    += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
            HighlightRTF(response);
        }

    }
}
