using PMIU.WRMIS.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PMIU.WRMIS.Services.HelperClasses
{
    public class ServiceHelper
    {
        public const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        public const string MSG_NO_RECORD_FOUND = "No Records Found";
        public const string MSG_RECORD_SAVED = "Record saved successfully";
        public const string MSG_SUCCESS = "SUCCESS";
        public const string MSG_RECORD_NOT_DELETED = "Record cannot be deleted.";
        public const string MSG_RECORD_DELETED = "Record deleted successfully.";

        public const string WT_MSG_NotWithinUserJurisdiction = "Invalid RD range";
        public const string WT_MSG_NoSBEAvailable = "No SBE available.Contact User Administrator";
        public const string WT_MSG_CaseExists = "A Case with the same parameters has already been logged in the system";


        /// <summary>
        /// Private method create the ServiceResponse with provided 
        /// list data code and message
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_StatusCode"></param>
        /// <param name="_StatusMessage"></param>
        /// <returns>ServiceResponse</returns>
        public ServiceResponse CreateResponse<TSource>(List<TSource> _LstData, int _StatusCode, string _StatusMessage)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            svcResponse.StatusCode = _StatusCode;
            svcResponse.StatusMessage = _StatusMessage;
            svcResponse.Data = _LstData;
            return svcResponse;
        }

        /// <summary>
        ///  Service response object with provided data
        ///  Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_StatusCode"></param>
        /// <param name="_StatusMessage"></param>
        /// <param name="_Data"></param>
        /// <returns></returns>
        public ServiceResponse CreateResponse(int _StatusCode, string _StatusMessage, Object _Data)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            svcResponse.StatusCode = _StatusCode;
            svcResponse.StatusMessage = _StatusMessage;
            svcResponse.Data = _Data;
            return svcResponse;
        }

        /// <summary>
        /// Tells:is there is data in dictionary object or not
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_Dictionary"></param>
        /// <returns></returns>
        public bool IsDictionaryEmptyOrNull(Dictionary<string, object> _Dictionary)
        {
            if (_Dictionary == null || _Dictionary.Count == 0)
                return true;
            return false;
        }

        public void LogNow_WebServices(string _MethodName, string _Message)
        {
            string LoggerDirPath = System.AppDomain.CurrentDomain.BaseDirectory + "Logging";

            if (!System.IO.Directory.Exists(LoggerDirPath))
                System.IO.Directory.CreateDirectory(LoggerDirPath);

            DateTime dtInstance = DateTime.Now;
            string LogFileName = "WRMIS_WebServices_" + dtInstance.Year.ToString("0000") + dtInstance.Month.ToString("00") + dtInstance.Day.ToString("00") + ".txt";
            string LogFilePath;
            LogFilePath = LoggerDirPath + "\\" + LogFileName;

            TextWriter twLogFile = new StreamWriter(LogFilePath, true);
            twLogFile.WriteLine(DateTime.Now + ": \t "+_MethodName + "\t" + _Message);
            twLogFile.Close();
        }
        public ServiceResponse CreateResponseMultipleData<TSource>(List<TSource> _LstData,object _Data, int _StatusCode, string _StatusMessage)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            svcResponse.StatusCode = _StatusCode;
            svcResponse.StatusMessage = _StatusMessage;
            svcResponse.Data = _LstData;
            svcResponse.Data1 = _Data;
            return svcResponse;
        }
    }
}