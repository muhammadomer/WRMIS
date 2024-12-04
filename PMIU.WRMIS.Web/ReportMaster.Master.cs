using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using PMIU.WRMIS.Exceptions;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Web
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    {
        CheckRoleRights checkRoleRights;
        string userRoleName;
        long userRoleId;
        RoleRightBits bits;
        RoleRightBits bitts;

        #region "Set dynamic page level message"
        //public string ModuleTitle
        //{
        //    //get
        //    //{
        //    //    return ltlModuleTitle.Text;
        //    //}
        //    //set
        //    //{
        //    //    ltlModuleTitle.Text = value;
        //    //}
        //}
        public string PageTitle
        {
            get
            {
                return "";// ltlPageTitle.Text;
            }
            set
            {
                //ltlPageTitle.Text = value;
            }
        }

        public string NavigationBar
        {
            get
            {
                return string.Empty;
            }
            set
            {
                // ltlNavigationBar.Text = value;
            }
        }
        #endregion

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {

            if (Session.Contents.Count == 0)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            HideMessageInstantly();

            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Div.Visible = false;
            //lblMsgs.Visible = false;            

            if (Session[SessionValues.UserID] == null)
            {
                Response.Redirect("~/Login.aspx", true);
                return;
            }

            if (!Page.IsPostBack)
            {
                string filepath = Request.Url.LocalPath;

                UA_Users user = SessionManagerFacade.UserInformation;
                SetHiddenFieldsValue(user);

                user_info.InnerText = String.Format("{0} {1} [{2}]", user.FirstName, user.LastName, user.UA_Roles.Name);

                if (!UserRoleHasRights(user.ID, filepath))
                {
                    Response.Redirect("~/AccessDenied.aspx");
                    return;
                }

                string PageUrl = HttpContext.Current.Request.Url.AbsolutePath;

                GetTreeViewItems(PageUrl);
                GetTimeIntervalValue();
            }
        }
        private void SetHiddenFieldsValue(UA_Users _user)
        {
            hdnUserID.Value = Convert.ToString(_user.ID);
            hdnUserDesignationID.Value = Convert.ToString(_user.DesignationID);
            hdnUserRoleID.Value = Convert.ToString(_user.RoleID);

            hdnDateFormat.Value = Utility.ReadConfiguration("DateFormat");
            hdnTimeFormat.Value = Utility.ReadConfiguration("TimeFormat");
        }
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        public string MessageText
        {
            get { return dvMessage.InnerText; }
            set { dvMessage.InnerText = value; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl Div
        {
            get { return dvMessage; }
            set { dvMessage = value; }
        }

        public void ShowMessage(string _MessageText, MessageType _MessageType = MessageType.Success)
        {
            //MessageText = _MessageText;

            lblMsgs.Text = _MessageText;

            if (_MessageType == MessageType.Success)
            {
                //Div.Attributes.Remove("class");
                //Div.Attributes.Add("class", "SuccessMsg");

                lblMsgs.CssClass = "SuccessMsg";

            }
            else if (_MessageType == MessageType.Error)
            {
                //Div.Attributes.Remove("class");
                //Div.Attributes.Add("class", "ErrorMsg");

                lblMsgs.CssClass = "ErrorMsg";
            }
            else
                lblMsgs.CssClass = "WarningMsg";

            //Div.Style.Remove(HtmlTextWriterStyle.Display);
            //Div.Style.Add(HtmlTextWriterStyle.Display, "block");

            //Div.Visible = true;
            lblMsgs.Visible = true;
            lblMsgs.Style.Add(HtmlTextWriterStyle.Display, "block");

            HideMessage();

            UpdatePanelMaster.Update();
        }

        public void HideMessage()
        {
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "hide", "<script>setTimeout(function () { $(\"#lblMsgs\").hide(); }, 5000);</script>");

            //Page.RegisterClientScriptBlock("hide", "setTimeout(function () { $(\"#ErrorMsg\").hide(); }, 5000);");
        }

        public void HideMessageInstantly()
        {
            //lblMsgs.Visible = false;
            lblMsgs.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session[SessionValues.UserID] = null;
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("~/Login.aspx", false);
        }

        public enum MessageType
        {
            Success,
            Error,
            Warning
        };

        #region Menu

        private void GetTreeViewItems(string _PageUrl)
        {
            UserBLL bllUser = new UserBLL();

            List<long> lstPageID = bllUser.GetParentPageIDs(_PageUrl);

            checkRoleRights = new CheckRoleRights(Convert.ToInt32(Session[SessionValues.UserID]));

            userRoleName = checkRoleRights.GetUserRole().Trim(); // will return the role name of the current logged In user.
            userRoleId = checkRoleRights.GetRoleId(userRoleName); // will return the role id of the current loggned in user.

            List<UA_Pages> ds = bllUser.GenerateMenu(userRoleId);

            string ParentNodes = String.Empty;

            foreach (UA_Pages level1DataRow in ds)
            {
                if (lstPageID.Contains(level1DataRow.ID))
                {
                    ParentNodes = ParentNodes + "<li class='active'><a href='#' class='dropdown-toggle'><i class=\"fa fa-dod\"></i><span>" + level1DataRow.Name.Trim() + "</span><b class='arrow fa fa-angle-right'></b></a>";
                }
                else
                {
                    ParentNodes = ParentNodes + "<li><a href='#' class='dropdown-toggle'><i class=\"fa fa-dod\"></i><span>" + level1DataRow.Name.Trim() + "</span><b class='arrow fa fa-angle-right'></b></a>";
                }

                ParentNodes = ParentNodes + GetChildRows(level1DataRow, lstPageID);
            }

            navlist.InnerHtml = ParentNodes;
        }

        private string GetChildRows(UA_Pages level1DataRow, List<long> lstPageID)
        {
            UserBLL bllUser = new UserBLL();

            List<UA_Pages> childRows = new List<UA_Pages>();

            childRows = bllUser.GetChildMenuRows(level1DataRow.ID, userRoleId);

            CheckRoleRights checkroleRights = new CheckRoleRights(Convert.ToInt32(Session[SessionValues.UserID]));

            string ChildLevelNodes = String.Empty;

            if (childRows.Count == 0)
            {
                ChildLevelNodes = "</li>";
            }
            else
            {
                ChildLevelNodes = "<ul class='submenu'>";

                foreach (UA_Pages childRow in childRows)
                {
                    bits = new RoleRightBits();

                    bits = checkroleRights.GetRoleRightBits(userRoleId, level1DataRow.ID);

                    if (bits.ViewBit != false) //To check if the visibility bit of page is false. If true then statements would be executed.
                    {
                        if (childRow.Description.CompareTo("#") == 0)
                        {
                            if (lstPageID.Contains(childRow.ID))
                            {
                                ChildLevelNodes = ChildLevelNodes + "<li class='active'><a href='#' class='dropdown-toggle'><span>" + childRow.Name.Trim() + "</span><b style='margin-left:10px;' class='arrow fa fa-angle-right'></b></a>";
                            }
                            else
                            {
                                ChildLevelNodes = ChildLevelNodes + "<li><a href='#' class='dropdown-toggle'><span>" + childRow.Name.Trim() + "</span><b style='margin-left:10px;' class='arrow fa fa-angle-right'></b></a>";
                            }

                            List<UA_Pages> nestedPages = new List<UA_Pages>();

                            nestedPages = bllUser.GetChildMenuRows(childRow.ID, userRoleId);

                            if (nestedPages.Count > 0)
                            {
                                ChildLevelNodes = ChildLevelNodes + GetChildRows(childRow, lstPageID);
                            }
                            else
                            {
                                ChildLevelNodes = ChildLevelNodes + "</li>";
                            }
                        }
                        else
                        {
                            if (lstPageID.Contains(childRow.ID))
                            {
                                ChildLevelNodes = ChildLevelNodes + "<li class='active'><a href='" + childRow.Description + "'>" + childRow.Name.Trim() + "</a></li>";
                            }
                            else
                            {
                                ChildLevelNodes = ChildLevelNodes + "<li><a href='" + childRow.Description + "'>" + childRow.Name.Trim() + "</a></li>";
                            }
                        }
                    }

                }

                ChildLevelNodes = ChildLevelNodes + "</ul></li>";
            }

            return ChildLevelNodes;
        }

        private bool UserRoleHasRights(long _UserID, string _PageName)
        {
            List<string> lstExcludedPages = new List<string>();

            //excludedPages.Add("HomePage");
            //excludedPages.Add("AccessDenied");
            //excludedPages.Add("");

            lstExcludedPages = Utility.ReadConfiguration("ExcludedPages").Split(',').ToList<string>();

            foreach (string excludedPage in lstExcludedPages)
            {
                if (_PageName.ToUpper().Trim().Contains(excludedPage.ToUpper().Trim()))
                    return true;
            }

            try
            {
                checkRoleRights = new CheckRoleRights(_UserID);
                long userRoleId = checkRoleRights.GetRoleID();

                if (userRoleId > 0)
                {
                    if (checkRoleRights.HasRoleRights_1(userRoleId, _PageName))
                    {
                        bitts = new RoleRightBits();
                        bitts = checkRoleRights.GetRoleRightBits_1(userRoleId, _PageName);

                        if (bitts != null)
                        {
                            //Session["RoleRights"] = bitts;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This function returns the role rights values for the page.
        /// Created On 16-03-2016
        /// </summary>
        /// <returns>UA_RoleRights</returns>
        public UA_RoleRights GetPageRoleRights()
        {
            List<UA_RoleRights> lstRoleRight = (List<UA_RoleRights>)Session[SessionValues.RoleRightList];

            if (lstRoleRight != null)
            {
                string PageUrl = HttpContext.Current.Request.Url.AbsolutePath;

                UA_RoleRights mdlRoleRights = lstRoleRight.FirstOrDefault(rr => rr.UA_Pages.Description.ToUpper().Trim() == PageUrl.ToUpper().Trim());

                return mdlRoleRights;
            }
            else
            {
                return null;
            }

        }

        #endregion

        #region SetInterval
        public void GetTimeIntervalValue()
        {
            UA_SystemParameters mdlIntervalValue = new UserBLL().GetSystemParameterValue((short)Constants.SystemParameter.AlertTimeInterval);
            hdnTimeInterval.Value = mdlIntervalValue.ParameterValue;
        }

        #endregion
    }
}