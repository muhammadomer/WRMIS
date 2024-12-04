using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure
{
    public partial class PhysicalLocation : BasePage
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            long ControlinfrastructureID = 0;
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    ControlinfrastructureID = Utility.GetNumericValueFromQueryString("ControlInfrastructureID", 0);

                    if (ControlinfrastructureID > 0)
                    {
                        hdnControlInfrastructureID.Value = Convert.ToString(ControlinfrastructureID);
                        hdnCotrolInfrastructureTypeID.Value = Convert.ToString(new ControlledInfrastructureBLL().GetStructureTypeIDByControlInfrastructureID(ControlinfrastructureID));
                        hlBack.NavigateUrl = string.Format("~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Search.aspx?ControlInfrastructureID={0}", ControlinfrastructureID);
                        LoadPhyicalLocation(ControlinfrastructureID);
                        ControlledInfrastructureDetails.ID = Convert.ToInt64(ControlinfrastructureID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion Page Load

        #region Set PageTitle
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ControlInfrastructurePhysicalLocation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set PageTitle

        #region Load PhyicalLocation
        private void LoadPhyicalLocation(long _ControlInfrastructureID)
        {
            BindIrrigationBoundariesGridView(_ControlInfrastructureID);
            BindAdministrativeBoundariesGridView(_ControlInfrastructureID);
        }

        #endregion Load PhyicalLocation

        #region Delete and Edit ConfirmMessage
        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteIrrigationBoundaries");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }
        private void AddEditConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnEdit = (Button)_e.Row.FindControl("btnSaveIrrigationBoundaries");

            if (btnEdit != null)
            {
                btnEdit.OnClientClick = "return confirm('All Data will be deleted and Gauge will be recalculate. Do you want to proced?');";
            }
        }

        #endregion Delete and Edit ConfirmMessage

        #region IrrigationBoundaries

        #region Bind IrrigationBoundaries GridView
        private void BindIrrigationBoundariesGridView(long _ControlInfrastructureID)
        {
            try
            {
                List<object> lstIrrigationBoundaries = new ControlledInfrastructureBLL().GetIrrigationBoundariesByControlInfrastructureID(_ControlInfrastructureID);
                gvIrrigationBoundaries.DataSource = lstIrrigationBoundaries;
                gvIrrigationBoundaries.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Bind IrrigationBoundaries GridView

        #region IrrigationBoundaries Gridview Method
        protected void gvIrrigationBoundaries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIrrigationBoundaries.PageIndex = e.NewPageIndex;
                gvIrrigationBoundaries.EditIndex = -1;
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIrrigationBoundaries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIrrigationBoundaries.EditIndex = -1;
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
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
                    List<object> lstIrrigationBoundaries = new ControlledInfrastructureBLL().GetIrrigationBoundariesByControlInfrastructureID(Convert.ToInt64(hdnControlInfrastructureID.Value));

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
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
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

        protected void gvIrrigationBoundaries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string irrigationBoundariesID = Convert.ToString(gvIrrigationBoundaries.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new ControlledInfrastructureBLL().DeleteIrrigationBoundaries(Convert.ToInt64(irrigationBoundariesID));
                if (IsDeleted)
                {
                    BindIrrigationBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
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
                BindIrrigationBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
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
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvIrrigationBoundaries.Rows[e.RowIndex];

                DropDownList ddlZone = (DropDownList)row.FindControl("ddlZone");
                DropDownList ddlCircle = (DropDownList)row.FindControl("ddlCircle");
                DropDownList ddlDivision = (DropDownList)row.FindControl("ddlDivision");
                #region "Datakeys"
                DataKey key = gvIrrigationBoundaries.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string irrigationBoundariesID = Convert.ToString(gvIrrigationBoundaries.DataKeys[e.RowIndex].Values[0]);

                FO_StructureIrrigationBoundaries irrigationBoundaries = new FO_StructureIrrigationBoundaries();

                irrigationBoundaries.ID = Convert.ToInt64(irrigationBoundariesID);

                if (ddlDivision != null)
                    irrigationBoundaries.DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                irrigationBoundaries.StructureID = Convert.ToInt32(hdnControlInfrastructureID.Value);
                irrigationBoundaries.StructureTypeID = Convert.ToInt64(hdnCotrolInfrastructureTypeID.Value);

                if (new ControlledInfrastructureBLL().IsDivisionExists(irrigationBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (irrigationBoundaries.ID == 0)
                {
                    irrigationBoundaries.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    irrigationBoundaries.CreatedDate = DateTime.Now;
                }
                else
                {
                    irrigationBoundaries.CreatedBy = Convert.ToInt32(CreatedBy);
                    irrigationBoundaries.CreatedDate = Convert.ToDateTime(CreatedDate);
                    irrigationBoundaries.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    irrigationBoundaries.ModifiedDate = DateTime.Now;
                }

                bool IsSave = new ControlledInfrastructureBLL().SaveIrrigationBoundaries(irrigationBoundaries);
                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(irrigationBoundariesID) == 0)
                        gvIrrigationBoundaries.PageIndex = 0;

                    gvIrrigationBoundaries.EditIndex = -1;
                    BindIrrigationBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIrrigationBoundaries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    if (gvIrrigationBoundaries.EditIndex == e.Row.RowIndex)
                    {

                        #region "Data Keys"
                        DataKey key = gvIrrigationBoundaries.DataKeys[e.Row.RowIndex];
                        string zoneID = Convert.ToString(key.Values[1]);
                        string circleID = Convert.ToString(key.Values[2]);
                        string divisionID = Convert.ToString(key.Values[3]);
                        #endregion

                        #region "Controls"
                        DropDownList ddlZone = (DropDownList)e.Row.FindControl("ddlZone");
                        DropDownList ddlCircle = (DropDownList)e.Row.FindControl("ddlCircle");
                        DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDivision");
                        #endregion

                        if (ddlZone != null)
                        {
                            Dropdownlist.DDLZones(ddlZone, false);
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
                                Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt32(ddlZone.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlCircle, circleID);
                            }
                        }

                        if (ddlDivision != null)
                        {
                            if (string.IsNullOrEmpty(ddlCircle.SelectedItem.Value))
                                Dropdownlist.DDLDivisions(ddlDivision, true);
                            else
                            {
                                Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value), Constants.IrrigationDomainID);
                                Dropdownlist.SetSelectedValue(ddlDivision, divisionID);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion IrrigationBoundaries Gridview Method

        #region "IrrigationBoundaries Dropdownlists Events"
        private void ResetDDLOnZoneSelectedIndexChange(object sender, EventArgs e)
        {

            DropDownList ddlZone = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlZone.NamingContainer;

            DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");

            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
        }
        private void ResetDDLOnCircleSelectedIndexChange(object sender, EventArgs e)
        {
            DropDownList ddlCircle = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlCircle.NamingContainer;

            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");

            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
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
                    Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt32(ddlZone.SelectedItem.Value));
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
                    Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value), Constants.IrrigationDomainID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion "IrrigationBoundariesDropdownlists Events"

        #endregion IrrigationBoundaries

        #region AdministrativeBoundaries

        #region Bind Administrative Boundaries GridView
        private void BindAdministrativeBoundariesGridView(long _ControlInfrastructureID)
        {
            try
            {
                List<object> lstAdminBoundaries = new ControlledInfrastructureBLL().GetAdministrativeBoundariesByControlInfrastructureID(_ControlInfrastructureID);
                gvAdministrativeBoundaries.DataSource = lstAdminBoundaries;
                gvAdministrativeBoundaries.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Bind Administrative Boundaries GridView

        #region AdministrativeBoundaries Gridview Method
        protected void gvAdministrativeBoundaries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.PageIndex = e.NewPageIndex;
                gvAdministrativeBoundaries.EditIndex = -1;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAdministrativeBoundaries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.EditIndex = -1;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
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
                    List<object> lstAdminBoundaries = new ControlledInfrastructureBLL().GetAdministrativeBoundariesByControlInfrastructureID(Convert.ToInt64(hdnControlInfrastructureID.Value));

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
                        FromRD = string.Empty,
                        ToRD = string.Empty,
                        FromRDTotal = string.Empty,
                        ToRDTotal = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
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

        protected void gvAdministrativeBoundaries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string adminBoundariesID = Convert.ToString(gvAdministrativeBoundaries.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new ControlledInfrastructureBLL().DeleteAdministrativeBoundaries(Convert.ToInt64(adminBoundariesID));
                if (IsDeleted)
                {
                    BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotDeleted.Description);
            }
        }

        protected void gvAdministrativeBoundaries_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.EditIndex = e.NewEditIndex;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(hdnControlInfrastructureID.Value));
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
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvAdministrativeBoundaries.Rows[e.RowIndex];

                DropDownList ddlDistrict = (DropDownList)row.FindControl("ddlDistrict");
                DropDownList ddlTehsil = (DropDownList)row.FindControl("ddlTehsil");
                DropDownList ddlPoliceStation = (DropDownList)row.FindControl("ddlPoliceStation");
                DropDownList ddlVillage = (DropDownList)row.FindControl("ddlVillage");

                #region "Datakeys"
                DataKey key = gvAdministrativeBoundaries.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string adminBoundariesID = Convert.ToString(gvAdministrativeBoundaries.DataKeys[e.RowIndex].Values[0]);

                FO_StructureAdminBoundaries adminBoundaries = new FO_StructureAdminBoundaries();

                adminBoundaries.ID = Convert.ToInt64(adminBoundariesID);

                if (ddlVillage != null)
                    adminBoundaries.VillageID = Convert.ToInt64(ddlVillage.SelectedItem.Value);

                if (ddlPoliceStation != null)
                    adminBoundaries.PoliceStationID = Convert.ToInt64(ddlPoliceStation.SelectedItem.Value);

                adminBoundaries.StructureID = Convert.ToInt64(hdnControlInfrastructureID.Value);
                adminBoundaries.StructureTypeID = Convert.ToInt64(hdnCotrolInfrastructureTypeID.Value);

                if (adminBoundaries.ID == 0)
                {
                    adminBoundaries.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    adminBoundaries.CreatedDate = DateTime.Now;
                }
                else
                {
                    adminBoundaries.CreatedBy = Convert.ToInt32(CreatedBy);
                    adminBoundaries.CreatedDate = Convert.ToDateTime(CreatedDate);
                    adminBoundaries.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    adminBoundaries.ModifiedDate = DateTime.Now;
                }

                if (new ControlledInfrastructureBLL().IsVillageExists(adminBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSaved = new ControlledInfrastructureBLL().SaveAdministrativeBoundaries(adminBoundaries);
                if (IsSaved)
                {
                    if (Convert.ToInt64(adminBoundariesID) == 0)
                        gvAdministrativeBoundaries.PageIndex = 0;

                    gvAdministrativeBoundaries.EditIndex = -1;
                    BindAdministrativeBoundariesGridView(adminBoundaries.StructureID.Value);
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void gvAdministrativeBoundaries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                    #endregion

                    #region "Controls"
                    DropDownList ddlDistrict = (DropDownList)e.Row.FindControl("ddlDistrict");
                    DropDownList ddlTehsil = (DropDownList)e.Row.FindControl("ddlTehsil");
                    DropDownList ddlPoliceStation = (DropDownList)e.Row.FindControl("ddlPoliceStation");
                    DropDownList ddlVillage = (DropDownList)e.Row.FindControl("ddlVillage");
                    #endregion

                    if (ddlDistrict != null)
                    {
                        Dropdownlist.DDLDistrictByStructureID(ddlDistrict, Convert.ToInt64(hdnControlInfrastructureID.Value));
                        Dropdownlist.SetSelectedValue(ddlDistrict, districtID);
                    }
                    if (ddlTehsil != null)
                    {
                        if (string.IsNullOrEmpty(ddlDistrict.SelectedItem.Value))
                        {
                            Dropdownlist.DDLTehsils(ddlTehsil, true);
                        }
                        else
                        {
                            Dropdownlist.DDLTehsils(ddlTehsil, false, Convert.ToInt32(ddlDistrict.SelectedItem.Value));
                            Dropdownlist.SetSelectedValue(ddlTehsil, tehsilID);
                        }
                    }
                    if (ddlPoliceStation != null)
                    {
                        if (string.IsNullOrEmpty(ddlTehsil.SelectedItem.Value))
                        {
                            Dropdownlist.DDLPoliceStations(ddlPoliceStation, true);
                        }
                        else
                        {
                            Dropdownlist.DDLPoliceStations(ddlPoliceStation, false, Convert.ToInt32(ddlTehsil.SelectedItem.Value));
                            Dropdownlist.SetSelectedValue(ddlPoliceStation, policeStationID);
                        }
                    }
                    if (ddlVillage != null)
                    {
                        if (string.IsNullOrEmpty(ddlPoliceStation.SelectedItem.Value))
                        {
                            Dropdownlist.DDLVillages(ddlVillage, true);
                        }
                        else
                        {
                            Dropdownlist.DDLVillages(ddlVillage, false, Convert.ToInt32(ddlPoliceStation.SelectedItem.Value));
                            Dropdownlist.SetSelectedValue(ddlVillage, villageID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion AdministrativeBoundaries Gridview Method

        #region AdministrativeBoundaries Dropdownlists Events"
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
                    Dropdownlist.DDLPoliceStations(ddlPoliceStation, false, Convert.ToInt32(ddlTehsil.SelectedItem.Value));
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
                    Dropdownlist.DDLVillages(ddlVillage, false, Convert.ToInt32(ddlPoliceStation.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion AdministrativeBoundaries Dropdownlists Events"

        #endregion AdministrativeBoundaries
    }
}