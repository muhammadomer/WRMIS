using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection
{
    public partial class GCBarrageHW : BasePage
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
                        LoadIGBarrageHW(floodInspectionID);
                        //if (hdnInspectionStatus.Value == "2")
                        //{
                        btnSave.Enabled = false;
                        txtRemarks.Enabled = false;
                        txtElectronicsWorkingGate.Enabled = false;
                        //}
                    }
                    //hlBack.NavigateUrl = "~/Modules/FloodOperations/FloodInspection/IndependentInspection/SearchIndependent.aspx?ShowHistory=true";

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
            Dropdownlist.DDLAvailableNotAvailable(ddlPoliceMonitoryCondition);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlLightingCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlDataBoardCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlTollHutCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLInspectionConditionsByGroup(ddlArmyCheckPostCondition, "Common", false, (int)Constants.DropDownFirstOption.Select);
        }

        private void LoadIGBarrageHW(long _FloodInspectionID)
        {
            int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
            int _InspectionTypeID = Utility.GetNumericValueFromQueryString("InspectionTypeID", 0);
            bool CanEdit = false;
            object oIGBarrageHWInformation = new FloodInspectionsBLL().GetIGCBarrageHWInformationByInspectionID(_FloodInspectionID);

            if (oIGBarrageHWInformation != null)
            {
                List<object> oIGCBarrageHWGatesInformation = new FloodInspectionsBLL().GetIGCBarrageHWGatesInformationByBarrageHWID(Convert.ToInt64(oIGBarrageHWInformation.GetType().GetProperty("IGCBarrageHWID").GetValue(oIGBarrageHWInformation)));

                if (oIGCBarrageHWGatesInformation != null)
                {
                    foreach (object oGateInformation in oIGCBarrageHWGatesInformation)
                    {
                        if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 1)
                        {
                            hdnElectricalWorkingGateID.Value = Convert.ToString(oGateInformation.GetType().GetProperty("ID").GetValue(oGateInformation));
                            txtElectricalWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblElectricalTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            txtElectricalWorkingGate.Attributes.Add("max", lblElectricalTotalGate.Text);
                            lblElectricalNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            //Buisness Rule BR8b
                            if (lblElectricalTotalGate.Text == "0")
                            {
                                txtElectricalWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtElectricalWorkingGate.Enabled = true;
                            }
                        }
                        else if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 2)
                        {
                            hdnElectronicsWorkingGateID.Value = Convert.ToString(oGateInformation.GetType().GetProperty("ID").GetValue(oGateInformation));
                            txtElectronicsWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblElectronicsTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            lblElectronicsNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            txtElectronicsWorkingGate.Attributes.Add("max", lblElectronicsTotalGate.Text);
                            //Buisness Rule BR8b
                            if (lblElectronicsTotalGate.Text == "0")
                            {
                                txtElectronicsWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtElectronicsWorkingGate.Enabled = true;
                            }
                        }
                        else if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 3)
                        {
                            hdnManualWorkingGateID.Value = Convert.ToString(oGateInformation.GetType().GetProperty("ID").GetValue(oGateInformation));
                            txtManualWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblManualTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            lblManualNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            txtManualWorkingGate.Attributes.Add("max", lblManualTotalGate.Text);
                            //Buisness Rule BR8b
                            if (lblManualTotalGate.Text == "0")
                            {
                                txtManualWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtManualWorkingGate.Enabled = true;
                            }
                        }

                        //if (hdnInspectionStatus.Value == "2")
                        //{
                        txtManualWorkingGate.Enabled = false;
                        txtElectricalWorkingGate.Enabled = false;
                        //}

                        //   bool CanEdit = new FloodOperationsBLL().CanEditFloodInspections(2015, SessionManagerFacade.UserInformation.UA_Designations.ID,  Convert.ToInt16(hdnInspectionStatus.Value));
                    }
                }

                txtTotalCameras.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("TotalCameras").GetValue(oIGBarrageHWInformation));
                txtOperationalCameras.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("OperationalCameras").GetValue(oIGBarrageHWInformation));

                lblNotOperationalCameras.Text = "";

                if (txtTotalCameras.Text != "" && txtOperationalCameras.Text != "")
                {
                    lblNotOperationalCameras.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("NonOperationalCameras").GetValue(oIGBarrageHWInformation));
                }

                txtCCTVIncharge.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("CCTVIncharge").GetValue(oIGBarrageHWInformation));
                txtCCTVInchargePhone.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("CCTVInchargePhone").GetValue(oIGBarrageHWInformation));
                txtRemarks.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("Remarks").GetValue(oIGBarrageHWInformation));
                txtOperationalDeckElevated.Text = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("OperationalDeckElevated").GetValue(oIGBarrageHWInformation));

                string ddlPolice = Convert.ToString(Convert.ToString(Utility.GetDynamicPropertyValue(oIGBarrageHWInformation, "PoliceMonitory")) == true.ToString() ? "1" : Convert.ToString(Utility.GetDynamicPropertyValue(oIGBarrageHWInformation, "PoliceMonitory")) == false.ToString() ? "0" : null);

                //Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("PoliceMonitory").GetValue(oIGBarrageHWInformation));

                Dropdownlist.SetSelectedValue(ddlPoliceMonitoryCondition, ddlPolice);
                Dropdownlist.SetSelectedValue(ddlLightingCondition, Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("LightConditionID").GetValue(oIGBarrageHWInformation)));
                Dropdownlist.SetSelectedValue(ddlDataBoardCondition, Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("DataBoardConditionID").GetValue(oIGBarrageHWInformation)));
                Dropdownlist.SetSelectedValue(ddlTollHutCondition, Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("TollHutConditionID").GetValue(oIGBarrageHWInformation)));
                Dropdownlist.SetSelectedValue(ddlArmyCheckPostCondition, Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("TollHutConditionID").GetValue(oIGBarrageHWInformation)));

                hdnIGCBarrageHWID.Value = Convert.ToString(Convert.ToInt64(oIGBarrageHWInformation.GetType().GetProperty("IGCBarrageHWID").GetValue(oIGBarrageHWInformation)));
                hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
                hdnIGCBarrageHWCreatedBy.Value = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("CreatedBy").GetValue(oIGBarrageHWInformation));
                hdnIGCBarrageHWCreatedDate.Value = Convert.ToString(oIGBarrageHWInformation.GetType().GetProperty("CreatedDate").GetValue(oIGBarrageHWInformation));

                if (hdnInspectionStatus.Value == "2")
                {
                    txtTotalCameras.Enabled = false;
                    txtOperationalCameras.Enabled = false;
                    txtCCTVIncharge.Enabled = false;
                    txtCCTVInchargePhone.Enabled = false;

                    txtOperationalDeckElevated.Enabled = false;

                    ddlPoliceMonitoryCondition.Enabled = false;
                    ddlLightingCondition.Enabled = false;
                    ddlDataBoardCondition.Enabled = false;
                    ddlTollHutCondition.Enabled = false;
                    ddlArmyCheckPostCondition.Enabled = false;

                    txtManualWorkingGate.Enabled = false;
                    txtElectricalWorkingGate.Enabled = false;

                    btnSave.Enabled = false;
                    txtRemarks.Enabled = false;
                    txtElectronicsWorkingGate.Enabled = false;
                }

                if (_InspectionTypeID == 1)
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                    if (CanEdit)
                    {
                        txtTotalCameras.Enabled = CanEdit;
                        txtOperationalCameras.Enabled = CanEdit;
                        txtCCTVIncharge.Enabled = CanEdit;
                        txtCCTVInchargePhone.Enabled = CanEdit;

                        txtOperationalDeckElevated.Enabled = CanEdit;

                        ddlPoliceMonitoryCondition.Enabled = CanEdit;
                        ddlLightingCondition.Enabled = CanEdit;
                        ddlDataBoardCondition.Enabled = CanEdit;
                        ddlTollHutCondition.Enabled = CanEdit;
                        ddlArmyCheckPostCondition.Enabled = CanEdit;

                        txtManualWorkingGate.Enabled = CanEdit;
                        txtElectricalWorkingGate.Enabled = CanEdit;

                        btnSave.Enabled = CanEdit;
                        txtRemarks.Enabled = CanEdit;
                        txtElectronicsWorkingGate.Enabled = CanEdit;
                    }
                    else
                    {
                        txtTotalCameras.Enabled = false;
                        txtOperationalCameras.Enabled = false;
                        txtCCTVIncharge.Enabled = false;
                        txtCCTVInchargePhone.Enabled = false;

                        txtOperationalDeckElevated.Enabled = false;

                        ddlPoliceMonitoryCondition.Enabled = false;
                        ddlLightingCondition.Enabled = false;
                        ddlDataBoardCondition.Enabled = false;
                        ddlTollHutCondition.Enabled = false;
                        ddlArmyCheckPostCondition.Enabled = false;

                        txtManualWorkingGate.Enabled = false;
                        txtElectricalWorkingGate.Enabled = false;

                        btnSave.Enabled = false;
                        txtRemarks.Enabled = false;
                        txtElectronicsWorkingGate.Enabled = false;
                    }
                }
                else
                {
                    CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                    if (CanEdit)
                    {
                        txtTotalCameras.Enabled = CanEdit;
                        txtOperationalCameras.Enabled = CanEdit;
                        txtCCTVIncharge.Enabled = CanEdit;
                        txtCCTVInchargePhone.Enabled = CanEdit;

                        txtOperationalDeckElevated.Enabled = CanEdit;

                        ddlPoliceMonitoryCondition.Enabled = CanEdit;
                        ddlLightingCondition.Enabled = CanEdit;
                        ddlDataBoardCondition.Enabled = CanEdit;
                        ddlTollHutCondition.Enabled = CanEdit;
                        ddlArmyCheckPostCondition.Enabled = CanEdit;

                        txtManualWorkingGate.Enabled = CanEdit;
                        txtElectricalWorkingGate.Enabled = CanEdit;

                        btnSave.Enabled = CanEdit;
                        txtRemarks.Enabled = CanEdit;
                        txtElectronicsWorkingGate.Enabled = CanEdit;
                    }
                }
            }
            else
            {
                List<object> oBarrageGatesInformation = new FloodInspectionsBLL().GetBarrageGatesInformationByFloodInspectionID(_FloodInspectionID);
                if (oBarrageGatesInformation != null)
                {
                    foreach (object oGateInformation in oBarrageGatesInformation)
                    {
                        if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 1)
                        {
                            txtElectricalWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblElectricalTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            lblElectricalNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            txtElectricalWorkingGate.Attributes.Add("max", lblElectricalTotalGate.Text);
                            //Buisness Rule BR8b
                            if (lblElectricalTotalGate.Text == "0")
                            {
                                txtElectricalWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtElectricalWorkingGate.Enabled = true;
                            }
                        }
                        else if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 2)
                        {
                            txtElectronicsWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblElectronicsTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            lblElectronicsNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            txtElectronicsWorkingGate.Attributes.Add("max", lblElectronicsTotalGate.Text);
                            //Buisness Rule BR8b
                            if (lblElectronicsTotalGate.Text == "0")
                            {
                                txtElectronicsWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtElectronicsWorkingGate.Enabled = true;
                            }
                        }
                        else if (Convert.ToInt64(oGateInformation.GetType().GetProperty("GateTypeID").GetValue(oGateInformation)) == 3)
                        {
                            txtManualWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("WorkingGates").GetValue(oGateInformation));
                            lblManualTotalGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("TotalGates").GetValue(oGateInformation));
                            lblManualNotWorkingGate.Text = Convert.ToString(oGateInformation.GetType().GetProperty("NotWorkingGates").GetValue(oGateInformation));
                            txtManualWorkingGate.Attributes.Add("max", lblManualTotalGate.Text);
                            //Buisness Rule BR8b
                            if (lblManualTotalGate.Text == "0")
                            {
                                txtManualWorkingGate.Enabled = false;
                            }
                            else
                            {
                                txtManualWorkingGate.Enabled = true;
                            }
                        }

                        //CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(2015, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);

                        if (hdnInspectionStatus.Value == "2")
                        {
                            txtManualWorkingGate.Enabled = false;
                            txtElectricalWorkingGate.Enabled = false;
                        }

                        //if (CanEdit)
                        //{
                        //    txtManualWorkingGate.Enabled = CanEdit;
                        //    txtElectricalWorkingGate.Enabled = CanEdit;
                        //}

                        if (_InspectionTypeID == 1)
                        {
                            CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 1);
                            if (CanEdit)
                            {
                                txtManualWorkingGate.Enabled = CanEdit;
                                txtElectricalWorkingGate.Enabled = CanEdit;
                            }
                            else
                            {
                                txtManualWorkingGate.Enabled = false;
                                txtElectricalWorkingGate.Enabled = false;
                            }
                        }
                        else
                        {
                            CanEdit = new FloodOperationsBLL().CanAddEditFloodInspections(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, Convert.ToInt16(hdnInspectionStatus.Value), 2);
                            if (CanEdit)
                            {
                                txtManualWorkingGate.Enabled = CanEdit;
                                txtElectricalWorkingGate.Enabled = CanEdit;
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FO_IGCBarrageHW iFO_IGCBarrageHWEntity = PrepareIGCBarrageHWEntity();

                int total = 0, individual = 0;

                if (txtTotalCameras.Text != "" && txtOperationalCameras.Text != "")
                {
                    total = Convert.ToInt32(txtTotalCameras.Text);
                    individual = Convert.ToInt32(txtOperationalCameras.Text);

                    if (total < individual)
                    {
                        Master.ShowMessage(Message.FO_GCBarrageHW_Cameras.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                total = 0;
                individual = 0;

                if (lblManualTotalGate.Text != "" && txtManualWorkingGate.Text != "")
                {
                    total = Convert.ToInt32(lblManualTotalGate.Text);
                    individual = Convert.ToInt32(txtManualWorkingGate.Text);

                    if (total < individual)
                    {
                        Master.ShowMessage(Message.FO_GCBarrageHW_Manual.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                total = 0;
                individual = 0;

                if (lblElectricalTotalGate.Text != "" && txtElectricalWorkingGate.Text != "")
                {
                    total = Convert.ToInt32(lblElectricalTotalGate.Text);
                    individual = Convert.ToInt32(txtElectricalWorkingGate.Text);

                    if (total < individual)
                    {
                        Master.ShowMessage(Message.FO_GCBarrageHW_Electrical.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                total = 0;
                individual = 0;

                if (lblElectronicsTotalGate.Text != "" && txtElectronicsWorkingGate.Text != "")
                {
                    total = Convert.ToInt32(lblElectronicsTotalGate.Text);
                    individual = Convert.ToInt32(txtElectronicsWorkingGate.Text);

                    if (total < individual)
                    {
                        Master.ShowMessage(Message.FO_GCBarrageHW_Electronic.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                //if (new FloodInspectionBLL().IsDivisionSummaryAlreadyExists(iGCProtectionInfrastructureEntity))
                //{
                //  Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                //  return;
                //}

                //if(SessionManagerFacade.UserInformation.RoleID = Constants.d)
                //if (txtCCTVInchargePhone.Text.Length != 11)
                //{
                //    Master.ShowMessage("CCTV Incharge Phone Must 11 Digits", SiteMaster.MessageType.Error);
                //    return;
                //}

                iFO_IGCBarrageHWEntity.ID = new FloodInspectionsBLL().SaveIGCBarrageHWInformation(iFO_IGCBarrageHWEntity);
                if (iFO_IGCBarrageHWEntity != null && iFO_IGCBarrageHWEntity.ID > 0)
                {
                    List<FO_IGCBarrageHWGates> iFO_IGCBarrageHWGatesListEntity = PrepareIGCBarrageHWGatesEntity(iFO_IGCBarrageHWEntity.ID);

                    bool isSaved = new FloodInspectionsBLL().SaveIGCBarrageHWGatesInformation(iFO_IGCBarrageHWGatesListEntity);

                    if (isSaved == true)
                    {
                        SearchIndependent.IsSaved = true;
                        //Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        //HttpContext.Current.Session.Add("DivisionSummaryID", divisionSummaryEntity.ID);
                        Response.Redirect("SearchIndependent.aspx?FloodInspectionID=" + iFO_IGCBarrageHWEntity.FloodInspectionID, false);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                //lblMessage.Text = ex.Message;
            }
        }

        private FO_IGCBarrageHW PrepareIGCBarrageHWEntity()
        {
            FO_IGCBarrageHW iGCBarrageHW = new FO_IGCBarrageHW();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (hdnFloodInspectionsID.Value != null && hdnFloodInspectionsID.Value != "0")
            {
                iGCBarrageHW.FloodInspectionID = Convert.ToInt32(hdnFloodInspectionsID.Value);
            }

            if (hdnIGCBarrageHWID.Value != null && hdnIGCBarrageHWID.Value != "0")
            {
                iGCBarrageHW.ID = Convert.ToInt32(hdnIGCBarrageHWID.Value);
                iGCBarrageHW.CreatedBy = Convert.ToInt32(hdnIGCBarrageHWCreatedBy.Value);
                iGCBarrageHW.CreatedDate = Convert.ToDateTime(hdnIGCBarrageHWCreatedDate.Value);

                iGCBarrageHW.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHW.ModifiedDate = DateTime.Now;
            }
            else
            {
                iGCBarrageHW.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHW.CreatedDate = DateTime.Now;
            }

            if (txtTotalCameras.Text != "")
            {
                iGCBarrageHW.TotalCameras = Convert.ToInt16(txtTotalCameras.Text);
            }

            if (txtOperationalCameras.Text != "")
            {
                iGCBarrageHW.OperationalCameras = Convert.ToInt16(txtOperationalCameras.Text);
            }

            iGCBarrageHW.CCTVIncharge = txtCCTVIncharge.Text;

            iGCBarrageHW.CCTVInchargePhone = txtCCTVInchargePhone.Text;

            if (ddlPoliceMonitoryCondition.SelectedValue != "")
                iGCBarrageHW.PoliceMonitory = Convert.ToBoolean(ddlPoliceMonitoryCondition.SelectedValue == "1" ? "True" : "False");

            if (ddlLightingCondition.SelectedValue != "")
            {
                iGCBarrageHW.LightConditionID = Convert.ToInt16(ddlLightingCondition.SelectedValue);
            }

            if (ddlDataBoardCondition.SelectedValue != "")
            {
                iGCBarrageHW.DataBoardConditionID = Convert.ToInt16(ddlDataBoardCondition.SelectedValue);
            }

            if (ddlTollHutCondition.SelectedValue != "")
            {
                iGCBarrageHW.TollHutConditionID = Convert.ToInt16(ddlTollHutCondition.SelectedValue);
            }

            if (ddlArmyCheckPostCondition.SelectedValue != "")
            {
                iGCBarrageHW.ArmyCPConditionID = Convert.ToInt16(ddlArmyCheckPostCondition.SelectedValue);
            }

            if (txtOperationalDeckElevated.Text != "")
            {
                iGCBarrageHW.OperationalDeckElevated = Convert.ToDouble(txtOperationalDeckElevated.Text);
            }

            iGCBarrageHW.Remarks = txtRemarks.Text.Trim();

            return iGCBarrageHW;
        }

        private List<FO_IGCBarrageHWGates> PrepareIGCBarrageHWGatesEntity(long _IGCBarrageHWID)
        {
            List<FO_IGCBarrageHWGates> iGCBarrageHWGatesList = new List<FO_IGCBarrageHWGates>();

            FO_IGCBarrageHWGates iGCBarrageHWGates = new FO_IGCBarrageHWGates();

            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (hdnElectricalWorkingGateID.Value != "" && hdnElectricalWorkingGateID.Value != "0")
            {
                iGCBarrageHWGates.GateTypeID = 1;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;
                iGCBarrageHWGates.ID = Convert.ToInt64(hdnElectricalWorkingGateID.Value);

                if (txtElectricalWorkingGate.Text != "")
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtElectricalWorkingGate.Text);

                if (lblElectricalTotalGate.Text != "")
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(lblElectricalTotalGate.Text);

                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(hdnIGCBarrageHWCreatedBy.Value);
                iGCBarrageHWGates.CreatedDate = Convert.ToDateTime(hdnIGCBarrageHWCreatedDate.Value);

                iGCBarrageHWGates.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.ModifiedDate = DateTime.Now;

                iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
            }
            else
            {
                iGCBarrageHWGates.GateTypeID = 1;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;

                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.CreatedDate = DateTime.Now;

                if (txtElectricalWorkingGate.Text != "")
                {
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtElectricalWorkingGate.Text);
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(txtElectricalWorkingGate.Text);
                    iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
                }
            }

            iGCBarrageHWGates = new FO_IGCBarrageHWGates();

            if (hdnElectronicsWorkingGateID.Value != "" && hdnElectronicsWorkingGateID.Value != "0")
            {
                iGCBarrageHWGates.GateTypeID = 2;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;
                iGCBarrageHWGates.ID = Convert.ToInt64(hdnElectronicsWorkingGateID.Value);

                if (txtElectronicsWorkingGate.Text != "")
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtElectronicsWorkingGate.Text);

                if (lblElectronicsTotalGate.Text != "")
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(lblElectronicsTotalGate.Text);

                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(hdnIGCBarrageHWCreatedBy.Value);
                iGCBarrageHWGates.CreatedDate = Convert.ToDateTime(hdnIGCBarrageHWCreatedDate.Value);

                iGCBarrageHWGates.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.ModifiedDate = DateTime.Now;
                iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
            }
            else
            {
                iGCBarrageHWGates.GateTypeID = 2;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;
                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.CreatedDate = DateTime.Now;

                if (txtElectronicsWorkingGate.Text != "")
                {
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtElectronicsWorkingGate.Text);
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(txtElectronicsWorkingGate.Text);
                    iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
                }
            }

            iGCBarrageHWGates = new FO_IGCBarrageHWGates();

            if (hdnManualWorkingGateID.Value != "" && hdnManualWorkingGateID.Value != "0")
            {
                iGCBarrageHWGates.GateTypeID = 3;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;
                iGCBarrageHWGates.ID = Convert.ToInt64(hdnManualWorkingGateID.Value);

                if (txtManualWorkingGate.Text != "")
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtManualWorkingGate.Text);

                if (lblManualTotalGate.Text != "")
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(lblManualTotalGate.Text);

                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(hdnIGCBarrageHWCreatedBy.Value);
                iGCBarrageHWGates.CreatedDate = Convert.ToDateTime(hdnIGCBarrageHWCreatedDate.Value);

                iGCBarrageHWGates.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.ModifiedDate = DateTime.Now;

                iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
            }
            else
            {
                iGCBarrageHWGates.GateTypeID = 3;
                iGCBarrageHWGates.IGCBarrageHWID = _IGCBarrageHWID;

                iGCBarrageHWGates.CreatedBy = Convert.ToInt32(mdlUser.ID);
                iGCBarrageHWGates.CreatedDate = DateTime.Now;

                if (txtManualWorkingGate.Text != "")
                {
                    iGCBarrageHWGates.WorkingGates = Convert.ToInt16(txtManualWorkingGate.Text);
                    iGCBarrageHWGates.TotalGates = Convert.ToInt16(txtManualWorkingGate.Text);
                    iGCBarrageHWGatesList.Add(iGCBarrageHWGates);
                }
            }

            return iGCBarrageHWGatesList;
        }
    }
}