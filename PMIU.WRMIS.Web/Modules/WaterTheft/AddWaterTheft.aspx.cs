using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using System.Web.UI.HtmlControls;
using PMIU.WRMIS.BLL.WaterTheft;
using System.IO;
using PMIU.WRMIS.AppBlocks;
using System.Data;
using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;
using PMIU.WRMIS.Web.Modules.ComplaintsManagement;


namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class AddWaterTheft : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    SetTitle();
                    BindDropDown();
                    txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    txtDateofChecking.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    BindDivisionDropdown(mdlUser);
                    BindChannelDropdown(mdlUser);
                    if (mdlUser.DesignationID == (long)Constants.Designation.SBE || mdlUser.DesignationID == (long)Constants.Designation.SDO || mdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
                    {
                        ddlChannel.Enabled = true;
                        ddlChannels.Enabled = true;
                    }
                    else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                    {
                        ddlChannel.Enabled = false;
                        ddlChannels.Enabled = false;
                        ddlChannels.SelectedIndex = 0;
                        ddlChannel.SelectedIndex = 0;
                    }
                    else if (mdlUser.DesignationID != null)
                    {
                        ddlChannel.Enabled = false;
                        ddlChannels.Enabled = false;
                        ddlChannels.SelectedIndex = 0;
                        ddlChannel.SelectedIndex = 0;
                    }
                    ddlOutlet.Enabled = false;
                    OutletInfos.Visible = false;
                    btnSaveChannelData.Visible = base.CanAdd;
                    btnSaveOutletIncidentData.Visible = base.CanAdd;
                    txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                    ChannelRemarks.Attributes.Add("maxlength", ChannelRemarks.MaxLength.ToString());
                }

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddWaterTheft);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption, true);

                ddlDivision.Enabled = false;
            }
            else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select, true);
            }

            else if (_MdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select, true);

            }

        }
        /// <summary>
        /// This function binds the Channel dropdown
        /// Created on 10-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>
        private void BindChannelDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                ddlChannel.Enabled = true;

                long DivisionID = -1;

                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannels, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);

            }
        }
        private void BindDropDown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            long UserId = mdlUser.ID;
            if (mdlUser.DesignationID == null)
                return;
            Dropdownlist.BindDropdownlist<List<object>>(ddlOffenceSite, CommonLists.GetOffenceSite());
            Dropdownlist.BindDropdownlist<List<object>>(ddlDefectiveType, new WaterTheftBLL().GetDefectiveType());
            Dropdownlist.BindDropdownlist<List<object>>(ddlChannelSide, CommonLists.GetWTChannelSides());

        }
        private string GetOutletInformation(dynamic _WaterTheftIncident, string _PropertyName)
        {
            return Convert.ToString(_WaterTheftIncident.GetType().GetProperty(_PropertyName).GetValue(_WaterTheftIncident, null));
        }
        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            long UserId = mdlUser.ID;
            long IrrigationLevelID;
            if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                IrrigationLevelID = 0;
            }
            else
            {
                IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;
            }

            long ChannelId = ddlChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlChannel.SelectedItem.Value);
            if (ChannelId != -1)
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new WaterTheftBLL().GetOutletByChannelId(ChannelId, UserId, IrrigationLevelID));
                ddlOutlet.Enabled = true;
            }
            else
            {
                ddlOutlet.Enabled = false;
                if (ddlOutlet.SelectedIndex > 0)
                {
                    ddlOutlet.SelectedIndex = 0;
                }

            }
            OutletInfos.Visible = false;
        }
        protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int outletId = ddlOutlet.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlOutlet.SelectedItem.Value);
            if (outletId != -1)
            {
                object outlet = new WaterTheftBLL().GetDatabyOutletId(outletId);
                if (outlet != null)
                {
                    txtRDS.Text = outlet.GetType().GetProperty("OutletRDs").GetValue(outlet).ToString();
                    txtSide.Text = outlet.GetType().GetProperty("ChannelSide").GetValue(outlet).ToString();
                    if (outlet.GetType().GetProperty("OutletType").GetValue(outlet) != null)
                    {
                        txtType.Text = outlet.GetType().GetProperty("OutletType").GetValue(outlet).ToString();
                    }
                    else
                    {
                        txtType.Text = string.Empty;
                    }
                    HttpContext.Current.Session["OutletRDsDB"] = outlet.GetType().GetProperty("OutletRDsDB").GetValue(outlet).ToString();
                    OutletInfos.Visible = true;
                }
                else
                {
                    OutletInfos.Visible = false;
                }


            }
            else
            {
                OutletInfos.Visible = false;
            }

        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    ddlChannel.Enabled = true;
                    ddlChannels.Enabled = true;

                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                    {
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannels, mdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                    }
                    else
                    {
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannels, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                    }

                }
                else
                {
                    ddlChannel.SelectedIndex = 0;
                    ddlChannel.Enabled = false;
                    ddlChannels.SelectedIndex = 0;
                    ddlChannels.Enabled = false;
                    ddlOutlet.SelectedIndex = 0;
                    ddlOutlet.Enabled = false;
                    OutletInfos.Visible = false;

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlOffenceSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            String OffenceSiteId = Convert.ToString(ddlOffenceSite.SelectedItem.Value);
            List<object> lstTheftType = new WaterTheftBLL().GetAllTheftType(OffenceSiteId);
            List<object> lstTheftTypeCondition = new WaterTheftBLL().GetOutletCondition(OffenceSiteId);

            Dropdownlist.BindDropdownlist<List<object>>(ddlOffenceType, lstTheftType);
            Dropdownlist.BindDropdownlist<List<object>>(ddlCutCondition, lstTheftTypeCondition);

            Dropdownlist.BindDropdownlist<List<object>>(ddlTheftType, lstTheftType);
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutletCondition, lstTheftTypeCondition);
            ResetForm();
        }
        protected int VerfiyChannelRDs()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            long UserId = mdlUser.ID;
            int MinRDs;
            int MaxRDs;
            if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                return 1;
            long IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;
            int ChannelId = Convert.ToInt32(ddlChannels.SelectedItem.Value);
            DataTable ChannelRDs = new WaterTheftBLL().VerfiyChannelRDs(ChannelId, UserId, IrrigationLevelID);
            if (ChannelRDs == null)
            {
                MinRDs = 0;
                MaxRDs = 0;
            }
            Decimal ChannelRD = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
            MinRDs = ChannelRDs.AsEnumerable().Min(r => r.Field<int>("MinRD"));
            MaxRDs = ChannelRDs.AsEnumerable().Max(r => r.Field<int>("MaxRD"));

            if (ChannelRD >= MinRDs && ChannelRD <= MaxRDs)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        private WT_WaterTheftCase PrepareWaterTheftOutletEntity()
        {
            long? OffenceTypeID = null;
            long UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            DateTime IncidentDateTime = Convert.ToDateTime(txtIncidentDate.Text);
            string time = TimePicker.GetTime();
            DateTime _FinalIncidentDateTime = PMIU.WRMIS.Common.Utility.GetParsedDateTime(txtIncidentDate.Text, time);
            WT_WaterTheftCase lstofCaseID = new WaterTheftBLL().GetWaterTheftID();
            long? IdentityNo = 0;
            if (lstofCaseID == null)
            {
                IdentityNo = 1;
            }
            else
            {
                IdentityNo = lstofCaseID.ID + 1;
            }

            string CaseNo = "";
            if (IdentityNo < 99)
            {
                CaseNo = "WT0000" + IdentityNo;
            }
            if (IdentityNo > 99 && IdentityNo < 999)
            {
                CaseNo = "WT000" + IdentityNo;
            }
            if (IdentityNo > 999 && IdentityNo < 9999)
            {
                CaseNo = "WT00" + IdentityNo;
            }
            if (IdentityNo > 9999 && IdentityNo < 99999)
            {
                CaseNo = "WT0" + IdentityNo;
            }
            if (IdentityNo > 99999 && IdentityNo < 999999)
            {
                CaseNo = "WT" + IdentityNo;
            }
            WT_WaterTheftCase _mdlWatertheftCase = new WT_WaterTheftCase();
            if (!string.IsNullOrEmpty(ddlTheftType.SelectedItem.Value))
                OffenceTypeID = Convert.ToInt64(ddlTheftType.SelectedItem.Value);
            _mdlWatertheftCase.ChannelID = Convert.ToInt32(ddlChannel.SelectedItem.Value);
            _mdlWatertheftCase.OutletID = Convert.ToInt32(ddlOutlet.SelectedItem.Value);
            _mdlWatertheftCase.TheftSiteRD = Convert.ToInt32(HttpContext.Current.Session["OutletRDsDB"]); //Convert.ToInt32(OutletRDsDB.Value);
            _mdlWatertheftCase.OffenceSide = txtSide.Text;
            _mdlWatertheftCase.IncidentDateTime = Convert.ToDateTime(_FinalIncidentDateTime);
            _mdlWatertheftCase.OffenceTypeID = OffenceTypeID; //Convert.ToInt64(ddlTheftType.SelectedItem.Value);
            _mdlWatertheftCase.TheftSiteConditionID = Convert.ToInt32(ddlOutletCondition.SelectedItem.Value);
            _mdlWatertheftCase.ValueofH = txtHValue.Text == string.Empty ? -1 : Convert.ToDouble(txtHValue.Text);
            _mdlWatertheftCase.UserID = UserId;
            _mdlWatertheftCase.CreatedBy = UserId;
            _mdlWatertheftCase.LogDateTime = null;
            _mdlWatertheftCase.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
            _mdlWatertheftCase.OffenceSite = Convert.ToString(ddlOffenceSite.SelectedItem.Value);
            _mdlWatertheftCase.CreatedDate = DateTime.Now;
            _mdlWatertheftCase.Remarks = txtRemarks.Text;
            _mdlWatertheftCase.IsActive = true;
            _mdlWatertheftCase.CaseNo = CaseNo;
            _mdlWatertheftCase.Source = "W";
            return _mdlWatertheftCase;
        }
        private WT_OutletDefectiveDetails PrepareOutletDefectiveDetail()
        {
            long UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            double? ValueB = null;
            if (!string.IsNullOrEmpty(txtBValue.Text))
                ValueB = Convert.ToDouble(txtBValue.Text);
            double? ValueDIA = null;
            if (!string.IsNullOrEmpty(txtDIAValue.Text))
                ValueDIA = Convert.ToDouble(txtDIAValue.Text);
            double? ValueofY = null;
            if (!string.IsNullOrEmpty(txtYValue.Text))
                ValueofY = Convert.ToDouble(txtYValue.Text);
            WT_OutletDefectiveDetails outletDefectiveDetails = new WT_OutletDefectiveDetails();
            outletDefectiveDetails.DefectiveTypeID = ddlDefectiveType.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlDefectiveType.SelectedItem.Value);
            outletDefectiveDetails.ValueOfB = ValueB;
            outletDefectiveDetails.ValueOfDia = ValueDIA;
            outletDefectiveDetails.ValueOfY = ValueofY;
            outletDefectiveDetails.CreatedBy = UserId;
            outletDefectiveDetails.CreatedDate = DateTime.Now;
            return outletDefectiveDetails;
        }
        private WT_WaterTheftStatus PrepareWaterTheftStatusEntity()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            WT_WaterTheftStatus WaterTheftCaseStatus = new WT_WaterTheftStatus();
            // long UserId = mdlUser.ID;
            // long IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;

            WaterTheftCaseStatus.AssignedByUserID = mdlUser.ID;
            WaterTheftCaseStatus.AssignedByDesignationID = Convert.ToInt64(mdlUser.DesignationID);
            WaterTheftCaseStatus.AssignedDate = DateTime.Now;
            WaterTheftCaseStatus.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
            WaterTheftCaseStatus.IsActive = true;

            if (mdlUser.DesignationID == (Int32)Constants.Designation.SBE)
            {
                WaterTheftCaseStatus.AssignedToUserID = mdlUser.ID;
                WaterTheftCaseStatus.AssignedToDesignationID = Convert.ToInt64(mdlUser.DesignationID);
            }
            else
            {
                String OffenceSiteId = Convert.ToString(ddlOffenceSite.SelectedItem.Value);
                Decimal ChannelOutletRDs;
                long ChannelId;

                if (OffenceSiteId == Constants.ChannelorOutlet.C.ToString())
                {
                    ChannelOutletRDs = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
                    ChannelId = Convert.ToInt64(ddlChannels.SelectedItem.Value);
                }
                else
                {
                    ChannelOutletRDs = Convert.ToDecimal(HttpContext.Current.Session["OutletRDsDB"]);  //Convert.ToInt32(OutletRDsDB.Value);
                    ChannelId = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                }

                long UserId = (long)mdlUser.ID;
                object GetSBEInformation = new WaterTheftBLL().GetRelevantSBE(ChannelId, ChannelOutletRDs, UserId);
                if (GetSBEInformation == null)
                    return WaterTheftCaseStatus;

                WaterTheftCaseStatus.AssignedToUserID = Convert.ToInt64(GetSBEInformation.GetType().GetProperty("UserID").GetValue(GetSBEInformation));
                WaterTheftCaseStatus.AssignedToDesignationID = Convert.ToInt64(GetSBEInformation.GetType().GetProperty("DesignationID").GetValue(GetSBEInformation));

            }
            return WaterTheftCaseStatus;
        }
        protected void btnSaveOutletIncidentData_Click(object sender, EventArgs e)
        {
            WT_WaterTheftCase _mdlWatertheftCase = PrepareWaterTheftOutletEntity();
            WT_WaterTheftStatus _mdlWaterTheftCaseStatus = PrepareWaterTheftStatusEntity();
            WT_OutletDefectiveDetails _mdloutletDefectiveDetails = PrepareOutletDefectiveDetail();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            try
            {
                if (_mdlWatertheftCase.IncidentDateTime > DateTime.Now)
                {
                    Master.ShowMessage(Message.EnterCorrectDate.Description, SiteMaster.MessageType.Error);
                    return;
                }

                long UserID = mdlUser.ID;
                long DesignationID = Convert.ToInt64(mdlUser.DesignationID);
                string ChannelName = ddlChannel.SelectedItem.Text;
                long DivisionID = -1;
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.WaterTheft);
                long StatusID = new WaterTheftBLL().AddWaterTheftCaseChannelorOutlet(mdlUser.ID, _mdlWatertheftCase, _mdlWaterTheftCaseStatus, _mdloutletDefectiveDetails
                      , DivisionID, 2, lstNameofFiles, Convert.ToString(Convert.ToString(Constants.ComplaintModuleReference.WT_O)), 0, 0, "W", ChannelName);

                if (StatusID == -1)
                {
                    Master.ShowMessage(Message.EnterCorrectRangeRD.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -2)
                {
                    Master.ShowMessage(Message.ReleventSBE.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -3)
                {
                    ddlOutletCondition.SelectedIndex = 0;
                    ddlTheftType.Enabled = true;
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -4)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //NotifyEvent _event = new NotifyEvent();
                //_event.Parameters.Add("WaterTheftID", StatusID);
                //_event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddWaterTheftCase, SessionManagerFacade.UserInformation.ID);
                Master.ShowMessage("Case ID :  " + _mdlWatertheftCase.CaseNo + " " + Message.WaterTheftRecordAdded.Description, SiteMaster.MessageType.Success);
                ResetForm();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private WT_WaterTheftCase PrepareWaterTheftChannelEntity()
        {
            long? ddlCutConditionID = null;
            long UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            Decimal ChannelRDs = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
            string TheftTime = ChannelTimePicker.GetTime();
            DateTime _FinalDateofChecking = PMIU.WRMIS.Common.Utility.GetParsedDateTime(txtDateofChecking.Text, TheftTime);
            WT_WaterTheftCase ChannelWatertheftCase = new WT_WaterTheftCase();
            WT_WaterTheftCase lstofCaseID = new WaterTheftBLL().GetWaterTheftID();
            long? IdentityNo = 0;
            if (lstofCaseID == null)
            {
                IdentityNo = 1;
            }
            else
            {
                IdentityNo = lstofCaseID.ID + 1;
            }
            string CaseNo = "";
            if (IdentityNo < 99)
            {
                CaseNo = "WT0000" + IdentityNo;
            }
            if (IdentityNo > 99 && IdentityNo < 999)
            {
                CaseNo = "WT000" + IdentityNo;
            }
            if (IdentityNo > 999 && IdentityNo < 9999)
            {
                CaseNo = "WT00" + IdentityNo;
            }
            if (IdentityNo > 9999 && IdentityNo < 99999)
            {
                CaseNo = "WT0" + IdentityNo;
            }
            if (IdentityNo > 99999 && IdentityNo < 999999)
            {
                CaseNo = "WT" + IdentityNo;
            }
            if (!string.IsNullOrEmpty(ddlCutCondition.SelectedItem.Value))
                ddlCutConditionID = Convert.ToInt64(ddlCutCondition.SelectedItem.Value);
            //long ddlCutConditionID = ddlCutCondition.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlCutCondition.SelectedItem.Value);
            ChannelWatertheftCase.ChannelID = Convert.ToInt32(ddlChannels.SelectedItem.Value);
            ChannelWatertheftCase.TheftSiteRD = Convert.ToInt32(ChannelRDs);
            ChannelWatertheftCase.OffenceSide = ddlChannelSide.SelectedItem.Value;
            ChannelWatertheftCase.IncidentDateTime = _FinalDateofChecking;
            ChannelWatertheftCase.OffenceTypeID = Convert.ToInt32(ddlOffenceType.SelectedItem.Value);
            ChannelWatertheftCase.TheftSiteConditionID = ddlCutConditionID;
            ChannelWatertheftCase.Remarks = ChannelRemarks.Text;
            ChannelWatertheftCase.OffenceSite = Convert.ToString(ddlOffenceSite.SelectedItem.Value);
            ChannelWatertheftCase.LogDateTime = null;
            ChannelWatertheftCase.CreatedBy = UserId;
            ChannelWatertheftCase.CreatedDate = DateTime.Now;
            ChannelWatertheftCase.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
            ChannelWatertheftCase.CaseNo = CaseNo;
            ChannelWatertheftCase.IsActive = true;
            ChannelWatertheftCase.UserID = UserId;
            ChannelWatertheftCase.Source = "W";
            return ChannelWatertheftCase;
        }
        protected void btnSaveChannelData_Click(object sender, EventArgs e)
        {

            try
            {
                WT_WaterTheftCase _mdlWatertheftCase = PrepareWaterTheftChannelEntity();
                WT_WaterTheftStatus _mdlWaterTheftCaseStatus = PrepareWaterTheftStatusEntity();
                WT_OutletDefectiveDetails _mdloutletDefectiveDetails = PrepareOutletDefectiveDetail();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long DivisionID = -1;
                string ChannelName = ddlChannels.SelectedItem.Text;
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.WaterTheft);
                long StatusID = new WaterTheftBLL().AddWaterTheftCaseChannelorOutlet(mdlUser.ID, _mdlWatertheftCase, _mdlWaterTheftCaseStatus, _mdloutletDefectiveDetails
                    , DivisionID, 1, lstNameofFiles, Convert.ToString(Constants.ComplaintModuleReference.WT_C), 0, 0, "W", ChannelName);

                if (StatusID == -1)
                {
                    Master.ShowMessage(Message.EnterCorrectRangeRD.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -2)
                {
                    Master.ShowMessage(Message.ReleventSBE.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -3)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (StatusID == -4)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }
                //NotifyEvent _event = new NotifyEvent();
                //_event.Parameters.Add("WaterTheftID", StatusID);
                //_event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddWaterTheftCase, SessionManagerFacade.UserInformation.ID);
                Master.ShowMessage("Case ID :  " + _mdlWatertheftCase.CaseNo + " " + Message.WaterTheftRecordAdded.Description, SiteMaster.MessageType.Success);
                ResetForm();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void ResetForm()
        {

            if (ddlChannels.SelectedIndex > 0)
            {
                ddlChannels.SelectedIndex = 0;
            }


            txtOutletRDLeft.Text = String.Empty;
            txtOutletRDRight.Text = String.Empty;
            ddlChannelSide.SelectedIndex = 0;
            txtDateofChecking.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now)); ;
            //txtTimeofChecking.Text = String.Empty;
            ddlOffenceType.SelectedIndex = 0;
            ddlCutCondition.SelectedIndex = 0;
            ddlCutCondition.Enabled = false;
            ChannelRemarks.Text = String.Empty;
            if (ddlChannel.SelectedIndex > 0)
            {
                ddlChannel.SelectedIndex = 0;
            }

            if (ddlOutlet.SelectedIndex > 0)
            {
                ddlOutlet.SelectedIndex = 0;
                //int outletId = ddlOutlet.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlOutlet.SelectedItem.Value);
                //if (outletId == -1)
                //{
                txtType.Text = String.Empty;
                txtRDS.Text = String.Empty;
                txtSide.Text = String.Empty;

                // }
            }
            txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now)); ;
            //  txtIncidentTime.Text = String.Empty;
            ddlTheftType.SelectedIndex = 0;
            ddlOutletCondition.SelectedIndex = 0;
            txtHValue.Text = String.Empty;
            ddlDefectiveType.SelectedIndex = 0;
            txtBValue.Text = String.Empty;
            txtYValue.Text = String.Empty;
            txtDIAValue.Text = String.Empty;
            txtRemarks.Text = String.Empty;

            if (ddlDivision.SelectedIndex > 0)
            {
                ddlDivision.SelectedIndex = 0;
            }

            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (mdlUser.DesignationID == (long)Constants.Designation.SBE || mdlUser.DesignationID == (long)Constants.Designation.SDO || mdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                ddlChannel.Enabled = true;
                ddlChannels.Enabled = true;
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                ddlChannel.Enabled = false;
                ddlChannels.Enabled = false;
                if (ddlChannels.SelectedIndex > 0)
                {
                    ddlChannels.SelectedIndex = 0;
                }

                if (ddlChannel.SelectedIndex > 0)
                {
                    ddlChannel.SelectedIndex = 0;
                }
            }
            else if (mdlUser.DesignationID != null)
            {
                ddlChannel.Enabled = false;
                ddlChannels.Enabled = false;
                if (ddlChannels.SelectedIndex > 0)
                {
                    ddlChannels.SelectedIndex = 0;
                }

                if (ddlChannel.SelectedIndex > 0)
                {
                    ddlChannel.SelectedIndex = 0;
                }
                //ddlChannels.SelectedIndex = 0;
                //ddlChannel.SelectedIndex = 0;
            }
            ddlOutlet.Enabled = false;
            OutletInfos.Visible = false;
        }
    }
}