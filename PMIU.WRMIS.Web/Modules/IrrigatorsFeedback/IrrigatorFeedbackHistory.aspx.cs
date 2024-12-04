using PMIU.WRMIS.BLL.IrrigatorsFeedback;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigatorsFeedback
{
    public partial class IrrigatorFeedbackHistory : BasePage
    {
        long IrrigatorID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    hlBack.NavigateUrl = "~/Modules/IrrigatorsFeedback/SearchIrrigatorFeedback.aspx?ShowHistory=true";
                    txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                    txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

                    if (!string.IsNullOrEmpty(Request.QueryString["IrrigatorID"]))
                    {
                        IrrigatorID = Convert.ToInt64(Request.QueryString["IrrigatorID"]);
                        BindIrrigatorData(IrrigatorID);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.IrrigatorFeedbackHistory] != null)
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

        private void BindIrrigatorData(long _IrrigatorID)
        {
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            IF_Irrigator mdlIrrigatorFeedback = bllIrrigatorFeedback.GetIrrigatorByID(_IrrigatorID);

            //lblZone.Text = mdlIrrigatorFeedback.CO_Division.CO_Circle.CO_Zone.Name;
            //lblCircle.Text = mdlIrrigatorFeedback.CO_Division.CO_Circle.Name;
            lblDivision.Text = mdlIrrigatorFeedback.CO_Division.Name;
            lblChannelName.Text = mdlIrrigatorFeedback.CO_Channel.NAME;
            lblIrrigatorName.Text = mdlIrrigatorFeedback.Name;
            //lblMobileNo1.Text = mdlIrrigatorFeedback.MobileNo1;
        }

        /// <summary>
        /// this function bind indent history with grid based on Channel ID, SubDiv ID, From Date, To Date
        /// Created On: 02/6/2016
        /// </summary>
        private void IrrigatorsFeedbackHistory()
        {
            List<dynamic> SearchCriteria = new List<dynamic>();

            IrrigatorID = Convert.ToInt64(Request.QueryString["IrrigatorID"]);

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (txtFromDate.Text != String.Empty)
            {
                FromDate = DateTime.Parse(txtFromDate.Text);
            }

            if (txtToDate.Text != String.Empty)
            {
                ToDate = DateTime.Parse(txtToDate.Text);
            }
            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                gvFeedbackHistory.Visible = false;
                return;
            }

            if (FromDate > DateTime.Now || ToDate > DateTime.Now)
            {
                Master.ShowMessage(Message.FromAndToDateCannotBeGreaterThanCurrentDate.Description, SiteMaster.MessageType.Error);
                gvFeedbackHistory.Visible = false;
                return;
            }


            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            List<IF_IrrigatorFeedback> lstChannelIndents = bllIrrigatorFeedback.GetFeedbackHistoryByIrrigatorID(IrrigatorID, FromDate, ToDate);
            gvFeedbackHistory.DataSource = lstChannelIndents;
            gvFeedbackHistory.DataBind();
            gvFeedbackHistory.Visible = true;

            SearchCriteria.Add(IrrigatorID);
            SearchCriteria.Add(txtFromDate.Text);
            SearchCriteria.Add(txtToDate.Text);
            SearchCriteria.Add(gvFeedbackHistory.PageIndex);
            Session[SessionValues.IrrigatorFeedbackHistory] = SearchCriteria;
        }

        private void BindGrid(long _IrrigatorID, DateTime _FromDate, DateTime _ToDate)
        {
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            List<IF_IrrigatorFeedback> lstChannelIndents = bllIrrigatorFeedback.GetFeedbackHistoryByIrrigatorID(_IrrigatorID, _FromDate, _ToDate);
            gvFeedbackHistory.DataSource = lstChannelIndents;
            gvFeedbackHistory.DataBind();
            gvFeedbackHistory.Visible = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                IrrigatorsFeedbackHistory();
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.IrrigatorFeedbackHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvFeedbackHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblFeedbackDate = (Label)e.Row.FindControl("lblFeedbackDate");
                    Label lblTailStatus = (Label)e.Row.FindControl("lblTailStatus");
                    Label lblRotationalViolation = (Label)e.Row.FindControl("lblRotationalViolation");
                    Label lblWaterTheft = (Label)e.Row.FindControl("lblWaterTheft");

                    lblFeedbackDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblFeedbackDate.Text));

                    //if (lblRotationalViolation.Text == "1")
                    //{
                    //    lblRotationalViolation.Text = "Yes";
                    //}
                    //else
                    //{
                    //    lblRotationalViolation.Text = "No";
                    //}

                    dynamic RotaionalViolation = CommonLists.GetYesNo(lblRotationalViolation.Text).FirstOrDefault<dynamic>();
                    if (RotaionalViolation != null)
                    {
                        lblRotationalViolation.Text = RotaionalViolation.GetType().GetProperty("Name").GetValue(RotaionalViolation, null);
                    }


                    //if (lblWaterTheft.Text == "1")
                    //{
                    //    lblWaterTheft.Text = "Yes";
                    //}
                    //else
                    //{
                    //    lblWaterTheft.Text = "No";
                    //}

                    dynamic WaterTheft = CommonLists.GetYesNo(lblWaterTheft.Text).FirstOrDefault<dynamic>();
                    if (WaterTheft != null)
                    {
                        lblWaterTheft.Text = WaterTheft.GetType().GetProperty("Name").GetValue(WaterTheft, null);
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
                    dynamic TailStatus = CommonLists.GetTailStatuses(lblTailStatus.Text).FirstOrDefault<dynamic>();
                    if (TailStatus != null)
                    {
                        lblTailStatus.Text = TailStatus.GetType().GetProperty("Name").GetValue(TailStatus, null);
                    }



                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFeedbackHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFeedbackHistory.PageIndex = e.NewPageIndex;
                IrrigatorsFeedbackHistory();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFeedbackHistory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvFeedbackHistory.EditIndex = -1;
                IrrigatorsFeedbackHistory();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindHistoryData()
        {
            List<dynamic> SearchCriteria = (List<dynamic>)Session[SessionValues.IrrigatorFeedbackHistory];

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (SearchCriteria[1] != String.Empty)
            {
                FromDate = DateTime.Parse(SearchCriteria[1]);
            }

            if (SearchCriteria[2] != String.Empty)
            {
                ToDate = DateTime.Parse(SearchCriteria[2]);
            }
            BindIrrigatorData(SearchCriteria[0]);
            txtFromDate.Text = SearchCriteria[1];
            txtToDate.Text = SearchCriteria[2];
            gvFeedbackHistory.PageIndex = (int)SearchCriteria[3];
            //IrrigatorsFeedbackHistory();
            IrrigatorFeedbackBLL bllIrrigatorFeedback = new IrrigatorFeedbackBLL();
            List<IF_IrrigatorFeedback> lstChannelIndents = bllIrrigatorFeedback.GetFeedbackHistoryByIrrigatorID(Convert.ToInt64(SearchCriteria[0]), FromDate, ToDate);
            gvFeedbackHistory.DataSource = lstChannelIndents;
            gvFeedbackHistory.DataBind();
            gvFeedbackHistory.Visible = true;
        }
    }
}