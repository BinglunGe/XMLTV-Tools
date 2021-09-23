using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLTV_Tools
{
    public partial class Form1 : Form
    {
        readonly 主逻辑 服务;
        public Form1()
        {
            InitializeComponent();
            服务 = new 主逻辑();
            INI INI分析器 = new INI("TEST.INI");
            INI分析器.分析();
            textBox1.Text = INI分析器.获取("设置", "接口网址");
            textBox2.Text = INI分析器.获取("设置", "输出网址");
            numericUpDown2.Value = Convert.ToInt32(INI分析器.获取("设置", "输出端口"));
            numericUpDown1.Value = Convert.ToInt32(INI分析器.获取("设置", "更新间隔"));
            timer1.Interval = ((int)numericUpDown1.Value) * 60 * 1000;
            处理正则(INI分析器);
        }
        private void 处理正则(INI 读配置)
        {
            int 条数 = Convert.ToInt32(读配置.获取("正则", "0"));
            for (int i = 1; i <= 条数; i++)
            {
                服务.正则表.Add(new string[]{
                    读配置.获取("正则", i.ToString()+".0"),
                    读配置.获取("正则", i.ToString()+".1")
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String 输入网址, 输出网址;
            输入网址 = textBox1.Text;
            输出网址 = textBox2.Text;
            int 输出端口;
            输出端口 = (int)numericUpDown2.Value;
            服务.监听灯 = this.panel1;
            服务.获取灯 = this.panel2;
            服务.进度条 = this.progressBar1;
            服务.文本框 = this.textBox3;
            服务.开启服务( 输入网址, 输出网址, 输出端口);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("已清空");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            服务.开关服务();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            String 输入网址, 输出网址;
            输入网址 = textBox1.Text;
            输出网址 = textBox2.Text;
            int 输出端口;
            输出端口 = (int)numericUpDown2.Value;
            服务.监听灯 = this.panel1;
            服务.获取灯 = this.panel2;
            服务.进度条 = this.progressBar1;
            服务.文本框 = this.textBox3;
            服务.开启服务(输入网址, 输出网址, 输出端口, true);
        }
    }
}
