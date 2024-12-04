using PMIU.WRMIS.BLL.SeasonalPlanning;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class FillingFraction : BasePage
    {
        #region ViewState

        public String TDailyID = "TDailyID";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDropDowns();
                    SetTitle();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindDropDowns()
        {
            try
            {
                //ddlSeason.DataSource = CommonLists.GetSeasonDropDown();
                //ddlSeason.DataTextField = "Name";
                //ddlSeason.DataValueField = "ID";
                //ddlSeason.DataBind();
                Dropdownlist.BindDropdownlist(ddlSeason, CommonLists.GetSeasonDropDown());

                ddlRimstation.DataSource = CommonLists.GetRimStations();
                ddlRimstation.DataTextField = "Name";
                ddlRimstation.DataValueField = "ID";
                ddlRimstation.DataBind();
                gvRimstation.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FillingFraction);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Value = ddlSeason.SelectedItem.Value;
                if (Value != "")
                {
                    ddlRimstation.Enabled = true;
                    ddlRimstation.ClearSelection();
                    ddlRimstation.Items.FindByText("Select").Selected = true;
                    gvRimstation.Visible = false;
                }
                else
                    ddlRimstation.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlRimstation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeason.SelectedItem.Value != "" && ddlRimstation.SelectedItem.Value != "")
                {
                    long SeasonID = Convert.ToInt64(ddlSeason.SelectedItem.Value);
                    long RimStationID = Convert.ToInt64(ddlRimstation.SelectedItem.Value);
                    BindGrid(SeasonID, RimStationID);
                    gvRimstation.EditIndex = -1;
                    gvRimstation.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid(long _SeasonID, long _RimStationID)
        {
            try
            {
                gvRimstation.DataSource = new SeasonalPlanningBLL().GetFillingFractionDetail(_SeasonID, _RimStationID);
                gvRimstation.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRimstation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRimstation.EditIndex = -1;
                BindGrid(Convert.ToInt64(ddlSeason.SelectedItem.Value), Convert.ToInt64(ddlRimstation.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRimstation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowID = e.RowIndex;
                long ID = Convert.ToInt64(((Label)gvRimstation.Rows[RowID].Cells[0].FindControl("lblId")).Text);
                Decimal MaxPercentage = Convert.ToDecimal(((TextBox)gvRimstation.Rows[RowID].Cells[2].FindControl("txtMaxPercentage")).Text);
                Decimal MinPercentage = Convert.ToDecimal(((TextBox)gvRimstation.Rows[RowID].Cells[3].FindControl("txtMinPercentage")).Text);
                Decimal LikelyPercentage = Convert.ToDecimal(((TextBox)gvRimstation.Rows[RowID].Cells[4].FindControl("txtLikelyPercentage")).Text);

                bool Result = new SeasonalPlanningBLL().UpdateFillingFraction(ID, MaxPercentage, MinPercentage, LikelyPercentage, Convert.ToInt64(Session[SessionValues.UserID]));
                if (Result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    gvRimstation.EditIndex = -1;
                    gvRimstation.PageIndex = 1;
                    BindGrid(Convert.ToInt64(ddlSeason.SelectedItem.Value), Convert.ToInt64(ddlRimstation.SelectedItem.Value));
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void gvRimstation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvRimstation.EditIndex = e.NewEditIndex;
                BindGrid(Convert.ToInt64(ddlSeason.SelectedItem.Value), Convert.ToInt64(ddlRimstation.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRimstation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRimstation.PageIndex = e.NewPageIndex;
                BindGrid(Convert.ToInt64(ddlSeason.SelectedItem.Value), Convert.ToInt64(ddlRimstation.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRimstation_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRimstation.EditIndex = -1;
                BindGrid(Convert.ToInt64(ddlSeason.SelectedItem.Value), Convert.ToInt64(ddlRimstation.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryGrid(long _TDailyID)
        {
            try
            {
                gvHistory.DataSource = new SeasonalPlanningBLL().GetFillingFractionHistory(_TDailyID);
                gvHistory.DataBind();
                lblName.Text = "History";
                lblName.ForeColor = Color.White;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            ViewState[TDailyID] = Convert.ToInt64((sender as LinkButton).CommandArgument);
            BindHistoryGrid(Convert.ToInt64(ViewState[TDailyID]));

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalHistory').modal();", true);
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHistory.PageIndex = e.NewPageIndex;
                BindHistoryGrid(Convert.ToInt64(Session[TDailyID]));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvHistory.EditIndex = -1;
                BindHistoryGrid(Convert.ToInt64(Session[TDailyID]));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }        
    }
}