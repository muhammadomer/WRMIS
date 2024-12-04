using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System.Web.UI.HtmlControls;
using PMIU.WRMIS.AppBlocks;
namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class ChannelPhysicalLocation : BasePage
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
                        hdnChannelID.Value = Convert.ToString(channelID);
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}", channelID);

                        hdnIsGaugesCalculated.Value = Convert.ToInt16(IsGaugeInformationExists()).ToString();
                        hdnDependanceExists.Value = Convert.ToInt16(IsDependanceExists()).ToString();
                        LoadPhyicalLocation(channelID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private bool IsGaugeInformationExists()
        {
            return new ChannelBLL().IsGaugeInformationExists(Convert.ToInt64(hdnChannelID.Value));

        }
        private bool IsDependanceExists()
        {
            return new ChannelBLL().IsIrrigationBoundariesDependencyExists(Convert.ToInt64(hdnChannelID.Value));
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChannelPhysicalLocation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadPhyicalLocation(long _ChannelID)
        {
            ChannelDetails.ChannelID = _ChannelID;
            BindIrrigationBoundariesGridView(_ChannelID);
            BindAdministrativeBoundariesGridView(_ChannelID);
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteIrrigationBoundaries");

            if (btnDelete != null && Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            {
                btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this record?')) {return confirm('All data would be deleted.')} else return false;";
            }
            else if (btnDelete != null && Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 0)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }

        }
        private void AddEditConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnEdit = (Button)_e.Row.FindControl("btnSaveIrrigationBoundaries");

            if (btnEdit != null && Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            {
                btnEdit.OnClientClick = "return confirm('All Data will be deleted and Gauge will be recalculate. Do you want to proced?');";
            }
        }
        #region "Irrigation Boundaries"

        #region "GridView Events"
        private void BindIrrigationBoundariesGridView(long _ChannelID)
        {
            try
            {
                List<object> lstIrrigationBoundaries = new ChannelBLL().GetIrrigationBoundariesByChannelID(_ChannelID);
                gvIrrigationBoundaries.DataSource = lstIrrigationBoundaries;
                gvIrrigationBoundaries.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddIrrigationBoundaries")
                {
                    List<object> lstIrrigationBoundaries = new ChannelBLL().GetIrrigationBoundariesByChannelID(Convert.ToInt64(hdnChannelID.Value));

                    lstIrrigationBoundaries.Add(
                    new
                    {
                        ID = 0,
                        ZoneName = string.Empty,
                        ZoneID = string.Empty,
                        CircleName = string.Empty,
                        CircleID = string.Empty,
                        DivisionName = string.Empty,
                        DivisionID = string.Empty,
                        SubDivisionName = string.Empty,
                        SubDivisionID = string.Empty,
                        SectionName = string.Empty,
                        SectionID = string.Empty,
                        SectionRD = string.Empty,
                        TotalRDs = string.Empty
                    });

                    gvIrrigationBoundaries.PageIndex = gvIrrigationBoundaries.PageCount;
                    gvIrrigationBoundaries.DataSource = lstIrrigationBoundaries;
                    gvIrrigationBoundaries.DataBind();

                    gvIrrigationBoundaries.EditIndex = gvIrrigationBoundaries.Rows.Count - 1;
                    gvIrrigationBoundaries.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string sectionLeftRD = string.Empty;
            string sectionRightRD = string.Empty;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    if (gvIrrigationBoundaries.EditIndex == e.Row.RowIndex)
                    {
                        if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
                            AddEditConfirmMessage(e);

                        #region "Data Keys"
                        DataKey key = gvIrrigationBoundaries.DataKeys[e.Row.RowIndex];
                        string zoneID = Convert.ToString(key.Values[1]);
                        string circleID = Convert.ToString(key.Values[2]);
                        string divisionID = Convert.ToString(key.Values[3]);
                        string subDivisionID = Convert.ToString(key.Values[4]);
                        string sectionID = Convert.ToString(key.Values[5]);
                        string sectionRD = Convert.ToString(key.Values[6]);
                        #endregion

                        #region "Controls"
                        DropDownList ddlZone = (DropDownList)e.Row.FindControl("ddlZone");
                        DropDownList ddlCircle = (DropDownList)e.Row.FindControl("ddlCircle");
                        DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDivision");
                        DropDownList ddlSubDivision = (DropDownList)e.Row.FindControl("ddlSubDivision");
                        DropDownList ddlSection = (DropDownList)e.Row.FindControl("ddlSection");

                        TextBox txtLeftRD = (TextBox)e.Row.FindControl("txtIrriRDLeft");
                        TextBox txtRightRD = (TextBox)e.Row.FindControl("txtIrriRDRight");
                        #endregion

                        if (ddlZone != null)
                        {
                            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
                            if (!string.IsNullOrEmpty(zoneID))
                                Dropdownlist.SetSelectedValue(ddlZone, zoneID);
                        }

                        if (ddlCircle != null)
                        {
                            if (string.IsNullOrEmpty(ddlZone.SelectedItem.Value))
                            {
                                Dropdownlist.DDLCircles(ddlCircle, true);
                            }
                            else
                            {
                                Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt32(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                                Dropdownlist.SetSelectedValue(ddlCircle, circleID);
                            }
                        }

                        if (ddlDivision != null)
                        {
                            if (string.IsNullOrEmpty(ddlCircle.SelectedItem.Value))
                                Dropdownlist.DDLDivisions(ddlDivision, true);
                            else
                            {
                                Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value), Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.Select, true);
                                Dropdownlist.SetSelectedValue(ddlDivision, divisionID);
                            }

                        }

                        if (ddlSubDivision != null)
                        {
                            if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select, true);
                            }
                            else
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt32(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                                Dropdownlist.SetSelectedValue(ddlSubDivision, subDivisionID);
                            }
                        }

                        if (ddlSection != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select, true);
                            }
                            else
                            {
                                Dropdownlist.DDLSections(ddlSection, false, Convert.ToInt32(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                                Dropdownlist.SetSelectedValue(ddlSection, sectionID);
                            }
                        }

                        if (!string.IsNullOrEmpty(sectionRD))
                        {
                            Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(sectionRD));
                            sectionLeftRD = tupleRD.Item1;
                            sectionRightRD = tupleRD.Item2;
                        }
                        if (txtLeftRD != null)
                            txtLeftRD.Text = sectionLeftRD;
                        if (txtRightRD != null)
                            txtRightRD.Text = sectionRightRD;

                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvIrrigationBoundaries.EditIndex = e.NewEditIndex;
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIrrigationBoundaries.EditIndex = -1;
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvIrrigationBoundaries.Rows[e.RowIndex];

                DropDownList ddlZone = (DropDownList)row.FindControl("ddlZone");
                DropDownList ddlCircle = (DropDownList)row.FindControl("ddlCircle");
                DropDownList ddlDivision = (DropDownList)row.FindControl("ddlDivision");
                DropDownList ddlSubDivision = (DropDownList)row.FindControl("ddlSubDivision");
                DropDownList ddlSection = (DropDownList)row.FindControl("ddlSection");
                TextBox txtLeftRD = (TextBox)row.FindControl("txtIrriRDLeft");
                TextBox txtRightRD = (TextBox)row.FindControl("txtIrriRDRight");

                string irrigationBoundariesID = Convert.ToString(gvIrrigationBoundaries.DataKeys[e.RowIndex].Values[0]);

                CO_ChannelIrrigationBoundaries irrigationBoundaries = new CO_ChannelIrrigationBoundaries();

                irrigationBoundaries.ID = Convert.ToInt64(irrigationBoundariesID);

                if (ddlSection != null)
                    irrigationBoundaries.SectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);

                if (txtLeftRD != null & txtRightRD != null)
                    irrigationBoundaries.SectionRD = Calculations.CalculateTotalRDs(txtLeftRD.Text, txtRightRD.Text);

                irrigationBoundaries.ChannelID = Convert.ToInt64(hdnChannelID.Value);

                if (new ChannelBLL().IsSectionExists(irrigationBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (new ChannelBLL().IsSectionRDsExists(irrigationBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                else if (ChannelDetails.ChannelTotalRDs < irrigationBoundaries.SectionRD)
                {
                    Master.ShowMessage("Section RD can not be greater than Channel Total RDs.", SiteMaster.MessageType.Error);
                    return;
                }

                if (Convert.ToInt16(hdnDependanceExists.Value.ToString()) == 1)
                {
                    Master.ShowMessage("Physical location can not be edited.", SiteMaster.MessageType.Error);
                    return;
                }

                // Update IsCalculated bit in Channel table to recalculate Gauges
                if (!new ChannelBLL().UpdateIsCalculated(Convert.ToInt64(hdnChannelID.Value), true))
                {
                    Master.ShowMessage("Internal server error occured.", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new ChannelBLL().SaveIrrigationBoundaries(irrigationBoundaries);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(irrigationBoundariesID) == 0)
                        gvIrrigationBoundaries.PageIndex = 0;

                    gvIrrigationBoundaries.EditIndex = -1;
                    BindIrrigationBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string irrigationBoundariesID = Convert.ToString(gvIrrigationBoundaries.DataKeys[e.RowIndex].Values[0]);

                if (Convert.ToInt16(hdnDependanceExists.Value.ToString()) == 1)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                // Update IsCalculated bit in Channel table to recalculate Gauges
                if (!new ChannelBLL().UpdateIsCalculated(Convert.ToInt64(hdnChannelID.Value), true))
                {
                    Master.ShowMessage("Internal server error occured.", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new ChannelBLL().DeleteIrrigationBoundaries(Convert.ToInt64(irrigationBoundariesID), Convert.ToInt64(hdnChannelID.Value));
                if (IsDeleted)
                {
                    BindIrrigationBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvIrrigationBoundaries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIrrigationBoundaries.PageIndex = e.NewPageIndex;
                gvIrrigationBoundaries.EditIndex = -1;
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion "End GridView Events"

        #region "Dropdownlists Events"
        private void ResetDDLOnZoneSelectedIndexChange(object sender, EventArgs e)
        {

            DropDownList ddlZone = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlZone.NamingContainer;

            DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");

            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);

            Dropdownlist.DDLSections(ddlSection, true);
        }
        private void ResetDDLOnCircleSelectedIndexChange(object sender, EventArgs e)
        {
            DropDownList ddlCircle = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlCircle.NamingContainer;

            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");

            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);

            Dropdownlist.DDLSections(ddlSection, true);
        }
        private void ResetDDLOnDivisionSelectedIndexChange(object sender, EventArgs e)
        {
            DropDownList ddlDivision = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;

            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");

            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);

            Dropdownlist.DDLSections(ddlSection, true);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnZoneSelectedIndexChange(sender, e);

                DropDownList ddlZone = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlZone.NamingContainer;

                if (gvRow != null)
                {
                    DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
                    Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt32(ddlZone.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnCircleSelectedIndexChange(sender, e);

                DropDownList ddlCircle = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlCircle.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                    Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value), Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnDivisionSelectedIndexChange(sender, e);

                DropDownList ddlDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt32(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSubDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubDivision.NamingContainer;

                if (gvRow != null)
                {
                    DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                    Dropdownlist.DDLSections(ddlSection, false, Convert.ToInt32(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion "End Dropdownlists Events"

        #endregion "End Irrigation Boundaries"

        #region "Administrative Boundaries"

        #region "GridView Events"
        private void BindAdministrativeBoundariesGridView(long _ChannelID)
        {
            try
            {
                List<object> lstAdminBoundaries = new ChannelBLL().GetAdministrativeBoundariesByChannelID(_ChannelID);
                gvAdministrativeBoundaries.DataSource = lstAdminBoundaries;
                gvAdministrativeBoundaries.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddAdminBoundaries")
                {
                    List<object> lstAdminBoundaries = new ChannelBLL().GetAdministrativeBoundariesByChannelID(Convert.ToInt64(hdnChannelID.Value));

                    lstAdminBoundaries.Add(new
                    {
                        ID = 0,
                        DistrictID = string.Empty,
                        DistrictName = string.Empty,
                        TehsilID = string.Empty,
                        TehsilName = string.Empty,
                        PoliceStationID = string.Empty,
                        PoliceStationName = string.Empty,
                        VillageID = string.Empty,
                        VillageName = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        ChannelSide = string.Empty,
                        ChannelSideID = string.Empty
                    });

                    gvAdministrativeBoundaries.PageIndex = gvAdministrativeBoundaries.PageCount;
                    gvAdministrativeBoundaries.DataSource = lstAdminBoundaries;
                    gvAdministrativeBoundaries.DataBind();

                    gvAdministrativeBoundaries.EditIndex = gvAdministrativeBoundaries.Rows.Count - 1;
                    gvAdministrativeBoundaries.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string FromLeftRD = string.Empty;
            string FromRightRD = string.Empty;
            string ToLeftRD = string.Empty;
            string ToRightRD = string.Empty;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvAdministrativeBoundaries.EditIndex == e.Row.RowIndex)
                {
                    #region "Datakeys"
                    DataKey key = gvAdministrativeBoundaries.DataKeys[e.Row.RowIndex];

                    string id = Convert.ToString(key.Values["ID"]);
                    string districtID = Convert.ToString(key.Values["DistrictID"]);
                    string tehsilID = Convert.ToString(key.Values["TehsilID"]);
                    string policeStationID = Convert.ToString(key.Values["PoliceStationID"]);
                    string villageID = Convert.ToString(key.Values["VillageID"]);
                    string fromRD = Convert.ToString(key.Values["FromRDTotal"]);
                    string toRD = Convert.ToString(key.Values["ToRDTotal"]);
                    string channelSide = Convert.ToString(key.Values["ChannelSideID"]);
                    #endregion

                    #region "Controls"
                    DropDownList ddlDistrict = (DropDownList)e.Row.FindControl("ddlDistrict");
                    DropDownList ddlTehsil = (DropDownList)e.Row.FindControl("ddlTehsil");
                    DropDownList ddlPoliceStation = (DropDownList)e.Row.FindControl("ddlPoliceStation");
                    DropDownList ddlVillage = (DropDownList)e.Row.FindControl("ddlVillage");
                    DropDownList ddlChannelSide = (DropDownList)e.Row.FindControl("ddlChannelSide");
                    TextBox txtFromRDLeft = (TextBox)e.Row.FindControl("txtFromRDLeft");
                    TextBox txtFromRDRight = (TextBox)e.Row.FindControl("txtFromRDRight");
                    TextBox txtToRDLeft = (TextBox)e.Row.FindControl("txtToRDLeft");
                    TextBox txtToRDRight = (TextBox)e.Row.FindControl("txtToRDRight");
                    #endregion

                    if (ddlDistrict != null)
                    {
                        Dropdownlist.DDLDistrictByChannelID(ddlDistrict, Convert.ToInt64(hdnChannelID.Value), (int)Constants.DropDownFirstOption.Select, true);
                        Dropdownlist.SetSelectedValue(ddlDistrict, districtID);
                    }
                    if (ddlTehsil != null)
                    {
                        if (string.IsNullOrEmpty(ddlDistrict.SelectedItem.Value))
                        {
                            Dropdownlist.DDLTehsils(ddlTehsil, true, -1, (int)Constants.DropDownFirstOption.Select, true);
                        }
                        else
                        {
                            Dropdownlist.DDLTehsils(ddlTehsil, false, Convert.ToInt32(ddlDistrict.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                            Dropdownlist.SetSelectedValue(ddlTehsil, tehsilID);
                        }
                    }
                    if (ddlPoliceStation != null)
                    {
                        if (string.IsNullOrEmpty(ddlTehsil.SelectedItem.Value))
                        {
                            Dropdownlist.DDLPoliceStations(ddlPoliceStation, true, -1, (int)Constants.DropDownFirstOption.Select, true);
                        }
                        else
                        {
                            Dropdownlist.DDLPoliceStations(ddlPoliceStation, false, Convert.ToInt32(ddlTehsil.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                            Dropdownlist.SetSelectedValue(ddlPoliceStation, policeStationID);
                        }
                    }
                    if (ddlVillage != null)
                    {
                        if (string.IsNullOrEmpty(ddlPoliceStation.SelectedItem.Value))
                        {
                            Dropdownlist.DDLVillages(ddlVillage, true, -1, (int)Constants.DropDownFirstOption.Select, true);
                        }
                        else
                        {
                            Dropdownlist.DDLVillages(ddlVillage, false, Convert.ToInt32(ddlPoliceStation.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                            Dropdownlist.SetSelectedValue(ddlVillage, villageID);
                        }
                    }

                    if (ddlChannelSide != null)
                    {
                        if (string.IsNullOrEmpty(channelSide))
                        {
                            Dropdownlist.DDLChannelSide(ddlChannelSide);
                        }
                        else
                        {
                            Dropdownlist.DDLChannelSide(ddlChannelSide);
                            Dropdownlist.SetSelectedValue(ddlChannelSide, channelSide);
                        }
                    }

                    // Check From RD is not null
                    if (!string.IsNullOrEmpty(fromRD))
                    {
                        Tuple<string, string> tupleFromRD = Calculations.GetRDValues(Convert.ToInt64(fromRD));
                        FromLeftRD = tupleFromRD.Item1;
                        FromRightRD = tupleFromRD.Item2;
                    }

                    if (txtFromRDLeft != null)
                        txtFromRDLeft.Text = FromLeftRD;
                    if (txtFromRDRight != null)
                        txtFromRDRight.Text = FromRightRD;

                    // Check To RD is not null
                    if (!string.IsNullOrEmpty(toRD))
                    {
                        Tuple<string, string> tupleToRD = Calculations.GetRDValues(Convert.ToInt64(toRD));
                        ToLeftRD = tupleToRD.Item1;
                        ToRightRD = tupleToRD.Item2;
                    }
                    if (txtToRDLeft != null)
                        txtToRDLeft.Text = ToLeftRD;
                    if (txtToRDRight != null)
                        txtToRDRight.Text = ToRightRD;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.EditIndex = e.NewEditIndex;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.EditIndex = -1;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvAdministrativeBoundaries.Rows[e.RowIndex];

                DropDownList ddlDistrict = (DropDownList)row.FindControl("ddlDistrict");
                DropDownList ddlTehsil = (DropDownList)row.FindControl("ddlTehsil");
                DropDownList ddlPoliceStation = (DropDownList)row.FindControl("ddlPoliceStation");
                DropDownList ddlVillage = (DropDownList)row.FindControl("ddlVillage");
                DropDownList ddlChannelSide = (DropDownList)row.FindControl("ddlChannelSide");

                TextBox txtFromRDLeft = (TextBox)row.FindControl("txtFromRDLeft");
                TextBox txtFromRDRight = (TextBox)row.FindControl("txtFromRDRight");
                TextBox txtToRDLeft = (TextBox)row.FindControl("txtToRDLeft");
                TextBox txtToRDRight = (TextBox)row.FindControl("txtToRDRight");


                string adminBoundariesID = Convert.ToString(gvAdministrativeBoundaries.DataKeys[e.RowIndex].Values[0]);

                CO_ChannelAdminBoundries adminBoundaries = new CO_ChannelAdminBoundries();

                adminBoundaries.ID = Convert.ToInt64(adminBoundariesID);

                if (ddlVillage != null)
                    adminBoundaries.VillageID = Convert.ToInt64(ddlVillage.SelectedItem.Value);

                if (ddlPoliceStation != null)
                    adminBoundaries.PoliceStationID = Convert.ToInt64(ddlPoliceStation.SelectedItem.Value);

                if (txtFromRDLeft != null & txtFromRDRight != null)
                    adminBoundaries.FromRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

                if (txtToRDLeft != null & txtToRDRight != null)
                    adminBoundaries.ToRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);

                if (ddlChannelSide != null)
                    adminBoundaries.ChannelSide = Convert.ToString(ddlChannelSide.SelectedItem.Value);

                adminBoundaries.ChannelID = Convert.ToInt64(hdnChannelID.Value);

                if (new ChannelBLL().IsVillageExists(adminBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (adminBoundaries.FromRD >= adminBoundaries.ToRD)
                {
                    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                    return;
                }
                else if (ChannelDetails.ChannelTotalRDs < adminBoundaries.FromRD)
                {
                    Master.ShowMessage("From RD can not be greater than Channel Total RDs.", SiteMaster.MessageType.Error);
                    return;
                }
                else if (ChannelDetails.ChannelTotalRDs < adminBoundaries.ToRD)
                {
                    Master.ShowMessage("To RD can not be greater than Channel Total RDs.", SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSaved = new ChannelBLL().SaveAdministrativeBoundaries(adminBoundaries);

                if (IsSaved)
                {
                    if (Convert.ToInt64(adminBoundariesID) == 0)
                        gvAdministrativeBoundaries.PageIndex = 0;

                    gvAdministrativeBoundaries.EditIndex = -1;
                    BindAdministrativeBoundariesGridView(adminBoundaries.ChannelID.Value);
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string adminBoundariesID = Convert.ToString(gvAdministrativeBoundaries.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new ChannelBLL().DeleteAdministrativeBoundaries(Convert.ToInt64(adminBoundariesID));
                if (IsDeleted)
                {
                    BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAdministrativeBoundaries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.PageIndex = e.NewPageIndex;
                gvAdministrativeBoundaries.EditIndex = -1;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion "End Gridview Events"

        #region "Dropdownlists Events"
        private void ResetDDLOnDistrictSelectedIndexChange(object sender, EventArgs e)
        {

            DropDownList ddlDistrict = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlDistrict.NamingContainer;

            DropDownList ddlTehsil = (DropDownList)gvRow.FindControl("ddlTehsil");
            DropDownList ddlPoliceStation = (DropDownList)gvRow.FindControl("ddlPoliceStation");
            DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");

            // Bind empty Tehsil dropdownlist
            Dropdownlist.DDLTehsils(ddlTehsil, true);
            // Bind empty Police Station dropdownlist
            Dropdownlist.DDLPoliceStations(ddlPoliceStation, true);
            // Bind empty Village dropdownlist
            Dropdownlist.DDLVillages(ddlVillage, true);
        }
        private void ResetDDLOnTehsilSelectedIndexChange(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlTehsil = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlTehsil.NamingContainer;

                DropDownList ddlPoliceStation = (DropDownList)gvRow.FindControl("ddlPoliceStation");
                DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");

                // Bind empty Police Station dropdownlist
                Dropdownlist.DDLPoliceStations(ddlPoliceStation, true);
                // Bind empty Village dropdownlist
                Dropdownlist.DDLVillages(ddlVillage, true);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnDistrictSelectedIndexChange(sender, e);

                DropDownList ddlDistrict = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDistrict.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlTehsil = (DropDownList)gvRow.FindControl("ddlTehsil");
                    Dropdownlist.DDLTehsils(ddlTehsil, false, Convert.ToInt32(ddlDistrict.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnTehsilSelectedIndexChange(sender, e);

                DropDownList ddlTehsil = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlTehsil.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlPoliceStation = (DropDownList)gvRow.FindControl("ddlPoliceStation");
                    Dropdownlist.DDLPoliceStations(ddlPoliceStation, false, Convert.ToInt32(ddlTehsil.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlPoliceStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlPoliceStation = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlPoliceStation.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlVillage = (DropDownList)gvRow.FindControl("ddlVillage");
                    Dropdownlist.DDLVillages(ddlVillage, false, Convert.ToInt32(ddlPoliceStation.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select, true);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion "End Dropdownlists Events"

        #endregion "End Administrative Boundaries"
    }
}