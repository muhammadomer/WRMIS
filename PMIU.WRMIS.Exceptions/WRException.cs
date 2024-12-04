using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Logging;

namespace PMIU.WRMIS.Exceptions
{
    public class WRException : Exception 
    {
        private long UserID;
        private Exception AppException;
        private string FileName;

        public WRException(long UserID, Exception Exp, string FileName = "WRMIS")
            : base(Exp.Message, Exp)
        {
            this.UserID = UserID;
            this.AppException = Exp;
            this.FileName = FileName;
        }

        public void LogException(Constants.MessageCategory _Category = Constants.MessageCategory.WebApp)
        {
            string ExceptionDetails = GetExceptionDetails(this.AppException);
            LogMessage.LogMessageNow(this.UserID, ExceptionDetails, Constants.MessageType.Error, _Category);
        }

        public string GetExceptionDetails(Exception e)
        {
            string eMessage = "";
            eMessage += "Exp: " + e.Message + "\t\t";
            if (e.InnerException == null)
                return eMessage;
            else
                return GetExceptionDetails(e.InnerException);

        }
    }
}
