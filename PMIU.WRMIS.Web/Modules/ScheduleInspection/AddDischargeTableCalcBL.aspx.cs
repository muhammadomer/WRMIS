using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.Notifications;

using System;
using NPOI.OpenXmlFormats.Dml;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddDischargeTableCalcBL : BasePage
    {
        #region ViewState Constants
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
                    BindGaugeCorrectionTypeDropDown();
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        bool? IsScheduled = null;
                        Session[ScheduleDetailID_VS] = Convert.ToInt64(Request.QueryString["ScheduleDetailID"]);
                        long PreparedByID = 0;
                        long StatusID = 0;
                        if (!string.IsNullOrEmpty(Request.QueryString["IsScheduled"]))
                        {
                            hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                            IsScheduled = Convert.ToBoolean(Request.QueryString["IsScheduled"]);
                            ViewState["IsScheduled"] = IsScheduled;
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
                    dynamic ScheduleDetailData = new ScheduleInspectionBLL().GetScheduleDetailDataBL(_ScheduleDetailID);
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
                    bool IsRecordAlreadyExists = new ScheduleInspectionBLL().IsDischargeCalcBLRecordAlreadyExists(_ScheduleDetailID, Convert.ToInt32(Session[GaugeID_VS]));
                    if (IsRecordAlreadyExists)
                    {
                        CO_ChannelGaugeDTPGatedStructure Data = new ScheduleInspectionBLL().GetDischargeCalcBLDatabyID(Convert.ToInt32(Session[GaugeID_VS]), _ScheduleDetailID);

                        if (SessionManagerFacade.UserInformation.UA_Designations != null)
                        {
                            if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                            {
                                var date = Utility.GetFormattedDate(Data.ReadingDate);
                                var Time = Utility.GetFormattedTime(Data.ObservationDate.Value);
                                if (Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]) == 0 && Data.ReadingDate == Convert.ToDateTime("01/01/1950 0:00:00"))
                                {
                                    //txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                    txtCurrentDate.Text = "Pending for approval";
                                    txtCurrentDate.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    txtCurrentDate.Text = Utility.GetFormattedDate(Data.ReadingDate);
                                }
                                // txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                txtCurrentDate.Enabled = false;
                                txtCurrentDate.CssClass = "aspNetDisabled form-control";
                                txtCurrentDate.Attributes.Add("required", "false");
                                //spanFromDate.Visible = false;
                                txtInspectionTime.Text = Convert.ToString(Time);
                                txtInspectionTime.Visible = true;
                                lblInspectionTime.Text = "Inspection Time";
                                txtInspectionTime.Enabled = false;
                                lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                                txtCoefficientDischarge.Text = Convert.ToString(Data.DischargeCoefficient);
                                txtCoefficientDischarge.Enabled = false;
                                txtCoefficientDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtGaugeValCorrection.Text = Convert.ToString(Data.GaugeValueCorrection);
                                txtGaugeValCorrection.Enabled = false;
                                txtGaugeValCorrection.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtValExpN.Text = Convert.ToString(Data.ExponentValue);
                                txtValExpN.Enabled = false;
                                txtValExpN.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtMeanDepth.Text = Convert.ToString(Data.MeanDepth);
                                txtMeanDepth.Enabled = false;
                                txtMeanDepth.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtObserveDischarge.Text = Convert.ToString(Data.DischargeObserved);
                                txtObserveDischarge.Enabled = false;
                                txtObserveDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtObservationdate.Text = Utility.GetFormattedDate(Data.ObservationDate);
                                txtObservationdate.Enabled = false;
                                txtObservationdate.CssClass = "aspNetDisabled form-control decimal2PInput";
                                txtRemarks.Text = Convert.ToString(Data.Remarks);
                                txtRemarks.Enabled = false;
                                if (Data.GaugeCorrectionType == true)
                                {
                                    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSourced));
                                }
                                else if (Data.GaugeCorrectionType == false)
                                {
                                    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSilted));
                                }

                                ddlGaugeCorrectionType.Enabled = false;
                                btnSave.CssClass += "btn btn-primary disabled";
                            }
                            else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                            {
                                var date = Utility.GetFormattedDate(Data.ReadingDate);
                                var Time = Utility.GetFormattedTime(Data.ObservationDate.Value);
                                long ChannelGaugeDTPGatedStructure = Convert.ToInt64(Data.ID);
                                ViewState["ChannelGaugeDTPGatedStructureID"] = Convert.ToInt64(ChannelGaugeDTPGatedStructure);

                                if (Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]) != 0 && Data.ReadingDate != Convert.ToDateTime("01/01/1950 0:00:00"))
                                {
                                    btnSave.Enabled = false;
                                    txtCurrentDate.Text = Utility.GetFormattedDate(Data.ReadingDate);
                                }
                                else
                                {
                                    btnSave.Enabled = true;
                                    txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                }
                                //txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                                txtCurrentDate.Enabled = true;
                                txtCurrentDate.CssClass = "form-control date-picker required";
                                txtCurrentDate.Attributes.Add("required", "required");
                                txtCurrentDate.ReadOnly = false;
                                //spanFromDate.Visible = false;
                                txtInspectionTime.Text = Convert.ToString(Time);
                                txtInspectionTime.Visible = true;
                                lblInspectionTime.Text = "Inspection Time";
                                txtInspectionTime.Enabled = true;
                                lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                                txtCoefficientDischarge.Text = Convert.ToString(Data.DischargeCoefficient);
                                txtCoefficientDischarge.Enabled = false;
                                txtCoefficientDischarge.CssClass = "form-control decimal2PInput";
                                //  txtCoefficientDischarge.ReadOnly = false;
                                txtGaugeValCorrection.Text = Convert.ToString(Data.GaugeValueCorrection);
                                txtGaugeValCorrection.Enabled = true;
                                txtGaugeValCorrection.CssClass = " form-control decimal2PInput";
                                txtValExpN.Text = Convert.ToString(Data.ExponentValue);
                                txtValExpN.Enabled = true;
                                txtValExpN.CssClass = "form-control decimal2PInput";
                                txtMeanDepth.Text = Convert.ToString(Data.MeanDepth);
                                txtMeanDepth.Enabled = true;
                                txtMeanDepth.CssClass = "form-control decimal2PInput";
                                txtObserveDischarge.Text = Convert.ToString(Data.DischargeObserved);
                                txtObserveDischarge.Enabled = true;
                                txtObserveDischarge.CssClass = "form-control decimal2PInput";
                                txtRemarks.Text = Convert.ToString(Data.Remarks);
                                txtRemarks.Enabled = true;
                                txtObservationdate.Text = Utility.GetFormattedDate(Data.ObservationDate);
                                txtObservationdate.Enabled = false;
                                txtObservationdate.CssClass = "aspNetDisabled form-control";
                                if (Data.GaugeCorrectionType == true)
                                {
                                    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSourced));
                                }
                                else if (Data.GaugeCorrectionType == false)
                                {
                                    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSilted));
                                }

                                ddlGaugeCorrectionType.Enabled = true;
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
                            txtCurrentDate.Enabled = false;  // changed by Rizwan sb 
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                        //if (Data.GaugeCorrectionType == true)
                        //{
                        //    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSourced));
                        //}
                        //else if (Data.GaugeCorrectionType == false)
                        //{
                        //    Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType, Convert.ToString((long)Constants.GaugeCorrectionType.BedSilted));
                        //}

                        //ddlGaugeCorrectionType.Enabled = true;
                        //btnSave.CssClass += "btn btn-primary";
                        ////btnSave.Text = "";
                    }
                    else
                    {

                        //Current date for textbox
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtCurrentDate.Text = Now;
                        txtCurrentDate.Enabled = false;  // changed by Rizwan sb 
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
                    dynamic DischargeData = new ScheduleInspectionBLL().GetGaugeChannelByDischargeID(Convert.ToInt32(Convert.ToInt32(_ScheduleDetailID)));
                    ScheduleInspection.Controls.GaugeInspection.Title = "Unscheduled Inspection";
                    ScheduleInspection.Controls.GaugeInspection.Status = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.PreparedBy = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ChannelName = Convert.ToString(DischargeData.GetType().GetProperty("ChannelName").GetValue(DischargeData, null));
                    ScheduleInspection.Controls.GaugeInspection.InspectionArea = Convert.ToString(DischargeData.GetType().GetProperty("InspectionRD").GetValue(DischargeData, null));
                    ScheduleInspection.Controls.GaugeInspection.FromDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ToDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.InspectedBy = Convert.ToString(DischargeData.GetType().GetProperty("CreatedBy").GetValue(DischargeData, null));
                    hdnCreatedDate.Value = Convert.ToString(DischargeData.GetType().GetProperty("CreatedDate").GetValue(DischargeData, null));

                    CO_ChannelGaugeDTPGatedStructure Data = new ScheduleInspectionBLL().GetDischargeCalcBLDatabyDischargeID(Convert.ToInt32(_ScheduleDetailID));
                    //hdnCreatedDate.Value = Convert.ToString(Data.CreatedDate);
                    Session[GaugeID_VS] = Data.GaugeID;
                    if (SessionManagerFacade.UserInformation.UA_Designations != null)
                    {

                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                        {
                            var date = Utility.GetFormattedDate(Data.ReadingDate);
                            var Time = Utility.GetFormattedTime((Data.ObservationDate.Value));
                            //txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            if (Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]) == 0 &&
                                Data.ReadingDate == Convert.ToDateTime("01/01/1950 0:00:00"))
                            {
                                txtCurrentDate.Text = Utility.GetFormattedDate(DateTime.Now);
                            }
                            else
                            {
                                txtCurrentDate.Text = Utility.GetFormattedDate(Data.ReadingDate);
                            }
                            txtCurrentDate.Enabled = false;
                            txtCurrentDate.CssClass = "aspNetDisabled form-control";
                            //spanFromDate.Visible = false;
                            txtInspectionTime.Text = Convert.ToString(Time);
                            txtInspectionTime.Visible = true;
                            lblInspectionTime.Text = "Inspection Time";
                            txtInspectionTime.Enabled = false;
                            lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                            txtCoefficientDischarge.Text = Convert.ToString(Data.DischargeCoefficient);
                            txtCoefficientDischarge.Enabled = false;
                            txtCoefficientDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtGaugeValCorrection.Text = Convert.ToString(Data.GaugeValueCorrection);
                            txtGaugeValCorrection.Enabled = false;
                            txtGaugeValCorrection.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtValExpN.Text = Convert.ToString(Data.ExponentValue);
                            txtValExpN.Enabled = false;
                            txtValExpN.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtMeanDepth.Text = Convert.ToString(Data.MeanDepth);
                            txtMeanDepth.Enabled = false;
                            txtMeanDepth.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtObserveDischarge.Text = Convert.ToString(Data.DischargeObserved);
                            txtObserveDischarge.Enabled = false;
                            txtObserveDischarge.CssClass = "aspNetDisabled form-control decimal2PInput";
                            txtRemarks.Text = Convert.ToString(Data.Remarks);
                            txtRemarks.Enabled = false;
                            txtObservationdate.Text = Utility.GetFormattedDate(Data.ObservationDate);
                            txtObservationdate.Enabled = false;
                            txtObservationdate.CssClass = "aspNetDisabled form-control";
                            if (Data.GaugeCorrectionType == true)
                            {
                                Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType,
                                    Convert.ToString((long)Constants.GaugeCorrectionType.BedSourced));
                            }
                            else if (Data.GaugeCorrectionType == false)
                            {
                                Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType,
                                    Convert.ToString((long)Constants.GaugeCorrectionType.BedSilted));
                            }

                            ddlGaugeCorrectionType.Enabled = false;
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                        else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            var date = Utility.GetFormattedDate(Data.ReadingDate);
                            var Time = Utility.GetFormattedTime(Data.ObservationDate.Value);
                            long ChannelGaugeDTPGatedStructure = Convert.ToInt64(Data.ID);
                            ViewState["ChannelGaugeDTPGatedStructureID"] =
                                Convert.ToInt64(ChannelGaugeDTPGatedStructure);

                            if (Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]) != 0 &&
                                Data.ReadingDate != Convert.ToDateTime("01/01/1950 0:00:00"))
                            {
                                btnSave.Enabled = false;
                                txtCurrentDate.Text = Utility.GetFormattedDate(Data.ReadingDate);
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
                            //spanFromDate.Visible = false;
                            txtInspectionTime.Text = Convert.ToString(Time);
                            txtInspectionTime.Visible = true;
                            lblInspectionTime.Text = "Inspection Time";
                            txtInspectionTime.Enabled = true;
                            lblInspectionTime.CssClass = "col-sm-5 col-lg-4 control-label";
                            txtCoefficientDischarge.Text = Convert.ToString(Data.DischargeCoefficient);
                            txtCoefficientDischarge.Enabled = false;
                            txtCoefficientDischarge.CssClass = "form-control decimal2PInput";
                            //  txtCoefficientDischarge.ReadOnly = false;
                            txtGaugeValCorrection.Text = Convert.ToString(Data.GaugeValueCorrection);
                            txtGaugeValCorrection.Enabled = true;
                            txtGaugeValCorrection.CssClass = " form-control decimal2PInput";
                            txtValExpN.Text = Convert.ToString(Data.ExponentValue);
                            txtValExpN.Enabled = true;
                            txtValExpN.CssClass = "form-control decimal2PInput";
                            txtMeanDepth.Text = Convert.ToString(Data.MeanDepth);
                            txtMeanDepth.Enabled = true;
                            txtMeanDepth.CssClass = "form-control decimal2PInput";
                            txtObserveDischarge.Text = Convert.ToString(Data.DischargeObserved);
                            txtObserveDischarge.Enabled = true;
                            txtObserveDischarge.CssClass = "form-control decimal2PInput";
                            txtRemarks.Text = Convert.ToString(Data.Remarks);
                            txtRemarks.Enabled = true;
                            txtObservationdate.Text = Utility.GetFormattedDate(Data.ObservationDate);
                            txtObservationdate.Enabled = false;
                            txtObservationdate.CssClass = "aspNetDisabled form-control";
                            if (Data.GaugeCorrectionType == true)
                            {
                                Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType,
                                    Convert.ToString((long)Constants.GaugeCorrectionType.BedSourced));
                            }
                            else if (Data.GaugeCorrectionType == false)
                            {
                                Dropdownlist.SetSelectedValue(ddlGaugeCorrectionType,
                                    Convert.ToString((long)Constants.GaugeCorrectionType.BedSilted));
                            }

                            ddlGaugeCorrectionType.Enabled = true;
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
                        txtCurrentDate.Enabled = false;  // changed by Rizwan sb 
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
        /// Created on 13-11-2015
        /// </summary>
        /// <returns>CO_ChannelGaugeDTPGatedStructure</returns>
        private CO_ChannelGaugeDTPGatedStructure GetValidatedDate()
        {
            try
            {
                CO_ChannelGaugeDTPGatedStructure mdlChannelGaugeDT = new CO_ChannelGaugeDTPGatedStructure();
                DateTime ReadingDate = Utility.GetParsedDate(txtCurrentDate.Text.Trim());
                //DateTime ObservationDate = Utility.GetParsedDate(txtObservationdate.Text.Trim());
                DateTime ObservationDate = Convert.ToDateTime(txtObservationdate.Text.Trim());

                //ReadingDate = ReadingDate.Add(DateTime.Now.TimeOfDay);
                //ReadingDate = Convert.ToDateTime("01/01/1950 0:00:00");
                // mdlChannelGaugeDT.ReadingDate = ReadingDate;
                mdlChannelGaugeDT.ObservationDate = ObservationDate;

                if (ObservationDate > ReadingDate)
                {
                    Master.ShowMessage("Observation Date should be less than or equal to Inspection date.", SiteMaster.MessageType.Error);

                    return null;
                }
                //if (mdlChannelGaugeDT.ReadingDate < DateTime.Now.Date)
                //{
                //    Master.ShowMessage(Message.OldDatesNotAllowed.Description, SiteMaster.MessageType.Error);

                //    return null;
                //}

                return mdlChannelGaugeDT;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //protected void RadioButtonListGaugeCorrection_OnSelectedIndexChanged(Object sender, EventArgs e)
        //{

        //    try
        //    {
        //        if (RadioButtonListGaugeCorrection.SelectedIndex != -1)
        //        {
        //            txtGaugeValCorrection.CssClass += " required";
        //            txtGaugeValCorrection.Attributes.Add("required", "true");
        //        }
        //    }
        //    catch (Exception exp)
        //    {

        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtCurrentDate.Text != "")
                //{
                //    if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                //    {
                //        bool IsInspectionDateInvalid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtCurrentDate.Text));
                //        if (IsInspectionDateInvalid)
                //        {
                //            Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                //            return;
                //        }
                //    }

                //}
                double? val = null;
                CO_ChannelGaugeDTPGatedStructure mdlChannelGaugeDT = new CO_ChannelGaugeDTPGatedStructure();
                DateTime ReadingDate = Utility.GetParsedDate(txtCurrentDate.Text.Trim());
                DateTime ObservationDate = Utility.GetParsedDate(txtObservationdate.Text.Trim());
                string ObservationTime = Utility.GetFormattedTime(DateTime.Now);
                mdlChannelGaugeDT.ID = Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]);
                //  mdlChannelGaugeDT.ObservationDate = Utility.GetParsedDateTime(txtObservationdate.Text, ObservationTime);
                if (Convert.ToInt64(ViewState["ChannelGaugeDTPGatedStructureID"]) == 0)
                {
                    ReadingDate = Convert.ToDateTime("01/01/1950 0:00:00");
                    mdlChannelGaugeDT.ReadingDate = ReadingDate;
                    if (ObservationDate > DateTime.Now.Date)
                    {
                        Master.ShowMessage("Observation Date should be less than or equal to Approval date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    if (ObservationDate < Convert.ToDateTime(ViewState["FromDate"]))
                    {
                        Master.ShowMessage("Observation Date should be greater than or equal to From date.", SiteMaster.MessageType.Error);
                        return;
                    }

                    mdlChannelGaugeDT.ObservationDate = Utility.GetParsedDateTime(txtObservationdate.Text, ObservationTime);
                    mdlChannelGaugeDT.CreatedBy = SessionManagerFacade.UserInformation.ID;
                    mdlChannelGaugeDT.CreatedDate = DateTime.Now;
                }
                else
                {
                    mdlChannelGaugeDT.ReadingDate = ReadingDate;
                    if (ReadingDate < DateTime.Now.Date)
                    {
                        Master.ShowMessage("Inspection Date should be greater than or equal to Current date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdlChannelGaugeDT.CreatedBy = Convert.ToInt64(ViewState["InspectedByID"]);
                    mdlChannelGaugeDT.ObservationDate = Utility.GetParsedDateTime(txtObservationdate.Text, ObservationTime);
                    mdlChannelGaugeDT.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                    mdlChannelGaugeDT.ModifiedDate = DateTime.Now;
                    mdlChannelGaugeDT.IsActive = true;
                    mdlChannelGaugeDT.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    //if (ObservationDate > ReadingDate)
                    //{
                    //    Master.ShowMessage("Observation Date should be less than or equal to Inspection date.", SiteMaster.MessageType.Error);
                    //    return;
                    //}
                }
                //if (mdlChannelGaugeDT == null)
                //{
                //    return;
                //}0

                mdlChannelGaugeDT.ExponentValue = Convert.ToDouble(txtValExpN.Text.Trim());
                mdlChannelGaugeDT.MeanDepth = Convert.ToDouble(txtMeanDepth.Text.Trim());
                mdlChannelGaugeDT.DischargeObserved = Convert.ToDouble(txtObserveDischarge.Text.Trim());
                mdlChannelGaugeDT.GaugeID = Convert.ToInt32(Session[GaugeID_VS]);
                mdlChannelGaugeDT.Remarks = txtRemarks.Text;
                if (null == ViewState["IsScheduled"] || Convert.ToBoolean(ViewState["IsScheduled"]) == true)
                {
                    mdlChannelGaugeDT.ScheduleDetailChannelID = Convert.ToInt64(Session[ScheduleDetailID_VS]);
                }
                else
                {
                    mdlChannelGaugeDT.ScheduleDetailChannelID = null;
                }
                //mdlChannelGaugeDT.CreatedBy = SessionManagerFacade.UserInformation.ID;
                if (hdnSource.Value != Configuration.RequestSource.RequestFromMobile)
                    mdlChannelGaugeDT.Source = Configuration.RequestSource.RequestFromWeb;
                else
                    mdlChannelGaugeDT.Source = Configuration.RequestSource.RequestFromMobile;
                //bool IsBedScouredChecked = RadioButtonListGaugeCorrection.Checked;
                //bool IsBedSiltedChecked = rbBedSilted.Checked;
                if (ddlGaugeCorrectionType.SelectedItem.Value != "")
                {
                    if (txtGaugeValCorrection.Text != "")
                    {
                        mdlChannelGaugeDT.GaugeValueCorrection = Convert.ToDouble(txtGaugeValCorrection.Text.Trim());
                        if (ddlGaugeCorrectionType.SelectedItem.Value == Convert.ToString((long)(Constants.GaugeCorrectionType.BedSourced)))
                        {
                            mdlChannelGaugeDT.GaugeCorrectionType = Constants.GaugeCorrectionScouredType;
                        }
                        else
                        {
                            mdlChannelGaugeDT.GaugeCorrectionType = Constants.GaugeCorrectionSiltedType;

                            if (mdlChannelGaugeDT.MeanDepth <= mdlChannelGaugeDT.GaugeValueCorrection)
                            {
                                Master.ShowMessage(Message.CannotBeGreaterThanEqualToBedSiltedCorrection.Description, SiteMaster.MessageType.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        Master.ShowMessage(Message.GaugeValueCorrectionRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                }
                else
                {
                    mdlChannelGaugeDT.GaugeValueCorrection = txtGaugeValCorrection.Text == "" ? val : Convert.ToDouble(txtGaugeValCorrection.Text.Trim());
                }

                mdlChannelGaugeDT.DischargeCoefficient = Calculations.GetBedCoefficientOfDischarge(mdlChannelGaugeDT.ExponentValue, mdlChannelGaugeDT.MeanDepth,
                    mdlChannelGaugeDT.DischargeObserved, mdlChannelGaugeDT.GaugeCorrectionType, mdlChannelGaugeDT.GaugeValueCorrection);
                txtCoefficientDischarge.Text = Convert.ToString(Math.Round(mdlChannelGaugeDT.DischargeCoefficient, 3));
                ChannelBLL bllChannel = new ChannelBLL();
                bool IsRecordSaved = bllChannel.AddBedLevelDTParameters(mdlChannelGaugeDT);
                if (IsRecordSaved)
                {
                    NotifyEvent _event = new NotifyEvent();
                    if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                    {
                        _event.Parameters.Add("GaugeID", mdlChannelGaugeDT.GaugeID);
                        _event.Parameters.Add("IsCrestParameters", "false");
                        _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditBedLevelParameters, SessionManagerFacade.UserInformation.ID);

                    }
                    if (null == ViewState["IsScheduled"] || Convert.ToBoolean(ViewState["IsScheduled"]) == true)
                    {

                        _event.Parameters.Add("ScheduleDetailID", Convert.ToInt64(Session[ScheduleDetailID_VS]));
                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMIsApprovedByDD, SessionManagerFacade.UserInformation.ID);
                        }
                        else
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMAssignedToDDForApproval, SessionManagerFacade.UserInformation.ID);
                        }
                    }
                    else
                    {

                        _event.Parameters.Add("ScheduleDetailID", null);
                        _event.Parameters.Add("_GaugeID", Convert.ToInt32(Session[GaugeID_VS]));
                        if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.DeputyDirector)))
                        {
                            _event.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMisapprovedbyDD, SessionManagerFacade.UserInformation.ID);
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
                btnSave.Enabled = true;
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindGaugeCorrectionTypeDropDown()
        {
            try
            {
                ddlGaugeCorrectionType.DataSource = CommonLists.GetGaugeCorrectionType();
                ddlGaugeCorrectionType.DataTextField = "Name";
                ddlGaugeCorrectionType.DataValueField = "ID";
                ddlGaugeCorrectionType.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlGaugeCorrectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlGaugeCorrectionType.SelectedItem.Value != "")
                {
                    txtGaugeValCorrection.Enabled = true;
                    txtGaugeValCorrection.CssClass = "form-control decimal2PInput required";
                    txtGaugeValCorrection.Attributes.Add("required", "true");
                }
                else
                {
                    txtGaugeValCorrection.Enabled = false;
                    txtGaugeValCorrection.CssClass = "form-control decimal2PInput";
                    txtGaugeValCorrection.Attributes.Remove("required");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddDischargeTableCalculationBL);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}