using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;

namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class AnnualOandMCostSD : BasePage
    {

        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _SmallDamID = Utility.GetNumericValueFromQueryString("SmallDamID", 0);
                    if (_SmallDamID > 0)
                    {
                        DamInfo._DAMID = _SmallDamID;
                        hdnSmallDamID.Value = Convert.ToString(_SmallDamID);


                        BindOMCostTypeGrid(_SmallDamID);

                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/SmallDams/DamSearch/SearchSD.aspx?ShowHistroy=1");

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Events
        protected void gvOMCost_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOMCost.PageIndex = e.NewPageIndex;
                gvOMCost.EditIndex = -1;
                BindOMCostTypeGrid(Convert.ToInt64(hdnSmallDamID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOMCost.EditIndex = -1;
                BindOMCostTypeGrid(Convert.ToInt64(hdnSmallDamID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddOMCost")
                {
                    List<object> lstOMCostType = new SmallDamsBLL().GetOMCost(Convert.ToInt64(hdnSmallDamID.Value));
                    lstOMCostType.Add(new
                    {
                        ID = 0,
                        ApprovedDate = Utility.GetFormattedDate(DateTime.Now),
                        Cost = string.Empty,
                        Description = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvOMCost.PageIndex = gvOMCost.PageCount;
                    gvOMCost.DataSource = lstOMCostType;
                    gvOMCost.DataBind();

                    gvOMCost.EditIndex = gvOMCost.Rows.Count - 1;
                    gvOMCost.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    //ID,ApprovedDate,Cost,Description,CreatedBy,CreatedDate
                    #region "Data Keys"
                    DataKey key = gvOMCost.DataKeys[e.Row.RowIndex];
                    Int64 ID = Convert.ToInt64(key.Values["ID"]);
                    string ApprovedDate = Convert.ToString(key.Values["ApprovedDate"]);
                    string cost = Convert.ToString(key.Values["Cost"]);
                    string description = Convert.ToString(key.Values["Description"]);

                    #endregion

                    #region "Controls"

                    TextBox txtApprovedDate = (TextBox)e.Row.FindControl("txtApprovedDate");
                    TextBox txtCost = (TextBox)e.Row.FindControl("txtCost");
                    TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");

                    #endregion

                    if (gvOMCost.EditIndex == e.Row.RowIndex)
                    {
                        txtApprovedDate.Text = String.Format("{0:dd-MMM-yyyy}", ApprovedDate);

                        if (!string.IsNullOrEmpty(cost))
                            txtCost.Text = cost;

                        if (!string.IsNullOrEmpty(description))
                            txtDescription.Text = description;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvOMCost.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new SmallDamsBLL().DeleteOMCost(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindOMCostTypeGrid(Convert.ToInt64(hdnSmallDamID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOMCost.EditIndex = e.NewEditIndex;
                BindOMCostTypeGrid(Convert.ToInt64(hdnSmallDamID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOMCost_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //ID,ApprovedDate,Cost,Description,CreatedBy,CreatedDate
                DataKey key = gvOMCost.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvOMCost.Rows[e.RowIndex];
                TextBox txtApprovedDate = (TextBox)row.FindControl("txtApprovedDate");
                TextBox txtCost = (TextBox)row.FindControl("txtCost");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");

                #endregion

                SD_OMCost OMCostType = new SD_OMCost();

                OMCostType.ID = Convert.ToInt16(ID);
                if (OMCostType.ID == 0)
                {
                    OMCostType.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    OMCostType.CreatedDate = DateTime.Now;
                    OMCostType.ModifiedBy = null;
                    OMCostType.ModifiedDate = null;
                }
                else
                {
                    OMCostType.CreatedBy = Convert.ToInt32(CreatedBy);
                    OMCostType.CreatedDate = Convert.ToDateTime(CreatedDate);
                    OMCostType.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    OMCostType.ModifiedDate = DateTime.Now;
                }

                OMCostType.SmallDamID = Convert.ToInt64(hdnSmallDamID.Value);
                OMCostType.ApprovedDate = Convert.ToDateTime(txtApprovedDate.Text);

                if (!string.IsNullOrEmpty(txtCost.Text))
                    OMCostType.Cost = Convert.ToInt64(txtCost.Text);

                if (!string.IsNullOrEmpty(txtDescription.Text))
                    OMCostType.Description = txtDescription.Text;


                if (new SmallDamsBLL().IsOMCostUnique(OMCostType))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new SmallDamsBLL().SaveOMCost(OMCostType);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(OMCostType.ID) == 0)
                        gvOMCost.PageIndex = 0;

                    gvOMCost.EditIndex = -1;
                    BindOMCostTypeGrid(Convert.ToInt64(hdnSmallDamID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindOMCostTypeGrid(Int64 _SmallDamID)
        {
            try
            {
                List<object> lstOMCostType = new SmallDamsBLL().GetOMCost(_SmallDamID);

                gvOMCost.DataSource = lstOMCostType;
                gvOMCost.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteOMCost");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }
        #endregion



    }
}