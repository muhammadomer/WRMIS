using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using System.IO;


namespace PMIU.WRMIS.Logging
{
    public class LogMessage
    {
        public LogMessage()
        {

        }

        public static void LogMessageNow(long _UserID, string _Message, Constants.MessageType _MessageType = Constants.MessageType.Info, Constants.MessageCategory _MessageCategory = Constants.MessageCategory.WebApp, string _FileInstance = "")
        {
            string LoggerDirPath = System.AppDomain.CurrentDomain.BaseDirectory + "Logging";

          

            if (!System.IO.Directory.Exists(LoggerDirPath))
                System.IO.Directory.CreateDirectory(LoggerDirPath);

            DateTime dtInstance = DateTime.Now;
            string LogFileName = "WRMIS_" + dtInstance.Year.ToString("0000") + dtInstance.Month.ToString("00") + dtInstance.Day.ToString("00") + ((_FileInstance.Length > 0)? "_" : "") + _FileInstance + ".txt";
            string LogFilePath;
            LogFilePath = LoggerDirPath + "\\" + LogFileName;

            TextWriter twLogFile = new StreamWriter(LogFilePath, true);
            twLogFile.WriteLine(DateTime.Now.ToString() + "\t\t" + _UserID + "\t" + _MessageCategory + "\t" + _MessageType + "\t" + _Message);
            twLogFile.Close();

            //try
            //{
                

            //}
            //catch (Exception e)
            //{

            //}
        }

    }
}
