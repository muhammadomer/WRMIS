using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class AddIndustry : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        EC_Industry mdlIndustry = new EC_Industry();
        EC_EffuentWaterDetails mdlEffWtrDtl = new EC_EffuentWaterDetails();
        List<EC_CanalSpecialWater> lstCSW = new List<EC_CanalSpecialWater>();
        List<object> lstCSWGrid = new List<object>();

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    Session[CSW_DataGrid] = null;
                    Session[CSW_Data] = null;
                    SetPageTitle();
                    LoadIndustryTypeDDL();
                    LoadEffluentWatersAddForm();
                    txtNo.Text = "";
                    CSW_HideDetail();
                    rbDrain.Checked = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void CSW_HideDetail()
        {
            pnlCSW.Visible = false;
            CSW_BindGrid();
            gv.Visible = false;
            CSW_ddlChnl.Visible = false;
            CSW_ddlOutlet.Visible = false;
            CSW_ddlSdie.Visible = false;
            CSW_ddlSupplySrc.Visible = false;
            CSW_txtRDL.Visible = false;
            CSW_txtRDR.Visible = false;
            CSW_txtInstlDate.Visible = false;
        }
        #region Loading Methods
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
            Dropdownlist.DDLLoading(ddlDschrgSrcs, false, (int)Constants.DropDownFirstOption.NoOption, lstDischarge);

            LoadEffluentWatersDivisionDDL(true);
            lblOutfall.Text = "Drain";
            ddlOutfall.Items.Clear();
            ddlOutfall.Items.Add(new ListItem("Select", ""));
            txtRDLeft.Text = "";
            txtRDRight.Text = "";

            txtInstlDate.Text = Utility.GetFormattedDate(DateTime.Now);
            txtInstlCost.Text = "";
            txtAgrmntSignedOn.Text = "";
            txtAgremntEndDate.Text = "";
            txtAgrmntParties.Text = "";
            rbChnl.Checked = false;
            rbDrain.Checked = true;

        }

        private void LoadIndustryTypeDDL()
        {
            List<object> lstIndustryType = bllEWC.IndustryType_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.Select, lstIndustryType);
        }
        #endregion

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
            mdlIndustry.ID = Convert.ToInt64(bllEWC.GetIndustryNo());
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
        }

        private void GetEffluentWaterDetails()
        {
        }

        private void ValidateSpecialWatersAddForm()
        {
            //RD within Division Range check
            //if (CSW_rbRD.Checked)
            //{
            //    //if (!bllEWC.IsChannelRDWithinDivision(mdlIndustry.DivisionID , Convert.ToInt64(CSW_ddlChannel.SelectedItem.Value)
            //    //    , Calculations.CalculateTotalRDs(CSW_txtRDLeft.Text, CSW_txtRDRight.Text)) )
            //    //{
            //    //    Master.ShowMessage(Message.RDNotInDivision.Description, SiteMaster.MessageType.Error);
            //    //    return;
            //    //}
            //}
            ////Future Date check on Installation Date
            //if (Utility.GetParsedDate(CSW_txtInstlDate.Text) > DateTime.Now)
            //{
            //    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
            //    return;
            //}
            ////Agreement signed on and Agreement End date check
            //if (!isNullOREmpty(txtAgrmntSignedOn.Text))
            //    mdlEffWtrDtl.AgreementSignedOn = Utility.GetParsedDate(txtAgrmntSignedOn.Text);
            //if (!isNullOREmpty(CSW_txtAgrmntEndDate.Text))
            //{
            //    if (!isNullOREmpty(CSW_txtAgrmntSign.Text))
            //    {
            //        if (Utility.GetParsedDate(CSW_txtAgrmntSign.Text) > Utility.GetParsedDate(CSW_txtAgrmntEndDate.Text))
            //        {
            //            Master.ShowMessage(Message.AgreementEndDateShouldBeGreaterThanInstalltionDate.Description, SiteMaster.MessageType.Error);
            //            return;
            //        }
            //    }
            //}
            //Unique combination check
            List<EC_CanalSpecialWater> lstDataList = Session["CSWData"] as List<EC_CanalSpecialWater>;
            if (lstDataList.Count > 0)
            {
            }
        }

        private void SaveIndustry()
        {
            try
            {
                GetIndustryData();
                if (bllEWC.GetIndustry(mdlIndustry.ID, string.Empty) != null)
                {
                    mdlIndustry.ID = Convert.ToInt64(bllEWC.GetIndustryNo());
                }
                if (!cbSrvcEffluent.Checked && !cbSrvcCanal.Checked)
                {
                    Master.ShowMessage(Message.SelectAService.Description, SiteMaster.MessageType.Error);
                    return;
                }
                mdlIndustry.IsEffluentWater = cbSrvcEffluent.Checked ? true : false;
                mdlIndustry.IsCanalWater = cbSrvcCanal.Checked ? true : false;

                if (cbSrvcEffluent.Checked)
                {
                    #region GetEffluentWaterDetails

                    mdlEffWtrDtl = new EC_EffuentWaterDetails();
                    mdlEffWtrDtl.IndustryID = mdlIndustry.ID;
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
                    mdlEffWtrDtl.CreatedDate = DateTime.Now;
                    mdlEffWtrDtl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    #endregion
                }
                else
                    mdlEffWtrDtl = null;

                if (cbSrvcCanal.Checked)
                {
                    lstCSW = Session[CSW_Data] as List<EC_CanalSpecialWater>;
                }
                else
                    lstCSW = null;

                bllEWC.SaveIndustry(mdlIndustry, mdlEffWtrDtl, lstCSW);
                Response.Redirect("Industry.aspx?RestoreState=1&isRecordSaved=true", false);
            }
            catch (Exception ex)
            {
                // Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
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
                    CSW_HideAddForm();
                    SaveIndustry();
                }
                else if (btn.ID.Equals("btnBack"))
                {
                    //Response.Redirect("AddIndustry.aspx", false);
                }
                else if (btn.ID.Equals("btnCSWSave"))
                {
                    CSW_GetFormData();
                    CSW_HideAddForm();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void cb_CheckedChanged(object sender, EventArgs e)
        {
            pnlEffluents.Visible = cbSrvcEffluent.Checked;
            if (cbSrvcEffluent.Checked)
                LoadEffluentWatersAddForm();
        }

        protected void cbCanal_CheckedChanged(object sender, EventArgs e)
        {
            pnlCSW.Visible = cbSrvcCanal.Checked;
            if (cbSrvcCanal.Checked)
            {
                List<object> lstDivisions = bllEWC.GetDivisionsByDomain(
                        bllEWC.GetDomainByInfrastructureID((long)Constants.StructureType.Channel));

                Dropdownlist.DDLLoading(ddlDiv_CSW, false, (int)Constants.DropDownFirstOption.Select, lstDivisions);
                gv.Visible = false;
                CSW_BindGrid();
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
            else if (radioRD.Checked)
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
                else if (sndr.ID.Equals("ddlPlantExists"))
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
                //Canal spcecial water drop downs
                else if (sndr.ID.Equals("ddlDiv_CSW"))
                {
                    if (!string.IsNullOrEmpty(sndr.SelectedItem.Value))
                    {
                        Session[CSW_DataGrid] = null;
                        Session[CSW_Data] = null;
                        CSW_BindGrid();
                        gv.Visible = true;
                    }
                    else
                    {
                        gv.Visible = false;
                    }
                }
                else if (sndr.ID.Equals("CSW_ddlChnl"))
                {
                    CSW_ddlOutlet.Items.Clear();
                    if (CSW_ddlOutlet.SelectedItem.Value == string.Empty)
                        return;

                    LoadOutletByChannel(Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value));
                }
                //Canal spcecial water drop downs - end
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion

        string CSW_DataGrid = "CanalWaterGird";
        string CSW_Data = "CanalWaterData";
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
        private void CSW_GetFormData()
        {
            List<object> lst = new List<object>();
            if (Session[CSW_DataGrid] != null)
                lst = Session[CSW_DataGrid] as List<object>;
            DateTime InsltDate = Utility.GetParsedDate(CSW_txtInstlDate.Text);
            if (InsltDate > DateTime.Now)
            {
                Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                return;
            }

            int? RD = null;
            long? outletID = null;
            long chanelID = Convert.ToInt64(CSW_ddlChnl.SelectedItem.Value);
            string chanel = CSW_ddlChnl.SelectedItem.Text;
            string form = radioRD.Checked ? "RD" : "Outlet";
            string formValue = "";
            string side = "";
            long? srcID = null;
            string fromName = "";

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
            }
            else
            {
                outletID = Convert.ToInt64(CSW_ddlOutlet.SelectedItem.Value);
                formValue = CSW_ddlOutlet.SelectedItem.Text;
                fromName = formValue;
            }


            if (CSW_ddlSupplySrc.SelectedItem.Value != string.Empty)
                srcID = Convert.ToInt64(CSW_ddlSupplySrc.SelectedItem.Value);

            List<EC_CanalSpecialWater> lstData = new List<EC_CanalSpecialWater>();
            if (Session[CSW_Data] != null)
                lstData = Session[CSW_Data] as List<EC_CanalSpecialWater>;

            if (lstData == null || lstData.Count == 0)
                lstData = new List<EC_CanalSpecialWater>();

            EC_CanalSpecialWater mdl = new EC_CanalSpecialWater();
            //TODO: industry ID
            mdl.DivisionID = Convert.ToInt64(ddlDiv_CSW.SelectedItem.Value);
            mdl.ChannelID = chanelID;
            mdl.SupplyFrom = fromName;
            mdl.ChannelOutletID = outletID;
            mdl.RD = RD;
            mdl.Side = side;
            mdl.SupplySourceID = srcID;
            mdl.InstallationDate = InsltDate;
            mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
            mdl.CreatedDate = DateTime.Now;
            if (CSW_txtAgrmntEndDate.Text != string.Empty)
                mdl.AgreementEndDate = Utility.GetParsedDate(CSW_txtAgrmntEndDate.Text);
            mdl.AgreementParties = CSW_txtAgrmntParties.Text;
            if (CSW_txtInstlCost.Text != string.Empty)
                mdl.InstallationCost = Convert.ToInt32(CSW_txtInstlCost.Text);
            if (CSW_txtAgrmntSignDate.Text != string.Empty)
                mdl.AgreementSignedOn = Utility.GetParsedDate(CSW_txtAgrmntSignDate.Text);

            if (hdf_Mode.Value.Equals("Edit"))
            {
                int index = Convert.ToInt32(Hdf_Index.Value);
                lstData.RemoveAt(index);
                lst.RemoveAt(index);


                List<object> lstNew = new List<object>();
                List<EC_CanalSpecialWater> lstDataNew = new List<EC_CanalSpecialWater>();
                int k = 0;

                foreach (var item in lst)
                {
                    item.GetType().GetProperty("ID").SetValue("ID", k);
                    lstNew.Add(item);
                    k = k + 1;
                }

                foreach (var item in lstData)
                    lstDataNew.Add(item);

                int id = lstNew.Count() + 1;
                lstDataNew.Add(mdl);
                lstNew.Add(new { ID = id, Channel = chanel, SForm = form, RO = formValue, Date = Utility.GetFormattedDate(InsltDate) });

                Session[CSW_DataGrid] = lstNew;
                Session[CSW_Data] = lstDataNew;
            }
            else
            {
                int id = lst.Count() + 1;
                lstData.Add(mdl);
                lst.Add(new { ID = id, Channel = chanel, SForm = form, RO = formValue, Date = Utility.GetFormattedDate(InsltDate) });

                Session[CSW_DataGrid] = lst;
                Session[CSW_Data] = lstData;
            }

            CSW_BindGrid();
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
        }
        private void CSW_BindGrid()
        {
            if (Session[CSW_DataGrid] != null)
                gv.DataSource = Session[CSW_DataGrid] as List<object>;
            else
                gv.DataSource = new List<object>();

            gv.DataBind();
            CSW_HideAddForm();
        }

        private void CSW_HideAddForm()
        {
            CSW_ddlChnl.Visible = false;
            CSW_ddlOutlet.Visible = false;
            CSW_ddlSdie.Visible = false;
            CSW_ddlSupplySrc.Visible = false;
            CSW_txtRDL.Visible = false;
            CSW_txtRDR.Visible = false;
            CSW_txtInstlDate.Visible = false;

        }
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string val = e.CommandName;
            if (val.Equals("Add"))
            {
                CSW_ddlChnl.Visible = true;
                CSW_ddlOutlet.Visible = true;
                CSW_ddlSdie.Visible = true;
                CSW_ddlSupplySrc.Visible = true;
                CSW_txtRDL.Visible = true;
                CSW_txtRDR.Visible = true;
                CSW_txtInstlDate.Visible = true;
                hdf_Mode.Value = "Add";
                CSW_LoadAddForm();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }
            if (val.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (Session[CSW_DataGrid] != null)
                {
                    List<object> lstTemp = Session[CSW_DataGrid] as List<object>;
                    lstTemp.RemoveAt(index - 1);
                    Session[CSW_DataGrid] = lstTemp;
                }
                if (Session[CSW_Data] != null)
                {
                    List<EC_CanalSpecialWater> lstTempp = Session[CSW_Data] as List<EC_CanalSpecialWater>;
                    lstTempp.RemoveAt(index - 1);
                    Session[CSW_Data] = lstTempp;
                }

                CSW_BindGrid();
            }
            if (val.Equals("Change"))
            {
                CSW_ddlChnl.Visible = true;
                CSW_ddlOutlet.Visible = true;
                CSW_ddlSdie.Visible = true;
                CSW_ddlSupplySrc.Visible = true;
                CSW_txtRDL.Visible = true;
                CSW_txtRDR.Visible = true;
                CSW_txtInstlDate.Visible = true;

                int index = Convert.ToInt32(e.CommandArgument);
                hdf_Mode.Value = "Edit";
                Hdf_Index.Value = (index - 1) + "";
                CSW_LoadAddForm();
                CSW_LoadEditForm(index);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
            }
        }

        public void CSW_LoadEditForm(int _Index)
        {
            if (Session[CSW_Data] != null)
            {
                EC_CanalSpecialWater mdl = (Session[CSW_Data] as List<EC_CanalSpecialWater>).ElementAt(_Index - 1);

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
            }

        }
    }
}