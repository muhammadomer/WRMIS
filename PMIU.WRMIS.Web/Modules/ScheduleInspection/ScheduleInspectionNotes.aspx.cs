using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ScheduleInspectionNotes : BasePage
    {

        #region ViewState Constants

        string PreparedByIDString = "PreparedByID";
        string StatusIDString = "StatusID";
        string PreparedByDesignationIDString = "PreparedByDesignationID";
        public const string ScheduleID_VS = "ScheduleID";

        #endregion

        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }

        private static string _Message = "";
        public static string ActionMessage
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
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
                        if (_IsSaved)
                        {
                            if (_Message != "")
                            {
                                Master.ShowMessage(_Message, SiteMaster.MessageType.Success);
                            }
                            else
                            {
                                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            }
                            _Message = string.Empty;
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        long PreparedByID = 0;
                        long StatusID = 0;
                        BindScheduleData(Convert.ToInt64(hdnScheduleID.Value), ref PreparedByID, ref StatusID);
                        BindGaugeInspection();
                        BindDischargeInspection();
                        BindOutletAlteration();
                        BindOutletPerformance();
                        BindOutletChecking();
                        BindTendersMonitoring();
                        BindClosureOperations();
                        BindGeneralInspections();
                        hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchSchedule.aspx";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void BindScheduleData(long _ScheduleID, ref long _PreparedByID, ref long _StatusID)
        {
            try
            {
                dynamic ScheduleData = new ScheduleInspectionBLL().GetScheduleData(_ScheduleID);
                string Title = ScheduleData.GetType().GetProperty("Name").GetValue(ScheduleData, null);
                string Status = ScheduleData.GetType().GetProperty("Status").GetValue(ScheduleData, null);
                string PreparedBy = ScheduleData.GetType().GetProperty("PreparedBy").GetValue(ScheduleData, null);
                _PreparedByID = ScheduleData.GetType().GetProperty("PreparedByID").GetValue(ScheduleData, null);
                DateTime FromDate = ScheduleData.GetType().GetProperty("FromDate").GetValue(ScheduleData, null);
                DateTime ToDate = ScheduleData.GetType().GetProperty("ToDate").GetValue(ScheduleData, null);
                _StatusID = ScheduleData.GetType().GetProperty("StatusID").GetValue(ScheduleData, null);
                ViewState[PreparedByDesignationIDString] = ScheduleData.GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleData, null);
                ScheduleInspection.Controls.ScheduleDetail.Title = Title;
                ScheduleInspection.Controls.ScheduleDetail.Status = Status;
                ScheduleInspection.Controls.ScheduleDetail.PreparedBy = PreparedBy;
                ScheduleInspection.Controls.ScheduleDetail.FromDate = Utility.GetFormattedDate(FromDate);
                ScheduleInspection.Controls.ScheduleDetail.ToDate = Utility.GetFormattedDate(ToDate);
                hdnSchedulePreparedByID.Value = Convert.ToString(_PreparedByID);
                hdnScheduleStatusID.Value = Convert.ToString(_StatusID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public bool IsToDisplayInspectionLink()
        {
            bool Flag = true;
            try
            {
                if (SessionManagerFacade.UserInformation.ID != Convert.ToInt64(hdnSchedulePreparedByID.Value) && (Convert.ToInt64(hdnScheduleStatusID.Value) == (long)Constants.SIScheduleStatus.Approved || Convert.ToInt64(hdnScheduleStatusID.Value) == (long)Constants.SIScheduleStatus.Rejected))
                {
                    Flag = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Flag;
        }
        public void BindGaugeInspection()
        {
            try
            {
                gvGuage.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.GaugeReading);
                gvGuage.DataBind();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindDischargeInspection()
        {
            try
            {
                gvDischarge.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.DischargeTableCalculation);
                gvDischarge.DataBind();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindOutletAlteration()
        {
            try
            {
                gvOutlet.DataSource = new ScheduleInspectionBLL().GetOutletsExistingRecords(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletAlteration);
                gvOutlet.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindOutletPerformance()
        {
            try
            {
                gvOutPer.DataSource = new ScheduleInspectionBLL().GetOutletsExistingRecords(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletPerformance);
                gvOutPer.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindOutletChecking()
        {
            try
            {
                gvOutletChecking.DataSource = new ScheduleInspectionBLL().GetScheduleDetailByScheduleIDInspectionTypeID(Convert.ToInt64(hdnScheduleID.Value), (long)Constants.SIInspectionType.OutletChecking);
                gvOutletChecking.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDischarge_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvDischarge.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string GaugeID = Convert.ToString(key.Values[1]);
                        string GaugeVal = Convert.ToString(key.Values[2]);
                        var hyperlink = e.Row.FindControl("hlInspectionotesDischargeTable") as HyperLink;
                        if (Convert.ToInt32(GaugeVal) == 1)
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddDischargeTableCalcBL.aspx?ScheduleDetailID={0}", Convert.ToInt32(ScheduleDetailID));
                            var IsDuplicate = new ScheduleInspectionBLL().IsDischargeCalcBLRecordAlreadyExists(Convert.ToInt64(ScheduleDetailID),Convert.ToInt32(GaugeID));
                            if (IsDuplicate)
                            {
                                hyperlink.CssClass = "btn btn-success btn_24 view green";
                            }
                        }
                        else if (Convert.ToInt32(GaugeVal) == 2)
                        {
                            var IsDuplicate = new ScheduleInspectionBLL().IsDischargeCalcCLRecordAlreadyExists(Convert.ToInt64(ScheduleDetailID),Convert.ToInt32(GaugeID));
                            if (IsDuplicate)
                            {
                                hyperlink.CssClass = "btn btn-success btn_24 view green";
                            }
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddDischargeTableCalcCL.aspx?ScheduleDetailID={0}", Convert.ToInt32(ScheduleDetailID));
                        }


                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvGuage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvGuage.DataKeys[e.Row.RowIndex];
                        string SchedulaDetailID = Convert.ToString(key.Values[0]);
                        string GaugeID = Convert.ToString(key.Values[1]);

                        var IsDuplicate = new ScheduleInspectionBLL().IsGaugeRecordAlreadyExists(Convert.ToInt64(SchedulaDetailID), Convert.ToInt32(GaugeID));
                        if (IsDuplicate)
                        {
                            var hyperlink = e.Row.FindControl("lblInspectionNotesGaugeReading") as HyperLink;
                            hyperlink.CssClass = "btn btn-success btn_24 view green";
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvOutlet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvOutlet.DataKeys[e.Row.RowIndex];
                        string SchedulaDetailID = Convert.ToString(key.Values[0]);
                        string OutletID = Convert.ToString(key.Values[1]);
                        var hyperlink = e.Row.FindControl("gvSOutletAltIN") as HyperLink;
                        var IsDuplicate = new ScheduleInspectionBLL().IsOutletAlterationRecordAlreadyExists(Convert.ToInt64(SchedulaDetailID), Convert.ToInt32(OutletID));
                        if (IsDuplicate)
                        {

                            hyperlink.CssClass = "btn btn-success btn_24 view green";
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID=" + Convert.ToInt64(SchedulaDetailID) + "&Type=false");
                        }
                        else
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddOutletAlterationInspection.aspx?ScheduleDetailID=" + Convert.ToInt64(SchedulaDetailID)); 
                        }
                    }
                    }
                }
            
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvOutPer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvOutPer.DataKeys[e.Row.RowIndex];
                        string SchedulaDetailID = Convert.ToString(key.Values[0]);
                        string OutletID = Convert.ToString(key.Values[1]);
                        var hyperlink = e.Row.FindControl("gvINOPR") as HyperLink;
                        var IsDuplicate = new ScheduleInspectionBLL().IsOutletPerformanceRecordAlreadyExists(Convert.ToInt64(SchedulaDetailID), Convert.ToInt32(OutletID));
                        if (IsDuplicate)
                        {

                            hyperlink.CssClass = "btn btn-success btn_24 view green";
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID=" + Convert.ToInt64(SchedulaDetailID) + "&Type=true");
                        }
                        else
                        {
                            hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddOutletPerformanceInspection.aspx?ScheduleDetailID=" + Convert.ToInt64(SchedulaDetailID)); 
                        }
                    }
                    }
                }
            
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvOutletChecking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvOutletChecking.DataKeys[e.Row.RowIndex];
                        string SchedulaDetailID = Convert.ToString(key.Values[0]);
                        string OutletID = Convert.ToString(key.Values[1]);
                        string ChannelID= Convert.ToString(key.Values[2]);
                        string OutletcheckingID = Convert.ToString(key.Values[3]);
                        long ScheduleID = 0;
                        if (!string.IsNullOrEmpty(Request.QueryString["ScheduleID"]))
                        {
                            ScheduleID = Convert.ToInt64(Request.QueryString["ScheduleID"]);
                        }
                        if (!OutletcheckingID.Equals("0"))
                        {
                            var hyperlink = e.Row.FindControl("gvOChl") as HyperLink;
                            hyperlink.CssClass = "btn btn-success btn_24 view green";
                            string url = String.Format("OutletChecking.aspx?OutletCheckingID={0}&Outlet={1}&ScheduleID={2}&From={3}", OutletcheckingID, OutletID, ScheduleID,"IN");
                            hyperlink.NavigateUrl = url; // String.Format("../WaterTheft/AddWaterTheft.aspx?ScheduleDetailID={0}&ChannelID={1}&Outlet={2}", SchedulaDetailID,ChannelID,OutletID);    
                        }
                        
                    }
                }
            }

            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        
        //public string GetOAParameterPageURL(bool _OutletRecordAlreadyExists, long _ScheduleID)
        //{
        //    string URL = string.Empty;
        //    try
        //    {
        //        if (_OutletRecordAlreadyExists)
        //        {
        //            URL = "~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID=" + _ScheduleID + "&Type=false";

        //        }
        //        else
        //        {
        //            URL = "~/Modules/ScheduleInspection/AddOutletAlterationInspection.aspx?ScheduleDetailID=" + _ScheduleID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //    return URL;
        //}

        //public string GetClassValue(bool _OutletRecordAlreadyExists)
        //{
        //    string ClassVal = string.Empty;
        //    try
        //    {
        //        if (_OutletRecordAlreadyExists)
        //        {
        //            ClassVal = "green";

        //        }
        //        else
        //        {
        //            ClassVal = "blue";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //    return ClassVal;
        //}

        //public string GetOutletPerformanceParameterPageURL(bool _OutletRecordAlreadyExists, long _ScheduleID)
        //{
        //    string URL = string.Empty;
        //    try
        //    {
        //        if (_OutletRecordAlreadyExists)
        //        {
        //            URL = "~/Modules/ScheduleInspection/ViewOutletInspection.aspx?ScheduleDetailID=" + _ScheduleID + "&Type=true";
        //        }
        //        else
        //        {
        //            URL = "~/Modules/ScheduleInspection/AddOutletPerformanceInspection.aspx?ScheduleDetailID=" + _ScheduleID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //    return URL;
        //}

        public void BindTendersMonitoring()
        {
            try
            {
                gvTenderMonitoring.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForTenderMonitoring(Convert.ToInt64(hdnScheduleID.Value));
                gvTenderMonitoring.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindClosureOperations()
        {
            try
            {
                gvClosureOperations.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForClosureOperations(Convert.ToInt64(hdnScheduleID.Value));
                gvClosureOperations.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindGeneralInspections()
        {
            try
            {
                gvGeneralInspections.DataSource = new ScheduleInspectionBLL().GetScheduleDetailForGeneralInspections(Convert.ToInt64(hdnScheduleID.Value));
                gvGeneralInspections.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvTenderMonitoring_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvTenderMonitoring.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string TenderNoticeID = Convert.ToString(key.Values[1]);
                        string TenderWorksID = Convert.ToString(key.Values[2]);
                        string WorkSourceID = Convert.ToString(key.Values[3]);
                        var hyperlink = e.Row.FindControl("hlTenders") as HyperLink;
                        //var IsDuplicate = new ScheduleInspectionBLL().IsADMRecordExists(Convert.ToInt64(TenderWorksID));
                        //if (IsDuplicate)
                        //{
                        //    hyperlink.CssClass = "btn btn-success btn_24 view green";
                        //    if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM)
                        //    {
                        //        hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorksID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                        //    }
                        //    else if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                        //    {
                        //        hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorksID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                        //    }
                           
                            
                        //}
                        //else
                        //{
                            if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM)
                            {
                                hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorksID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                            }
                            else if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                            {
                                hyperlink.NavigateUrl = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(TenderWorksID) + "&WorkSourceID=" + Convert.ToInt64(WorkSourceID));
                            }
                            
                            
                        //}
                    }
                }
            }

            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvClosureOperations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvClosureOperations.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string WorkSourceID = Convert.ToString(key.Values[1]);
                        string WorkSource = Convert.ToString(key.Values[2]);
                        string CWPID = Convert.ToString(key.Values[3]);
                        string RefMonitoringID = Convert.ToString(key.Values[4]);
                        var hyperlink = e.Row.FindControl("hlClosure") as HyperLink;
                        //var IsDuplicate = new ScheduleInspectionBLL().IsClosureProgressExists(Convert.ToInt64(ScheduleDetailID));
                        if (WorkSource.ToUpper() == "CLOSURE")
                        {
                            if (!string.IsNullOrEmpty(RefMonitoringID))
                            {
                                hyperlink.CssClass = "btn btn-success btn_24 view green";
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                                {
                                    hyperlink.NavigateUrl = string.Format("~/Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(WorkSourceID) + "&CWPID=" + Convert.ToInt64(CWPID) + "&RefMonitoringID=" + Convert.ToInt64(RefMonitoringID));
                                }



                            }
                            else
                            {
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                                {
                                    hyperlink.NavigateUrl = string.Format("~/Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + Convert.ToInt64(WorkSourceID) + "&CWPID=" + Convert.ToInt64(CWPID) + "&ScheduleDetailID=" + Convert.ToInt64(ScheduleDetailID));
                                }



                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(RefMonitoringID))
                            {
                                hyperlink.CssClass = "btn btn-success btn_24 view green";
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                                {
                                    hyperlink.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Works/AddWorkProgress.aspx?AWID=" + Convert.ToInt64(WorkSourceID) + "&RefMonitoringID=" + Convert.ToInt64(RefMonitoringID));
                                }



                            }
                            else
                            {
                                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                                {
                                    hyperlink.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/Works/AddWorkProgress.aspx?AWID=" + Convert.ToInt64(WorkSourceID) + "&ScheduleDetailID=" + Convert.ToInt64(ScheduleDetailID));
                                }



                            }
                        }
                      
                    }
                }
            }

            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvGeneralInspections_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != -1)
                    {
                        DataKey key = gvGeneralInspections.DataKeys[e.Row.RowIndex];
                        string ScheduleDetailID = Convert.ToString(key.Values[0]);
                        string IsInspected = Convert.ToString(key.Values[5]);
                        var hyperlink = e.Row.FindControl("hlGeneralInspections") as HyperLink;
                        if (Convert.ToBoolean(IsInspected))
                        {
                            hyperlink.CssClass = "btn btn-success btn_24 view green";
                        }
                        else
                        {
                            hyperlink.CssClass = "btn btn-primary btn_24 edit";
                        }
                            
                            //if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                            //{
                                hyperlink.NavigateUrl = string.Format("~/Modules/ScheduleInspection/AddGeneralInspections.aspx?ScheduleDetailID=" + Convert.ToInt64(ScheduleDetailID) + "&IsViewMode=" + Convert.ToBoolean(IsInspected)+"&Source=N");
                           // }
                        }
                }
            }

            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        public bool IsToDisplayInspectionLinkTender()
        {
            bool Flag = false;
            try
            {
                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                {
                    if (Convert.ToInt64(hdnScheduleStatusID.Value) == (long)Constants.SIScheduleStatus.Approved)
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                    
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Flag;
        }
   public bool IsToDisplayInspectionLinkGeneral()
        {
            bool Flag = false;
            try
            {
                //if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ADM || SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.XEN)
                //{
                    if (Convert.ToInt64(hdnScheduleStatusID.Value) == (long)Constants.SIScheduleStatus.Approved && SessionManagerFacade.UserInformation.ID == Convert.ToInt64(hdnSchedulePreparedByID.Value))
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                    
               // }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Flag;
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddInspectionNotes);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

       
    }
}