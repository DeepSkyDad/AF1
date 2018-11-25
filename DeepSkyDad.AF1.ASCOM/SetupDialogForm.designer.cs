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
            this.chkResetOnConnet = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpMaxPosition = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAlwaysOn = new System.Windows.Forms.CheckBox();
            this.numericUpDownSettleBuffer = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettleBuffer)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(109, 273);
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
            this.cmdCancel.Location = new System.Drawing.Point(174, 273);
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
            this.picASCOM.Location = new System.Drawing.Point(288, 9);
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
            this.label2.Location = new System.Drawing.Point(13, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "COM Port";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(160, 195);
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
            this.comboBoxComPort.Location = new System.Drawing.Point(159, 86);
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
            this.comboBoxStepSize.Location = new System.Drawing.Point(159, 113);
            this.comboBoxStepSize.Name = "comboBoxStepSize";
            this.comboBoxStepSize.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStepSize.TabIndex = 8;
            // 
            // chkResetOnConnet
            // 
            this.chkResetOnConnet.AutoSize = true;
            this.chkResetOnConnet.Location = new System.Drawing.Point(160, 218);
            this.chkResetOnConnet.Name = "chkResetOnConnet";
            this.chkResetOnConnet.Size = new System.Drawing.Size(173, 17);
            this.chkResetOnConnet.TabIndex = 9;
            this.chkResetOnConnet.Text = "Reset settings on next connect";
            this.chkResetOnConnet.UseVisualStyleBackColor = true;
            this.chkResetOnConnet.CheckedChanged += new System.EventHandler(this.chkResetOnConnet_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Step size";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // numericUpMaxPosition
            // 
            this.numericUpMaxPosition.Location = new System.Drawing.Point(160, 140);
            this.numericUpMaxPosition.Maximum = new decimal(new int[] {
            65000,
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
            this.numericUpMaxPosition.TabIndex = 11;
            this.numericUpMaxPosition.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Max. position (10k - 65k)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Settings";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkAlwaysOn
            // 
            this.chkAlwaysOn.AutoSize = true;
            this.chkAlwaysOn.Location = new System.Drawing.Point(160, 242);
            this.chkAlwaysOn.Name = "chkAlwaysOn";
            this.chkAlwaysOn.Size = new System.Drawing.Size(182, 17);
            this.chkAlwaysOn.TabIndex = 15;
            this.chkAlwaysOn.Text = "Always on (useful for focuser slip)";
            this.chkAlwaysOn.UseVisualStyleBackColor = true;
            this.chkAlwaysOn.CheckedChanged += new System.EventHandler(this.chkAlwaysOn_CheckedChanged);
            // 
            // numericUpDownSettleBuffer
            // 
            this.numericUpDownSettleBuffer.Location = new System.Drawing.Point(160, 166);
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
            this.label4.Location = new System.Drawing.Point(16, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Settle buffer (0ms - 5000ms)";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 309);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownSettleBuffer);
            this.Controls.Add(this.chkAlwaysOn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpMaxPosition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkResetOnConnet);
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
            this.Text = "DeepSkyDad Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpMaxPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSettleBuffer)).EndInit();
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
        private System.Windows.Forms.CheckBox chkResetOnConnet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpMaxPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAlwaysOn;
        private System.Windows.Forms.NumericUpDown numericUpDownSettleBuffer;
        private System.Windows.Forms.Label label4;
    }
}