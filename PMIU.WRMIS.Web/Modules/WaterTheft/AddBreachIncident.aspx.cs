using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class AddBreachIncident : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    SetTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    BindDropDown(mdlUser);
                    BindDivisionDropdown(mdlUser);
                    BindChannelDropdown(mdlUser);
                    Dropdownlist.DDLYesNo(ddlFieldStaff, (int)Constants.DropDownFirstOption.Select);
                    //ddlChannel.SelectedIndex = 0;
                    //ddlChannel.Enabled = false;
                    if (mdlUser.DesignationID == (long)Constants.Designation.SBE || mdlUser.DesignationID == (long)Constants.Designation.SDO || mdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
                    {
                        ddlChannel.Enabled = true;
                    }
                    else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                    {
                        ddlChannel.Enabled = false;
                    }
                    else if (mdlUser.DesignationID != null)
                    {
                        ddlChannel.Enabled = false;
                    }
                    btnSaveBreachData.Visible = base.CanAdd;
                  //  txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddBreachIncident);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar )
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
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar )
            {
                ddlChannel.Enabled = true;

                long DivisionID = -1;

                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);

            }
        }

        private void BindDropDown(UA_Users mdlUser)
        {
            long UserId = mdlUser.ID;
            if (mdlUser.DesignationID == null)
                return;
            Dropdownlist.BindDropdownlist<List<object>>(ddlSide, CommonLists.GetBreachSides());
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    ddlChannel.Enabled = true;

                    UA_Users mdlUser = SessionManagerFacade.UserInformation;

                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                    {
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                       
                    }
                    else
                    {
                        Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
                        
                    }

                }
                else
                {
                    ddlChannel.SelectedIndex = 0;

                    ddlChannel.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private WT_Breach  PreparedWTBreachEntity()
        {
            
            long UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            string TheftTime = BreachTimePicker.GetTime();
            WT_Breach BreachCase = new WT_Breach();
            string CurrentDateTime = Utility.GetFormattedDate(DateTime.Now);
            BreachCase.ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlChannel.SelectedItem.Value);
            Decimal ChannelRDs = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
            BreachCase.BreachSiteRD = Convert.ToInt32(ChannelRDs);
            BreachCase.BreachSide = ddlSide.SelectedItem.Value;
            BreachCase.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            DateTime _FinalIncidentDateTime = Utility.GetParsedDateTime(txtIncidentDate.Text, TheftTime);
            BreachCase.DateTime = _FinalIncidentDateTime;
            BreachCase.HeadDischarge =Convert.ToDouble(txtHeadDischarge.Text);
            BreachCase.BreachLength = Convert.ToDouble(txtLengthofbreach.Text);
            BreachCase.Remarks = txtRemarks.Text;
            if (ddlFieldStaff.SelectedValue != "")
            {
                BreachCase.FieldStaff = Convert.ToBoolean(ddlFieldStaff.SelectedValue == "1" ? "True" : "False");
            }
			
            BreachCase.UserID = UserId;
            BreachCase.LogDateTime = null;
            BreachCase.CreatedBy = UserId;
            BreachCase.CreatedDate = Convert.ToDateTime(CurrentDateTime);

            WT_Breach CaseID = new WaterTheftBLL().GetBreachCaseID();
            long? IdentityNo = 0;
            if (CaseID == null)
            {
                IdentityNo = 1;
            }
            else
            {
                IdentityNo = CaseID.ID + 1;
            }

            string CaseNo = "";
            if (IdentityNo < 99)
            {
                CaseNo = "BR0000" + IdentityNo;
            }
            if (IdentityNo > 99 && IdentityNo < 999)
            {
                CaseNo = "BR000" + IdentityNo;
            }
            if (IdentityNo > 999 && IdentityNo < 9999)
            {
                CaseNo = "BR00" + IdentityNo;
            }
            if (IdentityNo > 9999 && IdentityNo < 99999)
            {
                CaseNo = "BR0" + IdentityNo;
            }
            if (IdentityNo > 99999 && IdentityNo < 999999)
            {
                CaseNo = "BR" + IdentityNo;
            }

            BreachCase.CaseNo = CaseNo;
            return BreachCase;
        }

        protected int VerfiyChannelRDs()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            long UserId = mdlUser.ID;
            if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                return 1;
            long IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;
            int ChannelId = Convert.ToInt32(ddlChannel.SelectedItem.Value);
            DataTable ChannelRDs = new WaterTheftBLL().VerfiyChannelRDs(ChannelId, UserId, IrrigationLevelID);
            Decimal ChannelRD = Calculations.CalculateTotalRDs(txtOutletRDLeft.Text, txtOutletRDRight.Text);
            int MinRDs = ChannelRDs.AsEnumerable().Min(r => r.Field<int>("MinRD"));
            int MaxRDs = ChannelRDs.AsEnumerable().Max(r => r.Field<int>("MaxRD"));

            if (ChannelRD >= MinRDs && ChannelRD <= MaxRDs)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        protected void btnSaveBreachData_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                WT_Breach _mdlWaterTheftBreach = PreparedWTBreachEntity();


                if (_mdlWaterTheftBreach.DateTime > DateTime.Now)
                {
                    Master.ShowMessage(Message.EnterCorrectDate.Description, SiteMaster.MessageType.Error);
                    return;
                }

                int status = VerfiyChannelRDs();
                if (status == 0)
                {
                    Master.ShowMessage( Message.EnterCorrectRangeRD.Description, SiteMaster.MessageType.Error);
                    return;
                }

               
                WT_FeettoIgnore lstFeetToIgnor = new WaterTheftBLL().FeetToIgnore();
                int NoofFeet = lstFeetToIgnor.NoOfFeet;

                bool IsCaseExist = new WaterTheftBLL().IsBreachCaseExist(_mdlWaterTheftBreach, NoofFeet);
                //int Count = 1;
                // If Case is already exist
                if (IsCaseExist)
                {
                    //Hardcoded...
                    Master.ShowMessage( Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }

                long _BreachID = new WaterTheftBLL().AddBreachIncidentCase(_mdlWaterTheftBreach);
                long UserID = mdlUser.ID;
                long DesignationID = Convert.ToInt64(mdlUser.DesignationID);
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.WaterTheft);
                DataTable WTAttachmentTable = WaterTheftCase.GetDataTableForBreach(_BreachID, UserID, DesignationID, lstNameofFiles);
                int StatusID = new WaterTheftBLL().SaveBreachAttachments(WTAttachmentTable);

                PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                _event.Parameters.Add("BreachID", _BreachID);
                _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddBreachCase, SessionManagerFacade.UserInformation.ID);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                Response.Redirect("SearchBreachIncident.aspx?RP=1", false);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}