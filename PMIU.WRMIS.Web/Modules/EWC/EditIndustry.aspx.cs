using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
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

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class EditIndustry : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        EC_Industry mdlIndustry = new EC_Industry(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                if (!IsPostBack)
                { 
                    SetPageTitle();
                    LoadIndustryTypeDDL();
                    
                    long industryID = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["IndustryID"]))
                        industryID = Convert.ToInt64 (Request.QueryString["IndustryID"]);

                    EC_Industry mdl = bllEWC.GetIndustry(industryID, string.Empty);
                    if (mdl != null)
                        LoadIndustryDetails(mdl); 
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadIndustryDetails(EC_Industry _Industry)
        { 
            txtNo.Text = _Industry.ID.ToString();
            ddlType.Items.FindByValue(_Industry.IndustryTypeID.ToString()).Selected = true;
            txtName.Text = _Industry.IndustryName;
            ddlStatus.Items.FindByText(_Industry.IndustryStatus).Selected = true;
            if (_Industry.NTNNo != null) 
            {
                txtNTNL.Text = _Industry.NTNNo.Substring(0, _Industry.NTNNo.IndexOf('-'));
                txtNTNR.Text = _Industry.NTNNo.Substring(_Industry.NTNNo.IndexOf('-') + 1);
            }
            txtAdrs.Text = _Industry.Address;
            txtPhnNo.Text = _Industry.PhoneNo;
            txtFax.Text = _Industry.Fax;
            txtEmail.Text = _Industry.Email;
            txtX.Text = _Industry.GisX.ToString();
            txtY.Text = _Industry.GisY.ToString();
            if(_Industry.WTPlantExist != null)
            {
                ddlPlantExists.SelectedItem.Selected = false;
                if(_Industry.WTPlantExist == true)
                { 
                    ddlPlantExists.Items.FindByValue("1").Selected = true;
                    ddlPlantCondition.Items.FindByText(_Industry.WTPlantCondition).Selected = true;
                }
                else
                    ddlPlantExists.Items.FindByValue("2").Selected = true;
            }           
            txtCnctName.Text = _Industry.ContactPersonName;
            txtCnctCellNo.Text = _Industry.CPCellNo;
            txtCnctCNIC.Text = _Industry.CPCNIC;
            txtCnctEmail.Text = _Industry.CPEmail;

            if ((bool)_Industry.IsEffluentWater)
            {
                cbSrvcEffluent.Checked = true;
                if (bllEWC.IndustryBillGenerated(_Industry.ID, Constants.ECWServiceType.EFFLUENT.ToString()))
                    cbSrvcEffluent.Enabled = false; 
            } 

            if ((bool)_Industry.IsCanalWater)
            {
                cbSrvcCanal.Checked = true;
                if (bllEWC.IndustryBillGenerated(_Industry.ID, Constants.ECWServiceType.CANAL.ToString()))
                    cbSrvcEffluent.Enabled = false; 
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }  
        private void LoadIndustryTypeDDL()
        {
            List<object> lstIndustryType = bllEWC.IndustryType_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.Select, lstIndustryType);
        }  
        #region  Save Industry Logic
        private bool isNullOREmpty(string _Value)
        {
            if (string.IsNullOrEmpty(_Value))
                return true;

            return false;
        }
        private void GetIndustryData()
        {
            mdlIndustry = new EC_Industry();

            //Industry Info
            mdlIndustry.ID = Convert.ToInt64(txtNo.Text);
            mdlIndustry.IndustryName = txtName.Text.Trim();
            mdlIndustry.IndustryStatus = ddlStatus.SelectedItem.Text.Trim();
            mdlIndustry.IndustryTypeID = Convert.ToInt64(ddlType.SelectedItem.Value);

            //if (bllEWC.DoesIndustryExists(mdlIndustry.ID, mdlIndustry.IndustryName, mdlIndustry.IndustryTypeID))
            //{
            //    Master.ShowMessage(Message.IndustryDuplication.Description, SiteMaster.MessageType.Error);
            //    return;
            //}

            if (!isNullOREmpty(txtNTNR.Text) && isNullOREmpty(txtNTNL.Text))
            {
                Master.ShowMessage(Message.IncompleteNTN.Description, SiteMaster.MessageType.Error);
                return;
            }

            if (!isNullOREmpty(txtNTNR.Text))
                mdlIndustry.NTNNo = txtNTNL.Text + "-" + txtNTNR.Text;
            mdlIndustry.ServiceType = "this field is not used.";
            mdlIndustry.Address = txtAdrs.Text.Trim();
            mdlIndustry.PhoneNo = txtPhnNo.Text.Trim();
            mdlIndustry.Fax = txtFax.Text.Trim();
            mdlIndustry.Email = txtEmail.Text.Trim();

            if (!isNullOREmpty(txtX.Text))
                mdlIndustry.GisX = Convert.ToDouble(txtX.Text.Trim());
            if (!isNullOREmpty(txtY.Text))
                mdlIndustry.GisY = Convert.ToDouble(txtY.Text.Trim());

            //Plant Condition
            if (!isNullOREmpty(ddlPlantExists.SelectedItem.Value))
            {
                int val_DDL = Convert.ToInt32(ddlPlantExists.SelectedItem.Value);
                mdlIndustry.WTPlantExist = val_DDL == 1 ? true : false;

                if (!isNullOREmpty(ddlPlantCondition.SelectedItem.Value))
                    mdlIndustry.WTPlantCondition = ddlPlantCondition.SelectedItem.Text;
            }
            //Contact Person Details
            mdlIndustry.ContactPersonName = txtCnctName.Text.Trim();
            mdlIndustry.CPCellNo = txtCnctCellNo.Text.Trim();
            mdlIndustry.CPCNIC = txtCnctCNIC.Text.Trim();
            mdlIndustry.CPEmail = txtCnctEmail.Text.Trim();
            mdlIndustry.ModifiedBy = (int) SessionManagerFacade.UserInformation.ID;
            mdlIndustry.ModifiedDate = DateTime.Now;
        } 
        private void SaveIndustry(bool _EditService)
        {
            try
            {
                GetIndustryData();
                if (!cbSrvcEffluent.Checked && !cbSrvcCanal.Checked)
                {
                    Master.ShowMessage(Message.SelectAService.Description, SiteMaster.MessageType.Error);
                    return;
                }
                mdlIndustry.IsEffluentWater = cbSrvcEffluent.Checked ? true : false;
                mdlIndustry.IsCanalWater = cbSrvcCanal.Checked ? true : false; 

                bllEWC.UpdateIndustry(mdlIndustry);  
                if(!_EditService)
                    Response.Redirect("Industry.aspx?RestoreState=1&isRecordSaved=true", false);
                else
                    Response.Redirect("IndustryServices.aspx?ID=" + mdlIndustry.ID, false);
            }
            catch (Exception ex)
            { 
                Response.Redirect("Industry.aspx?RestoreState=1&isRecordSaved=false", false);
            }
        }

        #endregion

        #region Call Backs 
        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                if (btn.ID.Equals("btnSave"))
                {
                    SaveIndustry(false);
                }
                else if (btn.ID.Equals("btnSave2"))
                {
                    SaveIndustry(true);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        } 
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList sndr = (DropDownList)sender;

                string strValue = sndr.SelectedItem.Value;
                if (sndr.ID.Equals("ddlPlantExists"))
                {
                    ddlPlantCondition.Items.FindByValue(ddlPlantCondition.SelectedValue).Selected = false;
                    ddlPlantCondition.Items.FindByText("Select").Selected = true;
                    ddlPlantCondition.Enabled = false;
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (Convert.ToInt32(strValue) == 1)
                        {
                            ddlPlantCondition.Enabled = true;
                        }
                    }
                }  
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
         
        #endregion

        
    }
}
 