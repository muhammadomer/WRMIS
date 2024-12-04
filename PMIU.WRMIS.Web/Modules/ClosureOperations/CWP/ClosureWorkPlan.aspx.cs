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

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.ClosureWorkPlan
{
    public partial class ClosureWorkPlan : BasePage
    {
        #region Golobal  variables
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;
        UA_RoleRights mdlRoleRights;
        long userID; 
        #endregion

        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadLocationDDL();
                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        SetControlsValues();
                    } 
                }
                mdlRoleRights = Master.GetPageRoleRights();
                btnAddCWP.Visible = (bool)mdlRoleRights.BAdd;
                if (mdlRoleRights.BAdd == true || mdlRoleRights.BEdit == true)
                    gvCWP.Columns[4].Visible = true;
                else
                    gvCWP.Columns[4].Visible = false;

                //Cheif can see status Column
                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                    gvCWP.Columns[4].Visible = true; 
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void SetControlsValues()
        {
            object currentObj = Session["CurrentControlsValues"] as object;
            if (currentObj != null)
            {
                ddlZone.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj));
                ddlCircle.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj));
                ddlDivision.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                ddlYear.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("YearID").GetValue(currentObj));

                LoadCWPGrid();
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
                    YearID = ddlYear.SelectedItem.Value
                };
            Session["CurrentControlsValues"] = obj;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ClosureWorkPlan);
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
            if (string.IsNullOrEmpty(strValue))
                return;

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
            try {
                userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                if (userID > 0) // Irrigation Staff 
                    LoadAllRegionDDByUser(userID, SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID);
                else
                    LoadDDL(ZONE, 0);

                Dropdownlist.DDLLoading(ddlYear, false, (int)Constants.DropDownFirstOption.All, bllCO.GetUniqueYearFromCWP()); 
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region GridView Methods 

        protected void gvCWP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCWP.PageIndex = e.NewPageIndex;
                LoadCWPGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCWP_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
              try
              {
                  long ID = Convert.ToInt64(((Label)gvCWP.Rows[e.RowIndex].FindControl("lblID")).Text);
                  bool isDependencyExist = bllCO.ClosureWorkPlandDependency(ID);
                  if (isDependencyExist)
                  {
                      Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                      return;
                  }
                  bool isDeleted = bllCO.DeleteClosureWorkPlan(ID);
                  if (isDeleted)
                  {
                      Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                      LoadCWPGrid();
                  }
                  else
                      Master.ShowMessage(Message.NotAllowedToDelete.Description, SiteMaster.MessageType.Error);
              }
              catch (Exception ex)
              {
                  new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
              }
        }

        protected void gvCWP_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvCWP.EditIndex = -1;
                LoadCWPGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCWP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if(lblStatus != null )
                {
                    if (!string.IsNullOrEmpty(lblStatus.Text))
                    {
                        if (lblStatus.Text.ToUpper().Equals("PUBLISH"))
                        {
                            HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEditClosureWorkPlan");
                            if (hlEdit != null)
                            {
                                hlEdit.Enabled = false;
                            }
                            Button hlDelete = (Button)e.Row.FindControl("lbtnDeleteClosureWorkPlan");
                            if (hlDelete != null)
                            {
                                hlDelete.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
        protected void gvCWP_RowCreated(object sender, GridViewRowEventArgs e)
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
        private void LoadCWPGrid()
        {
            try
            {
                SaveSearchCriteriaInSession();
                UA_RoleRights rights = Master.GetPageRoleRights();
                string status = "PUBLISH";
                // Only thoese users ( most probably XEN and CE )
                // who has add OR edit rights can see the drafted and published
                //Both type of plans
                //Done after discussion with BA (Sadaf Zafar) and Sir Iqbal
                if (rights.BAdd == true || rights.BEdit == true)
                    status = "";

                if (SessionManagerFacade.UserInformation.DesignationID == (long)Constants.Designation.ChiefIrrigation)
                    status = "";

                List<long?> lstZoneID = new List<long?>();
                List<long?> lstCircleID = new List<long?>();
                List<long> lstDivisionID = new List<long>();

                if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                {
                    if (string.IsNullOrEmpty(ddlCircle.SelectedItem.Value))
                    {
                        if (string.IsNullOrEmpty(ddlZone.SelectedItem.Value))
                        {
                            foreach (ListItem item in ddlZone.Items)
                            {
                                if (!string.IsNullOrEmpty(item.Value))
                                    lstZoneID.Add(Convert.ToInt64(item.Value));
                            }
                        }
                        else 
                            lstZoneID.Add(Convert.ToInt64(ddlZone.SelectedItem.Value));
                    }
                    else
                        lstCircleID.Add(Convert.ToInt64(ddlCircle.SelectedItem.Value));
                }
                else
                    lstDivisionID.Add(Convert.ToInt64(ddlDivision.SelectedItem.Value));
                 
                gvCWP.DataSource = bllCO.GetCWP_By_DivID_Year(lstZoneID, lstCircleID,lstDivisionID, ddlYear.SelectedItem.Text, status);
                gvCWP.DataBind();
                gvCWP.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        { 
            LoadCWPGrid();

        }
        protected void btnAddCWP_Click(object sender, EventArgs e)
        {
            SaveSearchCriteriaInSession();
            Response.Redirect("AddClosureWorkPlan.aspx", false);
        }

      
    }
}