using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.AppBlocks;
namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet
{
    public partial class OutletVillage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    string outletID = Request.QueryString["OutletID"];
                    string channelID = Request.QueryString["ChannelID"];
                    string alterationID = Request.QueryString["AlterationID"];

                    if (!string.IsNullOrEmpty(channelID) && !string.IsNullOrEmpty(outletID) && !string.IsNullOrEmpty(alterationID) && !string.IsNullOrEmpty(alterationID))
                    {
                        hdnChannelID.Value = Convert.ToString(channelID);
                        hdnOutletID.Value = Convert.ToString(outletID);
                        hdnAlterationID.Value = Convert.ToString(alterationID);

                        LoadOutletVillages(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value), Convert.ToInt64(hdnAlterationID.Value));

                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID=" + Convert.ToString(hdnChannelID.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OutletVillages);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadOutletVillages(long _ChannelID, long _OutletID, long _AlterationID)
        {
            try
            {
                List<CO_Village> lstAdmin = new IN_OutletBLL().GetVillagesByChannelID(_ChannelID);

                LoadOutletData(_ChannelID, _OutletID, _AlterationID);

                BindOutletVillagesGrid(_ChannelID, _OutletID);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadOutletData(long _ChannelID, long _OutletID, long _AlterationID)
        {
            try
            {
                CO_Channel channel = new ChannelBLL().GetChannelByID(_ChannelID);
                lblChannelName.Text = channel.NAME;
                IN_OutletBLL outLetBLL = new IN_OutletBLL();
                lblChannelType.Text = outLetBLL.GetChannelTypeByID(channel.ChannelTypeID);

                CO_ChannelOutlets OutletData = outLetBLL.GetOutletByOutletID(_OutletID);
                lblOutletRD.Text = Calculations.GetRDText(OutletData.OutletRD);
                switch (OutletData.ChannelSide)
                {
                    case "L":
                        lblOutletSide.Text = Convert.ToString(Constants.ChannelSide.Left);
                        break;
                    case "R":
                        lblOutletSide.Text = Convert.ToString(Constants.ChannelSide.Right);
                        break;
                }

                CO_OutletAlterationHistroy OutletHistoryData = new IN_OutletBLL().GetOutletRefData(_AlterationID);
                lblOutletType.Text = new IN_OutletBLL().GetOLTypeAbbrvByID(OutletHistoryData.OutletTypeID);
                lblDesignDischarge.Text = Convert.ToString(OutletHistoryData.DesignDischarge);
                lblOutletHeight.Text = Convert.ToString(OutletHistoryData.OutletHeight);
                lblOutletCrest.Text = Convert.ToString(OutletHistoryData.OutletCrest);
                lblMMHead.Text = Convert.ToString(OutletHistoryData.OutletMMH);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindOutletVillagesGrid(long _ChannelID, long _OutletID)
        {
            try
            {
                List<object> lstVillages = new IN_OutletBLL().GetOutletVillagesByOutletID(Convert.ToInt64(hdnOutletID.Value));
                gvOutletVillage.DataSource = lstVillages;
                gvOutletVillage.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddOutletVillage")
                {
                    List<object> lstVillages = new IN_OutletBLL().GetOutletVillagesByOutletID(Convert.ToInt64(hdnOutletID.Value));
                    lstVillages.Add(new
                    {
                        ID = 0,
                        VillageID = string.Empty,
                        VillageName = string.Empty,
                        LocatedIn = string.Empty
                    });

                    gvOutletVillage.PageIndex = gvOutletVillage.PageCount;
                    gvOutletVillage.DataSource = lstVillages;
                    gvOutletVillage.DataBind();

                    gvOutletVillage.EditIndex = gvOutletVillage.Rows.Count - 1;
                    gvOutletVillage.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvOutletVillage.EditIndex == e.Row.RowIndex)
                {
                    #region "Datakeys"
                    DataKey key = gvOutletVillage.DataKeys[e.Row.RowIndex];

                    string id = Convert.ToString(key.Values["ID"]);
                    string villageID = Convert.ToString(key.Values["VillageID"]);
                    string locatedIn = Convert.ToString(key.Values["LocatedIn"]);
                    #endregion

                    #region "Controls"
                    DropDownList ddlOutletVillage = (DropDownList)e.Row.FindControl("ddlOutletVillage");
                    DropDownList ddlOutletLocatedIn = (DropDownList)e.Row.FindControl("ddlOutletLocatedIn");
                    #endregion

                    if (ddlOutletVillage != null)
                    {
                        Dropdownlist.BindDropdownlist<List<CO_Village>>(ddlOutletVillage, new IN_OutletBLL().GetVillagesByChannelID(Convert.ToInt64(hdnChannelID.Value)));
                        Dropdownlist.SetSelectedValue(ddlOutletVillage, villageID);
                    }
                    if (ddlOutletLocatedIn != null)
                    {
                        Dropdownlist.DDLYesNo(ddlOutletLocatedIn);
                        Dropdownlist.SetSelectedValue(ddlOutletLocatedIn, Convert.ToInt16(Convert.ToBoolean(locatedIn)).ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOutletVillage.EditIndex = e.NewEditIndex;
                BindOutletVillagesGrid(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOutletVillage.EditIndex = -1;
                BindOutletVillagesGrid(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Int64 ID = Convert.ToInt64(gvOutletVillage.DataKeys[e.RowIndex].Values["ID"]);
                //if (new ChannelBLL().IsGaugeDependanceExists(ID))
                //{
                //    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                //    return;
                //}

                bool isDeleted = new IN_OutletBLL().DeleteChannelOutletsLocation(ID);
                if (isDeleted)
                {
                    BindOutletVillagesGrid(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvOutletVillage.Rows[e.RowIndex];

                DropDownList ddlOutletVillage = (DropDownList)row.FindControl("ddlOutletVillage");
                DropDownList ddlOutletLocatedIn = (DropDownList)row.FindControl("ddlOutletLocatedIn");

                string ID = Convert.ToString(gvOutletVillage.DataKeys[e.RowIndex].Values[0]);

                CO_ChannelOutletsLocation outletLocation = new CO_ChannelOutletsLocation();

                outletLocation.ID = Convert.ToInt64(ID);

                if (ddlOutletVillage != null)
                    outletLocation.VillageID = Convert.ToInt64(ddlOutletVillage.SelectedItem.Value);

                if (ddlOutletLocatedIn != null)
                    outletLocation.LocatedIn = Convert.ToBoolean(Convert.ToInt16(ddlOutletLocatedIn.SelectedItem.Value));

                outletLocation.OutletID = Convert.ToInt64(hdnOutletID.Value);

                if (new IN_OutletBLL().IsOutletVillageExists(outletLocation))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                //Outlet Installed at Village field must have one and only one ‘Yes’ value
                else if (outletLocation.LocatedIn.Value && new IN_OutletBLL().IsOutletInstalled(outletLocation))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                //In case the value of  ‘Outlet Installed at Village’ is ‘Yes’, system will check if the village name is available against Outlet R.D in Administrative Boundaries
                if (outletLocation.LocatedIn.Value && !new IN_OutletBLL().IsVillageNameExistsInAdminBoundaries(Convert.ToInt64(hdnChannelID.Value), outletLocation.VillageID.Value))
                {
                    Master.ShowMessage("Outlet installed at village must exists in Administrative Boundaries.", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new IN_OutletBLL().SaveChannelOutletsLocation(outletLocation);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ID) == 0)
                        gvOutletVillage.PageIndex = 0;

                    gvOutletVillage.EditIndex = -1;
                    BindOutletVillagesGrid(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvOutletVillage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletVillage.PageIndex = e.NewPageIndex;
                gvOutletVillage.EditIndex = -1;
                BindOutletVillagesGrid(Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(hdnOutletID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}