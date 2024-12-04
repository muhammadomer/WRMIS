using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common;

using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;

namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class TechnicalParametersSD : BasePage
    {
        #region Initialize
        int CreatedBy = 0;
        DateTime CreatedDate = System.DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _SmallDamID = Utility.GetNumericValueFromQueryString("SmallDamID", 0);

                    if (_SmallDamID > 0)
                    {
                        DamNameType._DAMID = _SmallDamID;
                        long _TechParaID = Utility.GetNumericValueFromQueryString("TechParaID", 0);

                        hdnSmallDamID.Value = Convert.ToString(_SmallDamID);
                        hdnTechParaID.Value = Convert.ToString(_TechParaID);
                        BindDropDown();
                        if (_TechParaID > 0)
                        {
                            BindFormDate(_SmallDamID, _TechParaID);
                        }

                        hlBack.NavigateUrl = string.Format("~/Modules/SmallDams/DamSearch/SearchSD.aspx?ShowHistroy=1");

                    }
                  
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Save
                SD_DamTechPara DamTechParaEntity = PrepareTechParaTypeEntity();
               
                bool isSaved = new SmallDamsBLL().SaveTechPara(DamTechParaEntity);
                if (isSaved)
                {
                    if (Convert.ToInt64(hdnTechParaID.Value) > 0)
                    {
                        Master.ShowMessage("Record update successfully.", SiteMaster.MessageType.Success);

                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    ReadOnlyMode();
                    Response.Redirect("SearchSD.aspx?ShowHistroy=1", false);
                }

                #endregion
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        void BindFormDate(Int64 _SmallDamID, Int64 _TechParaID)
        {
            SmallDamsBLL smallDamBll = new SmallDamsBLL();
            object TechPara = smallDamBll.GetTechParaByID(_SmallDamID, _TechParaID);

            if (TechPara != null)
            {
                //SmallDamID,

                Dropdownlist.DDLSpillwayType(ddlType);
                if (TechPara.GetType().GetProperty("SpillwayTypeID").GetValue(TechPara) != null)
                {
                    ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(TechPara.GetType().GetProperty("SpillwayTypeID").GetValue(TechPara).ToString()));
                }

                hdnTechParaID.Value =  Convert.ToString(TechPara.GetType().GetProperty("ID").GetValue(TechPara));

                txtMaximumHeight.Text = String.Format("{0:0.00}", TechPara.GetType().GetProperty("MaxHeight").GetValue(TechPara));
                txtDamLength.Text = Convert.ToString(TechPara.GetType().GetProperty("DamLength").GetValue(TechPara));
                txtTotalRLDam.Text = String.Format("{0:0.00}", TechPara.GetType().GetProperty("TopRL").GetValue(TechPara));
                txtCatchmentArea.Text = Convert.ToString(TechPara.GetType().GetProperty("CatchmentArea").GetValue(TechPara));
                txtGrossStorageCapacity.Text = Convert.ToString(TechPara.GetType().GetProperty("GSC").GetValue(TechPara));
                txtLiveStorageCapacity.Text = Convert.ToString(TechPara.GetType().GetProperty("LSC").GetValue(TechPara));
                txtNormalPondLevel.Text = String.Format("{0:0.00}", TechPara.GetType().GetProperty("PondLevel").GetValue(TechPara));
                txtHighFloodLevel.Text = String.Format("{0:0.00}", TechPara.GetType().GetProperty("HFL").GetValue(TechPara));
                txtDesignedCCA.Text = Convert.ToString(TechPara.GetType().GetProperty("DesignedCA").GetValue(TechPara));
                txtLength.Text = Convert.ToString(TechPara.GetType().GetProperty("SpillwayLength").GetValue(TechPara));
                txtDesignDischarge.Text = Convert.ToString(TechPara.GetType().GetProperty("SpillwayDesignDischarge").GetValue(TechPara));
                txtClearWaterWay.Text = Convert.ToString(TechPara.GetType().GetProperty("ClearWaterWay").GetValue(TechPara));
                txtWaterSupply.Text = Convert.ToString(TechPara.GetType().GetProperty("WaterSupply").GetValue(TechPara));

                //txtLandAcquirePond.Text = Convert.ToString(TechPara.GetType().GetProperty("LandAcquiredPond").GetValue(TechPara));
                string[] LandAcquirePond = new string[3];
                LandAcquirePond = Convert.ToString(TechPara.GetType().GetProperty("LandAcquiredPond").GetValue(TechPara)).Split('-');
                if (LandAcquirePond.Count() == 3)
                {
                    txtLandAcquirePondA.Text = LandAcquirePond[0];
                    txtLandAcquirePondK.Text = LandAcquirePond[1];
                    txtLandAcquirePondM.Text = LandAcquirePond[2];
                }
                //txtLandAcquireChannel.Text = Convert.ToString(TechPara.GetType().GetProperty("LandAcquiredChannel").GetValue(TechPara));
                string[] LandAcquireChannel = new string[3];
                LandAcquireChannel = Convert.ToString(TechPara.GetType().GetProperty("LandAcquiredChannel").GetValue(TechPara)).Split('-');
                if (LandAcquireChannel.Count() == 3)
                {
                    txtLandAcquireChannelA.Text = LandAcquireChannel[0];
                    txtLandAcquireChannelK.Text = LandAcquireChannel[1];
                    txtLandAcquireChannelM.Text = LandAcquireChannel[2];
                }
                    CreatedBy = Convert.ToInt16(TechPara.GetType().GetProperty("CreatedBy").GetValue(TechPara));
                CreatedDate = Convert.ToDateTime(TechPara.GetType().GetProperty("CreatedDate").GetValue(TechPara));

            }

        }

        SD_DamTechPara PrepareTechParaTypeEntity()
        {
            SD_DamTechPara techPara = new SD_DamTechPara();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (Convert.ToInt64(hdnTechParaID.Value) == 0)
            {
                techPara.ID = 0;

                techPara.CreatedBy = Convert.ToInt16(mdlUser.ID);
                techPara.CreatedDate = System.DateTime.Now;
                //techPara.ModifiedBy = null;
                //techPara.ModifiedDate = null;
            }
            else
            {
                techPara.ID = Convert.ToInt16(hdnTechParaID.Value);
                techPara.CreatedBy = CreatedBy;
                techPara.CreatedDate = CreatedDate;
                techPara.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                techPara.ModifiedDate = System.DateTime.Now;
            }

            techPara.SmallDamID = Convert.ToInt64(hdnSmallDamID.Value);
            if (ddlType.SelectedIndex != 0)
            {
                techPara.SpillwayTypeID = Convert.ToInt16(ddlType.SelectedItem.Value);
            }

            if (txtMaximumHeight.Text != "")
                techPara.MaxHeight = Convert.ToDouble(txtMaximumHeight.Text);

            if (txtDamLength.Text != "")
                techPara.DamLength = Convert.ToDouble(txtDamLength.Text);
            
            if (txtTotalRLDam.Text != "")
                techPara.TopRL = Convert.ToDouble(txtTotalRLDam.Text);
            
            if (txtCatchmentArea.Text != "")
                techPara.CatchmentArea = Convert.ToDouble(txtCatchmentArea.Text);
            
            if (txtGrossStorageCapacity.Text != "")
                techPara.GSC = Convert.ToDouble(txtGrossStorageCapacity.Text);
            
            if (txtLiveStorageCapacity.Text != "")
                techPara.LSC = Convert.ToDouble(txtLiveStorageCapacity.Text);
            
            if (txtNormalPondLevel.Text != "")
                techPara.PondLevel = Convert.ToDouble(txtNormalPondLevel.Text);
            
            if (txtHighFloodLevel.Text != "")
                techPara.HFL = Convert.ToDouble(txtHighFloodLevel.Text);

            if (txtDesignedCCA.Text != "")
                techPara.DesignedCA = Convert.ToDouble(txtDesignedCCA.Text);
            
            if (txtLength.Text != "")
                techPara.SpillwayLength = Convert.ToDouble(txtLength.Text);
            
            if (txtDesignDischarge.Text != "")
                techPara.SpillwayDesignDischarge = Convert.ToDouble(txtDesignDischarge.Text);
            
            if (txtClearWaterWay.Text != "")
                techPara.ClearWaterWay = Convert.ToDouble(txtClearWaterWay.Text);
            
            if (txtWaterSupply.Text != "")
                techPara.WaterSupply = txtWaterSupply.Text;
            
            if (txtLandAcquirePondA.Text != "" && txtLandAcquirePondK.Text != "" && txtLandAcquirePondM.Text != "")
                techPara.LandAcquiredPond = txtLandAcquirePondA.Text + "-" + txtLandAcquirePondK.Text + "-" + txtLandAcquirePondM.Text;
            
            if (txtLandAcquireChannelA.Text != "" && txtLandAcquireChannelK.Text != "" && txtLandAcquireChannelM.Text != "")
                techPara.LandAcquiredChannel = txtLandAcquireChannelA.Text + "-" + txtLandAcquireChannelK.Text + "-" + txtLandAcquireChannelM.Text;


            return techPara;
        }


        void ReadOnlyMode()
        {
            ddlType.Enabled = false;

            txtMaximumHeight.Enabled = false;
            txtDamLength.Enabled = false;
            txtTotalRLDam.Enabled = false;
            txtCatchmentArea.Enabled = false;
            txtGrossStorageCapacity.Enabled = false;
            txtLiveStorageCapacity.Enabled = false;
            txtNormalPondLevel.Enabled = false;
            txtHighFloodLevel.Enabled = false;
            txtDesignedCCA.Enabled = false;
            txtLength.Enabled = false;
            txtDesignDischarge.Enabled = false;
            txtClearWaterWay.Enabled = false;
            txtWaterSupply.Enabled = false;
            txtLandAcquirePondA.Enabled = false;
            txtLandAcquirePondK.Enabled = false;
            txtLandAcquirePondM.Enabled = false;
            txtLandAcquireChannelA.Enabled = false;
            txtLandAcquireChannelK.Enabled = false;
            txtLandAcquireChannelM.Enabled = false;


        }

        private void BindDropDown()
        {
            //Dropdownlist.DDLGrossStorageCapacity(ddlGrossStorageCapacity);
            Dropdownlist.DDLSpillwayType(ddlType);
        }
        #endregion



    }
}