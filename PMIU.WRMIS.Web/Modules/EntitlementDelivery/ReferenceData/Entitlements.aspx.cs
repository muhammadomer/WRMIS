using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.SeasonalPlanning;
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

namespace PMIU.WRMIS.Web.Modules.EntitlementDelivery.ReferenceData
{
    public partial class Entitlements : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


            }
        }


        public void BindDropDown()
        {
            try
            {
                ddlCommand.DataSource = CommonLists.GetCommands();
                ddlCommand.DataTextField = "Name";
                ddlCommand.DataValueField = "ID";
                ddlCommand.DataBind();


                //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                //ddlSeason.DataTextField = "Name";
                //ddlSeason.DataValueField = "ID";
                //ddlSeason.DataBind();
                Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());
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
                    ddlSeason.Enabled = true;
                else
                    ddlSeason.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ddlSeason.SelectedItem.Value != "")
                {
                    

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelum_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelum_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelum_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelumHistory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvJhelumHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}