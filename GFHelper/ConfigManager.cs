using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GFHelper
{
    class ConfigManager
    {
        public static string fileName;

        private InstanceManager im;
        private Dictionary<string, string> config;
        private bool ifLoad;

        public ConfigManager(InstanceManager im)
        {
            Console.WriteLine("load");
            fileName = (string)Properties.Settings.Default["ConfigFile"];
            this.im = im;
            this.ifLoad = false;
            this.config = new Dictionary<string, string>();
            Console.WriteLine(fileName);
        }

        public bool Load()
        {
            if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return false;
            
            string[] con = File.ReadAllLines(fileName);

            try
            {
                foreach (string line in con)
                {
                    if (String.IsNullOrEmpty(line) || line[0] == '#') continue;//注释
                    string[] c = line.Split('=');
                    config.Add(c[0].Trim().ToLower(), c[1].Trim().ToLower());
                }

                this.ifLoad = true;
                return true;
            }
            catch(Exception e)
            {
                im.logger.Log("配置文件加载失败: " + e.ToString());
                return false;
            }

        }

        public string getConfigString(string key)
        {
            try
            {
                return config[key];
            }
            catch(KeyNotFoundException)
            {
                return String.Empty;
            }
        }

        public bool getConfigBool(string key)
        {
            try
            {
                return config[key] == "true";
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public int getConfigInt(string key)
        {
            try
            {
                return Convert.ToInt32(config[key]);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
