 using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
public delegate void DelegatePopulateData(); 
public partial class ClosureWorkItem : BasePage
    {
        //Data Members 
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        Dictionary<string, object> unit_data = new Dictionary<string, object>();
        List<object> lstUnits = new List<object>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                        hdnF_CWID.Value = Request.QueryString["CWID"];   
                        
                    LoadClosueWorkInfo(); 
                    if (!string.IsNullOrEmpty(Request.QueryString["CWPID"])) 
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/ClosureOperationPlanDetail.aspx?CWPID=" + Request.QueryString["CWPID"] ; 

                    if (!string.IsNullOrEmpty(Request.QueryString["ViewMode"]))
                        hdcF_ViewMode.Value = "" + Request.QueryString["ViewMode"];

                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                    if (mdlRoleRights != null)
                        btnAdd.Enabled = (bool)mdlRoleRights.BAdd;

                    if (Convert.ToBoolean(hdcF_ViewMode.Value))
                    {
                        btnAdd.Visible = false;
                    }

                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (btnEdit != null)
                        btnEdit.Enabled = (bool)mdlRoleRights.BEdit;
                    if (btnDelete != null)
                        btnDelete.Enabled = (bool)mdlRoleRights.BDelete;

                    if (Convert.ToBoolean(hdcF_ViewMode.Value))
                    {
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }

                    Label lblQty = (Label)e.Row.FindControl("lblSancQty");
                    if(lblQty != null)
                    {
                        string val = lblQty.Text;
                        lblQty.Text = Utility.GetRoundOffValue(val);
                    }

                    Label lblRate = (Label)e.Row.FindControl("lblTSRate");
                    if (lblRate != null)
                    {
                        string val = lblRate.Text;
                        lblRate.Text = Utility.GetRoundOffValue(val);
                    }


                    //Show contractor amount 
                    long ID = Convert.ToInt64(((Label)e.Row.FindControl("lblID")).Text);
                    string amount = Utility.GetRoundOffValue( bllCO.ContractorAmountPerWorkItem(ID)) ;
                    ((Label)e.Row.FindControl("lblAmnt")).Text = amount;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotal = (Label)e.Row.FindControl("lblTotal"); 
                    List<object> temp  = bllCO.GetClosureWorkItems_ByCWID( Convert.ToInt64(hdnF_CWID.Value));
                    if (temp != null && temp.Count > 0)
                        lblTotal.Text = Utility.GetRoundOffValue(
                            temp
                            .AsEnumerable<object>()
                            .Sum(x =>
                                    Convert.ToDecimal(
                                    x.GetType().GetProperty("TSAmount").GetValue(x, null))
                                    ).ToString());
                    else
                        lblTotal.Text = Utility.GetRoundOffValue("");
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long cwID = Convert.ToInt64(hdnF_CWID.Value);
                    lstUnits = bllCO.GetClosureWorkItems_ByCWID(cwID); 
                    object mdlObj = new { ID = 0, ItemDescription = "", SanctionedQty = "",UnitID = "" ,Unit = "", TSRate = "", TSAmount = "" };

                    lstUnits.Add(mdlObj);

                    gvUnits.PageIndex = gvUnits.PageCount;
                    gvUnits.DataSource = lstUnits;
                    gvUnits.DataBind();

                    gvUnits.EditIndex = gvUnits.Rows.Count - 1;
                    gvUnits.DataBind();

                    GridViewRow row = gvUnits.Rows[gvUnits.Rows.Count - 1];
                    row.FindControl("txtDesc").Focus();
                    DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");

                    if (ddlUnit != null)
                    {
                        List<object> lst = bllCO.GetAllUnit().Where(x => x.IsActive == true).Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
                        Dropdownlist.DDLLoading(ddlUnit, false, (int)Constants.DropDownFirstOption.NoOption, lst);

                        HiddenField lblDomainID = (HiddenField)row.FindControl("hdnFUnit");
                        if (!string.IsNullOrEmpty(lblDomainID.Value))
                            Dropdownlist.SetSelectedValue(ddlUnit, lblDomainID.Value);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUnits.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CW_WorkItem mdl = new CW_WorkItem();        
                mdl.ID = Convert.ToInt64(((Label)gvUnits.Rows[e.RowIndex].FindControl("lblID")).Text);;
                bool IsDeleted = (bool)bllCO.CWItems_Operations(Constants.CRUD_DELETE, mdl);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
                else
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvUnits.Rows[e.RowIndex];
                CW_WorkItem mdl = new CW_WorkItem();
                mdl.ID = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                mdl.ItemDescription = ((TextBox)row.FindControl("txtDesc")).Text.Trim();
                mdl.WorkID = Convert.ToInt64(hdnF_CWID.Value);
                CW_WorkItem mdlResult = (CW_WorkItem)bllCO.CWItems_Operations(Constants.CHECK_DUPLICATION, mdl);
                if (mdlResult != null && mdl.ID != mdlResult.ID)
                {
                    Master.ShowMessage("Unique Item Description is required.", SiteMaster.MessageType.Error);
                    return;
                }

                if (Convert.ToInt64(((TextBox)row.FindControl("txtSnctQty")).Text) <= 0)
                { 
                    Master.ShowMessage("Sanctioned Quantity cannot be Zero.", SiteMaster.MessageType.Error);
                    return;
                }
                mdl.SanctionedQty = Convert.ToInt64(((TextBox)row.FindControl("txtSnctQty")).Text);
                mdl.TSUnitID = Convert.ToInt32(((DropDownList)row.FindControl("ddlUnit")).SelectedItem.Value); ;
                if(Convert.ToDouble(((TextBox)row.FindControl("txtTSRate")).Text) <= 0)
                { 
                    Master.ShowMessage("Sanctioned Rate cannot be Zero.", SiteMaster.MessageType.Error);
                    return;
                }
                mdl.TSRate = Convert.ToDouble(((TextBox)row.FindControl("txtTSRate")).Text) ;

                bool IsRecordSaved = false;

                if (mdl.ID == 0)
                {
                    mdl.CreatedDate = DateTime.Now;
                    mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    IsRecordSaved = (bool)bllCO.CWItems_Operations(Constants.CRUD_CREATE, mdl);
                }
                else
                {
                    mdl.ModifiedDate = DateTime.Now;
                    mdl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    IsRecordSaved = (bool)bllCO.CWItems_Operations(Constants.CRUD_UPDATE, mdl);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success); 
                    gvUnits.EditIndex = -1;
                    BindGrid();
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvUnits.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvUnits_RowEditing(object sender, GridViewEditEventArgs e)
        {
                try
                {
                    gvUnits.EditIndex = e.NewEditIndex;
                    BindGrid();
                    GridViewRow row = gvUnits.Rows[e.NewEditIndex];
                    row.FindControl("txtDesc").Focus();
                    DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");

                    if (ddlUnit != null)
                    {
                        List<object> lstUnits = bllCO.GetAllUnit().Where(x=> x.IsActive == true).Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
                        Dropdownlist.DDLLoading(ddlUnit, false, (int)Constants.DropDownFirstOption.NoOption, lstUnits);

                        HiddenField lblDomainID = (HiddenField)row.FindControl("hdnFUnit");
                        if (!string.IsNullOrEmpty(lblDomainID.Value)) 
                            Dropdownlist.SetSelectedValue(ddlUnit, lblDomainID.Value);
                    }
                }
                catch (Exception exp)
                {
                    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                }
        }
        protected void gvUnits_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvUnits.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }            
            
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ClosureWorkItem);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadClosueWorkInfo()
        {
            ucInfo._ID = Convert.ToInt64(hdnF_CWID.Value);

            ucInfo.isCWP = false;
        }
        private void BindGrid()
        {
            try 
            { 
                long cwID = Convert.ToInt64(hdnF_CWID.Value);
                lstUnits = bllCO.GetClosureWorkItems_ByCWID(cwID);
                gvUnits.DataSource = lstUnits;
                gvUnits.DataBind();

                if (Convert.ToBoolean(hdcF_ViewMode.Value))// Published  
                {
                    gvUnits.Columns[6].Visible = true;
                    gvUnits.Columns[7].Visible = false;
                }
                else // drafted
                {
                    gvUnits.Columns[6].Visible = false;
                    gvUnits.Columns[7].Visible = true;
                } 
                //if (!bllCO.IsTenderAwarded(cwID))
                //    gvUnits.Columns[6].Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
            
    }
}