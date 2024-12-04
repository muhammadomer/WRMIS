using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigatorsFeedback;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigatorsFeedback
{
    public partial class SearchIrrigator : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    hlAddIrrigator.NavigateUrl = "~/Modules/IrrigatorsFeedback/AddIrrigator.aspx";
                    BindDropdowns();
                    //hlAddIrrigator.Visible = base.CanAdd;
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchIrrigator] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDropdowns()
        {
            Dropdownlist.DDLZones(ddlZone);
            Dropdownlist.DDLStatus(ddlIrrigatorStatus);
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID);
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;

                ddlChannelName.SelectedIndex = 0;
                ddlChannelName.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    ddlDivision.Enabled = true;
                }
                ddlChannelName.SelectedIndex = 0;
                ddlChannelName.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == String.Empty)
                {
                    ddlChannelName.SelectedIndex = 0;
                    ddlChannelName.Enabled = false;
                }
                else
                {
                    List<GetTailofChannelinDivision_Result> lstChannelWhichDontHaveOffTakesAtTail = GetChannelWhichDontHaveOffTakesAtTail();
                    Dropdownlist.BindDropdownlist<List<GetTailofChannelinDivision_Result>>(ddlChannelName, lstChannelWhichDontHaveOffTakesAtTail, (int)Constants.DropDownFirstOption.Select, "ChannelName", "ChannelID");
                    ddlChannelName.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function return all channels Which Dont Have Off Takes At Tail.
        /// Created On:11/05/2016
        /// </summary>
        /// <returns>List<GetChannelOfDivision_Result></returns>
        public List<GetTailofChannelinDivision_Result> GetChannelWhichDontHaveOffTakesAtTail()
        {
            IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();
            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            List<GetTailofChannelinDivision_Result> lstChannelNames = bllIrrigator.GetChannelsByDivisionID(DivisionID);
            //DailyDataBLL bllDailyData = new DailyDataBLL();

            //lstChannelNames = lstChannelNames.Where(lsn => !bllDailyData.HasOffTake((long)lsn.GaugeID)).ToList();
            return lstChannelNames;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function bind data to gridview on the basis of given parameters
        /// Created On: 11/05/2016
        /// </summary>
        private void BindGrid()
        {
            List<dynamic> SearchCriteria = new List<dynamic>();
            long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
            long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);

            long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
            long ChannelID = ddlChannelName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannelName.SelectedItem.Value);
            string IrrigatorStatus = ddlIrrigatorStatus.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlIrrigatorStatus.SelectedItem.Value);
            string MobileNo = txtIrrigatorMobileNo.Text == string.Empty ? "" : Convert.ToString(txtIrrigatorMobileNo.Text);

            gvIrrigator.DataSource = new IrrigatorFeedbackBLL().GetIrrigatorByDivisonIDAndChannelID(ZoneID, CircleID, DivisionID, ChannelID, MobileNo, IrrigatorStatus);
            gvIrrigator.DataBind();

            SearchCriteria.Add(ZoneID);
            SearchCriteria.Add(CircleID);
            SearchCriteria.Add(DivisionID);
            SearchCriteria.Add(ChannelID);
            SearchCriteria.Add(IrrigatorStatus);
            SearchCriteria.Add(MobileNo);
            SearchCriteria.Add(gvIrrigator.PageIndex);
            Session[SessionValues.SearchIrrigator] = SearchCriteria;
        }

        protected void gvIrrigator_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int rowIndex = e.NewEditIndex;
                long UserID = Convert.ToInt64(((Label)gvIrrigator.Rows[rowIndex].Cells[0].FindControl("lblIrrigatorID")).Text);
                Response.Redirect("~/Modules/IrrigatorsFeedback/AddIrrigator.aspx?UserID=" + UserID, false);// + "&PageID=" + base.PageID);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 6/6/16 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchIrrigator);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvIrrigator_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblIrrigatorStatus = (Label)e.Row.FindControl("lblgvIrrigatorStatus");

                    //if (lblIrrigatorStatus.Text == "1")
                    //{
                    //    lblIrrigatorStatus.Text = "Active";
                    //}
                    //else
                    //{
                    //    lblIrrigatorStatus.Text = "Inactive";
                    //}

                    dynamic IrrigatorStatus = CommonLists.GetStatuses(lblIrrigatorStatus.Text).FirstOrDefault<dynamic>();
                    if (IrrigatorStatus != null)
                    {
                        lblIrrigatorStatus.Text = IrrigatorStatus.GetType().GetProperty("Name").GetValue(IrrigatorStatus, null);
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindHistoryData()
        {
            BindDropdowns();
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.SearchIrrigator];
            long ZoneID = Convert.ToInt64(SearchCriteria[0]);
            if (ZoneID != -1)
            {
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID.ToString());
                BindCircleDropdown(ZoneID);
                long CircleID = Convert.ToInt64(SearchCriteria[1]);
                ddlZone.Enabled = true;
                if (CircleID != -1)
                {
                    Dropdownlist.SetSelectedValue(ddlCircle, CircleID.ToString());
                    BindDivisionDropdown(CircleID);
                    long DivisionID = Convert.ToInt64(SearchCriteria[2]);
                    ddlCircle.Enabled = true;
                    if (DivisionID != -1)
                    {
                        Dropdownlist.SetSelectedValue(ddlDivision, DivisionID.ToString());
                        long ChannelID = Convert.ToInt64(SearchCriteria[3]);
                        ddlDivision.Enabled = true;
                        if (ChannelID != -1)
                        {
                            IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();
                            List<GetTailofChannelinDivision_Result> lstChannelNames = bllIrrigator.GetChannelsByDivisionID(DivisionID);
                            Dropdownlist.BindDropdownlist<List<GetTailofChannelinDivision_Result>>(ddlChannelName, lstChannelNames, (int)Constants.DropDownFirstOption.Select, "ChannelName", "ChannelID");
                            Dropdownlist.SetSelectedValue(ddlChannelName, ChannelID.ToString());
                            ddlChannelName.Enabled = true;
                        }
                    }
                }
            }
            Dropdownlist.SetSelectedValue(ddlIrrigatorStatus, Convert.ToString(SearchCriteria[4]));
            txtIrrigatorMobileNo.Text = Convert.ToString(SearchCriteria[5]);
            gvIrrigator.PageIndex = (int)SearchCriteria[6];
            BindGrid();
        }

        protected void gvIrrigator_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvIrrigator.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIrrigator_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIrrigator.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}