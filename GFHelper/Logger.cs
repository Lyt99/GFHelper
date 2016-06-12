using System.Threading.Tasks;
using System.IO;
using System;
using System.Text;
using System.Threading;

namespace GFHelper
{
    class Logger
    {
        public bool ifLog;
        private bool locked = false;
        private InstanceManager im;
        private FileStream fs;
        private string logFileName = "log.log";

        public Logger(InstanceManager im)
        {
            this.ifLog = true;
            this.im = im;
            fs = new FileStream(logFileName, FileMode.Append);

        }

        ~Logger()
        {
            fs.Close();
        }

        public void Log(string logstr)
        {
            while (locked)
                Thread.Sleep(100);
            locked = true;
            Console.WriteLine("Logging...");
            if (!ifLog) return;
            string log = string.Format("[{0}]\n{1}\n", DateTime.Now.ToString(), logstr);
            byte[] logbyte = Encoding.Default.GetBytes(log);
            fs.Write(logbyte, 0, logbyte.Length);
            fs.Flush();
            Console.WriteLine("Logged.");
            locked = false;

        }
    }
}
