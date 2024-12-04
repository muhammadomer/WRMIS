using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class BreachCaseView : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long breachCaseId = 0;
            try
            {

                if (!IsPostBack)
                {
                    SetTitle();
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    BindDropDown(mdlUser);
                    BindDivisionDropdown(mdlUser);


                    if (!string.IsNullOrEmpty(Request.QueryString["BreachCaseID"]))
                    {
                        breachCaseId = Convert.ToInt64(Request.QueryString["BreachCaseID"]);
                        BreachCaseId.Value = Convert.ToString(breachCaseId);
                        BindChannelDropdown(mdlUser, breachCaseId);
                        LoadBreachCaseData(breachCaseId);
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["ET"])) // from employee tracking 
                    {
                        lbtnBackToET.Visible = true;
                        hlBack.Visible = false;
                    }
                    else
                    {
                        lbtnBackToET.Visible = false;
                        hlBack.Visible = true;
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.BreachCaseView);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDivisionDropdown(UA_Users _MdlUser)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);

                ddlDivision.Enabled = false;
            }
            else if (_MdlUser.DesignationID != null && _MdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, 0, (int)Constants.DropDownFirstOption.Select);
            }

            else if (_MdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);

            }

        }

        /// <summary>
        /// This function binds the Channel dropdown
        /// Created on 10-05-2016
        /// </summary>
        /// <param name="_MdlUser"></param>
        private void BindChannelDropdown(UA_Users _MdlUser, long breachCaseId)
        {
            if (_MdlUser.DesignationID == (long)Constants.Designation.SBE || _MdlUser.DesignationID == (long)Constants.Designation.SDO || _MdlUser.DesignationID == (long)Constants.Designation.Ziladaar)
            {
                ddlChannel.Enabled = true;
                long DivisionID = -1;
                WT_Breach ObjBreach = new WaterTheftBLL().GetBreachCaseDatebyID(breachCaseId);
                DivisionID = Convert.ToInt64(ObjBreach.DivisionID);
                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, _MdlUser.ID, (long)_MdlUser.UA_Designations.IrrigationLevelID, DivisionID, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                long DivisionID = -1;
                WT_Breach ObjBreach = new WaterTheftBLL().GetBreachCaseDatebyID(breachCaseId);
                DivisionID = Convert.ToInt64(ObjBreach.DivisionID);
                Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlChannel, _MdlUser.ID, 0, DivisionID, (int)Constants.DropDownFirstOption.Select);

            }

        }
        private void BindDropDown(UA_Users mdlUser)
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlSide, CommonLists.GetWTChannelSides());
        }
        private void LoadBreachCaseData(long _BreachCaseId)
        {
            WT_Breach ObjBreach = new WT_Breach();
            string LeftRD = string.Empty;
            string RightRD = string.Empty;
            ObjBreach = new WaterTheftBLL().GetBreachCaseDatebyID(_BreachCaseId);
            Dropdownlist.SetSelectedValue(ddlChannel, ObjBreach.ChannelID.ToString());
            Dropdownlist.SetSelectedValue(ddlDivision, ObjBreach.DivisionID.ToString());
            string RDs = ObjBreach.BreachSiteRD.ToString();
            if (!string.IsNullOrEmpty(RDs))
            {
                Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(RDs));
                LeftRD = tupleRD.Item1;
                RightRD = tupleRD.Item2;
            }
            txtOutletRDLeft.Text = LeftRD;
            txtOutletRDRight.Text = RightRD;
            Dropdownlist.SetSelectedValue(ddlSide, ObjBreach.BreachSide.ToString());
            if (Convert.ToBoolean(ObjBreach.FieldStaff))
            {
                ddlFieldStaff.SelectedItem.Text = "Yes";
            }
            else
            {
                ddlFieldStaff.SelectedItem.Text = "No";
            }
            DateTime datetime = Convert.ToDateTime(ObjBreach.DateTime);
            string BreachDate = datetime.ToString("yyyy-MM-dd");
            string IncidentTime = datetime.ToString("HH:mm tt");
            txtIncidentDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime(BreachDate)));
            txtIncidentTime.Text = IncidentTime;
            txtHeadDischarge.Text = ObjBreach.HeadDischarge.ToString();
            txtLengthofbreach.Text = ObjBreach.BreachLength.ToString();
            txtRemarks.Text = ObjBreach.Remarks.ToString();

        }

        protected void lnkViewAttachments_Click(object sender, EventArgs e)
        {
            try
            {
                long breachCaseId = Convert.ToInt64(Request.QueryString["BreachCaseID"]);
                List<dynamic> lstWTCaseWorking = new WaterTheftBLL().GetBreachCaseAttachment(breachCaseId);
                gvViewAttachment.DataSource = lstWTCaseWorking;
                gvViewAttachment.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Attachments", "$('#viewAttachment').modal();", true);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string AttachmentPath = Convert.ToString(gvViewAttachment.DataKeys[e.Row.RowIndex].Value);
                List<string> lstName = new List<string>();
                lstName.Add(AttachmentPath);
                WebFormsTest.FileUploadControl FileUploadControl = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl");
                FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl.Size = lstName.Count;
                FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.WaterTheft, lstName);


            }
        }

    }
}