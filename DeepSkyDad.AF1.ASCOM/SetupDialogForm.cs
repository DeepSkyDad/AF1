using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Utilities;

namespace ASCOM.DeepSkyDad.AF1
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private Focuser _f;

        public SetupDialogForm(Focuser f)
        {
            _f = f;
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Focuser.comPort = (string)comboBoxComPort.SelectedItem;
            Focuser.maxPosition = (int)numericUpMaxPosition.Value;
            Focuser.maxMovement = (int)numericUpMaxMovement.Value;
            Focuser.stepSize = (string)comboBoxStepSize.SelectedItem;
            Focuser.traceState = chkTrace.Checked;
            Focuser.resetOnConnect = chkResetOnConnect.Checked;
            Focuser.setPositonOnConnect = chkSetPositionOnConnect.Checked;
            if(Focuser.setPositonOnConnect)
                Focuser.setPositionOnConnectValue = (int)numericSetPositionOnConnectValue.Value;
            Focuser.alwaysOn = chkAlwaysOn.Checked;
            Focuser.reverseDirection = chkReverseDirection.Checked;
            Focuser.settleBuffer = (int)numericUpDownSettleBuffer.Value;
            Focuser.currentMove = (string)currentMoveComboBox.SelectedItem;
            Focuser.currentAo = (string)currentAoComboBox.SelectedItem;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            ToolTip tt = new ToolTip();
            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 1000;
            tt.ReshowDelay = 500;
            tt.SetToolTip(alwaysOnLabel, "Can only be turned off for step size 1. With microstep precision focusing, coils must be powered at all times.");

            chkTrace.Checked = Focuser.traceState;
            numericUpMaxPosition.Value = Focuser.maxPosition;
            numericUpMaxMovement.Value = Focuser.maxMovement;
            comboBoxStepSize.Text = Focuser.stepSize;
            chkResetOnConnect.Checked = Focuser.resetOnConnect;
            chkSetPositionOnConnect.Checked = Focuser.setPositonOnConnect;
            numericSetPositionOnConnectValue.Value = Focuser.setPositionOnConnectValue;
            numericSetPositionOnConnectValue.Visible = Focuser.setPositonOnConnect;
            chkAlwaysOn.Checked = Focuser.alwaysOn;
            chkAlwaysOn.Enabled = Focuser.stepSize == "1";
            chkReverseDirection.Checked = Focuser.reverseDirection;
            numericUpDownSettleBuffer.Value = Focuser.settleBuffer;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Focuser.comPort))
            {
                comboBoxComPort.SelectedItem = Focuser.comPort;
            }
            currentMoveComboBox.Text = Focuser.currentMove;
            currentAoComboBox.Text = Focuser.currentAo;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkAlwaysOn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkResetOnConnet_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkTrace_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonFirmwareInfo_Click(object sender, EventArgs e)
        {
            var comPort = (string)comboBoxComPort.SelectedItem;
            if (string.IsNullOrWhiteSpace(comPort))
            {
                MessageBox.Show("Please select COM port", "Error");
                return;
            }

            if (_f.Connected)
            {
                var firmwareVersion = _f.CommandString("GSFR");
                ShowNonBlockingMessageBox($"v{firmwareVersion}", "Firmware version");
                return;
            }

            try
            {
                Focuser.comPort = comPort;
                _f.Connected = true;
                var firmwareVersion = _f.CommandString("GSFR");
                ShowNonBlockingMessageBox($"v{firmwareVersion}", "Firmware version");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Connection to the focuser failed ({ex.Message})", "Error");
            }
            finally
            {
                if (_f.Connected)
                    _f.Connected = false;
            }
        }

        private void ShowNonBlockingMessageBox(string text, string caption)
        {
            Thread t = new Thread(() => MessageBox.Show(text, caption));
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void chkSetPositionOnConnect_CheckedChanged(object sender, EventArgs e)
        {
            numericSetPositionOnConnectValue.Visible = chkSetPositionOnConnect.Checked;
        }

        private void comboBoxStepSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxStepSize.Text != "1")
            {
                chkAlwaysOn.Enabled = false;
                chkAlwaysOn.Checked = true;
            } else
            {
                chkAlwaysOn.Enabled = true;
            }
        }

        private void buttonReboot_Click(object sender, EventArgs e)
        {
            var comPort = (string)comboBoxComPort.SelectedItem;
            if (string.IsNullOrWhiteSpace(comPort))
            {
                MessageBox.Show("Please select COM port", "Error");
                return;
            }

            try
            {
                Focuser.comPort = comPort;
                _f.Connected = true;
                _f.CommandBlind("RBOT");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection to the focuser failed ({ex.Message})", "Error");
            }
            finally
            {
                if (_f.Connected)
                    _f.Disconnect();
            }

            MessageBox.Show("Reboot successful!");
        }
    }
}
