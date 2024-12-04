using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.ScheduleInspection;
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
    public partial class SearchWork : BasePage
    {
        #region Golobal  variables
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;
        UA_RoleRights mdlRoleRights;
        long userID;
        #endregion

        AssetsWorkBLL AWbll = new AssetsWorkBLL();
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadLocationDDL();
                    LoadOtherDDL();
                    if (!string.IsNullOrEmpty(Request.QueryString["RrecordSaved"]))
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                    {
                        SetControlsValues();
                    }
                    btnAddWork.Enabled = base.CanAdd;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadOtherDDL()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlWorkType, AWbll.GetWorkType(false), (int)Constants.DropDownFirstOption.All);


            Dropdownlist.BindDropdownlist<List<object>>(ddlProgressStatus, AWbll.GetProgressStatus(), (int)Constants.DropDownFirstOption.All);


            Dropdownlist.BindDropdownlist<List<object>>(ddlFinancialYear, AWbll.GetFinancialYear(), (int)Constants.DropDownFirstOption.All);
            if (ddlFinancialYear.Items.Count > 1)
                ddlFinancialYear.SelectedIndex = 1;

            Dropdownlist.BindDropdownlist<List<object>>(ddlWorkStatus, CommonLists.GetWorkStatus(), (int)Constants.DropDownFirstOption.All);




        }

        protected void SetControlsValues()
        {
            object currentObj = Session["CurrentControlsValues"] as object;
            if (currentObj != null)
            {
                ddlZone.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj));
                ddlCircle.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj));
                ddlDivision.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                ddlFinancialYear.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("YearID").GetValue(currentObj));

                ddlWorkType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("WorkType").GetValue(currentObj));
                ddlWorkStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("WorkStatus").GetValue(currentObj));
                ddlProgressStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ProgressStatus").GetValue(currentObj));
                txtWorkName.Text = Convert.ToString(currentObj.GetType().GetProperty("WorkName").GetValue(currentObj));

                LoadWorkGrid();
            }

        }
        protected void SaveSearchCriteriaInSession()
        {
            Session["CurrentControlsValues"] = null;
            object obj = new
            {
                ZoneID = ddlZone.SelectedItem.Value,
                CircleID = ddlCircle.SelectedItem.Value,
                DivisionID = ddlDivision.SelectedItem.Value,
                YearID = ddlFinancialYear.SelectedItem.Value,

                WorkType = ddlWorkType.SelectedItem.Value,
                WorkStatus = ddlWorkStatus.SelectedItem.Value,
                ProgressStatus = ddlProgressStatus.SelectedItem.Value,
                WorkName = txtWorkName.Text
            };
            Session["CurrentControlsValues"] = obj;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssestsAndWorks);
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

                    Dropdownlist.DDLLoading(ddlDivision, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                    {
                        ddlDivision.SelectedIndex = 1;
                    }
                }

                lstChild = (List<object>)lstData.ElementAt(2);
                if (lstChild.Count > 0) // Circle
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    Dropdownlist.DDLLoading(ddlCircle, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                        ddlCircle.SelectedIndex = 1;
                }

                lstChild = (List<object>)lstData.ElementAt(3);
                if (lstChild.Count > 0) // Zone
                {
                    if (lstChild.Count == 1)
                        selectOption = (int)Constants.DropDownFirstOption.NoOption;

                    Dropdownlist.DDLLoading(ddlZone, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                        ddlZone.SelectedIndex = 1;
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
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList sndr = (DropDownList)sender;
            string strValue = sndr.SelectedItem.Value;
            //if (string.IsNullOrEmpty(strValue))
            //    return;

            if (sndr.ID.Equals("ddlZone"))
            {
                DisableDD(ddlCircle);
                DisableDD(ddlDivision);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlCircle"))
            {
                DisableDD(ddlDivision);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(DIVISION, Convert.ToInt64(strValue));
            }

            SaveSearchCriteriaInSession();

        }
        protected void WorkName_Change(object sender, EventArgs e)
        {
            SaveSearchCriteriaInSession();
        }
        private void LoadDDL(int _DDLType, long _SearchID)
        {
            WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
            List<object> lstData = new List<object>();
            DropDownList ddlToLoad = null;

            switch (_DDLType)
            {
                case ZONE: // Zone
                    lstData = bll_waterLosses.GetAllZones();
                    ddlToLoad = ddlZone;
                    break;
                case CIRCLE: // Circle
                    lstData = bll_waterLosses.GetCirclesByZoneID(_SearchID);
                    ddlToLoad = ddlCircle;
                    break;

                case DIVISION: // Division
                    lstData = bll_waterLosses.GetDivisionsByCircleID(_SearchID);
                    ddlToLoad = ddlDivision;
                    break;
                default:
                    break;
            }
            if (lstData.Count > 0)
                Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
        }
        private void LoadLocationDDL()
        {
            try
            {
                userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                if (userID > 0) // Irrigation Staff 
                    LoadAllRegionDDByUser(userID, SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID);
                else
                    LoadDDL(ZONE, 0);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region GridView Methods




        protected void gvWork_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Publish")
                {


                    // Validate work , check at least one Workitem exist for this work
                    if (AWbll.isWorkItemExist(Convert.ToInt64(e.CommandArgument)))
                    {
                        string status = AWbll.IsAssetWorkPlanCostEstimationCorrect(Convert.ToInt64(e.CommandArgument));
                        if (!string.IsNullOrEmpty(status))
                        {
                            Master.ShowMessage("Work Cannot be published. Estimation cost of Work and its Items should be same.", SiteMaster.MessageType.Error);
                            return;
                        }
                        AWbll.UpdateWorkStatus(Convert.ToInt64(e.CommandArgument), SessionManagerFacade.UserInformation.ID);
                        Master.ShowMessage(Message.AssetWorks_Published.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.AtLestOneWorkItem.Description, SiteMaster.MessageType.Warning);
                    }
                    LoadWorkGrid();
                }
                if (e.CommandName == "Unpublish")
                {
                    bool isInTender = AWbll.IsAWInTender(Convert.ToInt64(e.CommandArgument));
                    if (isInTender)
                    {
                        Master.ShowMessage("This work cannot be un-published, one or more of it's associated works are in tender process.", SiteMaster.MessageType.Error);
                        return;
                    }
                    else
                    {
                        AWbll.UpdateWorkStatusDraft(Convert.ToInt64(e.CommandArgument));
                        Master.ShowMessage(Message.AssetWorks_unpublish.Description, SiteMaster.MessageType.Success);
                        LoadWorkGrid();
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWork_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvWork.PageIndex = e.NewPageIndex;
                LoadWorkGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWork_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long AssetWorkID = Convert.ToInt64(((Label)gvWork.Rows[e.RowIndex].FindControl("lblID")).Text);

                bool IsDeleted = new AssetsWorkBLL().DeleteWorkByID(AssetWorkID);

                if (IsDeleted)
                {
                    LoadWorkGrid();
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWork_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvWork.EditIndex = -1;
                LoadWorkGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWork_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblWorkStatus = (Label)e.Row.FindControl("lblWorkStatus");
                if (lblWorkStatus != null)
                {
                    if (!string.IsNullOrEmpty(lblWorkStatus.Text))
                    {
                        if (lblWorkStatus.Text.ToUpper().Equals("DRAFT"))
                        {
                            HyperLink hlWorkItemDetail = (HyperLink)e.Row.FindControl("hlWorkItemDetail");
                            HyperLink hlProgress = (HyperLink)e.Row.FindControl("hlProgress");
                            HyperLink hlProgressHistory = (HyperLink)e.Row.FindControl("hlProgressHistory");
                            if (hlProgress != null)
                            {
                                hlProgress.Visible = false;
                            }

                            if (hlProgressHistory != null)
                            {
                                hlProgressHistory.Visible = false;
                            }

                            if (hlWorkItemDetail != null)
                            {
                                hlWorkItemDetail.Visible = false;
                            }


                        }
                    }
                    if (lblWorkStatus.Text.ToUpper().Equals("CONTRACT AWARDED"))
                    {
                        Button hlPublish = (Button)e.Row.FindControl("btnPublish");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                        HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                        HyperLink hlWorkItems = (HyperLink)e.Row.FindControl("hlWorkItems");
                        Label WorkID = (Label)e.Row.FindControl("lblID");
                        hlWorkItems.NavigateUrl = "~/Modules/AssetsAndWorks/Works/WorkItem.aspx?WorkStauts=CA&CWID=" + WorkID.Text + "&ViewMode=true";

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = false;
                        }
                        if (hlPublish != null)
                        {
                            hlPublish.Visible = false;
                        }
                        if (hlEdit != null)
                        {
                            hlEdit.Visible = false;
                        }

                        object obj = AWbll.GetContractorNameWithAmountByWorkID(Convert.ToInt64(WorkID.Text));

                        if (obj != null)
                        {
                            Label lblContractorName = (Label)e.Row.FindControl("lblContractorName");
                            Label lblContractorAmount = (Label)e.Row.FindControl("lblContractorAmount");
                            lblContractorAmount.Text = Utility.GetRoundOffValue(Convert.ToString(obj.GetType().GetProperty("ContractorAmount").GetValue(obj)));
                            lblContractorName.Text = Convert.ToString(obj.GetType().GetProperty("ContractorName").GetValue(obj));
                        }


                    }
                    else
                    {
                        Label lblContractorName = (Label)e.Row.FindControl("lblContractorName");
                        Label lblContractorAmount = (Label)e.Row.FindControl("lblContractorAmount");
                        lblContractorAmount.Text = "";
                        lblContractorName.Text = "";
                    }

                    if (!string.IsNullOrEmpty(lblWorkStatus.Text))
                    {
                        if (lblWorkStatus.Text.ToUpper().Equals("PUBLISHED"))
                        {
                            Button hlPublish = (Button)e.Row.FindControl("btnPublish");
                            Button btnUnPublish = (Button)e.Row.FindControl("btnUnPublish");
                            HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                            HyperLink hlWorkItemDetail = (HyperLink)e.Row.FindControl("hlWorkItemDetail");
                            Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                            HyperLink hlProgress = (HyperLink)e.Row.FindControl("hlProgress");
                            HyperLink hlProgressHistory = (HyperLink)e.Row.FindControl("hlProgressHistory");
                            HyperLink hlWorkItems = (HyperLink)e.Row.FindControl("hlWorkItems");
                            Label WorkID = (Label)e.Row.FindControl("lblID");



                            if (btnDelete != null)
                            {
                                btnDelete.Visible = false;
                            }
                            if (hlPublish != null)
                            {
                                hlPublish.Visible = false;
                            }
                            if (hlProgress != null)
                            {
                                hlProgress.Enabled = false;
                            }
                            if (hlProgressHistory != null)
                            {
                                hlProgressHistory.Enabled = false;
                            }
                            if (hlEdit != null)
                            {
                                hlEdit.Visible = false;
                            }
                            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                            {
                                btnUnPublish.Visible = true;
                            }
                            hlWorkItems.NavigateUrl = "~/Modules/AssetsAndWorks/Works/WorkItem.aspx?WorkStauts=CA&CWID=" + WorkID.Text + "&ViewMode=true";

                        }
                    }
                }

            }
        }
        protected void gvWork_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEditClosureWorkPlan");
                        if (hlEdit != null)
                        {
                            hlEdit.Enabled = (bool)mdlRoleRights.BEdit;
                        }
                        Button hlDelete = (Button)e.Row.FindControl("lbtnDeleteClosureWorkPlan");
                        if (hlDelete != null)
                        {
                            hlDelete.Enabled = (bool)mdlRoleRights.BDelete;
                        }
                        HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlClosureWorkPlanDetail");
                        if (hlDetail != null)
                        {
                            hlDetail.Enabled = (bool)mdlRoleRights.BView;
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
        private long? GetValue(String _StrValue)
        {
            if (string.IsNullOrEmpty(_StrValue))
                return null;

            return Convert.ToInt64(_StrValue);
        }
        private void LoadWorkGrid()
        {
            try
            {
                Dictionary<string, object> dd_SearchWork = new Dictionary<string, object>();
                try
                {

                    long ZoneID = 0;
                    long CircleID = 0;
                    long DivisionID = 0;
                    string FinancialYear = "";
                    long WorkTypeID = 0;
                    string WorkStatus = "";
                    long ProgressStatusID = 0;
                    string WorkName = "";
                    ZoneID = ddlZone == null ? 0 : Convert.ToInt64(ddlZone.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlZone.SelectedItem.Value));
                    CircleID = ddlCircle == null ? 0 : Convert.ToInt64(ddlCircle.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlCircle.SelectedItem.Value));
                    DivisionID = ddlDivision == null ? 0 : Convert.ToInt64(ddlDivision.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlDivision.SelectedItem.Value));
                    FinancialYear = ddlFinancialYear == null ? "" : ddlFinancialYear.SelectedItem.Text == "All" ? "" : ddlFinancialYear.SelectedItem.Text;
                    WorkTypeID = ddlWorkType == null ? 0 : Convert.ToInt64(ddlWorkType.SelectedItem.Value == "" ? "0" : ddlWorkType.SelectedItem.Value);
                    WorkStatus = ddlWorkStatus == null ? "" : (ddlWorkStatus.SelectedItem.Text == "All" ? "" : ddlWorkStatus.SelectedItem.Text);
                    ProgressStatusID = ddlProgressStatus == null ? 0 : Convert.ToInt64(ddlProgressStatus.SelectedItem.Value == "" ? "0" : ddlProgressStatus.SelectedItem.Value);
                    WorkName = txtWorkName.Text;
                    dd_SearchWork.Clear();
                    dd_SearchWork.Add("ZoneID", ZoneID);
                    dd_SearchWork.Add("CircleID", CircleID);
                    dd_SearchWork.Add("DivisionID", DivisionID);
                    dd_SearchWork.Add("FinancialYear", FinancialYear);
                    dd_SearchWork.Add("WorkType", WorkTypeID);
                    dd_SearchWork.Add("WorkStatus", WorkStatus);
                    dd_SearchWork.Add("ProgressStatus", ProgressStatusID);
                    dd_SearchWork.Add("WorkName", WorkName);
                    List<object> lstObj = AWbll.GetWorkbySearchCriteria(dd_SearchWork);
                    if (!(SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN)) && base.CanEdit == false)
                    {
                        lstObj.RemoveAll(x => x.GetType().GetProperty("WorkStatus").GetValue(x).ToString().ToUpper() == "DRAFT");
                    }
                    gvWork.DataSource = lstObj;
                    gvWork.DataBind();
                    gvWork.Visible = true;
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadWorkGrid();
            SaveSearchCriteriaInSession();
        }
        protected void btnAddCWP_Click(object sender, EventArgs e)
        {
            SaveSearchCriteriaInSession();
            Response.Redirect("AddClosureWorkPlan.aspx", false);
        }

        protected void btnAddWork_Click(object sender, EventArgs e)
        {

        }


    }
}