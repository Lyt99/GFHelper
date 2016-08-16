using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace GFHelper
{
    class ServerHelper
    {

        private Dictionary<string, string> serverList;
        private InstanceManager im;

        public ServerHelper(InstanceManager im)
        {
            this.im = im;
            this.serverList = new Dictionary<string, string>();
        }

        public string GetLocalAddress()
        {
            return "0.0.0.0";
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "0.0.0.0";
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }

    }

        public string DoPost(string url, string data)
        {
            try
            {
                Console.WriteLine("doPost(): " + url + "====" + data);
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Encoding = Encoding.UTF8;
                byte[] postData = wc.Encoding.GetBytes(data);
                string result = Encoding.UTF8.GetString(wc.UploadData(url, "POST", postData));
                return result;
            }
            catch(System.Net.WebException e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        public bool ReadServerInfo(string data)
        {
            try
            {
                XmlDocument serverxml = new XmlDocument();
                serverxml.LoadXml(data);
                var serverrootele = serverxml.DocumentElement;
                var servers = serverrootele.GetElementsByTagName("server");
                foreach (XmlElement item in servers)
                {
                    string servername = item.GetElementsByTagName("name")[0].InnerText;
                    string serveraddr = item.GetElementsByTagName("addr")[0].InnerText;
                    serverList.Add(serveraddr, servername);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public int GetServerNumber()
        {
            return serverList.Count;
        }

        private string GetHost(string addr)
        {
            return addr.Split('/')[2];
        }

        public KeyValuePair<string, string> GetServerFromDictionary(string host)
        {
            foreach (var item in serverList)
            {
                if (GetHost(item.Key) == host) return item;
            }
            return new KeyValuePair<string, string>("", "");
        }

        public void SendDataToServer_t(string url, string data, bool ifProcessData = false)
        {
            Task.Run(() =>
            {
               SendDataToServer(url, data, ifProcessData);
            });
        }
        //慎用
        public string SendDataToServer(string url, string data, bool ifProcessData = false)
        {
            string host = Models.SimpleInfo.host;
            string outdatacode = AuthCode.Encode(data, Models.SimpleInfo.sign);
            string requeststring = String.Format("uid={0}&outdatacode={1}&req_id={2}", Models.SimpleInfo.uid, HttpUtility.UrlEncode(outdatacode), ++Models.SimpleInfo.reqid);
            Console.WriteLine(requeststring);
            string result = DoPost(host + url, requeststring);

            string nresult = AuthCode.Decode(result, Models.SimpleInfo.sign);
            if (String.IsNullOrEmpty(nresult)) nresult = result;

            if (ifProcessData)
            {
                var par = System.Web.HttpUtility.ParseQueryString(requeststring);
                par["outdatacode"] = data;

                im.listener.processMainData(url.ToString(),par,nresult);
            }
            return nresult;
        }

        public bool UploadBuildResult(dynamic clientjson, int gunid)
        {
            string url = "http://gfdb.baka.pw/api/upload.php";
            StringBuilder sb = new StringBuilder();
            sb.Append("mp=" + Convert.ToInt32(clientjson.mp).ToString());
            sb.Append("&ammo=" + Convert.ToInt32(clientjson.ammo).ToString());
            sb.Append("&mre=" + Convert.ToInt32(clientjson.mre).ToString());
            sb.Append("&part=" + Convert.ToInt32(clientjson.part).ToString());
            sb.Append("&gunid=" + gunid.ToString());
            sb.Append("&time=" + CommonHelper.ConvertDateTimeInt(DateTime.UtcNow));
            sb.Append(string.Format("&extra={0},{1},{2}", Models.SimpleInfo.uid, Data.userInfo.level, Models.SimpleInfo.platform.ToString()));

            string result = DoPost(url, sb.ToString());

            return result == "1";

        }

    }
}
