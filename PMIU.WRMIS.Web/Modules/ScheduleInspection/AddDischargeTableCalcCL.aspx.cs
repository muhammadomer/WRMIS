using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddDischargeTableCalcCL : BasePage
    {
        #region Constants
        string PreparedByDesignationIDString = "PreparedByDesignationID";
        public const string ScheduleDetailID_VS = "ScheduleDetailID";
        public const string GaugeID_VS = "GaugeID";
        public const string ScheduleID = "ScheduleID";
        public const string PreparedByID = "PreparedByID";
        public const string DesignationID = "DesignationID";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtObservationdate.Text = Utility.GetFormattedDate(DateTime.Now);
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        bool? IsScheduled = null;
                        Session[ScheduleDetailID_VS] = Convert.ToInt64(Request.QueryString["ScheduleDetailID"]);
                        long PreparedByID = 0;
                        long StatusID = 0;
                        txtCoefficientofDischarge.Enabled = false;

                        if (!string.IsNullOrEmpty(Request.QueryString["IsScheduled"]))
                        {
                            IsScheduled = Convert.ToBoolean(Request.QueryString["IsScheduled"]);
                            ViewState["IsScheduled"] = IsScheduled;
                            hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                            //hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchInspection.aspx";
                            BindScheduleDetailData(Convert.ToInt64(Session[ScheduleDetailID_VS]), ref PreparedByID, ref StatusID, IsScheduled);
                        }
                        else
                        {
                            BindScheduleDetailData(Convert.ToInt64(Session[ScheduleDetailID_VS]), ref PreparedByID, ref StatusID, IsScheduled);
                            hlBack.NavigateUrl = "~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value);
                        }



                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void BindScheduleDetailData(long _ScheduleDetailID, ref long _PreparedByID, ref long _StatusID, bool? IsScheduled)
        {
            try
            {
                if (null == IsScheduled || IsScheduled == true)
                {
                    dynamic ScheduleDetailData = new ScheduleInspectionBLL().GetScheduleDetailDataCL(_ScheduleDetailID);
                    string Title = ScheduleDetailData[0].GetType().GetProperty("Name").GetValue(ScheduleDetailData[0], null);
                    string Status = ScheduleDetailData[0].GetType().GetProperty("Status").GetValue(ScheduleDetailData[0], null);
                    string PreparedBy = ScheduleDetailData[0].GetType().GetProperty("PreparedBy").GetValue(ScheduleDetailData[0], null);
                    Session[PreparedByID] = ScheduleDetailData[0].GetType().GetProperty("PreparedByID").GetValue(ScheduleDetailData[0], null);
                    DateTime FromDate = ScheduleDetailData[0].GetType().GetProperty("FromDate").GetValue(ScheduleDetailData[0], null);
                    ViewState["FromDate"] = FromDate;
                    DateTime ToDate = ScheduleDetailData[0].GetType().GetProperty("ToDate").GetValue(ScheduleDetailData[0], null);
                    string ChannelName = ScheduleDetailData[0].GetType().GetProperty("ChannelName").GetValue(ScheduleDetailData[0], null);
                    string InspectionArea = ScheduleDetailData[0].GetType().GetProperty("InspectionArea").GetValue(ScheduleDetailData[0], null);
                    string InspectedBy = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("InspectedBy").GetValue(ScheduleDetailData[0], null));
                    long InspectedByID = Convert.ToInt64(ScheduleDetailData[0].GetType().GetProperty("InspectedByID").GetValue(ScheduleDetailData[0], null));
                    ViewState["InspectedByID"] = Convert.ToInt64(InspectedByID);
                    _StatusID = ScheduleDetailData[0].GetType().GetProperty("StatusID").GetValue(ScheduleDetailData[0], null);
                    ViewState[PreparedByDesignationIDString] = ScheduleDetailData[0].GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleDetailData[0], null);
                    Session[GaugeID_VS] = ScheduleDetailData[0].GetType().GetProperty("GaugeID").GetValue(ScheduleDetailData[0], null);
                    hdnScheduleID.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("ScheduleID").GetValue(ScheduleDetailData[0], null));
                    hdnCreatedDate.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("CreatedDate").GetValue(ScheduleDetailData[0], null));
                    hdnSource.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("Source").GetValue(ScheduleDetailData[0], null));
                    bool IsRecordAlreadyExists = new ScheduleInspectionBLL().IsDischargeCalcCLRecordAlreadyExists(_ScheduleDetailID, Convert.ToInt32(Session[GaugeID_VS]));
                    if (IsRecordAlreadyExists)
                    {
                        CO_ChannelGaugeDTPFall data = new ScheduleInspectionBLL().GetDischargeCalcCLDatabyID(Convert.ToInt32(Session[GaugeID_VS]), _ScheduleDetailID);
                        if (SessionManagerFacade.UserInformation.UA_Designations != null)
                        {
                            if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                            {
                                var date = Utility.GetFormattedDate(data.ReadingDate);
                                var Time = Utility.GetFormattedTime(data.ObservationDate.Value);
                                if (Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]) == 0 &&
                                    data.ReadingDate == Convert.ToDateTime("01/01/1950 0:00:00"))
                                {
                                    //txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                    txtCurrentDate.Text = "Pending for approval";
                                    txtCurrentDate.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    txtCurrentDate.Text = Utility.GetFormattedDate(data.ReadingDate);
                                }

                                txtCurrentDate.Enabled = false;
                                txtCurrentDate.CssClass = "aspNetDisabled form-control";
                                txtInspectionTime.Text = Convert.ToString(Time);
                                txtInspectionTime.Visible = true;
                                lblInspectionTime.Text = "Inspection Time";
                                txtInspectionTime.Enabled = false;
                                lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                                txtBrdthofFall.Text = Convert.ToString(data.BreadthFall);
                                txtBrdthofFall.Enabled = false;
                                txtBrdthofFall.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtCoefficientofDischarge.Text = Convert.ToString(data.DischargeCoefficient);
                                txtCoefficientofDischarge.Enabled = false;
                                txtHeadabvCrest.Text = Convert.ToString(data.HeadAboveCrest);
                                txtHeadabvCrest.Enabled = false;
                                txtHeadabvCrest.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtObservedDischarge.Text = Convert.ToString(data.DischargeObserved);
                                txtObservedDischarge.Enabled = false;
                                txtObservedDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtRemarks.Text = Convert.ToString(data.Remarks);
                                txtRemarks.Enabled = false;
                                txtObservationdate.Text = Utility.GetFormattedDate(data.ObservationDate);
                                txtObservationdate.Enabled = false;
                                txtObservationdate.CssClass = "aspNetDisabled form-control decimal2PInput";
                                btnSave.CssClass += "btn btn-primary disabled";
                            }
                            else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                            {
                                var date = Utility.GetFormattedDate(data.ReadingDate);
                                var Time = Utility.GetFormattedTime(data.ObservationDate.Value);
                                long CO_ChannelGaugeDTPFall = Convert.ToInt64(data.ID);
                                ViewState["CO_ChannelGaugeDTPFallID"] = Convert.ToInt64(CO_ChannelGaugeDTPFall);
                                if (Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]) != 0 &&
                                    data.ReadingDate != Convert.ToDateTime("01/01/1950 0:00:00"))
                                {
                                    btnSave.Enabled = false;
                                    txtCurrentDate.Text = Utility.GetFormattedDate(data.ReadingDate);

                                }
                                else
                                {
                                    btnSave.Enabled = true;
                                    txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                }

                                txtCurrentDate.Enabled = true;
                                txtCurrentDate.CssClass = "form-control date-picker required";
                                txtCurrentDate.Attributes.Add("required", "required");
                                txtCurrentDate.ReadOnly = false;
                                txtInspectionTime.Text = Convert.ToString(Time);
                                txtInspectionTime.Visible = true;
                                lblInspectionTime.Text = "Inspection Time";
                                txtInspectionTime.Enabled = true;
                                lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                                txtBrdthofFall.Text = Convert.ToString(data.BreadthFall);
                                txtBrdthofFall.Enabled = true;
                                txtBrdthofFall.CssClass = "form-control decimal2PInput";
                                txtCoefficientofDischarge.Text = Convert.ToString(data.DischargeCoefficient);
                                txtCoefficientofDischarge.Enabled = true;
                                txtHeadabvCrest.Text = Convert.ToString(data.HeadAboveCrest);
                                txtHeadabvCrest.Enabled = true;
                                txtHeadabvCrest.CssClass = "form-control decimal2PInput";
                                txtObservedDischarge.Text = Convert.ToString(data.DischargeObserved);
                                txtObservedDischarge.Enabled = true;
                                txtObservedDischarge.CssClass = "form-control decimal2PInput";
                                txtRemarks.Text = Convert.ToString(data.Remarks);
                                txtRemarks.Enabled = true;
                                txtObservationdate.Text = Utility.GetFormattedDate(data.ObservationDate);
                                txtObservationdate.Enabled = false;
                                txtObservationdate.CssClass = "aspNetDisabled form-control";
                                btnSave.CssClass += "btn btn-primary";
                            }
                            else
                            {
                                string Now = Utility.GetFormattedDate(DateTime.Now);
                                txtCurrentDate.Text = Now;
                                txtCurrentDate.Enabled = false; // changed by Rizwan sb 
                                btnSave.CssClass += "btn btn-primary disabled";
                            }
                        }
                        else
                        {
                            string Now = Utility.GetFormattedDate(DateTime.Now);
                            txtCurrentDate.Text = Now;
                            txtCurrentDate.Enabled = false; // changed by Rizwan sb 
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                    }
                    else
                    {
                        //Current date for textbox
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtCurrentDate.Enabled = false;
                        txtCurrentDate.Text = Now;
                        btnSave.CssClass += "btn btn-primary";
                    }
                    ScheduleInspection.Controls.GaugeInspection.Title = Title;
                    ScheduleInspection.Controls.GaugeInspection.Status = Status;
                    ScheduleInspection.Controls.GaugeInspection.PreparedBy = PreparedBy;
                    ScheduleInspection.Controls.GaugeInspection.ChannelName = ChannelName;
                    ScheduleInspection.Controls.GaugeInspection.InspectionArea = InspectionArea;
                    ScheduleInspection.Controls.GaugeInspection.FromDate = Utility.GetFormattedDate(FromDate);
                    ScheduleInspection.Controls.GaugeInspection.ToDate = Utility.GetFormattedDate(ToDate);
                    ScheduleInspection.Controls.GaugeInspection.InspectedBy = InspectedBy == "" ? "N/A" : InspectedBy;
                }
                else
                {
                    dynamic DischargeData = new ScheduleInspectionBLL().GetGaugeChannelByDischargeCLID(Convert.ToInt32(Convert.ToInt32(_ScheduleDetailID)));
                    ScheduleInspection.Controls.GaugeInspection.Title = "Unscheduled Inspection";
                    ScheduleInspection.Controls.GaugeInspection.Status = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.PreparedBy = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ChannelName = Convert.ToString(DischargeData.GetType().GetProperty("ChannelName").GetValue(DischargeData, null));
                    ScheduleInspection.Controls.GaugeInspection.InspectionArea = Convert.ToString(DischargeData.GetType().GetProperty("InspectionRD").GetValue(DischargeData, null));
                    ScheduleInspection.Controls.GaugeInspection.FromDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ToDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.InspectedBy = Convert.ToString(DischargeData.GetType().GetProperty("CreatedBy").GetValue(DischargeData, null));
                    hdnCreatedDate.Value = Convert.ToString(DischargeData.GetType().GetProperty("CreatedDate").GetValue(DischargeData, null));

                    CO_ChannelGaugeDTPFall data = new ScheduleInspectionBLL().GetDischargeCalcCLDatabyDischargeID(Convert.ToInt32(_ScheduleDetailID));
                    Session[GaugeID_VS] = data.GaugeID;
                    if (SessionManagerFacade.UserInformation.UA_Designations != null)
                    {
                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                        {
                            var date = Utility.GetFormattedDate(data.ReadingDate);
                            var Time = Utility.GetFormattedTime(data.ObservationDate.Value);
                            if (Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]) == 0 && data.ReadingDate == Convert.ToDateTime("01/01/1950 0:00:00"))
                            {
                                txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            }
                            else
                            {
                                txtCurrentDate.Text = Utility.GetFormattedDate(data.ReadingDate);
                            }
                            //  txtCurrentDate.Text = Convert.ToString(date);
                            txtCurrentDate.Enabled = false;
                            txtInspectionTime.Text = Convert.ToString(Time);
                            txtInspectionTime.Visible = true;
                            lblInspectionTime.Text = "Inspection Time";
                            txtInspectionTime.Enabled = false;
                            lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                            txtBrdthofFall.Text = Convert.ToString(data.BreadthFall);
                            txtBrdthofFall.Enabled = false;
                            txtBrdthofFall.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtCoefficientofDischarge.Text = Convert.ToString(data.DischargeCoefficient);
                            txtCoefficientofDischarge.Enabled = false;
                            txtHeadabvCrest.Text = Convert.ToString(data.HeadAboveCrest);
                            txtHeadabvCrest.Enabled = false;
                            txtHeadabvCrest.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtObservedDischarge.Text = Convert.ToString(data.DischargeObserved);
                            txtObservedDischarge.Enabled = false;
                            txtObservedDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtRemarks.Text = Convert.ToString(data.Remarks);
                            txtRemarks.Enabled = false;
                            txtObservationdate.Text = Utility.GetFormattedDate(data.ObservationDate);
                            txtObservationdate.Enabled = false;
                            txtObservationdate.CssClass = "aspNetDisabled form-control";
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                        else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            var date = Utility.GetFormattedDate(data.ReadingDate);
                            var Time = Utility.GetFormattedTime(data.ObservationDate.Value);
                            long CO_ChannelGaugeDTPFall = Convert.ToInt64(data.ID);
                            ViewState["CO_ChannelGaugeDTPFallID"] = Convert.ToInt64(CO_ChannelGaugeDTPFall);
                            if (Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]) != 0 && data.ReadingDate != Convert.ToDateTime("01/01/1950 0:00:00"))
                            {
                                btnSave.Enabled = false;
                                txtCurrentDate.Text = Utility.GetFormattedDate(data.ReadingDate);

                            }
                            else
                            {
                                btnSave.Enabled = true;
                                txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            }
                            txtCurrentDate.Text = Convert.ToString(date);
                            txtCurrentDate.Enabled = true;
                            txtInspectionTime.Text = Convert.ToString(Time);
                            txtInspectionTime.Visible = true;
                            lblInspectionTime.Text = "Inspection Time";
                            txtInspectionTime.Enabled = true;
                            lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                            txtBrdthofFall.Text = Convert.ToString(data.BreadthFall);
                            txtBrdthofFall.Enabled = true;
                            txtBrdthofFall.CssClass = "form-control decimal2PInput";
                            txtCoefficientofDischarge.Text = Convert.ToString(data.DischargeCoefficient);
                            txtCoefficientofDischarge.Enabled = true;
                            // txtCoefficientofDischarge.ReadOnly = false;
                            txtHeadabvCrest.Text = Convert.ToString(data.HeadAboveCrest);
                            txtHeadabvCrest.Enabled = true;
                            txtHeadabvCrest.CssClass = "form-control decimal2PInput";
                            txtObservedDischarge.Text = Convert.ToString(data.DischargeObserved);
                            txtObservedDischarge.Enabled = true;
                            txtObservedDischarge.CssClass = "form-control decimal2PInput";
                            txtRemarks.Text = Convert.ToString(data.Remarks);
                            txtRemarks.Enabled = true;
                            txtObservationdate.Text = Utility.GetFormattedDate(data.ObservationDate);
                            txtObservationdate.Enabled = false;
                            txtObservationdate.CssClass = "aspNetDisabled form-control";
                            btnSave.CssClass += "btn btn-primary";
                            btnSave.Text = "Approve";
                        }
                        else
                        {
                            string Now = Utility.GetFormattedDate(DateTime.Now);
                            txtCurrentDate.Text = Now;
                            txtCurrentDate.Enabled = false; // changed by Rizwan sb 
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                    }
                    else
                    {
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtCurrentDate.Text = Now;
                        txtCurrentDate.Enabled = false; // changed by Rizwan sb 
                        btnSave.CssClass += "btn btn-primary disabled";
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// This function validates the date and returns model object with validated date.
        /// Created on 18-11-2015
        /// </summary>
        /// <returns>CO_ChannelGaugeDTPFall</returns>
        private CO_ChannelGaugeDTPFall GetValidatedDate()
        {
            try
            {
                CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = new CO_ChannelGaugeDTPFall();

                DateTime ReadingDate = Utility.GetParsedDate(txtCurrentDate.Text.Trim());
                ReadingDate = ReadingDate.Add(DateTime.Now.TimeOfDay);
                mdlChannelGaugeDTFall.ReadingDate = ReadingDate;
                if (mdlChannelGaugeDTFall.ReadingDate < DateTime.Now.Date)
                {
                    Master.ShowMessage(Message.OldDatesNotAllowed.Description, SiteMaster.MessageType.Error);
                    return null;
                }

                return mdlChannelGaugeDTFall;
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCurrentDate.Text != "")
                {
                    if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                    {
                        bool IsInspectionDateInvalid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtCurrentDate.Text));
                        if (IsInspectionDateInvalid)
                        {
                            Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }

                }
                //CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = GetValidatedDate();
                CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = new CO_ChannelGaugeDTPFall();
                DateTime ReadingDate = Utility.GetParsedDate(txtCurrentDate.Text.Trim());
                DateTime ObservationDate = Utility.GetParsedDate(txtObservationdate.Text.Trim());
                string ObservationTime = Utility.GetFormattedTime(DateTime.Now);
                ReadingDate = ReadingDate.Add(DateTime.Now.TimeOfDay);
                mdlChannelGaugeDTFall.ID = Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]);

                //mdlChannelGaugeDTFall.ReadingDate = ReadingDate;
                if (Convert.ToInt64(ViewState["CO_ChannelGaugeDTPFallID"]) == 0)
                {
                    ReadingDate = Convert.ToDateTime("01/01/1950 0:00:00");
                    mdlChannelGaugeDTFall.ReadingDate = ReadingDate;
                    if (ObservationDate > DateTime.Now.Date)
                    {
                        Master.ShowMessage("Observation Date should be less than or equal to Inspection date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    if (ObservationDate < Convert.ToDateTime(ViewState["FromDate"]))
                    {
                        Master.ShowMessage("Observation Date should be greater than or equal to From date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdlChannelGaugeDTFall.CreatedBy = SessionManagerFacade.UserInformation.ID;
                    mdlChannelGaugeDTFall.ObservationDate = Utility.GetParsedDateTime(txtObservationdate.Text, ObservationTime);
                    mdlChannelGaugeDTFall.CreatedDate = DateTime.Now;

                }
                else
                {
                    mdlChannelGaugeDTFall.ReadingDate = ReadingDate;
                    if (ReadingDate < DateTime.Now.Date)
                    {
                        Master.ShowMessage("Inspection Date should be greater than or equal to Current date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdlChannelGaugeDTFall.CreatedBy = Convert.ToInt64(ViewState["InspectedByID"]);
                    mdlChannelGaugeDTFall.ObservationDate = Utility.GetParsedDateTime(txtObservationdate.Text, ObservationTime);
                    mdlChannelGaugeDTFall.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                    mdlChannelGaugeDTFall.ModifiedDate = DateTime.Now;
                    mdlChannelGaugeDTFall.IsActive = true;
                    mdlChannelGaugeDTFall.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }
                //if (mdlChannelGaugeDTFall == null)
                //{
                //    return;
                //}
                mdlChannelGaugeDTFall.BreadthFall = Convert.ToDouble(txtBrdthofFall.Text.Trim());
                mdlChannelGaugeDTFall.HeadAboveCrest = Convert.ToDouble(txtHeadabvCrest.Text.Trim());
                mdlChannelGaugeDTFall.DischargeObserved = Convert.ToDouble(txtObservedDischarge.Text.Trim());
                mdlChannelGaugeDTFall.GaugeID = Convert.ToInt32(Session[GaugeID_VS]);
                mdlChannelGaugeDTFall.Remarks = txtRemarks.Text;
                if (null == ViewState["IsScheduled"] || Convert.ToBoolean(ViewState["IsScheduled"]) == true)
                {
                    mdlChannelGaugeDTFall.ScheduleDetailChannelID = Convert.ToInt64(Session[ScheduleDetailID_VS]);
                }
                else
                {
                    mdlChannelGaugeDTFall.ScheduleDetailChannelID = null;
                }
                //mdlChannelGaugeDTFall.ScheduleDetailChannelID = Convert.ToInt64(Session[ScheduleDetailID_VS]);
                //mdlChannelGaugeDTFall.CreatedBy = SessionManagerFacade.UserInformation.ID;
                if (hdnSource.Value != Configuration.RequestSource.RequestFromMobile)
                    mdlChannelGaugeDTFall.Source = Configuration.RequestSource.RequestFromWeb;
                else
                    mdlChannelGaugeDTFall.Source = Configuration.RequestSource.RequestFromMobile;
                mdlChannelGaugeDTFall.DischargeCoefficient = Calculations.GetCrestCoefficientOfDischarge((double)mdlChannelGaugeDTFall.BreadthFall, mdlChannelGaugeDTFall.HeadAboveCrest,
                    mdlChannelGaugeDTFall.DischargeObserved);
                txtCoefficientofDischarge.Text = Convert.ToString(mdlChannelGaugeDTFall.DischargeCoefficient);
                ChannelBLL bllChannel = new ChannelBLL();
                bool IsRecordSaved = bllChannel.AddCrestLevelDTParameters(mdlChannelGaugeDTFall);
                if (IsRecordSaved)
                {
                    NotifyEvent _event = new NotifyEvent();
                    if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                    {
                        _event.Parameters.Add("GaugeID", mdlChannelGaugeDTFall.GaugeID);
                        _event.Parameters.Add("IsCrestParameters", "true");
                        _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditCrestLevelParameters, SessionManagerFacade.UserInformation.ID);
                    }

                    if (null == ViewState["IsScheduled"] || Convert.ToBoolean(ViewState["IsScheduled"]) == true)
                    {

                        _event.Parameters.Add("ScheduleDetailID", Convert.ToInt64(Session[ScheduleDetailID_VS]));
                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMCrestLevelIsApprovedByDD, SessionManagerFacade.UserInformation.ID);
                        }
                        else
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMCrestLevelAssignedToDDForApproval, SessionManagerFacade.UserInformation.ID);
                        }
                    }
                    else
                    {

                        _event.Parameters.Add("ScheduleDetailID", null);
                        _event.Parameters.Add("_GaugeID", Convert.ToInt32(Session[GaugeID_VS]));
                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCLisapprovedbyDD, SessionManagerFacade.UserInformation.ID);
                        }
                        //else
                        //{
                        //   _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMisapprovedbyDD, SessionManagerFacade.UserInformation.ID);
                        //}
                    }

                    ScheduleInspectionNotes.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value), false);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddDischargeTableCalculationCL);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}