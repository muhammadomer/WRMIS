

using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class FundsRleaseType : BasePage
    {
        ReferenceDataBLL ACBLL = new ReferenceDataBLL();

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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvFundsReleaseType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label lblDescription = (Label)gvFundsReleaseType.Rows[e.NewEditIndex].FindControl("lblDescription");
                gvFundsReleaseType.EditIndex = e.NewEditIndex;
                BindGrid();
                //GridViewRow row = gvFundsReleaseType.Rows[e.NewEditIndex];
                //DropDownList ddlReleaseType = (DropDownList)row.FindControl("ddlReleaseType");
                //TextBox txtTypeName = (TextBox)row.FindControl("txtTypeName");
                //if (ddlReleaseType != null)
                //{
                //    Dropdownlist.BindDropdownlist<List<object>>(ddlReleaseType, CommonLists.FundsReleaseType(),(int)Constants.DropDownFirstOption.NoOption);
                //    if (!string.IsNullOrEmpty(lblDescription.Text))
                //        Dropdownlist.SetSelectedText(ddlReleaseType, lblDescription.Text);
                //}
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundsReleaseType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<object> lstUnits = ACBLL.GetFundsReleaseType();
                object mdlObj = new { ID = 0, TypeName = "", Description = "", isEdit = "" };

                lstUnits.Add(mdlObj);

                gvFundsReleaseType.PageIndex = gvFundsReleaseType.PageCount;
                gvFundsReleaseType.DataSource = lstUnits;
                gvFundsReleaseType.DataBind();

                gvFundsReleaseType.EditIndex = gvFundsReleaseType.Rows.Count - 1;
                gvFundsReleaseType.DataBind();

                //GridViewRow row = gvFundsReleaseType.Rows[gvFundsReleaseType.Rows.Count - 1];
                //DropDownList ddlReleaseType = (DropDownList)row.FindControl("ddlReleaseType");
                //if (ddlReleaseType != null)
                //{
                //    Dropdownlist.BindDropdownlist<List<object>>(ddlReleaseType, CommonLists.FundsReleaseType(), (int)Constants.DropDownFirstOption.Select);
                //}
            }

        }

        protected void gvFundsReleaseType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvFundsReleaseType.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            List<object> lstAS = ACBLL.GetFundsReleaseType();
            Session["FundsReleaseType"] = lstAS;
            gvFundsReleaseType.DataSource = lstAS;
            gvFundsReleaseType.DataBind();
        }

        protected void gvFundsReleaseType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                DataKey key = gvFundsReleaseType.DataKeys[e.RowIndex];
                long ModifiedBy = SessionManagerFacade.UserInformation.ID;
                long ID = Convert.ToInt64(key.Values["ID"]);
                string txtDescription = ((TextBox)gvFundsReleaseType.Rows[RowIndex].FindControl("txtDescription")).Text;
                string TypeName = Convert.ToString(((TextBox)gvFundsReleaseType.Rows[RowIndex].FindControl("txtTypeName")).Text);


                AT_FundReleaseTypes frt = new AT_FundReleaseTypes();
                frt.ID = ID;
                frt.TypeName = TypeName;
                frt.Description = txtDescription;
                frt.IsEdit = true;
                AT_FundReleaseTypes Unique = ACBLL.GetFundsReleaseTypeByName(frt);

                if (Unique == null)
                {

                    frt.CreatedDate = DateTime.Now;
                    frt.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    if (ACBLL.SaveUpdateFundsReleaseType(frt))
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    gvFundsReleaseType.EditIndex = -1;
                    BindGrid();
                }
                else
                {
                    if (Unique.ID == ID)
                    {
                        frt.ModifiedDate = DateTime.Now;
                        frt.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                        if (ACBLL.SaveUpdateFundsReleaseType(frt))
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        }
                        else
                        {
                            Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                        }
                        gvFundsReleaseType.EditIndex = -1;
                        BindGrid();
                    }
                    else
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    }

                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundsReleaseType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                AT_FundReleaseTypes mdl = new AT_FundReleaseTypes();
                mdl.ID = Convert.ToInt64(((Label)gvFundsReleaseType.Rows[e.RowIndex].FindControl("lblID")).Text);
                AT_FundRelease fr = ACBLL.GetFundRelease(mdl.ID);
                if (fr == null || fr.ID == 0)
                {
                    bool IsDeleted = ACBLL.DeleteFundsReleaseType(mdl);
                    if (IsDeleted)
                    {
                        Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                        BindGrid();
                    }
                    else
                        Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundsReleaseType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region "Data Keys"
                    DataKey key = gvFundsReleaseType.DataKeys[e.Row.RowIndex];
                    bool IsEdit = Convert.ToBoolean(key.Values["IsEdit"]);

                    #endregion "Data Keys"

                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    if (!IsEdit)
                    {
                        btnDelete.Enabled = false;
                        btnEdit.Enabled = false;
                    }
                    else
                    {
                        btnDelete.Enabled = true;
                        btnEdit.Enabled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}