using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QLNhiemVu
{
    class Log
    {
        public static void write(Exception ex)
        {
            string filePath = @"log\ERROR_" + DateTime.Now.ToString("yyyyMMdd") +".log";

            StreamWriter writer = new StreamWriter(filePath , true);            
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                writer.Close();
            
        }

        public static void debug(string str_val)
        {
            string filePath = @"log\Debug_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(str_val);
            writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            writer.Close();
        }
    }
}
