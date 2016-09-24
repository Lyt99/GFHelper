using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace GFHelper
{
    class ConfigManager
    {
        struct ConfigNode
        {
            public string key;
            public string value;

            public ConfigNode(string key, string value)
            {
                this.key = key;
                this.value = value;
            }

            public override string ToString()
            {
                return this.value;
            }
        }

        public static string fileName;

        private InstanceManager im;
        private Dictionary<int, ConfigNode> config;
        private int maxline = 0;

        public ConfigManager(InstanceManager im)
        {
            fileName = (string)Properties.Settings.Default["ConfigFile"];
            this.im = im;
            this.config = new Dictionary<int, ConfigNode>();
            im.logger.Log(fileName);
        }

        public bool Load()
        {
            if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return false;
            
            string[] con = File.ReadAllLines(fileName);

            try
            {
                int linenum = -1;
                foreach (string line in con)
                {
                    ++linenum;
                    if (String.IsNullOrEmpty(line) || line[0] == '#') continue;//注释
                    string[] c = line.Split('=');
                    config.Add(linenum, new ConfigNode(c[0].Trim().ToLower(), c[1].Trim().ToLower()));
                }

                maxline = con.Length;
                return true;
            }
            catch(Exception e)
            {
                im.logger.Log("配置文件加载失败: " + e.ToString());
                return false;
            }

        }

        public void Save()
        {
            try
            {
                var configfile = File.ReadAllLines(fileName);
                string[] newconfigfile = new string[maxline];

                for (int i = 0; i < configfile.Length; ++i)
                    newconfigfile[i] = configfile[i];

                foreach (var line in config)
                {
                    newconfigfile[line.Key] = String.Format("{0}={1}", line.Value.key, line.Value.value);
                }

                File.WriteAllLines(fileName, newconfigfile);
            }
            catch(Exception e)
            {
                im.logger.Log("配置文件保存失败: " + e.Message);
            }
        }

        private ConfigNode findConfig(string key)
        {
            foreach(var i in config)
                if (i.Value.key == key) return i.Value;

            throw new KeyNotFoundException();
        }

        public void SetConfig(string key, object value)
        {
            try
            {
                var i = findConfig(key);
                i.value = value.ToString();
            }
            catch(KeyNotFoundException)
            {
                ConfigNode cn = new ConfigNode();
                cn.key = key;
                cn.value = value.ToString();
                this.config.Add(maxline++, cn);
            }

            this.Save();
        }

        public string getConfigString(string key)
        {
            try
            {
                return findConfig(key).ToString();
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
                return findConfig(key).ToString() == "true";
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
                return Convert.ToInt32(findConfig(key).ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
