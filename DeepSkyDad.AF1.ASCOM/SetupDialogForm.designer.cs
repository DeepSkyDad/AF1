namespace ASCOM.DeepSkyDad.AF1
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.comboBoxStepSize = new System.Windows.Forms.ComboBox();
            this.chkResetOnConnect = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAlwaysOn = new System.Windows.Forms.CheckBox();
            this.numericUpDownSettleBuffer = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonFirmwareInfo = new System.Windows.Forms.Button();
            this.chkReverseDirection = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpMaxPosition = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpMaxMovement = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.chkSetPositionOnConnect = new System.Windows.Forms.CheckBox();
            this.numericSetPositionOnConnectValue = new System.Windows.Forms.NumericUpDown();
            this.alwaysOnLabel = new System.Windows.Forms.Label();
            this.buttonReboot = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.currentMoveComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.currentAoComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettleBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSetPositionOnConnectValue)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(474, 324);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(541, 323);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.DeepSkyDad.AF1.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(552, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "COM Port";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(18, 291);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            this.chkTrace.CheckedChanged += new System.EventHandler(this.chkTrace_CheckedChanged);
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(161, 48);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(121, 21);
            this.comboBoxComPort.TabIndex = 7;
            // 
            // comboBoxStepSize
            // 
            this.comboBoxStepSize.FormattingEnabled = true;
            this.comboBoxStepSize.Items.AddRange(new object[] {
            "1",
            "1/2",
            "1/4",
            "1/8"});
            this.comboBoxStepSize.Location = new System.Drawing.Point(161, 75);
            this.comboBoxStepSize.Name = "comboBoxStepSize";
            this.comboBoxStepSize.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStepSize.TabIndex = 8;
            this.comboBoxStepSize.SelectedIndexChanged += new System.EventHandler(this.comboBoxStepSize_SelectedIndexChanged);
            // 
            // chkResetOnConnect
            // 
            this.chkResetOnConnect.AutoSize = true;
            this.chkResetOnConnect.Location = new System.Drawing.Point(312, 50);
            this.chkResetOnConnect.Name = "chkResetOnConnect";
            this.chkResetOnConnect.Size = new System.Drawing.Size(93, 17);
            this.chkResetOnConnect.TabIndex = 9;
            this.chkResetOnConnect.Text = "Reset settings";
            this.chkResetOnConnect.UseVisualStyleBackColor = true;
            this.chkResetOnConnect.CheckedChanged += new System.EventHandler(this.chkResetOnConnet_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Step size";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "General settings";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkAlwaysOn
            // 
            this.chkAlwaysOn.AutoSize = true;
            this.chkAlwaysOn.Location = new System.Drawing.Point(18, 245);
            this.chkAlwaysOn.Name = "chkAlwaysOn";
            this.chkAlwaysOn.Size = new System.Drawing.Size(15, 14);
            this.chkAlwaysOn.TabIndex = 15;
            this.chkAlwaysOn.UseVisualStyleBackColor = true;
            this.chkAlwaysOn.CheckedChanged += new System.EventHandler(this.chkAlwaysOn_CheckedChanged);
            // 
            // numericUpDownSettleBuffer
            // 
            this.numericUpDownSettleBuffer.Location = new System.Drawing.Point(162, 102);
            this.numericUpDownSettleBuffer.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownSettleBuffer.Name = "numericUpDownSettleBuffer";
            this.numericUpDownSettleBuffer.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSettleBuffer.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Settle buffer (0ms - 5000ms)";
            // 
            // buttonFirmwareInfo
            // 
            this.buttonFirmwareInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFirmwareInfo.Location = new System.Drawing.Point(25, 324);
            this.buttonFirmwareInfo.Name = "buttonFirmwareInfo";
            this.buttonFirmwareInfo.Size = new System.Drawing.Size(97, 24);
            this.buttonFirmwareInfo.TabIndex = 18;
            this.buttonFirmwareInfo.Text = "Firmware version";
            this.buttonFirmwareInfo.UseVisualStyleBackColor = true;
            this.buttonFirmwareInfo.Click += new System.EventHandler(this.buttonFirmwareInfo_Click);
            // 
            // chkReverseDirection
            // 
            this.chkReverseDirection.AutoSize = true;
            this.chkReverseDirection.Location = new System.Drawing.Point(18, 268);
            this.chkReverseDirection.Name = "chkReverseDirection";
            this.chkReverseDirection.Size = new System.Drawing.Size(109, 17);
            this.chkReverseDirection.TabIndex = 20;
            this.chkReverseDirection.Text = "Reverse direction";
            this.chkReverseDirection.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Maximum position";
            // 
            // numericUpMaxPosition
            // 
            this.numericUpMaxPosition.Location = new System.Drawing.Point(162, 128);
            this.numericUpMaxPosition.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpMaxPosition.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpMaxPosition.Name = "numericUpMaxPosition";
            this.numericUpMaxPosition.Size = new System.Drawing.Size(120, 20);
            this.numericUpMaxPosition.TabIndex = 21;
            this.numericUpMaxPosition.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Maximum movement";
            // 
            // numericUpMaxMovement
            // 
            this.numericUpMaxMovement.Location = new System.Drawing.Point(162, 154);
            this.numericUpMaxMovement.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numericUpMaxMovement.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpMaxMovement.Name = "numericUpMaxMovement";
            this.numericUpMaxMovement.Size = new System.Drawing.Size(120, 20);
            this.numericUpMaxMovement.TabIndex = 23;
            this.numericUpMaxMovement.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(308, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 24);
            this.label11.TabIndex = 31;
            this.label11.Text = "On next connect";
            // 
            // chkSetPositionOnConnect
            // 
            this.chkSetPositionOnConnect.AutoSize = true;
            this.chkSetPositionOnConnect.Location = new System.Drawing.Point(312, 78);
            this.chkSetPositionOnConnect.Name = "chkSetPositionOnConnect";
            this.chkSetPositionOnConnect.Size = new System.Drawing.Size(81, 17);
            this.chkSetPositionOnConnect.TabIndex = 32;
            this.chkSetPositionOnConnect.Text = "Set position";
            this.chkSetPositionOnConnect.UseVisualStyleBackColor = true;
            this.chkSetPositionOnConnect.CheckedChanged += new System.EventHandler(this.chkSetPositionOnConnect_CheckedChanged);
            // 
            // numericSetPositionOnConnectValue
            // 
            this.numericSetPositionOnConnectValue.Location = new System.Drawing.Point(414, 76);
            this.numericSetPositionOnConnectValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericSetPositionOnConnectValue.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericSetPositionOnConnectValue.Name = "numericSetPositionOnConnectValue";
            this.numericSetPositionOnConnectValue.Size = new System.Drawing.Size(120, 20);
            this.numericSetPositionOnConnectValue.TabIndex = 33;
            this.numericSetPositionOnConnectValue.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericSetPositionOnConnectValue.Visible = false;
            // 
            // alwaysOnLabel
            // 
            this.alwaysOnLabel.AutoSize = true;
            this.alwaysOnLabel.Location = new System.Drawing.Point(35, 245);
            this.alwaysOnLabel.Name = "alwaysOnLabel";
            this.alwaysOnLabel.Size = new System.Drawing.Size(163, 13);
            this.alwaysOnLabel.TabIndex = 34;
            this.alwaysOnLabel.Text = "Always on (useful for focuser slip)";
            // 
            // buttonReboot
            // 
            this.buttonReboot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReboot.Location = new System.Drawing.Point(127, 324);
            this.buttonReboot.Name = "buttonReboot";
            this.buttonReboot.Size = new System.Drawing.Size(97, 24);
            this.buttonReboot.TabIndex = 35;
            this.buttonReboot.Text = "Reboot AF1";
            this.buttonReboot.UseVisualStyleBackColor = true;
            this.buttonReboot.Click += new System.EventHandler(this.buttonReboot_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Current - move";
            // 
            // currentMoveComboBox
            // 
            this.currentMoveComboBox.FormattingEnabled = true;
            this.currentMoveComboBox.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%"});
            this.currentMoveComboBox.Location = new System.Drawing.Point(161, 180);
            this.currentMoveComboBox.Name = "currentMoveComboBox";
            this.currentMoveComboBox.Size = new System.Drawing.Size(121, 21);
            this.currentMoveComboBox.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 211);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Current - always on";
            // 
            // currentAoComboBox
            // 
            this.currentAoComboBox.FormattingEnabled = true;
            this.currentAoComboBox.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%"});
            this.currentAoComboBox.Location = new System.Drawing.Point(161, 207);
            this.currentAoComboBox.Name = "currentAoComboBox";
            this.currentAoComboBox.Size = new System.Drawing.Size(121, 21);
            this.currentAoComboBox.TabIndex = 38;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 360);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.currentAoComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.currentMoveComboBox);
            this.Controls.Add(this.buttonReboot);
            this.Controls.Add(this.alwaysOnLabel);
            this.Controls.Add(this.numericSetPositionOnConnectValue);
            this.Controls.Add(this.chkSetPositionOnConnect);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpMaxMovement);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpMaxPosition);
            this.Controls.Add(this.chkReverseDirection);
            this.Controls.Add(this.buttonFirmwareInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownSettleBuffer);
            this.Controls.Add(this.chkAlwaysOn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkResetOnConnect);
            this.Controls.Add(this.comboBoxStepSize);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeepSkyDad AF1 Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettleBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxMovement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSetPositionOnConnectValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.ComboBox comboBoxStepSize;
        private System.Windows.Forms.CheckBox chkResetOnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAlwaysOn;
        private System.Windows.Forms.NumericUpDown numericUpDownSettleBuffer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonFirmwareInfo;
        private System.Windows.Forms.CheckBox chkReverseDirection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpMaxPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpMaxMovement;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkSetPositionOnConnect;
        private System.Windows.Forms.NumericUpDown numericSetPositionOnConnectValue;
        private System.Windows.Forms.Label alwaysOnLabel;
        private System.Windows.Forms.Button buttonReboot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox currentMoveComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox currentAoComboBox;
    }
}