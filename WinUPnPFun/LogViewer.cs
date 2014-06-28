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

namespace WinUPnPFun
{
    public partial class LogViewer : Form
    {
        string localLog;
        bool updateRequired = false;
        double elapsed = 0;

        public LogViewer()
        {
            InitializeComponent();
        }

        public string log {
                set{
                    localLog += value;
                    updateRequired = true;
                }
        }

        private void LogViewer_Load(object sender, EventArgs e)
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(LogViewer_FormClosing);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (updateRequired)
            {
                richTextBox1.Text = localLog;
                updateRequired = false;
            }

            elapsed = elapsed + 0.1;
            textBox1.Text = Math.Floor(elapsed).ToString() + " seconds elapsed";
        }

        private void LogViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

    }
}
