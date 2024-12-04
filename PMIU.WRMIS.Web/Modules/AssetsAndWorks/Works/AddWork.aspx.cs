using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Works
{
    public partial class AddWork : BasePage
    {
        #region Golobal  variables
        Dictionary<string, object> dd_Work = new Dictionary<string, object>();
        #endregion

        AssetsWorkBLL AWbll = new AssetsWorkBLL();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        List<object> ControlsValues = new List<object>();
        bool isViewMode = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    long viewMode = Utility.GetNumericValueFromQueryString("View", 0);
                    long editMode = Utility.GetNumericValueFromQueryString("Eidt", 0);
                    hdnAuctionedWorkID.Value = Convert.ToString(editMode == 0 ? viewMode : editMode);
                    CheckScreenMode(viewMode, editMode);
                    if (editMode > 0)
                    {
                        lblPageTitle.Text = "Edit Work";
                    }
                    if (viewMode > 0)
                    {
                        lblPageTitle.Text = "Work";
                    }
                    // IsControlsEnabled(isViewMode);
                    SetPageTitle();

                    //  BindUserLocation();
                    Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.NoOption);

                    LoadOtherDDL();
                    if (editMode > 0 || viewMode > 0)
                    {
                        SetControlValues(editMode == 0 ? viewMode : editMode);
                    }
                    else
                    {
                        BindsGrid();
                        BindsGridFlood();
                        BindsGridInfra();
                    }
                    IsControlsEnabled(isViewMode);
                    if (editMode != 0)
                    {
                        hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/SearchWork.aspx?CWID=" + Request.QueryString["Eidt"];
                    }
                    else
                    {
                        hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/SearchWork.aspx?CWID=" + Request.QueryString["View"];
                    }

                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void CheckScreenMode(long viewMode, long editMode)
        {
            if (viewMode != editMode)
            {
                if (viewMode > 0)
                {
                    isViewMode = false;
                }
                else
                {
                    isViewMode = true;
                }
            }
        }


        protected void IsControlsEnabled(bool isEnabled)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
          //  ddlFinancialYear.Enabled = isEnabled;
            ddlFinancialYear.Enabled = false;
            ddlFinancialYear.CssClass = ddlFinancialYear.CssClass.Replace("required", "");
            ddlDivision.Enabled = isEnabled;
            txtWorkName.Enabled = isEnabled;
            ddlWorkType.Enabled = isEnabled;
            ddlFundingSource.Enabled = isEnabled;
            txtCost.Enabled = isEnabled;
            txtCompletionPeriod.Enabled = isEnabled;
            ddlCompletionPeriodUnit.Enabled = isEnabled;
            txtStartDate.Enabled = isEnabled;
            txtEndDate.Enabled = isEnabled;
            txtSanctionNo.Enabled = isEnabled;
            txtSnctnDate.Enabled = isEnabled;
            ddlErnsMnyType.Enabled = isEnabled;
            txtErnsMny.Enabled = isEnabled;
            txtndrFee.Enabled = isEnabled;
            txtDesc.Enabled = isEnabled;
            btnAddWork.Visible = isEnabled;

            if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
            {
                btnAddWork.Visible = true;
                gvWork.Enabled = true;
                gvFlood.Enabled = true;
                gv_Infra.Enabled = true;
            }
            else
            {
                gvWork.Enabled = isEnabled;
                gvFlood.Enabled = isEnabled;
                gv_Infra.Enabled = isEnabled;
                if (gvWork.Enabled == false)
                {
                    gvWork.Columns[5].Visible = false;
                }
                if (gvFlood.Enabled == false)
                {
                    gvFlood.Columns[3].Visible = false;
                }
                if (gv_Infra.Enabled == false)
                {
                    gv_Infra.Columns[3].Visible = false;
                }
            }

        }

        public string GetPropertyValue(object obj, string PropertyName)
        {
            string PValue = obj.GetType().GetProperty(PropertyName).GetValue(obj).ToString();
            return PValue;
        }
        public void SetControlValues(long WorkID)
        {
            string startDate = "";
            string endDate = "";
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            object assetWork = AWbll.GetAssetWorkByID(WorkID);
            hdnWorkStatus.Value = Convert.ToString(assetWork.GetType().GetProperty("WorkStatus").GetValue(assetWork));

            if (Convert.ToString(assetWork.GetType().GetProperty("StartDate").GetValue(assetWork)) != "")
            {
                startDate = Convert.ToString(assetWork.GetType().GetProperty("StartDate").GetValue(assetWork));
            }
            if (Convert.ToString(assetWork.GetType().GetProperty("EndDate").GetValue(assetWork)) != "")
            {
                endDate = Convert.ToString(assetWork.GetType().GetProperty("EndDate").GetValue(assetWork));
            }
            string santnDate = GetPropertyValue(assetWork, "SanctionDate");
            //1st Sec
            long WID = Convert.ToInt64(GetPropertyValue(assetWork, "ID"));
            ddlFinancialYear.SelectedValue = GetPropertyValue(assetWork, "FinancialYear");
            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
            {
                Dropdownlist.DDLWorksDivision(ddlDivision, Convert.ToInt64(GetPropertyValue(assetWork, "DivisionID")), (int)Constants.DropDownFirstOption.NoOption);
            }
            ddlDivision.SelectedValue = GetPropertyValue(assetWork, "DivisionID");
            txtWorkName.Text = GetPropertyValue(assetWork, "WorkName");
            ddlFundingSource.SelectedValue = GetPropertyValue(assetWork, "FundingSourceID");
            ddlWorkType.SelectedValue = GetPropertyValue(assetWork, "AssetWorkTypeID");
            // ddlAssetCategory.SelectedItem.Text = GetPropertyValue(assetWork, "AssetType");
            if (Convert.ToString(assetWork.GetType().GetProperty("AssetType").GetValue(assetWork)) == "Asset")
            {
                ddlAssetCategory.SelectedValue = "1";
            }
            else if (Convert.ToString(assetWork.GetType().GetProperty("AssetType").GetValue(assetWork)) == "Flood")
            {
                ddlAssetCategory.SelectedValue = "2";
            }
            else
            {
                ddlAssetCategory.SelectedValue = "3";
            }
            ddlAssetCategory.Enabled = false;
            ddlAssetCategory.CssClass = ddlAssetCategory.CssClass.Replace("required", "");
            if (startDate != "")
                txtStartDate.Text = Convert.ToDateTime(startDate).ToString("dd-MMM-yyyy");
            if (endDate != "")
                txtEndDate.Text = Convert.ToDateTime(endDate).ToString("dd-MMM-yyyy");
            // 2nd Sec Estimation Cost
            txtCost.Text = GetPropertyValue(assetWork, "EstimatedCost");
            txtCompletionPeriod.Text = GetPropertyValue(assetWork, "CompletionPeriod");
            ddlCompletionPeriodUnit.Text = GetPropertyValue(assetWork, "CompletionPeriodUnit");
            txtWorkName.Text = GetPropertyValue(assetWork, "WorkName");
            ddlFundingSource.SelectedValue = GetPropertyValue(assetWork, "FundingSourceID");
            // 3rd Sec  Technical sec and Tendering
            txtSanctionNo.Text = GetPropertyValue(assetWork, "SanctionNo");
            txtSnctnDate.Text = Convert.ToDateTime(santnDate).ToString("dd-MMM-yyyy");
            txtErnsMny.Text = GetPropertyValue(assetWork, "EarnestMoney");
            txtndrFee.Text = GetPropertyValue(assetWork, "TenderFees");
            ddlErnsMnyType.SelectedValue = GetPropertyValue(assetWork, "EarnestMoneyType");
            txtDesc.Text = GetPropertyValue(assetWork, "Description");
            if (WID > 0)
            {
                if (ddlAssetCategory.SelectedValue == "1")
                {
                    PnlAssets.Visible = true;
                    PnlFlood.Visible = false;
                    PnlInfrastructure.Visible = false;
                    List<object> Lstdetail = AWbll.GetAssetWorkDetailByWorkID(WID);
                    ControlsValues = new List<object>();
                    foreach (object item in Lstdetail)
                    {
                        ControlsValues.Add(new { ID = GetPropertyValue(item, "ID"), CID = GetPropertyValue(item, "Category"), LID = GetPropertyValue(item, "Level"), AID = GetPropertyValue(item, "AssetName"), SID = GetPropertyValue(item, "SubCategory") });
                    }
                    CreateStructureTable(Lstdetail.Count);
                    GridStateAfterDelete();

                    for (int i = 0; i < Lstdetail.Count; i++)
                    {
                        Label lblID = (Label)gvWork.Rows[i].Cells[0].FindControl("lblID");
                        lblID.Text = GetPropertyValue(Lstdetail[i], "ID");

                    }
                }
                else if (ddlAssetCategory.SelectedValue == "2")
                {
                    PnlAssets.Visible = false;
                    PnlFlood.Visible = true;
                    PnlInfrastructure.Visible = false;
                    List<object> Lstdetail = AWbll.GetFloodWorkDetailByWorkID(null, WID);
                    ControlsValues = new List<object>();
                    foreach (object item in Lstdetail)
                    {
                        ControlsValues.Add(new { ID = GetPropertyValue(item, "ID"), TID = GetPropertyValue(item, "StructureType"), SID = GetPropertyValue(item, "StructureName"), Source = GetPropertyValue(item, "Source"), SName = GetPropertyValue(item, "StructureNameText") });
                    }
                    FloodCreateStructureTable(Lstdetail.Count);
                    EditFloodGridStateAfterDelete();

                    for (int i = 0; i < Lstdetail.Count; i++)
                    {
                        Label lblID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        lblID.Text = GetPropertyValue(Lstdetail[i], "ID");

                    }
                }
                else if (ddlAssetCategory.SelectedValue == "3")
                {
                    PnlAssets.Visible = false;
                    PnlFlood.Visible = false;
                    PnlInfrastructure.Visible = true;
                    List<object> Lstdetail = AWbll.GetInfraWorkDetailByWorkID(WID);
                    ControlsValues = new List<object>();
                    foreach (object item in Lstdetail)
                    {
                        ControlsValues.Add(new { ID = GetPropertyValue(item, "ID"), TID = GetPropertyValue(item, "StructureType"), SID = GetPropertyValue(item, "StructureName"), Source = GetPropertyValue(item, "Source"), SName = GetPropertyValue(item, "StructureNameText") });
                    }
                    InfraCreateStructureTable(Lstdetail.Count);
                    EditInfraGridStateAfterDelete();

                    for (int i = 0; i < Lstdetail.Count; i++)
                    {
                        Label lblID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        lblID.Text = GetPropertyValue(Lstdetail[i], "ID");

                    }
                }

            }
            hfWorkID.Value = Convert.ToString(WorkID);
        }
        public DataTable CreateStructureTable(int count)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < count; i++)
            {
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Column1" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column3" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column4" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column5" + i + "", typeof(string)));
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt;
            gvWork.DataSource = dt;
            gvWork.DataBind();
            return dt;
        }
        public DataTable FloodCreateStructureTable(int count)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < count; i++)
            {
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Column1" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column3" + i + "", typeof(string)));
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            ViewState["CurrentTableFlood"] = dt;
            gvFlood.DataSource = dt;
            gvFlood.DataBind();
            return dt;
        }
        public DataTable InfraCreateStructureTable(int count)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < count; i++)
            {
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Column1" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2" + i + "", typeof(string)));
                dt.Columns.Add(new DataColumn("Column3" + i + "", typeof(string)));
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            ViewState["CurrentTableInfra"] = dt;
            gv_Infra.DataSource = dt;
            gv_Infra.DataBind();
            return dt;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddAssetWork);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region DDL Operations
        private void LoadAllRegionDDByUser(long _UserID, long? _BoundryLevelID)
        {
            try
            {
                if (_BoundryLevelID == null)
                    return;
                int selectOption = (int)Constants.DropDownFirstOption.All;
                List<object> lstData = new WaterLossesBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_BoundryLevelID));
                List<object> lstChild = (List<object>)lstData.ElementAt(0);
                lstChild = (List<object>)lstData.ElementAt(1);
                if (lstChild.Count > 0) // Division
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    // Dropdownlist.DDLLoading(ddlDivision, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                    {
                        //    ddlDivision.SelectedIndex = 1;
                    }
                }

                lstChild = (List<object>)lstData.ElementAt(2);
                if (lstChild.Count > 0) // Circle
                {
                    //if (lstChild.Count == 1)
                    //    selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    ////Dropdownlist.DDLLoading(ddlCircle, false, selectOption, lstChild);
                    //if (lstChild.Count == 1)
                    //   ddlCircle.SelectedIndex = 1;
                }

                lstChild = (List<object>)lstData.ElementAt(3);
                if (lstChild.Count > 0) // Zone
                {
                    // if (lstChild.Count == 1)
                    //     selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    //// Dropdownlist.DDLLoading(ddlZone, false, selectOption, lstChild);
                    // if (lstChild.Count == 1)
                    //   //  ddlZone.SelectedIndex = 1;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void DisableDD(DropDownList DDL)
        {
            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("All", ""));
        }

        private void BindsGrid()
        {

            List<object> lst = new List<object>();

            gvWork.DataSource = lst;
            gvWork.DataBind();
        }
        private void BindsGridFlood()
        {

            List<object> lst = new List<object>();

            gvFlood.DataSource = lst;
            gvFlood.DataBind();
        }
        private void BindsGridInfra()
        {

            List<object> lst = new List<object>();

            gv_Infra.DataSource = lst;
            gv_Infra.DataBind();
        }
        protected void ddlInfrastructuresType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int index = row.RowIndex;

            DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfraType");
            DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfraStructure");

            if (ddlInfrastructuresType.SelectedItem.Value != "")
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
                }
                else if (InfrastructureTypeSelectedValue == 2)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);

                }
                else if (InfrastructureTypeSelectedValue == 3)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
                }
            }
            else
            {
                ddlInfrastructuresName.Items.Clear();
            }
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int index = row.RowIndex;

            DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlType");
            DropDownList ddlInfraStructure = (DropDownList)row.FindControl("ddlName");

            if (ddlInfrastructuresType.SelectedItem.Value != "")
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 6)
                {
                    Dropdownlist.DDLAssetWorkChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));

                }
                else if (InfrastructureTypeSelectedValue == 7)
                {

                    Dropdownlist.DDLAssetWorkChannelOutlet(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));

                }
                else if (InfrastructureTypeSelectedValue == 9)
                {
                    Dropdownlist.DDLAssetWorkSmallDams(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                }
                else if (InfrastructureTypeSelectedValue == 10)
                {
                    Dropdownlist.DDLAssetWorkSmallDamsChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                }
                else if (InfrastructureTypeSelectedValue == 1)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, _Users.ID, 1);
                }
                else if (InfrastructureTypeSelectedValue == 2)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, _Users.ID, 2);

                }
                else if (InfrastructureTypeSelectedValue == 3)
                {
                    Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, _Users.ID, 3);
                }
            }
            else
            {
                ddlInfraStructure.Items.Clear();
            }
        }

        #endregion
        #region GridView Methods


        protected void gvWork_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddWorkAssetAssociate")
                {

                    if (gvWork.Rows.Count == 0)
                    {
                        SetfirstRow();
                    }
                    else
                    {

                        AddNewRowToGrid();
                    }


                }
                else if (e.CommandName == "Delete")
                {
                    //   LinkButton lb = (LinkButton)sender;
                    //   GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
                    GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    object gv = gvWork.DataSource;
                    int rowID = gvRow.RowIndex;
                    if (ViewState["CurrentTable"] != null)
                    {

                        DataTable dt = (DataTable)ViewState["CurrentTable"];
                        if (dt.Rows.Count >= 1)
                        {
                            if (gvRow.RowIndex <= dt.Rows.Count - 1)
                            {
                                dt.Rows.Remove(dt.Rows[rowID]);

                            }
                        }
                        ViewState["CurrentTable"] = dt;
                        if (ViewState["CurrentTable"] != null)
                        {
                            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                            if (dtCurrentTable.Rows.Count > 0)
                            {
                                int rowIndex = 0;
                                for (int i = 0; i < gvWork.Rows.Count; i++)
                                {
                                    if (rowID != i)
                                    {
                                        Label lblID = (Label)gvWork.Rows[rowIndex].Cells[0].FindControl("lblID");
                                        DropDownList ddlCategory = (DropDownList)gvWork.Rows[rowIndex].Cells[1].FindControl("ddlCategory");
                                        DropDownList ddlLevel = (DropDownList)gvWork.Rows[rowIndex].Cells[3].FindControl("ddlLevel");
                                        DropDownList ddlAssetName = (DropDownList)gvWork.Rows[rowIndex].Cells[4].FindControl("ddlAssetName");
                                        DropDownList ddlSubCategory = (DropDownList)gvWork.Rows[rowIndex].Cells[2].FindControl("ddlSubCategory");
                                        long lbID = lblID.Text == string.Empty ? 0 : Convert.ToInt64(lblID.Text);
                                        ControlsValues.Add(new { ID = lbID, CID = ddlCategory.SelectedValue, LID = ddlLevel.SelectedValue, AID = ddlAssetName.SelectedValue, SID = ddlSubCategory.SelectedValue });
                                    }
                                    rowIndex++;
                                }
                            }
                        }
                        gvWork.DataSource = dt;
                        gvWork.DataBind();
                    }
                    GridStateAfterDelete();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWork_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvFlood_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddFlodAssociate")
                {

                    if (gvFlood.Rows.Count == 0)
                    {
                        FloodSetfirstRow();
                    }
                    else
                    {

                        FloodAddNewRowToGrid();
                    }


                }
                else if (e.CommandName == "DeleteFlood")
                {
                    GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    object gv = gvFlood.DataSource;
                    int rowID = gvRow.RowIndex;
                    if (ViewState["CurrentTableFlood"] != null)
                    {

                        DataTable dt = (DataTable)ViewState["CurrentTableFlood"];
                        if (dt.Rows.Count >= 1)
                        {
                            if (gvRow.RowIndex <= dt.Rows.Count - 1)
                            {
                                dt.Rows.Remove(dt.Rows[rowID]);

                            }
                        }
                        ViewState["CurrentTableFlood"] = dt;
                        if (ViewState["CurrentTableFlood"] != null)
                        {
                            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTableFlood"];
                            if (dtCurrentTable.Rows.Count > 0)
                            {
                                int rowIndex = 0;
                                for (int i = 0; i < gvFlood.Rows.Count; i++)
                                {
                                    if (rowID != i)
                                    {
                                        Label lblID = (Label)gvFlood.Rows[rowIndex].Cells[0].FindControl("lblIDF");
                                        DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[rowIndex].Cells[1].FindControl("ddlInfraType");
                                        DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[rowIndex].Cells[2].FindControl("ddlInfraStructure");
                                        long lbID = lblID.Text == string.Empty ? 0 : Convert.ToInt64(lblID.Text);
                                        ControlsValues.Add(new { ID = lbID, TID = ddlInfraType.SelectedValue, SID = ddlInfraStructure.SelectedValue });
                                    }
                                    rowIndex++;
                                }
                            }
                        }
                        gvFlood.DataSource = dt;
                        gvFlood.DataBind();
                    }
                    FloodGridStateAfterDelete();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_Infra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddInfra")
                {

                    if (gv_Infra.Rows.Count == 0)
                    {
                        InfraSetfirstRow();
                    }
                    else
                    {

                        InfraAddNewRowToGrid();
                    }


                }
                else if (e.CommandName == "DeleteInfra")
                {
                    GridViewRow gvRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                    object gv = gv_Infra.DataSource;
                    int rowID = gvRow.RowIndex;
                    if (ViewState["CurrentTableInfra"] != null)
                    {

                        DataTable dt = (DataTable)ViewState["CurrentTableInfra"];
                        if (dt.Rows.Count >= 1)
                        {
                            if (gvRow.RowIndex <= dt.Rows.Count - 1)
                            {
                                dt.Rows.Remove(dt.Rows[rowID]);

                            }
                        }
                        ViewState["CurrentTableInfra"] = dt;
                        if (ViewState["CurrentTableInfra"] != null)
                        {
                            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTableInfra"];
                            if (dtCurrentTable.Rows.Count > 0)
                            {
                                int rowIndex = 0;
                                for (int i = 0; i < gv_Infra.Rows.Count; i++)
                                {
                                    if (rowID != i)
                                    {
                                        Label lblID = (Label)gv_Infra.Rows[rowIndex].Cells[0].FindControl("lblIDInfra");
                                        DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[rowIndex].Cells[1].FindControl("ddlType");
                                        DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[rowIndex].Cells[2].FindControl("ddlName");
                                        long lbID = lblID.Text == string.Empty ? 0 : Convert.ToInt64(lblID.Text);
                                        ControlsValues.Add(new { ID = lbID, TID = ddlInfraType.SelectedValue, SID = ddlInfraStructure.SelectedValue });
                                    }
                                    rowIndex++;
                                }
                            }
                        }
                        gv_Infra.DataSource = dt;
                        gv_Infra.DataBind();
                    }
                    InfraGridStateAfterDelete();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
        #region InfraConditionRowAdd
        private void InfraSetfirstRow()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            ViewState["CurrentTableInfra"] = dt;
            gv_Infra.DataSource = dt;
            gv_Infra.DataBind();
            DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[0].Cells[1].FindControl("ddlType");
            DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[0].Cells[2].FindControl("ddlName");

            Dropdownlist.DDLAssetWorkInfrastructureType(ddlInfraType);
            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
            if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
            {
                ddlInfraType.CssClass = "required form-control";
                ddlInfraStructure.CssClass = "required form-control";
            }

        }
        private void InfraAddNewRowToGrid()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (ViewState["CurrentTableInfra"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTableInfra"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        Label ID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[i].Cells[1].FindControl("ddlType");
                        DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[i].Cells[2].FindControl("ddlName");
                        ControlsValues.Add(new { ID = Convert.ToInt64(ID.Text == string.Empty ? "0" : ID.Text), TID = ddlInfraType.SelectedValue, SID = ddlInfraStructure.SelectedValue });
                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }
                    }
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTableInfra"] = dtCurrentTable;
                    gv_Infra.DataSource = dtCurrentTable;
                    gv_Infra.DataBind();

                }
            }

            InfraSetPreviousRow();
        }
        private void InfraSetPreviousRow()
        {
            if (ViewState["CurrentTableInfra"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataTable dt = (DataTable)ViewState["CurrentTableInfra"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        Label lblID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[i].Cells[0].FindControl("ddlType");
                        DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[i].Cells[1].FindControl("ddlName");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        Dropdownlist.DDLAssetWorkInfrastructureType(ddlInfraType);

                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraType.SelectedValue = ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString();

                        if (ddlInfraType.SelectedValue == "6")
                            Dropdownlist.DDLAssetWorkChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                        else if (ddlInfraType.SelectedValue == "7")
                            Dropdownlist.DDLAssetWorkChannelOutlet(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                        //smalldams
                        else if (ddlInfraType.SelectedValue == "9")
                            Dropdownlist.DDLAssetWorkSmallDams(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                       else if (ddlInfraType.SelectedValue == "10")
                            Dropdownlist.DDLAssetWorkSmallDamsChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                            //protection,Barrage,Drain
                        else
                            Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, Convert.ToInt64(ddlInfraType.SelectedValue));

                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();
                        if (i == dt.Rows.Count - 2)
                        {
                            DropDownList ddlI = (DropDownList)gv_Infra.Rows[i + 1].Cells[0].FindControl("ddlType");
                            DropDownList ddlS = (DropDownList)gv_Infra.Rows[i + 1].Cells[1].FindControl("ddlName");


                            Dropdownlist.DDLAssetWorkInfrastructureType(ddlI);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlS, null);
                        }
                    }

                }
            }
        }


        private void InfraGridStateAfterDelete()
        {
            if (ViewState["CurrentTableInfra"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                //bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                DataTable dt = (DataTable)ViewState["CurrentTableInfra"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[i].Cells[1].FindControl("ddlType");
                        DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[i].Cells[2].FindControl("ddlName");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        Dropdownlist.DDLAssetWorkInfrastructureType(ddlInfraType);

                        if (!string.IsNullOrEmpty(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString()))
                        {
                            int InfraTypeID = Convert.ToInt32(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]));
                            long InfraStructureID = Convert.ToInt64(ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]));

                            if (InfraTypeID == 6)
                                Dropdownlist.DDLAssetWorkChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                            else if (InfraTypeID == 7)
                                Dropdownlist.DDLAssetWorkChannelOutlet(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                            //smalldams
                            else  if (InfraTypeID == 9)
                                Dropdownlist.DDLAssetWorkSmallDams(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                            else  if (InfraTypeID == 10)
                                Dropdownlist.DDLAssetWorkSmallDamsChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                            else
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, Convert.ToInt64(InfraTypeID));
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
                        }
                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraType.SelectedValue = ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();

                    }
                }

            }
        }
        private void EditInfraGridStateAfterDelete()
        {
            if (ViewState["CurrentTableInfra"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                //bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                DataTable dt = (DataTable)ViewState["CurrentTableInfra"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        DropDownList ddlInfraType = (DropDownList)gv_Infra.Rows[i].Cells[1].FindControl("ddlType");
                        DropDownList ddlInfraStructure = (DropDownList)gv_Infra.Rows[i].Cells[2].FindControl("ddlName");
                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        Dropdownlist.DDLAssetWorkInfrastructureType(ddlInfraType);
                        if (!string.IsNullOrEmpty(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString()))
                        {
                            if (Convert.ToString(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])) == "6")
                            {
                                Dropdownlist.SetSelectedValue(ddlInfraType, "6");
                                Dropdownlist.DDLAssetWorkChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])) == "7")
                            {
                                Dropdownlist.SetSelectedValue(ddlInfraType, "7");
                                Dropdownlist.DDLAssetWorkChannelOutlet(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])) == "9")
                            {
                                Dropdownlist.SetSelectedValue(ddlInfraType, "9");
                                Dropdownlist.DDLAssetWorkSmallDams(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])) == "10")
                            {
                                Dropdownlist.SetSelectedValue(ddlInfraType, "10");
                                Dropdownlist.DDLAssetWorkSmallDamsChannel(ddlInfraStructure, Convert.ToInt64(ddlDivision.SelectedValue));
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Protection Infrastructure")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Protection Infrastructure");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 1);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Control Structure1")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Barrage/Headwork");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 2);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Drain")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Drain");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 3);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }

                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
                        }
                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();

                    }
                }

            }
        }
        #endregion InfraConditionRowAdd

        #region FloodConditionRowAdd
        private void FloodSetfirstRow()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            ViewState["CurrentTableFlood"] = dt;
            gvFlood.DataSource = dt;
            gvFlood.DataBind();
            DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[0].Cells[1].FindControl("ddlInfraType");
            DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[0].Cells[2].FindControl("ddlInfraStructure");

            Dropdownlist.DDLInfrastructureType(ddlInfraType);
            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
            if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
            {
                ddlInfraType.CssClass = "required form-control";
                ddlInfraStructure.CssClass = "required form-control";
            }

        }
        private void FloodAddNewRowToGrid()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (ViewState["CurrentTableFlood"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTableFlood"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        Label ID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[i].Cells[1].FindControl("ddlInfraType");
                        DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[i].Cells[2].FindControl("ddlInfraStructure");
                        ControlsValues.Add(new { ID = Convert.ToInt64(ID.Text == string.Empty ? "0" : ID.Text), TID = ddlInfraType.SelectedValue, SID = ddlInfraStructure.SelectedValue });
                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }
                    }
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTableFlood"] = dtCurrentTable;
                    gvFlood.DataSource = dtCurrentTable;
                    gvFlood.DataBind();

                }
            }

            FloodSetPreviousRow();
        }
        private void FloodSetPreviousRow()
        {
            if (ViewState["CurrentTableFlood"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataTable dt = (DataTable)ViewState["CurrentTableFlood"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        Label lblID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[i].Cells[0].FindControl("ddlInfraType");
                        DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[i].Cells[1].FindControl("ddlInfraStructure");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        Dropdownlist.DDLInfrastructureType(ddlInfraType);
                        // Dropdownlist.BindDropdownlist<List<object>>(ddlInfraType, CommonLists.AssetsCategory(), (int)Constants.DropDownFirstOption.Select);
                        //required to bind later
                        // Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, AWbll.GetAssetNameBySubCategoryAndLevelID(Convert.ToInt64(ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i])), Convert.ToInt64(ControlsValues[i].GetType().GetProperty("LID").GetValue(ControlsValues[i]))));
                        Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, Convert.ToInt64(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])));



                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraType.SelectedValue = ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();
                        if (i == dt.Rows.Count - 2)
                        {
                            DropDownList ddlI = (DropDownList)gvFlood.Rows[i + 1].Cells[0].FindControl("ddlInfraType");
                            DropDownList ddlS = (DropDownList)gvFlood.Rows[i + 1].Cells[1].FindControl("ddlInfraStructure");

                            //   Dropdownlist.DDLInfrastructureType(ddlI);
                            // Dropdownlist.BindDropdownlist<List<object>>(ddlI, CommonLists.AssetsCategory(), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.DDLInfrastructureType(ddlI);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlS, null);
                        }
                    }

                }
            }
        }


        private void FloodGridStateAfterDelete()
        {
            if (ViewState["CurrentTableFlood"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                DataTable dt = (DataTable)ViewState["CurrentTableFlood"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[i].Cells[1].FindControl("ddlInfraType");
                        DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[i].Cells[2].FindControl("ddlInfraStructure");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        // Dropdownlist.BindDropdownlist<List<object>>(ddlInfraType, CommonLists.AssetsCategory(), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.DDLInfrastructureType(ddlInfraType);

                        if (!string.IsNullOrEmpty(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString()))
                        {
                            int InfraTypeID = Convert.ToInt32(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]));
                            long InfraStructureID = Convert.ToInt64(ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]));
                            //required to change later
                            // Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, AWbll.GetAssetNameBySubCategoryAndLevelID(InfraTypeID, InfraTypeID), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, Convert.ToInt64(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i])));
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
                        }
                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraType.SelectedValue = ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();

                    }
                }

            }
        }
        private void EditFloodGridStateAfterDelete()
        {
            if (ViewState["CurrentTableFlood"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                DataTable dt = (DataTable)ViewState["CurrentTableFlood"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        DropDownList ddlInfraType = (DropDownList)gvFlood.Rows[i].Cells[1].FindControl("ddlInfraType");
                        DropDownList ddlInfraStructure = (DropDownList)gvFlood.Rows[i].Cells[2].FindControl("ddlInfraStructure");
                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlInfraType.CssClass = "required form-control";
                            ddlInfraStructure.CssClass = "required form-control";
                        }

                        Dropdownlist.DDLInfrastructureType(ddlInfraType);
                        if (!string.IsNullOrEmpty(ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString()))
                        {
                            if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Protection Infrastructure")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Protection Infrastructure");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 1);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Control Structure1")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Barrage/Headwork");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 2);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                            else if (Convert.ToString(ControlsValues[i].GetType().GetProperty("Source").GetValue(ControlsValues[i])) == "Drain")
                            {
                                Dropdownlist.SetSelectedText(ddlInfraType, "Drain");
                                Dropdownlist.DDLAssetWorksInfrastructureNameByType(ddlInfraStructure, mdlUser.ID, 3);
                                Dropdownlist.SetSelectedText(ddlInfraStructure, Convert.ToString(ControlsValues[i].GetType().GetProperty("SName").GetValue(ControlsValues[i])));
                            }
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlInfraStructure, null, (int)Constants.DropDownFirstOption.Select);
                        }
                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        // ddlInfraType.SelectedValue = ControlsValues[i].GetType().GetProperty("TID").GetValue(ControlsValues[i]).ToString();
                        ddlInfraStructure.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();

                    }
                }

            }
        }
        #endregion FloodConditionRowAdd


        #region AssetConditionRowAdd
        private void SetfirstRow()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dt.Columns.Add(new DataColumn("Column4", typeof(string)));
            dt.Columns.Add(new DataColumn("Column5", typeof(string)));
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            gvWork.DataSource = dt;
            gvWork.DataBind();
            DropDownList ddlCategory = (DropDownList)gvWork.Rows[0].Cells[1].FindControl("ddlCategory");
            DropDownList ddlSubCategory = (DropDownList)gvWork.Rows[0].Cells[2].FindControl("ddlSubCategory");
            DropDownList ddlLevel = (DropDownList)gvWork.Rows[0].Cells[3].FindControl("ddlLevel");
            DropDownList ddlAssetName = (DropDownList)gvWork.Rows[0].Cells[4].FindControl("ddlAssetName");

            Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, AWbll.GetAssetCategoryList(), (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, null, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, null, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.BindDropdownlist<List<object>>(ddlLevel, AWbll.GetIrrigationLevel(Convert.ToInt64(mdlUser.ID), IsHeadQuarterDivision), (int)Constants.DropDownFirstOption.Select);
            if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
            {
                ddlCategory.CssClass = "required form-control";
                ddlSubCategory.CssClass = "required form-control";
                ddlLevel.CssClass = "required form-control";
                ddlAssetName.CssClass = "required form-control";
            }


        }
        private void AddNewRowToGrid()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        Label ID = (Label)gvWork.Rows[i].Cells[0].FindControl("lblID");
                        DropDownList ddlCategory = (DropDownList)gvWork.Rows[i].Cells[1].FindControl("ddlCategory");
                        DropDownList ddlLevel = (DropDownList)gvWork.Rows[i].Cells[3].FindControl("ddlLevel");
                        DropDownList ddlAssetName = (DropDownList)gvWork.Rows[i].Cells[4].FindControl("ddlAssetName");
                        DropDownList ddlSubCategory = (DropDownList)gvWork.Rows[i].Cells[2].FindControl("ddlSubCategory");
                        ControlsValues.Add(new { ID = Convert.ToInt64(ID.Text == string.Empty ? "0" : ID.Text), CID = ddlCategory.SelectedValue, LID = ddlLevel.SelectedValue, AID = ddlAssetName.SelectedValue, SID = ddlSubCategory.SelectedValue });
                        //Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, AWbll.GetAssetCategoryList(), (int)Constants.DropDownFirstOption.Select);
                        //Dropdownlist.BindDropdownlist<List<object>>(ddlLevel, AWbll.GetIrrigationLevel(), (int)Constants.DropDownFirstOption.Select);
                        //Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, AWbll.GetSubCategoriesByCategoryID(Convert.ToInt32(ddlCategory.SelectedItem.Value)));
                        //Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, AWbll.GetAssetNameBySubCategoryAndLevelID(Convert.ToInt32(ddlSubCategory.SelectedItem.Value), Convert.ToInt64(ddlLevel.SelectedItem.Value)));
                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlCategory.CssClass = "required form-control";
                            ddlSubCategory.CssClass = "required form-control";
                            ddlLevel.CssClass = "required form-control";
                            ddlAssetName.CssClass = "required form-control";
                        }
                    }
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gvWork.DataSource = dtCurrentTable;
                    gvWork.DataBind();

                }
            }

            SetPreviousRow();
        }
        private void SetPreviousRow()
        {
            if (ViewState["CurrentTable"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);

                DataTable dt = (DataTable)ViewState["CurrentTable"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        Label lblID = (Label)gvWork.Rows[i].Cells[0].FindControl("lblID");
                        DropDownList ddlCategory = (DropDownList)gvWork.Rows[i].Cells[0].FindControl("ddlCategory");
                        DropDownList ddlSubCategory = (DropDownList)gvWork.Rows[i].Cells[1].FindControl("ddlSubCategory");
                        DropDownList ddlLevel = (DropDownList)gvWork.Rows[i].Cells[2].FindControl("ddlLevel");
                        DropDownList ddlAssetName = (DropDownList)gvWork.Rows[i].Cells[3].FindControl("ddlAssetName");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlCategory.CssClass = "required form-control";
                            ddlSubCategory.CssClass = "required form-control";
                            ddlLevel.CssClass = "required form-control";
                            ddlAssetName.CssClass = "required form-control";
                        }

                        Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, AWbll.GetAssetCategoryList(), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.BindDropdownlist<List<object>>(ddlLevel, AWbll.GetIrrigationLevel(Convert.ToInt64(mdlUser.ID), IsHeadQuarterDivision), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, AWbll.GetSubCategoriesByCategoryID(Convert.ToInt32(ControlsValues[i].GetType().GetProperty("CID").GetValue(ControlsValues[i]))));
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, AWbll.GetAssetNameBySubCategoryAndLevelID(Convert.ToInt64(ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i])), Convert.ToInt64(ControlsValues[i].GetType().GetProperty("LID").GetValue(ControlsValues[i])), Convert.ToInt64(hdnAuctionedWorkID.Value)));

                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlCategory.SelectedValue = ControlsValues[i].GetType().GetProperty("CID").GetValue(ControlsValues[i]).ToString();
                        ddlSubCategory.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();
                        ddlLevel.SelectedValue = ControlsValues[i].GetType().GetProperty("LID").GetValue(ControlsValues[i]).ToString();
                        ddlAssetName.SelectedValue = ControlsValues[i].GetType().GetProperty("AID").GetValue(ControlsValues[i]).ToString();
                        if (i == dt.Rows.Count - 2)
                        {
                            DropDownList ddlC = (DropDownList)gvWork.Rows[i + 1].Cells[0].FindControl("ddlCategory");
                            DropDownList ddlS = (DropDownList)gvWork.Rows[i + 1].Cells[1].FindControl("ddlSubCategory");
                            DropDownList ddlL = (DropDownList)gvWork.Rows[i + 1].Cells[2].FindControl("ddlLevel");
                            DropDownList ddlA = (DropDownList)gvWork.Rows[i + 1].Cells[3].FindControl("ddlAssetName");


                            Dropdownlist.BindDropdownlist<List<object>>(ddlC, AWbll.GetAssetCategoryList(), (int)Constants.DropDownFirstOption.Select);

                            Dropdownlist.BindDropdownlist<List<object>>(ddlL, AWbll.GetIrrigationLevel(Convert.ToInt64(mdlUser.ID), IsHeadQuarterDivision), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlS, null);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlA, null);
                        }
                    }

                }
            }
        }


        private void GridStateAfterDelete()
        {
            if (ViewState["CurrentTable"] != null)
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                bool IsHeadQuarterDivision = new AssetsWorkBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                DataTable dt = (DataTable)ViewState["CurrentTable"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Label lblID = (Label)gvWork.Rows[i].Cells[0].FindControl("lblID");
                        DropDownList ddlCategory = (DropDownList)gvWork.Rows[i].Cells[1].FindControl("ddlCategory");
                        DropDownList ddlSubCategory = (DropDownList)gvWork.Rows[i].Cells[2].FindControl("ddlSubCategory");
                        DropDownList ddlLevel = (DropDownList)gvWork.Rows[i].Cells[3].FindControl("ddlLevel");
                        DropDownList ddlAssetName = (DropDownList)gvWork.Rows[i].Cells[4].FindControl("ddlAssetName");

                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN && hdnWorkStatus.Value.ToUpper() == "CONTRACT AWARDED")
                        {
                            ddlCategory.CssClass = "required form-control";
                            ddlSubCategory.CssClass = "required form-control";
                            ddlLevel.CssClass = "required form-control";
                            ddlAssetName.CssClass = "required form-control";
                        }

                        Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, AWbll.GetAssetCategoryList(), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.BindDropdownlist<List<object>>(ddlLevel, AWbll.GetIrrigationLevel(Convert.ToInt64(mdlUser.ID), IsHeadQuarterDivision), (int)Constants.DropDownFirstOption.Select);
                        if (!string.IsNullOrEmpty(ControlsValues[i].GetType().GetProperty("CID").GetValue(ControlsValues[i]).ToString()))
                        {
                            int CategoryID = Convert.ToInt32(ControlsValues[i].GetType().GetProperty("CID").GetValue(ControlsValues[i]));
                            long SubCategoryID = Convert.ToInt64(ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]));
                            long LevelID = Convert.ToInt64(ControlsValues[i].GetType().GetProperty("LID").GetValue(ControlsValues[i]));
                            int AssetID = Convert.ToInt32(ControlsValues[i].GetType().GetProperty("AID").GetValue(ControlsValues[i]));

                            Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, AWbll.GetSubCategoriesByCategoryID(CategoryID), (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, AWbll.GetAssetNameBySubCategoryAndLevelID(SubCategoryID, LevelID, Convert.ToInt64(hdnAuctionedWorkID.Value)), (int)Constants.DropDownFirstOption.Select);
                        }
                        else
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, null, (int)Constants.DropDownFirstOption.Select);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, null, (int)Constants.DropDownFirstOption.Select);
                        }
                        lblID.Text = ControlsValues[i].GetType().GetProperty("ID").GetValue(ControlsValues[i]).ToString();
                        ddlCategory.SelectedValue = ControlsValues[i].GetType().GetProperty("CID").GetValue(ControlsValues[i]).ToString();
                        ddlSubCategory.SelectedValue = ControlsValues[i].GetType().GetProperty("SID").GetValue(ControlsValues[i]).ToString();
                        ddlLevel.SelectedValue = ControlsValues[i].GetType().GetProperty("LID").GetValue(ControlsValues[i]).ToString();
                        ddlAssetName.SelectedValue = ControlsValues[i].GetType().GetProperty("AID").GetValue(ControlsValues[i]).ToString();

                        //if (lblPageTitle.Text == "Work")
                        //{
                        //    ddlCategory.CssClass = ddlCategory.CssClass.Replace("required", "");
                        //    ddlSubCategory.CssClass = ddlSubCategory.CssClass.Replace("required", "");
                        //    ddlLevel.CssClass = ddlLevel.CssClass.Replace("required", "");
                        //    ddlAssetName.CssClass = ddlAssetName.CssClass.Replace("required", "");

                        //}
                    }
                }

            }
        }
        #endregion AssetConditionRowAdd
        private long? GetValue(String _StrValue)
        {
            if (string.IsNullOrEmpty(_StrValue))
                return null;

            return Convert.ToInt64(_StrValue);
        }

        public List<object> SaveAssociationWithAssets(bool isUpdate = false)
        {
            List<object> lstAssetWorkDetail = new List<object>();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            Session["FloodDuplicate"] = "false";
            Session["AssetDuplicate"] = "false";
            Session["InfraDuplicate"] = "false";
            //ControlsValues = Session["CtrlVal"] as List<object>;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DropDownList ddlAssetName = (DropDownList)gvWork.Rows[i].Cells[4].FindControl("ddlAssetName");
                        Label lblID = (Label)gvWork.Rows[i].Cells[0].FindControl("lblID");
                        long AssetItemID = Convert.ToInt64(ddlAssetName.SelectedItem.Value == string.Empty ? "0" : ddlAssetName.SelectedItem.Value);

                        object obj = (from item in lstAssetWorkDetail
                                      where (Convert.ToInt64(item.GetType().GetProperty("AssetItemsID").GetValue(item)) == AssetItemID)
                                      select item.GetType().GetProperty("AssetItemsID").GetValue(item)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["AssetDuplicate"] = "true";

                        }

                        if (isUpdate && !string.IsNullOrEmpty(lblID.Text) && AssetItemID > 0)
                        {
                            long WDetialID = Convert.ToInt64(lblID.Text);
                            lstAssetWorkDetail.Add(new AM_AssetWorkDetail { ID = WDetialID, AssetItemsID = AssetItemID });
                        }
                        else if (AssetItemID > 0)
                        {
                            lstAssetWorkDetail.Add(new AM_AssetWorkDetail { AssetItemsID = AssetItemID });
                        }

                    }
                }
            }
            return lstAssetWorkDetail;
        }
        public List<object> SaveAssociationWithFlood(bool isUpdate = false)
        {
            object bllGetStructureTypeIDByvalue = null;
            Session["FloodDuplicate"] = "false";
            Session["AssetDuplicate"] = "false";
            Session["InfraDuplicate"] = "false";
            List<object> lstFloodWorkDetail = new List<object>();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            //ControlsValues = Session["CtrlVal"] as List<object>;
            if (ViewState["CurrentTableFlood"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTableFlood"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //ddlInfraType
                        long InfrastructureTypeID = 0;
                        DropDownList ddlInfrastructuresType = (DropDownList)gvFlood.Rows[i].Cells[0].FindControl("ddlInfraType");
                        DropDownList ddlInfrastructuresName = (DropDownList)gvFlood.Rows[i].Cells[1].FindControl("ddlInfraStructure");
                        Label lblID = (Label)gvFlood.Rows[i].Cells[0].FindControl("lblIDF");
                        long InfraStructureID = Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value == string.Empty ? "0" : ddlInfrastructuresName.SelectedItem.Value);

                        /////////////////////////////////////////////////////////////////////////////////////////////
                        if (Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value) == 1)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        else if (Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value) == 2)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        else if (Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value) == 3)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        object obj = (from item in lstFloodWorkDetail
                                      where (Convert.ToInt64(item.GetType().GetProperty("StructureTypeID").GetValue(item)) == InfrastructureTypeID &&
                                          Convert.ToInt64(item.GetType().GetProperty("StructureID").GetValue(item)) == InfraStructureID)
                                      select item.GetType().GetProperty("StructureTypeID").GetValue(item)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["FloodDuplicate"] = "true";

                        }

                        if (isUpdate && !string.IsNullOrEmpty(lblID.Text) && InfraStructureID > 0)
                        {
                            long WDetialID = Convert.ToInt64(lblID.Text);
                            lstFloodWorkDetail.Add(new AM_AssetWorkDetail { ID = WDetialID, StructureTypeID = InfrastructureTypeID, StructureID = InfraStructureID });
                        }
                        else if (InfraStructureID > 0)
                        {
                            lstFloodWorkDetail.Add(new AM_AssetWorkDetail { StructureTypeID = InfrastructureTypeID, StructureID = InfraStructureID });
                        }

                    }
                }
            }
            return lstFloodWorkDetail;
        }

        public List<object> SaveAssociationWithInfra(bool isUpdate = false)
        {
            object bllGetStructureTypeIDByvalue = null;
            Session["FloodDuplicate"] = "false";
            Session["AssetDuplicate"] = "false";
            Session["InfraDuplicate"] = "false";
            List<object> lstInfraWorkDetail = new List<object>();
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (ViewState["CurrentTableInfra"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTableInfra"];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        DropDownList ddlInfrastructuresType = (DropDownList)gv_Infra.Rows[i].Cells[0].FindControl("ddlType");
                        DropDownList ddlInfrastructuresName = (DropDownList)gv_Infra.Rows[i].Cells[1].FindControl("ddlName");
                        Label lblID = (Label)gv_Infra.Rows[i].Cells[0].FindControl("lblIDInfra");
                        long InfrastructureTypeID = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                        long InfraStructureID = Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value == string.Empty ? "0" : ddlInfrastructuresName.SelectedItem.Value);
                        /////////////////////////////////////////Protection,Barrage,Drain////////////////////////////////////////////////////
                        if (InfrastructureTypeID == 1)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        else if (InfrastructureTypeID == 2)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        else if (InfrastructureTypeID == 3)
                        {
                            bllGetStructureTypeIDByvalue = new AssetsWorkBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt64(ddlInfrastructuresName.SelectedItem.Value));
                            InfrastructureTypeID = Convert.ToInt64(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));

                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////

                        object obj = (from item in lstInfraWorkDetail
                                      where (Convert.ToInt64(item.GetType().GetProperty("StructureTypeID").GetValue(item)) == InfrastructureTypeID &&
                                          Convert.ToInt64(item.GetType().GetProperty("StructureID").GetValue(item)) == InfraStructureID)
                                      select item.GetType().GetProperty("StructureTypeID").GetValue(item)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["InfraDuplicate"] = "true";

                        }

                        if (isUpdate && !string.IsNullOrEmpty(lblID.Text) && InfraStructureID > 0)
                        {
                            long WDetialID = Convert.ToInt64(lblID.Text);
                            lstInfraWorkDetail.Add(new AM_AssetWorkDetail { ID = WDetialID, StructureTypeID = InfrastructureTypeID, StructureID = InfraStructureID });
                        }
                        else if (InfraStructureID > 0)
                        {
                            lstInfraWorkDetail.Add(new AM_AssetWorkDetail { StructureTypeID = InfrastructureTypeID, StructureID = InfraStructureID });
                        }

                    }
                }
            }
            return lstInfraWorkDetail;
        }
        protected void btnAddCWP_Click(object sender, EventArgs e)
        {

            Response.Redirect("AddClosureWorkPlan.aspx", false);
        }


        public bool IsDateLieFinancialYear(DateTime date)
        {
            DateTime.Today.AddYears(-1);
            DateTime StartFinancilaYear = new DateTime(DateTime.Today.AddYears(-1).Year, 7, 1);
            DateTime EndFinancialYear = new DateTime(DateTime.Today.Year, 6, 30);
            if (date < StartFinancilaYear || date > EndFinancialYear)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void btnAddWork_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    DateTime fromDate = Utility.GetParsedDate(txtStartDate.Text);
                    DateTime toDate = Utility.GetParsedDate(txtEndDate.Text);
                    if (fromDate > toDate)
                    {
                        Master.ShowMessage(Message.StartDateCannotBeGreaterThanEndDate.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txtSnctnDate.Text))
                {
                    DateTime SncnDate = Utility.GetParsedDate(txtSnctnDate.Text);
                    if (!string.IsNullOrEmpty(txtStartDate.Text))
                    {
                        if (IsDateLieFinancialYear(Utility.GetParsedDate(txtStartDate.Text)))
                        {
                            Master.ShowMessage(Message.DateMustLieInFinancialYear.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(txtEndDate.Text))
                    {
                        if (IsDateLieFinancialYear(Utility.GetParsedDate(txtEndDate.Text)))
                        {
                            Master.ShowMessage(Message.DateMustLieInFinancialYear.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    if (IsDateLieFinancialYear(SncnDate))
                    {
                        Master.ShowMessage(Message.DateMustLieInFinancialYear.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                }
                if (ddlErnsMnyType.SelectedItem.Value.Equals("% of Financial Bid"))
                {
                    if (Convert.ToInt32(txtErnsMny.Text) > 5)
                    {
                        Master.ShowMessage("% of Financial Bid cannot be more than 5%.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (txtCost.Text != "")
                {
                    if (Convert.ToInt64(txtCost.Text) == 0)
                    {
                        Master.ShowMessage("Cost should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (txtCompletionPeriod.Text != "")
                {
                    if (Convert.ToInt32(txtCompletionPeriod.Text) == 0)
                    {
                        Master.ShowMessage("Completion period should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (txtErnsMny.Text != "")
                {
                    if (Convert.ToDouble(txtErnsMny.Text) == 0)
                    {
                        Master.ShowMessage("Earnest money should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }

                if (txtndrFee.Text != "")
                {
                    if (Convert.ToDouble(txtndrFee.Text) == 0)
                    {
                        Master.ShowMessage("Tender fee should be greater than zero.", SiteMaster.MessageType.Error);
                        return;
                    }
                }



                AssetsWorkBLL bllAW = new AssetsWorkBLL();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Controls"
                long id = 0;
                string FinancialYear = ddlFinancialYear.SelectedItem.Text;
                string DivisionID = ddlDivision.SelectedItem.Value;
                string WorkName = txtWorkName.Text.Trim();
                string AssetWorkTypeID = ddlWorkType.SelectedItem.Value;
                string FundingSource = ddlFundingSource.SelectedItem.Value;
                string AssetType = ddlAssetCategory.SelectedItem.Text;
                string EstimatedCost = txtCost.Text.Trim();
                string CompletionPeriodFlag = "true";
                string CompletionPeriod = txtCompletionPeriod.Text.Trim();
                string CompletionPeriodUnit = ddlCompletionPeriodUnit.SelectedItem.Value;
                DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim() == string.Empty ? "01-01-0001" : txtStartDate.Text);
                DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim() == string.Empty ? "01-01-0001" : txtEndDate.Text);
                string SanctionNo = txtSanctionNo.Text.Trim();
                DateTime SanctionDate = Convert.ToDateTime(txtSnctnDate.Text.Trim());
                string EarnestMoneyType = ddlErnsMnyType.SelectedItem.Value;
                string EarnestMoney = txtErnsMny.Text.Trim();
                string TenderFees = txtndrFee.Text.Trim();
                string Description = txtDesc.Text.Trim();
                DateTime WorkStatusDate = DateTime.Now;
                int WorkStatusBy = Convert.ToInt32(mdlUser.ID);
                int CreatedBy = Convert.ToInt32(mdlUser.ID);
                id = hfWorkID.Value == string.Empty ? 0 : Convert.ToInt64(hfWorkID.Value);
                bool isUpdate = id == 0 ? false : true;
                string WorkStatus = "";
                if (id == 0)
                {
                    WorkStatus = "Draft";
                }
                dd_Work.Clear();
                dd_Work.Add("ID", id);
                dd_Work.Add("FinancialYear", FinancialYear);
                dd_Work.Add("DivisionID", DivisionID);
                dd_Work.Add("WorkName", WorkName);
                dd_Work.Add("FundingSource", FundingSource);
                dd_Work.Add("AssetWorkTypeID", AssetWorkTypeID);
                dd_Work.Add("AssetType", AssetType);
                dd_Work.Add("EstimatedCost", EstimatedCost);
                dd_Work.Add("CompletionPeriodFlag", CompletionPeriodFlag);
                dd_Work.Add("CompletionPeriod", CompletionPeriod);
                dd_Work.Add("CompletionPeriodUnit", CompletionPeriodUnit);
                dd_Work.Add("StartDate", StartDate);
                dd_Work.Add("EndDate", EndDate);
                dd_Work.Add("SanctionNo", SanctionNo);
                dd_Work.Add("SanctionDate", SanctionDate);
                dd_Work.Add("EarnestMoneyType", EarnestMoneyType);
                dd_Work.Add("EarnestMoney", EarnestMoney);
                dd_Work.Add("TenderFees", TenderFees);
                dd_Work.Add("Description", Description);
                dd_Work.Add("WorkStatus", WorkStatus);
                dd_Work.Add("WorkStatusDate", WorkStatusDate);
                dd_Work.Add("WorkStatusBy", WorkStatusBy);
                dd_Work.Add("CreatedBy", CreatedBy);
                if (AssetType == "Asset")
                {
                    dd_Work.Add("AssetWorkDetail", SaveAssociationWithAssets(isUpdate));
                }
                else if (AssetType == "Flood")
                {
                    dd_Work.Add("FloodWorkDetail", SaveAssociationWithFlood(isUpdate));
                }
                else if (AssetType == "Infrastructure")
                {
                    dd_Work.Add("InfraWorkDetail", SaveAssociationWithInfra(isUpdate));
                }

                //id == 0 && 
                if (bllAW.IsWorkUnique(dd_Work))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (Session["FloodDuplicate"].ToString() == "true")
                {
                    Master.ShowMessage("Unique value is required in association with flood", SiteMaster.MessageType.Error);
                    return;
                }
                if (Session["AssetDuplicate"].ToString() == "true")
                {
                    Master.ShowMessage("Unique value is required in association with Assets", SiteMaster.MessageType.Error);
                    return;
                }
                if (Session["InfraDuplicate"].ToString() == "true")
                {
                    Master.ShowMessage("Unique value is required in association with Infrastructure", SiteMaster.MessageType.Error);
                    return;
                }

                #endregion
                bool status = false;
                if (id == 0)
                {
                    status = (bool)bllAW.Work_Operations(Constants.CRUD_CREATE, dd_Work);
                }
                else
                {
                    status = (bool)bllAW.Work_Operations(Constants.CRUD_UPDATE, dd_Work);
                    if (AssetType == "Asset" && gvWork.Rows.Count == 0)
                        bllAW.AssetWorkDetailDelete(id);
                    if (AssetType == "Flood" && gvFlood.Rows.Count == 0)
                        bllAW.AssetWorkDetailDelete(id);
                    if (AssetType == "Infrastructure" && gv_Infra.Rows.Count == 0)
                        bllAW.AssetWorkDetailDelete(id);
                }
                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                }
                //Session["CtrlVal"] = null;
                Response.Redirect("~/Modules/AssetsAndWorks/Works/SearchWork.aspx?CWID=" + Request.QueryString["Eidt"] + "&RrecordSaved=" + status);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        //public void BindUserLocation()
        //{
        //    List<long> lstUserZone = new List<long>();
        //    List<long> lstUserCircle = new List<long>();
        //    List<long> lstUserDivision = new List<long>();

        //    long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

        //    UA_Users mdlUser = new UserBLL().GetUserByID(UserID);
        //    if (mdlUser.RoleID != Constants.AdministratorRoleID)
        //    {
        //        if (mdlUser.UA_Designations.IrrigationLevelID != null)
        //        {
        //            List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

        //            if (lstAssociatedLocation.Count() > 0)
        //            {
        //                if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
        //                {
        //                    #region Division Level Bindings

        //                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
        //                    {
        //                        lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

        //                        CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
        //                        lstUserCircle.Add((long)mdlDivision.CircleID);
        //                        lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
        //                    }

        //                    List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

        //                    long SelectedZoneID = lstZone.FirstOrDefault().ID;

        //                    List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

        //                    long SelectedCircleID = lstCircle.FirstOrDefault().ID;

        //                    List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

        //                    long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

        //                    ddlDivision.DataSource = lstDivision;
        //                    ddlDivision.DataTextField = "Name";
        //                    ddlDivision.DataValueField = "ID";
        //                    ddlDivision.DataBind();
        //                    ddlDivision.SelectedValue = SelectedDivisionID.ToString();

        //                    #endregion Division Level Bindings
        //                }
        //            }
        //            else
        //            {
        //                Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
        //            }
        //        }
        //        else
        //        {
        //            Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
        //        }
        //    }
        //    else
        //    {
        //        Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
        //    }

        //}
        public void LoadOtherDDL()
        {
            List<object> lstyear = new List<object>();
            string Year = Convert.ToString(DateTime.Today.AddYears(-1).Year + "-" + DateTime.Today.Year.ToString());
            lstyear.Add(new { ID = 1, Name = Year });
            Dropdownlist.BindDropdownlist<List<object>>(ddlFinancialYear, lstyear, (int)Constants.DropDownFirstOption.NoOption);
            LoadWorkTypeDDL();
        }
        private void LoadWorkTypeDDL()
        {
            Dropdownlist.DDLLoading(ddlWorkType, false, (int)Constants.DropDownFirstOption.NoOption, AWbll.GetWorkType(false));
            Dropdownlist.DDLLoading(ddlFundingSource, false, (int)Constants.DropDownFirstOption.Select,
               new TenderManagementBLL().GetFundingSourceList());
            Dropdownlist.DDLAssetsCategory(ddlAssetCategory, (int)Constants.DropDownFirstOption.Select);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlCategory.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSubCategory = (DropDownList)gvRow.FindControl("ddlSubCategory");
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, AWbll.GetSubCategoriesByCategoryID(Convert.ToInt32(ddlCategory.SelectedItem.Value)));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSubCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubCategory.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlLevel = (DropDownList)gvRow.FindControl("ddlLevel");
                    DropDownList ddlAssetName = (DropDownList)gvRow.FindControl("ddlAssetName");
                    if (ddlLevel != null && !String.IsNullOrEmpty(ddlLevel.SelectedItem.Value) && !string.IsNullOrEmpty(ddlSubCategory.SelectedItem.Value))
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, AWbll.GetAssetNameBySubCategoryAndLevelID(Convert.ToInt32(ddlSubCategory.SelectedItem.Value), Convert.ToInt64(ddlLevel.SelectedItem.Value), Convert.ToInt64(hdnAuctionedWorkID.Value)));
                    }
                    else
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, null);
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlLevel = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlLevel.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlAssetName = (DropDownList)gvRow.FindControl("ddlAssetName");
                    DropDownList ddlSubCategory = (DropDownList)gvRow.FindControl("ddlSubCategory");
                    if (ddlSubCategory != null && !String.IsNullOrEmpty(ddlSubCategory.SelectedItem.Value) && !string.IsNullOrEmpty(ddlLevel.SelectedItem.Value))
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, AWbll.GetAssetNameBySubCategoryAndLevelID(Convert.ToInt32(ddlSubCategory.SelectedItem.Value), Convert.ToInt64(ddlLevel.SelectedItem.Value), Convert.ToInt64(hdnAuctionedWorkID.Value)));
                    }
                    else
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, null);
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAssetCategory.SelectedItem.Text == "Asset")
            {
                PnlAssets.Visible = true;
                PnlFlood.Visible = false;
                PnlInfrastructure.Visible = false;
            }
            else if (ddlAssetCategory.SelectedItem.Text == "Flood")
            {
                PnlAssets.Visible = false;
                PnlFlood.Visible = true;
                PnlInfrastructure.Visible = false;
            }
            else if (ddlAssetCategory.SelectedItem.Text == "Infrastructure")
            {
                PnlAssets.Visible = false;
                PnlFlood.Visible = false;
                PnlInfrastructure.Visible = true;
            }
            else
            {
                PnlAssets.Visible = false;
                PnlFlood.Visible = false;
                PnlInfrastructure.Visible = false;
            }
        }
    }
}