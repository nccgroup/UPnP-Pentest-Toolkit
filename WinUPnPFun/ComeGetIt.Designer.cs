namespace WinUPnPFun
{
    partial class ComeGetIt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComeGetIt));
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.sourceIP = new System.Windows.Forms.TextBox();
            this.destIP = new System.Windows.Forms.TextBox();
            this.sourcePort = new System.Windows.Forms.TextBox();
            this.destPort = new System.Windows.Forms.TextBox();
            this.webserverLog = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.webServerResponse = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.scpdURL = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.webserverPort = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.disableWebServer = new System.Windows.Forms.CheckBox();
            this.deviceDescURL = new System.Windows.Forms.TextBox();
            this.currentFuzzCase = new System.Windows.Forms.TextBox();
            this.useFuzzCases = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hitCounter = new System.Windows.Forms.Label();
            this.networkInterfaces = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.intervalText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.askedFor = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.msearchResponse = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.msearchLog = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.mimicDeviceLog = new System.Windows.Forms.TextBox();
            this.savedDeviceTree = new System.Windows.Forms.TreeView();
            this.savedDeviceName = new System.Windows.Forms.TextBox();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.Location = new System.Drawing.Point(11, 50);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(414, 152);
            this.textBox2.TabIndex = 3;
            // 
            // sourceIP
            // 
            this.sourceIP.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sourceIP.Location = new System.Drawing.Point(14, 295);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(223, 20);
            this.sourceIP.TabIndex = 4;
            this.sourceIP.Text = "192.168.0.15";
            this.sourceIP.TextChanged += new System.EventHandler(this.sourceIP_TextChanged);
            // 
            // destIP
            // 
            this.destIP.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.destIP.Location = new System.Drawing.Point(13, 341);
            this.destIP.Name = "destIP";
            this.destIP.Size = new System.Drawing.Size(223, 20);
            this.destIP.TabIndex = 5;
            this.destIP.Text = "239.255.255.250";
            // 
            // sourcePort
            // 
            this.sourcePort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sourcePort.Location = new System.Drawing.Point(327, 295);
            this.sourcePort.Name = "sourcePort";
            this.sourcePort.Size = new System.Drawing.Size(98, 20);
            this.sourcePort.TabIndex = 6;
            this.sourcePort.Text = "1900";
            // 
            // destPort
            // 
            this.destPort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.destPort.Location = new System.Drawing.Point(326, 341);
            this.destPort.Name = "destPort";
            this.destPort.Size = new System.Drawing.Size(98, 20);
            this.destPort.TabIndex = 7;
            this.destPort.Text = "1900";
            // 
            // webserverLog
            // 
            this.webserverLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webserverLog.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webserverLog.Location = new System.Drawing.Point(17, 147);
            this.webserverLog.Multiline = true;
            this.webserverLog.Name = "webserverLog";
            this.webserverLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.webserverLog.Size = new System.Drawing.Size(492, 131);
            this.webserverLog.TabIndex = 9;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.trackBar1.Location = new System.Drawing.Point(102, 233);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(317, 45);
            this.trackBar1.TabIndex = 11;
            this.trackBar1.Value = 1000;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // webServerResponse
            // 
            this.webServerResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webServerResponse.Location = new System.Drawing.Point(17, 297);
            this.webServerResponse.Multiline = true;
            this.webServerResponse.Name = "webServerResponse";
            this.webServerResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.webServerResponse.Size = new System.Drawing.Size(492, 159);
            this.webServerResponse.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.scpdURL);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.webserverPort);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.disableWebServer);
            this.groupBox1.Controls.Add(this.deviceDescURL);
            this.groupBox1.Controls.Add(this.currentFuzzCase);
            this.groupBox1.Controls.Add(this.useFuzzCases);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.webServerResponse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.hitCounter);
            this.groupBox1.Controls.Add(this.webserverLog);
            this.groupBox1.Location = new System.Drawing.Point(473, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(522, 526);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Web Server";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 25;
            this.label18.Text = "SCPD URL:";
            // 
            // scpdURL
            // 
            this.scpdURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scpdURL.Location = new System.Drawing.Point(17, 108);
            this.scpdURL.Name = "scpdURL";
            this.scpdURL.Size = new System.Drawing.Size(491, 20);
            this.scpdURL.TabIndex = 24;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(223, 474);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 13);
            this.label17.TabIndex = 23;
            this.label17.Text = "Current fuzz case:";
            // 
            // webserverPort
            // 
            this.webserverPort.Location = new System.Drawing.Point(459, 40);
            this.webserverPort.Name = "webserverPort";
            this.webserverPort.Size = new System.Drawing.Size(50, 20);
            this.webserverPort.TabIndex = 22;
            this.webserverPort.Text = "9090";
            this.webserverPort.TextChanged += new System.EventHandler(this.webserverPort_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(368, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "Web Server Port:";
            // 
            // disableWebServer
            // 
            this.disableWebServer.AutoSize = true;
            this.disableWebServer.Location = new System.Drawing.Point(229, 44);
            this.disableWebServer.Name = "disableWebServer";
            this.disableWebServer.Size = new System.Drawing.Size(108, 17);
            this.disableWebServer.TabIndex = 20;
            this.disableWebServer.Text = "Use Custom URL";
            this.disableWebServer.UseVisualStyleBackColor = true;
            this.disableWebServer.CheckedChanged += new System.EventHandler(this.disableWebServer_CheckedChanged);
            // 
            // deviceDescURL
            // 
            this.deviceDescURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceDescURL.Location = new System.Drawing.Point(17, 64);
            this.deviceDescURL.Name = "deviceDescURL";
            this.deviceDescURL.Size = new System.Drawing.Size(491, 20);
            this.deviceDescURL.TabIndex = 19;
            // 
            // currentFuzzCase
            // 
            this.currentFuzzCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFuzzCase.Location = new System.Drawing.Point(226, 490);
            this.currentFuzzCase.Name = "currentFuzzCase";
            this.currentFuzzCase.Size = new System.Drawing.Size(282, 20);
            this.currentFuzzCase.TabIndex = 18;
            // 
            // useFuzzCases
            // 
            this.useFuzzCases.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.useFuzzCases.AutoSize = true;
            this.useFuzzCases.Enabled = false;
            this.useFuzzCases.Location = new System.Drawing.Point(101, 492);
            this.useFuzzCases.Name = "useFuzzCases";
            this.useFuzzCases.Size = new System.Drawing.Size(107, 17);
            this.useFuzzCases.TabIndex = 17;
            this.useFuzzCases.Text = "Load Fuzz Cases";
            this.useFuzzCases.UseVisualStyleBackColor = true;
            this.useFuzzCases.CheckedChanged += new System.EventHandler(this.useFuzzCases_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button4.Location = new System.Drawing.Point(20, 488);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Open";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 462);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 13);
            this.label15.TabIndex = 14;
            this.label15.Text = "Fuzz cases:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Current Device Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Log:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Device Description URL:";
            // 
            // hitCounter
            // 
            this.hitCounter.AutoSize = true;
            this.hitCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hitCounter.Location = new System.Drawing.Point(16, 20);
            this.hitCounter.Name = "hitCounter";
            this.hitCounter.Size = new System.Drawing.Size(157, 20);
            this.hitCounter.TabIndex = 10;
            this.hitCounter.Text = "Web Server Stopped";
            // 
            // networkInterfaces
            // 
            this.networkInterfaces.FormattingEnabled = true;
            this.networkInterfaces.Location = new System.Drawing.Point(16, 36);
            this.networkInterfaces.Name = "networkInterfaces";
            this.networkInterfaces.Size = new System.Drawing.Size(403, 21);
            this.networkInterfaces.TabIndex = 14;
            this.networkInterfaces.SelectedIndexChanged += new System.EventHandler(this.networkInterfaces_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Network Interface:";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(124, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 43);
            this.button2.TabIndex = 16;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.intervalText);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.trackBar1);
            this.groupBox2.Controls.Add(this.sourceIP);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.destPort);
            this.groupBox2.Controls.Add(this.destIP);
            this.groupBox2.Controls.Add(this.sourcePort);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(439, 380);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Advertise UPnP Device:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "NOTIFY Beacon:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(327, 279);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Source Port:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 322);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Dest IP:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 279);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Source IP";
            // 
            // intervalText
            // 
            this.intervalText.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.intervalText.Location = new System.Drawing.Point(14, 238);
            this.intervalText.Name = "intervalText";
            this.intervalText.Size = new System.Drawing.Size(82, 20);
            this.intervalText.TabIndex = 18;
            this.intervalText.Text = "1000";
            this.intervalText.TextChanged += new System.EventHandler(this.intervalText_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Beacon Interval (ms):";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(10, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 10);
            this.label13.TabIndex = 29;
            this.label13.Text = "Network Capture Stopped";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.askedFor);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.msearchResponse);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.msearchLog);
            this.groupBox3.Location = new System.Drawing.Point(12, 549);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(983, 173);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "M-Search Responder";
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(679, 17);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(290, 17);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "Send Multiple Responses (For each known device type)";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(331, 38);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Request:";
            // 
            // askedFor
            // 
            this.askedFor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.askedFor.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.askedFor.Location = new System.Drawing.Point(11, 53);
            this.askedFor.Multiline = true;
            this.askedFor.Name = "askedFor";
            this.askedFor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.askedFor.Size = new System.Drawing.Size(310, 111);
            this.askedFor.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Log:";
            // 
            // msearchResponse
            // 
            this.msearchResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.msearchResponse.Location = new System.Drawing.Point(659, 53);
            this.msearchResponse.Multiline = true;
            this.msearchResponse.Name = "msearchResponse";
            this.msearchResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.msearchResponse.Size = new System.Drawing.Size(310, 111);
            this.msearchResponse.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(656, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Response:";
            // 
            // msearchLog
            // 
            this.msearchLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.msearchLog.Location = new System.Drawing.Point(330, 53);
            this.msearchLog.Multiline = true;
            this.msearchLog.Name = "msearchLog";
            this.msearchLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.msearchLog.Size = new System.Drawing.Size(310, 111);
            this.msearchLog.TabIndex = 24;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.networkInterfaces);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(438, 128);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(310, 102);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(109, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Cycle UDN GUID";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(17, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 59);
            this.button3.TabIndex = 20;
            this.button3.Text = "Load Saved Device";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(14, 102);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 18);
            this.label19.TabIndex = 21;
            this.label19.Text = "Device:";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.mimicDeviceLog);
            this.groupBox5.Controls.Add(this.savedDeviceTree);
            this.groupBox5.Controls.Add(this.savedDeviceName);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Location = new System.Drawing.Point(1011, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(266, 710);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Saved Device";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(14, 368);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 13);
            this.label20.TabIndex = 26;
            this.label20.Text = "SOAPAction Log:";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(132, 25);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(113, 59);
            this.button5.TabIndex = 25;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // mimicDeviceLog
            // 
            this.mimicDeviceLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mimicDeviceLog.Location = new System.Drawing.Point(17, 384);
            this.mimicDeviceLog.Multiline = true;
            this.mimicDeviceLog.Name = "mimicDeviceLog";
            this.mimicDeviceLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mimicDeviceLog.Size = new System.Drawing.Size(228, 317);
            this.mimicDeviceLog.TabIndex = 24;
            // 
            // savedDeviceTree
            // 
            this.savedDeviceTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savedDeviceTree.Location = new System.Drawing.Point(17, 147);
            this.savedDeviceTree.Name = "savedDeviceTree";
            this.savedDeviceTree.ShowNodeToolTips = true;
            this.savedDeviceTree.Size = new System.Drawing.Size(228, 213);
            this.savedDeviceTree.TabIndex = 23;
            this.savedDeviceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.savedDeviceTree_AfterSelect);
            // 
            // savedDeviceName
            // 
            this.savedDeviceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savedDeviceName.Location = new System.Drawing.Point(17, 124);
            this.savedDeviceName.Name = "savedDeviceName";
            this.savedDeviceName.Size = new System.Drawing.Size(228, 20);
            this.savedDeviceName.TabIndex = 22;
            this.savedDeviceName.Text = "No Device Loaded";
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // timer4
            // 
            this.timer4.Enabled = true;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // ComeGetIt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 729);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ComeGetIt";
            this.Text = "ComeGetIt - Spoof UPnP Device";
            this.Load += new System.EventHandler(this.ComeGetIt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox sourceIP;
        private System.Windows.Forms.TextBox destIP;
        private System.Windows.Forms.TextBox sourcePort;
        private System.Windows.Forms.TextBox destPort;
        private System.Windows.Forms.TextBox webserverLog;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox webServerResponse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label hitCounter;
        private System.Windows.Forms.ComboBox networkInterfaces;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox intervalText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox msearchLog;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox msearchResponse;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox askedFor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox deviceDescURL;
        private System.Windows.Forms.CheckBox disableWebServer;
        private System.Windows.Forms.TextBox webserverPort;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox scpdURL;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox savedDeviceName;
        private System.Windows.Forms.TreeView savedDeviceTree;
        private System.Windows.Forms.TextBox mimicDeviceLog;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox currentFuzzCase;
        private System.Windows.Forms.CheckBox useFuzzCases;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Timer timer4;

    }
}