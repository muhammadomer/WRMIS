using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.ScheduleInspection;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddSchedule : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleID"]))
                    {
                        hdnScheduleID.Value = Convert.ToString(Request.QueryString["ScheduleID"]);
                        btnSave.Visible = base.CanEdit;
                        GetSavedInformation(Convert.ToInt64(Request.QueryString["ScheduleID"]));
                    }
                    else
                    {
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        btnSave.Visible = base.CanAdd;
                        //txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(+1)));
                        BindScreenControls();
                    }
                    hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchSchedule.aspx";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void GetSavedInformation(long _ScheduleID)
        {
            try
            {
                SI_Schedule Schedule = new ScheduleInspectionBLL().GetScheduleBasicInformation(_ScheduleID);
                if (Schedule != null)
                {
                    txtScheduleTitle.Text = Schedule.Name;
                    txtFromDate.Text = Utility.GetFormattedDate(Schedule.FromDate);
                    txtToDate.Text = Utility.GetFormattedDate(Schedule.ToDate);
                    txtTourDescripton.Text = Schedule.Description;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (Utility.GetParsedDate(txtFromDate.Text) >= DateTime.Now.Date && Utility.GetParsedDate(txtToDate.Text) >= DateTime.Now.Date)
                {
                    if (Utility.GetParsedDate(txtToDate.Text) >= Utility.GetParsedDate(txtFromDate.Text))
                    {
                        if (hdnScheduleID.Value == "0")  // add case
                        {
                            DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                            SI_Schedule Schedule = new SI_Schedule();
                            Schedule.Name = txtScheduleTitle.Text;
                            Schedule.FromDate = FromDate;
                            Schedule.ToDate = ToDate;
                            Schedule.Description = txtTourDescripton.Text;
                            Schedule.CreatedDate = DateTime.Now;
                            Schedule.CreatedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                            Schedule.ModifiedDate = DateTime.Now;
                            Schedule.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                            int ScheduleID = new ScheduleInspectionBLL().AddSchedule(Schedule, Convert.ToInt32(SessionManagerFacade.UserInformation.ID), Convert.ToInt32(SessionManagerFacade.UserInformation.DesignationID));

                            if (ScheduleID >= 0)
                            {
                                ScheduleDetail.IsSaved = true;
                                Response.Redirect("~/Modules/ScheduleInspection/ScheduleDetail.aspx?ScheduleID=" + Convert.ToInt64(ScheduleID), false);

                            }
                            else
                            {
                                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                            }
                        }
                        else  // edit case
                        {

                            SI_Schedule ScheduleData = new ScheduleInspectionBLL().GetScheduleBasicInformation(Convert.ToInt64(hdnScheduleID.Value));

                            DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                            SI_Schedule Schedule = new SI_Schedule();
                            Schedule.Name = txtScheduleTitle.Text;
                            Schedule.FromDate = FromDate;
                            Schedule.ToDate = ToDate;
                            Schedule.Description = txtTourDescripton.Text;
                            Schedule.CreatedDate = ScheduleData.CreatedDate;
                            Schedule.CreatedBy = ScheduleData.CreatedBy;
                            Schedule.ModifiedDate = DateTime.Now;
                            Schedule.ModifiedBy = Convert.ToInt64(Session[SessionValues.UserID]);
                            Schedule.ID = Convert.ToInt64(hdnScheduleID.Value);
                            bool Result = new ScheduleInspectionBLL().CheckScheduleDetailDates(Convert.ToInt64(hdnScheduleID.Value), FromDate, ToDate);
                            if (!Result)
                            {
                                bool isUpdated = new ScheduleInspectionBLL().UpdateSchedule(Schedule);
                                if (!isUpdated)
                                {
                                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                                }
                                else
                                {
                                    SearchSchedule.IsSaved = true;
                                    Response.Redirect("~/Modules/ScheduleInspection/SearchSchedule.aspx", false);
                                }
                            }
                            else
                                Master.ShowMessage(Message.DateRangeWasInUse.Description, SiteMaster.MessageType.Error);
                        }
                    }
                    else
                        Master.ShowMessage(Message.TODateCannotBeLessFromDate.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.TODateAndFromDate.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindScreenControls()
        {
            if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.DeputyDirector || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.SE)
            {
                txtScheduleTitle.CssClass = "form-control";
                txtScheduleTitle.Enabled = false;
                txtFromDate.CssClass = "form-control";
                txtFromDate.Enabled = false;
                txtToDate.CssClass = "form-control";
                txtToDate.Enabled = false;
                txtTourDescripton.CssClass = "form-control";
                txtTourDescripton.Enabled = false;
                btnSave.Visible = false; ;

            }

        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchSchedule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}