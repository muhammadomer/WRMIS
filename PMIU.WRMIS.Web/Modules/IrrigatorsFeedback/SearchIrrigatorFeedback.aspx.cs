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
    public partial class SearchIrrigatorFeedback : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdowns();
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchIrrigatorFeedback] != null)
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

        private void BindDropdowns()
        {
            Dropdownlist.DDLZones(ddlZone);
            Dropdownlist.DDLTailStatus(ddlTailStatus);
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

        public List<GetTailofChannelinDivision_Result> GetChannelWhichDontHaveOffTakesAtTail()
        {
            IrrigatorFeedbackBLL bllIrrigator = new IrrigatorFeedbackBLL();
            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            List<GetTailofChannelinDivision_Result> lstChannelNames = bllIrrigator.GetChannelsByDivisionID(DivisionID);
            DailyDataBLL bllDailyData = new DailyDataBLL();

            //lstChannelNames = lstChannelNames.Where(lsn => !bllDailyData.HasOffTake((long)lsn.GaugeID)).ToList();
            return lstChannelNames;
        }

        private void BindGrid()
        {
            List<dynamic> SearchCriteria = new List<dynamic>();
            long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
            long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
            long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
            long ChannelID = ddlChannelName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannelName.SelectedItem.Value);
            int TailStatus = ddlTailStatus.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlTailStatus.SelectedItem.Value);
            //string TailStatus = ddlTailStatus.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlTailStatus.SelectedItem.Value);
            string IrrigatorStatus = ddlIrrigatorStatus.SelectedItem.Value == string.Empty ? "" : Convert.ToString(ddlIrrigatorStatus.SelectedItem.Value);
            string MobileNo = txtIrrigatorMobileNo.Text == string.Empty ? "" : Convert.ToString(txtIrrigatorMobileNo.Text);
            //DateTime? FromDate = txtFromDate.Text == String.Empty ? (DateTime?)null : DateTime.Parse(txtFromDate.Text);
            //DateTime? ToDate = txtToDate.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);
            bool FrontTS = chkFront.Checked;
            bool LeftTS = chkLeft.Checked;
            bool RightTS = chkRight.Checked;

            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            List<IF_IrrigatorFeedback> lstIrrigatorFeedback = new List<IF_IrrigatorFeedback>();
            gvIrrigatorFeedback.DataSource = bllIrrigatorFeedback.GetIrrigatorFeedback(ZoneID, CircleID, DivisionID, ChannelID, FrontTS, LeftTS, RightTS, IrrigatorStatus, MobileNo, TailStatus);
            gvIrrigatorFeedback.DataBind();

            SearchCriteria.Add(ZoneID);
            SearchCriteria.Add(CircleID);
            SearchCriteria.Add(DivisionID);
            SearchCriteria.Add(ChannelID);
            SearchCriteria.Add(IrrigatorStatus);
            SearchCriteria.Add(MobileNo);
            SearchCriteria.Add(TailStatus);
            SearchCriteria.Add(FrontTS);
            SearchCriteria.Add(LeftTS);
            SearchCriteria.Add(RightTS);
            SearchCriteria.Add(gvIrrigatorFeedback.PageIndex);
            Session[SessionValues.SearchIrrigatorFeedback] = SearchCriteria;
        }

        protected void gvIrrigatorFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTailFront = (Label)e.Row.FindControl("lblgvTailFront");
                    Label lblTailLeft = (Label)e.Row.FindControl("lblgvTailLeft");
                    Label lblTailRight = (Label)e.Row.FindControl("lblgvTailRight");
                    Label lblTailStatus = (Label)e.Row.FindControl("lblgvTailStatus");
                    Label lblTailStatusDate = (Label)e.Row.FindControl("lblgvTailStatusDate");
                    Label lblIrrigatorStatus = (Label)e.Row.FindControl("lblgvIrrigatorStatus");
                    Label lblMobile2 = (Label)e.Row.FindControl("lblgvMobile2");
                    Label lblClose = (Label)e.Row.FindControl("lblChannelClosed");




                    //DataKey key = gvIrrigatorFeedback.DataKeys[e.Row.RowIndex];
                    //bool IsClosed = Convert.ToBoolean(key.Values["ChannelClosed"]);

                    if (lblMobile2.Text == "")
                    {
                        lblMobile2.Text = Constants.Dash;
                    }

                    if (lblTailFront.Text == "True")
                    {
                        lblTailFront.Text = Constants.Tick;
                    }
                    else
                    {
                        lblTailFront.Text = Constants.Dash;
                    }

                    if (lblTailLeft.Text == "True")
                    {
                        lblTailLeft.Text = Constants.Tick;
                    }
                    else
                    {
                        lblTailLeft.Text = Constants.Dash;
                    }

                    if (lblTailRight.Text == "True")
                    {
                        lblTailRight.Text = Constants.Tick;
                    }
                    else
                    {
                        lblTailRight.Text = Constants.Dash;
                    }

                    //if (lblTailStatus.Text == "1")
                    //{
                    //    lblTailStatus.Text = "Dry";
                    //}
                    //else if (lblTailStatus.Text == "2")
                    //{
                    //    lblTailStatus.Text = "Short Tail";
                    //}
                    //else if (lblTailStatus.Text == "3")
                    //{
                    //    lblTailStatus.Text = "Authorized Tail";
                    //}
                    //else if (lblTailStatus.Text == "4")
                    //{
                    //    lblTailStatus.Text = "Excessive Tail";
                    //}

                    if (lblTailStatus.Text != "")
                    {
                        dynamic TailStatus = CommonLists.GetTailStatuses(lblTailStatus.Text).FirstOrDefault<dynamic>();
                        string Statuss = TailStatus.GetType().GetProperty("Name").GetValue(TailStatus, null);
                        string Date = Utility.GetFormattedDate(Convert.ToDateTime(lblTailStatusDate.Text));



                        if (Convert.ToBoolean(lblClose.Text) == true && Statuss == "Dry")
                        {
                            lblTailStatus.Text = "Closed" + " (" + Date + ")";
                            HyperLink hl = (HyperLink)e.Row.FindControl("hlAddFeedback");
                            if (hl != null)
                                hl.Visible = false;
                        }
                        else
                        {
                            //dynamic TailStatus = CommonLists.GetTailStatuses(lblTailStatus.Text).FirstOrDefault<dynamic>();
                            //lblTailStatus.Text = TailStatus.GetType().GetProperty("Name").GetValue(TailStatus, null);
                            //string Date = Utility.GetFormattedDate(Convert.ToDateTime(lblTailStatusDate.Text));
                            lblTailStatus.Text = Statuss + " (" + Date + ")";   //lblTailStatus.Text = TailStatus.GetType().GetProperty("Name").GetValue(TailStatus, null) + " (" + Date + ")";
                        }
                    }
                    else
                    {
                        lblTailStatus.Text = string.Empty;
                    }

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

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 6/6/16 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchIrrigatorFeedback);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindHistoryData()
        {
            BindDropdowns();
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.SearchIrrigatorFeedback];
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

            Dropdownlist.SetSelectedValue(ddlTailStatus, Convert.ToString(SearchCriteria[6]));

            chkFront.Checked = Convert.ToBoolean(SearchCriteria[7]);
            chkLeft.Checked = Convert.ToBoolean(SearchCriteria[8]);
            chkRight.Checked = Convert.ToBoolean(SearchCriteria[9]);

            gvIrrigatorFeedback.PageIndex = (int)SearchCriteria[10];
            BindGrid();
        }

        protected void gvIrrigatorFeedback_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvIrrigatorFeedback.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIrrigatorFeedback_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIrrigatorFeedback.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}