using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class ChannelParentFeeder : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long channelID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]))
                    {
                        channelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        ChannelDetails.ChannelID = channelID;
                        hdnChannelID.Value = Convert.ToString(channelID);
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}", channelID);
                        BindChannelParentFeederGridview(channelID);
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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelParentFeeder);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindChannelParentFeederGridview(long _ChannelID)
        {
            List<object> lstParentFeederChannel = new ChannelBLL().GetChannelParentFeedersByChannelID(_ChannelID);
            gvParentChannelsFeeders.DataSource = lstParentFeederChannel;
            gvParentChannelsFeeders.DataBind();
        }
        protected void gvParentChannelsFeeders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstChannelParentFeeder = new ChannelBLL().GetChannelParentFeedersByChannelID(Convert.ToInt64(hdnChannelID.Value));
                    lstChannelParentFeeder.Add(new
                    {
                        ID = 0,
                        ParentFeederChannelID = -1,
                        ParentFeederChannelName = string.Empty,
                        RelationshipTypeID = string.Empty,
                        RelationshipTypeName = string.Empty,
                        SideID = string.Empty,
                        SideName = string.Empty,
                        ChannelRD = string.Empty,
                        TotalChannelRD = string.Empty,
                        TotalParentFeederChannelRD = string.Empty,
                        ParentFeederChannelRD = string.Empty,
                        StructureTypeID = 0
                    });

                    gvParentChannelsFeeders.PageIndex = gvParentChannelsFeeders.PageCount;
                    gvParentChannelsFeeders.DataSource = lstChannelParentFeeder;
                    gvParentChannelsFeeders.DataBind();

                    gvParentChannelsFeeders.EditIndex = gvParentChannelsFeeders.Rows.Count - 1;
                    gvParentChannelsFeeders.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvParentChannelsFeeders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string relationShip = string.Empty;
            string channelLeftRD = string.Empty;
            string channelRightRD = string.Empty;
            string parentFeederLeftRD = string.Empty;
            string parentFeederRightRD = string.Empty;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvParentChannelsFeeders.EditIndex == e.Row.RowIndex)
                {

                    #region "Datakeys"
                    DataKey key = gvParentChannelsFeeders.DataKeys[e.Row.RowIndex];

                    string id = Convert.ToString(key.Values["ID"]);
                    string parentFeederChannelID = Convert.ToString(key.Values["ParentFeederChannelID"]);
                    string relationshipTypeID = Convert.ToString(key.Values["RelationshipTypeID"]);
                    string sideID = Convert.ToString(key.Values["SideID"]);
                    string channelRD = Convert.ToString(key.Values["TotalChannelRD"]);
                    string channelParentFeederRD = Convert.ToString(key.Values["TotalParentFeederChannelRD"]);
                    string structureTypeID = Convert.ToString(key.Values["StructureTypeID"]);
                    // 
                    string parentFeederID = parentFeederChannelID + ";" + structureTypeID;
                    #endregion

                    #region "Controls"
                    DropDownList ddlChannelParentFeeder = (DropDownList)e.Row.FindControl("ddlChannelParentFeeder");
                    DropDownList ddlRelationShip = (DropDownList)e.Row.FindControl("ddlRelationShip");
                    DropDownList ddlSide = (DropDownList)e.Row.FindControl("ddlSide");

                    TextBox txtLeftChannelRD = (TextBox)e.Row.FindControl("txtLeftChannelRD");
                    TextBox txtRightChannelRD = (TextBox)e.Row.FindControl("txtRightChannelRD");
                    TextBox txtLeftParentFeederRD = (TextBox)e.Row.FindControl("txtLeftParentFeederRD");
                    TextBox txtRightParentFeederRD = (TextBox)e.Row.FindControl("txtRightParentFeederRD");

                    Label lblChannelRD = (Label)e.Row.FindControl("lblChannelRD");
                    Label lblParentFeederRD = (Label)e.Row.FindControl("lblParentFeederChannelRD");

                    Panel pnlChannelRD = (Panel)e.Row.FindControl("pnlChannelRD");
                    Panel pnlParentFeederRD = (Panel)e.Row.FindControl("pnlParentFeederRD");

                    #endregion

                    if (ddlChannelParentFeeder != null)
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlChannelParentFeeder, new ChannelBLL().GetChannelStructures(Convert.ToInt64(hdnChannelID.Value)));
                        Dropdownlist.SetSelectedValue(ddlChannelParentFeeder, parentFeederID);
                    }

                    if (ddlRelationShip != null)
                    {
                        if (Convert.ToInt32(parentFeederID.Split(';')[1]) != 0 && Convert.ToInt32(parentFeederID.Split(';')[1]) != (int)Constants.StructureType.Channel)
                        {
                            Dropdownlist.BindDropdownlist<List<object>>(ddlRelationShip, CommonLists.GetChannelRelationshipTypes(null, true));
                        }
                        else
                            Dropdownlist.BindDropdownlist<List<object>>(ddlRelationShip, CommonLists.GetChannelRelationshipTypes());

                        Dropdownlist.SetSelectedValue(ddlRelationShip, relationshipTypeID);
                    }

                    if (ddlSide != null)
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlSide, CommonLists.GetChannelSides(null, true));
                        Dropdownlist.SetSelectedValue(ddlSide, sideID);
                    }
                    // Check RD at chennel is not empty
                    if (channelRD != null && channelRD != string.Empty)
                    {
                        Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(channelRD));
                        channelLeftRD = tupleFromRD.Item1; // Channel left textbox RD
                        channelRightRD = tupleFromRD.Item2; // Channel right textbox RD
                    }

                    if (txtLeftChannelRD != null)
                        txtLeftChannelRD.Text = channelLeftRD;
                    if (txtRightChannelRD != null)
                        txtRightChannelRD.Text = channelRightRD;

                    // Check RD at parent feeder channelk is not empty
                    if (!string.IsNullOrEmpty(channelParentFeederRD))
                    {
                        Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(channelParentFeederRD));
                        parentFeederLeftRD = tupleToRD.Item1; // Parent Feeder left Textbox RD
                        parentFeederRightRD = tupleToRD.Item2; // Parent Feeder right Textbox RD
                    }
                    if (txtLeftParentFeederRD != null)
                        txtLeftParentFeederRD.Text = parentFeederLeftRD;
                    if (txtRightParentFeederRD != null)
                        txtRightParentFeederRD.Text = parentFeederRightRD;

                    // In case of Dam/Barrage/HeadWorks as parent
                    if (Convert.ToInt32(ddlChannelParentFeeder.SelectedItem.Value.Split(';')[1]) != (int)Constants.StructureType.Channel)
                    {
                        pnlParentFeederRD.CssClass = "hidden";
                        pnlChannelRD.CssClass = "hidden";
                        lblParentFeederRD.CssClass = "block";
                        lblParentFeederRD.Text = "0+0";
                        Tuple<string, string> tupleTotalRDs = Calculations.GetRDValues(Convert.ToDouble(channelRD));

                        lblChannelRD.CssClass = tupleTotalRDs.Item1 + "+" + tupleTotalRDs.Item2;
                    }
                    // Channel only
                    else
                    {

                        if (ddlRelationShip != null)
                            relationShip = Convert.ToString(ddlRelationShip.SelectedItem.Value);

                        Constants.ChannelRelation relation = (Constants.ChannelRelation)Enum.Parse(typeof(Constants.ChannelRelation), relationShip);

                        switch (relation)
                        {
                            case Constants.ChannelRelation.P:

                                pnlParentFeederRD.CssClass = "block";
                                lblParentFeederRD.CssClass = "hidden";
                                pnlChannelRD.CssClass = "hidden";
                                lblChannelRD.CssClass = "block";

                                break;
                            case Constants.ChannelRelation.F:

                                pnlChannelRD.CssClass = "block";
                                lblChannelRD.CssClass = "hidden";
                                pnlParentFeederRD.CssClass = "hidden";
                                lblParentFeederRD.CssClass = "block";

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvParentChannelsFeeders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvParentChannelsFeeders.EditIndex = -1;
                BindChannelParentFeederGridview(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvParentChannelsFeeders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvParentChannelsFeeders.EditIndex = e.NewEditIndex;
                BindChannelParentFeederGridview(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvParentChannelsFeeders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvParentChannelsFeeders.DataKeys[e.RowIndex].Values[0]);

                bool isDeleted = new ChannelBLL().DeleteChannelParentFeeder(Convert.ToInt64(ID));
                if (isDeleted)
                {
                    BindChannelParentFeederGridview(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvParentChannelsFeeders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvParentChannelsFeeders.Rows[e.RowIndex];
            string[] channelParentFeeder = null;

            try
            {
                #region "Controls"
                DropDownList ddlChannelParentFeeder = (DropDownList)row.FindControl("ddlChannelParentFeeder");
                DropDownList ddlRelationShip = (DropDownList)row.FindControl("ddlRelationShip");
                DropDownList ddlSide = (DropDownList)row.FindControl("ddlSide");

                TextBox txtLeftChannelRD = (TextBox)row.FindControl("txtLeftChannelRD");
                TextBox txtRightChannelRD = (TextBox)row.FindControl("txtRightChannelRD");
                TextBox txtLeftParentFeederRD = (TextBox)row.FindControl("txtLeftParentFeederRD");
                TextBox txtRightParentFeederRD = (TextBox)row.FindControl("txtRightParentFeederRD");
                #endregion

                string ID = Convert.ToString(gvParentChannelsFeeders.DataKeys[e.RowIndex].Values["ID"]);

                CO_ChannelParentFeeder parentFeeder = new CO_ChannelParentFeeder();

                parentFeeder.ID = Convert.ToInt64(ID);

                if (ddlChannelParentFeeder != null)
                {
                    channelParentFeeder = ddlChannelParentFeeder.SelectedItem.Value.Split(';');

                    parentFeeder.ParrentFeederID = Convert.ToInt64(channelParentFeeder[0]);
                    parentFeeder.StructureTypeID = Convert.ToInt64(channelParentFeeder[1]);
                }

                if (ddlRelationShip != null)
                    parentFeeder.RelationType = Convert.ToString(ddlRelationShip.SelectedItem.Value);

                if (ddlSide != null)
                    parentFeeder.ChannelSide = Convert.ToString(ddlSide.SelectedItem.Value);

                if (txtLeftChannelRD != null & txtRightChannelRD != null)
                    parentFeeder.ChannelRDS = Calculations.CalculateTotalRDs(txtLeftChannelRD.Text, txtRightChannelRD.Text);

                if (txtLeftParentFeederRD != null & txtRightParentFeederRD != null)
                    parentFeeder.ParrentFeederRDS = Calculations.CalculateTotalRDs(txtLeftParentFeederRD.Text, txtRightParentFeederRD.Text);


                parentFeeder.ChannelID = Convert.ToInt64(hdnChannelID.Value);

                Constants.ChannelRelation relation = (Constants.ChannelRelation)Enum.Parse(typeof(Constants.ChannelRelation), parentFeeder.RelationType);
                if (Convert.ToInt32(channelParentFeeder[1]) != (int)Constants.StructureType.Channel && relation == Constants.ChannelRelation.F)
                {
                    Master.ShowMessage("Dam/Barrage can not be Feeder.", SiteMaster.MessageType.Error);
                    return;
                }
                if (relation == Constants.ChannelRelation.F)
                {
                    Tuple<int, int> tupleRDs = new ChannelBLL().GetIrrigationBoundariesRDs(Convert.ToInt64(hdnChannelID.Value));
                    if (parentFeeder.ChannelRDS <= tupleRDs.Item1)
                    {
                        Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                        return;
                    }
                }
                if (new ChannelBLL().IsChannelParentFeederExists(parentFeeder.ID, Convert.ToInt64(channelParentFeeder[0]), Convert.ToInt64(channelParentFeeder[1]), parentFeeder.ChannelID.Value))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (Convert.ToInt64(channelParentFeeder[1]) == (int)Constants.StructureType.Channel)
                {
                    if (Convert.ToString(ddlRelationShip.SelectedItem.Value) == Convert.ToString(Constants.ChannelRelation.P))
                    {
                        Tuple<int, int> tupleRDs = new ChannelBLL().GetIrrigationBoundariesRDs(Convert.ToInt64(channelParentFeeder[0]));

                        if (!(tupleRDs.Item1 <= parentFeeder.ParrentFeederRDS && tupleRDs.Item2 >= parentFeeder.ParrentFeederRDS))
                        {
                            Master.ShowMessage("Parent/Feeder R.Ds are out of range", SiteMaster.MessageType.Error);
                            return;
                        }
                        if (new ChannelBLL().IsChannelSideAndParrentFeederRDSExists(parentFeeder.ChannelSide, parentFeeder.ParrentFeederRDS))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    else if (Convert.ToString(ddlRelationShip.SelectedItem.Value) == Convert.ToString(Constants.ChannelRelation.F))
                    {
                        Tuple<int, int> tupleRDs = new ChannelBLL().GetIrrigationBoundariesRDs(Convert.ToInt64(hdnChannelID.Value));

                        if (!(tupleRDs.Item1 <= parentFeeder.ChannelRDS && tupleRDs.Item2 >= parentFeeder.ChannelRDS))
                        {
                            Master.ShowMessage("Channel R.Ds are out of range", SiteMaster.MessageType.Error);
                            return;
                        }
                        if (new ChannelBLL().IsChannelSideAndRDExists(parentFeeder.ChannelSide, parentFeeder.ChannelRDS))
                        {
                            Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                }

                bool isSaved = new ChannelBLL().SaveChannelParentFeeder(parentFeeder);
                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ID) == 0)
                        gvParentChannelsFeeders.PageIndex = 0;

                    gvParentChannelsFeeders.EditIndex = -1;
                    BindChannelParentFeederGridview(parentFeeder.ChannelID.Value);
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void FindChannelParentFeederRDs(string[] channelParentFeeder, DropDownList ddlRelationship, TextBox txtLeftChannelRD, TextBox txtRightChannelRD,
          TextBox txtLeftParentFeederRD, TextBox txtRightParentFeederRD, Label lblChannelRD, Label lblParentFeederRD, Panel pnlChannelRD, Panel pnlParentFeederRD)
        {
            double rdAtChannel = 0;
            double rdAtParentFeeder = 0;
            string relationShip = string.Empty;
            Tuple<string, string> tupleTotalRDs = null;

            if (Convert.ToInt32(channelParentFeeder[1]) != (int)Constants.StructureType.Channel)
            {
                rdAtChannel = new ChannelBLL().GetRDAtChannel(Convert.ToInt64(hdnChannelID.Value));
                tupleTotalRDs = Calculations.GetRDValues(rdAtChannel);

                pnlParentFeederRD.CssClass = "hidden";
                pnlChannelRD.CssClass = "hidden";

                txtLeftChannelRD.Text = tupleTotalRDs.Item1;
                txtRightChannelRD.Text = tupleTotalRDs.Item2;
                txtLeftParentFeederRD.Text = "0";
                txtRightParentFeederRD.Text = "0";

                lblChannelRD.Text = Calculations.GetRDText(rdAtChannel);
                lblParentFeederRD.Text = "0+0";

                lblParentFeederRD.CssClass = "block";
                lblChannelRD.CssClass = "block";
            }
            else
            {
                if (ddlRelationship != null)
                    relationShip = Convert.ToString(ddlRelationship.SelectedItem.Value);

                Constants.ChannelRelation relation = (Constants.ChannelRelation)Enum.Parse(typeof(Constants.ChannelRelation), relationShip);

                switch (relation)
                {
                    case Constants.ChannelRelation.P:

                        rdAtChannel = new ChannelBLL().GetRDAtChannel(Convert.ToInt64(hdnChannelID.Value));
                        tupleTotalRDs = Calculations.GetRDValues(rdAtChannel);

                        pnlParentFeederRD.CssClass = "block";
                        lblParentFeederRD.CssClass = "hidden";
                        pnlChannelRD.CssClass = "hidden";
                        lblChannelRD.CssClass = "block";

                        lblChannelRD.Text = Calculations.GetRDText(rdAtChannel);

                        txtLeftChannelRD.Text = tupleTotalRDs.Item1;
                        txtRightChannelRD.Text = tupleTotalRDs.Item2;

                        break;
                    case Constants.ChannelRelation.F:

                        dynamic channelTotalRD = new ChannelBLL().GetChannelTotalRD(Convert.ToInt64(channelParentFeeder[0]));
                        string channelTotalsRD = Convert.ToString(channelTotalRD.GetType().GetProperty("ChannelTotalRDs").GetValue(channelTotalRD, null));

                        double.TryParse(channelTotalsRD, out rdAtParentFeeder);

                        tupleTotalRDs = Calculations.GetRDValues((double)rdAtParentFeeder);

                        pnlChannelRD.CssClass = "block";
                        lblChannelRD.CssClass = "hidden";
                        pnlParentFeederRD.CssClass = "hidden";
                        lblParentFeederRD.CssClass = "block";

                        lblParentFeederRD.Text = Calculations.GetRDText(rdAtParentFeeder);


                        txtLeftParentFeederRD.Text = tupleTotalRDs.Item1;
                        txtRightParentFeederRD.Text = tupleTotalRDs.Item2;

                        break;
                }
            }
        }
        protected void ddlRelationShip_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] channelParentFeeder = null;
            string relationShip = string.Empty;

            RestFieldsOnSelectedIndexChange(sender, e);

            DropDownList ddlRelationship = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlRelationship.NamingContainer;

            if (gvRow != null)
            {
                DropDownList ddlChannelParentFeeder = (DropDownList)gvRow.FindControl("ddlChannelParentFeeder");
                TextBox txtLeftChannelRD = (TextBox)gvRow.FindControl("txtLeftChannelRD");
                TextBox txtRightChannelRD = (TextBox)gvRow.FindControl("txtRightChannelRD");
                TextBox txtLeftParentFeederRD = (TextBox)gvRow.FindControl("txtLeftParentFeederRD");
                TextBox txtRightParentFeederRD = (TextBox)gvRow.FindControl("txtRightParentFeederRD");

                Label lblChannelRD = (Label)gvRow.FindControl("lblChannelRD");
                Label lblParentFeederRD = (Label)gvRow.FindControl("lblParentFeederChannelRD");

                Panel pnlChannelRD = (Panel)gvRow.FindControl("pnlChannelRD");
                Panel pnlParentFeederRD = (Panel)gvRow.FindControl("pnlParentFeederRD");

                if (ddlChannelParentFeeder != null)
                    channelParentFeeder = Convert.ToString(ddlChannelParentFeeder.SelectedItem.Value).Split(';');

                if (channelParentFeeder != null && channelParentFeeder.Length == 2)
                {
                    try
                    {
                        if (ddlRelationship != null)
                            relationShip = Convert.ToString(ddlRelationship.SelectedItem.Value);

                        Constants.ChannelRelation relation = (Constants.ChannelRelation)Enum.Parse(typeof(Constants.ChannelRelation), relationShip);

                        if (Convert.ToInt32(channelParentFeeder[1]) != (int)Constants.StructureType.Channel && relation == Constants.ChannelRelation.F)
                        {
                            Master.ShowMessage("Dam/Barrage can not be Feeder.", SiteMaster.MessageType.Error);
                            ddlRelationship.SelectedItem.Value = Convert.ToString(Constants.ChannelRelation.P);
                            return;
                        }
                        else
                            FindChannelParentFeederRDs(channelParentFeeder, ddlRelationship, txtLeftChannelRD, txtRightChannelRD
                                , txtLeftParentFeederRD, txtRightParentFeederRD, lblChannelRD, lblParentFeederRD, pnlChannelRD, pnlParentFeederRD);
                    }
                    catch (Exception ex)
                    {
                        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                    }
                }
            }
        }
        protected void ddlChannelParentFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlChannelParentFeeder = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlChannelParentFeeder.NamingContainer;

            if (gvRow != null)
            {

                DropDownList ddlRelationShip = (DropDownList)gvRow.FindControl("ddlRelationShip");

                // Clear all items from relationship dropdownlist.
                ddlRelationShip.Items.Clear();

                if (Convert.ToInt32(ddlChannelParentFeeder.SelectedItem.Value.Split(';')[1]) != (int)Constants.StructureType.Channel)
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlRelationShip, CommonLists.GetChannelRelationshipTypes(null, true));
                }
                else
                    Dropdownlist.BindDropdownlist<List<object>>(ddlRelationShip, CommonLists.GetChannelRelationshipTypes());

                ddlRelationShip.SelectedIndex = 0;

                RestFieldsOnSelectedIndexChange(sender, e);
            }
        }
        private void RestFieldsOnSelectedIndexChange(object sender, EventArgs e)
        {
            DropDownList ddlChannelParentFeeder = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlChannelParentFeeder.NamingContainer;

            if (gvRow != null)
            {
                DropDownList ddlSide = (DropDownList)gvRow.FindControl("ddlSide");

                TextBox txtLeftChannelRD = (TextBox)gvRow.FindControl("txtLeftChannelRD");
                TextBox txtRightChannelRD = (TextBox)gvRow.FindControl("txtRightChannelRD");
                TextBox txtLeftParentFeederRD = (TextBox)gvRow.FindControl("txtLeftParentFeederRD");
                TextBox txtRightParentFeederRD = (TextBox)gvRow.FindControl("txtRightParentFeederRD");

                Label lblChannelRD = (Label)gvRow.FindControl("lblChannelRD");
                Label lblParentFeederRD = (Label)gvRow.FindControl("lblParentFeederChannelRD");

                Panel pnlChannelRD = (Panel)gvRow.FindControl("pnlChannelRD");
                Panel pnlParentFeederRD = (Panel)gvRow.FindControl("pnlParentFeederRD");

                ddlSide.SelectedIndex = 0;

                txtLeftChannelRD.Text = string.Empty;
                txtRightChannelRD.Text = string.Empty;
                txtLeftParentFeederRD.Text = string.Empty;
                txtRightParentFeederRD.Text = string.Empty;

                pnlChannelRD.CssClass = "block";
                pnlParentFeederRD.CssClass = "block";

                lblChannelRD.Text = string.Empty;
                lblParentFeederRD.Text = string.Empty;

                lblChannelRD.CssClass = "hidden";
                lblParentFeederRD.CssClass = "hidden";
            }

        }
        protected void gvParentChannelsFeeders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvParentChannelsFeeders.PageIndex = e.NewPageIndex;
                gvParentChannelsFeeders.EditIndex = -1;
                BindChannelParentFeederGridview(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}