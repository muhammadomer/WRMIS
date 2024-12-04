using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddOutletPerformanceInspection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtInspectionDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        btnSave.Visible = base.CanAdd;
                        hdnScheduleDetailChannelID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        GetOutletInspection();
                        hlBack.NavigateUrl = string.Format("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + hdnScheduleID.Value);                       
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddOutletPerformanceInspectionNotes);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void GetOutletInspection()
        {
            try
            {
                dynamic bllOutletInspection = new ScheduleInspectionBLL().GetOutletInspection(Convert.ToInt64(hdnScheduleDetailChannelID.Value), (long)Constants.SIInspectionType.OutletPerformance);

                OutletPerformanceInspection.ScheduleTitle = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleTitle");
                OutletPerformanceInspection.ScheduleStatus = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleStatus");
                OutletPerformanceInspection.PreparedBy = Utility.GetDynamicPropertyValue(bllOutletInspection, "PreparedBy");
                OutletPerformanceInspection.FromDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "FromDate");
                OutletPerformanceInspection.ToDate = Utility.GetDynamicPropertyValue(bllOutletInspection, "ToDate");
                OutletPerformanceInspection.ChannelName = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelName");
                OutletPerformanceInspection.OutletName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletRDChannelSide");
                OutletPerformanceInspection.OutletTypeName = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeName");
                hdnChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ChannelID");
                hdnOutletID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletID");
                hdnOutletTypeID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "OutletTypeID");
                hdnScheduleDetailChannelID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleDetailChannelID");

                hdnDischarge.Value = Convert.ToString(new ScheduleInspectionBLL().GetOutletDischarge(Convert.ToInt64(hdnOutletID.Value)));
                hdnScheduleID.Value = Utility.GetDynamicPropertyValue(bllOutletInspection, "ScheduleID");
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CalculateEfficiency(string _Discharge, string _DesignDischarge)
        {
            string efficiency = string.Empty;
            if (!string.IsNullOrEmpty(_DesignDischarge))
            {
                double discharge = Convert.ToDouble(_DesignDischarge);
                if (discharge != 0)
                {
                    if (!string.IsNullOrEmpty(_Discharge))
                    {
                        efficiency = Math.Round(((Convert.ToDouble(_Discharge) / discharge) * 100), 2).ToString();
                    }
                    else
                    {
                        efficiency = string.Empty;
                    }
                }
                else
                {
                    efficiency = string.Empty;
                }
            }
            else
            {
                efficiency = string.Empty;
            }
            return efficiency;
        }

        protected void Check_InspectionDates(object sender, EventArgs e)
        {
            try
            {
                if (txtInspectionDate.Text != "")
                {
                    bool IsInspectionDateInValid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtInspectionDate.Text));
                    if (IsInspectionDateInValid)
                    {
                        Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtInspectionDate.Text = Now;
                    }
                }

            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtInspectionDate.Text != "")
            {
                bool IsInspectionDateInvalid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtInspectionDate.Text));
                if (IsInspectionDateInvalid)
                {
                    Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            double? OutletHeight = null;
            double? DBWidth = null;
            try
            {
                CO_ChannelOutletsPerformance outletPerformance = new CO_ChannelOutletsPerformance();
                outletPerformance.OutletID = Convert.ToInt64(hdnOutletID.Value);
                outletPerformance.IsActive = true;
                outletPerformance.CreatedDate = DateTime.Now;
                outletPerformance.CreatedBy = SessionManagerFacade.UserInformation.ID;
                outletPerformance.ObservationDate = Utility.GetParsedDate(txtInspectionDate.Text).Add(DateTime.Now.TimeOfDay);
                outletPerformance.HeadAboveCrest = Convert.ToDouble(txtHeadAboveCrest.Text);
                outletPerformance.WorkingHead = Convert.ToDouble(txtWorkingHead.Text);
                outletPerformance.ObservedHeightY = !string.IsNullOrEmpty(txtOutletHeight.Text) ? Convert.ToDouble(txtOutletHeight.Text) : OutletHeight;
                outletPerformance.ObservedWidthB = !string.IsNullOrEmpty(txtDBWidth.Text) ? Convert.ToDouble(txtDBWidth.Text) : DBWidth;
                outletPerformance.Discharge = Convert.ToDouble(txtDischarge.Text);
                outletPerformance.UserID = SessionManagerFacade.UserInformation.ID;
                outletPerformance.ScheduleDetailChannelID = Convert.ToInt64(hdnScheduleDetailChannelID.Value);
                outletPerformance.Source = Configuration.RequestSource.RequestFromWeb;
                outletPerformance.Remarks = txtComments.Text;
                outletPerformance.CreatedBy = SessionManagerFacade.UserInformation.ID;
                outletPerformance.CreatedDate = DateTime.Now;
                outletPerformance.ModifiedBy = SessionManagerFacade.UserInformation.ID;
                outletPerformance.ModifiedDate = DateTime.Now;

                bool isSaved = new ScheduleInspectionBLL().AddOutletPerformanceInspection(outletPerformance);
                if (isSaved)
                {
                    ScheduleInspectionNotes.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + hdnScheduleID.Value, false);
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}