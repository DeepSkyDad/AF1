using CliWrap;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSkyDad.AF1.Arduino.FirmwareUpgrade
{
    public partial class Form1 : Form
    {
        private static string _output;
        private static PrivateFontCollection _fonts = new PrivateFontCollection();

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }

        public Form1()
        {
            InitializeComponent();
           
            //var test = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            AddFontFromResource("DeepSkyDad.AF1.Arduino.FirmwareUpgrade.EmbeddedFiles.TitilliumWeb-Light.ttf");
            AddFontFromResource("DeepSkyDad.AF1.Arduino.FirmwareUpgrade.EmbeddedFiles.AlegreyaSansSC-Bold.ttf");
            //_fonts.AddFontFile("TitilliumWeb-Light.ttf");
            //_fonts.AddFontFile("AlegreyaSansSC-Bold.ttf");

            this.ControlBox = false;
            this.Text = String.Empty;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Font = new Font(_fonts.Families[1], 9, FontStyle.Regular);
            this.ForeColor = Color.White;
            this.BackColor = Color.FromArgb(24, 24, 24);

            this.labelTitle.Font = new Font(_fonts.Families[0], 12, FontStyle.Bold);
            this.labelTitle.Left = (this.ClientSize.Width - this.labelTitle.Width) / 2;
            this.labelTitle.Top = 20;

            this.btnChooseFirmware.ForeColor = Color.Black;
            this.btnOutput.ForeColor = Color.Black;
            this.btnUpload.ForeColor = Color.Black;
            this.btnExit.ForeColor = Color.Black;

            UpdateStatusLabel();
            
        }

        private void comPortCombo_DropDown(object sender, EventArgs e)
        {
            this.comPortCombo.Items.Clear();
            var i = 0;
            foreach (var port in SerialPort.GetPortNames())
            {
                this.comPortCombo.Items.Insert(i, port);
                i++;
            }
        }

        private void btnChooseFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Choose firmware .hex file";
            theDialog.Filter = "HEX files|*.hex";
            theDialog.InitialDirectory = KnownFolders.Downloads.Path;
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxFirmwareFile.Text = theDialog.FileName.ToString();
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(this.textBoxFirmwareFile.Text) || string.IsNullOrWhiteSpace(this.comPortCombo.Text))
            {
                labelStatus.ForeColor = Color.Orange;
                UpdateStatusLabel();
                return;
            }

            labelStatus.ForeColor = Color.Yellow;
            UpdateStatusLabel("Uploading...");
            SetEnabled(false);

            using (var cli = new Cli("Avrdude/avrdude.exe"))
            {
                // Execute
                var output = await cli.ExecuteAsync($"-CAvrdude/avrdude.conf -v -patmega328p -carduino -P{this.comPortCombo.Text} -b57600 -D -Uflash:w:\"{this.textBoxFirmwareFile.Text}\":i ");
                // Extract output
                var code = output.ExitCode;
                var stdOut = output.StandardOutput;
                var stdErr = output.StandardError;
                var startTime = output.StartTime;
                var exitTime = output.ExitTime;
                var runTime = output.RunTime;
                //output.ThrowIfError();

                this.btnOutput.Visible = true;

                if (output.ExitCode != 0)
                {
                    _output = output.StandardError;
                    SetEnabled(true);
                    labelStatus.ForeColor = Color.Red;
                    UpdateStatusLabel("Upload failed - click on Output for details");
                }
                else
                {
                    _output = output.HasError ? output.StandardError : output.StandardOutput;
                    SetEnabled(true);
                    labelStatus.ForeColor = Color.Green;
                    UpdateStatusLabel("Upload successful!");
                }
            }
        }

        private void btnOutputDet_Click(object sender, EventArgs e)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Font = new Font(_fonts.Families[1], 9, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(24, 24, 24)
            };
            RichTextBox tb = new RichTextBox() {
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Text = _output
            };
            prompt.Controls.Add(tb);
            prompt.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SetEnabled(bool enabled)
        {
            this.btnUpload.Enabled = enabled;
            this.btnChooseFirmware.Enabled = enabled;
            this.comPortCombo.Enabled = enabled;
            this.btnExit.Enabled = enabled;
        }

        private void UpdateStatusLabel(string msg = null)
        {
            var messageList = new List<string>();
            if (msg != null)
            {
                messageList.Add(msg);
            }

            if (string.IsNullOrWhiteSpace(this.textBoxFirmwareFile.Text))
            {
                messageList.Add("Please, select firmare HEX file");
            }

            if (string.IsNullOrWhiteSpace(this.comPortCombo.Text))
            {
                messageList.Add("Please, select COM port");
            }

            labelStatus.Text = string.Join(", ", messageList);
        }

        private static void AddFontFromResource(string fontResourceName)
        {
            var fontBytes = GetFontResourceBytes(fontResourceName);
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            _fonts.AddMemoryFont(fontData, fontBytes.Length);
            Marshal.FreeCoTaskMem(fontData);
        }

        private static byte[] GetFontResourceBytes(string fontResourceName)
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontResourceName);
            if (resourceStream == null)
                throw new Exception(string.Format("Unable to find font '{0}' in embedded resources.", fontResourceName));
            var fontBytes = new byte[resourceStream.Length];
            resourceStream.Read(fontBytes, 0, (int)resourceStream.Length);
            resourceStream.Close();
            return fontBytes;
        }
    }
}
