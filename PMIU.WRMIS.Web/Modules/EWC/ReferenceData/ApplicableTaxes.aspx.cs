using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
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

namespace PMIU.WRMIS.Web.Modules.EWC.ReferenceData
{
    public partial class ApplicableTaxes : BasePage
    {
        //Data Members  
        List<object> lstData = new List<object>();
        Effluent_WaterChargesBLL bll_EWC = new Effluent_WaterChargesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    rbEffluent.Checked = true;
                    divEffluent.Visible = true;
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
        private void BindGrid()
        { 
            lstData = bll_EWC.Taxes_GetList(GetSource());
            GridView currentGrid = gv;
            if (GetSource().Equals(Constants.ECWServiceType.CANAL.ToString()))
            {
                currentGrid = gvC;
            }
           
            currentGrid.DataSource = lstData;
            currentGrid.DataBind();
        }

        private string GetSource() 
        {
            if (rbCanal.Checked)
                return Constants.ECWServiceType.CANAL.ToString();

            return Constants.ECWServiceType.EFFLUENT.ToString();
        }

        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridView currentGrid = (GridView)sender;
                if (e.CommandName == "Add")
                {
                    lstData = bll_EWC.Taxes_GetList(GetSource());
                    object mdl = new { ID = 0, Tax = "", TypeID = 0 , Type = "", Amount = 0,AmountS="", Description = "", IsActive = true};
                    
                    lstData.Add(mdl);

                    currentGrid.PageIndex = currentGrid.PageCount;
                    currentGrid.DataSource = lstData;
                    currentGrid.DataBind();

                    int index = currentGrid.Rows.Count - 1;
                    currentGrid.EditIndex = index;
                    currentGrid.DataBind();
                    currentGrid.Rows[index].FindControl("txtName").Focus();
                    DropDownList ddlType = (DropDownList)currentGrid.Rows[index].FindControl("ddlType");

                    if (ddlType != null)
                    { 
                        Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.NoOption, bll_EWC.GetTaxType());

                        HiddenField hdnFTypeID = (HiddenField)currentGrid.Rows[index].FindControl("TypeID");
                        if (!string.IsNullOrEmpty(hdnFTypeID.Value))
                            Dropdownlist.SetSelectedValue(ddlType, hdnFTypeID.Value);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView currentGrid = (GridView)sender;
                currentGrid.PageIndex = e.NewPageIndex;
                BindGrid();
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
                GridView currentGrid = (GridView)sender;
                long id = Convert.ToInt32(((Label)currentGrid.Rows[e.RowIndex].FindControl("lblID")).Text);

                EC_ApplicableTaxes mdl = new EC_ApplicableTaxes(); mdl.ID = id;
                
                // check for closure work type associated with other modules 
                if ((bool)bll_EWC.Operations_Taxes(Constants.CHECK_ASSOCIATION, mdl))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //delete record after all varification
                if ((bool)bll_EWC.Operations_Taxes(Constants.CRUD_DELETE, mdl))
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
             
                GridView currentGrid = (GridView)sender;
                GridViewRow row = currentGrid.Rows[e.RowIndex];

                EC_ApplicableTaxes mdl = new EC_ApplicableTaxes();
                mdl.ID = Convert.ToInt32(((Label)row.Cells[0].FindControl("lblID")).Text);
                mdl.ApplicableTax = ((TextBox)row.Cells[1].FindControl("txtName")).Text.Trim();
                mdl.IsActive = ((CheckBox)row.FindControl("cb_Active")).Checked;
                mdl.Description = ((TextBox)row.Cells[2].FindControl("txtDesc")).Text.Trim();
                mdl.Amount = Convert.ToDouble(((TextBox)row.Cells[1].FindControl("txtAmnt")).Text.Trim());
                mdl.PaymentTypeID = Convert.ToInt32(((DropDownList)row.Cells[1].FindControl("ddlType")).SelectedItem.Value);
                mdl.Source = GetSource();

                //Check if Name already exists
                EC_ApplicableTaxes obj =   (EC_ApplicableTaxes)bll_EWC.Operations_Taxes(Constants.CRUD_READ, mdl);
                if (obj != null && mdl.ID != obj.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool status = false;
                if (mdl.ID == 0) // Add scenario
                {
                    mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    mdl.CreatedDate = DateTime.Now;

                    status = (bool)bll_EWC.Operations_Taxes(Constants.CRUD_CREATE, mdl);
                }
                else //Edit Scenario
                {
                    mdl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    mdl.ModifiedDate = DateTime.Now;

                    status = (bool)bll_EWC.Operations_Taxes(Constants.CRUD_UPDATE, mdl);
                }

                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (mdl.ID == 0)
                    {
                        currentGrid.PageIndex = 0;
                    }
                    currentGrid.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                ((GridView)sender).EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView currentGrid = (GridView)sender;
                currentGrid.EditIndex = e.NewEditIndex;
                BindGrid();
                int index = e.NewEditIndex;
                currentGrid.Rows[index].FindControl("txtName").Focus();
                DropDownList ddlType = (DropDownList)currentGrid.Rows[index].FindControl("ddlType");

                if (ddlType != null)
                {
                    Dropdownlist.DDLLoading(ddlType, false, (int)Constants.DropDownFirstOption.NoOption, bll_EWC.GetTaxType());

                    HiddenField hdnFTypeID = (HiddenField)currentGrid.Rows[index].FindControl("TypeID");
                    if (!string.IsNullOrEmpty(hdnFTypeID.Value))
                        Dropdownlist.SetSelectedValue(ddlType, hdnFTypeID.Value);
                }
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
                ((GridView)sender).EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEffluent.Checked)
            {
                divCanal.Visible = false;
                divEffluent.Visible = true; 
            }
            else 
            {
                divCanal.Visible = true;
                divEffluent.Visible = false;
            }

            BindGrid();
        }
    }
}