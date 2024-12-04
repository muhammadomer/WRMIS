using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using System.Data;
using System.Net.Mail;
using PMIU.WRMIS.Model;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;


namespace PMIU.WRMIS.Common
{
    public static class Utility
    {
        /// <summary>
        /// This function return formatted date as MMM dd, yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Formatted date</returns>
        /// 
        public static string FormattedDate(DateTime date)
        {
            string dateFormat = "ddMMMyyyy";
            string _date = date.ToString(dateFormat);
            return _date;
        }

        public static string GetFormattedDate(DateTime date)
        {
            string dateFormat = ReadConfiguration("DateFormat");
            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = "dd-MMM-yyyy";

            string _date = date.ToString(dateFormat);

            return _date;
        }

        public static string GetFormattedDate(DateTime? ndate)
        {
            DateTime date;
            if (ndate == null)
                return null;
            else
                date = ndate.Value;

            string dateFormat = ReadConfiguration("DateFormat");
            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = "dd-MMM-yyyy";

            string _date = date.ToString(dateFormat);

            return _date;
        }

        /// <summary>
        /// This function return formatted time e.g 07:30 AM // 12 hour clock // hour is always 2 digits
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Formatted date</returns>
        public static string GetFormattedTime(DateTime date)
        {
            string timeFormat = ReadConfiguration("TimeFormat");
            if (string.IsNullOrEmpty(timeFormat))
                timeFormat = "hh:mm tt";

            string _time = date.ToString(timeFormat);

            return _time;
        }
        /// <summary>
        /// This function read application setting based on the passed in string        
        /// </summary>
        /// <param name="Key">The name of the key</param>
        /// <returns>The value of the app setting in the web.Config
        //     or String.Empty if no setting found</returns>
        public static string ReadConfiguration(string Key)
        {
            string value = string.Empty;
            if (ConfigurationManager.AppSettings.Count > 0 && ConfigurationManager.AppSettings[Key] != null)
            {
                value = ConfigurationManager.AppSettings[Key];
            }
            return value;
        }
        /// <summary>
        //     Converts the specified string representation of a date to its System.DateTime
        //     equivalent using the specified format and culture-specific format information.
        //     The format of the string representation must match the specified format exactly.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>An object that is equivalent to the date contained in date parameter, as specified
        //     by format and provider.</returns>
        public static DateTime GetParsedDate(string date)
        {
            string dateFormat = ReadConfiguration("DateFormat");
            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = "dd-MMM-yyyy";

            DateTime parsedDate = DateTime.ParseExact(date.Trim(), dateFormat, CultureInfo.InvariantCulture);
            return parsedDate;
        }

