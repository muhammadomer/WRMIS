using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Modules.ComplaintsManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class AddGaugeInspection : BasePage
    {

        #region ViewState Constants
        string PreparedByDesignationIDString = "PreparedByDesignationID";
        public const string ScheduleDetailID_VS = "ScheduleDetailID";
        public const string GaugeID_VS = "GaugeID";
        public const string PreparedByID = "PreparedByID";
        public const string DesignationID = "DesignationID";
        public const string ScheduleID = "ScheduleID";
        public const string DivisionID = "DivisionID";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        Session[ScheduleDetailID_VS] = Convert.ToInt64(Request.QueryString["ScheduleDetailID"]);
                        long PreparedByID = 0;
                        long StatusID = 0;
                        bool? IsScheduled = null;
                        if (!string.IsNullOrEmpty(Request.QueryString["IsScheduled"]))
                        {

                            IsScheduled = Convert.ToBoolean(Request.QueryString["IsScheduled"]);
                            hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                            //hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchInspection.aspx";
                            BindScheduleDetailData(Convert.ToInt64(Session[ScheduleDetailID_VS]), ref PreparedByID, ref StatusID, IsScheduled);
                        }
                        else
                        {
                            BindScheduleDetailData(Convert.ToInt64(Session[ScheduleDetailID_VS]), ref PreparedByID, ref StatusID, IsScheduled);
                            hlBack.NavigateUrl = "~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value);
                        }

                        if (!string.IsNullOrEmpty(Request.QueryString["ET"])) // from employee tracking 
                        {
                            lbtnBackToET.Visible = true;
                            hlBack.Visible = false;
                            btnSave.Visible = false;
                        }
                        else
                        {
                            lbtnBackToET.Visible = false;
                            hlBack.Visible = true;
                        }

                        txtGaugeFt.Attributes.Add("onblur", "javascript:CalculateDischarge('" + hdnGaugeID.Value + "','" + txtGaugeFt.ClientID + "','" + lblDischarge.ClientID + "');");
                    }
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetChannelGaugeMinMaxValues(long _ChannelID)
        {
            Tuple<double?, double?> tplChannelTypeMinMaxValue = new DailyDataBLL().GetChannelTypeMinMaxValueByChannelID(_ChannelID);
            double minValue = tplChannelTypeMinMaxValue.Item1.HasValue ? tplChannelTypeMinMaxValue.Item1.Value : 0;
            double maxValue = tplChannelTypeMinMaxValue.Item2.HasValue ? tplChannelTypeMinMaxValue.Item2.Value : 0;

            hdnGaugeMinValue.Value = Convert.ToString(minValue);
            hdnGaugeMaxValue.Value = Convert.ToString(maxValue);
        }
        private void BindScheduleDetailData(long _ScheduleDetailID, ref long _PreparedByID, ref long _StatusID, bool? IsScheduled)
        {
            try
            {
                if (null == IsScheduled || IsScheduled == true)
                {
                    dynamic ScheduleDetailData = new ScheduleInspectionBLL().GetScheduleDetailData(_ScheduleDetailID);

                    if (ScheduleDetailData != null && ScheduleDetailData.Count > 0)
                    {
                        hdnGaugeReadingID.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("GaugeReadingID").GetValue(ScheduleDetailData[0], null));
                        string Title = ScheduleDetailData[0].GetType().GetProperty("Name").GetValue(ScheduleDetailData[0], null);
                        string Status = ScheduleDetailData[0].GetType().GetProperty("Status").GetValue(ScheduleDetailData[0], null);
                        string PreparedBy = ScheduleDetailData[0].GetType().GetProperty("PreparedBy").GetValue(ScheduleDetailData[0], null);
                        hdnDivisionID.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("DivisionID").GetValue(ScheduleDetailData[0], null));
                        Session[PreparedByID] = ScheduleDetailData[0].GetType().GetProperty("PreparedByID").GetValue(ScheduleDetailData[0], null);
                        DateTime FromDate = ScheduleDetailData[0].GetType().GetProperty("FromDate").GetValue(ScheduleDetailData[0], null);
                        DateTime ToDate = ScheduleDetailData[0].GetType().GetProperty("ToDate").GetValue(ScheduleDetailData[0], null);
                        string ChannelName = ScheduleDetailData[0].GetType().GetProperty("ChannelName").GetValue(ScheduleDetailData[0], null);
                        long ChannelID = ScheduleDetailData[0].GetType().GetProperty("ChannelID").GetValue(ScheduleDetailData[0], null);
                        string InspectionArea = ScheduleDetailData[0].GetType().GetProperty("InspectionArea").GetValue(ScheduleDetailData[0], null);
                        string InspectedBy = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("InspectedBy").GetValue(ScheduleDetailData[0], null));
                        _StatusID = ScheduleDetailData[0].GetType().GetProperty("StatusID").GetValue(ScheduleDetailData[0], null);
                        ViewState[PreparedByDesignationIDString] = ScheduleDetailData[0].GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleDetailData[0], null);
                        Session[GaugeID_VS] = ScheduleDetailData[0].GetType().GetProperty("GaugeID").GetValue(ScheduleDetailData[0], null);
                        hdnGaugeID.Value = Convert.ToString(Session[GaugeID_VS]);
                        hdnScheduleID.Value = Convert.ToString(ScheduleDetailData[0].GetType().GetProperty("ScheduleID").GetValue(ScheduleDetailData[0], null));

                        GetChannelGaugeMinMaxValues(Convert.ToInt64(ChannelID));

                        ScheduleInspection.Controls.GaugeInspection.Title = Title;
                        ScheduleInspection.Controls.GaugeInspection.Status = Status;
                        ScheduleInspection.Controls.GaugeInspection.PreparedBy = PreparedBy;
                        ScheduleInspection.Controls.GaugeInspection.ChannelName = ChannelName;
                        ScheduleInspection.Controls.GaugeInspection.InspectionArea = InspectionArea;
                        ScheduleInspection.Controls.GaugeInspection.FromDate = Utility.GetFormattedDate(FromDate);
                        ScheduleInspection.Controls.GaugeInspection.ToDate = Utility.GetFormattedDate(ToDate);
                        ScheduleInspection.Controls.GaugeInspection.InspectedBy = InspectedBy == "" ? "N/A" : InspectedBy;
                        bool DuplicateRecord = new ScheduleInspectionBLL().IsGaugeRecordAlreadyExists(_ScheduleDetailID, Convert.ToInt32(Session[GaugeID_VS]));
                        if (DuplicateRecord)
                        {
                            dynamic GaugeData = new ScheduleInspectionBLL().GetGaugeRecordbyID(Convert.ToInt32(Session[GaugeID_VS]), _ScheduleDetailID);
                            string Date = Utility.GetFormattedDate(GaugeData.GetType().GetProperty("CreatedDate").GetValue(GaugeData, null));
                            string Time = Utility.GetFormattedTime(GaugeData.GetType().GetProperty("ReadingDateTime").GetValue(GaugeData, null));
                            txtCurrentDate.Text = Convert.ToString(Date);
                            txtInspectionTime.Text = Convert.ToString(Time);
                            txtInspectionTime.Visible = true;
                            lblInspectionTime.Text = "Inspection Time";
                            txtInspectionTime.Enabled = false;
                            lblInspectionTime.CssClass = "col-sm-4 col-lg-3 control-label";
                            bool IsGaugeFixed = GaugeData.GetType().GetProperty("IsGaugeFixed").GetValue(GaugeData, null);
                            RadioButtonListGaugeFixed.SelectedIndex = IsGaugeFixed == true ? 0 : 1;
                            bool IsGaugePainted = GaugeData.GetType().GetProperty("IsGaugePainted").GetValue(GaugeData, null);
                            RadioButtonListGaugePainted.SelectedIndex = IsGaugePainted == true ? 0 : 1;
                            //Need to make this filed nullable in databese
                            var GaugeValue = GaugeData.GetType().GetProperty("GaugeValue").GetValue(GaugeData, null);
                            txtGaugeFt.Text = Convert.ToString(GaugeValue);
                            //Need to make this filed nullable in databese
                            var DischargeVal = GaugeData.GetType().GetProperty("DailyDischarge").GetValue(GaugeData, null);
                            lblDischarge.Text = Convert.ToString(DischargeVal);
                            txtRemarks.Text = GaugeData.GetType().GetProperty("Remarks").GetValue(GaugeData, null);
                            //VIEW UPLOADED FILES AS LINK
                            List<string> lstFileNames = new ScheduleInspectionBLL().GetUploadedFileNames(Convert.ToInt32(Session[GaugeID_VS]), Convert.ToInt64(hdnGaugeReadingID.Value));
                            FileUploadControl.Visible = false;
                            if (lstFileNames.Count > 0)
                                PreviewImage(lstFileNames[0]);

                            txtCurrentDate.Enabled = false;
                            txtCurrentDate.CssClass = "aspNetDisabled form-control disabled-future-date-picker";
                            spanFromDate.Visible = false;
                            RadioButtonListGaugeFixed.Enabled = false;
                            RadioButtonListGaugePainted.Enabled = false;
                            txtGaugeFt.Enabled = false;
                            lblDischarge.Enabled = false;
                            txtRemarks.Enabled = false;
                            btnSave.CssClass += "btn btn-primary disabled";
                        }
                        else if (!DuplicateRecord)
                        {
                            string Now = Utility.GetFormattedDate(DateTime.Now);
                            txtCurrentDate.Text = Now;
                        }
                    }
                }
                else
                {
                    dynamic GaugeReadingData = new ScheduleInspectionBLL().GetGaugeChannelByGaugeReadingID(Convert.ToInt32(Session[ScheduleDetailID_VS]));
                    ScheduleInspection.Controls.GaugeInspection.Title = "Unscheduled Inspection";
                    ScheduleInspection.Controls.GaugeInspection.Status = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.PreparedBy = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ChannelName = Convert.ToString(GaugeReadingData.GetType().GetProperty("ChannelName").GetValue(GaugeReadingData, null));
                    ScheduleInspection.Controls.GaugeInspection.InspectionArea = Convert.ToString(GaugeReadingData.GetType().GetProperty("InspectionRD").GetValue(GaugeReadingData, null));
                    ScheduleInspection.Controls.GaugeInspection.FromDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.ToDate = "N/A";
                    ScheduleInspection.Controls.GaugeInspection.InspectedBy = Convert.ToString(GaugeReadingData.GetType().GetProperty("CreatedBy").GetValue(GaugeReadingData, null));



                    dynamic GaugeData = new ScheduleInspectionBLL().GetChannelGaugeRecordbyID(Convert.ToInt32(Session[ScheduleDetailID_VS]));
                    hdnGaugeReadingID.Value = Convert.ToString(GaugeData.GetType().GetProperty("ID").GetValue(GaugeData, null));
                    string Date = Utility.GetFormattedDate(GaugeData.GetType().GetProperty("CreatedDate").GetValue(GaugeData, null));
                    string Time = Utility.GetFormattedTime(GaugeData.GetType().GetProperty("ReadingDateTime").GetValue(GaugeData, null));
                    txtCurrentDate.Text = Convert.ToString(Date);
                    txtInspectionTime.Text = Convert.ToString(Time);
                    txtInspectionTime.Visible = true;
                    lblInspectionTime.Text = "Inspection Time";
                    txtInspectionTime.Enabled = false;
                    lblInspectionTime.CssClass = "col-sm-4 col-lg-3 control-label";
                    var GaugeID = GaugeData.GetType().GetProperty("GaugeID").GetValue(GaugeData, null);
                    hdnGaugeID.Value = Convert.ToString(GaugeID);
                    bool IsGaugeFixed = GaugeData.GetType().GetProperty("IsGaugeFixed").GetValue(GaugeData, null);
                    RadioButtonListGaugeFixed.SelectedIndex = IsGaugeFixed == true ? 0 : 1;
                    bool IsGaugePainted = GaugeData.GetType().GetProperty("IsGaugePainted").GetValue(GaugeData, null);
                    RadioButtonListGaugePainted.SelectedIndex = IsGaugePainted == true ? 0 : 1;
                    //Need to make this filed nullable in databese
                    var GaugeValue = GaugeData.GetType().GetProperty("GaugeValue").GetValue(GaugeData, null);
                    txtGaugeFt.Text = Convert.ToString(GaugeValue);
                    //Need to make this filed nullable in databese
                    var DischargeVal = GaugeData.GetType().GetProperty("DailyDischarge").GetValue(GaugeData, null);
                    lblDischarge.Text = Convert.ToString(DischargeVal);
                    txtRemarks.Text = GaugeData.GetType().GetProperty("Remarks").GetValue(GaugeData, null);
                    //VIEW UPLOADED FILES AS LINK
                    //List of File Names saved against a specific ID
                    List<string> lstFileNames = new ScheduleInspectionBLL().GetUploadedFileNames(Convert.ToInt32(GaugeID), Convert.ToInt64(hdnGaugeReadingID.Value));
                    FileUploadControl.Visible = false;
                    if (lstFileNames.Count > 0)
                        PreviewImage(lstFileNames[0]);

                    txtCurrentDate.Enabled = false;
                    txtCurrentDate.CssClass = "aspNetDisabled form-control disabled-future-date-picker";
                    spanFromDate.Visible = false;
                    RadioButtonListGaugeFixed.Enabled = false;
                    RadioButtonListGaugePainted.Enabled = false;
                    txtGaugeFt.Enabled = false;
                    lblDischarge.Enabled = false;
                    txtRemarks.Enabled = false;
                    btnSave.CssClass += "btn btn-primary disabled";


                }
                List<dynamic> ComplaintsData = new ScheduleInspectionBLL().GetComplaintsForGaugeInspection((long)Constants.ModuleName.ScheduleInspections, Convert.ToInt64(hdnGaugeReadingID.Value));
                gvComplaints.DataSource = ComplaintsData;
                gvComplaints.DataBind();
                if (ComplaintsData.Count > 0)
                {
                    gvComplaints.Visible = true;
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
                if (txtCurrentDate.Text != "")
                {
                    bool IsInspectionDateInvalid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtCurrentDate.Text));
                    if (IsInspectionDateInvalid)
                    {
                        Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (Convert.ToDateTime(txtCurrentDate.Text) > DateTime.Today)
                {
                    Master.ShowMessage(Message.Inspectiondate.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (RadioButtonListGaugeFixed.SelectedIndex == -1 && RadioButtonListGaugePainted.SelectedIndex == -1)
                {
                    Master.ShowMessage(Message.GaugeValuesRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (RadioButtonListGaugePainted.SelectedIndex == 0 && RadioButtonListGaugeFixed.SelectedIndex == 0 && txtGaugeFt.Text == "")
                {
                    Master.ShowMessage(Message.gaugeRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //File Upload user Control
                double? val = null;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.ScheduleInspection);
                if (lstNameofFiles.Count == 0)
                {
                    Master.ShowMessage(Message.FileFormatNotSupported.Description, SiteMaster.MessageType.Error);
                    return;
                }
                SI_ChannelGaugeReading GaugeData = new SI_ChannelGaugeReading();
                GaugeData.GaugeID = Convert.ToInt32(Session[GaugeID_VS]);
                GaugeData.ScheduleDetailChannelID = Convert.ToInt64(Session[ScheduleDetailID_VS]);
                GaugeData.CreatedDate = Convert.ToDateTime(txtCurrentDate.Text);
                GaugeData.GaugeValue = txtGaugeFt.Text == "" ? val : Convert.ToDouble(txtGaugeFt.Text);
                GaugeData.IsGaugeFixed = RadioButtonListGaugeFixed.SelectedValue == "True" ? true : false;
                GaugeData.IsGaugePainted = RadioButtonListGaugePainted.SelectedValue == "True" ? true : false;

                if (!string.IsNullOrEmpty(txtGaugeFt.Text))
                {
                    GaugeData.DailyDischarge = new DailyDataBLL().CalculateDischarge(Convert.ToInt64(hdnGaugeID.Value), Convert.ToDouble(txtGaugeFt.Text));
                }
                else
                {
                    GaugeData.DailyDischarge = val;
                }
                GaugeData.GaugePhoto = lstNameofFiles[0].Item3;
                GaugeData.Remarks = txtRemarks.Text;
                GaugeData.CreatedBy = Convert.ToInt64(Session[PreparedByID]);
                GaugeData.DesignationID = Convert.ToInt32(ViewState[PreparedByDesignationIDString]);
                GaugeData.GaugeReaderID = Convert.ToInt32(Session[PreparedByID]);
                GaugeData.Source = Configuration.RequestSource.RequestFromWeb;
                GaugeData.ReadingDateTime = DateTime.Now;
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                bool? IsComplaintGenerated = false;
                List<string> ComplaintIDs = new List<string>();

                long GaugeRedingID = new ScheduleInspectionBLL().SaveGaugeReadingData(GaugeData, mdlUser, Convert.ToInt64(hdnDivisionID.Value), ref IsComplaintGenerated, ref ComplaintIDs);


                if (IsComplaintGenerated == true)
                {
                    string IDs = string.Join(",", ComplaintIDs);
                    string Message = "Complaint(s) : " + IDs + " has been generated.";
                    ScheduleInspectionNotes.ActionMessage = Message;
                    ScheduleInspectionNotes.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value), false);

                }
                else
                {
                    ScheduleInspectionNotes.IsSaved = true;
                    Response.Redirect("~/Modules/ScheduleInspection/ScheduleInspectionNotes.aspx?ScheduleID=" + Convert.ToInt64(hdnScheduleID.Value), false);
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void Check_InspectionDates(object sender, EventArgs e)
        {
            try
            {
                if (txtCurrentDate.Text != "")
                {
                    bool IsInspectionDateInValid = new ScheduleInspectionBLL().CheckInspectionDateCheck(Convert.ToInt64(hdnScheduleID.Value), Convert.ToDateTime(txtCurrentDate.Text));
                    if (IsInspectionDateInValid)
                    {
                        Master.ShowMessage(Message.InspectionDateInvalid.Description, SiteMaster.MessageType.Error);
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtCurrentDate.Text = Now;
                    }
                }

            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CalculateDischarge(long _GaugeID, double _GaugeValue)
        {
            StringBuilder Message = new StringBuilder();
            double? Discharge = new DailyDataBLL().CalculateDischarge(_GaugeID, _GaugeValue);
            Message.Append(Convert.ToString(Discharge));
            Message.Append(";");
            double? Discharge115 = new ScheduleInspectionBLL().GetGaugeDesignDischargeByID(_GaugeID);
            if (Discharge > (Discharge115 * 1.15))
            {
                Message.Append("Discharge on the given Gauge is Excessive");
            }

            return Message.ToString();
        }
        protected void Check_GaugeValueEnabled(object sender, EventArgs e)
        {
            try
            {
                if (RadioButtonListGaugeFixed.SelectedItem.Value == "True" && RadioButtonListGaugePainted.SelectedItem.Value == "True")
                {
                    txtGaugeFt.Enabled = true;
                }
                else
                {
                    txtGaugeFt.Text = "";
                    lblDischarge.Text = "--";
                    txtGaugeFt.Enabled = false;
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddGaugeInspection);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void PreviewImage(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            //lnkFile.Text = "File: " + filename;
            //lnkFile.NavigateUrl = Utility.GetImageURL(Configuration.IrrigationNetwork, filename);

            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl1.Size = 1;
            FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.ScheduleInspection, lstName);

        }
    }
}