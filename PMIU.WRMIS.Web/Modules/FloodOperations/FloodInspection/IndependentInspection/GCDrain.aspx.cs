using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class GCDrain : BasePage
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
                        FloodInspectionDetail1.ShowInspectionStatusProp = false;
                        hdnFloodInspectionsID.Value = Convert.ToString(floodInspectionID);
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(floodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
                        LoadIGCDrain(floodInspectionID);
                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";
                    //hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?FloodInspectionID={0}", floodInspectionID);
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
            Dropdownlist.DDLYesNo(ddlDrainWaterEnvironmentallyTreated, (int)Constants.DropDownFirstOption.Select);
        }

        private void LoadIGCDrain(long _FloodInspectionID)
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            object oIGCDrainInformation = new FloodInspectionsBLL().GetIGCDrainInformationByInspectionID(_FloodInspectionID);

            if (oIGCDrainInformation != null)
            {
                txtExistingCapacity.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("ExistingCapacity").GetValue(oIGCDrainInformation));
                txtImprovedCapacity.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("ImprovedCapacity").GetValue(oIGCDrainInformation));

                txtCurrentLevel.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("CurrentLevel").GetValue(oIGCDrainInformation));

                string valDrainWaterET = null;

                if (
                    Convert.ToString(oIGCDrainInformation.GetType().GetProperty("DrainWaterET").GetValue(oIGCDrainInformation)).ToLower() == "true")
                {
                    valDrainWaterET = "1";
                }
                else if (
                    Convert.ToString(oIGCDrainInformation.GetType().GetProperty("DrainWaterET").GetValue(oIGCDrainInformation)).ToLower() == "false")
                {
                    valDrainWaterET = "0";
                }

                Dropdownlist.SetSelectedValue(ddlDrainWaterEnvironmentallyTreated, valDrainWaterET);

                txtBedWidth.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("OutfallBedWidth").GetValue(oIGCDrainInformation));

                txtFullSupplyWidth.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("OutfallFullSupplyDepth").GetValue(oIGCDrainInformation));
                txtNumberOfGovernment.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("BridgeGovtNo").GetValue(oIGCDrainInformation));

                txtNumberOfPrivate.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("BridgePvtNo").GetValue(oIGCDrainInformation));
                txtRemarks.Text = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("Remarks").GetValue(oIGCDrainInformation));

                hdnIGCDrainID.Value = Convert.ToString(Convert.ToInt64(oIGCDrainInformation.GetType().GetProperty("IGCDrainID").GetValue(oIGCDrainInformation)));
                hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
                hdnIGCDrainCreatedBy.Value = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("CreatedBy").GetValue(oIGCDrainInformation));
                hdnIGCDrainCreatedDate.Value = Convert.ToString(oIGCDrainInformation.GetType().GetProperty("CreatedDate").GetValue(oIGCDrainInformation));
            }
            if (hdnInspectionStatus.Value == "2")
            {
                txtExistingCapacity.Enabled = false;
                txtImprovedCapacity.Enabled = false;
                txtCurrentLevel.Enabled = false;
                lblDrainWaterEnvironmentallyTreated.Enabled = false;
                txtBedWidth.Enabled = false;
                txtFullSupplyWidth.Enabled = false;
                txtNumberOfGovernment.Enabled = false;
                txtNumberOfPrivate.Enabled = false;
                txtRemarks.Enabled = false;
                ddlDrainWaterEnvironmentallyTreated.Enabled = false;
                btnSave.Enabled = false;
            }
            if (_InspectionTypeID == 1)
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                if (CanEdit)
                {
                    txtExistingCapacity.Enabled = CanEdit;
                    txtImprovedCapacity.Enabled = CanEdit;
                    txtCurrentLevel.Enabled = CanEdit;
                    lblDrainWaterEnvironmentallyTreated.Enabled = CanEdit;
                    txtBedWidth.Enabled = CanEdit;
                    txtFullSupplyWidth.Enabled = CanEdit;
                    txtNumberOfGovernment.Enabled = CanEdit;
                    txtNumberOfPrivate.Enabled = CanEdit;
                    txtRemarks.Enabled = CanEdit;
                    ddlDrainWaterEnvironmentallyTreated.Enabled = CanEdit;
                    btnSave.Enabled = CanEdit;
                }
                else
                {
                    txtExistingCapacity.Enabled = false;
                    txtImprovedCapacity.Enabled = false;
                    txtCurrentLevel.Enabled = false;
                    lblDrainWaterEnvironmentallyTreated.Enabled = false;
                    txtBedWidth.Enabled = false;
                    txtFullSupplyWidth.Enabled = false;
                    txtNumberOfGovernment.Enabled = false;
                    txtNumberOfPrivate.Enabled = false;
                    txtRemarks.Enabled = false;
                    ddlDrainWaterEnvironmentallyTreated.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            else
            {
                CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                if (CanEdit)
                {
                    txtExistingCapacity.Enabled = CanEdit;
                    txtImprovedCapacity.Enabled = CanEdit;
                    txtCurrentLevel.Enabled = CanEdit;
                    lblDrainWaterEnvironmentallyTreated.Enabled = CanEdit;
                    txtBedWidth.Enabled = CanEdit;
                    txtFullSupplyWidth.Enabled = CanEdit;
                    txtNumberOfGovernment.Enabled = CanEdit;
                    txtNumberOfPrivate.Enabled = CanEdit;
                    txtRemarks.Enabled = CanEdit;
                    ddlDrainWaterEnvironmentallyTreated.Enabled = CanEdit;
                    btnSave.Enabled = CanEdit;
                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FO_IGCDrain iFO_IGCDrainEntity = PrepareIGCDrainEntity();
            try
            {
                //if (new FloodInspectionBLL().IsDivisionSummaryAlreadyExists(iGCProtectionInfrastructureEntity))
                //{
                //  Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                //  return;
                //}

                //if(SessionManagerFacade.UserInformation.RoleID = Constants.d)

                bool isSaved = new FloodInspectionsBLL().SaveIGCDrain(iFO_IGCDrainEntity);

                if (isSaved == true)
                {
                    SearchIndependent.IsSaved = true;
                    //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    //HttpContext.Current.Session.Add("DivisionSummaryID", divisionSummaryEntity.ID);
                    Response.Redirect("SearchIndependent.aspx?FloodInspectionID=" + iFO_IGCDrainEntity.FloodInspectionID, false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                //lblMessage.Text = ex.Message;
            }
        }

        private FO_IGCDrain PrepareIGCDrainEntity()
        {
            FO_IGCDrain iGCDrain = new FO_IGCDrain();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (hdnFloodInspectionsID.Value != null && hdnFloodInspectionsID.Value != "0")
            {
                iGCDrain.FloodInspectionID = Convert.ToInt32(hdnFloodInspectionsID.Value);
            }

            if (hdnIGCDrainID.Value != null && hdnIGCDrainID.Value != "0")
            {
                iGCDrain.ID = Convert.ToInt32(hdnIGCDrainID.Value);
                iGCDrain.CreatedBy = Convert.ToInt32(hdnIGCDrainCreatedBy.Value);
                iGCDrain.CreatedDate = Convert.ToDateTime(hdnIGCDrainCreatedDate.Value);

                iGCDrain.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCDrain.ModifiedDate = DateTime.Now;
            }
            else
            {
                iGCDrain.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCDrain.CreatedDate = DateTime.Now;
            }

            if (txtExistingCapacity.Text != "")
            {
                iGCDrain.ExistingCapacity = Convert.ToDouble(txtExistingCapacity.Text);
            }

            if (txtImprovedCapacity.Text != "")
            {
                iGCDrain.ImprovedCapacity = Convert.ToDouble(txtImprovedCapacity.Text);
            }

            if (txtCurrentLevel.Text != "")
            {
                iGCDrain.CurrentLevel = Convert.ToDouble(txtCurrentLevel.Text);
            }

            if (ddlDrainWaterEnvironmentallyTreated.SelectedValue != "")
                iGCDrain.DrainWaterET = Convert.ToBoolean(ddlDrainWaterEnvironmentallyTreated.SelectedValue == "1" ? "True" : "False");

            if (txtBedWidth.Text != "")
            {
                iGCDrain.OutfallBedWidth = Convert.ToDouble(txtBedWidth.Text);
            }

            if (txtFullSupplyWidth.Text != "")
            {
                iGCDrain.OutfallFullSupplyDepth = Convert.ToDouble(txtFullSupplyWidth.Text);
            }

            if (txtNumberOfGovernment.Text != "")
            {
                iGCDrain.BridgeGovtNo = Convert.ToInt16(txtNumberOfGovernment.Text);
            }

            if (txtNumberOfPrivate.Text != "")
            {
                iGCDrain.BridgePvtNo = Convert.ToInt16(txtNumberOfPrivate.Text);
            }

            if (txtRemarks.Text != "")
            {
                iGCDrain.Remarks = txtRemarks.Text.Trim();
            }

            return iGCDrain;
        }
    }
}