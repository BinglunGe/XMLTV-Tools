using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTV_Tools
{
    class INI
    {
        public Hashtable 分析结果=new Hashtable();
        private readonly string 文件内容;
        private string 读文件(string 名字)
        {
            string 结果 = File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + 名字);
            return 结果;
        }
        public INI(string 文件名)
        {
            文件内容 = 读文件(文件名);
        }
        public void 分析()
        {
            分析节组();
        }
        public void 分析节组()
        {
            string[] 行数组 = 文件内容.Split('\n');
            List<int> 行数列表 = new List<int>();
            int i = 0, j = 0, k;
            foreach (string 行 in 行数组)
            {
                i++;
                if (行[0] == '[')
                {
                    行数列表.Add(i);
                    j++;
                }
            }
            行数列表.Add(i + 1);
            for (k = 0; k < j; k++)
            {
                分析键值对组(行数组[行数列表[k]-1], ref 行数组, 行数列表[k], 行数列表[k + 1] - 1);
            }

            Console.WriteLine(分析结果);

        }
        private void 分析键值对组(string 节, ref string[] 行数组, int 起, int 终)
        {
            节 = 节.Substring(0, 节.Length - 2).Substring(1);
            分析结果.Add(节, new Hashtable());
            for (int i = 起; i < 终; i++)
            {
                string 行 = 行数组[i];
                string[] 键值对 = 行.Split(new char[] { '=' }, 2);
                string 键 = 键值对[0];
                string 值 = 键值对[1];
                Console.Write(键 + "\t");
                Console.WriteLine(值);
                ((Hashtable)分析结果[节]).Add(键, 值);
            }   
        }
        public string 获取(string 节, string 键)
        {
            string 结果=(string)(((Hashtable)分析结果[节])[键]);
            if (结果[结果.Length-1] == '\r') 结果 = 结果.Substring(0, 结果.Length - 1);
            return 结果;
        }
    }
}
