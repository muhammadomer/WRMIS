using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common.Controls;
namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class SearchUnscheduledInspection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 06-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ActionOnSchedule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindDropdownlists()
        {
            try
            {
                // Bind Channel type dropdownlist
                Dropdownlist.DDLChannelTypes(ddlChannelType, (int)Constants.DropDownFirstOption.All);
                // Bind Flow type dropdownlist
                Dropdownlist.DDLFlowTypes(ddlFlowType, (int)Constants.DropDownFirstOption.All);
                // Bind Common name dropdownlist
                Dropdownlist.DDLCommandNames(ddlCommandName, (int)Constants.DropDownFirstOption.All);
                //Bind Parent Channel dropdownlist

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}