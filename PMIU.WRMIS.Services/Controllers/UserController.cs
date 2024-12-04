using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PMIU.WRMIS.BLL.UserAdministration;
using System.Threading.Tasks;
using System.Web;
using PMIU.WRMIS.Services.Models;
using PMIU.WRMIS.Services.HelperClasses;

namespace PMIU.WRMIS.Services.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        PMIU.WRMIS.BLL.LoginBLL bllLogin = new BLL.LoginBLL();
        PMIU.WRMIS.BLL.UserAdministration.RolesBLL bllRole = new BLL.UserAdministration.RolesBLL();
        PMIU.WRMIS.BLL.UserAdministration.UserAdministrationBLL bllUserAdmin = new BLL.UserAdministration.UserAdministrationBLL();
        UserBLL bllUsers = new UserBLL();
        ServiceHelper srvcHlpr = new ServiceHelper();
        /// <summary>
        /// Verify user name and passwords
        /// </summary>
        /// <param name="_LoginParams"></param>
        /// <returns>reponse objct with contians User ID, name along with other info</returns>
        [HttpPost]
        [Route("Login")]  
        public ServiceResponse Login( [FromBody] Dictionary<string, string> _LoginParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string name = "", password = ""; 
                
                if (_LoginParams == null || _LoginParams.Count == 0)
                {
                    svcResponse.StatusCode = 1000;
                    svcResponse.StatusMessage = "FAILURE: No Params.";
                    svcResponse.Data = null;
                    svcResponse = CreateResponseObject(1000, "FAILURE: Parameteres are null.");
                    return svcResponse;
                }
                // See whether Dictionary contains this string.
                if (_LoginParams.ContainsKey("_UserName") && _LoginParams.ContainsKey("_Password"))
                {
                    name = _LoginParams["_UserName"];
                    password = _LoginParams["_Password"]; 
                }
                else
                {
                    svcResponse = CreateResponseObject(1001, "FAILURE:One of parameters is not available.");
                    return svcResponse;
                }


                string EncPassword = PMIU.WRMIS.Web.Common.WRMISEncryption.EncryptString(password);

                UA_Users mdlUser = bllLogin.ValidateUser(name, EncPassword);
                if (mdlUser != null)
                { 
                    
                    string sToken = bllUsers.GenerateToken(mdlUser.ID);
                    string _RoleName = bllRole.GetUserRoleByID(Convert.ToInt64(mdlUser.RoleID)) ;
                    
                    if (sToken != null)
                    {
                        if (mdlUser.DesignationID == null || mdlUser.DesignationID == 2)
                        {
                            svcResponse = CreateResponseObject(1003, "Application is not designed for Admin.");
                        }
                        else {
                            svcResponse.StatusCode = 0;
                            svcResponse.StatusMessage = "SUCCESS:";
                            svcResponse.Data = new { UserID = mdlUser.ID, AuthToken = sToken, FullName = mdlUser.FirstName + " " + mdlUser.LastName, DesignationID = mdlUser.DesignationID, RoleName = _RoleName };

                        }
                     }
                    else
                    {
                        svcResponse = CreateResponseObject(1002, "Not a valid user.");
                    } 
                }
                else
                { 
                    svcResponse = CreateResponseObject(1003, "Invalid credentials. Please try again.");
                }
            }
            catch (Exception exp)
            {
                svcResponse = CreateResponseObject(1004,"FAILURE: Unknown error occured. Please try again later.");
                string excetionMsg = exp.Message;
            } 
            return svcResponse;
        }

        /// <summary>
        /// returns the list of screens names assigned against the user role 
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserRights")]
        public ServiceResponse GetUserRights(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstRoleRights = bllLogin.GetAndroidUserRoleRights(_UserID);
                if (lstRoleRights.Count > 0)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = lstRoleRights;
                }
                else
                    svcResponse = CreateResponseObject(1004, "FAILURE:No record found.");
            }
            catch (Exception exp)
            {
                svcResponse = CreateResponseObject(1004, "FAILURE: Unknown error occured. Please try again later." );
                string excetionMsg = exp.Message;
            }
            return svcResponse; 
        }

        /// <summary>
        /// returns the list of screens names assigned against the user role 
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSystemParameters")]
        public ServiceResponse GetSystemParameterValue(String _ParameterKey)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (_ParameterKey == null || _ParameterKey.Equals("") || _ParameterKey.Equals("null"))
                {
                    _ParameterKey = null;
                }
                List<UA_SystemParameters> lstSysParams = bllUserAdmin.GetSystemParameterValue(_ParameterKey);
                if (lstSysParams.Count > 0)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = lstSysParams;
                }
                else
                    svcResponse = CreateResponseObject(1004, "FAILURE:No record found.");
            }
            catch (Exception exp)
            {
                svcResponse = CreateResponseObject(1004, "FAILURE: Unknown error occured. Please try again later.");
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Verify old password and update new password
        /// </summary>
        /// <param name="_changeParams"></param>
        [HttpPost]
        [Route("ChangePassword")]
        public ServiceResponse ChangePassword([FromBody] Dictionary<string, string> _changeParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string  oldPassword = "", newPassword = "";
                long UserID;

                if (_changeParams == null || _changeParams.Count == 0)
                {

                    svcResponse = CreateResponseObject(1000, "FAILURE: Parameteres are null.");
                    return svcResponse;
                }
                // See whether Dictionary contains this string.
                if (_changeParams.ContainsKey("_UserID") && _changeParams.ContainsKey("_OldPassword") && _changeParams.ContainsKey("_NewPassword"))
                {

                    UserID = Convert.ToInt64(_changeParams["_UserID"].ToString());
                    oldPassword = _changeParams["_OldPassword"];
                    newPassword = _changeParams["_NewPassword"];
                }
                else
                {
                    svcResponse = CreateResponseObject(1001, "FAILURE: One of parameters is not available.");
                    return svcResponse;
                }
                string EncOldPassword = PMIU.WRMIS.Web.Common.WRMISEncryption.EncryptString(oldPassword);
                string EncNewPassword = PMIU.WRMIS.Web.Common.WRMISEncryption.EncryptString(newPassword);


               
                UA_Users ObjUser = new UserBLL().GetUserbyID(Convert.ToInt64(UserID));
                string password = Convert.ToString(PMIU.WRMIS.Web.Common.WRMISEncryption.DecryptString(ObjUser.Password));
                
                if (!password.Equals(oldPassword))
                {
                    svcResponse = CreateResponseObject(1001, "FAILURE: Old Password is not correct.");
                    return svcResponse;
                }
                bllUsers.UserPasswordUpdation(Convert.ToInt64(UserID), Convert.ToString(PMIU.WRMIS.Web.Common.WRMISEncryption.EncryptString(newPassword)));
                svcResponse = CreateResponseObject(0, "SUCCESS: Password changed successfully.");
                       
                  
              
            }
            catch (Exception exp)
            {
                svcResponse = CreateResponseObject(1004, "FAILURE: Unknown error occured. Please try again later.");
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Verify old password and update new password
        /// </summary>
        /// <param name="_LoginParams"></param>
        /// <returns>reponse objct with contians User ID, name along with other info</returns>
        [HttpPost]
        [Route("ForgotPassword")]
        public ServiceResponse ForgotPassword([FromBody] Dictionary<string, string> _passwordParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string phoneNo = "";
                
                if (_passwordParams == null || _passwordParams.Count == 0)
                {

                    svcResponse = CreateResponseObject(1000, "FAILURE: Parameteres are null.");
                    return svcResponse;
                }
                // See whether Dictionary contains this string.
                if (_passwordParams.ContainsKey("_PhoneNo"))
                {

                    phoneNo = _passwordParams["_PhoneNo"];
                    
                }
                else
                {
                    svcResponse = CreateResponseObject(1001, "FAILURE:One of parameters is not available.");
                    return svcResponse;
                }
                

                try
                {
                    UA_Users ObjUser = new UserAdministrationBLL().GetUserPasswordID(phoneNo);
                    if (ObjUser != null)
                    {
                        string password = Convert.ToString(PMIU.WRMIS.Web.Common.WRMISEncryption.DecryptString(ObjUser.Password));
                        string _SMSMessage = "WRMIS Login Password: " + password + "";
                        PMIU.WRMIS.Common.Utility.SendSMS(phoneNo, _SMSMessage);
                        svcResponse = CreateResponseObject(0, "SUCCESS:Password Sent at your mobile number. Please go back to login");

                        
                    }
                    else
                    {
                        svcResponse = CreateResponseObject(1001, "Invalid Mobile Number. Please try again.");
                        return svcResponse;
                    }


                }
                catch (Exception e)
                {
                    svcResponse = CreateResponseObject(1004, "FAILURE: Unknown error occured. Please try again later.");

                }
               

                
            }
            catch (Exception exp)
            {
                svcResponse = CreateResponseObject(1004, "FAILURE: Unknown error occured. Please try again later.");
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Private method create the ServiceResponse with provided 
        /// code and message
        /// </summary>
        /// <param name="_StatusCode"></param>
        /// <param name="_StatusMessage"></param>
        /// <returns>ServiceResponse</returns>
        public ServiceResponse CreateResponseObject(int _StatusCode, string _StatusMessage)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            svcResponse.StatusCode = _StatusCode;
            svcResponse.StatusMessage = _StatusMessage;
            svcResponse.Data = null;

            return svcResponse;
        }
        [HttpGet]
        [Route("GetCurrentDateTime")]
        public ServiceResponse GetInspectionTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                string dateFormat = "yyyy-MM-dd hh:mm tt";
                string _date = DateTime.Now.ToString(dateFormat);
                if (_date != null)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = _date;
                }
                else
                {
                    svcResponse.StatusCode = 1001;
                    svcResponse.StatusMessage = "FAILURE: No Records Found.";
                    svcResponse.Data = null;
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetCurrentDateTime", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
    }

}
