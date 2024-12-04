using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.FloodOperations;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class GCProtectionInfrastructure : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdown();
                    txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString());
                    int floodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    //hdnProtectioninfrastructure.Value = floodInspectionsID;

                    if (floodInspectionID > 0)
                    {
                        FloodInspectionDetail1.FloodInspectionIDProp = floodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                        LoadIGCProtectionInfrastructure(floodInspectionID);
                    }
                    // hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx?InfrastructureID={0}", ParentInfrastructureID);
                    //BindParentTypeDropDown();
                    //CheckParentName(Convert.ToInt64(ParentInfrastructureID));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDropdown()
        {

            Dropdownlist.DDLInspectionConditionsByGroup(ddlGaugeReaderHutCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlWatchingHutCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLYesNo(ddlRiverGaugeCondition, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlServiceRoadCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FO_IGCProtectionInfrastructure iGCProtectionInfrastructureEntity = PrepareIGCProtectionInfrastructureEntity();
            try
            {
                //if (new FloodInspectionBLL().IsDivisionSummaryAlreadyExists(iGCProtectionInfrastructureEntity))
                //{
                //  Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                //  return;
                //}

                //if(SessionManagerFacade.UserInformation.RoleID = Constants.d)


                bool isSaved = new FloodInspectionsBLL().SaveIGCProtectionInfrastructure(iGCProtectionInfrastructureEntity);
                if (isSaved)
                {
                    SearchIndependent.IsSaved = true;
                    //Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    //HttpContext.Current.Session.Add("DivisionSummaryID", divisionSummaryEntity.ID);
                    Response.Redirect("SearchIndependent.aspx?FloodInspectionID=" + iGCProtectionInfrastructureEntity.FloodInspectionID, false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                //lblMessage.Text = ex.Message;
            }
        }

        private FO_IGCProtectionInfrastructure PrepareIGCProtectionInfrastructureEntity()
        {
            FO_IGCProtectionInfrastructure iGCProtectionInfrastructure = new FO_IGCProtectionInfrastructure();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            iGCProtectionInfrastructure.ID = Convert.ToInt64(hdnGCProtectionInfrastructureID.Value);
            if (hdnFloodInspectionsID.Value != null && hdnFloodInspectionsID.Value != "0")
            {
                iGCProtectionInfrastructure.FloodInspectionID = Convert.ToInt32(hdnFloodInspectionsID.Value);
            }
            if (iGCProtectionInfrastructure.ID == 0)
            {
                iGCProtectionInfrastructure.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCProtectionInfrastructure.CreatedDate = DateTime.Now;
            }
            else
            {
                iGCProtectionInfrastructure.CreatedBy = Convert.ToInt32(hdnGCProtectionInfrastructureCreatedBy.Value);
                iGCProtectionInfrastructure.CreatedDate = Convert.ToDateTime(hdnGCProtectionInfrastructureCreatedDate.Value);
                iGCProtectionInfrastructure.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCProtectionInfrastructure.ModifiedDate = DateTime.Now;
            }
            if (ddlGaugeReaderHutCondition.SelectedValue != "")
            {
                iGCProtectionInfrastructure.GRHutConditionID = Convert.ToInt16(ddlGaugeReaderHutCondition.SelectedValue);
            }
            if (ddlWatchingHutCondition.SelectedValue != "")
            {
                iGCProtectionInfrastructure.WatchingHutConditionID = Convert.ToInt16(ddlWatchingHutCondition.SelectedValue);
            }
            if (ddlServiceRoadCondition.SelectedValue != "")
            {
                iGCProtectionInfrastructure.ServiceRoadConditionID = Convert.ToInt16(ddlServiceRoadCondition.SelectedValue);
            }
            if (ddlRiverGaugeCondition.SelectedValue != "")
            {
                iGCProtectionInfrastructure.RiverGauge = Convert.ToBoolean(ddlRiverGaugeCondition.SelectedValue == "1" ? "True" : "False");
            }

            iGCProtectionInfrastructure.Remarks = txtRemarks.Text.Trim();
            return iGCProtectionInfrastructure;
        }


        private void LoadIGCProtectionInfrastructure(long _FloodInspectionID)
        {

            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;

            FO_IGCProtectionInfrastructure iGCProtectionInfrastructure = new FloodInspectionsBLL().GetIGCProtectionInfrastructureByInspectionID(_FloodInspectionID);

            if (iGCProtectionInfrastructure != null)
            {
                Dropdownlist.SetSelectedValue(ddlGaugeReaderHutCondition, Convert.ToString(iGCProtectionInfrastructure.GRHutConditionID));
                //Dropdownlist.SetSelectedValue(ddlRiverGaugeCondition, Convert.ToString(iGCProtectionInfrastructure.RiverGauge == true ? "1" : "0"));
                Dropdownlist.SetSelectedValue(ddlRiverGaugeCondition, Convert.ToString(iGCProtectionInfrastructure.RiverGauge == true ? "1" : Convert.ToString(iGCProtectionInfrastructure.RiverGauge == false ? "0" : null)));
                Dropdownlist.SetSelectedValue(ddlServiceRoadCondition, Convert.ToString(iGCProtectionInfrastructure.ServiceRoadConditionID));
                Dropdownlist.SetSelectedValue(ddlWatchingHutCondition, Convert.ToString(iGCProtectionInfrastructure.WatchingHutConditionID));
                txtRemarks.Text = Convert.ToString(iGCProtectionInfrastructure.Remarks);

                hdnGCProtectionInfrastructureID.Value = Convert.ToString(iGCProtectionInfrastructure.ID);
                hdnFloodInspectionsID.Value = Convert.ToString(iGCProtectionInfrastructure.FloodInspectionID);
                hdnGCProtectionInfrastructureCreatedBy.Value = Convert.ToString(iGCProtectionInfrastructure.CreatedBy);
                hdnGCProtectionInfrastructureCreatedDate.Value = Convert.ToString(iGCProtectionInfrastructure.CreatedDate);

                //if (hdnInspectionStatus.Value == "2")
                //{
                ddlGaugeReaderHutCondition.Enabled = false;
                ddlRiverGaugeCondition.Enabled = false;
                ddlServiceRoadCondition.Enabled = false;
                ddlWatchingHutCondition.Enabled = false;
                txtRemarks.Enabled = false;
                btnSave.Enabled = false;
                //}
            }
            if (hdnInspectionStatus.Value == "2")
            {
                ddlGaugeReaderHutCondition.Enabled = false;
                ddlRiverGaugeCondition.Enabled = false;
                ddlServiceRoadCondition.Enabled = false;
                ddlWatchingHutCondition.Enabled = false;
                txtRemarks.Enabled = false;
                btnSave.Enabled = false;
            }
            if (_InspectionTypeID == 1)
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                if (CanEdit)
                {
                    ddlGaugeReaderHutCondition.Enabled = CanEdit;
                    ddlRiverGaugeCondition.Enabled = CanEdit;
                    ddlServiceRoadCondition.Enabled = CanEdit;
                    ddlWatchingHutCondition.Enabled = CanEdit;
                    txtRemarks.Enabled = CanEdit;
                    btnSave.Enabled = CanEdit;
                }
                else
                {
                    ddlGaugeReaderHutCondition.Enabled = false;
                    ddlRiverGaugeCondition.Enabled = false;
                    ddlServiceRoadCondition.Enabled = false;
                    ddlWatchingHutCondition.Enabled = false;
                    txtRemarks.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            else
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                if (CanEdit)
                {
                    ddlGaugeReaderHutCondition.Enabled = CanEdit;
                    ddlRiverGaugeCondition.Enabled = CanEdit;
                    ddlServiceRoadCondition.Enabled = CanEdit;
                    ddlWatchingHutCondition.Enabled = CanEdit;
                    txtRemarks.Enabled = CanEdit;
                    btnSave.Enabled = CanEdit;
                }

            }



            //UA_Users mdlUser = SessionManagerFacade.UserInformation;

            //if (iGCProtectionInfrastructure.ID != null && iGCProtectionInfrastructure.ID>0)
            //{
            //  iGCProtectionInfrastructure.CreatedBy = iGCProtectionInfrastructure.CreatedBy;
            //  iGCProtectionInfrastructure.CreatedDate = iGCProtectionInfrastructure.CreatedDate;
            //  iGCProtectionInfrastructure.ModifiedBy = Convert.ToInt32(mdlUser.ID);
            //  iGCProtectionInfrastructure.ModifiedDate = DateTime.Now;
            //}
            //else
            //{
            //  iGCProtectionInfrastructure.CreatedBy = Convert.ToInt32(mdlUser.ID);
            //  iGCProtectionInfrastructure.CreatedDate = DateTime.Now;
            //}

        }

    }
}