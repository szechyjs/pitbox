using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PitBox
{
    public class Firmware
    {
        Dictionary<string, Board> mBoards;
        string mErrorString, mStdOutString;

        public Firmware()
        {
            mBoards = new Dictionary<string, Board>();
            mBoards.Add("uno", new Board { Mcu = "atmega328p", BaudRate = 115200, Protocol = "arduino" });
            mBoards.Add("promicro16", new Board { Mcu = "atmega32u4", BaudRate = 57600, Protocol = "avr109" });
            mErrorString = "";
            mStdOutString = "";
        }

        public bool Upload(string board_string, string port)
        {
            var board = mBoards[board_string];

            if (board_string == "promicro16")
                port = GetBootloaderPort(port);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "firmware\\avrdude.exe";
            startInfo.Arguments = "-Cfirmware\\avrdude.conf -p" + board.Mcu + " -c" +
                                    board.Protocol + " -P" + port + " -b" + board.BaudRate +
                                    " -D -Uflash:w:firmware\\" + board_string + ".hex:i";
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            Process proc = new Process();
            proc.StartInfo = startInfo;

            proc.OutputDataReceived += new DataReceivedEventHandler(AvrDudeStdOutHandler);
            proc.ErrorDataReceived += new DataReceivedEventHandler(AvrDudeErrorHandler);

            proc.Start();
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            if (!proc.WaitForExit(15 * 1000))
            {
                proc.Kill();
                return false;
            }

            return true;
        }

        private string GetBootloaderPort(string port)
        {
            string retval = port;
            List<string> before = SerialPort.GetPortNames().ToList();

            SerialPort comPort = new SerialPort(port);
            comPort.Open();
            comPort.BaudRate = 1200;
            comPort.Close();

            Thread.Sleep(300);

            int elapsed = 0;
            while (elapsed < 10000)
            {
                List<string> now = SerialPort.GetPortNames().ToList();

                List<string> diff = new List<string>(now);
                foreach (string aPort in before)
                    diff.Remove(aPort);

                if (diff.Count > 0)
                {
                    retval = diff[0];
                    break;
                }

                before = now;
                Thread.Sleep(250);
                elapsed += 250;
            }

            return retval;
        }

        private void AvrDudeStdOutHandler(object sender, DataReceivedEventArgs stdOut)
        {
            if (!String.IsNullOrEmpty(stdOut.Data))
                mStdOutString += stdOut.Data;
        }

        private void AvrDudeErrorHandler(object sender, DataReceivedEventArgs err)
        {
            if (!String.IsNullOrEmpty(err.Data))
                mErrorString += err.Data;
        }

        public string GetStdOut()
        {
            return mStdOutString;
        }

        public string GetStdErr()
        {
            return mErrorString;
        }
    }

    public class Board
    {
        public string Mcu { get; set; }
        public string Protocol { get; set; }
        public int BaudRate { get; set; }
    }

}
