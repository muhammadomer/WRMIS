using PMIU.WRMIS.BLL.WaterTheft;
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

namespace PMIU.WRMIS.Web.Modules.WaterTheft.ReferenceData
{
    public partial class TypeOfOffence : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindOffenceTypeGridView();
                   // btnAddType.Visible = base.CanAdd;
                    
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ReferenceData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region "Offence Type GridView Events"
        private void BindOffenceTypeGridView()
        {
            try
            {
                // List<WT_OffenceType> lstOffenceType = new WaterTheftBLL().GetOffenceType();
                string OffenceSite = Constants.ChannelorOutlet.C.ToString(); // "C";
                List<object> lstTheftType = new WaterTheftBLL().GetAllTheftType(OffenceSite);
                gvOffenceType.DataSource = lstTheftType;
                gvOffenceType.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // string OffenceSite = PMIU.WRMIS.Common.Constants.WTOffenceSite.Channel.ToString();
                if (e.CommandName == "AddTypeOffenceType")
                {
                    List<object> lstOffenceType = new WaterTheftBLL().GetOffenceType();

                    lstOffenceType.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,

                    });

                    gvOffenceType.PageIndex = gvOffenceType.PageCount;
                    gvOffenceType.DataSource = lstOffenceType;
                    gvOffenceType.DataBind();

                    gvOffenceType.EditIndex = gvOffenceType.Rows.Count - 1;
                    gvOffenceType.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOffenceType.PageIndex = e.NewPageIndex;
                gvOffenceType.EditIndex = -1;
                BindOffenceTypeGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOffenceType.EditIndex = -1;
                BindOffenceTypeGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
            
                gvOffenceType.EditIndex = e.NewEditIndex;
                BindOffenceTypeGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long UserId = (long)Session[SessionValues.UserID];
                GridViewRow row = gvOffenceType.Rows[e.RowIndex];
                TextBox txtOffenceType = (TextBox)row.FindControl("txtOffenceType");
                string OffenceID = Convert.ToString(gvOffenceType.DataKeys[e.RowIndex].Values[0]);

                WT_OffenceType OffenceType = new WT_OffenceType();

                OffenceType.ID = Convert.ToInt64(OffenceID);

                bool IsExist = new WaterTheftBLL().IsExistOffenceType(txtOffenceType.Text);
                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }


                if (txtOffenceType != null)
                    OffenceType.Name = txtOffenceType.Text;
                OffenceType.Description = txtOffenceType.Text;
                OffenceType.ChannelOutlet = Constants.ChannelorOutlet.C.ToString(); // "C";
                OffenceType.IsActive = true;
                OffenceType.CreatedBy = UserId;
                OffenceType.CreatedDate = DateTime.Now;

                bool IsSaved = new WaterTheftBLL().SaveOffenceType(OffenceType);

                if (IsSaved)
                {
                    if (Convert.ToInt64(OffenceID) == 0)
                        gvOffenceType.PageIndex = 0;

                    gvOffenceType.EditIndex = -1;
                    BindOffenceTypeGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string OffenceID = Convert.ToString(gvOffenceType.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new WaterTheftBLL().DeleteOffenceType(Convert.ToInt64(OffenceID));
                if (IsDeleted)
                {
                    BindOffenceTypeGridView();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
                // Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffenceType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long ID = Convert.ToInt64(gvOffenceType.DataKeys[e.Row.RowIndex].Value);
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                bool CaseStatus = new WaterTheftBLL().IsTheftTypeExist(ID);
                if (CaseStatus)
                {
                    btnEdit.CssClass += " disabled";
                }
             }
        }

        #endregion "End Offence Type GridView Events"
    }
}