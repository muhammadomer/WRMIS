using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class ViewOffenders : BasePage
    {
        List<WT_WaterTheftOffender> lstOffenders = new List<WT_WaterTheftOffender>();

        #region ViewStates
        public string AA_VS = "AA";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState[AA_VS] = Convert.ToInt32(Request.QueryString["AA"]);

                hlBack.NavigateUrl = string.Format("~/Modules/WaterTheft/WaterTheftIncidentWorking.aspx?WaterTheftID=" + Convert.ToInt64(Request.QueryString["WaterTheftID"]) + "&AA=" + Convert.ToInt32(ViewState[AA_VS]));

                WaterTheftCase.GetWaterTheftCaseAssignee(Convert.ToInt64(Request.QueryString["WaterTheftID"]));

                BindGrid();
            }
        }

        public void BindGrid()
        {
            try
            {
                gvOffender.DataSource = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);
                gvOffender.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewOffenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvOffender_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOffender.EditIndex = -1;
                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOffender.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstOffenders = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);

                    WT_WaterTheftOffender Offender = new WT_WaterTheftOffender();

                    Offender.ID = -1;
                    Offender.OffenderName = "";
                    Offender.CNIC = "";
                    Offender.Address = "";
                    lstOffenders.Add(Offender);
                    gvOffender.PageIndex = gvOffender.PageCount;
                    gvOffender.DataSource = lstOffenders;
                    gvOffender.DataBind();
                    gvOffender.EditIndex = gvOffender.Rows.Count - 1;
                    gvOffender.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOffender_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOffender.EditIndex = -1;
                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOffender_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOffender.EditIndex = e.NewEditIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long ID = Convert.ToInt32(((Label)gvOffender.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool result = new WaterTheftBLL().DeleteOffender(ID, WaterTheftCase.WaterTheftID);
                if (result)
                {
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
                else
                    Master.ShowMessage(Message.RoleNotDeleted.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RoleNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int ID = Convert.ToInt32(((Label)gvOffender.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                string Name = ((TextBox)gvOffender.Rows[rowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string CNIC = ((TextBox)gvOffender.Rows[rowIndex].Cells[2].FindControl("txtCNIC")).Text.Trim();
                string Address = ((TextBox)gvOffender.Rows[rowIndex].Cells[2].FindControl("txtAddress")).Text.Trim();

                if (Name != "" && CNIC != "" && Address != "")
                {
                    WT_WaterTheftOffender ObjSave = new WT_WaterTheftOffender();
                    ObjSave.WaterTheftID = WaterTheftCase.WaterTheftID;
                    ObjSave.OffenderName = Name;
                    ObjSave.CNIC = CNIC;
                    ObjSave.Address = Address;

                    if (ID == -1)  // add new record
                    {
                        new WaterTheftBLL().AddOffender(ObjSave);
                        gvOffender.PageIndex = 0;
                        gvOffender.EditIndex = -1;
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else  // update record 
                    {
                        ObjSave.ID = ID;
                        new WaterTheftBLL().UpdateOffender(ObjSave);
                        gvOffender.PageIndex = 0;
                        gvOffender.EditIndex = -1;
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (SessionManagerFacade.UserInformation.DesignationID.Value == WaterTheftCase.AssignedToDesignationID)
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                    if (mdlRoleRights != null)
                    {
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                            if (btnAdd != null)
                            {
                                btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                            }
                        }
                        else if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                            LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                            if (btnEdit != null)
                            {
                                btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                            }

                            if (btnDelete != null)
                            {
                                btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                            }
                        }
                    }

                }
                else  // it is only view case 
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = false;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = false;
                        }
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