using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "firmware\\avrdude.exe";
            startInfo.Arguments = "-Cfirmware\\avrdude.conf -q -q -p" + board.Mcu + " -c" +
                                    board.Protocol + " -P" + port + " -b" + board.BaudRate +
                                    " -D -Uflash:w:firmware\\" + board_string + ".hex:i";
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = false;
            Process proc = new Process();
            proc.StartInfo = startInfo;

            proc.OutputDataReceived += new DataReceivedEventHandler(AvrDudeStdOutHandler);
            proc.ErrorDataReceived += new DataReceivedEventHandler(AvrDudeErrorHandler);

            proc.Start();
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            if (!proc.WaitForExit(15 * 1000))
                proc.Kill();

            return true;
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
    }

    public class Board
    {
        public string Mcu { get; set; }
        public string Protocol { get; set; }
        public int BaudRate { get; set; }
    }

}
