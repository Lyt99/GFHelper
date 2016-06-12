using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nekoxy;
using System.Windows.Controls;
using Codeplex.Data;
using System.Windows.Threading;
using System.Web;
using System.Collections.Specialized;
using GFHelper.Models;

namespace GFHelper
{
    class Listener
    {

        private InstanceManager im;

        private string token;//讲道理，这就是sign
        private string uid;

        public Listener(InstanceManager im)
        {

            this.im = im;
            

            token = "";
        }

        ~Listener()
        {
            HttpProxy.Shutdown();
        }

        public void Shutdown()
        {
            HttpProxy.Shutdown();
        }

        public void startProxy(int port)
        {

            if (!im.serverHelper.downloadServerInfo())
            {
                im.uiHelper.setStatusBarText("服务器列表获取失败！");
                return;
            }
            
            int servernum = im.serverHelper.getServersNumber();

            HttpProxy.Shutdown();

            HttpProxy.AfterSessionComplete += (obj) => Task.Run(() => { processData(obj); });
            HttpProxy.Startup(port, false, false);

            string proxyaddr = String.Format("{0}:{1}", im.serverHelper.getLocalAddress(), port);

            im.uiHelper.setStatusBarText(String.Format("代理在{0}上开启成功，等待连接……已添加{1}个服务器", proxyaddr, servernum));

        }


        private void processData(Session obj)
        {
            string host = obj.Request.Headers.Host;
            KeyValuePair<string, string> server = im.serverHelper.getServerFromDictionary(host);

            if (server.Key == "" && server.Value == "") return;

            string api = obj.Request.RequestLine.URI.Substring(server.Key.Length - ("http://" + host).Length);
            Console.WriteLine(api);
            switch (api)
            {
                case RequestUrls.GetDigitalUid:
                    {
                        string data = obj.Response.BodyAsString;
                        string decoded = AuthCode.Decode(data, "yundoudou");
                        Console.WriteLine(decoded);
                        dynamic jsonobj = DynamicJson.Parse(decoded);
                        token = jsonobj.sign;
                        uid = jsonobj.uid;

                        //wtf?
                        Models.SimpleUserInfo.sign = token;
                        Models.SimpleUserInfo.uid = uid;

                        im.uiHelper.setStatusBarText_InThread(String.Format("已登录服务器: {0}，token: {1}，uid: {2}", server.Value, token, uid));
                        Models.SimpleUserInfo.host = server.Key;
                        break;
                    }


                case RequestUrls.GetServerTime://同步服务器时间
                    {
                        if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(uid))
                        {
                            im.uiHelper.setStatusBarText_InThread("未获取到token！请重新登录游戏以使本工具正常工作！");
                            break;
                        }
                        if (String.IsNullOrEmpty(token)) break;
                        string decoded = AuthCode.Decode(obj.Response.BodyAsString, token);
                        dynamic jsonobj = DynamicJson.Parse(decoded);

                        Console.WriteLine("Server: " + jsonobj.now + " \nClient: " + CommonHelper.ConvertDateTimeInt(DateTime.Now));
                        SimpleUserInfo.timeoffset = Convert.ToInt32(jsonobj.now) - CommonHelper.ConvertDateTimeInt(DateTime.Now);//server = local + offset
                        Console.WriteLine("Set timeoffset: " + SimpleUserInfo.timeoffset);
                        break;
                    }


                default:
                    {
                        if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(uid))
                        {
                            im.uiHelper.setStatusBarText_InThread("未获取到token！请重新登录游戏以使本工具正常工作！");
                        }
                        else
                        {
                            try {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("api: " + api + "\n");
                                NameValueCollection clientdata = new NameValueCollection();
                                string serverdata = AuthCode.Decode(obj.Response.BodyAsString, token);

                                if (String.IsNullOrEmpty(serverdata))//没有加密
                                    serverdata = obj.Response.BodyAsString;

                                if (!String.IsNullOrEmpty(obj.Response.BodyAsString))
                                {

                                    clientdata = HttpUtility.ParseQueryString(obj.Request.BodyAsString);
                                    if (clientdata.AllKeys.Contains("outdatacode"))
                                    {
                                        clientdata["outdatacode"] = AuthCode.Decode(clientdata["outdatacode"], token, SimpleUserInfo.timeoffset);
                                        Console.WriteLine("outdatacode: " + clientdata["outdatacode"]);
                                        sb.Append("client: " + clientdata["outdatacode"] + "\n");

                                    }
                                    else
                                        clientdata["outdatacode"] = "[]";

                                    sb.Append("server: " + serverdata);
                                    im.logger.Log(sb.ToString());
                                    
                                }

                                if(serverdata.Length < 100)
                                Console.WriteLine("Serverdata: " + serverdata);
                                processMainData(api, clientdata, serverdata);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        break;
                    }

            }
        }


        public void processMainData(string api, NameValueCollection client, string server)
        {
            
            dynamic clientjson = DynamicJson.Parse(client["outdatacode"]);
            switch (api)
            {
                case RequestUrls.GetUserInfo:
                    {
                        im.dataHelper.ReadUserInfo(server);
                        im.uiHelper.setUserInfo();
                        im.autoOperation.SetTeamInfo();
                        break;
                    }

                //建造
                case RequestUrls.DevelopGun:
                    {
                        
                        dynamic serverjson = DynamicJson.Parse(server);

                        int buildslot = Convert.ToInt32(clientjson.build_slot);
                        int resultid = Convert.ToInt32(serverjson.gun_id);
                        im.uiHelper.setDevelopingTimer(im.timer, buildslot, resultid, CommonHelper.ConvertDateTimeInt(DateTime.Now));
                        im.uiHelper.setStatusBarText_InThread(String.Format("建造结果: {0} 于建造槽{1}", Data.gunInfo[resultid].name, buildslot));
                        break;
                    }


                case RequestUrls.FinishDevelopGun:
                    {
                        im.uiHelper.setFactoryTimerDefault(Convert.ToInt32(clientjson.build_slot));
                        break;
                    }

                //后勤有关
                case RequestUrls.StartOperation:
                    {
                        Models.AutoOperationInfo ao = null;
                        im.mainWindow.Dispatcher.Invoke(() =>
                        {
                            ao = new Models.AutoOperationInfo(Convert.ToInt32(clientjson.team_id), Convert.ToInt32(clientjson.operation_id));
                        });
                        
                        im.autoOperation.AddTimerStartOperation(ao);
                        break;
                    }

                case RequestUrls.FinishOperation:
                    {
                        Models.AutoOperationInfo ao = im.autoOperation.FindOperation(Convert.ToInt32(clientjson.operation_id));

                        im.mainWindow.Dispatcher.Invoke(() =>
                        {
                            im.autoOperation.operationList.Remove(ao);
                        });

                        break;
                    }

                case RequestUrls.AbortOperation:
                    {
                        Models.AutoOperationInfo ao = im.autoOperation.FindOperation(Convert.ToInt32(clientjson.operation_id));
                        im.autoOperation.operationList.Remove(ao);
                        break;
                    }

                //资源恢复
                case RequestUrls.RecoverResource:
                    {

                        break;
                    }
            }

        }





    }
}