        public static string GetParsedTime(DateTime date)
        {
            string timeFormat = ReadConfiguration("TimeFormat");
            if (string.IsNullOrEmpty(timeFormat))
                timeFormat = "HH:mm:ss tt";

            string _time = date.ToString(timeFormat);

            return _time;
        }
        public static DateTime GetParsedDateTime(string _Date, string _Time)
        {
            DateTime date = GetParsedDate(_Date);
            DateTime time = Convert.ToDateTime(_Time);
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// This function return default image path in case image not exists
        /// Created on: 19-02-2016
        /// </summary>
        /// <param name="_Image"></param>
        /// <returns></returns>
        public static string IsImageExists(string _Image)
        {
            if (string.IsNullOrEmpty(_Image))
                _Image = "noimage.jpg";

            return _Image;
        }
        public static Constants.SessionOrShift GetSession(DateTime time)
        {
            Constants.SessionOrShift session = Constants.SessionOrShift.Morning;
            // 12:01 AM to 12:00 PM
            if (time.AddSeconds(0) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 1, 0) && time.AddSeconds(0) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0))
                session = Constants.SessionOrShift.Morning;
            // 12:01 PM to 12:00 AM
            else if (time.AddSeconds(0) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 1, 0) && time.AddSeconds(0) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0))
                session = Constants.SessionOrShift.Evening;

            return session;
        }
        public static string GetDynamicPropertyValue(dynamic _Object, string _PropertyName)
        {
            return Convert.ToString(_Object.GetType().GetProperty(_PropertyName).GetValue(_Object, null));
        }
        public static string GetImageURL(string _ModuleName, string _ImageName)
        {
            string URL = string.Empty;
            URL = @"/Handlers/GetFiles.ashx?fileName=" + _ImageName + "&moduleName=" + _ModuleName;
            return URL;
        }
        public static string GetImagePath(string _ModuleName)
        {
            return ReadConfiguration("BasePath") + _ModuleName;
        }
        public static string GetFormattedDateTime(DateTime _DateTime)
        {
            return GetFormattedDate(_DateTime) + " " + GetFormattedTime(_DateTime);
        }

        public static string GetFormattedDateTimeJavaScript(DateTime _DateTime)
        {
            return GetFormattedDate(_DateTime) + "'+'\\n'+'" + GetFormattedTime(_DateTime);
        }

        public static bool SendEmail(UA_EmailNotification mdlEmailNotification)
        {
            //long EmaiID = 0;
            try
            {
                List<string> lstTo = new List<string>();
                if (mdlEmailNotification.EmailTo != null)
                    lstTo = mdlEmailNotification.EmailTo.Split(';').ToList<string>();

                List<string> lstCC = new List<string>();
                if (mdlEmailNotification.EmailCC != null)
                    lstCC = mdlEmailNotification.EmailCC.Split(';').ToList<string>();

                List<MailAddress> lstMailAddressTo = new List<MailAddress>();
                List<MailAddress> lstMailAddressCC = new List<MailAddress>();
                foreach (string sTo in lstTo)
                {
                    if (sTo.Trim() == "")
                        continue;
                    lstMailAddressTo.Add(new MailAddress(sTo));
                }

                foreach (string sCC in lstCC)
                {
                    if (sCC.Trim() == "")
                        continue;
                    lstMailAddressCC.Add(new MailAddress(sCC));
                }

                if (lstMailAddressTo.Count <= 0)
                {
                    mdlEmailNotification.ServerMessage = "No email specified.";
                    mdlEmailNotification.Status = 1;
                    ++mdlEmailNotification.TryCount;
                    return true;
                }

                SendEmail(
                   lstMailAddressTo,
                   lstMailAddressCC,
                   mdlEmailNotification.EmailSubject,
                   mdlEmailNotification.EmailBody
                   );

                mdlEmailNotification.ServerMessage = "SENT";
                mdlEmailNotification.Status = 1;
                ++mdlEmailNotification.TryCount;
                return true;
            }
            catch (Exception exp)
            {
                mdlEmailNotification.ServerMessage = exp.Message;
                ++mdlEmailNotification.TryCount;
                return false;
            }


        }

        public static void SendEmail(List<MailAddress> _lstTo, List<MailAddress> _lstCC, string _Subject, string _Body)
        {
            SmtpClient oSmtpClient = new SmtpClient();
            MailMessage oMailMessage = new MailMessage();

            string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString();
            string FromAddress = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
            string FromName = System.Configuration.ConfigurationManager.AppSettings["FromName"].ToString();

            oSmtpClient.Host = SMTPServer;
            MailAddress From = new MailAddress(FromAddress, FromName);

            oMailMessage.From = From;

            foreach (MailAddress mailTo in _lstTo)
                oMailMessage.To.Add(mailTo);
            foreach (MailAddress mailCC in _lstCC)
                oMailMessage.CC.Add(mailCC);


            oMailMessage.Subject = _Subject;
            oMailMessage.Body = _Body;
            oMailMessage.IsBodyHtml = true;

            oSmtpClient.EnableSsl = true;

            oSmtpClient.Port = 587;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential("pmiu.wrmis@gmail.com", "pmiuwrmis@1234");
            oSmtpClient.Credentials = nc;

            oSmtpClient.Send(oMailMessage);


        }

        public static bool SendSMS(UA_SMSNotification mdlSMSNotification)
        {
            try
            {
                SendSMS(mdlSMSNotification.MobileNumber, mdlSMSNotification.SMSText);
                mdlSMSNotification.ServerMessage = "SENT";
                mdlSMSNotification.Status = 1;
                mdlSMSNotification.SMSSentDate = System.DateTime.Now;
                ++mdlSMSNotification.TryCount;
                return true;
            }
            catch (Exception exp)
            {
                mdlSMSNotification.ServerMessage = exp.Message;
                ++mdlSMSNotification.TryCount;
                return false;
            }
        }

        public static void SendSMS(string _To, string _SMSMessage)
        {
            string Response = "";

            _To = AppendCountryCode(_To);

            if (_To.Length != 12)
            {
                Response = "Mobile number is not in correct format.";
                throw new Exception(Response);
            }

            string SMSURI = System.Configuration.ConfigurationManager.AppSettings["SMSURI"].ToString();
            string Username = System.Configuration.ConfigurationManager.AppSettings["Username"].ToString();
            string Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
            string ClientID = System.Configuration.ConfigurationManager.AppSettings["ClientID"].ToString();
            string Mask = System.Configuration.ConfigurationManager.AppSettings["Mask"].ToString();
            string Language = System.Configuration.ConfigurationManager.AppSettings["Language"].ToString();


            string Parameters = "";

            Parameters = string.Format("username={0}&password={1}&clientid={2}&mask={3}&msg={4}&to={5}&language={6}",
                                                                        Username,
                                                                        Password,
                                                                        ClientID,
                                                                        Mask,
                                                                        _SMSMessage,
                                                                        _To,
                                                                        Language);
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Response = wc.UploadString(SMSURI, Parameters);
            }

            if (Response.ToUpper().Contains("SENT SUCCESSFULLY"))
            {

            }
            else
            {
                throw new Exception(Response);
            }
        }

        public static string AppendCountryCode(string _MobileNumber)
        {
            if (_MobileNumber.StartsWith("92") && _MobileNumber.Length == 12)
                return _MobileNumber;
            return "92" + _MobileNumber.Substring(1);
        }

        public static string GetStringValueFromQueryString(string _KeyName, string _DefaultVal)
        {
            string ReturnVal = "";
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.QueryString != null
              && HttpContext.Current.Request.QueryString.Count > 0 && HttpContext.Current.Request.QueryString[_KeyName] != null
              && HttpContext.Current.Request.QueryString[_KeyName] != "")
            {
                ReturnVal = HttpContext.Current.Request.QueryString[_KeyName];

                if (ReturnVal == "" && _DefaultVal != "")
                    ReturnVal = _DefaultVal;
            }
            else
            {
                ReturnVal = "";
            }

            return ReturnVal;
        }

        public static int GetNumericValueFromQueryString(string _KeyName, int _DefaultVal)
        {
            int ReturnVal = 0;
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.QueryString != null
              && HttpContext.Current.Request.QueryString.Count > 0 && HttpContext.Current.Request.QueryString[_KeyName] != null
              && IsNumeric(HttpContext.Current.Request.QueryString[_KeyName]))
            {
                ReturnVal = int.Parse(HttpContext.Current.Request.QueryString[_KeyName]);
            }

            if (ReturnVal == 0 && _DefaultVal > 0)
                ReturnVal = _DefaultVal;

            return ReturnVal;
        }

        public static double GetDoubleValueFromQueryString(string _KeyName, double _DefaultVal)
        {
            double ReturnVal = 0;
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.QueryString != null
              && HttpContext.Current.Request.QueryString.Count > 0 && HttpContext.Current.Request.QueryString[_KeyName] != null
              && IsDouble(HttpContext.Current.Request.QueryString[_KeyName]))
            {
                ReturnVal = double.Parse(HttpContext.Current.Request.QueryString[_KeyName]);
            }

            if (ReturnVal == 0 && _DefaultVal > 0)
                ReturnVal = _DefaultVal;

            return ReturnVal;
        }

        public static string GetUserIPAddress()
        {
            if (HttpContext.Current != null)
            {
                HttpContext objContext = HttpContext.Current;

                if (objContext.Request != null)
                {

                    string ipList = objContext.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];

                    if (!string.IsNullOrEmpty(ipList))
                    {
                        return ipList;
                    }

                    ipList = objContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (!string.IsNullOrEmpty(ipList))
                    {
                        return ipList.Split(',')[0];
                    }

                    ipList = objContext.Request.ServerVariables["REMOTE_ADDR"];
                    return ipList;
                }
            }

            return "";

        }

        public static string GetUserAgent()
        {

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    return HttpContext.Current.Request.UserAgent.ToString();
                }
            }

            return "";

        }

        public static bool IsNumeric(this string _Input)
        {
            int output;
            return int.TryParse(_Input, out output);
        }

        public static bool IsDouble(this string _Input)
        {
            double output;
            return double.TryParse(_Input, out output);
        }

        public static string GetTDailyShortName(short? _TDailyID, int _Season)
        {
            string ShortName = "";
            if (_Season == (int)Constants.Seasons.Kharif)
            {
                if (_TDailyID == 1)
                    ShortName = "Apr1";
                else if (_TDailyID == 2)
                    ShortName = "Apr2";
                else if (_TDailyID == 3)
                    ShortName = "Apr3";
                else if (_TDailyID == 4)
                    ShortName = "May1";
                else if (_TDailyID == 5)
                    ShortName = "May2";
                else if (_TDailyID == 6)
                    ShortName = "May3";
                else if (_TDailyID == 7)
                    ShortName = "Jun1";
                else if (_TDailyID == 8)
                    ShortName = "Jun2";
                else if (_TDailyID == 9)
                    ShortName = "Jun3";
                else if (_TDailyID == 10)
                    ShortName = "Jul1";
                else if (_TDailyID == 11)
                    ShortName = "Jul2";
                else if (_TDailyID == 12)
                    ShortName = "Jul3";
                else if (_TDailyID == 13)
                    ShortName = "Aug1";
                else if (_TDailyID == 14)
                    ShortName = "Aug2";
                else if (_TDailyID == 15)
                    ShortName = "Aug3";
                else if (_TDailyID == 16)
                    ShortName = "Sep1";
                else if (_TDailyID == 17)
                    ShortName = "Sep2";
                else if (_TDailyID == 18)
                    ShortName = "Sep3";
            }
            else
            {
                if (_TDailyID == 19)
                    ShortName = "Oct1";
                else if (_TDailyID == 20)
                    ShortName = "Oct2";
                else if (_TDailyID == 21)
                    ShortName = "Oct3";
                else if (_TDailyID == 22)
                    ShortName = "Nov1";
                else if (_TDailyID == 23)
                    ShortName = "Nov2";
                else if (_TDailyID == 24)
                    ShortName = "Nov3";
                else if (_TDailyID == 25)
                    ShortName = "Dec1";
                else if (_TDailyID == 26)
                    ShortName = "Dec2";
                else if (_TDailyID == 27)
                    ShortName = "Dec3";
                else if (_TDailyID == 28)
                    ShortName = "Jan1";
                else if (_TDailyID == 29)
                    ShortName = "Jan2";
                else if (_TDailyID == 30)
                    ShortName = "Jan3";
                else if (_TDailyID == 31)
                    ShortName = "Feb1";
                else if (_TDailyID == 32)
                    ShortName = "Feb2";
                else if (_TDailyID == 33)
                    ShortName = "Feb3";
                else if (_TDailyID == 34)
                    ShortName = "Mar1";
                else if (_TDailyID == 35)
                    ShortName = "Mar2";
                else if (_TDailyID == 36)
                    ShortName = "Mar3";
            }
            return ShortName;
        }

        public static int GetCurrentSeasonForView()
        {
            int Season = 1;
            if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
                 || ((DateTime.Now.Month >= (int)Constants.PlanningMonthsAndDays.KharifPlanningApril) && (DateTime.Now.Month < (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember)) ||
                (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay))
                Season = (int)Constants.Seasons.Kharif;

            return Season;
        }
        public static String GetRoundOffValue(string value)
        {
            if (value == null)
                return string.Empty;
            else if (value == string.Empty || value.Equals("0"))
                return "0.00";
            else
            {
                string no = Convert.ToDecimal(value).ToString("###,###,###.##");

                if (no.Contains("."))
                {
                    //if (no.Substring(no.IndexOf(".") + 1).Length >= 2)
                    //    return no;

                    if (no.IndexOf(".") == 0)
                        no = "0" + no;

                    if (no.Substring(no.IndexOf(".") + 1).Length == 1)
                        no = no + "0";

                    return no;
                }
                else
                    return no + ".00";
            }
            //return "0.00";
        }

        public static String GetRoundOffValue(double? value)
        {
            if (value == null)
                return value.ToString();
            else
            {
                string no = GetRoundOffValue(value.ToString());
                return no;

                //return Convert.ToDecimal(value).ToString("###,###,###.##");
            }
        }

        public static String GetRoundOffValueAccounts(double? value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else if (value == 0)
            {
                return "0";
            }
            else
            {
                return Convert.ToDecimal(value).ToString("###,###,###.##");
            }
        }
        public static String GetRoundOffValueOneDecimal(double? value)
        {
            if (value == null || value == 0)
                return value.ToString();
            else
            {
                string str = "";
                if (value.ToString().Contains("."))
                {

                    if (value < 1)
                        str = Convert.ToDecimal(value).ToString("0.#");
                    else
                        str = Convert.ToDecimal(value).ToString("###,###,###.#");
                }
                else
                    str = Convert.ToDecimal(value).ToString("###,###,###.0");

                if (!str.Contains("0.") && !str.Contains("-"))
                {
                    if (Convert.ToDouble(str) < 1)
                        str = "0" + str;
                }

                if (!str.Contains("."))
                    str = str + ".0";
                return str;
            }

        }

        public static bool PlanningDaysLimit()
        {
            bool Result = true;

            long Day = DateTime.Now.Day;
            long Month = DateTime.Now.Month;
            if ((Month > (int)Constants.PlanningMonthsAndDays.KharifPlanningApril && Month < (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember)
                || (Month > (int)Constants.PlanningMonthsAndDays.RabiPlanningOctober
                || Month < (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch)
                || (Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && Day < (int)Constants.PlanningMonthsAndDays.PlanningDay)
                || (Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember && Day < (int)Constants.PlanningMonthsAndDays.PlanningDay)
                )
                Result = false;

            return Result;
        }
        public static string GetTimeFormatted(DateTime? ndate)
        {
            DateTime date;
            if (ndate == null)
                return null;
            else
                date = ndate.Value;

            string dateFormat = ReadConfiguration("TimeFormat");
            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = "hh:mm tt";

            string _time = date.ToString(dateFormat);



            return _time;
        }

        public static List<dynamic> GetGroups(long _NoOfGroups)
        {
            List<dynamic> lstGroups = new List<dynamic>();

            if (_NoOfGroups == (int)Constants.NoOfGroups.Five)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"},
                new { ID="2", Name="B"},
                new { ID="3", Name="C"},
                new { ID="4", Name="D"},
                new { ID="5", Name="E"}
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Four)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"},
                new { ID="2", Name="B"},
                new { ID="3", Name="C"},
                new { ID="4", Name="D"}
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Three)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"},
                new { ID="2", Name="B"},
                new { ID="3", Name="C"}                
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Two)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"},
                new { ID="2", Name="B"}                                
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.One)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"}                          
                };
                return lstGroups;
            }

            return lstGroups;
        }


        public static List<dynamic> GetGroupName(long _NoOfGroups)
        {
            List<dynamic> lstGroups = new List<dynamic>();

            if (_NoOfGroups == (int)Constants.NoOfGroups.Five)
            {
                lstGroups = new List<dynamic>
                {                
                new { ID="5", Name="E"}
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Four)
            {
                lstGroups = new List<dynamic>
                {                
                new { ID="4", Name="D"}
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Three)
            {
                lstGroups = new List<dynamic>
                {                
                new { ID="3", Name="C"}                
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.Two)
            {
                lstGroups = new List<dynamic>
                {                
                new { ID="2", Name="B"}                                
                };
                return lstGroups;
            }
            else if (_NoOfGroups == (int)Constants.NoOfGroups.One)
            {
                lstGroups = new List<dynamic>
                {
                new { ID="1", Name="A"}                          
                };
                return lstGroups;
            }

            return lstGroups;
        }


        public static string GetNextGroupName(string _CurGroupName)
        {
            string Nextgroup = "";

            if (_CurGroupName == "")
                Nextgroup = "A";
            else if (_CurGroupName == "A")
                Nextgroup = "B";
            else if (_CurGroupName == "B")
                Nextgroup = "C";
            else if (_CurGroupName == "C")
                Nextgroup = "D";
            else if (_CurGroupName == "D")
                Nextgroup = "E";

            return Nextgroup;
        }

        public static List<dynamic> GetSubGroups()
        {
            List<dynamic> lstSubGroups = new List<dynamic>();

            lstSubGroups = new List<dynamic>
            {
                new { ID="6", Name="Sub Group 1"},
                new { ID="7", Name="Sub Group 2"},
                new { ID="8", Name="Sub Group 3"}
            };
            return lstSubGroups;
        }

        public static List<dynamic> GetPriority()
        {
            List<dynamic> lstPriority = new List<dynamic>();

            lstPriority = new List<dynamic>
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="LR"},
                new { ID="2", Name="RL"}                
            };
            return lstPriority;
        }

        public static List<dynamic> GetAllSubGroups()
        {
            List<dynamic> lstSubGroups = new List<dynamic>();

            lstSubGroups = new List<dynamic>
            {
                new { ID="1", Name="A1"},
                new { ID="2", Name="A2"},
                new { ID="3", Name="A3"},

                new { ID="4", Name="B1"},
                new { ID="5", Name="B2"},
                new { ID="6", Name="B3"},

                new { ID="7", Name="C1"},
                new { ID="8", Name="C2"},
                new { ID="9", Name="C3"},

                new { ID="10", Name="D1"},
                new { ID="11", Name="D2"},
                new { ID="12", Name="D3"},

                new { ID="13", Name="E1"},
                new { ID="14", Name="E2"},
                new { ID="15", Name="E3"}
            };
            return lstSubGroups;
        }

        public static List<dynamic> GetSubGroupsOfGroup(long _NoOfGroup, long _NoOfSubgroup)
        {
            List<dynamic> lstSubGroups = new List<dynamic>();

            if (_NoOfGroup == (int)Constants.NoOfGroups.One)
            {
                if (_NoOfSubgroup == 2)
                {
                    lstSubGroups = new List<dynamic>
                    {
                    new { ID="1", Name="A1"},
                    new { ID="2", Name="A2"}                    
                    };
                }
                else
                {
                    lstSubGroups = new List<dynamic>
                    {
                    new { ID="1", Name="A1"},
                    new { ID="2", Name="A2"},
                    new { ID="3", Name="A3"}            
                    };
                }
            }
            else if (_NoOfGroup == (int)Constants.NoOfGroups.Two)
            {
                if (_NoOfSubgroup == 2)
                {
                    lstSubGroups = new List<dynamic>
                    {                
                    new { ID="4", Name="B1"},
                    new { ID="5", Name="B2"}                    
                    };
                }
                else
                {
                    lstSubGroups = new List<dynamic>
                    {                
                    new { ID="4", Name="B1"},
                    new { ID="5", Name="B2"},
                    new { ID="6", Name="B3"}
                    };
                }
            }
            else if (_NoOfGroup == (int)Constants.NoOfGroups.Three)
            {
                if (_NoOfSubgroup == 2)
                {
                    lstSubGroups = new List<dynamic>
                    {
                    new { ID="7", Name="C1"},
                    new { ID="8", Name="C2"}                    
                    };
                }
                else
                {
                    lstSubGroups = new List<dynamic>
                    {
                    new { ID="7", Name="C1"},
                    new { ID="8", Name="C2"},
                    new { ID="9", Name="C3"}
                    };
                }
            }
            else if (_NoOfGroup == (int)Constants.NoOfGroups.Four)
            {
                if (_NoOfSubgroup == 2)
                {
                    lstSubGroups = new List<dynamic>
                    {                
                    new { ID="10", Name="D1"},
                    new { ID="11", Name="D2"}                    
                    };
                }
                else
                {
                    lstSubGroups = new List<dynamic>
                {                
                    new { ID="10", Name="D1"},
                    new { ID="11", Name="D2"},
                    new { ID="12", Name="D3"}
                };
                }
            }
            else if (_NoOfGroup == (int)Constants.NoOfGroups.Five)
            {
                if (_NoOfSubgroup == 2)
                {
                    lstSubGroups = new List<dynamic>
                    {                
                    new { ID="13", Name="E1"},
                    new { ID="14", Name="E2"}                  
                    };
                }
                else
                {
                    lstSubGroups = new List<dynamic>
                    {                
                    new { ID="13", Name="E1"},
                    new { ID="14", Name="E2"},
                    new { ID="15", Name="E3"}
                    };
                }
            }

            return lstSubGroups;
        }

        public static List<ListItem> GetFinancialYear()
        {
            List<ListItem> lstYears = new List<ListItem>();

            DateTime Now = DateTime.Now;

            if (Now.Month <= 6)
            {
                for (int Year = Now.Year; Year > Now.Year - 15; Year--)
                {
                    DateTime CurrentYear = new DateTime(Year, 1, 1);

                    string FinancialYear = string.Format("{0}-{1}", Year - 1, CurrentYear.ToString("yy"));

                    lstYears.Add(new ListItem
                    {
                        Text = FinancialYear,
                        Value = FinancialYear
                    });
                }
            }
            else
            {
                for (int Year = Now.Year; Year > Now.Year - 15; Year--)
                {
                    DateTime NextYear = new DateTime(Year + 1, 1, 1);

                    string FinancialYear = string.Format("{0}-{1}", Year, NextYear.ToString("yy"));

                    lstYears.Add(new ListItem
                    {
                        Text = FinancialYear,
                        Value = FinancialYear
                    });
                }
            }
            return lstYears;
        }

        /// <summary>
        /// This function converts the Cusecs value to MAF
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_LstTDailyCusecs"></param>
        /// <param name="_Year"></param>
        /// <returns>double</returns>
        public static double GetMAF(List<dynamic> _LstTDailyCusecs, int _Year)
        {
            double TotalCusecs = 0;

            foreach (dynamic TDailyCusecs in _LstTDailyCusecs)
            {
                int TDailyID = Convert.ToInt32(GetDynamicPropertyValue(TDailyCusecs, "TDailyID"));
                string ShortName = GetDynamicPropertyValue(TDailyCusecs, "ShortName");
                double Cusecs = Convert.ToDouble(GetDynamicPropertyValue(TDailyCusecs, "Cusecs"));

                string MonthName = ShortName.Remove(ShortName.Length - 1);
                int Month = DateTime.Parse(string.Format("{0} {1},{2}", 1, MonthName, _Year)).Month;

                if (TDailyID % 3 == 0)
                {
                    int DaysInMonth = DateTime.DaysInMonth(_Year, Month);

                    if (DaysInMonth == 31)
                    {
                        TotalCusecs = TotalCusecs + (Cusecs * Constants.TDailyConversion);
                    }
                    else if (DaysInMonth == 30)
                    {
                        TotalCusecs = TotalCusecs + Cusecs;
                    }
                    else if (DaysInMonth == 29)
                    {
                        TotalCusecs = TotalCusecs + (Cusecs * Constants.LeapYearTrue);
                    }
                    else if (DaysInMonth == 28)
                    {
                        TotalCusecs = TotalCusecs + (Cusecs * Constants.LeapYearFalse);
                    }
                }
                else
                {
                    TotalCusecs = TotalCusecs + Cusecs;
                }
            }

            return TotalCusecs * Constants.MAFConversion;
        }

        public static List<dynamic> GetEffluentSreviceType()
        {
            List<dynamic> lstSreviceType = new List<dynamic>();

            lstSreviceType = new List<dynamic>
            {
                new { ID="1", Name="Effluent Water"},
                new { ID="2", Name="Canal Special Water"}                
            };
            return lstSreviceType;
        }

        public static List<dynamic> GetTaxforType()
        {
            List<dynamic> lstSreviceType = new List<dynamic>();

            lstSreviceType = new List<dynamic>
            {
                new { ID="1", Name="Repair"},
                new { ID="2", Name="New Purchase"}                
            };
            return lstSreviceType;
        }

        public static List<dynamic> GetTaxOnType()
        {
            List<dynamic> lstSreviceType = new List<dynamic>();

            lstSreviceType = new List<dynamic>
            {
                new { ID="1", Name="PMIU Staff"},
                new { ID="2", Name="Field Staff"}                
            };
            return lstSreviceType;
        }

        public static List<dynamic> GetSanctionOnType()
        {
            List<dynamic> lstSanctionType = new List<dynamic>();

            lstSanctionType = new List<dynamic>
            {
                new { ID="1", Name="4 Wheels Vehicle"},
                new { ID="2", Name="2 Wheels Vehicle"}                
            };
            return lstSanctionType;
        }

        public static string RemoveComma(string amount)
        {

            string[] amoutpart = amount.Split('.');
            string amp2 = amoutpart[0];
            string[] amountPart2 = amp2.Split(',');
            string finalAmoutn = "";
            foreach (var item in amountPart2)
            {
                finalAmoutn = finalAmoutn + item;
            }


            return finalAmoutn;

        }

        public static string GetCurrentFinancialYear()
        {

            DateTime Now = DateTime.Now;
            string FinancialYear = "";
            if (Now.Month <= 6)
            {
                FinancialYear = string.Format("{0}-{1}", Now.Year - 1, Now.ToString("yy"));
            }
            else
            {
                FinancialYear = string.Format("{0}-{1}", Now.Year, (Now.Year + 1).ToString().Substring(2));
            }
            return FinancialYear;
        }
    }
}
