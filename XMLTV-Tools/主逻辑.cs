using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static XMLTV_Tools.下载文件;

namespace XMLTV_Tools
{
    public class 网络服务
    {
        public object 进度条 = null;
        public object 获取灯 = null;
        public object 文本框 = null;
        public List<string[]> 正则表 = new List<string[]>();
        public string 结果 = "尚未更新!";
        public 网络服务()
        {
        }
        private void 读缓存文件()
        {
            结果 = File.ReadAllText(Directory.GetCurrentDirectory() + "\\"+ "XMLTV.XML");
            for(int i = 0; i < 正则表.Count; i++)
            {
                Regex rep_regex = new Regex(正则表[i][0]);
                结果 = rep_regex.Replace(结果, 正则表[i][1]);
                //结果 = Regex.Replace(结果, 正则表[i][0], 正则表[i][1]);
            }
            打字("更新完毕!");
        }
        public void 更新指南(Boolean 强制更新,  string 地址)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + "XMLTV.XML") ||强制更新)
            {
                下载文件 下载器 = new 下载文件();
                下载器.状态 += 更新下载进度条;
                下载器.结束 += 更新输出;
                下载器.下载(地址, "XMLTV.XML");
                读缓存文件();
            }
            else
            {
                读缓存文件();
            }
        }
        private void 更新下载进度条(int 最小值, int 最大值, int 当前值, float 百分比)
        {
            System.Windows.Forms.ProgressBar progressBar1 = (System.Windows.Forms.ProgressBar)进度条;
            progressBar1.Maximum = 最大值;
            progressBar1.Value = (int)当前值;
        }

        private void 更新输出()
        {
            读缓存文件();
            System.Windows.Forms.Panel panel1 = (System.Windows.Forms.Panel)获取灯;
            panel1.BackColor = System.Drawing.Color.Green;
        }

        private void 打字(string 字)
        {
            System.Windows.Forms.TextBox textBox3 = (TextBox)文本框;
            textBox3.Text += "\n\r"+字;
        }
    }
    public class 主逻辑
    {
        private string 输入网址;
        private string 输出网址;
        private int 输出端口;
        网络服务 服务 = null;
        网络监听 监听 = null;
        public List<string[]> 正则表=new List<string[]>();
        public object 监听灯 = null;
        public object 获取灯 = null;
        public object 进度条 = null;
        public object 文本框 = null;
        public void 开启服务(string 用户输入网址, string 用户输出网址, int 用户输出端口, Boolean 强制启动 = false) {
            /// <summary>
            /// 开启整个服务, 在保存完毕设置或者软件开始后执行。
            /// </summary>
            输入网址 = 用户输入网址;
            输出网址 = 用户输出网址;
            输出端口 = 用户输出端口;
            启动服务(进度条, 获取灯, 文本框, 强制启动);
        }
        private void 启动服务(object 进度条, object 获取灯, object 文本框, Boolean 强制启动=false)
        {
            /// <summary>
            /// 启动软件。
            /// </summary>
            DialogResult 对话框 = MessageBox.Show("是否更新节目？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            服务 = new 网络服务();
            服务.进度条 = 进度条;
            服务.获取灯 = 获取灯;
            服务.文本框 = 文本框;
            服务.正则表 = 正则表;
            if (对话框 == DialogResult.OK||强制启动)
            {
                服务.更新指南( true,  输入网址);
            }
            服务.更新指南( false, 输入网址);
            监听 = new 网络监听();
            监听.变灯 += 变灯;
            监听.地址 = 输出网址;
            监听.端口 = 输出端口;
            监听.内容 = 服务.结果;
            监听.开关(true);
        }
        private void 变灯(Boolean 状态)
        {
            System.Windows.Forms.Panel panel1 = (System.Windows.Forms.Panel)监听灯;
            if(状态) panel1.BackColor = System.Drawing.Color.Green;
            else panel1.BackColor = System.Drawing.Color.Red;
        }
        public void 开关服务()
        {
            if (监听==null) return;
            监听.开关(false);
        }
    }
}
