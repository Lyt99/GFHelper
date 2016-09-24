using System;
using System.Web;
using System.IO.Compression;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace GFHelper
{
    class UpdateManager
    {

        private string baseurl = @"http://rescnf.gf.ppgame.com/data/";
        private string filename;
        private InstanceManager im;
        public UpdateManager(InstanceManager im)
        {
            this.im = im;
        }

        public bool CheckUpdate(string dataVersion)
        {
            return !File.Exists("catchdata") || (im.configManager.getConfigBool("autoupdate") && (dataVersion != im.configManager.getConfigString("dataversion")));
        }
        public void UpdateCatchData()
        {
            filename = String.Format("catchdata_{0}.dat", im.serverHelper.getDataVersion());
            
            string url = baseurl + filename;
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += DownloadProgress;
            wc.DownloadFileCompleted += Complete;
            wc.DownloadFileAsync(new Uri(url), filename);
        }

        private void Complete(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                GZipStream gzs = new GZipStream(new FileStream(filename, FileMode.Open), CompressionMode.Decompress);
                FileStream c = new FileStream("catchdata", FileMode.Create);
                gzs.CopyTo(c);
                im.uiHelper.setStatusBarText_InThread(String.Format("catchdata已更新至版本{0}", im.serverHelper.getDataVersion()));
                im.configManager.SetConfig("dataversion", im.serverHelper.getDataVersion());
                c.Close();
                gzs.Close();
                File.Delete(filename);
                im.dataHelper.StartReadCatchData();
            }
            catch(Exception ex)
            {
                im.uiHelper.setStatusBarText_InThread(String.Format("更新失败!错误原因: {0}", ex.Message));
                im.listener.Shutdown();
            }
            
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            string message = String.Format("catchdata下载中..下载进度：{0}%, 请勿登录", e.ProgressPercentage);
            im.uiHelper.setStatusBarText_InThread(message);
        }
    }
}
