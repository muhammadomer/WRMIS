using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
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
    public partial class TempIndustry : BasePage
    {
        Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        List<CanalWater> lstNew = new List<CanalWater>();
        string CanalSpecialWater = "CanalSpecialWater";
        string EditRowIndex = "RowIndex";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    List<object> lstDivisions = bllEWC.GetDivisionsByDomain(
                                 bllEWC.GetDomainByInfrastructureID((long)Constants.StructureType.Channel));

                    Dropdownlist.DDLLoading(ddlDiv, false, (int)Constants.DropDownFirstOption.Select, lstDivisions);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region

        private void LoadSpecialWatersAddForm(bool _AddMode)
        {
            List<object> lst = bllEWC.GetChannelDrainByDivision(Convert.ToInt64(ddlDiv.SelectedItem.Value), true);
            Dropdownlist.DDLLoading(CSW_ddlChannel, false, (int)Constants.DropDownFirstOption.Select, lst);



            List<object> lstDschrgSrc = bllEWC.DSSource_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(CSW_ddlDschrg, false, (int)Constants.DropDownFirstOption.Select, lstDschrgSrc);

            CSW_txtInstlDate.Text = Utility.GetFormattedDate(DateTime.Now);
            CSW_txtAgrmntSign.Text = Utility.GetFormattedDate(DateTime.Now);

            divOutlet_CSW.Visible = false;

            CSW_rbRD.Checked = true;

            if (_AddMode)
            {
                CSW_txtRDLeft.Text = "";
                CSW_txtRDRight.Text = "";
                CSW_ddlChnlSide.ClearSelection();
                CSW_txtInstlCost.Text = "";
                CSW_txtAgrmntPrty.Text = "";
                CSW_ddlOutlet.ClearSelection();
                CSW_txtAgrmntEndDate.Text = "";

                CSW_rbRD.Checked = true;
                CSW_rbOutlet.Checked = false;
                divRD_CSW.Visible = true;
                divOutlet_CSW.Visible = false;
            }
        }

        protected void gvSCW_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    if (string.IsNullOrEmpty(ddlDiv.SelectedItem.Value))
                    {
                        Master.ShowMessage(Message.SelectADivision.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    LoadSpecialWatersAddForm(true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);
                    ViewState[EditRowIndex] = null;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            try
            {
                if (ViewState[CanalSpecialWater] != null)
                    gvSCW.DataSource = (List<CanalWater>)ViewState[CanalSpecialWater];
                else
                    gvSCW.DataSource = new List<object>();
                gvSCW.DataBind();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [Serializable]
        public class CanalWater
        {
            public long IndustryID { get; set; }
            public long DivisionID { get; set; }
            public long ChannelID { get; set; }
            public string ChannelName { get; set; }
            public long? ChannelOutletID { get; set; }
            public string ChannelOutletName { get; set; }
            public string SupplyFrom { get; set; }
            public int? RD { get; set; }
            public string CalculatedRD { get; set; }
            public string Side { get; set; }
            public long SupplySourceID { get; set; }
            public DateTime InstallationDate { get; set; }
            public string InstallationDateForGrid { get; set; }
            public int InstallationCost { get; set; }
            public DateTime AgreementSignedOn { get; set; }
            public DateTime AgreementEndDate { get; set; }
            public string AgreementParties { get; set; }

        }

        private bool CheckDuplicateValue(List<CanalWater> lstCanalWater)
        {
            long ChannelID = Convert.ToInt64(CSW_ddlChannel.SelectedItem.Value);
            DateTime InstallationDate = Convert.ToDateTime(CSW_txtInstlDate.Text);
            bool isExist = false;

            if (CSW_rbRD.Checked)
            {
                int RD = Calculations.CalculateTotalRDs(CSW_txtRDLeft.Text, CSW_txtRDRight.Text);
                string Side = Convert.ToString(CSW_ddlChnlSide.SelectedItem.Value);
                isExist = lstCanalWater.Any(x => x.ChannelID == ChannelID && x.RD == RD && x.InstallationDate == InstallationDate && x.Side == Side);
            }
            else
            {
                long OutletID = CSW_ddlOutlet.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(CSW_ddlOutlet.SelectedItem.Value);
                isExist = lstCanalWater.Any(x => x.ChannelID == ChannelID && x.ChannelOutletID == OutletID && x.InstallationDate == InstallationDate);
            }

            return isExist;
        }

        protected void btnCSWSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[EditRowIndex] != null) //Update Existing Record
                {
                    if (ViewState[CanalSpecialWater] != null)
                    {
                        int index = Convert.ToInt32(ViewState[EditRowIndex]);

                        lstNew = (List<CanalWater>)ViewState[CanalSpecialWater];


                        if (CheckDuplicateValue(lstNew))
                        {
                            Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                            return;
                        }



                        lstNew[index] = CreateDataModel();
                    }
                }
                else //Add New Record
                {


                    if (ViewState[CanalSpecialWater] != null)
                        lstNew = (List<CanalWater>)ViewState[CanalSpecialWater];


                    if (CheckDuplicateValue(lstNew))
                    {
                        Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    CanalWater mdlCSW = CreateDataModel();
                    lstNew.Add(mdlCSW);


                }
                ViewState[CanalSpecialWater] = lstNew;
                BindGrid();
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private CanalWater CreateDataModel()
        {
            int? RD_ = null;
            if (CSW_rbRD.Checked)
            {
                //Calculate RD
                RD_ = Calculations.CalculateTotalRDs(CSW_txtRDLeft.Text, CSW_txtRDRight.Text);
            }

            CanalWater mdlCSW = new CanalWater();

            mdlCSW.IndustryID = 1;
            mdlCSW.DivisionID = Convert.ToInt64(ddlDiv.SelectedItem.Value);
            mdlCSW.ChannelID = Convert.ToInt64(CSW_ddlChannel.SelectedItem.Value);
            mdlCSW.ChannelName = Convert.ToString(CSW_ddlChannel.SelectedItem.Text);
            mdlCSW.ChannelOutletID = CSW_ddlOutlet.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(CSW_ddlOutlet.SelectedItem.Value);
            mdlCSW.ChannelOutletName = CSW_ddlOutlet.SelectedItem.Value == string.Empty ? "" : Convert.ToString(CSW_ddlOutlet.SelectedItem.Text);
            mdlCSW.SupplyFrom = CSW_rbRD.Checked ? CSW_rbRD.Text.Trim().ToUpper() : CSW_rbOutlet.Text.Trim().ToUpper();
            mdlCSW.RD = RD_;
            mdlCSW.CalculatedRD = Calculations.GetRDText(RD_);
            mdlCSW.Side = CSW_ddlChnlSide.SelectedItem.Value;
            mdlCSW.SupplySourceID = Convert.ToInt64(CSW_ddlDschrg.SelectedItem.Value);
            mdlCSW.InstallationDate = Convert.ToDateTime(CSW_txtInstlDate.Text);
            mdlCSW.InstallationDateForGrid = Utility.GetFormattedDate(Convert.ToDateTime(CSW_txtInstlDate.Text));
            mdlCSW.InstallationCost = Convert.ToInt32(CSW_txtInstlCost.Text);
            mdlCSW.AgreementSignedOn = Convert.ToDateTime(CSW_txtAgrmntSign.Text);
            mdlCSW.AgreementEndDate = Convert.ToDateTime(CSW_txtAgrmntEndDate.Text);
            mdlCSW.AgreementParties = CSW_txtAgrmntPrty.Text;

            return mdlCSW;

        }

        protected void gvSCW_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (ViewState[CanalSpecialWater] != null)
                {
                    List<CanalWater> lstCSW = (List<CanalWater>)ViewState[CanalSpecialWater];

                    if (lstCSW.Count > 0)
                    {
                        lstCSW.RemoveAt(e.RowIndex);
                        ViewState[CanalSpecialWater] = lstCSW;
                        gvSCW.DataSource = lstCSW;
                        gvSCW.DataBind();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSCW_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int RowIndex = e.NewEditIndex;
                ViewState[EditRowIndex] = RowIndex;
                LoadSpecialWatersAddForm(false);

                List<CanalWater> lstCSW = (List<CanalWater>)ViewState[CanalSpecialWater];

                Dropdownlist.SetSelectedValue(CSW_ddlChannel, Convert.ToString(lstCSW[RowIndex].ChannelID));

                if (lstCSW[RowIndex].SupplyFrom.Trim().ToUpper() == "RD")
                {
                    CSW_rbRD.Checked = true;
                    CSW_rbOutlet.Checked = false;
                    divRD_CSW.Visible = true;
                    divOutlet_CSW.Visible = false;
                }
                else
                {
                    CSW_rbOutlet.Checked = true;
                    CSW_rbRD.Checked = false;
                    divRD_CSW.Visible = false;
                    divOutlet_CSW.Visible = true;
                }

                Tuple<string, string> tuple = Calculations.GetRDValues(lstCSW[RowIndex].RD);
                CSW_txtRDLeft.Text = tuple.Item1;
                CSW_txtRDRight.Text = tuple.Item2;

                Dropdownlist.SetSelectedValue(CSW_ddlChnlSide, lstCSW[RowIndex].Side);
                Dropdownlist.SetSelectedValue(CSW_ddlDschrg, Convert.ToString(lstCSW[RowIndex].SupplySourceID));

                CSW_txtInstlDate.Text = Convert.ToString(Utility.GetFormattedDate(lstCSW[RowIndex].InstallationDate));
                CSW_txtInstlCost.Text = Convert.ToString(lstCSW[RowIndex].InstallationCost);
                CSW_txtAgrmntSign.Text = Convert.ToString(Utility.GetFormattedDate(lstCSW[RowIndex].AgreementSignedOn));
                CSW_txtAgrmntEndDate.Text = Convert.ToString(Utility.GetFormattedDate(lstCSW[RowIndex].AgreementEndDate));
                CSW_txtAgrmntPrty.Text = Convert.ToString(lstCSW[RowIndex].AgreementParties);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "openModal();", true);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvSCW_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblRD = (Label)e.Row.FindControl("lblRD");
                    Label lblOutlet = (Label)e.Row.FindControl("lblOutlet");
                    if (lblRD.Text == "-")
                    {
                        lblRD.Visible = false;
                    }
                    else
                    {
                        lblOutlet.Visible = false;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void CSW_ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (CSW_ddlChannel.SelectedItem.Value == "")
                {

                    CSW_rbRD_CheckedChanged(null, null);

                }
                else
                {
                    CSW_ddlOutlet.Enabled = true;

                    List<CO_ChannelOutlets> lstOutlets = bllEWC.GetOutletsByDivisionIDandChannelID(Convert.ToInt64(ddlDiv.SelectedItem.Value), Convert.ToInt64(CSW_ddlChannel.SelectedItem.Value));



                    List<object> lstOutlet = lstOutlets.Select(x => new { ID = x.ID, Name = Calculations.GetRDText(x.OutletRD) + "/" + x.ChannelSide }).ToList<object>();

                    Dropdownlist.DDLLoading(CSW_ddlOutlet, false, (int)Constants.DropDownFirstOption.Select, lstOutlet);

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void CSW_rbRD_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CSW_rbRD.Checked)
                {
                    divOutlet_CSW.Visible = false;
                    divRD_CSW.Visible = true;
                    CSW_ddlOutlet.ClearSelection();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script1", "Reset();", true);
                }
                else
                {
                    divRD_CSW.Visible = false;
                    divOutlet_CSW.Visible = true;
                    CSW_txtRDLeft.Text = "";
                    CSW_txtRDRight.Text = "";
                    CSW_ddlChnlSide.ClearSelection();
                    if (CSW_ddlChannel.SelectedItem.Value == "")
                    {
                        CSW_ddlOutlet.Enabled = false;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script2", "Reset();", true);
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EC_CanalSpecialWater mdlCanalSpecialWater = new EC_CanalSpecialWater();
                List<CanalWater> lstAdvertisementSource = (List<CanalWater>)ViewState[CanalSpecialWater];

                for (int i = 0; i < lstAdvertisementSource.Count(); i++)
                {
                    mdlCanalSpecialWater.IndustryID = lstAdvertisementSource[i].IndustryID;
                    mdlCanalSpecialWater.DivisionID = lstAdvertisementSource[i].DivisionID;
                    mdlCanalSpecialWater.ChannelID = lstAdvertisementSource[i].ChannelID;
                    mdlCanalSpecialWater.SupplyFrom = lstAdvertisementSource[i].SupplyFrom;

                    if (CSW_ddlOutlet.SelectedItem.Value != "")
                    {
                        mdlCanalSpecialWater.ChannelOutletID = lstAdvertisementSource[i].ChannelOutletID;
                    }
                    else
                    {
                        mdlCanalSpecialWater.ChannelOutletID = lstAdvertisementSource[i].ChannelID;
                    }

                    mdlCanalSpecialWater.RD = lstAdvertisementSource[i].RD;
                    mdlCanalSpecialWater.Side = lstAdvertisementSource[i].Side;
                    mdlCanalSpecialWater.SupplySourceID = lstAdvertisementSource[i].SupplySourceID;
                    mdlCanalSpecialWater.InstallationDate = lstAdvertisementSource[i].InstallationDate;
                    mdlCanalSpecialWater.InstallationCost = lstAdvertisementSource[i].InstallationCost;
                    mdlCanalSpecialWater.AgreementSignedOn = lstAdvertisementSource[i].AgreementSignedOn;
                    mdlCanalSpecialWater.AgreementEndDate = lstAdvertisementSource[i].AgreementEndDate;
                    mdlCanalSpecialWater.AgreementParties = lstAdvertisementSource[i].AgreementParties;


                    bllEWC.AddCanalSpecialWater(mdlCanalSpecialWater);
                }
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion
    }
}