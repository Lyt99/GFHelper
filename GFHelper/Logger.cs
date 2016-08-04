using System.Threading.Tasks;
using System.IO;
using System;
using System.Text;
using System.Threading;

namespace GFHelper
{
    class Logger
    {
        private bool ifBuildLog;
        private bool ifLog;
        private InstanceManager im;
        //private FileStream fs;
        private string logFileName = "log.log";
        private string buildLogFileName = "buildlog.txt";

        public Logger(InstanceManager im)
        {
            this.im = im;
            this.ifLog = false;
            this.ifBuildLog = false;

        }

        public void SetLogState(bool state){this.ifLog = state;}
        public void SetBuildLogState(bool state){this.ifBuildLog = state;}
        public bool GetIfLog() { return ifLog; }
        public bool GetIfBuildLog() { return ifBuildLog; }


        public void Log(string logstr, string logFile = "", bool forcelog = false)
        {
            if (!ifLog && !forcelog) return;
            if (String.IsNullOrEmpty(logFile)) logFile = this.logFileName;

            Console.WriteLine("Start Logging..." + logFile);
            FileStream fs = new FileStream(logFile, FileMode.Append);
            
            string log = string.Format("[{0}]\n{1}\n", DateTime.Now.ToString(), logstr);
            byte[] logbyte = Encoding.Default.GetBytes(log);
            fs.Write(logbyte, 0, logbyte.Length);
            fs.Flush();
            fs.Close();
        }

        public void LogBuildResult(string logstr)
        {

            Console.WriteLine(logstr);

            FileStream fs = new FileStream(buildLogFileName, FileMode.Append);
            if (!ifLog) return;
            string log = string.Format("[{0}]\n{1}\n", DateTime.Now.ToString(), logstr);
            byte[] logbyte = Encoding.Default.GetBytes(log);
            fs.Write(logbyte, 0, logbyte.Length);
            fs.Flush();
            fs.Close();
        }
    }
}
