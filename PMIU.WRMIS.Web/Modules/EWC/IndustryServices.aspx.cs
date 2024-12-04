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
    public partial class IndustryServices : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        EC_EffuentWaterDetails mdlEffWtrDtl = new EC_EffuentWaterDetails();
        List<EC_CanalSpecialWater> lstCSW = new List<EC_CanalSpecialWater>();
        List<object> lstCSWGrid = new List<object>();

        string CSW_DataGrid_Edit = "CSWDataGridEdit";
        string CSW_Data_Edit = "CSWDataEdit";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    long industryID = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                        industryID = Convert.ToInt64(Request.QueryString["ID"]);

                    EC_Industry mdl = bllEWC.GetIndustry(industryID, string.Empty);
                    if (mdl != null)
                        LoadIndustryDetails(mdl);

                    Session[CSW_DataGrid_Edit] = null;
                    Session[CSW_Data_Edit] = null;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadIndustryDetails(EC_Industry _Industry)
        {
            lblID.Text = _Industry.ID.ToString();
            lblName.Text = _Industry.IndustryName;
            lblType.Text = _Industry.EC_IndustryType.Name;
            lblStatus.Text = _Industry.IndustryStatus;

            //if ((bool)_Industry.IsEffluentWater)
            {
                pnlEffluents.Visible = true;
                LoadEffluentWatersAddForm();

                if (bllEWC.IsBillingTheService(_Industry.ID, Constants.ECWServiceType.EFFLUENT.ToString()))
                {
                    LoadEffluentWatersDetails(bllEWC.GetIndustryEffuentWaterDetails(_Industry.ID));
                    divEffDeAc.Visible = true;
                }
            }

            // if ((bool)_Industry.IsCanalWater)
            {
                pnlCSW.Visible = true;
                List<object> lstDivisions = bllEWC.GetDivisionsByDomain(
                       bllEWC.GetDomainByInfrastructureID((long)Constants.StructureType.Channel));

                Dropdownlist.DDLLoading(ddlDiv_CSW, false, (int)Constants.DropDownFirstOption.Select, lstDivisions);
                if (bllEWC.IsBillingTheService(_Industry.ID, Constants.ECWServiceType.CANAL.ToString()))
                {
                    string Value = bllEWC.GetIndustryCSWDivision(_Industry.ID).ToString();
                    if (Value != "-1")
                    {
                        ddlDiv_CSW.Items.FindByValue(bllEWC.GetIndustryCSWDivision(_Industry.ID).ToString()).Selected = true;
                        ddlDiv_CSW.Enabled = false;
                    }
                }
                CSW_BindGrid(_Industry.ID);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadEffluentWatersDivisionDDL(bool _IsDrain)
        {
            ddlDiv.Items.Clear();
            if (_IsDrain)
            {
                //List<object> lstDivisions = bllEWC.GetDivisionsByDomain(
                //    bllEWC.GetDomainByInfrastructureID((long)Constants.StructureType.Drain));

                List<object> lstDivisions = bllEWC.GetDivisions();

                Dropdownlist.DDLLoading(ddlDiv, false, (int)Constants.DropDownFirstOption.Select, lstDivisions);
            }
            else
            {
                List<object> lstDivisions = bllEWC.GetDivisionsByDomain(
                                  bllEWC.GetDomainByInfrastructureID((long)Constants.StructureType.Channel));

                Dropdownlist.DDLLoading(ddlDiv, false, (int)Constants.DropDownFirstOption.Select, lstDivisions);
            }
        }
        private void LoadEffluentWatersAddForm()
        {
            rbDrain.Checked = true;

            List<object> lstDischarge = bllEWC.DSSource_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlDschrgSrcs, false, (int)Constants.DropDownFirstOption.Select, lstDischarge);

            LoadEffluentWatersDivisionDDL(true);
            lblOutfall.Text = "Drain";
            ddlOutfall.Items.Clear();
            ddlOutfall.Items.Add(new ListItem("Select", ""));

            txtRDLeft.Text = "";
            txtRDRight.Text = "";
            txtInstlDate.Text = Utility.GetFormattedDate(DateTime.Now); //being a required field it is pre-filled
            txtInstlCost.Text = "";
            txtAgrmntSignedOn.Text = "";
            txtAgremntEndDate.Text = "";
            txtAgrmntParties.Text = "";
        }
        private void LoadEffluentWatersDetails(EC_EffuentWaterDetails _EffWtr)
        {
            radioInActiveEff.Checked = _EffWtr.InActive == null ? false : (bool)_EffWtr.InActive;
            hdnEffWtrID.Value = _EffWtr.ID.ToString();
            rbChnl.Checked = rbDrain.Checked = false;
            if (_EffWtr.InfrastructureTypeID == 6) //Channel
                rbChnl.Checked = true;
            else
                rbDrain.Checked = true;

            if (_EffWtr.DischargeSourceID != null)
                ddlDschrgSrcs.Items.FindByValue("" + _EffWtr.DischargeSourceID).Selected = true;

            LoadEffluentWatersDivisionDDL(rbDrain.Checked);

            if (ddlDiv.Items.FindByValue("" + _EffWtr.DivisionID) != null)
            {
                ddlDiv.Items.FindByValue("" + _EffWtr.DivisionID).Selected = true;
            }

            lblOutfall.Text = rbDrain.Checked ? "Drain" : "Channel";

            ddlOutfall.Items.Clear();
            Dropdownlist.DDLLoading(ddlOutfall, false, (int)Constants.DropDownFirstOption.Select,
                        bllEWC.GetChannelDrainByDivision(Convert.ToInt64(_EffWtr.DivisionID), rbChnl.Checked));
            ddlOutfall.Items.FindByValue("" + _EffWtr.StructureID).Selected = true;

            if (ddlOutfallSide.Items.FindByValue(_EffWtr.Side) != null)
            {
                ddlOutfallSide.Items.FindByValue(_EffWtr.Side).Selected = true;
            }

            if (_EffWtr.RD != null)
            {
                Tuple<string, string> tupleFrom = Calculations.GetRDValues(Convert.ToDouble(_EffWtr.RD));
                txtRDLeft.Text = tupleFrom.Item1;
                txtRDRight.Text = tupleFrom.Item2;
            }
            txtInstlDate.Text = _EffWtr.InstallationDate == null ? string.Empty : Utility.GetFormattedDate(_EffWtr.InstallationDate); //being a required field it is pre-filled
            txtInstlCost.Text = _EffWtr.InstallationCost.ToString();
            txtAgrmntSignedOn.Text = _EffWtr.AgreementSignedOn == null ? string.Empty : Utility.GetFormattedDate(_EffWtr.AgreementSignedOn);
            txtAgremntEndDate.Text = _EffWtr.AgreementEndDate == null ? string.Empty : Utility.GetFormattedDate(_EffWtr.AgreementEndDate);
            txtAgrmntParties.Text = _EffWtr.AgreementParties;

            //if bill generated 
            if (bllEWC.IndustryBillGenerated((long)_EffWtr.IndustryID, Constants.ECWServiceType.EFFLUENT.ToString()))
            {
                txtInstlDate.Enabled = false;
                txtInstlDate.CssClass = txtInstlDate.CssClass.Replace("required", "");
                ddlDiv.Enabled = false;
                ddlDiv.CssClass = ddlDiv.CssClass.Replace("required", "");
            }
        }
        private void LoadOutletByChannel(long _ChannalID)
        {
            CSW_ddlOutlet.Items.Clear();
            if (_ChannalID == 0)
                return;
            List<CO_ChannelOutlets> lstOutlets =
                bllEWC.GetOutletsByDivisionIDandChannelID(Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value), _ChannalID);
            List<object> lstOutlet = lstOutlets
                .Select(x => new { ID = x.ID, Name = Calculations.GetRDText(x.OutletRD) + "/" + x.ChannelSide }).ToList<object>();
            Dropdownlist.DDLLoading(CSW_ddlOutlet, false, (int)Constants.DropDownFirstOption.Select, lstOutlet);
        }
        private void CSW_BindGrid(long _IndustryID)
        {
            List<object> lstCSW = bllEWC.GetIndustryCSWDetails(_IndustryID);
            if (lstCSW.Count() > 0)
                ddlDiv_CSW.Enabled = false;
            else
                ddlDiv_CSW.Enabled = true;

            if (lstCSW != null)
                gv.DataSource = lstCSW;
            else
                gv.DataSource = new List<object>();

            gv.DataBind();
            if (ddlDiv_CSW.SelectedItem.Value != string.Empty)
                gv.Visible = true;
            else
                gv.Visible = false;
        }

        #region  Save Logic

        private bool isNullOREmpty(string _Value)
        {
            if (string.IsNullOrEmpty(_Value))
                return true;

            return false;
        }
        private void GetEffluentWaterDetails()
        {
            mdlEffWtrDtl = new EC_EffuentWaterDetails();
            if (!isNullOREmpty(hdnEffWtrID.Value))
                mdlEffWtrDtl.ID = Convert.ToInt64(hdnEffWtrID.Value);
            mdlEffWtrDtl.InActive = radioInActiveEff.Checked;
            mdlEffWtrDtl.IndustryID = Convert.ToInt64(lblID.Text);
            mdlEffWtrDtl.DivisionID = Convert.ToInt64(ddlDiv.SelectedItem.Value);
            mdlEffWtrDtl.InfrastructureTypeID = rbDrain.Checked ? (long)Constants.StructureType.Drain : (long)Constants.StructureType.Channel;
            mdlEffWtrDtl.StructureID = Convert.ToInt64(ddlOutfall.SelectedItem.Value);
            mdlEffWtrDtl.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);

            if (rbChnl.Checked)
            {
                if (!bllEWC.IsChannelRDWithinDivision(mdlEffWtrDtl.DivisionID, (long)mdlEffWtrDtl.StructureID, (int)mdlEffWtrDtl.RD))
                {
                    Master.ShowMessage(Message.RDNotInDivision.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            else if (rbDrain.Checked)
            {
                if (!bllEWC.IsDrainRDWithinDivision(mdlEffWtrDtl.DivisionID, (long)mdlEffWtrDtl.StructureID, (int)mdlEffWtrDtl.RD))
                {
                    Master.ShowMessage(Message.RDNotInDivision.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }

            mdlEffWtrDtl.Side = ddlOutfallSide.SelectedValue;
            if (ddlDschrgSrcs.SelectedItem.Value != string.Empty)
                mdlEffWtrDtl.DischargeSourceID = Convert.ToInt64(ddlDschrgSrcs.SelectedItem.Value);

            if (Utility.GetParsedDate(txtInstlDate.Text) > DateTime.Now)
            {
                Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                return;
            }
            mdlEffWtrDtl.InstallationDate = Utility.GetParsedDate(txtInstlDate.Text);

            if (!isNullOREmpty(txtInstlCost.Text))
                mdlEffWtrDtl.InstallationCost = Convert.ToInt32(txtInstlCost.Text);

            if (!isNullOREmpty(txtAgrmntSignedOn.Text))
                mdlEffWtrDtl.AgreementSignedOn = Utility.GetParsedDate(txtAgrmntSignedOn.Text);

            if (!isNullOREmpty(txtAgremntEndDate.Text))
            {
                if (!isNullOREmpty(txtAgrmntSignedOn.Text))
                {
                    if (Utility.GetParsedDate(txtAgrmntSignedOn.Text) > Utility.GetParsedDate(txtAgremntEndDate.Text))
                    {
                        Master.ShowMessage(Message.AgreementEndDateShouldBeGreaterThanInstalltionDate.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                mdlEffWtrDtl.AgreementEndDate = Utility.GetParsedDate(txtAgremntEndDate.Text);
            }
            mdlEffWtrDtl.AgreementParties = txtAgrmntParties.Text.Trim();

            mdlEffWtrDtl.InActive = radioInActiveEff.Checked;
            if ((bool)mdlEffWtrDtl.InActive)
                mdlEffWtrDtl.FromDate = DateTime.Now;


            if (bllEWC.IsBillingTheService((long)mdlEffWtrDtl.IndustryID, Constants.ECWServiceType.EFFLUENT.ToString()))
            {
                mdlEffWtrDtl.ModifiedDate = DateTime.Now;
                mdlEffWtrDtl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
            }
            else
            {
                mdlEffWtrDtl.CreatedDate = DateTime.Now;
                mdlEffWtrDtl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
            }

            bllEWC.UpdateIndustryEffluentWaterDetail(mdlEffWtrDtl);
            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
        }
        private void SaveIndustryEffluentWaterDetails()
        {
            try
            {
                GetEffluentWaterDetails();


                // if(Addmode)     LoadEffluentWatersAddForm();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        #endregion

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                if (btn.ID.Equals("btnEffSave"))
                {
                    SaveIndustryEffluentWaterDetails();
                }
                else if (btn.ID.Equals("btnCSWSave"))
                {
                    CSW_SaveData();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            LoadEffluentWatersDivisionDDL(rbDrain.Checked);
            lblOutfall.Text = rbDrain.Checked ? "Drain" : "Channel";

            //Canal spcecial water radio buttons
            if (radionOutlet.Checked)
            {
                divOutlet.Visible = true;
                CSW_ddlOutlet.Items.Clear();
                if (CSW_ddlChnl.SelectedItem.Value != string.Empty)
                    LoadOutletByChannel(Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value));
                divRD.Visible = false;
            }
            else
                if (radioRD.Checked)
                {
                    divRD.Visible = true;
                    CSW_txtRDL.Text = "";
                    CSW_txtRDR.Text = "";

                    divOutlet.Visible = false;
                }
            //Canal spcecial water radio buttons -end
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList sndr = (DropDownList)sender;

                string strValue = sndr.SelectedItem.Value;
                //Effluent Waters Division Dropdown 
                if (sndr.ID.Equals("ddlDiv"))
                {
                    if (string.IsNullOrEmpty(strValue))
                        return;
                    ddlOutfall.Items.Clear();
                    Dropdownlist.DDLLoading(ddlOutfall, false, (int)Constants.DropDownFirstOption.Select,
                        bllEWC.GetChannelDrainByDivision(Convert.ToInt64(ddlDiv.SelectedItem.Value), rbChnl.Checked));
                }
                ////Canal spcecial water drop downs
                else if (sndr.ID.Equals("ddlDiv_CSW"))
                {
                    if (!string.IsNullOrEmpty(sndr.SelectedItem.Value))
                    {
                        Session[CSW_DataGrid_Edit] = null;
                        Session[CSW_Data_Edit] = null;
                        CSW_BindGrid(Convert.ToInt64(lblID.Text));
                        gv.Visible = true;
                    }
                    else
                    {
                        gv.Visible = false;
                    }
                }
                else if (sndr.ID.Equals("CSW_ddlChnl"))
                {
                    //CSW_ddlOutlet.Items.Clear();
                    //if (CSW_ddlOutlet.SelectedItem.Value == string.Empty)
                    //    return;

                    //LoadOutletByChannel(Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value));

                    if (radionOutlet.Checked)
                    {
                        if (CSW_ddlChnl.SelectedItem.Value != string.Empty)
                            LoadOutletByChannel(Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value));
                    }
                }
                //Canal spcecial water drop downs - end
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void CSW_SaveData()
        {
            List<object> lst = new List<object>();
            if (Session[CSW_DataGrid_Edit] != null)
                lst = Session[CSW_DataGrid_Edit] as List<object>;

            int? RD = null;
            long? outletID = null;
            long chanelID = Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value);
            string chanel = CSW_ddlChnl.SelectedItem.Text;
            string form = radioRD.Checked ? "RD" : "Outlet";
            string formValue = "";
            string side = "";
            long? srcID = null;
            string fromName = "";

            EC_CanalSpecialWater mdl = new EC_CanalSpecialWater();
            mdl.ID = Convert.ToInt64(Hdf_Index.Value);
            mdl.IndustryID = Convert.ToInt64(lblID.Text);
            mdl.DivisionID = Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value);
            mdl.ChannelID = chanelID;

            if (radioRD.Checked)
            {
                RD = Calculations.CalculateTotalRDs(CSW_txtRDL.Text, CSW_txtRDR.Text);
                formValue = Calculations.GetRDText(RD);
                side = CSW_ddlSdie.SelectedItem.Value;
                fromName = formValue + "/" + side;
                if (!bllEWC.IsChannelRDWithinDivision(Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value), chanelID, (int)RD))
                {
                    Master.ShowMessage(Message.RDNotInDivision.Description, SiteMaster.MessageType.Error);
                    return;
                }
                EC_CanalSpecialWater mdlDuplicate = bllEWC.CSWDuplicationExists(Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value), chanelID, RD, null);
                if (mdlDuplicate != null && mdl.ID != mdlDuplicate.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            else
            {
                outletID = Convert.ToInt64(CSW_ddlOutlet.SelectedItem.Value);
                formValue = CSW_ddlOutlet.SelectedItem.Text;
                fromName = formValue;
                EC_CanalSpecialWater mdlDplct = bllEWC.CSWDuplicationExists(Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value), chanelID, null, outletID);
                if (mdlDplct != null && mdl.ID != mdlDplct.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            DateTime InsltDate = Utility.GetParsedDate(CSW_txtInstlDate.Text);

            if (CSW_ddlSupplySrc.SelectedItem.Value != string.Empty)
                srcID = Convert.ToInt64(CSW_ddlSupplySrc.SelectedItem.Value);


            mdl.SupplyFrom = fromName;
            mdl.ChannelOutletID = outletID;
            mdl.RD = RD;
            mdl.Side = side;
            mdl.SupplySourceID = srcID;
            mdl.InstallationDate = InsltDate;
            if (CSW_txtInstlCost.Text != string.Empty)
                mdl.InstallationCost = Convert.ToInt32(CSW_txtInstlCost.Text);
            if (CSW_txtAgrmntSignDate.Text != string.Empty)
                mdl.AgreementSignedOn = Utility.GetParsedDate(CSW_txtAgrmntSignDate.Text);
            if (CSW_txtAgrmntEndDate.Text != string.Empty)
                mdl.AgreementEndDate = Utility.GetParsedDate(CSW_txtAgrmntEndDate.Text);
            mdl.AgreementParties = CSW_txtAgrmntParties.Text;
            mdl.InActive = radioCSWDeActivate.Checked;
            if ((bool)mdl.InActive)
                mdl.FromDate = DateTime.Now;

            if (hdf_Mode.Value.Equals("Edit"))
            {
                mdl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.ModifiedDate = DateTime.Now;
                bllEWC.UpdateCSWRecord(mdl);
            }
            else
            {
                mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.CreatedDate = DateTime.Now;
                bllEWC.SaveIndustryCSWDetails(mdl);
            }
            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            CSW_BindGrid(Convert.ToInt64(lblID.Text));

            UpdatePanel2.Visible = false;
        }
        private void CSW_LoadAddForm()
        {
            //Load Channels
            CSW_ddlChnl.Items.Clear();
            Dropdownlist.DDLLoading(CSW_ddlChnl, false, (int)Constants.DropDownFirstOption.Select,
                bllEWC.GetChannelDrainByDivision(Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value), true));
            //Supply Source
            List<object> lstDischarge = bllEWC.DSSource_GetList().Where(x => x.IsActive == true)
                .Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(CSW_ddlSupplySrc, false, (int)Constants.DropDownFirstOption.Select, lstDischarge);

            //Radio buttons default state
            radionOutlet.Checked = false;
            radioRD.Checked = true;

            divOutlet.Visible = false;
            divRD.Visible = true;
            CSW_ddlOutlet.Items.Clear();

            //Empty Text fields
            CSW_txtRDL.Text = CSW_txtRDR.Text = CSW_txtAgrmntParties.Text = CSW_txtInstlCost.Text = string.Empty;
            CSW_txtInstlDate.Text = CSW_txtAgrmntSignDate.Text = CSW_txtAgrmntEndDate.Text = Utility.GetFormattedDate(DateTime.Now);

            UpdatePanel2.Visible = true;
        }

        public void CSW_LoadEditForm(long _Index)
        {

            {
                divCSWDeAc.Visible = true;
                EC_CanalSpecialWater mdl = bllEWC.GetCSWRecord(_Index);
                Hdf_Index.Value = mdl.ID.ToString();
                CSW_ddlChnl.Items.FindByValue(mdl.ChannelID + "").Selected = true;
                if (mdl.RD != null)
                {
                    radioRD.Checked = true;
                    divRD.Visible = true;
                    Tuple<string, string> tupleFrom = Calculations.GetRDValues(Convert.ToDouble(mdl.RD));
                    CSW_txtRDL.Text = tupleFrom.Item1;
                    CSW_txtRDR.Text = tupleFrom.Item2;

                    //Hide outlet details
                    radionOutlet.Checked = false;
                    divOutlet.Visible = false;
                }
                else
                {
                    radionOutlet.Checked = true;
                    divOutlet.Visible = true;
                    LoadOutletByChannel(Convert.ToInt64(mdl.ChannelID));
                    CSW_ddlOutlet.Items.FindByValue("" + mdl.ChannelOutletID).Selected = true;

                    //Hide RD details
                    radioRD.Checked = false;
                    divRD.Visible = false;
                }

                CSW_txtInstlDate.Text = Utility.GetFormattedDate(mdl.InstallationDate);
                CSW_txtInstlCost.Text = mdl.InstallationCost.ToString();
                CSW_txtAgrmntParties.Text = mdl.AgreementParties;
                if (mdl.SupplySourceID != null)
                    CSW_ddlSupplySrc.Items.FindByValue("" + mdl.SupplySourceID).Selected = true;
                if (mdl.AgreementSignedOn != null)
                    CSW_txtAgrmntSignDate.Text = Utility.GetFormattedDate(mdl.AgreementSignedOn);
                if (mdl.AgreementEndDate != null)
                    CSW_txtAgrmntEndDate.Text = Utility.GetFormattedDate(mdl.AgreementEndDate);
                radioCSWDeActivate.Checked = mdl.InActive == null ? false : (bool)mdl.InActive;
            }

        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string val = e.CommandName;
            if (val.Equals("Add"))
            {
                hdf_Mode.Value = "Add";
                CSW_LoadAddForm();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }
            if (val.Equals("Remove"))
            {
                long id = Convert.ToInt64(e.CommandArgument);
                bool Result = bllEWC.DeleteCSWRecord(id);
                if (Result)
                    CSW_BindGrid(Convert.ToInt64(lblID.Text));
                else
                    Master.ShowMessage(Message.ChildRecordExist.Description, SiteMaster.MessageType.Error);

            }
            if (val.Equals("Change"))
            {
                long id = Convert.ToInt64(e.CommandArgument);
                hdf_Mode.Value = "Edit";
                CSW_LoadAddForm();
                CSW_LoadEditForm(id);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }
        }
    }
}