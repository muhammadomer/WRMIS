using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery
{
    public partial class EntitlementDistribution : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindCommand();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindCommand()
        {
            try
            {
                ddlCommand.DataSource = CommonLists.GetCommands();
                ddlCommand.DataValueField = "ID";
                ddlCommand.DataTextField = "Name";
                ddlCommand.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindMainCanals()
        {
            try
            {
                new EntitlementDeliveryBLL().GetMainCanals(Convert.ToInt64(ddlCommand.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCommand.SelectedItem.Value != "")
                {
                    ddlSeason.Enabled = true;
                    ddlSeason.ClearSelection();
                    //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                    //ddlSeason.DataValueField = "ID";
                    //ddlSeason.DataTextField = "Name";
                    //ddlSeason.DataBind();
                    Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());

                    ddlYear.Enabled = true;
                    ddlYear.ClearSelection();
                    ddlYear.DataSource = new EntitlementDeliveryBLL().GetYears(Convert.ToInt64(ddlCommand.SelectedItem.Value));
                    ddlYear.DataValueField = "ID";
                    ddlYear.DataTextField = "Name";
                    ddlYear.DataBind();
                    ddlYear.Items.Insert(0, new ListItem("Select", ""));

                    ddlMainCanals.Enabled = true;
                    ddlMainCanals.ClearSelection();
                    ddlMainCanals.DataSource = new EntitlementDeliveryBLL().GetMainCanals(Convert.ToInt64(ddlCommand.SelectedItem.Value));
                    ddlMainCanals.DataValueField = "ID";
                    ddlMainCanals.DataValueField = "Name";
                    ddlMainCanals.DataBind();
                    ddlMainCanals.Items.Insert(0, new ListItem("Select", ""));

                    ddlBranchCanal.ClearSelection();
                    ddlBranchCanal.Enabled = false;
                    ddlDistributary.ClearSelection();
                    ddlDistributary.Enabled = false;
                    ddlMinor.ClearSelection();
                    ddlMinor.Enabled = false;
                    ddlSubMinor.ClearSelection();
                    ddlSubMinor.Enabled = false;
                }
                else
                {
                    ddlMainCanals.ClearSelection();
                    ddlMainCanals.Enabled = false;
                    ddlYear.ClearSelection();
                    ddlYear.Enabled = false;
                    ddlSeason.ClearSelection();
                    ddlSeason.Enabled = false;
                    ddlBranchCanal.ClearSelection();
                    ddlBranchCanal.Enabled = false;
                    ddlDistributary.ClearSelection();
                    ddlDistributary.Enabled = false;
                    ddlMinor.ClearSelection();
                    ddlMinor.Enabled = false;
                    ddlSubMinor.ClearSelection();
                    ddlSubMinor.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }




    }
}