using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.BLL.EffluentAndWaterCharges;
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

namespace PMIU.WRMIS.Web.Modules.EWC.ReferenceData
{
    public partial class IndustryType : BasePage
    {
        //Data Members 
        Dictionary<string, object> dd_data = new Dictionary<string, object>();
        List<EC_IndustryType> lstWorkType = new List<EC_IndustryType>();
        Effluent_WaterChargesBLL bll_EWC = new Effluent_WaterChargesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
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
            lstWorkType = bll_EWC.IndustryType_GetList();
            gv.DataSource = lstWorkType;
            gv.DataBind();
        }
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstWorkType = bll_EWC.IndustryType_GetList();

                    EC_IndustryType mdl = new EC_IndustryType();
                    mdl.ID = 0;
                    mdl.Name = "";
                    mdl.Description = "";
                    mdl.IsActive = true;

                    lstWorkType.Add(mdl);

                    gv.PageIndex = gv.PageCount;
                    gv.DataSource = lstWorkType;
                    gv.DataBind();

                    gv.EditIndex = gv.Rows.Count - 1;
                    gv.DataBind();
                    gv.Rows[gv.Rows.Count - 1].FindControl("txtName").Focus();
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
                gv.PageIndex = e.NewPageIndex;
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
                long id = Convert.ToInt32(((Label)gv.Rows[e.RowIndex].FindControl("lblID")).Text);

                EC_IndustryType mdl = new EC_IndustryType(); mdl.ID = id;

                // check for closure work type associated with other modules 
                if ((bool)bll_EWC.Operations_IndustryType(Constants.CHECK_ASSOCIATION, mdl))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //delete record after all varification
                if ((bool)bll_EWC.Operations_IndustryType(Constants.CRUD_DELETE, mdl))
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
                EC_IndustryType mdl = new EC_IndustryType(); 

                GridViewRow row = gv.Rows[e.RowIndex];
                mdl.ID = Convert.ToInt32(((Label)row.Cells[0].FindControl("lblID")).Text);
                mdl.Name = ((TextBox)row.Cells[1].FindControl("txtName")).Text.Trim();
                mdl.IsActive = ((CheckBox)row.FindControl("cb_Active")).Checked; 
                mdl.Description = ((TextBox)row.Cells[2].FindControl("txtDesc")).Text.Trim();

                //Check if Name already exists
                EC_IndustryType obj = (EC_IndustryType)bll_EWC.Operations_IndustryType(Constants.CRUD_READ, mdl);
                if (obj != null && mdl.ID != obj.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                } 

                bool status = false;
                if (mdl.ID == 0) // Add scenario
                {
                    mdl.CreatedBy = (int) SessionManagerFacade.UserInformation.ID;
                    mdl.CreatedDate = DateTime.Now;

                    status = (bool)bll_EWC.Operations_IndustryType(Constants.CRUD_CREATE, mdl);
                }
                else //Edit Scenario
                {
                    mdl.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    mdl.ModifiedDate = DateTime.Now;

                    status = (bool)bll_EWC.Operations_IndustryType(Constants.CRUD_UPDATE, mdl);
                }

                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (mdl.ID == 0)
                    {
                        gv.PageIndex = 0;
                    }
                    gv.EditIndex = -1;
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
                gv.EditIndex = -1;
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
                gv.EditIndex = e.NewEditIndex;
                BindGrid();
                gv.Rows[e.NewEditIndex].FindControl("txtName").Focus();

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
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }  
    }
}