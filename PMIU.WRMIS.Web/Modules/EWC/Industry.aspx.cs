using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.EWC
{
    public partial class Industry : BasePage
    {
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;
        private Effluent_WaterChargesBLL bllEWC = new Effluent_WaterChargesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    //long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    //if (userID > 0 && boundryLvlID != null) // Irrigation Staff
                    //{
                    //    LoadAllRegionDDByUser(userID, boundryLvlID);
                    //}
                    //else
                    LoadDDL(ZONE, 0);
                    LoadIndustryTypeDDL();

                    if (!string.IsNullOrEmpty(Request.QueryString["isRecordSaved"]))
                    {
                        if (Convert.ToBoolean(Request.QueryString["isRecordSaved"].ToString()))
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        else
                            Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        SetControlsValues();
                    }

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
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlCircle"))
            {
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(DIVISION, Convert.ToInt64(strValue));
            }

            SaveSearchCriteriaInSession();

        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.EffluentandWaterCharges);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
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

                    Dropdownlist.DDLLoading(ddlDiv, false, selectOption, lstChild);
                    if (lstChild.Count == 1)
                    {
                        ddlDiv.SelectedIndex = 1;
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
                    ddlToLoad = ddlDiv;
                    break;

                default:
                    break;
            }
            if (lstData.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.All, lstData);
                ddlToLoad.Enabled = true;
            }
            //if (lstData.Count == 1)
            //{
            //    ddlToLoad.SelectedIndex = 1;
            //    ddlToLoad.Items.RemoveAt(0);
            //}
        }
        private void LoadIndustryTypeDDL()
        {
            List<object> lstIndustryType = bllEWC.IndustryType_GetList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.All, lstIndustryType);
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO: 
                //add when start adding data in DDLs
                SaveSearchCriteriaInSession();
                Button btn = (Button)sender;
                if (btn.ID.Equals("btnSearch"))
                {
                    LoadGrid();
                }
                else if (btn.ID.Equals("btnAdd"))
                {

                    Response.Redirect("AddIndustry.aspx", false);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void SetControlsValues()
        {
            object currentObj = Session["CurrentControlsValues"] as object;
            if (currentObj != null)
            {
                ddlZone.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("ZoneID").GetValue(currentObj));

                DisableDD(ddlCircle);
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(ddlZone.SelectedValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(ddlZone.SelectedValue));

                ddlCircle.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CircleID").GetValue(currentObj));

                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(ddlCircle.SelectedValue))
                    LoadDDL(DIVISION, Convert.ToInt64(ddlCircle.SelectedValue));


                ddlDiv.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                ddlType.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("Type").GetValue(currentObj));
                ddlStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("Status").GetValue(currentObj));
                txtIndNo.Text = Convert.ToString(currentObj.GetType().GetProperty("No").GetValue(currentObj));
                txtIndName.Text = Convert.ToString(currentObj.GetType().GetProperty("Name").GetValue(currentObj));

                LoadGrid();
            }

        }
        protected void LoadGrid()
        {
            try
            {

                SaveSearchCriteriaInSession();
                Hashtable SearchCriteria = new Hashtable();
                long? SelectedIndustryID = null;
                string SelectedLevelText = null;
                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                long? SelectedIndustryType = null;
                string IndustryName = null;
                string SelectedIndustryStatus = null;

                if (ddlZone.SelectedItem.Value != String.Empty)
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }

                if (ddlCircle.SelectedItem.Value != String.Empty)
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }

                if (ddlDiv.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDiv.SelectedItem.Value);
                }

                if (ddlType.SelectedItem.Value != String.Empty)
                {
                    SelectedIndustryType = Convert.ToInt64(ddlType.SelectedItem.Value);
                }

                if (txtIndNo.Text != "")
                {
                    SelectedIndustryID = Convert.ToInt64(txtIndNo.Text);
                }

                if (ddlStatus.SelectedItem.Value != "0")
                {
                    SelectedIndustryStatus = Convert.ToString(ddlStatus.SelectedItem.Text);
                }

                if (txtIndName.Text != "")
                {
                    IndustryName = txtIndName.Text;
                }

                IEnumerable<DataRow> IeAssets = new Effluent_WaterChargesBLL().GetIndustrySearch(SelectedLevelText, SelectedIndustryID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedIndustryType, SelectedIndustryStatus, IndustryName);
                var LstAssets = IeAssets.Select(dataRow => new
                {
                    IndustryID = dataRow.Field<long>("IndustryID"),
                    EWDivision = dataRow.Field<string>("EWDivision"),
                    CWDivision = dataRow.Field<string>("CWDivision"),
                    IndustryType = dataRow.Field<string>("IndustryType"),
                    IndustryName = dataRow.Field<string>("IndustryName"),
                    Status = dataRow.Field<string>("Status")
                }).ToList();
                gv.DataSource = LstAssets;
                gv.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void SaveSearchCriteriaInSession()
        {
            Session["CurrentControlsValues"] = null;
            object obj = new
            {
                ZoneID = ddlZone.SelectedItem.Value,
                CircleID = ddlCircle.SelectedItem.Value,
                DivisionID = ddlDiv.SelectedItem.Value,
                Type = ddlType.SelectedItem.Value,
                Status = ddlStatus.SelectedItem.Value,
                No = txtIndNo.Text.Trim(),
                Name = txtIndName.Text.Trim()
            };
            Session["CurrentControlsValues"] = obj;
        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv.EditIndex = -1;
                LoadGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long lblID = Convert.ToInt64(((Label)gv.Rows[e.RowIndex].FindControl("lblID")).Text);

                bool IsDeleted = new Effluent_WaterChargesBLL().DeleteIndustryByID(lblID);

                if (IsDeleted)
                {
                    LoadGrid();
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

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("BillingDetail"))
            {
                Session["IndustryName"] = "";
                Session["EWDivision"] = "";
                Session["CWDivision"] = "";
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                DataKey key = gv.DataKeys[row.RowIndex];
                Session["IndustryName"] = Convert.ToString(key["IndustryName"]);
                Session["EWDivision"] = Convert.ToString(key["EWDivision"]);
                Session["CWDivision"] = Convert.ToString(key["CWDivision"]);
                //SaveSearchCriteriaInSession();
                Response.Redirect("~/Modules/EWC/BillDetail.aspx?ID=" + Convert.ToString(key["IndustryID"]), false);
            }
        }

    }
}