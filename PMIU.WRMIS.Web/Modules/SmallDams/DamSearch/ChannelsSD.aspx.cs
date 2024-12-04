using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common;


using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.SmallDams.Controls;

namespace PMIU.WRMIS.Web.Modules.SmallDams.DamSearch
{
    public partial class ChannelsSD : BasePage
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
                        DamNameType._DAMID = _SmallDamID;
                        hdnSmallDamID.Value = Convert.ToString(_SmallDamID);
                        BindChannelGrid(_SmallDamID);
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
        protected void gvChannels_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannels.PageIndex = e.NewPageIndex;
                gvChannels.EditIndex = -1;
                BindChannelGrid(Convert.ToInt64(hdnSmallDamID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvChannels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void gvChannels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    //ID,ApprovedDate,Cost,Description,CreatedBy,CreatedDate
                    #region "Data Keys"
                    DataKey key = gvChannels.DataKeys[e.Row.RowIndex];
                    Int64 ID = Convert.ToInt64(key.Values["ID"]);
                    string ParentType = Convert.ToString(key.Values["ParentType"]);
                    string ParentName = Convert.ToString(key.Values["ParentName"]);
                    string ChannelName = Convert.ToString(key.Values["ChannelName"]);
                    string ChannelCode = Convert.ToString(key.Values["ChannelCode"]);
                    double ChannelCapacity = Convert.ToDouble(key.Values["ChannelCapacity"]);
                    string OffTakingRD = Convert.ToString(key.Values["OffTakingRD"]);
                    string OffTakingSide = Convert.ToString(key.Values["OffTakingSide"]);
                    string TailRD = Convert.ToString(key.Values["TailRD"]);
                    double DesignedCCA = Convert.ToDouble(key.Values["DesignedCCA"]);
                  
                    #endregion

                    #region "Controls"
                    Label lblParentType = (Label)e.Row.FindControl("lblParentType");
                    Label lblParentName = (Label)e.Row.FindControl("lblParentName");
                    Label lblChannelCode = (Label)e.Row.FindControl("lblChannelCode");
                    Label lblChannelName = (Label)e.Row.FindControl("lblChannelName");
                    Label lblChannelCapacity = (Label)e.Row.FindControl("lblChannelCapacity");
                    Label lblOffTakingRD = (Label)e.Row.FindControl("lblOffTakingRD");
                    Label lblOffTakingSide = (Label)e.Row.FindControl("lblOffTakingSide");
                    Label lblTailRD = (Label)e.Row.FindControl("lblTailRD");
                    Label lblDesignedCCA = (Label)e.Row.FindControl("lblDesignedCCA");

                    #endregion

                    //if (gvChannels.EditIndex == e.Row.RowIndex)
                    //{
                    lblParentType.Text = Convert.ToString(ParentType);
                    lblParentName.Text = Convert.ToString(ParentName);
                        lblChannelCode.Text = Convert.ToString(ChannelCode);
                        lblChannelName.Text = Convert.ToString(ChannelName);
                        lblChannelCapacity.Text = Convert.ToString(ChannelCapacity);
                        lblOffTakingRD.Text = Convert.ToString(OffTakingRD);
                        lblOffTakingSide.Text = Convert.ToString(OffTakingSide);
                        lblTailRD.Text = Convert.ToString(TailRD);
                        lblDesignedCCA.Text = Convert.ToString(DesignedCCA);


                        //if (!string.IsNullOrEmpty(description))
                        //    txtDescription.Text = description;
                    //}
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvChannels.DataKeys[e.RowIndex].Values[0]);

                if(new SmallDamsBLL().ISChannelDependancyExits(Convert.ToInt64(ID)))
                {
                     Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsDeleted = new SmallDamsBLL().DeleteChannels(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindChannelGrid(Convert.ToInt64(hdnSmallDamID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvChannels_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvChannels_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

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

        private void BindChannelGrid(Int64 _SmallDamID)
        {
            try
            {
                List<object> lstOMCostType = new SmallDamsBLL().GetChannels(_SmallDamID);

                gvChannels.DataSource = lstOMCostType;
                gvChannels.DataBind();

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