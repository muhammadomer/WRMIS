using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web
{
    public partial class AccessDenied : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Code = "";
                if (Request.QueryString.Count > 0)
                {
                    Code = Request.QueryString[0].ToString();
                }

                if (!IsPostBack)
                {
                    SetPageTitle(Code);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-03-2016
        /// </summary>
        private void SetPageTitle(string _Code)
        {
            //PageName PageTitle = PageName.AccessDenied;

            Master.ModuleTitle = string.Empty;

            if (_Code.Trim() == "E")
            {
                //PageTitle = PageName.UnknownError;
                msg.InnerText = "Some Unknown Error Occured. Please try again later.";
            }

            //Tuple<string, string, string> pageTitle = base.SetPageTitle(PageTitle);
            //Master.ModuleTitle = pageTitle.Item1;
            //Master.PageTitle = pageTitle.Item2;
            //Master.NavigationBar = pageTitle.Item3;
        }
    }
}