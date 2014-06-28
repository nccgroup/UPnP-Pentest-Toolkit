/*

Released as open source by NCC Group Plc - http://www.nccgroup.com/

Developed by David.Middlehurst (@dtmsecurity), david dot middlehurst at nccgroup dot com

https://github.com/nccgroup/UPnP-Pentest-Toolkit

Released under AGPL see LICENSE for more information

This tool is a proof of concept and is intended to be used for research purposes in a trusted environment.

*/
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WinUPnPFun
{
    public class WebServer
    {
        public Learn.device device;
        public string ErrorMessage = "";
        public string mimicDeviceLog = "";
        public string fuzzCase = "";

        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            try
            {

                _listener.Start();
            }
            catch(Exception err)
            {
                ErrorMessage = "Could not start web server, run as Administrator!" + err.ToString();
            }
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

        public static byte[] CreateAnImage()
        {
            var bmp = new System.Drawing.Bitmap(120,120);
            Image srcImage = global::WinUPnPFun.Properties.Resources.ncclogo;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(srcImage, new Rectangle(0, 0, 120, 120));
            var memStream = new MemoryStream();
            bmp.Save(memStream, ImageFormat.Jpeg);
            return memStream.ToArray();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx.Request.HttpMethod == "SUBSCRIBE")
                                {
                                    _responderMethod(ctx.Request);
                                    string rstr = "";
                                    byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.AddHeader("SID","UPnPPentestToolkit-CallBack-SID");
                                    ctx.Response.AddHeader("TIMEOUT","Second-3600");
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                }
                                else if (ctx.Request.HttpMethod == "POST")
                                {
                                    _responderMethod(ctx.Request);

                                    if (device == null)
                                    {

                                        string soapAction = "";

                                        if (ctx.Request.Headers["SOAPACTION"] != null)
                                        {
                                            soapAction = ctx.Request.Headers["SOAPACTION"];
                                            if (soapAction.Contains("#"))
                                            {
                                                string[] soapActionParts = soapAction.Split(new string[] { "#" }, StringSplitOptions.None);
                                                soapAction = soapActionParts[1];
                                                soapAction = soapAction.Replace("\"", "");
                                            }
                                        }
                                        else
                                        {
                                            soapAction = "Browse";
                                        }

                                        string rstr = "";

                                        if (soapAction.Contains("Browse"))
                                        {

                                            rstr =
                                                "<?xml version=\"1.0\"?>" +
                                                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                                                "<SOAP-ENV:Body>" +
                                                "<m:BrowseResponse xmlns:m=\"urn:schemas-upnp-org:service:ContentDirectory:1\">" +
                                                "<Result xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">&lt;DIDL-Lite xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\" xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\"&gt;&lt;container id=\"0\" restricted=\"1\" parentID=\"-1\" childCount=\"0\"&gt;&lt;dc:title&gt;UPnP-Pentest-Toolkit&lt;/dc:title&gt;&lt;upnp:class&gt;object.container&lt;/upnp:class&gt;&lt;/container&gt;&lt;/DIDL-Lite&gt;</Result>" +
                                                "<NumberReturned xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">1</NumberReturned>" +
                                                "<TotalMatches xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">1</TotalMatches>" +
                                                "<UpdateID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">0</UpdateID>" +
                                                "</m:BrowseResponse>" +
                                                "</SOAP-ENV:Body>" +
                                                "</SOAP-ENV:Envelope>";

                                        }
                                        if (soapAction.Contains("GetSearchCapabilities"))
                                        {
                                            rstr =
                                                "<?xml version=\"1.0\"?>" +
                                                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                                                "<SOAP-ENV:Body>" +
                                                "<m:GetSearchCapabilitiesResponse xmlns:m=\"urn:schemas-upnp-org:service:ContentDirectory:1\">" +
                                                "<SearchCaps xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">@id,@refID,dc:title,upnp:class,upnp:genre,upnp:artist,upnp:author,upnp:author@role,upnp:album,dc:creator,res@size,res@duration,res@protocolInfo,res@protection,dc:publisher,dc:language,upnp:originalTrackNumber,dc:date,upnp:producer,upnp:rating,upnp:actor,upnp:director,upnp:toc,dc:description,microsoft:userRatingInStars,microsoft:userEffectiveRatingInStars,microsoft:userRating,microsoft:userEffectiveRating,microsoft:serviceProvider,microsoft:artistAlbumArtist,microsoft:artistPerformer,microsoft:artistConductor,microsoft:authorComposer,microsoft:authorOriginalLyricist,microsoft:authorWriter,upnp:userAnnotation,upnp:channelName,upnp:longDescription,upnp:programTitle,upnp:episodeNumber</SearchCaps>" +
                                                "</m:GetSearchCapabilitiesResponse>" +
                                                "</SOAP-ENV:Body>" +
                                                "</SOAP-ENV:Envelope>";
                                        }
                                        if (soapAction.Contains("GetSortCapabilities"))
                                        {
                                            rstr =
                                                "<?xml version=\"1.0\"?>" +
                                                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                                                "<SOAP-ENV:Body>" +
                                                "<m:GetSortCapabilitiesResponse xmlns:m=\"urn:schemas-upnp-org:service:ContentDirectory:1\">" +
                                                "<SortCaps xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">dc:title,upnp:genre,upnp:album,dc:creator,res@size,res@duration,res@bitrate,dc:publisher,dc:language,upnp:originalTrackNumber,dc:date,upnp:producer,upnp:rating,upnp:actor,upnp:director,upnp:toc,dc:description,microsoft:year,microsoft:userRatingInStars,microsoft:userEffectiveRatingInStars,microsoft:userRating,microsoft:userEffectiveRating,microsoft:serviceProvider,microsoft:artistAlbumArtist,microsoft:artistPerformer,microsoft:artistConductor,microsoft:authorComposer,microsoft:authorWriter,microsoft:sourceUrl,upnp:userAnnotation,upnp:channelName,upnp:longDescription,upnp:programTitle,upnp:episodeNumber</SortCaps>" +
                                                "</m:GetSortCapabilitiesResponse>" +
                                                "</SOAP-ENV:Body>" +
                                                "</SOAP-ENV:Envelope>";
                                        }

                                        byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                        ctx.Response.ContentLength64 = buf.Length;
                                        ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                                        ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                                    }
                                    else
                                    {
                                        string incomingSOAPAction;

                                        if (ctx.Request.Headers["SOAPACTION"] != null)
                                        {
                                            incomingSOAPAction = ctx.Request.Headers["SOAPACTION"];
                                            if (incomingSOAPAction.Contains("#"))
                                            {
                                                string[] incomingSOAPActionParts = incomingSOAPAction.Split(new string[] { "#" }, StringSplitOptions.None);
                                                incomingSOAPAction = incomingSOAPActionParts[1];
                                                incomingSOAPAction = incomingSOAPAction.Replace("\"", "");
                                            }
                                        }
                                        else
                                        {
                                            incomingSOAPAction = "Browse";
                                        }

                                        if (device.actions.ContainsKey(incomingSOAPAction))
                                        {
                                            string rstr = device.actions[incomingSOAPAction];

                                            if (fuzzCase == "")
                                            {


                                                rstr = rstr.Replace("<FUZZ-string-HERE>", "UPT");
                                                rstr = rstr.Replace("<FUZZ-i4-HERE>", "1");
                                                rstr = rstr.Replace("<FUZZ-ui2-HERE>", "1");
                                                rstr = rstr.Replace("<FUZZ-ui4-HERE>", "1");
                                                rstr = rstr.Replace("<FUZZ-int-HERE>", "1");
                                                rstr = rstr.Replace("<FUZZ-bin.base64-HERE>", "UPT");
                                                rstr = rstr.Replace("<FUZZ-bool-HERE>", "true");
                                                rstr = rstr.Replace("<FUZZ-boolean-HERE>", "true");


                                                foreach (string dataType in device.actionDataTypes[incomingSOAPAction])
                                                {
                                                    rstr = rstr.Replace("<DEFAULT-FUZZ-" + dataType + "-HERE>", "");
                                                    rstr = rstr.Replace("<FUZZ-" + dataType + "-HERE>", "");

                                                }
                                            }
                                            else
                                            {
                                                foreach (string dataType in device.actionDataTypes[incomingSOAPAction])
                                                {
                                                    rstr = rstr.Replace("<FUZZ-" + dataType + "-HERE>", fuzzCase);
                                                    rstr = rstr.Replace("<DEFAULT-FUZZ-" + dataType + "-HERE>", "");
                                                }
                                            }


                                              string requestContent = "";
                                              if (ctx.Request.HasEntityBody)
                                              {

                                                  using (System.IO.Stream body = ctx.Request.InputStream) // here we have data
                                                  {
                                                      using (System.IO.StreamReader reader = new System.IO.StreamReader(body, ctx.Request.ContentEncoding))
                                                      {
                                                          requestContent = reader.ReadToEnd();
                                                      }
                                                  }
                                              }


                                            string requestFrom = ctx.Request.RemoteEndPoint.ToString();

                                            mimicDeviceLog = "Source: " + requestFrom + "\r\n\r\n" + "SOAP Action: " + incomingSOAPAction + "\r\n" + "\r\nRequest:\r\n" + requestContent + "\r\n\r\nResponse:\r\n"+ rstr + "\r\n---\r\n"+ mimicDeviceLog;


                                            byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                            ctx.Response.ContentLength64 = buf.Length;
                                            ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                                        }
                                        else
                                        {
                                            string rstr = "No such action";
                                            byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                            ctx.Response.ContentLength64 = buf.Length;
                                            ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                        }
                                    }
                                }else if(ctx.Request.RawUrl.Contains("?image")){
                                    _responderMethod(ctx.Request);
                                    byte[] buf = CreateAnImage();
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                }
                                else if (ctx.Request.RawUrl.Contains("localResourceID="))
                                {
                                    string[] recourceURLParts = ctx.Request.RawUrl.Split(new string[] {"?localResourceID="}, StringSplitOptions.None);
                                    string resourceID = recourceURLParts[1].Trim();
                                    if (device != null)
                                    {
                                        Byte[] resourceData = device.downloadedURLs[resourceID];
                                        ctx.Response.ContentLength64 = resourceData.Length;
                                        ctx.Response.ContentType = device.mimeTypes[resourceID];
                                        ctx.Response.OutputStream.Write(resourceData, 0, resourceData.Length);
                                    }
                                    else
                                    {
                                        Console.Write("No device resource\r\n");
                                    }

                                }
                                else
                                {
                                    string rstr = _responderMethod(ctx.Request);
                                    byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                }
                            }
                            catch { }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception err) { Console.Write(err.ToString()); }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}