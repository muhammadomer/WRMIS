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
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Works
{
    public delegate void DelegatePopulateData();
    public partial class WorkItem : BasePage
    {
        //Data Members 
        double TotalAwardedAmount = 0.0;
        double TotalAmount = 0.0;
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        AssetsWorkBLL bllAW = new AssetsWorkBLL();
        Dictionary<string, object> unit_data = new Dictionary<string, object>();
        List<object> lstUnits = new List<object>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string ws="";
                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                        hdnF_CWID.Value = Request.QueryString["CWID"];
                    if (!string.IsNullOrEmpty(Request.QueryString["WorkStauts"]))
                        ws = Request.QueryString["WorkStauts"];
                    if (ws.Equals("CA"))
                    {
                        gvUnits.Enabled = false;
                    }

                    LoadClosueWorkInfo();
                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                        hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/SearchWork.aspx?CWID=" + Request.QueryString["CWID"];

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
                    if (lblQty != null && !string.IsNullOrEmpty(lblQty.Text))
                    {
                        string val = lblQty.Text;
                        lblQty.Text = Utility.GetRoundOffValue(val);
                    }

                    Label lblRate = (Label)e.Row.FindControl("lblTSRate");
                    Label lblTAmnt = (Label)e.Row.FindControl("lblTAmnt");
                    if (lblRate != null && !string.IsNullOrEmpty(lblRate.Text))
                    {
                        string val = lblRate.Text;
                        lblRate.Text = Utility.GetRoundOffValue(val);
                        
                    }
                    if (lblTAmnt != null && !string.IsNullOrEmpty(lblTAmnt.Text))
                    {
                        TotalAmount = TotalAmount + Convert.ToDouble(lblTAmnt.Text);
                    }

                    //Show contractor amount 
                    long ID = Convert.ToInt64(((Label)e.Row.FindControl("lblID")).Text);
                    string amount = Utility.GetRoundOffValue(bllAW.ContractorAmountPerWorkItem(ID));
                    TotalAwardedAmount = TotalAwardedAmount+Convert.ToDouble(amount);
                    ((Label)e.Row.FindControl("lblAmnt")).Text = amount;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
                    Label lblTotalAwardedAmount = (Label)e.Row.FindControl("lblTotalAwardedAmount");

                    lblTotalAwardedAmount.Text = Convert.ToString(string.Format("{0:#,##0.##}", TotalAwardedAmount));
                    lblTotalAmount.Text = Convert.ToString(string.Format("{0:#,##0.##}", TotalAmount));
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
                    lstUnits = bllCO.GetClosureWorkItems_ByCWID(cwID, "ASSETWORK");
                    object mdlObj = new { ID = 0, ItemDescription = "", SanctionedQty = "", UnitID = "", Unit = "", TSRate = "", TSAmount = "" };

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
                mdl.ID = Convert.ToInt64(((Label)gvUnits.Rows[e.RowIndex].FindControl("lblID")).Text); ;
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

                mdl.SanctionedQty = Convert.ToInt64(((TextBox)row.FindControl("txtSnctQty")).Text);
                mdl.TSUnitID = Convert.ToInt32(((DropDownList)row.FindControl("ddlUnit")).SelectedItem.Value); ;
                mdl.TSRate = Convert.ToDouble(((TextBox)row.FindControl("txtTSRate")).Text);

                if(mdl.SanctionedQty==0)
                {
                    Master.ShowMessage("Sanctioned quantity should be greater than zero.", SiteMaster.MessageType.Error);
                    return;
                }
                if(mdl.TSRate==0)
                {
                    Master.ShowMessage("Technical sanction rate should be greater than zero.", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsRecordSaved = false;

                if (mdl.ID == 0)
                {
                    mdl.CreatedDate = DateTime.Now;
                    mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    IsRecordSaved = (bool)bllCO.CWItems_Operations(Constants.CRUD_CREATE, mdl, "ASSETWORK");
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
                    List<object> lstUnits = bllCO.GetAllUnit().Where(x => x.IsActive == true).Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddAssetWorkItem);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadClosueWorkInfo()
        {
            ucInfo._WorkID = Convert.ToInt64(hdnF_CWID.Value);
            //ucInfo. = false;
        }
        private void BindGrid()
        {
            try
            {
                long cwID = Convert.ToInt64(hdnF_CWID.Value);
                lstUnits = bllCO.GetClosureWorkItems_ByCWID(cwID, "ASSETWORK");
                gvUnits.DataSource = lstUnits;
                gvUnits.DataBind();

                if (Convert.ToBoolean(hdcF_ViewMode.Value))// Published  
                {
                    gvUnits.Columns[6].Visible = true;
                    gvUnits.Columns[7].Visible = false;
                    gvUnits.FooterRow.Visible = true;
                }
                else // drafted
                {
                    gvUnits.Columns[6].Visible = false;
                    gvUnits.Columns[7].Visible = true;
                    gvUnits.FooterRow.Visible = false;
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