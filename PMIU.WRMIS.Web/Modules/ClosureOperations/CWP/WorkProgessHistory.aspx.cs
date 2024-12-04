using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
    public partial class WorkProgessHistory : BasePage
    {
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadDropdowns();
                    if (!string.IsNullOrEmpty(Request.QueryString["CWPID"]))
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/ClosureOperationPlanDetail.aspx?CWPID=" + Request.QueryString["CWPID"];
                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                        hdnF_CWID.Value = Request.QueryString["CWID"];

                    LoadClosueWorkInfo();
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-10));
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        SetControlsValues();
                    }
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.WorkProgressHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadClosueWorkInfo()
        {
            ucInfo._ID = Convert.ToInt64(hdnF_CWID.Value);
            ucInfo.isCWP = false;
        }
        private void LoadDropdowns()
        {
            List<object> lst = bllCO.GetWorkStatusList();
            Dropdownlist.DDLLoading(ddlWorkStatus, false, (int)Constants.DropDownFirstOption.NoOption, lst);
            ddlWorkStatus.Items.Insert(0, new ListItem("All", ""));

            long designationID = (long)SessionManagerFacade.UserInformation.DesignationID; 
            List<string> lstNames = new List<string>();
            if (SessionManagerFacade.UserAssociatedLocations.UserID == 0)
            {
                ddlInspectedBy.Items.Clear(); 
                ddlInspectedBy.Items.Insert(0, new ListItem("All", "")); 
                ddlInspectedBy.Items.Insert(1, new ListItem("ADM", "" + (long)Constants.Designation.ADM));
                ddlInspectedBy.Items.Insert(2, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
                ddlInspectedBy.Items.Insert(3, new ListItem("SE", "" + (long)Constants.Designation.SE));
                ddlInspectedBy.Items.Insert(4, new ListItem("Cheif Irrigation", "" + (long)Constants.Designation.ChiefIrrigation));  
            }
            else
            {
                if (designationID == (long) Constants.Designation.XEN ||designationID == (long) Constants.Designation.ADM)
                {
                    ddlInspectedBy.Items.Clear(); 
                    ddlInspectedBy.Items.Insert(0, new ListItem(bllCO.GetDesignationByID(designationID),  ""+ designationID));  
                }
                else if (designationID == (long) Constants.Designation.SE)
                {  
                    ddlInspectedBy.Items.Clear();

                    ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                    ddlInspectedBy.Items.Insert(1, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
                    ddlInspectedBy.Items.Insert(2, new ListItem("SE", "" + (long)Constants.Designation.SE));  
                      
                }
                else if (designationID == (long) Constants.Designation.ChiefIrrigation)
                { 
                    ddlInspectedBy.Items.Clear();

                    ddlInspectedBy.Items.Insert(0, new ListItem("All", ""));
                    ddlInspectedBy.Items.Insert(1, new ListItem("XEN", "" + (long)Constants.Designation.XEN));
                    ddlInspectedBy.Items.Insert(2, new ListItem("SE", "" + (long)Constants.Designation.SE));
                    ddlInspectedBy.Items.Insert(3, new ListItem("Cheif Irrigation", "" + (long)Constants.Designation.ChiefIrrigation));  
                }
            } 
        }
        private bool VerifyDateRange()
        {
            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
                return true;
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now ;

            if(!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = Utility.GetParsedDate(txtFromDate.Text);
                if (fromDate > DateTime.Now )
                {
                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = Utility.GetParsedDate(txtToDate.Text);
                //Future date check
                if (toDate > DateTime.Now)
                {
                    Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                if (fromDate > toDate)
                {
                    Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                    return false;
                }
            }
            
            return true;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!VerifyDateRange())
                return;

            BindGrid();
            Session["WPH_SearchCriteria"] = null;
            SaveSearchCriteriaInSession();
        }

        private void BindGrid()
        {
            try
            {
                DateTime? fromDate = null;
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    fromDate = Utility.GetParsedDate(txtFromDate.Text);

                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(txtToDate.Text))
                    toDate = Utility.GetParsedDate(txtToDate.Text);

                int workStatusID = 0;
                if (!string.IsNullOrEmpty(ddlWorkStatus.SelectedItem.Value))
                    workStatusID = Convert.ToInt32(ddlWorkStatus.SelectedItem.Value);

                List<long> inspectedBy = new List<long>();
                long workID = Convert.ToInt64(hdnF_CWID.Value);
                if (string.IsNullOrEmpty(ddlInspectedBy.SelectedItem.Value))
                {
                    foreach (ListItem i in ddlInspectedBy.Items)
                    {
                        if (!string.IsNullOrEmpty(i.Value))
                        {
                            inspectedBy.Add(Convert.ToInt64(i.Value));
                        }
                    }

                }
                else
                    inspectedBy.Add(Convert.ToInt64(ddlInspectedBy.SelectedItem.Value));
                List<object> lstData = bllCO.GetWorkProgressHistory(workStatusID, inspectedBy, workID, fromDate, toDate);
                gvCWP.DataSource = lstData;
                gvCWP.DataBind(); 
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvCWP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Detail"))
                {
                    GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    long id = Convert.ToInt64(((Label)gvrow.FindControl("lblID")).Text);
                    string URL = "~/Modules/ClosureOperations/CWP/AddWorkProgress.aspx?CWID=" + hdnF_CWID.Value
                        + "&CWPID=" + Request.QueryString["CWPID"] + "&WItemID=" + id;
                    Response.Redirect(URL, false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void SaveSearchCriteriaInSession()
        {
            object obj = new
            {
                WorkStatusID = ddlWorkStatus.SelectedItem.Value,
                InspectedBy = ddlInspectedBy.SelectedItem.Value,
                FromDate = txtFromDate.Text,
                ToDate = txtToDate.Text
            };
            Session["WPH_SearchCriteria"] = obj;
        }

        protected void SetControlsValues()
        {
            object currentObj = Session["WPH_SearchCriteria"] as object;
            if (currentObj != null)
            {
                ddlWorkStatus.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("WorkStatusID").GetValue(currentObj));
                ddlInspectedBy.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("InspectedBy").GetValue(currentObj));
                txtFromDate.Text = Convert.ToString(currentObj.GetType().GetProperty("FromDate").GetValue(currentObj));
                txtToDate.Text = Convert.ToString(currentObj.GetType().GetProperty("ToDate").GetValue(currentObj));

            }

        }

        protected void gvCWP_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvCWP.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCWP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCWP.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}