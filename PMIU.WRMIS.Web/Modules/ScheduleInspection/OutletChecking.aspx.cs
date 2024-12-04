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
using PMIU.WRMIS.BLL.ScheduleInspection;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class OutletChecking : BasePage
    {
        long ScheduleChannelDetailID = 0;
        long OutletCheckingID = 0;
        string ScheduleID = "";
        long ChannelID = 0;
        string Outlet = "";
        string From = "";
        SI_OutletChecking oc;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    SetTitle();
                    txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    #region Get URL parameters
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        ScheduleChannelDetailID = Convert.ToInt64(Request.QueryString["ScheduleDetailID"]);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["OutletCheckingID"]))
                    {
                        OutletCheckingID = Convert.ToInt64(Request.QueryString["OutletCheckingID"]);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["Outlet"]))
                    {
                        Outlet = Request.QueryString["Outlet"];
                    }
                   
                    #endregion
                    if (!string.IsNullOrEmpty(Request.QueryString["ET"])) // from employee tracking 
                    {
                        lbtnBackToET.Visible = true;
                        hlBack.Visible = false;
                        btnSaveOutletIncidentData.Visible = false;
                    }

                    // When come only for View OutletChecking
                    if (OutletCheckingID > 0)
                    {
                         oc = new ScheduleInspectionBLL().GetOutletCheckingByID(OutletCheckingID);
                        BindFormControls(oc);
                    }
                    // When come for ADD OutletChecking Record
                    else
                    {
                        if (Outlet.Contains('/'))
                        {
                            BindDropDown(ChannelID, GetOutlet(Outlet));    
                        }
                        else
                        {
                            BindDropDown(ChannelID,"", Convert.ToInt64(Outlet));   
                        }
                        
                        ddlOutlet.Enabled = false;
                        ddlChannel.Enabled = false;
                        btnSaveOutletIncidentData.Visible = base.CanAdd;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "RemoveRequiredAtAddTime();", true);

                    }
                    txtIncidentDate.Enabled = false;
                    txtIncidentDate.CssClass = "aspNetDisabled form-control disabled-future-date-picker";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["From"]))
                {
                    string url = "";
                    From = Request.QueryString["From"];
                    ScheduleID=Request.QueryString["ScheduleID"];
                    if (From=="IN") // Inspection Notes
                    {
                        url = String.Format("ScheduleInspectionNotes.aspx?ScheduleID={0}", ScheduleID);    
                    }
                    else if (From=="SI") // Scheduel Inspection
                    {
                        url = String.Format("ScheduleInspection.aspx?From={0}", "OutletChecking");    
                    }
                    else if (From == "SC") // Schedule Calander
                    {
                        url = String.Format("ScheduleCalendar.aspx?From={0}", "OutletChecking");
                    }
                    
                    hlBack.NavigateUrl = url;
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public string GetOutlet(string Outlet)
        {
            string[] parsOutLet = Outlet.Split('/');
            string p1 = "";
            string p2 = "";
            string finalOutLet = "";
            if (parsOutLet[0].Length > 4)
            {
                p1 = parsOutLet[0].Substring(0, 2);
                p2 = parsOutLet[0].Substring(2);
            }
            else
            {
                p1 = parsOutLet[0].Substring(0, 1);
                p2 = parsOutLet[0].Substring(1);
            }
            finalOutLet = p1 + "+" + p2 + "/" + parsOutLet[1];
            return finalOutLet;
        }

        public void BindFormControls(SI_OutletChecking oc)
        {
            BindDropDown(oc.SI_ScheduleDetailChannel.ChannelID, "", oc.OutletID);
            txtHValue.Text = Convert.ToString(oc.HValue);
            Dropdownlist.SetSelectedText(ddlOutletCondition, oc.OutletCheckCondition);
            txtRemarks.Text = oc.Remarks;
            txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(oc.ReadingMobileDate));
            TimePicker.SetTime(Convert.ToString(oc.ReadingMobileDate.TimeOfDay));
            ddlOutlet.Enabled = false;
            ddlChannel.Enabled = false;
            btnSaveOutletIncidentData.Visible = false;
            txtRemarks.Enabled = false;
            txtRemarks.CssClass = "aspNetDisabled form-control";
            txtHValue.Enabled = false;
            TimePicker.DisbaleTimePicker();
            txtHValue.CssClass = "aspNetDisabled form-control";
            ddlOutletCondition.Enabled = false;
            txtIncidentDate.Enabled = false;
            txtIncidentDate.CssClass = "aspNetDisabled form-control disabled-future-date-picker";
            FileUploadControl.Visible = false;

            if (!string.IsNullOrEmpty(oc.Attachment))
            {
                fileAttachment.NavigateUrl = Utility.GetImageURL(Configuration.ScheduleInspection, oc.Attachment);
                fileAttachment.Text = oc.Attachment.Substring(oc.Attachment.LastIndexOf('_') + 1);
                fileAttachment.Attributes["FullName"] = oc.Attachment;
                fileAttachment.Visible = true;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "RemoveRequiredatViewTimeAndDisableDateCrossIcone();", true);
        }
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OutletChecking);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }



        /// <summary>
        /// This function binds the Channel dropdown
        /// Created on 10-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>


        private void BindDropDown(long ChannelID, string OutletName = "", long OutletID = 0)
        {
            CO_Channel chnl = new ChannelBLL().GetChannelByID(ChannelID);
            List<object> lstChannel = new List<object>();
            lstChannel.Add(new { ID = chnl.ID, Name = chnl.NAME });
            Dropdownlist.BindDropdownlist<List<object>>(ddlChannel, lstChannel, (int)Constants.DropDownFirstOption.NoOption);
            ddlChannel_SelectedIndexChanged(null, null);
            ddlOutlet.ClearSelection();
            if (OutletID == 0)
            {
                Dropdownlist.SetSelectedText(ddlOutlet, OutletName);
            }
            else
            {
                Dropdownlist.SetSelectedValue(ddlOutlet, OutletID.ToString());
            }
            ddlOutlet_SelectedIndexChanged(null, null);
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutletCondition, CommonLists.GetOutletConditionType(), (int)Constants.DropDownFirstOption.Select);
        }
        private string GetOutletInformation(dynamic _WaterTheftIncident, string _PropertyName)
        {
            return Convert.ToString(_WaterTheftIncident.GetType().GetProperty(_PropertyName).GetValue(_WaterTheftIncident, null));
        }
        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            long ChannelId = ddlChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt32(ddlChannel.SelectedItem.Value);
            if (ChannelId != -1)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Outlet"]))
                {
                    Outlet = Request.QueryString["Outlet"];
                }
                if (Outlet.Contains('/'))
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new ScheduleInspectionBLL().GetOutletByOutletID_OR_OutletName(0,Outlet));
                }
                else
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new ScheduleInspectionBLL().GetOutletByOutletID_OR_OutletName(Convert.ToInt32(Outlet)));
                }
               
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
            //try
            //{
            //    if (ddlDivision.SelectedItem.Value != string.Empty)
            //    {
            //        ddlChannel.Enabled = true;
            //        ddlChannels.Enabled = true;

            //        UA_Users mdlUser = SessionManagerFacade.UserInformation;

            //        long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

            //        if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            //        {
            //            Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
            //            Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannels, mdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
            //        }
            //        else
            //        {
            //            Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
            //            Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannels, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select, true);
            //        }

            //    }
            //    else
            //    {
            //        ddlChannel.SelectedIndex = 0;
            //        ddlChannel.Enabled = false;
            //        ddlChannels.SelectedIndex = 0;
            //        ddlChannels.Enabled = false;
            //        ddlOutlet.SelectedIndex = 0;
            //        ddlOutlet.Enabled = false;
            //        OutletInfos.Visible = false;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        private SI_OutletChecking PrepareOutletEntity()
        {
            long UserId = Convert.ToInt32(Session[SessionValues.UserID]);
            DateTime IncidentDateTime = Convert.ToDateTime(txtIncidentDate.Text);
            string time = TimePicker.GetTime();
            DateTime _FinalIncidentDateTime = PMIU.WRMIS.Common.Utility.GetParsedDateTime(txtIncidentDate.Text, time);
            List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.ScheduleInspection);
            SI_OutletChecking oc = new SI_OutletChecking();
            oc.OutletID = Convert.ToInt32(ddlOutlet.SelectedItem.Value);
            oc.ReadingMobileDate = _FinalIncidentDateTime;
            oc.OutletCheckCondition = ddlOutletCondition.SelectedItem.Text;
            oc.Remarks = txtRemarks.Text;
            oc.Attachment = lstNameofFiles[0].Item3;
            oc.ScheduleDetailChannelID = Convert.ToInt64(Request.QueryString["ScheduleDetailID"]);
            oc.HValue = txtHValue.Text == string.Empty ? -1 : Convert.ToDouble(txtHValue.Text);
            oc.CreatedBy = UserId;
            oc.CreatedDate = DateTime.Now;
            //oc.Remarks = txtRemarks.Text;
            oc.Source = "W";
            return oc;
        }


        protected void btnSaveOutletCheckingData_Click(object sender, EventArgs e)
        {

            try
            {
                SI_OutletChecking _oc = PrepareOutletEntity();
                bool Status = new ScheduleInspectionBLL().AddOutletChecking(_oc);
                _oc.SI_ScheduleDetailChannel = new SI_ScheduleDetailChannel();
                _oc.SI_ScheduleDetailChannel.ChannelID = Convert.ToInt64(ddlChannel.SelectedValue);
                if (!Status)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (Convert.ToInt64(ddlOutletCondition.SelectedValue) == (long)Constants.OutletConditionOnOutletChecking.WaterTheft)
                    {
                        Response.Redirect("../WaterTheft/AddWaterTheft.aspx");
                    }
                    BindFormControls(_oc);
                    hlBack.Attributes.Add("onclick", "javascript:history.go(-2);");
                    btnSaveOutletIncidentData.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }


        private void ResetForm()
        {
            if (ddlChannel.SelectedIndex > 0)
            {
                ddlChannel.SelectedIndex = 0;
            }
            if (ddlOutlet.SelectedIndex > 0)
            {
                ddlOutlet.SelectedIndex = 0;
                txtType.Text = String.Empty;
                txtRDS.Text = String.Empty;
                txtSide.Text = String.Empty;
            }
            txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));

            ddlOutletCondition.SelectedIndex = 0;
            txtHValue.Text = String.Empty;
            txtRemarks.Text = string.Empty;
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (mdlUser.DesignationID == (long)Constants.Designation.SBE || mdlUser.DesignationID == (long)Constants.Designation.SDO || mdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                ddlChannel.Enabled = true;
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                ddlChannel.Enabled = false;
                if (ddlChannel.SelectedIndex > 0)
                {
                    ddlChannel.SelectedIndex = 0;
                }
            }
            else if (mdlUser.DesignationID != null)
            {
                ddlChannel.Enabled = false;
                if (ddlChannel.SelectedIndex > 0)
                {
                    ddlChannel.SelectedIndex = 0;
                }

            }
            ddlOutlet.Enabled = false;
            OutletInfos.Visible = false;
        }
    }
}