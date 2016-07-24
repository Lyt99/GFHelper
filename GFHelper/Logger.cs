using System.Threading.Tasks;
using System.IO;
using System;
using System.Text;
using System.Threading;

namespace GFHelper
{
    class Logger
    {
        public bool ifBuildLog;
        public bool ifLog;
        private bool locked = false;
        private InstanceManager im;
        //private FileStream fs;
        private string logFileName = "log.log";
        private string buildLogFileName = "buildlog.txt";

        public Logger(InstanceManager im)
        {
            this.im = im;
            this.ifLog = false;

        }


        public void Log(string logstr, string logFile = null, bool forcelog = false)
        {
            if (String.IsNullOrEmpty(logFile)) logFile = this.logFileName;

            Console.WriteLine(logstr);
            while (locked)
                Thread.Sleep(100);

            FileStream fs = new FileStream(logFile, FileMode.Append);
            if (!ifLog || !forcelog) return;
            locked = true;
            string log = string.Format("[{0}]\n{1}\n", DateTime.Now.ToString(), logstr);
            byte[] logbyte = Encoding.Default.GetBytes(log);
            fs.Write(logbyte, 0, logbyte.Length);
            fs.Flush();
            fs.Close();
            locked = false;
        }

        public void LogBuildResult(string logstr)
        {

            Console.WriteLine(logstr);
            while (locked)
                Thread.Sleep(100);

            FileStream fs = new FileStream(buildLogFileName, FileMode.Append);
            if (!ifLog) return;
            locked = true;
            string log = string.Format("[{0}]\n{1}\n", DateTime.Now.ToString(), logstr);
            byte[] logbyte = Encoding.Default.GetBytes(log);
            fs.Write(logbyte, 0, logbyte.Length);
            fs.Flush();
            fs.Close();
            locked = false;
        }
    }
}
