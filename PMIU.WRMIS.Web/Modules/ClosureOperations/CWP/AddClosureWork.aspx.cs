using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
    //User of this screen is XEN only 
    public partial class AddClosureWork : BasePage
    {
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL(); 
        TenderManagementBLL bllTM = new TenderManagementBLL();
        
        private const int  D = 1, EM = 2, BW = 3, OGP = 4, OR = 5, CSW = 6; 

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadWorkTypeDDL();
                    long userID = SessionManagerFacade.UserInformation.ID;
                    long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    Session["DataRelevantUser"] = userID;
                    if (string.IsNullOrEmpty(Request.QueryString["CWID"]))
                    {
                        LoadWorkTypeDivs(Convert.ToInt32(ddlWorkType.SelectedItem.Value));
                    }


                    if (!string.IsNullOrEmpty(Request.QueryString["CWPID"]))
                    {
                        hdnF_CWP_ID.Value = Request.QueryString["CWPID"];
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/ClosureOperationPlanDetail.aspx?CWPID=" + hdnF_CWP_ID.Value;
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                    {
                        try
                        {
                            lblPageTitle.Text = "Edit Closure Work";
                            hdnF_ID.Value = Request.QueryString["CWID"];
                            if (boundryLvlID == null || boundryLvlID != 3)
                                Session["DataRelevantUser"] = -1;
                            LaodClosureWorkData(Convert.ToInt64(Request.QueryString["CWID"]));
                        }
                        catch (Exception exp)
                        {
                            new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                        }
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["ViewMode"]))
                    {
                        lblPageTitle.Text = "Closure Work";
                        hdF_Mode.Value = "V";
                        DisableFields();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
       
        #region DropDown List Events
        protected void ddlWorkType_SelectedIndexChanged(object sender, EventArgs e)
        { 
            HideWorkDivs();
            if (string.IsNullOrEmpty(ddlWorkType.SelectedItem.Value))
                return;

            int type = Convert.ToInt32(ddlWorkType.SelectedItem.Value);
            LoadWorkTypeDivs(type);
            
        }

        private void LoadWorkTypeDivs(int _WorkType)
        {
            HideWorkDivs();
            switch (_WorkType)
            {
                case D://desliting
                    LoadDiv_Desilting();
                    break;
                case EM: // Electrical/Mechanical
                    LoadDiv_ElectricalMechanical();
                    break;
                case BW://Building Work
                    LoadDiv_BuildingWork();
                    break;
                case OGP: //Oiling/Greasing/Painting
                    LoadDiv_OilingGreasingPainting();
                    break;
                case OR: //outlet painting
                    LoadDiv_OutletRepainting();
                    break;
                case CSW: //Channel Structure Work
                    LoadDiv_ChannelStructureWork();
                    break;

                default:
                    LoadDiv_OtherWork();
                    break;
            }
        }
        protected void ddlOtherSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlOtherSubDiv.SelectedItem.Value))
                return;


            List<object> lstdata = bllCO.GetSectionBySubDivID(Convert.ToInt64(ddlOtherSubDiv.SelectedItem.Value));
            Dropdownlist.DDLLoading(ddlOtherSec, false, (int)Constants.DropDownFirstOption.Select, lstdata);
            if (lstdata != null && lstdata.Count > 0)
                ddlOtherSec.Enabled = true;
            else
                ddlOtherSec.Enabled = false;
        } 
        protected void ddlORSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<object> lstData = new List<object>();

                ddlORSec.Items.Clear();
                ddlORSec.Items.Insert(0, new ListItem("Select", ""));

                if (string.IsNullOrEmpty(ddlORSubDiv.SelectedItem.Value))
                {
                    //load channels
                    lstData.Clear();
                    lstData = bllCO.GetChannelByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
                    Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);
                    return;
                } 
                long subDivID = Convert.ToInt64(ddlORSubDiv.SelectedItem.Value);
                
                lstData = bllCO.GetSectionBySubDivID(subDivID);
                Dropdownlist.DDLLoading(ddlORSec, false, (int)Constants.DropDownFirstOption.Select, lstData);

                lstData.Clear();
                lstData = bllCO.GetChannelBySubDivID(subDivID);
                Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlORSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(ddlORSec.SelectedItem.Value))
            //{
            //    long subDivID = Convert.ToInt64(ddlORSubDiv.SelectedItem.Value);
            //    List<object> lstData = new List<object>();
            //    lstData = bllCO.GetChannelBySubDivID(subDivID);
            //    Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData); 
            //}
            //else
           try {
                if (string.IsNullOrEmpty(ddlORSec.SelectedItem.Value))
                    return;
                long secID = Convert.ToInt64(ddlORSec.SelectedItem.Value);
                List<object> lstData = new List<object>();
                lstData = bllCO.GetChannelBySectionID(secID);
                Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);
            } 
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlOGPSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlOGPSubDiv.SelectedItem.Value))
                return;
            long subDivID = Convert.ToInt64(ddlOGPSubDiv.SelectedItem.Value); 
            Dropdownlist.DDLLoading(ddlOGPSec, false, (int)Constants.DropDownFirstOption.Select, bllCO.GetSectionBySubDivID(subDivID));
        }
        protected void ddl_EM_Struct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long userID = Convert.ToInt64(Session["DataRelevantUser"]);
                if (ddl_EM_Struct.SelectedItem.Text.Equals("Headwork/Barrage"))
                {
                    ddl_EM_Name.Items.Clear();
                    Dropdownlist.DDLLoading(ddl_EM_Name, false, (int)Constants.DropDownFirstOption.Select,
                       bllCO.GetBarragesByXEN(userID));
                }
                else
                {
                    ddl_EM_Name.Items.Clear();
                    Dropdownlist.DDLLoading(ddl_EM_Name, false, (int)Constants.DropDownFirstOption.Select,
                        bllCO.GetChannelByXEN(userID));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddl_D_Dstrct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddl_D_Dstrct.SelectedItem.Value))
                    return;

                long ID = Convert.ToInt64(ddl_D_Dstrct.SelectedItem.Value);
                Dropdownlist.DDLLoading(ddl_D_tehsil, false, (int)Constants.DropDownFirstOption.Select,
                    bllCO.GetTehsilByDistrict(ID));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        } 
        #endregion

        #region Helper Methods
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddAnnualCanalClosureProgram);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadWorkTypeDDL()
        {
            List<object> lstWorkType = bllCO.GetAllClosureWorkType()
                .Where(x => x.IsActive == true)
                .Select(x => new { ID = x.ID, Name = x.Name })
                .ToList<object>();
          //  lstWorkType.Add(new { ID = "0", Name = "Others" }); 

            Dropdownlist.DDLLoading(ddlWorkType, false, (int)Constants.DropDownFirstOption.NoOption, lstWorkType);
           
           // Dropdownlist.DDLLoading(ddlFundingSrc, false, (int)Constants.DropDownFirstOption.Select, bllTM.GetFundingSourceList());

           
        }

        private void LoadDiv_Desilting()
        {
            try
            {
                long userID = Convert.ToInt64(Session["DataRelevantUser"]);

                Dropdownlist.DDLLoading(ddl_D_Chnl, false, (int)Constants.DropDownFirstOption.Select,
                         bllCO.GetChannelByXEN(userID));

                Dropdownlist.DDLLoading(ddl_D_Dstrct, false, (int)Constants.DropDownFirstOption.Select,
                   bllCO.GetDistrictByXEN(userID));

                div_D.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        } 
        private void LoadDiv_ElectricalMechanical()
        {
            Dropdownlist.DDLLoading(ddl_EM_Name, false, (int)Constants.DropDownFirstOption.Select,
            bllCO.GetBarragesByXEN(Convert.ToInt64(Session["DataRelevantUser"])));
            div_EM.Visible = true;
        }
        private void LoadDiv_BuildingWork()
        {        
            cb_BW_GRH.Checked = cb_BW_Ofc.Checked = cb_BW_Othrz.Checked = cb_BW_Res.Checked = cb_BW_RHs.Checked = false; 
            div_BW.Visible = true;
        }
        private void LoadDiv_OilingGreasingPainting()
        {
            List<object> lstData = new List<object>();
            //Load Subdivisions
            lstData = bllCO.GetSubdivisionByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
            Dropdownlist.DDLLoading(ddlOGPSubDiv, false, (int)Constants.DropDownFirstOption.Select, lstData);

            cbGF.Checked = cbGP.Checked = cbGOGP.Checked = cbOthrz.Checked = false;
            div_OGP.Visible = true;
        }
        private void LoadDiv_OutletRepainting()
        {
            List<object> lstData = new List<object>();

            //Load Subdivisions
            lstData = bllCO.GetSubdivisionByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
            Dropdownlist.DDLLoading(ddlORSubDiv, false, (int)Constants.DropDownFirstOption.Select, lstData);
            //load channels
            lstData.Clear();
            lstData = bllCO.GetChannelByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
            Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);

            div_OR.Visible = true;
        }        
        private void LoadDiv_ChannelStructureWork()
        {
            List<object> lstChannels = bllCO.GetChannelByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
            Dropdownlist.DDLLoading(ddlCSWChannels, false, (int)Constants.DropDownFirstOption.Select, lstChannels);
            if (lstChannels != null && lstChannels.Count > 0)
                ddlCSWChannels.Enabled = true;
            else
                ddlCSWChannels.Enabled = false;

            div_CSW.Visible = true;
        } 
        
        private void LoadDiv_OtherWork()
        {
            List<object> lstdata = bllCO.GetSubdivisionByXEN(Convert.ToInt64(Session["DataRelevantUser"]));
            Dropdownlist.DDLLoading(ddlOtherSubDiv, false, (int)Constants.DropDownFirstOption.Select, lstdata);
            if (lstdata != null && lstdata.Count > 0)
                ddlOtherSubDiv.Enabled = true;
            else
                ddlOtherSubDiv.Enabled = false;

            div_Other.Visible = true;
        }
        private void HideWorkDivs()
        {
            div_BW.Visible = false;
            div_CSW.Visible = false;
            div_D.Visible = false;
            div_EM.Visible = false;
            div_OGP.Visible = false;
            div_OR.Visible = false;
            div_Other.Visible = false;
        }

        #endregion 

        private void SaveRecord()
        {
            try
            {
                int type = Convert.ToInt32(ddlWorkType.SelectedItem.Value);
                CW_ClosureWork mdl = new CW_ClosureWork();
                mdl.ID = Convert.ToInt64(hdnF_ID.Value);
                mdl.CWPID = Convert.ToInt64(hdnF_CWP_ID.Value);
                mdl.WorkName = txtWorkName.Text.Trim();
                mdl.FundingSourceID = 0;
                mdl.WorkTypeID = Convert.ToInt32(ddlWorkType.SelectedItem.Value);

                //Duplication check
                CW_ClosureWork mdlTemp = new CW_ClosureWork();
                mdlTemp.WorkName = mdl.WorkName;
                mdlTemp.WorkTypeID = mdl.WorkTypeID;
                mdlTemp.CWPID = mdl.CWPID;

                CW_ClosureWork mdlRecord = (CW_ClosureWork)bllCO.ClosureWork_Operations(Constants.CHECK_DUPLICATION, mdlTemp);
                if (mdlRecord != null && mdl.ID != mdlRecord.ID)
                {
                    Master.ShowMessage("Closure Work Plan ,Work Name and Work Type combination already exist.", SiteMaster.MessageType.Error);
                    return;
                }
                //End of Duplication check

                switch (type)
                {
                    case D://desliting
                        long channelID = Convert.ToInt64(ddl_D_Chnl.SelectedItem.Value);
                        mdl.DS_ChannelID = channelID;
                        mdl.DS_SiltRemoved = Convert.ToInt32(txtD_Silt.Text);
                        int fromRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);
                        int toRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);
                        if (fromRD > toRD)
                        {
                            Master.ShowMessage(Message.FromRDGreaterThanToRD.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if (!bllCO.IsRDWithinUserDivision(fromRD, Convert.ToInt64(Session["DataRelevantUser"]), channelID))
                        {
                            Master.ShowMessage(Message.FromRDNotInUserJurisdiction.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if (!bllCO.IsRDWithinUserDivision(toRD, Convert.ToInt64(Session["DataRelevantUser"]), channelID))
                        {
                            Master.ShowMessage(Message.ToRDNotInUserJurisdiction.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        mdl.DS_FromRD = fromRD;
                        mdl.DS_ToRD = toRD;
                        if (!string.IsNullOrEmpty(ddl_D_Dstrct.SelectedItem.Value))
                            mdl.DS_DistrictID = Convert.ToInt64(ddl_D_Dstrct.SelectedItem.Value);
                        else
                            mdl.DS_DistrictID = null;

                        if (!string.IsNullOrEmpty(ddl_D_tehsil.SelectedItem.Value))
                            mdl.DS_TehsilID = Convert.ToInt64(ddl_D_tehsil.SelectedItem.Value);
                        else
                            mdl.DS_TehsilID = null;
                        break;

                    case EM: // Electrical/Mechanical
                        mdl.EM_HBChannel = ddl_EM_Struct.SelectedItem.Text;
                        if (ddl_EM_Struct.SelectedItem.Text.Equals("Channel"))
                            mdl.EM_ChannelID = Convert.ToInt64(ddl_EM_Name.SelectedItem.Value);
                        else
                            mdl.EM_HBID = Convert.ToInt64(ddl_EM_Name.SelectedItem.Value);
                        break;

                    case BW://Building Work
                        mdl.BW_GRHut = cb_BW_GRH.Checked;
                        mdl.BW_Office = cb_BW_Ofc.Checked;
                        mdl.BW_Others = cb_BW_Othrz.Checked;
                        mdl.BW_Residence = cb_BW_Res.Checked;
                        mdl.BW_RestHouse = cb_BW_RHs.Checked;
                        break;

                    case OGP: //Oiling/Greasing/Painting
                        mdl.OP_GaugeFixing = cbGF.Checked;
                        mdl.OP_GaugePainting = cbGP.Checked;
                        mdl.OP_OilGreasePaint = cbGOGP.Checked;
                        mdl.OP_Others = cbOthrz.Checked;
                        mdl.OP_SubDivID = Convert.ToInt64(ddlOGPSubDiv.SelectedItem.Value);
                        if (!string.IsNullOrEmpty(ddlOGPSec.SelectedItem.Value))
                            mdl.OP_SectionID = Convert.ToInt64(ddlOGPSec.SelectedItem.Value);
                        else
                            mdl.OP_SectionID = null;
                        break;

                    case OR: //outlet painting
                        string subDiv = ddlORSubDiv.SelectedItem.Value, chnl = ddlORChannels.SelectedItem.Value;
                        if (string.IsNullOrEmpty(subDiv) && string.IsNullOrEmpty(chnl))
                        {
                            Master.ShowMessage(Message.SelectSubDivOrChannel.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        if (!string.IsNullOrEmpty(subDiv))
                        {
                            mdl.OR_SubDivID = Convert.ToInt64(subDiv);
                            if (!string.IsNullOrEmpty(ddlORSec.SelectedItem.Value))
                                mdl.OR_SectionID = Convert.ToInt64(ddlORSec.SelectedItem.Value);
                            else
                                mdl.OR_SectionID = null;
                        }
                        else
                            mdl.OR_SubDivID = null;
                        if (!string.IsNullOrEmpty(chnl))
                            mdl.OR_ChannelID = Convert.ToInt64(chnl);
                        else
                            mdl.OR_ChannelID = null;
                        break;

                    case CSW: //Channel Structure Work 
                        mdl.CS_ChannelID = Convert.ToInt64(ddlCSWChannels.SelectedItem.Value);
                        break;

                    default: //Other Work Type
                        mdl.OW_SubDivID = Convert.ToInt64(ddlOtherSubDiv.SelectedItem.Value);
                        if (!string.IsNullOrEmpty(ddlOtherSec.SelectedItem.Value))
                            mdl.OW_SectionID = Convert.ToInt64(ddlOtherSec.SelectedItem.Value);
                        else
                            mdl.OW_SectionID = null;
                        break;
                }

                mdl.FundingSourceID = null;// Convert.ToInt64(ddlFundingSrc.SelectedItem.Value);
                //bool isFixedFundingSRC = bllTM.IsFundingSourceFixed(mdl.FundingSourceID);

                //if (isFixedFundingSRC && string.IsNullOrEmpty(txtCost.Text))
                //{
                //    Master.ShowMessage("Cost is a required field.", SiteMaster.MessageType.Error);
                //    return;
                //}
                if (!string.IsNullOrEmpty(txtCost.Text))
                {
                    if (Convert.ToInt64(txtCost.Text) <= 0)
                    {
                        Master.ShowMessage("Enter a valid Estimated Cost value.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdl.EstimatedCost = Convert.ToInt64(txtCost.Text);
                } 

                //if (isFixedFundingSRC && cbClsurPrd.Checked == false && string.IsNullOrEmpty(txtCompPrd.Text))
                //{
                //    Master.ShowMessage("Completion Period is a required field.", SiteMaster.MessageType.Error);
                //    return;
                //}
                mdl.CompletionPeriodFlag = cbClsurPrd.Checked;
                if (!string.IsNullOrEmpty(txtCompPrd.Text) && cbClsurPrd.Checked == false)
                {
                    mdl.CompletionPeriod = Convert.ToInt32(txtCompPrd.Text);
                    mdl.CompletionPeriodUnit = ddlCompPrdType.SelectedItem.Value;
                }
                else
                {
                    mdl.CompletionPeriod = null;
                    mdl.CompletionPeriodUnit = null;
                }
                if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    DateTime fromDate = Utility.GetParsedDate(txtStartDate.Text); DateTime toDate = Utility.GetParsedDate(txtEndDate.Text);
                    if (fromDate > toDate)
                    {
                        Master.ShowMessage(Message.StartDateCannotBeGreaterThanEndDate.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    if (!IsValidDate(Utility.GetParsedDate(txtStartDate.Text)))
                    {
                        Master.ShowMessage("Enter a valid start date.", SiteMaster.MessageType.Error);
                        return;
                    }
                    mdl.StartDate = Utility.GetParsedDate(txtStartDate.Text);
                }
                
                else
                    mdl.StartDate = null;

                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    if (!IsValidDate(Utility.GetParsedDate(txtEndDate.Text)))
                    {
                        Master.ShowMessage("Enter a valid end date.", SiteMaster.MessageType.Error);
                        return;
                    }
                     mdl.EndDate = Utility.GetParsedDate(txtEndDate.Text);
                }
                   
                else
                    mdl.EndDate = null;


                //if (isFixedFundingSRC && string.IsNullOrEmpty(txtSanctionNo.Text))
                //{
                //    Master.ShowMessage("Sanction No. is a required field.", SiteMaster.MessageType.Error);
                //    return;
                //}
                mdl.SanctionNo = txtSanctionNo.Text;
                //if (isFixedFundingSRC && string.IsNullOrEmpty(TxtSnctnDate.Text))
                //{
                //    Master.ShowMessage("Sanction Date is a required field.", SiteMaster.MessageType.Error);
                //    return;
                //}
                if (!string.IsNullOrEmpty(TxtSnctnDate.Text))
                    mdl.SanctionDate = Utility.GetParsedDate(TxtSnctnDate.Text);
                if (Convert.ToInt64(txtErnsMny.Text) <= 0)
                {
                    Master.ShowMessage("Enter a valid Earnest Money value.", SiteMaster.MessageType.Error);
                    return;
                }
                mdl.EarnestMoney = Convert.ToInt64(txtErnsMny.Text);
                mdl.EarnestMoneyType = ddlErnsMnyType.SelectedItem.Value;
                if (ddlErnsMnyType.SelectedItem.Value.Equals("% of Financial Bid"))
                {
                    if (mdl.EarnestMoney > 5)
                    {
                        Master.ShowMessage("% of Financial Bid cannot be more than 5%.", SiteMaster.MessageType.Error);
                        return;
                    }
                }
                if (Convert.ToInt64(txtndrFee.Text) <= 0)
                {
                    Master.ShowMessage("Enter a valid Tender Fee value.", SiteMaster.MessageType.Error);
                    return;
                }
                mdl.TenderFees = Convert.ToInt32(txtndrFee.Text);
                mdl.Description = txtDesc.Text.Trim();


                if (mdl.ID == 0)
                {
                    mdl.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdl.CreatedDate = DateTime.Now;
                }
                else
                {
                    mdl.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdl.ModifiedDate = DateTime.Now;
                }

                bool isRecordSaved = false;
                if (mdl.ID == 0)
                    isRecordSaved = (bool)bllCO.ClosureWork_Operations(Constants.CRUD_CREATE, mdl);
                else
                    isRecordSaved = (bool)bllCO.ClosureWork_Operations(Constants.CRUD_UPDATE, mdl);


                Response.Redirect("ClosureOperationPlanDetail.aspx?CWPID=" + hdnF_CWP_ID.Value + "&isRecordSaved=" + isRecordSaved, false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void DisableFields()
        {
            txtWorkName.Enabled = false; 
          //  ddlFundingSrc.Enabled = false; 
            ddlWorkType.Enabled = false;

            ddl_D_Chnl.Enabled = false;
            txtD_Silt.Enabled = false;
            txtFromRDLeft.Enabled = false;
            txtFromRDRight.Enabled = false;
            txtToRDLeft.Enabled = false;
            txtToRDRight.Enabled = false;
            ddl_D_Dstrct.Enabled = false;
            ddl_D_tehsil.Enabled = false;

            ddl_EM_Struct.Enabled = false;
            ddl_EM_Name.Enabled = false;

            cb_BW_GRH.Enabled = false;
            cb_BW_Ofc.Enabled = false;
            cb_BW_Othrz.Enabled = false;
            cb_BW_Res.Enabled = false;
            cb_BW_RHs.Enabled = false;

            cbGF.Enabled = false;
            cbGP.Enabled = false;
            cbGOGP.Enabled = false;
            cbOthrz.Enabled = false;
            ddlOGPSubDiv.Enabled = false;
            ddlOGPSec.Enabled = false;

            ddlORSubDiv.Enabled = false;
            ddlORSec.Enabled = false;
            ddlORChannels.Enabled = false;

            ddlCSWChannels.Enabled = false;
            ddlOtherSubDiv.Enabled = false;
            ddlOtherSec.Enabled = false;

            txtCost.Enabled = false;
            cbClsurPrd.Enabled = false;
            txtCompPrd.Enabled = false;
            ddlCompPrdType.Enabled = false;

            divStartDate.Disabled = true;
            txtStartDate.Enabled = false;
            divEndDate.Disabled = true;
            txtEndDate.Enabled = false;
            txtSanctionNo.Enabled = false;
            divSanctionDate.Disabled = true;
            TxtSnctnDate.Enabled = false;
            txtErnsMny.Enabled = false;
            ddlErnsMnyType.Enabled = false;
            txtndrFee.Enabled = false;
            txtDesc.Enabled = false;

            btnSave.Visible = false;
        }

        private void LaodClosureWorkData(long _ClosureWorkID)
        {
            CW_ClosureWork mdlParam = new CW_ClosureWork();     mdlParam.ID = _ClosureWorkID;
            CW_ClosureWork mdl= (CW_ClosureWork)bllCO.ClosureWork_Operations(Constants.CRUD_READ, mdlParam);


            if (Convert.ToInt32( Session["DataRelevantUser"].ToString()) == -1 )
                Session["DataRelevantUser"] = mdl.CreatedBy;

            hdnF_ID.Value = "" + mdl.ID;
            hdnF_CWP_ID.Value = "" + mdl.CWPID;
            txtWorkName.Text = mdl.WorkName.Trim() ;
           // ddlFundingSrc.Items.FindByValue("" + mdl.FundingSourceID).Selected = true; 
            ddlWorkType.Items.FindByValue("" + mdl.WorkTypeID).Selected = true;
            ddlWorkType.Enabled = false;

            int type = Convert.ToInt32(ddlWorkType.SelectedItem.Value);
            HideWorkDivs();
            try
            {
                switch (type)
                {
                    case D://desliting
                        LoadDiv_Desilting();
                        ddl_D_Chnl.Items.FindByValue("" + mdl.DS_ChannelID).Selected = true; 
                        txtD_Silt.Text = "" + mdl.DS_SiltRemoved;
                        Tuple<string, string> tupleFrom= Calculations.GetRDValues( Convert.ToDouble(mdl.DS_FromRD) );
                        txtFromRDLeft.Text = tupleFrom.Item1;
                        txtFromRDRight.Text = tupleFrom.Item2;
                         Tuple<string, string> tupleTo = Calculations.GetRDValues( Convert.ToDouble(mdl.DS_ToRD) );
                        txtToRDLeft.Text = tupleTo.Item1;
                        txtToRDRight.Text = tupleTo.Item2;

                        ddl_D_Dstrct.Items.FindByValue(mdl.DS_DistrictID == null ? "" : mdl.DS_DistrictID + "").Selected = true;
                        if (mdl.DS_DistrictID != null)
                        {
                            Dropdownlist.DDLLoading(ddl_D_tehsil, false, (int)Constants.DropDownFirstOption.Select,
                                    bllCO.GetTehsilByDistrict(Convert.ToInt64(mdl.DS_DistrictID)));
                            ddl_D_tehsil.Items.FindByValue(mdl.DS_TehsilID == null ? "" : mdl.DS_TehsilID + "").Selected = true;
                        }
                        break;

                    case EM: // Electrical/Mechanical  
                        ddl_EM_Struct.Items.FindByText(mdl.EM_HBChannel).Selected = true;
                        if (ddl_EM_Struct.SelectedItem.Text.Equals("Headwork/Barrage"))
                        {
                            ddl_EM_Name.Items.Clear();
                            Dropdownlist.DDLLoading(ddl_EM_Name, false, (int)Constants.DropDownFirstOption.Select,
                               bllCO.GetBarragesByXEN(Convert.ToInt64(Session["DataRelevantUser"])));
                            try
                            {
                                ddl_EM_Name.Items.FindByValue("" + mdl.EM_HBID).Selected = true;
                            }
                            catch (Exception ex)
                            {
                                ddl_EM_Name.Items.FindByValue("").Selected = true;
                            }
                        }
                        else
                        {
                            ddl_EM_Name.Items.Clear();
                            Dropdownlist.DDLLoading(ddl_EM_Name, false, (int)Constants.DropDownFirstOption.Select,
                                bllCO.GetChannelByXEN(Convert.ToInt64(Session["DataRelevantUser"])));
                             ddl_EM_Name.Items.FindByValue("" + mdl.EM_ChannelID).Selected = true;
                        }
                        div_EM.Visible = true;
                        break;

                    case BW://Building Work
                        LoadDiv_BuildingWork();
                        cb_BW_GRH.Checked = mdl.BW_GRHut == null ? false : (bool)mdl.BW_GRHut;
                        cb_BW_Ofc.Checked = mdl.BW_Office == null ? false : (bool)mdl.BW_Office;
                        cb_BW_Othrz.Checked = mdl.BW_Others == null ? false : (bool)mdl.BW_Others;
                        cb_BW_Res.Checked = mdl.BW_Residence == null ? false : (bool)mdl.BW_Residence;
                        cb_BW_RHs.Checked = mdl.BW_RestHouse == null ? false : (bool)mdl.BW_RestHouse;
                        break;

                    case OGP: //Oiling/Greasing/Painting
                        LoadDiv_OilingGreasingPainting();
                        cbGF.Checked = mdl.OP_GaugeFixing == null ? false : (bool) mdl.OP_GaugeFixing;
                        cbGP.Checked = mdl.OP_GaugePainting == null ? false : (bool) mdl.OP_GaugePainting;
                        cbGOGP.Checked = mdl.OP_OilGreasePaint == null ? false : (bool) mdl.OP_OilGreasePaint;
                        cbOthrz.Checked = mdl.OP_Others == null ? false : (bool) mdl.OP_Others;
                        ddlOGPSubDiv.Items.FindByValue("" + mdl.OP_SubDivID).Selected = true;
                        if (mdl.OP_SubDivID != null)
                        {
                            Dropdownlist.DDLLoading(ddlOGPSec, false, (int)Constants.DropDownFirstOption.Select,
                                    bllCO.GetSectionBySubDivID(Convert.ToInt64(mdl.OP_SubDivID)));
                            ddlOGPSec.Items.FindByValue(mdl.OP_SectionID == null ? "" : mdl.OP_SectionID + "").Selected = true; 
                        }
                    
                        break;

                    case OR: //outlet painting
                        LoadDiv_OutletRepainting();
                        ddlORSubDiv.Items.FindByValue(mdl.OR_SubDivID == null ? "" : mdl.OR_SubDivID +"").Selected = true;
                        if (mdl.OR_SubDivID != null)
                        {
                            long subDivID = Convert.ToInt64(ddlORSubDiv.SelectedItem.Value);
                            List<object> lstData = new List<object>();
                            lstData = bllCO.GetSectionBySubDivID(subDivID);
                            Dropdownlist.DDLLoading(ddlORSec, false, (int)Constants.DropDownFirstOption.Select, lstData);
                            ddlORSec.Items.FindByValue(mdl.OR_SectionID == null ? "" : mdl.OR_SectionID + "").Selected = true;

                            if (mdl.OR_SectionID != null)
                            {
                                lstData.Clear();
                                lstData = bllCO.GetChannelBySectionID(Convert.ToInt64(ddlORSec.SelectedItem.Value));
                                Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);
                                ddlORChannels.Items.FindByValue(mdl.OR_ChannelID == null ? "" : mdl.OR_ChannelID + "").Selected = true;
                            }
                            else
                            {
                                lstData.Clear();
                                lstData = bllCO.GetChannelBySubDivID(subDivID);
                                Dropdownlist.DDLLoading(ddlORChannels, false, (int)Constants.DropDownFirstOption.Select, lstData);
                                ddlORChannels.Items.FindByValue(mdl.OR_ChannelID == null ? "" : mdl.OR_ChannelID + "").Selected = true;
                            }
                        }
                        else
                            ddlORChannels.Items.FindByValue(mdl.OR_ChannelID == null ? "" : mdl.OR_ChannelID + "").Selected = true;
                        break;

                    case CSW: //Channel Structure Work 
                        LoadDiv_ChannelStructureWork();
                        ddlCSWChannels.Items.FindByValue("" + mdl.CS_ChannelID ).Selected = true;
                        break;

                    default:
                        LoadDiv_OtherWork();
                        ddlOtherSubDiv.Items.FindByValue("" + mdl.OW_SubDivID).Selected = true;

                        if (mdl.OW_SubDivID != null)
                        {
                            Dropdownlist.DDLLoading(ddlOtherSec, false, (int)Constants.DropDownFirstOption.Select,
                                bllCO.GetSectionBySubDivID(Convert.ToInt64(mdl.OW_SubDivID)));
                            ddlOtherSec.Items.FindByValue(mdl.OW_SectionID == null ? "" : mdl.OW_SectionID+"").Selected = true;
                        } 
                        break; 
                }
            }       
             catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            txtCost.Text = "" + mdl.EstimatedCost;
            cbClsurPrd.Checked = mdl.CompletionPeriodFlag == null ? false : (bool)mdl.CompletionPeriodFlag;
            txtCompPrd.Text = "" + mdl.CompletionPeriod;
            ddlCompPrdType.Items.FindByValue(mdl.CompletionPeriodUnit == null ? "Days" : mdl.CompletionPeriodUnit + "").Selected = true;
            
            //cbClsurPrd.Enabled = false;
           // txtCompPrd.Attributes.Add("disabled", "disabled");
            //ddlCompPrdType.Attributes.Add("disabled", "disabled");
            if (cbClsurPrd.Checked)
            {
                txtCompPrd.Attributes.Add("disabled", "disabled");
                ddlCompPrdType.Attributes.Add("disabled", "disabled");
            }

            if (mdl.StartDate != null)
                txtStartDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(mdl.StartDate));
            
            if (mdl.EndDate != null)
                txtEndDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(mdl.EndDate));
             
            txtSanctionNo.Text ="" + mdl.SanctionNo  ;
            if (mdl.SanctionDate != null)
                TxtSnctnDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(mdl.SanctionDate));

            txtErnsMny.Text = "" + mdl.EarnestMoney ;
            ddlErnsMnyType.Items.FindByValue(mdl.EarnestMoneyType).Selected = true;
            if (mdl.TenderFees != null)
               txtndrFee.Text = "" + mdl.TenderFees ;
            txtDesc.Text = mdl.Description;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private bool IsValidDate(DateTime _Date)
        {
            string fnYear = bllCO.GetFinancialYearbyCWP(Convert.ToInt64(hdnF_CWP_ID.Value));
            DateTime start =   Convert.ToDateTime("01-Oct-" + (fnYear.Substring(0,4)));
            DateTime end = Convert.ToDateTime("30-Apr-" + (fnYear.Substring(5,4)));

            if (_Date >= start && _Date <= end)
                return true;

            return false;
        }
    }
}