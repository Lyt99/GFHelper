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

        public string getLocalAddress()
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

        public string doPost(string url, string data)
        {
            Console.WriteLine("doPost(): " + url + "---" + data);
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Encoding = Encoding.UTF8;
            byte[] postData = wc.Encoding.GetBytes(data);
            string result = Encoding.UTF8.GetString(wc.UploadData(url, "POST", postData));
            return result;
        }

        public bool downloadServerInfo()
        {
            string url = "http://adr.transit.gf.ppgame.com/index.php";
            try
            {
                string result = doPost(url, "c=game&a=serverList&channel=cn_mica");

                XmlDocument serverxml = new XmlDocument();
                serverxml.LoadXml(result);
                var serverrootele = serverxml.DocumentElement;
                var servers = serverrootele.GetElementsByTagName("server");
                foreach (XmlElement item in servers)
                {
                    string servername = item.GetElementsByTagName("name")[0].InnerText;
                    string serveraddr = item.GetElementsByTagName("addr")[0].InnerText;
                    Console.WriteLine(serveraddr, servername);
                    serverList.Add(serveraddr, servername);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public int getServersNumber()
        {
            return serverList.Count;
        }

        private string getHost(string addr)
        {
            return addr.Split('/')[2];
        }

        public KeyValuePair<string, string> getServerFromDictionary(string host)
        {
            foreach (var item in serverList)
            {
                if (getHost(item.Key) == host) return item;
            }
            return new KeyValuePair<string, string>("", "");
        }

        public void sendDataToServer_t(string url, string data, bool ifProcessData = false)
        {
            Task.Run(() =>
            {
               sendDataToServer(url, data, ifProcessData);
            });
        }
        //慎用
        public string sendDataToServer(string url, string data, bool ifProcessData = false)
        {
            string host = Models.SimpleUserInfo.host;
            string outdatacode = AuthCode.Encode(data, Models.SimpleUserInfo.sign);
            string requeststring = String.Format("uid={0}&outdatacode={1}&req_id={2}", Models.SimpleUserInfo.uid, HttpUtility.UrlEncode(outdatacode), CommonHelper.ConvertDateTimeInt(DateTime.Now, true));
            Console.WriteLine(requeststring);
            string result = doPost(host + url, requeststring);

            string nresult = AuthCode.Decode(result, Models.SimpleUserInfo.sign);
            if (String.IsNullOrEmpty(nresult)) nresult = result;

            if (ifProcessData)
            {
                var par = System.Web.HttpUtility.ParseQueryString(requeststring);
                par["outdatacode"] = data;

                im.listener.processMainData(url.ToString(),par,nresult);
            }
            return nresult;
        }
    }
}
