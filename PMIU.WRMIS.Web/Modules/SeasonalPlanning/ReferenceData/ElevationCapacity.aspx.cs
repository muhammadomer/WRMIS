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

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData
{
    public partial class ElevationCapacity : BasePage
    {
        #region ViewState
        public String RecordID = "RecordID";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindDropDowns();
                    if (Request.QueryString["ShowMsg"] != null)
                    {
                        if (Request.QueryString["ShowMsg"].ToString() == "YES")
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }

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

        public void BindGrid(DateTime _SelectedDate, long _RimStationID)
        {
            try
            {
                gvElevation.DataSource = new SeasonalPlanningBLL().GetRecordsOfSelectedDate(_SelectedDate, _RimStationID);
                gvElevation.DataBind();
                gvElevation.Visible = true;
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
                ddlRimStation.DataSource = CommonLists.GetRimStationsForElevationCapacity();
                ddlRimStation.DataTextField = "Name";
                ddlRimStation.DataValueField = "ID";
                ddlRimStation.DataBind();

                ddlDates.DataSource = new SeasonalPlanningBLL().GetSavedDates();
                ddlDates.DataTextField = "Year";
                ddlDates.DataValueField = "Year";
                ddlDates.DataBind();
                ddlDates.Items.Insert(0, new ListItem("Select", ""));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlRimStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRimStation.SelectedItem.Value != "")
                    ddlDates.Enabled = true;
                else
                    ddlDates.Enabled = false;
                gvElevation.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRimStation.SelectedItem.Value != "" && ddlDates.SelectedItem.Value != "")
                {
                    BindGrid(Convert.ToDateTime(ddlDates.SelectedItem.Value), Convert.ToInt64(ddlRimStation.SelectedItem.Value));
                    gvElevation.EditIndex = -1;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvElevation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvElevation.EditIndex = -1;

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvElevation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long RecordID = Convert.ToInt64(((Label)gvElevation.Rows[Convert.ToInt32(e.RowIndex)].Cells[0].FindControl("lblID")).Text);
                decimal Capacity = Convert.ToDecimal(((TextBox)gvElevation.Rows[Convert.ToInt32(e.RowIndex)].Cells[2].FindControl("txtCapacity")).Text);
                bool Result = new SeasonalPlanningBLL().UpDateElevationCapacity(RecordID, Capacity, Convert.ToInt64(Session[SessionValues.UserID]));
                if (Result)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    gvElevation.EditIndex = -1;
                    if (ddlRimStation.SelectedItem.Value != "" && ddlDates.SelectedItem.Value != "")
                    {
                        BindGrid(Convert.ToDateTime(ddlDates.SelectedItem.Value), Convert.ToInt64(ddlRimStation.SelectedItem.Value));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvElevation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (ddlRimStation.SelectedItem.Value != "" && ddlDates.SelectedItem.Value != "")
                {
                    gvElevation.EditIndex = e.NewEditIndex;
                    BindGrid(Convert.ToDateTime(ddlDates.SelectedItem.Value), Convert.ToInt64(ddlRimStation.SelectedItem.Value));
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvElevation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindHistoryGrid(long _ID)
        {
            try
            {
                gvHistory.DataSource = new SeasonalPlanningBLL().GetElevationCapacityHistory(_ID);
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
            try
            {
                ViewState[RecordID] = Convert.ToInt64((sender as LinkButton).CommandArgument);
                BindHistoryGrid(Convert.ToInt64(ViewState[RecordID]));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ModalHistory').modal();", true);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_PageIndexChanged(object sender, EventArgs e)
        {
            gvHistory.EditIndex = -1;
            BindHistoryGrid(Convert.ToInt64(Session[RecordID]));
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistory.PageIndex = e.NewPageIndex;
            BindHistoryGrid(Convert.ToInt64(Session[RecordID]));
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("AddElevationCapacity.aspx");
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}