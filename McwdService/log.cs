using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace McwdService
{
    public class log
    {
        public static void writelog(string e2)
        {
            string filename = "测温数据上传日志";
            string files = @"测温数据上传日志\";
            string txtname = "数据上传" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            FileInfo file = new FileInfo("d:\\"+ files + txtname);    //看电脑路径
            StreamWriter sw = null;
            if (!Directory.Exists("d:\\" + filename))
            {
                Directory.CreateDirectory("d:\\" + filename);
                if (!file.Exists)
                {
                    sw = file.CreateText();
                    sw.WriteLine("============================================================================");
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                    sw.WriteLine(e2.ToString());
                }
                else
                {
                    sw = file.AppendText();
                    sw.WriteLine("============================================================================");
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                    sw.WriteLine(e2.ToString());

                }
            }
            else
            {
                if (!file.Exists)
                {
                    sw = file.CreateText();
                    sw.WriteLine("============================================================================");
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                    sw.WriteLine(e2.ToString());
                }
                else
                {
                    sw = file.AppendText();
                    sw.WriteLine("============================================================================");
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                    sw.WriteLine(e2.ToString());

                }
            }
            sw.Close();

        }
    }
}
