using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PitBox
{
    public partial class Form1 : C1.Win.C1Ribbon.C1RibbonForm
    {
        SerialPort mSerialPort;
        Packet mPacket;
        Thread mReadThread;
        Thread mCommInitThread;
        ManualResetEvent mShutdownEvent = new ManualResetEvent(false);
        ManualResetEvent mPauseEvent = new ManualResetEvent(true);
        bool mConnectionEstablished = false;

        private static Mutex mSerialPortMutex = new Mutex();
        private readonly ushort CurrentFirmwareVersion = 2;

        public static string BoardType { get; set; }

        public Form1()
        {
            InitializeComponent();
            mPacket = new Packet();
            Form1.CheckForIllegalCrossThreadCalls = false;

            if (ApplicationDeployment.IsNetworkDeployed)
                lblVersion.Text = "Version: " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            else
                lblVersion.Text = "Version: " + Application.ProductVersion;
        }

        private void StartCommThreads()
        {
            mReadThread = new Thread(new ThreadStart(ReadComm));
            mReadThread.Start();

            mCommInitThread = new Thread(new ThreadStart(EstablishConnection));
            mCommInitThread.Start();
        }

        private void PauseCommThread()
        {
            mPauseEvent.Reset();
        }

        private void ResumeCommThread()
        {
            mPauseEvent.Set();
        }

        private void StopCommThread()
        {
            mShutdownEvent.Set();
            mPauseEvent.Set();
            mReadThread.Join();
        }

        public void SetupComm()
        {
            var ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                // TODO: Some kind of retry timer?
                return;
            }

            foreach (var port in ports)
                serialPortCombo.Items.Add(port);

            mSerialPort = new SerialPort();
            mSerialPort.BaudRate = 9600;
            mSerialPort.DtrEnable = true;

            serialPortCombo.SelectedIndex = 0;
        }

        public void ReadComm()
        {
            int bytes_available;
            UInt16 headerBytesRead = 0, payloadBytesRemain = 0;
            bool havePacket = false;

            byte[] prefix = {80, 66, 83, 80, 32, 118, 49, 58};
            var encoding = new ASCIIEncoding();
            while (true)
            {
                mPauseEvent.WaitOne(Timeout.Infinite);

                if (mShutdownEvent.WaitOne(0))
                    break;

                bytes_available = mSerialPort.BytesToRead;

                if (havePacket)
                    continue;

                if (headerBytesRead < 8)
                {
                    while (bytes_available > 0)
                    {
                        var next_read = mSerialPort.ReadByte();
                        var next_char = Convert.ToChar(next_read);
                        byte next_byte = (byte)next_char;
                        bytes_available--;

                        if (next_byte == prefix[headerBytesRead])
                        {
                            headerBytesRead++;
                            if (headerBytesRead == 8)
                            {
                                // Found start of packet, break
                                break;
                            }
                        }
                        else
                        {
                            // Wrong character in prefix; reset framing.
                            if (next_byte == prefix[0])
                            {
                                headerBytesRead = 1;
                            }
                            else
                            {
                                headerBytesRead = 0;
                            }
                        }
                    }
                }

                // Read the remainder of the header, if not yet found.
                if (headerBytesRead < 12)
                {
                    if (bytes_available < 4)
                    {
                        continue;
                    }
                    var ptype = new byte[2];
                    mSerialPort.Read(ptype, 0, 2);
                    mPacket.Type = BitConverter.ToUInt16(ptype, 0);
                    var payloadBytes = new byte[2];
                    mSerialPort.Read(payloadBytes, 0, 2);
                    payloadBytesRemain = BitConverter.ToUInt16(payloadBytes, 0);
                    bytes_available -= 4;
                    headerBytesRead += 4;

                    // Check that the length field is not bogus
                    if (payloadBytesRemain > 112)
                        continue;
                }

                if (bytes_available == 0 || headerBytesRead < 12)
                    continue;

                if (payloadBytesRemain > 0)
                {
                    int bytes_to_read = (payloadBytesRemain >= bytes_available) ? bytes_available : payloadBytesRemain;
                    var read_buff = new byte[bytes_to_read];
                    mSerialPort.Read(read_buff, 0, bytes_to_read);
                    mPacket.AppendByte(read_buff, bytes_to_read);
                    payloadBytesRemain -= (UInt16)bytes_to_read;
                    bytes_available -= bytes_to_read;
                }

                if (payloadBytesRemain > 0)
                    continue;

                if (!havePacket)
                {
                    if (bytes_available < 4)
                        continue;

                    var footer_bytes = new byte[4];
                    mSerialPort.Read(footer_bytes, 0, 4);

                    havePacket = true;
                }

                if (havePacket)
                {
                    var packet = mPacket.GetPacket();
                    textBox1.AppendText(encoding.GetString(packet, 0, 8));
                    textBox1.AppendText(" Payload length: " + BitConverter.ToInt16(packet, 10));
                    textBox1.AppendText(" Type: " + mPacket.Type + Environment.NewLine);

                    switch (mPacket.Type)
                    {
                        case 32:
                            UpdateConfigValues();
                            break;

                        case 01:
                            UpdateHardwareVersion();
                            mConnectionEstablished = true;
                            lblConnectionStatus.Text = "Connected";
                            break;

                        default:
                            break;
                    }

                    mPacket = new Packet();
                    havePacket = false;
                    headerBytesRead = 0;
                    payloadBytesRemain = 0;
                }
            }
        }

        private void UpdateConfigValues()
        {
            txtRed.Text = mPacket.GetTag(0x01).ToString();
            txtGreen.Text = mPacket.GetTag(0x02).ToString();
            txtDelay.Text = mPacket.GetTag(0x03).ToString();
        }

        private void UpdateHardwareVersion()
        {
            var boardVersion = mPacket.GetTag(0x01);
            
            lblHwVersion.Text = "Firmware Version: " + boardVersion.ToString();

            if (boardVersion < CurrentFirmwareVersion)
                MessageBox.Show("The current firmware is out of date. Please flash the latest firmware to ensure proper functionality.", "New Firmware");
        }

        private void SendPacket(Packet packet)
        {
            mSerialPortMutex.WaitOne();
            if (mSerialPort != null)
                if (mSerialPort.IsOpen)
                {
                    var bytes = packet.GetPacket();
                    mSerialPort.Write(bytes, 0, bytes.Length);
                }
            mSerialPortMutex.ReleaseMutex();
        }

        private void SendPing()
        {
            var packet = new Packet();
            packet.Type = 129;
            SendPacket(packet);
        }

        private void SendGetConfig()
        {
            var packet = new Packet();
            packet.Type = 16;
            SendPacket(packet);
        }

        private void SendSetConfig()
        {
            var packet = new Packet();

            packet.Type = 32;         // 16 = 0x10, 32 = 0x20, 129 = 0x81
            var red = UInt16.Parse(txtRed.Text);
            var green = UInt16.Parse(txtGreen.Text);
            var delay = UInt16.Parse(txtDelay.Text);

            packet.AddTag(0x01, red);
            packet.AddTag(0x02, green);
            packet.AddTag(0x03, delay);

            SendPacket(packet);
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            if (mConnectionEstablished)
                SendSetConfig();
            else
                MessageBox.Show("A connection hasn't been established with board. Configuration can't be uploaded.", "No Connection");
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            if (mConnectionEstablished)
                SendGetConfig();
            else
                MessageBox.Show("A connection hasn't been established wit the board. Configuration can't be read.", "No Connection");
        }

        private void serialPortCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mSerialPort.IsOpen)
            {
                if (mReadThread != null)
                {
                    PauseCommThread();
                }
                DebugMessage("Closing " + mSerialPort.PortName);
                mSerialPort.Close();
                lblCommStatus.Text = "Closed";
                mConnectionEstablished = false;
                lblConnectionStatus.Text = "Not Connected";
                lblHwVersion.Text = "Firmware Version:";
            }

            if (serialPortCombo.SelectedItem == null)
                return;

            mSerialPort.PortName = serialPortCombo.SelectedItem.Text;

            StartComm();
        }

        private void StartComm()
        {
            try
            {
                DebugMessage("Opening " + mSerialPort.PortName);
                mSerialPort.Open();
            }
            catch (System.IO.IOException)
            {
                DebugMessage("Failed opening " + mSerialPort.PortName);
                return;
            }

            if (mSerialPort.IsOpen)
            {
                lblCommStatus.Text = "Open";

                if (mReadThread != null)
                    ResumeCommThread();
                else
                    StartCommThreads();
            }
        }

        private void StopComm()
        {
            if (mReadThread != null)
                if (mReadThread.IsAlive)
                    StopCommThread();

            if (mSerialPort != null)
                if (mSerialPort.IsOpen)
                    mSerialPort.Close();
        }

        private void PauseComm()
        {
            if (mReadThread != null)
                if (mReadThread.IsAlive)
                    PauseCommThread();

            if (mSerialPort != null)
                if (mSerialPort.IsOpen)
                    mSerialPort.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopComm();
        }

        private void time_Validating(object sender, CancelEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            try
            {
                UInt16.Parse(text);
            }
            catch (Exception)
            {
                e.Cancel = true;
                MessageBox.Show("This must be an integer between " + UInt16.MinValue + " and " + UInt16.MaxValue );
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DebugMessage("Setting up COM port");
            SetupComm();
        }

        private void EstablishConnection()
        {
            while (true)
            {
                mPauseEvent.WaitOne(Timeout.Infinite);

                if (mShutdownEvent.WaitOne(0))
                    return;

                if (!mConnectionEstablished)
                {
                    int retries = 1;
                    int delay_ms;

                    DebugMessage("Attempting to ping board");
                    SendPing();
                    Thread.Sleep(50);

                    while (!mConnectionEstablished)
                    {
                        mPauseEvent.WaitOne(Timeout.Infinite);

                        if (mShutdownEvent.WaitOne(0))
                            return;

                        delay_ms = GetBackoff(retries, 200, 10);
                        DebugMessage("Ping failed, retry in " + (delay_ms / 1000) + "s");
                        Thread.Sleep(delay_ms);
                        retries++;
                        SendPing();
                        Thread.Sleep(50);
                    }
                    DebugMessage("Ping successful, getting config");
                    SendGetConfig();
                }
            }
        }

        private static int GetBackoff(int c, int interval, int ceiling = 10)
        {
            if (c <= 0)
                throw new ArgumentOutOfRangeException("Pass a number greater than zero");

            int max_k = (int)Math.Pow(2, Math.Min(c, ceiling)) - 1;

            Random r = new Random();
            int k = r.Next(max_k);

            return k * interval;
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            BoardSelect bs = new BoardSelect();
            if (bs.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
            {
                DebugMessage("Closing " + mSerialPort.PortName);
                PauseComm();
                Thread.Sleep(500);
                Firmware fw = new Firmware();
                if (!fw.Upload(BoardType, serialPortCombo.SelectedItem.Text))
                    MessageBox.Show("Flashing did not complete successfully, double check your connections.", "Flash Fail");

                MessageBox.Show(fw.GetStdErr(), "Flash Output");

                StartComm();
            }
        }

        private void DebugMessage(string message)
        {
            textBox1.AppendText(message + Environment.NewLine);
        }
    }
}
