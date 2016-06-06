using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Web;
using System.Text.RegularExpressions;
using Nekoxy;
using Codeplex.Data;

namespace GFHelper_ConsoleTest
{
    class Program
    {

        private static Dictionary<string, string> serverList = new Dictionary<string, string>();
        private static int unixtimeoffset = 0;
        private static string token;
        private static string uid;

        static void Main(string[] args)
        {
            //token = "63d60903eab97f3ddbb88ef213db3cf1";
            downloadServerInfo();
            HttpProxy.Shutdown();
            //HttpProxy.UpstreamProxyConfig = new ProxyConfig(ProxyConfigType.SpecificProxy, null, 8000);
            HttpProxy.AfterSessionComplete += sessionComplete;

            Console.WriteLine("代理开启：0.0.0.0:8888...");
            HttpProxy.Startup(8888, false, false);
            Console.ReadKey();
            
        }

        private static void sessionComplete(Session obj)
        {
            Task.Run(() =>
            {
                processData(obj);
            });
        }

        private static void processData(Session obj)
        {
            string host = obj.Request.Headers.Host;
            KeyValuePair<string, string> server = getServerFromDictionary(host);

            if (server.Key != "" && server.Value != "") 
            {
                //dirty code
                string api = obj.Request.RequestLine.URI.Substring(server.Key.Length - ("http://" + host).Length);
                Console.WriteLine(api);



                
                switch (api)
                {
                    case "Index/getDigitalSkyNbUid"://core part
                        string data = obj.Response.BodyAsString;
                        string decoded = AuthCode.Decode(data, "yundoudou");
                        Console.WriteLine(decoded);
                        dynamic jsonobj = DynamicJson.Parse(decoded);
                        token = jsonobj.sign;
                        uid = jsonobj.uid;
                        Console.WriteLine("已获取Token: {0}", token);
                        break;

                    default:

                        string req = obj.Request.BodyAsString;
                        req = System.Web.HttpUtility.UrlDecode(req);
                        Console.WriteLine(req);
                        try
                        {
                            string outcode = HttpUtility.ParseQueryString(req)["outdatacode"];
                            Console.WriteLine(outcode);
                            string decode = AuthCode.Decode(outcode, token);
                            Console.WriteLine("Request(token):" + decode);
                            decode = AuthCode.Decode(outcode, uid);
                            Console.WriteLine("Request(uid):" + decode);
                        }
                        catch (Exception) { }



                        if (String.IsNullOrEmpty(token))
                        {
                            Console.WriteLine("TOKEN NOT FOUND!");
                            break;
                        }
                        string ddata = obj.Response.BodyAsString;
                        string ddecoded = AuthCode.Decode(ddata, token);
                        if (!String.IsNullOrEmpty(ddecoded))
                            Console.WriteLine(ddecoded);
                        else
                            Console.WriteLine(ddata);
                        break;
                }

            }
        }

        private static void downloadServerInfo()
        {
            string url = "http://adr.transit.gf.ppgame.com/index.php";
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.Encoding = Encoding.UTF8;
                byte[] postData = wc.Encoding.GetBytes("c=game&a=serverList&channel=cn_mica");
                string result = Encoding.UTF8.GetString(wc.UploadData(url, "POST", postData));

                XmlDocument serverxml = new XmlDocument();
                serverxml.LoadXml(result);
                var serverrootele = serverxml.DocumentElement;
                var servers = serverrootele.GetElementsByTagName("server");
                foreach(XmlElement item in servers)
                {
                    string servername = item.GetElementsByTagName("name")[0].InnerText;
                    string serveraddr = item.GetElementsByTagName("addr")[0].InnerText;
                    serverList.Add(serveraddr, servername);
                    Console.WriteLine("加入服务器至字典:{0}", servername);
                } 
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

        }
        
        private static string getHost(string addr)
        {
            return addr.Split('/')[2];
        }

        private static KeyValuePair<string, string> getServerFromDictionary(string host)
        {
            foreach(var item in serverList)
            {
                if (getHost(item.Key) == host) return item;
            }
            return new KeyValuePair<string, string>("","");
        }



    }


}
