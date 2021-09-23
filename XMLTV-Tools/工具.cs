
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace XMLTV_Tools
{
    /***
    *	Title："基础工具" 项目
    *		主题：文件下载
    *	Description：
    *		功能：
    *		    1、HTTP方式下载文件（可获取文件的下载进度、下载结束、下载错误）
    *	Date：2021
    *	Version：0.1版本
    *	Author：Coffee
    *	    Link: https://blog.csdn.net/xiaochenxihua/article/details/118827818
    *	Modify Recoder：
    */
    public class 下载文件
    {
        #region   基础参数
        //定义一个下载状态的委托
        public delegate void 下载状态(int 最小值, int 最大值, int 当前值, float 百分比);
        //声明下载状态委托
        public event 下载状态 状态;

        //定义一个下载结束的委托
        public delegate void 下载结束();
        //声明下载结束
        public event 下载结束 结束;
        /*
        //定义一个异常信息的委托
        public delegate void 下载错误(string error);
        //声明异常信息委托
        public event 下载错误 错误;*/

        #endregion



        #region   公有方法

        /// <summary>
        /// HTTP方式下载文件
        /// </summary>
        /// <param name="URL">下载文件的URL</param>
        /// <param name="filePathAndName">文件保存的路径和名称（比如：c:\software\update.exe）</param>
        public void 下载(string URL, string filePathAndName)
        {
            if (string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(filePathAndName)) return;

                filePathAndName=Directory.GetCurrentDirectory()+ "\\" +filePathAndName;
                
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var totalBytes = httpWebResponse.ContentLength;
                var stream = httpWebResponse.GetResponseStream();
                using (FileStream fs = new FileStream(filePathAndName, FileMode.Create))
                {
                    long totalDownloadedByte = 0;
                    var by = new byte[2048];
                    if (stream != null)
                    {
                        var osize = stream.Read(by, 0, by.Length);
                        while (osize > 0)
                        {
                            totalDownloadedByte = osize + totalDownloadedByte;
                            fs.Write(by, 0, osize);
                            osize = stream.Read(by, 0, by.Length);
                            var percent = totalDownloadedByte / (float)totalBytes * 100;
                            状态?.Invoke(0, (int)totalBytes, (int)totalDownloadedByte, percent);
                        }
                    }
                }
                结束?.Invoke();
            
        }

        #endregion


    }//Class_end
    public class 网络监听
    {
        public string 地址 = "";
        public int 端口 = 0;
        public string 内容 = "错误!!";
        private Boolean 状态 = false;
        private HttpListener 监听器 = null;
        private Boolean 线程红绿灯 = false;
        public delegate void 网络变灯(Boolean 状态);
        public event 网络变灯 变灯;
        public void 开关(Boolean 强制)
        {
            if (状态)
            {
                关闭();
                if (强制)
                {
                    开启();
                }
            }
            else
            {
                开启();
            }
        }
        private void 开启()
        {
            监听器 = new HttpListener
            {
                AuthenticationSchemes = AuthenticationSchemes.Anonymous
            };
            try
            {
                监听器.Prefixes.Add("http://" + 地址 + ":" + 端口 + "/");
                监听器.Start();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                变灯?.Invoke(false);
                线程红绿灯 = true;
                return;
            }
            线程红绿灯 = false;
            状态 = true;
            变灯?.Invoke(true);
            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    if (线程红绿灯)
                    {
                        Console.WriteLine("关闭");
                        状态 = false;
                        监听器.Close();
                        System.Threading.Thread.CurrentThread.Abort();
                    }
                    HttpListenerContext httpListenerContext = 监听器.GetContext();
                    httpListenerContext.Response.StatusCode = 200;
                    using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream))
                    {
                        writer.Write(内容);
                    }

                }
            })).Start();
            变灯?.Invoke(true);
        }
        private void 关闭()
        {
            变灯?.Invoke(false) ;
            线程红绿灯 = true;
        }
    }
}