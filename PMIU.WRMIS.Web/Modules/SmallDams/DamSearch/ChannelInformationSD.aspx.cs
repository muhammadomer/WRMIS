using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;
using PMIU.WRMIS.AppBlocks;


namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class ChannelInformationSD : BasePage
    {
        #region Initialize
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
                        long _ChannelID = Utility.GetNumericValueFromQueryString("ChannelID", 0);
                        hdnSmallDamID.Value = Convert.ToString(_SmallDamID);
                        hdnChannelID.Value = Convert.ToString(_ChannelID);
                       
                        BindDropDown();
                        hlBack.NavigateUrl = string.Format("~/Modules/SmallDams/DamSearch/ChannelsSD.aspx?SmallDamID={0}&ChannelID=0",_SmallDamID);
                        if (_ChannelID != 0)
                        {
                            BindFormData(_SmallDamID, _ChannelID);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Events
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                #region Save
                SD_SmallChannel ChannelsEntity = PrepareChannelEntity();

                if( new SmallDamsBLL().IsChannelsUnique(ChannelsEntity))
                {
                     Master.ShowMessage("Unique channel code is required.", SiteMaster.MessageType.Error);
                     return;
                }
                
                bool isSaved = new SmallDamsBLL().SaveChannels(ChannelsEntity);
                
                if (isSaved)
                {
                    if (Convert.ToInt64(hdnChannelID.Value) > 0)
                    {
                        Master.ShowMessage("Record update successfully.", SiteMaster.MessageType.Success);

                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    ReadOnlyMode();
                    //HttpContext.Current.Session.Add("FloodInspectionID", DamTypeEntity.ID);
                    Response.Redirect("ChannelsSD.aspx?SmallDamID=" + hdnSmallDamID.Value + "&ChannelID=0", false);
                }
                #endregion
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlParentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParentType.SelectedIndex != 0)
            {
                if (ddlParentType.SelectedIndex == 1)
                {
                    Dropdownlist.DDLDamNameByID(ddlParent, false, Convert.ToInt64(hdnSmallDamID.Value));
                    lblParent.Text = "Dam";
                }
                if (ddlParentType.SelectedIndex == 2)
                {
                    Dropdownlist.DDLChannelsByDamID(ddlParent, false, Convert.ToInt64(hdnSmallDamID.Value));
                    lblParent.Text = "Channel";
                }
            }
            if (ddlParentType.SelectedIndex == 0)
            {
                Dropdownlist.DDLDamNameByID(ddlParent, true, -1, (int)Constants.DropDownFirstOption.Select);
                lblParent.Text = "Dam/Channel";
            }
        }
        #endregion
        #region Function
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        SD_SmallChannel PrepareChannelEntity()
        {
            SD_SmallChannel smallChannel = new SD_SmallChannel();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (Convert.ToInt64(hdnChannelID.Value) == 0)
            {
                smallChannel.ID = 0;
                smallChannel.CreatedBy = Convert.ToInt32(mdlUser.ID);
                smallChannel.CreatedDate = DateTime.Now;
                smallChannel.ModifiedBy = null;
                smallChannel.ModifiedDate = null;
            }
            else
            {
                smallChannel.ID = Convert.ToInt64(hdnChannelID.Value);
                smallChannel.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                smallChannel.ModifiedDate = DateTime.Now;
                smallChannel.CreatedDate = Convert.ToDateTime(hdnCreatedDate.Value);
            }

            smallChannel.SmallDamID = Convert.ToInt64(hdnSmallDamID.Value);
            smallChannel.Name = txtChannelName.Text;
            smallChannel.ChannelCode = txtChannelCode.Text;
            if (txtChannelCapacity.Text != "")
            smallChannel.Capacity = Convert.ToDouble(txtChannelCapacity.Text);
            if (txtDesignedCCA.Text != "")
            smallChannel.DesignedCCA = Convert.ToDouble(txtDesignedCCA.Text);

            if (ddlParentType.SelectedIndex != 0)
                smallChannel.ParentType = ddlParentType.SelectedItem.Text;    
            
            if(ddlParent.SelectedIndex != 0)
            smallChannel.ParentID = Convert.ToInt64(ddlParent.SelectedItem.Value);

            smallChannel.ParentRD = Calculations.CalculateTotalRDs(txtOffTakingRDLeft.Text, txtOffTakingRDRight.Text);


            if (ddlOffTakingSide.SelectedIndex != 0)
            smallChannel.ParentSide = ddlOffTakingSide.SelectedItem.Text;
            
            smallChannel.TailRD = Calculations.CalculateTotalRDs(txtTailRDLeft.Text, txtTailRDRight.Text);

            if (txtAuthorizedGauge.Text != "")
            smallChannel.AuthorizedGauge = Convert.ToDouble(txtAuthorizedGauge.Text);

            if (txtMaxGaugeValue.Text != "")
            smallChannel.MaxGauge = Convert.ToDouble(txtMaxGaugeValue.Text);

            if (txtMaxDischargeValue.Text != "")
                smallChannel.MaxDischarge = Convert.ToDouble(txtMaxDischargeValue.Text);
            if (rdolStatus.SelectedValue == "1")
            {
                smallChannel.IsActive = true;
            }
            else
            {
                smallChannel.IsActive = false;
            }

            return smallChannel;
        }

        void ReadOnlyMode()
        {
            txtChannelName.Enabled = false;
            txtChannelCode.Enabled = false;
            txtChannelCapacity.Enabled = false;
            txtDesignedCCA.Enabled = false;
            ddlParentType.Enabled = false;
            ddlParent.Enabled = false;
            txtOffTakingRDLeft.Enabled = false;
            txtOffTakingRDRight.Enabled = false;
            ddlOffTakingSide.Enabled = false;
            txtTailRDLeft.Enabled = false;
            txtTailRDRight.Enabled = false;
            txtAuthorizedGauge.Enabled = false;
            txtMaxGaugeValue.Enabled = false;
            txtMaxDischargeValue.Enabled = false;
            rdolStatus.Enabled = false;

        }
        void BindDropDown()
        {
            Dropdownlist.DDLPrentTypeSD(ddlParentType);
            Dropdownlist.DDLOffTakingSidesSD(ddlOffTakingSide);
            Dropdownlist.DDLDamNameByID(ddlParent, true, -1, (int)Constants.DropDownFirstOption.Select);
        }
        void BindFormData(Int64 _SmallDamID, Int64 _ChannelID)
        {

            object smallChannel = new SmallDamsBLL().GetChannelsByID(_SmallDamID, _ChannelID);
            if (smallChannel != null)
            {
                txtChannelName.Text = Convert.ToString(smallChannel.GetType().GetProperty("Name").GetValue(smallChannel));
                txtChannelCode.Text = Convert.ToString(smallChannel.GetType().GetProperty("ChannelCode").GetValue(smallChannel));
                txtChannelCapacity.Text = Convert.ToString(smallChannel.GetType().GetProperty("Capacity").GetValue(smallChannel));
                txtDesignedCCA.Text = Convert.ToString(smallChannel.GetType().GetProperty("DesignedCCA").GetValue(smallChannel));

                Dropdownlist.DDLPrentTypeSD(ddlParentType);
                ddlParentType.SelectedIndex = ddlParentType.Items.IndexOf(ddlParentType.Items.FindByText(smallChannel.GetType().GetProperty("ParentType").GetValue(smallChannel).ToString()));


                if (ddlParentType.SelectedIndex == 1)
                {
                    Dropdownlist.DDLDamNameByID(ddlParent, false, Convert.ToInt64(hdnSmallDamID.Value));
                }
                else if (ddlParentType.SelectedIndex == 2)
                {
                    Dropdownlist.DDLChannelsByDamID(ddlParent, false, Convert.ToInt64(hdnSmallDamID.Value));  
                }
                ddlParent.SelectedIndex = ddlParent.Items.IndexOf(ddlParent.Items.FindByValue(smallChannel.GetType().GetProperty("ParentID").GetValue(smallChannel).ToString()));

                Tuple<string, string> OffTakingRD = Calculations.GetRDValues(Convert.ToInt64(smallChannel.GetType().GetProperty("ParentRD").GetValue(smallChannel)));
                txtOffTakingRDLeft.Text = OffTakingRD.Item1;
                txtOffTakingRDRight.Text = OffTakingRD.Item2;

                Dropdownlist.DDLOffTakingSidesSD(ddlOffTakingSide);
              
                
                ddlOffTakingSide.SelectedIndex = ddlOffTakingSide.Items.IndexOf(ddlOffTakingSide.Items.FindByText(smallChannel.GetType().GetProperty("ParentSide").GetValue(smallChannel).ToString()));

                Tuple<string, string> TailRD = Calculations.GetRDValues(Convert.ToInt64(smallChannel.GetType().GetProperty("TailRD").GetValue(smallChannel)));
                txtTailRDLeft.Text = TailRD.Item1;
                txtTailRDRight.Text = TailRD.Item2;

                txtAuthorizedGauge.Text = Convert.ToString(smallChannel.GetType().GetProperty("AuthorizedGauge").GetValue(smallChannel));
                txtMaxGaugeValue.Text = String.Format("{0:0.00}", smallChannel.GetType().GetProperty("MaxGauge").GetValue(smallChannel));
                txtMaxDischargeValue.Text = String.Format("{0:0.00}", smallChannel.GetType().GetProperty("MaxDischarge").GetValue(smallChannel));

                if (Convert.ToBoolean(smallChannel.GetType().GetProperty("IsActive").GetValue(smallChannel)))
                {
                    rdolStatus.SelectedIndex = 0;
                }
                else
                {
                    rdolStatus.SelectedIndex = 1;
                }
                hdnCreatedDate.Value = Convert.ToString(smallChannel.GetType().GetProperty("CreatedDate").GetValue(smallChannel));
                MainHeading.InnerHtml = "Edit Channel Information";
            }
        }
        #endregion






    }
}