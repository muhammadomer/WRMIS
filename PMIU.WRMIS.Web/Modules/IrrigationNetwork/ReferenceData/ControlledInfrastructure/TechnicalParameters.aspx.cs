using System.Linq;
using System.Web;
using System;
using System.Web.UI;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure
{
    public partial class TechnicalParameters : BasePage
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            long ControlinfrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGateType();
                    ControlinfrastructureID = Utility.GetNumericValueFromQueryString("ControlInfrastructureID", 0);
                    hdnControlInfrastructureID.Value = Convert.ToString(ControlinfrastructureID);
                    ControlledInfrastructureDetails.ID = Convert.ToInt64(ControlinfrastructureID);
                    bool IsRecordExists = new ControlledInfrastructureBLL().IsRecordExists(ControlinfrastructureID);
                    if (IsRecordExists)
                    {
                        LoadStructureTechParaDetail(ControlinfrastructureID);
                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Search.aspx?ControlInfrastructureID={0}", ControlinfrastructureID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Page Load

        #region Set PageTitle
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ControlInfrastructureGauges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set PageTitle

        #region Button Event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CO_StructureTechPara StructureTechParaEntity = PrepareStructureTechParaEntity();
            bool isSaved = new ControlledInfrastructureBLL().SaveStructureTechPara(StructureTechParaEntity);
            if (isSaved)
            {
                Search.IsSaved = true;
                Response.Redirect("~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Search.aspx?ControlInfrastructureID=" + StructureTechParaEntity.ID, false);
            }
        }

        #endregion Button Event

        #region Bind DropDown
        private void BindGateType()
        {
            Dropdownlist.DDLGateType(ddlType);
        }
        #endregion Bind DropDown

        #region LoadStructureTechParaDetail
        private void LoadStructureTechParaDetail(long _StructureTechParaID)
        {
            try
            {
                CO_StructureTechPara bllIControlnfrastructure = new ControlledInfrastructureBLL().GetStructureTechParaeByID(_StructureTechParaID);
                hdnStructureTechParaID.Value = Convert.ToString(bllIControlnfrastructure.ID);
                if (bllIControlnfrastructure != null)
                {
                    #region Design Parameters
                    hdnControlInfrastructureID.Value = Convert.ToString(bllIControlnfrastructure.StationID);
                    txtDesignedDischarge.Text = Convert.ToString(bllIControlnfrastructure.DesignDischarge);
                    txtNormalPondLevel.Text = Convert.ToString(bllIControlnfrastructure.NormalPondLevel);

                    txtUpstreamFloorLevel.Text = Convert.ToString(bllIControlnfrastructure.UpstreamFloorLevel);
                    txtDownstreamFloorLevel.Text = Convert.ToString(bllIControlnfrastructure.DownstreamFloorLevel);
                    txtNoOfDivideWalls.Text = Convert.ToString(bllIControlnfrastructure.DivideWallsNo);
                    txtNoOfTurbines.Text = Convert.ToString(bllIControlnfrastructure.TurbinesNo);
                    txtWidthbetweenAbutments.Text = Convert.ToString(bllIControlnfrastructure.WidthBtwAbuments);
                    #endregion Design Parameters

                    #region Gates Details
                    Dropdownlist.SetSelectedText(ddlType, bllIControlnfrastructure.GateType == "R" ? "Radial" : "Vertical Lift");
                    txtNoOfManualGates.Text = Convert.ToString(bllIControlnfrastructure.ManualGatesNo);
                    txtNOofElectronicGates.Text = Convert.ToString(bllIControlnfrastructure.ElectronicGatesNo);
                    txtNoOfElectricalGates.Text = Convert.ToString(bllIControlnfrastructure.ElectricalGatesNo);
                    txtNoOfMainWeirGates.Text = Convert.ToString(bllIControlnfrastructure.MainWeirGateNo);
                    txtMainWeirCrestLevel.Text = Convert.ToString(bllIControlnfrastructure.MainWeirCrestLevel);
                    txtHeightOfEachWeirGate.Text = Convert.ToString(bllIControlnfrastructure.MainWeirGateHeight);
                    txtWidthOfEachWeirGate.Text = Convert.ToString(bllIControlnfrastructure.MainWeirGateWidth);
                    txtNoOfUndersluiceGates.Text = Convert.ToString(bllIControlnfrastructure.UndersluiceGateNo);
                    txtUndersluiceCrestLevel.Text = Convert.ToString(bllIControlnfrastructure.UndersluiceCrestLevel);
                    txtNoOfLeftUndersluiceGates.Text = Convert.ToString(bllIControlnfrastructure.LeftUndersluiceGateNo);
                    txtNoOfRightUndersluiceGates.Text = Convert.ToString(bllIControlnfrastructure.RightUndersluiceGateNo);
                    txtWidthOfEachUndersluiceGate.Text = Convert.ToString(bllIControlnfrastructure.UndersluiceGateWidth);
                    txtHeightOfEachUndersluiceGate.Text = Convert.ToString(bllIControlnfrastructure.UndersluiceGateHeight);
                    #endregion Gates Details

                    #region Fish Ladder Details
                    txtNoOfFishLadders.Text = Convert.ToString(bllIControlnfrastructure.FishLaddersNo);
                    txtWidthofeachFishLadder.Text = Convert.ToString(bllIControlnfrastructure.FishLadderWidth);
                    #endregion Fish Ladder Details

                    #region Piers Details
                    txtNoOfPiers.Text = Convert.ToString(bllIControlnfrastructure.PiersNo);
                    txtWidthOfEachPier.Text = Convert.ToString(bllIControlnfrastructure.PiersWidth);
                    #endregion Piers Details
                    hdnCreatedDate.Value = Convert.ToString(bllIControlnfrastructure.CreatedDate);




                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private CO_StructureTechPara PrepareStructureTechParaEntity()
        {
            CO_StructureTechPara ObjeStructureTechPara = new CO_StructureTechPara();

            #region Design Parameters
            ObjeStructureTechPara.ID = Convert.ToInt32(hdnStructureTechParaID.Value);
            if (!string.IsNullOrEmpty(hdnControlInfrastructureID.Value))
                ObjeStructureTechPara.StationID = Convert.ToInt64(hdnControlInfrastructureID.Value);
            if (!string.IsNullOrEmpty(txtDesignedDischarge.Text))
                ObjeStructureTechPara.DesignDischarge = Convert.ToDouble(txtDesignedDischarge.Text);
            if (!string.IsNullOrEmpty(txtNormalPondLevel.Text))
                ObjeStructureTechPara.NormalPondLevel = Convert.ToDouble(txtNormalPondLevel.Text);
            if (!string.IsNullOrEmpty(txtUpstreamFloorLevel.Text))
                ObjeStructureTechPara.UpstreamFloorLevel = Convert.ToDouble(txtUpstreamFloorLevel.Text);
            if (!string.IsNullOrEmpty(txtDownstreamFloorLevel.Text))
                ObjeStructureTechPara.DownstreamFloorLevel = Convert.ToDouble(txtDownstreamFloorLevel.Text);
            if (!string.IsNullOrEmpty(txtNoOfDivideWalls.Text))
                ObjeStructureTechPara.DivideWallsNo = Convert.ToInt16(txtNoOfDivideWalls.Text);
            if (!string.IsNullOrEmpty(txtNoOfTurbines.Text))
                ObjeStructureTechPara.TurbinesNo = Convert.ToInt16(txtNoOfTurbines.Text);
            if (!string.IsNullOrEmpty(txtWidthbetweenAbutments.Text))
                ObjeStructureTechPara.WidthBtwAbuments = Convert.ToDouble(txtWidthbetweenAbutments.Text);
            #endregion Design Parameters

            #region Gates Details
            ObjeStructureTechPara.GateType = ddlType.SelectedItem.Text == "Radial" ? "R" : "VL";
            if (!string.IsNullOrEmpty(txtNoOfManualGates.Text))
                ObjeStructureTechPara.ManualGatesNo = Convert.ToInt16(txtNoOfManualGates.Text);
            if (!string.IsNullOrEmpty(txtNOofElectronicGates.Text))
                ObjeStructureTechPara.ElectronicGatesNo = Convert.ToInt16(txtNOofElectronicGates.Text);
            if (!string.IsNullOrEmpty(txtNoOfElectricalGates.Text))
                ObjeStructureTechPara.ElectricalGatesNo = Convert.ToInt16(txtNoOfElectricalGates.Text);
            if (!string.IsNullOrEmpty(txtNoOfMainWeirGates.Text))
                ObjeStructureTechPara.MainWeirGateNo = Convert.ToInt16(txtNoOfMainWeirGates.Text);
            if (!string.IsNullOrEmpty(txtMainWeirCrestLevel.Text))
                ObjeStructureTechPara.MainWeirCrestLevel = Convert.ToDouble(txtMainWeirCrestLevel.Text);
            if (!string.IsNullOrEmpty(txtHeightOfEachWeirGate.Text))
                ObjeStructureTechPara.MainWeirGateHeight = Convert.ToDouble(txtHeightOfEachWeirGate.Text);
            if (!string.IsNullOrEmpty(txtWidthOfEachWeirGate.Text))
                ObjeStructureTechPara.MainWeirGateWidth = Convert.ToDouble(txtWidthOfEachWeirGate.Text);
            if (!string.IsNullOrEmpty(txtNoOfUndersluiceGates.Text))
                ObjeStructureTechPara.UndersluiceGateNo = Convert.ToInt16(txtNoOfUndersluiceGates.Text);
            if (!string.IsNullOrEmpty(txtUndersluiceCrestLevel.Text))
                ObjeStructureTechPara.UndersluiceCrestLevel = Convert.ToDouble(txtUndersluiceCrestLevel.Text);
            if (!string.IsNullOrEmpty(txtNoOfLeftUndersluiceGates.Text))
                ObjeStructureTechPara.LeftUndersluiceGateNo = Convert.ToInt16(txtNoOfLeftUndersluiceGates.Text);
            if (!string.IsNullOrEmpty(txtNoOfRightUndersluiceGates.Text))
                ObjeStructureTechPara.RightUndersluiceGateNo = Convert.ToInt16(txtNoOfRightUndersluiceGates.Text);
            if (!string.IsNullOrEmpty(txtWidthOfEachUndersluiceGate.Text))
                ObjeStructureTechPara.UndersluiceGateWidth = Convert.ToDouble(txtWidthOfEachUndersluiceGate.Text);
            if (!string.IsNullOrEmpty(txtHeightOfEachUndersluiceGate.Text))
                ObjeStructureTechPara.UndersluiceGateHeight = Convert.ToDouble(txtHeightOfEachUndersluiceGate.Text);

            #endregion Gates Details

            #region Fish Ladder Details
            if (!string.IsNullOrEmpty(txtNoOfFishLadders.Text))
                ObjeStructureTechPara.FishLaddersNo = Convert.ToInt16(txtNoOfFishLadders.Text);
            if (!string.IsNullOrEmpty(txtWidthofeachFishLadder.Text))
                ObjeStructureTechPara.FishLadderWidth = Convert.ToDouble(txtWidthofeachFishLadder.Text);

            #endregion Fish Ladder Details

            #region Piers Details

            if (!string.IsNullOrEmpty(txtNoOfPiers.Text))
                ObjeStructureTechPara.PiersNo = Convert.ToInt16(txtNoOfPiers.Text);
            if (!string.IsNullOrEmpty(txtWidthOfEachPier.Text))
                ObjeStructureTechPara.PiersWidth = Convert.ToDouble(txtWidthOfEachPier.Text);

            #endregion Piers Details


            if (ObjeStructureTechPara.ID == 0)
            {
                ObjeStructureTechPara.CreatedDate = DateTime.Now;
                ObjeStructureTechPara.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);

            }
            else
            {
                ObjeStructureTechPara.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
                ObjeStructureTechPara.ModifiedDate = DateTime.Now;
                ObjeStructureTechPara.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjeStructureTechPara.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
            }
            return ObjeStructureTechPara;

        }
        #endregion LoadStructureTechParaDetail
    }
}