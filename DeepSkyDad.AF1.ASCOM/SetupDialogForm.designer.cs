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
            this.buttonReboot = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.currentMoveComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.currentHoldComboBox = new System.Windows.Forms.ComboBox();
            this.advancedPanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.coilsModeCombo = new System.Windows.Forms.ComboBox();
            this.idleCoilsTimeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.showAdvancedBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettleBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxMovement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSetPositionOnConnectValue)).BeginInit();
            this.advancedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idleCoilsTimeoutNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(12, 245);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(77, 244);
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
            this.picASCOM.Location = new System.Drawing.Point(887, 9);
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
            this.chkTrace.Location = new System.Drawing.Point(18, 99);
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
            this.comboBoxStepSize.Location = new System.Drawing.Point(175, 42);
            this.comboBoxStepSize.Name = "comboBoxStepSize";
            this.comboBoxStepSize.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStepSize.TabIndex = 8;
            // 
            // chkResetOnConnect
            // 
            this.chkResetOnConnect.AutoSize = true;
            this.chkResetOnConnect.Location = new System.Drawing.Point(32, 217);
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
            this.label3.Location = new System.Drawing.Point(29, 46);
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
            // numericUpDownSettleBuffer
            // 
            this.numericUpDownSettleBuffer.Location = new System.Drawing.Point(176, 69);
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
            this.label4.Location = new System.Drawing.Point(28, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Settle buffer (0ms - 5000ms)";
            // 
            // buttonFirmwareInfo
            // 
            this.buttonFirmwareInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFirmwareInfo.Location = new System.Drawing.Point(334, 243);
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
            this.chkReverseDirection.Location = new System.Drawing.Point(18, 76);
            this.chkReverseDirection.Name = "chkReverseDirection";
            this.chkReverseDirection.Size = new System.Drawing.Size(109, 17);
            this.chkReverseDirection.TabIndex = 20;
            this.chkReverseDirection.Text = "Reverse direction";
            this.chkReverseDirection.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Maximum position";
            // 
            // numericUpMaxPosition
            // 
            this.numericUpMaxPosition.Location = new System.Drawing.Point(176, 95);
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
            this.label6.Location = new System.Drawing.Point(28, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Maximum movement";
            // 
            // numericUpMaxMovement
            // 
            this.numericUpMaxMovement.Location = new System.Drawing.Point(176, 121);
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
            this.label11.Location = new System.Drawing.Point(22, 176);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 24);
            this.label11.TabIndex = 31;
            this.label11.Text = "On next connect";
            // 
            // chkSetPositionOnConnect
            // 
            this.chkSetPositionOnConnect.AutoSize = true;
            this.chkSetPositionOnConnect.Location = new System.Drawing.Point(32, 247);
            this.chkSetPositionOnConnect.Name = "chkSetPositionOnConnect";
            this.chkSetPositionOnConnect.Size = new System.Drawing.Size(81, 17);
            this.chkSetPositionOnConnect.TabIndex = 32;
            this.chkSetPositionOnConnect.Text = "Set position";
            this.chkSetPositionOnConnect.UseVisualStyleBackColor = true;
            this.chkSetPositionOnConnect.CheckedChanged += new System.EventHandler(this.chkSetPositionOnConnect_CheckedChanged);
            // 
            // numericSetPositionOnConnectValue
            // 
            this.numericSetPositionOnConnectValue.Location = new System.Drawing.Point(175, 246);
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
            this.numericSetPositionOnConnectValue.Size = new System.Drawing.Size(121, 20);
            this.numericSetPositionOnConnectValue.TabIndex = 33;
            this.numericSetPositionOnConnectValue.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericSetPositionOnConnectValue.Visible = false;
            // 
            // buttonReboot
            // 
            this.buttonReboot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReboot.Location = new System.Drawing.Point(334, 213);
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
            this.label7.Location = new System.Drawing.Point(331, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Current - move:";
            // 
            // currentMoveComboBox
            // 
            this.currentMoveComboBox.FormattingEnabled = true;
            this.currentMoveComboBox.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%"});
            this.currentMoveComboBox.Location = new System.Drawing.Point(442, 97);
            this.currentMoveComboBox.Name = "currentMoveComboBox";
            this.currentMoveComboBox.Size = new System.Drawing.Size(116, 21);
            this.currentMoveComboBox.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(331, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Current - hold:";
            // 
            // currentHoldComboBox
            // 
            this.currentHoldComboBox.FormattingEnabled = true;
            this.currentHoldComboBox.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%"});
            this.currentHoldComboBox.Location = new System.Drawing.Point(442, 123);
            this.currentHoldComboBox.Name = "currentHoldComboBox";
            this.currentHoldComboBox.Size = new System.Drawing.Size(116, 21);
            this.currentHoldComboBox.TabIndex = 38;
            // 
            // advancedPanel
            // 
            this.advancedPanel.Controls.Add(this.label10);
            this.advancedPanel.Controls.Add(this.label14);
            this.advancedPanel.Controls.Add(this.label13);
            this.advancedPanel.Controls.Add(this.buttonReboot);
            this.advancedPanel.Controls.Add(this.buttonFirmwareInfo);
            this.advancedPanel.Controls.Add(this.label12);
            this.advancedPanel.Controls.Add(this.label6);
            this.advancedPanel.Controls.Add(this.label8);
            this.advancedPanel.Controls.Add(this.numericUpMaxMovement);
            this.advancedPanel.Controls.Add(this.label5);
            this.advancedPanel.Controls.Add(this.currentHoldComboBox);
            this.advancedPanel.Controls.Add(this.numericUpMaxPosition);
            this.advancedPanel.Controls.Add(this.coilsModeCombo);
            this.advancedPanel.Controls.Add(this.label7);
            this.advancedPanel.Controls.Add(this.idleCoilsTimeoutNumeric);
            this.advancedPanel.Controls.Add(this.label4);
            this.advancedPanel.Controls.Add(this.currentMoveComboBox);
            this.advancedPanel.Controls.Add(this.numericUpDownSettleBuffer);
            this.advancedPanel.Controls.Add(this.label9);
            this.advancedPanel.Controls.Add(this.label3);
            this.advancedPanel.Controls.Add(this.numericSetPositionOnConnectValue);
            this.advancedPanel.Controls.Add(this.comboBoxStepSize);
            this.advancedPanel.Controls.Add(this.chkResetOnConnect);
            this.advancedPanel.Controls.Add(this.label11);
            this.advancedPanel.Controls.Add(this.chkSetPositionOnConnect);
            this.advancedPanel.Location = new System.Drawing.Point(312, 2);
            this.advancedPanel.Name = "advancedPanel";
            this.advancedPanel.Size = new System.Drawing.Size(560, 270);
            this.advancedPanel.TabIndex = 40;
            this.advancedPanel.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(319, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 24);
            this.label10.TabIndex = 47;
            this.label10.Text = "System";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(22, 10);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 24);
            this.label14.TabIndex = 46;
            this.label14.Text = "Movement";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(331, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 13);
            this.label13.TabIndex = 45;
            this.label13.Text = "Coils mode:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(331, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 13);
            this.label12.TabIndex = 43;
            this.label12.Text = "Idle coils timeout (ms):";
            // 
            // coilsModeCombo
            // 
            this.coilsModeCombo.FormattingEnabled = true;
            this.coilsModeCombo.Items.AddRange(new object[] {
            "Always on",
            "Idle - off",
            "Idle - coils timeout (ms)"});
            this.coilsModeCombo.Location = new System.Drawing.Point(442, 43);
            this.coilsModeCombo.Name = "coilsModeCombo";
            this.coilsModeCombo.Size = new System.Drawing.Size(116, 21);
            this.coilsModeCombo.TabIndex = 44;
            this.coilsModeCombo.SelectedIndexChanged += new System.EventHandler(this.coilsModeCombo_SelectedIndexChanged);
            // 
            // idleCoilsTimeoutNumeric
            // 
            this.idleCoilsTimeoutNumeric.Location = new System.Drawing.Point(442, 71);
            this.idleCoilsTimeoutNumeric.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.idleCoilsTimeoutNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.idleCoilsTimeoutNumeric.Name = "idleCoilsTimeoutNumeric";
            this.idleCoilsTimeoutNumeric.Size = new System.Drawing.Size(116, 20);
            this.idleCoilsTimeoutNumeric.TabIndex = 42;
            this.idleCoilsTimeoutNumeric.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(319, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(126, 24);
            this.label9.TabIndex = 34;
            this.label9.Text = "Coils control";
            // 
            // showAdvancedBtn
            // 
            this.showAdvancedBtn.Location = new System.Drawing.Point(142, 244);
            this.showAdvancedBtn.Name = "showAdvancedBtn";
            this.showAdvancedBtn.Size = new System.Drawing.Size(79, 25);
            this.showAdvancedBtn.TabIndex = 41;
            this.showAdvancedBtn.Text = "Advanced >>";
            this.showAdvancedBtn.UseVisualStyleBackColor = true;
            this.showAdvancedBtn.Click += new System.EventHandler(this.showAdvancedBtn_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 284);
            this.Controls.Add(this.showAdvancedBtn);
            this.Controls.Add(this.advancedPanel);
            this.Controls.Add(this.chkReverseDirection);
            this.Controls.Add(this.label1);
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
            this.advancedPanel.ResumeLayout(false);
            this.advancedPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idleCoilsTimeoutNumeric)).EndInit();
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
        private System.Windows.Forms.Button buttonReboot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox currentMoveComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox currentHoldComboBox;
        private System.Windows.Forms.Panel advancedPanel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox coilsModeCombo;
        private System.Windows.Forms.NumericUpDown idleCoilsTimeoutNumeric;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button showAdvancedBtn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
    }
}