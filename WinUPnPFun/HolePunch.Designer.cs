namespace WinUPnPFun
{
    partial class HolePunch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HolePunch));
            this.portMapDeviceLog = new System.Windows.Forms.TextBox();
            this.portMappingTable = new System.Windows.Forms.DataGridView();
            this.RemoteHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExternalPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Protocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InternalPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InternalClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaseDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portMappingDevices = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.leaseDurationEntry = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.portMappingDescEntry = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.enabledEntry = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.protocolEntry = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.internalClientEntry = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.internalPortEntry = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.externalPortEntry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.remoteHostEntry = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.portMappingTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // portMapDeviceLog
            // 
            this.portMapDeviceLog.Location = new System.Drawing.Point(10, 10);
            this.portMapDeviceLog.Multiline = true;
            this.portMapDeviceLog.Name = "portMapDeviceLog";
            this.portMapDeviceLog.Size = new System.Drawing.Size(324, 100);
            this.portMapDeviceLog.TabIndex = 16;
            this.portMapDeviceLog.TextChanged += new System.EventHandler(this.portMapDeviceLog_TextChanged);
            // 
            // portMappingTable
            // 
            this.portMappingTable.AllowUserToAddRows = false;
            this.portMappingTable.AllowUserToDeleteRows = false;
            this.portMappingTable.AllowUserToOrderColumns = true;
            this.portMappingTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portMappingTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.portMappingTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RemoteHost,
            this.ExternalPort,
            this.Protocol,
            this.InternalPort,
            this.InternalClient,
            this.Enabled,
            this.Description,
            this.LeaseDuration});
            this.portMappingTable.Location = new System.Drawing.Point(337, 12);
            this.portMappingTable.Name = "portMappingTable";
            this.portMappingTable.ReadOnly = true;
            this.portMappingTable.Size = new System.Drawing.Size(843, 511);
            this.portMappingTable.TabIndex = 17;
            this.portMappingTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.portMappingTable_CellContentClick);
            // 
            // RemoteHost
            // 
            this.RemoteHost.HeaderText = "Remote Host";
            this.RemoteHost.Name = "RemoteHost";
            this.RemoteHost.ReadOnly = true;
            // 
            // ExternalPort
            // 
            this.ExternalPort.HeaderText = "External Port";
            this.ExternalPort.Name = "ExternalPort";
            this.ExternalPort.ReadOnly = true;
            // 
            // Protocol
            // 
            this.Protocol.HeaderText = "Protocol";
            this.Protocol.Name = "Protocol";
            this.Protocol.ReadOnly = true;
            // 
            // InternalPort
            // 
            this.InternalPort.HeaderText = "Internal Port";
            this.InternalPort.Name = "InternalPort";
            this.InternalPort.ReadOnly = true;
            // 
            // InternalClient
            // 
            this.InternalClient.HeaderText = "Internal Client";
            this.InternalClient.Name = "InternalClient";
            this.InternalClient.ReadOnly = true;
            // 
            // Enabled
            // 
            this.Enabled.HeaderText = "Enabled";
            this.Enabled.Name = "Enabled";
            this.Enabled.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "PortMappingDescription";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // LeaseDuration
            // 
            this.LeaseDuration.HeaderText = "Lease Duration";
            this.LeaseDuration.Name = "LeaseDuration";
            this.LeaseDuration.ReadOnly = true;
            // 
            // portMappingDevices
            // 
            this.portMappingDevices.FormattingEnabled = true;
            this.portMappingDevices.Location = new System.Drawing.Point(10, 40);
            this.portMappingDevices.Name = "portMappingDevices";
            this.portMappingDevices.Size = new System.Drawing.Size(299, 21);
            this.portMappingDevices.TabIndex = 1;
            this.portMappingDevices.SelectedIndexChanged += new System.EventHandler(this.portMappingDevices_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.deleteButton);
            this.groupBox1.Controls.Add(this.addButton);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.leaseDurationEntry);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.portMappingDescEntry);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.enabledEntry);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.protocolEntry);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.internalClientEntry);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.internalPortEntry);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.externalPortEntry);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.remoteHostEntry);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.portMappingDevices);
            this.groupBox1.Location = new System.Drawing.Point(12, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 407);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Port Mapping";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(208, 388);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "* Required to delete";
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(112, 362);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(96, 39);
            this.deleteButton.TabIndex = 39;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(10, 362);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(96, 39);
            this.addButton.TabIndex = 38;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 316);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Lease Duration:";
            // 
            // leaseDurationEntry
            // 
            this.leaseDurationEntry.Location = new System.Drawing.Point(10, 332);
            this.leaseDurationEntry.Name = "leaseDurationEntry";
            this.leaseDurationEntry.Size = new System.Drawing.Size(100, 20);
            this.leaseDurationEntry.TabIndex = 9;
            this.leaseDurationEntry.Text = "3600";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Port Mapping Description:";
            // 
            // portMappingDescEntry
            // 
            this.portMappingDescEntry.Location = new System.Drawing.Point(10, 290);
            this.portMappingDescEntry.Name = "portMappingDescEntry";
            this.portMappingDescEntry.Size = new System.Drawing.Size(287, 20);
            this.portMappingDescEntry.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 233);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Enabled:";
            // 
            // enabledEntry
            // 
            this.enabledEntry.Location = new System.Drawing.Point(10, 249);
            this.enabledEntry.Name = "enabledEntry";
            this.enabledEntry.Size = new System.Drawing.Size(100, 20);
            this.enabledEntry.TabIndex = 7;
            this.enabledEntry.Text = "True";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Protocol: *";
            // 
            // protocolEntry
            // 
            this.protocolEntry.Location = new System.Drawing.Point(10, 125);
            this.protocolEntry.Name = "protocolEntry";
            this.protocolEntry.Size = new System.Drawing.Size(166, 20);
            this.protocolEntry.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Internal Client:";
            // 
            // internalClientEntry
            // 
            this.internalClientEntry.Location = new System.Drawing.Point(10, 210);
            this.internalClientEntry.Name = "internalClientEntry";
            this.internalClientEntry.Size = new System.Drawing.Size(166, 20);
            this.internalClientEntry.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Internal Port:";
            // 
            // internalPortEntry
            // 
            this.internalPortEntry.Location = new System.Drawing.Point(185, 167);
            this.internalPortEntry.Name = "internalPortEntry";
            this.internalPortEntry.Size = new System.Drawing.Size(116, 20);
            this.internalPortEntry.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "External Port: *";
            // 
            // externalPortEntry
            // 
            this.externalPortEntry.Location = new System.Drawing.Point(10, 167);
            this.externalPortEntry.Name = "externalPortEntry";
            this.externalPortEntry.Size = new System.Drawing.Size(116, 20);
            this.externalPortEntry.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Remote Host (Usually Blank):  *";
            // 
            // remoteHostEntry
            // 
            this.remoteHostEntry.Location = new System.Drawing.Point(10, 86);
            this.remoteHostEntry.Name = "remoteHostEntry";
            this.remoteHostEntry.Size = new System.Drawing.Size(166, 20);
            this.remoteHostEntry.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Port Mapping Devices:";
            // 
            // loadingImage
            // 
            this.loadingImage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loadingImage.BackColor = System.Drawing.Color.White;
            this.loadingImage.Image = global::WinUPnPFun.Properties.Resources.circle_loading;
            this.loadingImage.Location = new System.Drawing.Point(124, 12);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(62, 82);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingImage.TabIndex = 15;
            this.loadingImage.TabStop = false;
            this.loadingImage.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(326, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(8, 8);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // HolePunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.loadingImage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.portMappingTable);
            this.Controls.Add(this.portMapDeviceLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HolePunch";
            this.Text = "Hole Punch - Port Mapping Tool";
            this.Load += new System.EventHandler(this.HolePunch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.portMappingTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox loadingImage;
        private System.Windows.Forms.TextBox portMapDeviceLog;
        private System.Windows.Forms.DataGridView portMappingTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemoteHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExternalPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn Protocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn InternalPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn InternalClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaseDuration;
        private System.Windows.Forms.ComboBox portMappingDevices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox remoteHostEntry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox leaseDurationEntry;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox portMappingDescEntry;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox enabledEntry;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox protocolEntry;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox internalClientEntry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox internalPortEntry;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox externalPortEntry;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}