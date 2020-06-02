namespace FrameTruyenThong
{
    partial class FrmCmd0x09
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtgCmd0x09 = new System.Windows.Forms.DataGridView();
            this.colSessionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIPECR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIPServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSSID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSSIDLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKeyLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModeECR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIPServerECR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortServerECR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgCmd0x09)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 616);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1274, 36);
            this.panel1.TabIndex = 0;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 7);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(1106, 7);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Gửi";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1187, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtgCmd0x09);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1274, 616);
            this.panel2.TabIndex = 1;
            // 
            // dtgCmd0x09
            // 
            this.dtgCmd0x09.AllowUserToAddRows = false;
            this.dtgCmd0x09.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgCmd0x09.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgCmd0x09.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgCmd0x09.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSessionID,
            this.colIPAddress,
            this.colIPECR,
            this.colIPServer,
            this.colPortServer,
            this.colSSID,
            this.colSSIDLength,
            this.colKey,
            this.colKeyLength,
            this.colModeECR,
            this.colIPServerECR,
            this.colPortServerECR});
            this.dtgCmd0x09.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgCmd0x09.Location = new System.Drawing.Point(0, 0);
            this.dtgCmd0x09.Name = "dtgCmd0x09";
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgCmd0x09.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dtgCmd0x09.RowHeadersVisible = false;
            this.dtgCmd0x09.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtgCmd0x09.Size = new System.Drawing.Size(1274, 616);
            this.dtgCmd0x09.TabIndex = 0;
            this.dtgCmd0x09.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgCmd0x09_CellEndEdit);
            this.dtgCmd0x09.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtgCmd0x09_CellFormatting);
            this.dtgCmd0x09.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dtgCmd0x09_EditingControlShowing);
            // 
            // colSessionID
            // 
            this.colSessionID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSessionID.DataPropertyName = "SessionID";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSessionID.DefaultCellStyle = dataGridViewCellStyle2;
            this.colSessionID.HeaderText = "ID";
            this.colSessionID.Name = "colSessionID";
            this.colSessionID.ReadOnly = true;
            // 
            // colIPAddress
            // 
            this.colIPAddress.DataPropertyName = "IPAddress";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colIPAddress.DefaultCellStyle = dataGridViewCellStyle3;
            this.colIPAddress.HeaderText = "Địa chỉ IP";
            this.colIPAddress.Name = "colIPAddress";
            this.colIPAddress.ReadOnly = true;
            // 
            // colIPECR
            // 
            this.colIPECR.DataPropertyName = "IPECR";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colIPECR.DefaultCellStyle = dataGridViewCellStyle4;
            this.colIPECR.HeaderText = "IP của ECR";
            this.colIPECR.MaxInputLength = 15;
            this.colIPECR.Name = "colIPECR";
            // 
            // colIPServer
            // 
            this.colIPServer.DataPropertyName = "IPServer";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colIPServer.DefaultCellStyle = dataGridViewCellStyle5;
            this.colIPServer.HeaderText = "IP của Server";
            this.colIPServer.MaxInputLength = 15;
            this.colIPServer.Name = "colIPServer";
            // 
            // colPortServer
            // 
            this.colPortServer.DataPropertyName = "PortServer";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPortServer.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPortServer.HeaderText = "Port của Server";
            this.colPortServer.MaxInputLength = 10;
            this.colPortServer.Name = "colPortServer";
            // 
            // colSSID
            // 
            this.colSSID.DataPropertyName = "SSID";
            this.colSSID.HeaderText = "SSID";
            this.colSSID.Name = "colSSID";
            this.colSSID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSSID.Width = 150;
            // 
            // colSSIDLength
            // 
            this.colSSIDLength.DataPropertyName = "SSIDLength";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSSIDLength.DefaultCellStyle = dataGridViewCellStyle7;
            this.colSSIDLength.HeaderText = "Độ dài SSID";
            this.colSSIDLength.Name = "colSSIDLength";
            this.colSSIDLength.ReadOnly = true;
            this.colSSIDLength.Width = 50;
            // 
            // colKey
            // 
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colKey.Width = 150;
            // 
            // colKeyLength
            // 
            this.colKeyLength.DataPropertyName = "KeyLength";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colKeyLength.DefaultCellStyle = dataGridViewCellStyle8;
            this.colKeyLength.HeaderText = "Độ dài Key";
            this.colKeyLength.Name = "colKeyLength";
            this.colKeyLength.ReadOnly = true;
            this.colKeyLength.Width = 50;
            // 
            // colModeECR
            // 
            this.colModeECR.DataPropertyName = "ModeECR";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colModeECR.DefaultCellStyle = dataGridViewCellStyle9;
            this.colModeECR.HeaderText = "Chế độ ECR";
            this.colModeECR.MaxInputLength = 1;
            this.colModeECR.Name = "colModeECR";
            this.colModeECR.Width = 50;
            // 
            // colIPServerECR
            // 
            this.colIPServerECR.DataPropertyName = "IPServerECR";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colIPServerECR.DefaultCellStyle = dataGridViewCellStyle10;
            this.colIPServerECR.HeaderText = "IP Server của ECR";
            this.colIPServerECR.MaxInputLength = 15;
            this.colIPServerECR.Name = "colIPServerECR";
            // 
            // colPortServerECR
            // 
            this.colPortServerECR.DataPropertyName = "PortServerECR";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPortServerECR.DefaultCellStyle = dataGridViewCellStyle11;
            this.colPortServerECR.HeaderText = "Port Server của ECR";
            this.colPortServerECR.MaxInputLength = 10;
            this.colPortServerECR.Name = "colPortServerECR";
            // 
            // FrmCmd0x09
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 652);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCmd0x09";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmCmd0x09";
            this.Load += new System.EventHandler(this.FrmCmd0x09_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgCmd0x09)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dtgCmd0x09;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIPECR;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIPServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSSIDLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKeyLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModeECR;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIPServerECR;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortServerECR;
    }
}