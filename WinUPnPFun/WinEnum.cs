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
using PcapDotNet;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace WinUPnPFun
{
    public partial class WinEnum : Form
    {
        IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
        Dictionary<String, String> windowsVersions = new Dictionary<String, String>();
        ConcurrentDictionary<String, String> windowsDevices = new ConcurrentDictionary<String, String>();
        Int32 numberWindowsDevices = 0;
        List<String> upnpTypes = new List<String>();
        List<WebServer> webServers = new List<WebServer>();

        BackgroundWorker windowsEnumBw = new BackgroundWorker();
        BackgroundWorker fakeRouterBw = new BackgroundWorker();

        String fakeRouterName = "Click Me!";
        String fakeRouterUrl = "http://www.youtube.com/watch?v=dQw4w9WgXcQ";
        Int32 fakeRouterHowMany = 1;
        Int32 fakeMode = 0;
        public WinEnum()
        {
            InitializeComponent();

            windowsEnumBw.WorkerSupportsCancellation = true;
            windowsEnumBw.WorkerReportsProgress = true;
            windowsEnumBw.DoWork += new DoWorkEventHandler(windowsEnumBw_DoWork);
            windowsEnumBw.ProgressChanged += new ProgressChangedEventHandler(windowsEnumBw_ProgressChanged);
            windowsEnumBw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(windowsEnumBw_RunWorkerCompleted);
            fakeRouterBw.WorkerSupportsCancellation = true;
            fakeRouterBw.WorkerReportsProgress = true;
            fakeRouterBw.DoWork += new DoWorkEventHandler(fakeRouterBw_DoWork);
            fakeRouterBw.ProgressChanged += new ProgressChangedEventHandler(fakeRouterBw_ProgressChanged);
            fakeRouterBw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fakeRouterBw_RunWorkerCompleted);
        }

        public void updateDataGrid()
        {
            if (windowsDevices.Count != numberWindowsDevices)
            {
                dataGridView1.Rows.Clear();

                foreach (KeyValuePair<string, string> entry in windowsDevices)
                {
                    dataGridView1.Rows.Add(entry.Key, entry.Value);
                }

                numberWindowsDevices = windowsDevices.Count;
            }
        }


        public void osEnum(HttpListenerRequest request)
        {
            IPEndPoint source = request.RemoteEndPoint;
            String userAgent = request.UserAgent;
            if (userAgent.Contains("Microsoft") && userAgent.Contains("Windows"))
            {
                //Example: Microsoft-Windows/6.3 UPnP/1.0

                Regex regex = new Regex(@"Microsoft-Windows/(\d\.\d)");
                Match match = regex.Match(userAgent);

                if (match.Success)
                {
                    if (windowsVersions.ContainsKey(match.Groups[1].Value))
                    {

                        string windowsVersion = windowsVersions[match.Groups[1].Value];

                        if (!windowsDevices.ContainsKey(source.Address.ToString()))
                        {
                            windowsDevices.TryAdd(source.Address.ToString(), windowsVersion);
                        }
                    }
                }
            }
        }

        public string windowsEnumSendResponse(HttpListenerRequest request)
        {
            osEnum(request);
            return string.Format("Content");
        }

 
        static void notifySpoof(LivePacketDevice selectedDevice, string notifyString, string sourceIP, ushort sourcePort, string destIP, ushort destPort)
        {

            byte[] temp = System.Text.Encoding.ASCII.GetBytes(notifyString);



            EthernetLayer ethernetLayer = new EthernetLayer
            {
                Source = LivePacketDeviceExtensions.GetMacAddress(selectedDevice),
                Destination = new MacAddress("01:00:5E:7F:FF:FA"),
                EtherType = EthernetType.None,

            };

            IpV4Layer ipV4Layer = new IpV4Layer
            {
                Source = new IpV4Address(sourceIP),
                CurrentDestination = new IpV4Address(destIP),
                Fragmentation = IpV4Fragmentation.None,
                HeaderChecksum = null,

                Identification = 1,
                Options = IpV4Options.None,
                Protocol = null,
                Ttl = 64,
                TypeOfService = 0,
            };

            UdpLayer udpLayer = new UdpLayer
            {
                SourcePort = sourcePort,
                DestinationPort = destPort,
                Checksum = null,
                CalculateChecksumValue = true,
            };

            PayloadLayer payloadLayer = new PayloadLayer
            {
                Data = new Datagram(temp),
            };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);

            using (PacketCommunicator communicator = selectedDevice.Open(69559, PacketDeviceOpenAttributes.Promiscuous, 1000)) // read timeout
            {
                communicator.SendPacket(builder.Build(DateTime.Now));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!windowsEnumBw.IsBusy)
            {
                windowsEnumBw.RunWorkerAsync();
            }
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void windowsEnumBw_DoWork(object sender, DoWorkEventArgs e) {
            int ifaceID = networkInterfaces.SelectedIndex;

            LivePacketDevice device = allDevices[ifaceID];

            string srcIP = "";

            for (int i = 0; i != device.Addresses.Count; ++i)
            {
                DeviceAddress address = device.Addresses[i];
                if (address.Address.Family.ToString() == "Internet")
                {
                    string[] addressParts = address.Address.ToString().Split();
                    srcIP = addressParts[1];
                }
            }

            if (srcIP != "")
            {

                string srcURL = "http://" + srcIP + ":9090/";

                WebServer ws = new WebServer(windowsEnumSendResponse, srcURL);
                ws.Run();

                for (int i = 0; i < 10; i++)
                {
                    foreach (string upnpType in upnpTypes)
                    {

                        string notifyString = "NOTIFY * HTTP/1.1\r\n" +
                            "Cache-Control: max-age = 300\r\n" +
                            "Host: 239.255.255.250:1900\r\n" +
                            "Location: " + srcURL + Guid.NewGuid().ToString() + "\r\n" +
                            "NT: " + upnpType + "\r\n" +
                            "NTS: ssdp:alive\r\n" +
                            "SERVER: UPnP-Pentest-Toolkit\r\n" +
                            "USN: uuid:" + Guid.NewGuid().ToString() + "\r\n" +
                            "\r\n";

                        notifySpoof(device, notifyString, srcIP, 1900, "239.255.255.250", 1900);
                    }
                    Thread.Sleep(1000);
                }

                Thread.Sleep(60000);

                ws.Stop();
            }
        }
        private void windowsEnumBw_ProgressChanged(object sender, ProgressChangedEventArgs e) { }
        private void windowsEnumBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { }

        private void fakeRouterBw_DoWork(object sender, DoWorkEventArgs e) {
            int ifaceID = networkInterfaces.SelectedIndex;

            LivePacketDevice device = allDevices[ifaceID];

            string srcIP = "";

            for (int i = 0; i != device.Addresses.Count; ++i)
            {
                DeviceAddress address = device.Addresses[i];
                if (address.Address.Family.ToString() == "Internet")
                {
                    string[] addressParts = address.Address.ToString().Split();
                    srcIP = addressParts[1];
                }
            }

            if (srcIP != "")
            {
                string srcURL = "http://" + srcIP + ":9090/";

                WebServer ws = new WebServer(fakeRouterSendResponse, srcURL);
                ws.Run();

                for (int i = 0; i < fakeRouterHowMany; i++)
                {

                    foreach (string upnpType in upnpTypes)
                    {

                        string notifyString = "NOTIFY * HTTP/1.1\r\n" +
                            "Cache-Control: max-age = 300\r\n" +
                            "Host: 239.255.255.250:1900\r\n" +
                            "Location: " + srcURL + Guid.NewGuid().ToString() + "\r\n" +
                            "NT: " + upnpType + "\r\n" +
                            "NTS: ssdp:alive\r\n" +
                            "SERVER: UPnP-Pentest-Toolkit\r\n" +
                            "USN: uuid:" + Guid.NewGuid().ToString() + "\r\n" +
                            "\r\n";

                        notifySpoof(device, notifyString, srcIP, 1900, "239.255.255.250", 1900);
                      
                    }
                }

                Thread.Sleep(600000);

                ws.Stop();
            }
        }
        private void fakeRouterBw_ProgressChanged(object sender, ProgressChangedEventArgs e) { }
        private void fakeRouterBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                fakeRouterName = textBox1.Text;
            }
            if (textBox2.Text != "")
            {
                fakeRouterUrl = textBox2.Text;
            }
            if (textBox3.Text != "")
            {
                fakeRouterHowMany = Int32.Parse(textBox3.Text);
            }


            if (!fakeRouterBw.IsBusy)
            {
                fakeRouterBw.RunWorkerAsync();
            }
        }

        public string fakeRouterSendResponse(HttpListenerRequest request)
        {
            osEnum(request);

            string fakeRouterBaseDescription = "";


            if(fakeMode == 0){
                fakeRouterBaseDescription = "<?xml version=\"1.0\"?><root xmlns=\"urn:schemas-upnp-org:device-1-0\"><specVersion><major>1</major><minor>0</minor></specVersion><device><deviceType>urn:schemas-upnp-org:device:InternetGatewayDevice:1</deviceType><friendlyName>" + fakeRouterName + "</friendlyName><manufacturer>Rick</manufacturer><manufacturerURL>" + fakeRouterUrl + "</manufacturerURL><modelDescription>Rick</modelDescription><modelName>Rick</modelName><modelNumber>Rick</modelNumber><modelURL>" + fakeRouterUrl + "</modelURL><serialNumber>Rick</serialNumber><UDN>uuid:" + Guid.NewGuid().ToString() + "</UDN><iconList><icon><mimetype>image/gif</mimetype><width>48</width><height>48</height><depth>24</depth><url>icon.gif</url></icon></iconList><serviceList><service><serviceType>urn:schemas-upnp-org:service:Layer3Forwarding:1</serviceType><serviceId>urn:upnp-org:serviceId:Layer3Forwarding1</serviceId><controlURL>/ctl/L3F</controlURL><eventSubURL>/evt/L3F</eventSubURL><SCPDURL>/L3F.xml</SCPDURL></service></serviceList><deviceList><device><deviceType>urn:schemas-upnp-org:device:WANDevice:1</deviceType><friendlyName>WANDevice</friendlyName><manufacturer>MiniUPnP</manufacturer><manufacturerURL>http://www.youtube.com/watch?v=dQw4w9WgXcQ</manufacturerURL><modelDescription>WAN Device</modelDescription><modelName>WAN Device</modelName><modelNumber>20120731</modelNumber><modelURL>http://www.youtube.com/watch?v=dQw4w9WgXcQ</modelURL><serialNumber>Rick</serialNumber><UDN>uuid:" + Guid.NewGuid().ToString() + "</UDN><UPC>123456789012</UPC><serviceList><service><serviceType>urn:schemas-upnp-org:service:WANCommonInterfaceConfig:1</serviceType><serviceId>urn:upnp-org:serviceId:WANCommonIFC1</serviceId><controlURL>/ctl/CmnIfCfg</controlURL><eventSubURL>/evt/CmnIfCfg</eventSubURL><SCPDURL>/WANCfg.xml</SCPDURL></service></serviceList><deviceList><device><deviceType>urn:schemas-upnp-org:device:WANConnectionDevice:1</deviceType><friendlyName>WANConnectionDevice</friendlyName><manufacturer>Rick</manufacturer><manufacturerURL>http://www.youtube.com/watch?v=dQw4w9WgXcQ</manufacturerURL><modelDescription>MiniUPnP daemon</modelDescription><modelName>MiniUPnPd</modelName><modelNumber>20120731</modelNumber><modelURL>http://www.youtube.com/watch?v=dQw4w9WgXcQ</modelURL><serialNumber>Rick</serialNumber><UDN>uuid:" + Guid.NewGuid().ToString() + "</UDN><UPC>123456789012</UPC><serviceList><service><serviceType>urn:schemas-upnp-org:service:WANIPConnection:1</serviceType><serviceId>urn:upnp-org:serviceId:WANIPConn1</serviceId><controlURL>/ctl/IPConn</controlURL><eventSubURL>/evt/IPConn</eventSubURL><SCPDURL>/WANIPCn.xml</SCPDURL></service></serviceList></device></deviceList></device></deviceList><presentationURL>" + fakeRouterUrl + "</presentationURL></device></root>";
            }

            if(fakeMode == 1){
               fakeRouterBaseDescription = "<?xml version=\"1.0\"?><root xmlns=\"urn:schemas-upnp-org:device-1-0\"><specVersion><major>1</major><minor>0</minor></specVersion><device><UDN>uuid:" + Guid.NewGuid().ToString() + "</UDN><friendlyName>" + fakeRouterName + "</friendlyName><deviceType>urn:schemas-upnp-org:device:MediaServer:1</deviceType><manufacturer>Microsoft Corporation</manufacturer><manufacturerURL>http://www.microsoft.com</manufacturerURL><modelName>Windows Media Player Sharing</modelName><modelNumber>12.0</modelNumber><modelURL>http://go.microsoft.com/fwlink/?LinkId=105926</modelURL><serialNumber>S-1-5-21-2287713506-505016645-1800792052-1001</serialNumber><dlna:X_DLNADOC xmlns:dlna=\"urn:schemas-dlna-org:device-1-0\">DMS-1.50</dlna:X_DLNADOC><microsoft:magicPacketWakeSupported xmlns:microsoft=\"urn:schemas-microsoft-com:WMPNSS-1-0\">1</microsoft:magicPacketWakeSupported><iconList><icon><mimetype>image/jpeg</mimetype><width>120</width><height>120</height><depth>24</depth><url>/upnphost/udhisapi.dll?content=uuid:19901bf6-4490-4be2-8690-7e95234e26fc</url></icon></iconList><serviceList><service><serviceType>urn:schemas-upnp-org:service:ConnectionManager:1</serviceType><serviceId>urn:upnp-org:serviceId:ConnectionManager</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:upnp-org:serviceId:ConnectionManager</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:upnp-org:serviceId:ConnectionManager</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:2c3d8049-4823-4f2b-97a8-f7ab17c6abdb</SCPDURL></service><service><serviceType>urn:schemas-upnp-org:service:ContentDirectory:1</serviceType><serviceId>urn:upnp-org:serviceId:ContentDirectory</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:upnp-org:serviceId:ContentDirectory</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:upnp-org:serviceId:ContentDirectory</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:bf61c87d-1ac7-4f28-b621-534563eb59c9</SCPDURL></service><service><serviceType>urn:microsoft.com:service:X_MS_MediaReceiverRegistrar:1</serviceType><serviceId>urn:microsoft.com:serviceId:X_MS_MediaReceiverRegistrar</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:microsoft.com:serviceId:X_MS_MediaReceiverRegistrar</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:21afdb06-ddb1-4c7b-b94f-521e4fdcdb67+urn:microsoft.com:serviceId:X_MS_MediaReceiverRegistrar</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:66ce6d8c-7e58-428a-84ae-1b9e09f95da1</SCPDURL></service></serviceList><presentationURL>" + fakeRouterUrl + "</presentationURL></device></root>";
            }

            if (fakeMode == 2)
            {
                fakeRouterBaseDescription = "<?xml version=\"1.0\"?><root xmlns=\"urn:schemas-upnp-org:device-1-0\" xmlns:df=\"http://schemas.microsoft.com/windows/2008/09/devicefoundation\" xmlns:microsoft=\"urn:schemas-microsoft-com:WMPDMR-1-0\" xmlns:pnpx=\"http://schemas.microsoft.com/windows/pnpx/2005/11\"><specVersion><major>1</major><minor>0</minor></specVersion><device><deviceType>urn:schemas-upnp-org:device:MediaRenderer:1</deviceType><friendlyName>" + fakeRouterName + "</friendlyName><modelName>" + fakeRouterName + "</modelName><modelDescription>Digital Media Renderer</modelDescription><manufacturer>Microsoft Corporation</manufacturer><manufacturerURL>http://www.microsoft.com</manufacturerURL><modelURL>http://xbox.com</modelURL><UDN>uuid:" + Guid.NewGuid().ToString() + "</UDN><df:X_containerId>{2BDAA238-07DF-4EFA-9DA8-E7D888E0B8F6}</df:X_containerId><dlna:X_DLNACAP xmlns:dlna=\"urn:schemas-dlna-org:device-1-0\"/><dlna:X_DLNADOC xmlns:dlna=\"urn:schemas-dlna-org:device-1-0\">DMR-1.50</dlna:X_DLNADOC><pnpx:X_deviceCategory>MediaDevices</pnpx:X_deviceCategory><pnpx:X_hardwareId>LOLCAT</pnpx:X_hardwareId><iconList><icon><mimetype>image/jpeg</mimetype><width>120</width><height>120</height><depth>24</depth><url>/upnphost/udhisapi.dll?content=uuid:ce86489c-976b-4dd3-be1a-64ccddbf6031</url></icon></iconList><serviceList><service><serviceType>urn:schemas-upnp-org:service:RenderingControl:1</serviceType><serviceId>urn:upnp-org:serviceId:RenderingControl</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:RenderingControl</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:RenderingControl</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:439e9e51-d938-472d-87af-df7d7534e82e</SCPDURL></service><service><serviceType>urn:schemas-upnp-org:service:AVTransport:1</serviceType><serviceId>urn:upnp-org:serviceId:AVTransport</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:AVTransport</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:AVTransport</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:b851bd2d-4131-4b05-882b-d6a8401488cd</SCPDURL></service><service><serviceType>urn:schemas-upnp-org:service:ConnectionManager:1</serviceType><serviceId>urn:upnp-org:serviceId:ConnectionManager</serviceId><controlURL>/upnphost/udhisapi.dll?control=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:ConnectionManager</controlURL><eventSubURL>/upnphost/udhisapi.dll?event=uuid:2bdaa238-07df-4efa-9da8-e7d888e0b8f6+urn:upnp-org:serviceId:ConnectionManager</eventSubURL><SCPDURL>/upnphost/udhisapi.dll?content=uuid:11219e36-01b6-4390-870d-1efe62b73f45</SCPDURL></service></serviceList><presentationURL>" + fakeRouterUrl + "</presentationURL></device></root>";
            }


            return fakeRouterBaseDescription;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fakeMode = comboBox1.SelectedIndex;
        }

        private void WinEnum_Load(object sender, EventArgs e)
        {
                        if (!IsAdministrator())
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

            windowsVersions.Add("6.4", "Windows 10");
            windowsVersions.Add("6.3", "Windows 8.1");
            windowsVersions.Add("6.2", "Windows 8");
            windowsVersions.Add("6.1", "Windows 7");
            windowsVersions.Add("6.0", "Windows Vista");
            windowsVersions.Add("5.2", "Windows XP Pro x64");
            windowsVersions.Add("5.1", "Windows XP");

            upnpTypes.Add("upnp:rootdevice");
            upnpTypes.Add("urn:schemas-upnp-org:device:InternetGatewayDevice:1");
            upnpTypes.Add("urn:schemas-upnp-org:service:WANPPPConnection:1");
            upnpTypes.Add("urn:schemas-upnp-org:service:WANIPConnection:1");
            upnpTypes.Add("urn:schemas-upnp-org:service:WANPPPConnection:1");

            comboBox1.Items.Add("Router");
            comboBox1.Items.Add("Media Server");
            comboBox1.Items.Add("Media Player");
            comboBox1.SelectedIndex = 0;

            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];

                string ifaceIP = "";

                for (int j = 0; j != device.Addresses.Count; ++j)
                {
                    DeviceAddress address = device.Addresses[j];
                    if (address.Address.Family.ToString() == "Internet")
                    {
                        string[] addressParts = address.Address.ToString().Split();
                        ifaceIP = addressParts[1];
                    }
                }

                networkInterfaces.Items.Add(String.Format("{0} {2} {1}", ifaceIP.ToString(), device.Name, device.Description != null ? String.Format(" ({0})", device.Description) : "[nNo device description]"));
          
            }
        }

        private void guiTimer_Tick(object sender, EventArgs e)
        {
            updateDataGrid();
        }
    }
}
