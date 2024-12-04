using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain
{
    public partial class DrainPhysicalLocation : BasePage
    {

        #region ViewState Constants

        public const string DrainID_VS = "DrainID";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (!string.IsNullOrEmpty(Request.QueryString["DrainID"]))
                    {
                        Session[DrainID_VS] = Convert.ToInt64(Request.QueryString["DrainID"]);

                        BindDrainDataByID(Convert.ToInt64(Session[DrainID_VS]));

                        LoadPhyicalLocation(Convert.ToInt64(Session[DrainID_VS]));
                        hlBack.NavigateUrl = "~/Modules/IrrigationNetwork/ReferenceData/Drain/SearchDrain.aspx";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void BindDrainDataByID(long _DrainID)
        {
            try
            {
                //dynamic DrainData = new DrainBLL().GetDrainDataByID(_DrainID);                
                //string DrainNAme = DrainData.GetType().GetProperty("Name").GetValue(DrainData, null);
                //double TotalLength = DrainData.GetType().GetProperty("TotalLength").GetValue(DrainData, null);
                //double? CatchmentArea = DrainData.GetType().GetProperty("CatchmentArea").GetValue(DrainData, null);
                //string MajorBuildupArea = DrainData.GetType().GetProperty("BuildupArea").GetValue(DrainData, null);
                //bool DrainStatus = DrainData.GetType().GetProperty("IsActive").GetValue(DrainData, null);                

                //IrrigationNetwork.Controls.DrainDetails.DrainName = DrainNAme;
                //IrrigationNetwork.Controls.DrainDetails.TotalLength = Convert.ToString(TotalLength);
                //IrrigationNetwork.Controls.DrainDetails.CatchmentArea = Convert.ToString(CatchmentArea);
                //IrrigationNetwork.Controls.DrainDetails.MajorBuildupArea = MajorBuildupArea;
                //IrrigationNetwork.Controls.DrainDetails.DrainStatus = DrainStatus == true ? "Active" : "Inactive";

                FO_Drain DrainData = new DrainBLL().GetDrainDataByID(_DrainID);
                IrrigationNetwork.Controls.DrainDetails.DrainName = DrainData.Name;
                IrrigationNetwork.Controls.DrainDetails.TotalLength = Convert.ToString(DrainData.TotalLength);
                IrrigationNetwork.Controls.DrainDetails.CatchmentArea = Convert.ToString(DrainData.CatchmentArea);
                IrrigationNetwork.Controls.DrainDetails.MajorBuildupArea = DrainData.BuildupArea;
                IrrigationNetwork.Controls.DrainDetails.DrainStatus = DrainData.IsActive == true ? "Active" : "Inactive";
            }
            catch (Exception)
            {

                throw;
            }

        }


        private void LoadPhyicalLocation(long _DrainID)
        {
            BindIrrigationBoundariesGridView(_DrainID);
            BindAdministrativeBoundariesGridView(_DrainID);
        }
        #region "Irrigation Boundaries"

        #region "GridView Events"
        private void BindIrrigationBoundariesGridView(long _DrainID)
        {
            try
            {
                List<object> lstIrrigationBoundaries = new DrainBLL().GetIrrigationBoundariesByDrainID(_DrainID);
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
                    List<object> lstirrigationboundaries = new DrainBLL().GetIrrigationBoundariesByDrainID(Convert.ToInt64(Session[DrainID_VS]));

                    lstirrigationboundaries.Add(
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
                        FromRd = string.Empty,
                        ToRd = string.Empty,
                        FromRdTotal = string.Empty,
                        ToRdTotal = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvIrrigationBoundaries.PageIndex = gvIrrigationBoundaries.PageCount;
                    gvIrrigationBoundaries.DataSource = lstirrigationboundaries;
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
            string FromLeftRD = string.Empty;
            string FromRightRD = string.Empty;
            string ToLeftRD = string.Empty;
            string ToRightRD = string.Empty;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (gvIrrigationBoundaries.EditIndex == e.Row.RowIndex)
                    {


                        #region "Data Keys"
                        DataKey key = gvIrrigationBoundaries.DataKeys[e.Row.RowIndex];
                        string zoneID = Convert.ToString(key.Values[1]);
                        string circleID = Convert.ToString(key.Values[2]);
                        string divisionID = Convert.ToString(key.Values[3]);
                        string SubDivisionID = Convert.ToString(key.Values[4]);
                        string SectionID = Convert.ToString(key.Values[5]);
                        string fromRD = Convert.ToString(key.Values[6]);
                        string toRD = Convert.ToString(key.Values[7]);
                        #endregion

                        #region "Controls"
                        DropDownList ddlZone = (DropDownList)e.Row.FindControl("ddlZone");
                        DropDownList ddlCircle = (DropDownList)e.Row.FindControl("ddlCircle");
                        DropDownList ddlDivision = (DropDownList)e.Row.FindControl("ddlDivision");
                        DropDownList ddlSubDivision = (DropDownList)e.Row.FindControl("ddlSubDivision");
                        DropDownList ddlSection = (DropDownList)e.Row.FindControl("ddlSection");


                        TextBox txtFromRDLeft = (TextBox)e.Row.FindControl("txtFromRDLeft");
                        TextBox txtFromRDRight = (TextBox)e.Row.FindControl("txtFromRDRight");
                        TextBox txtToRDLeft = (TextBox)e.Row.FindControl("txtToRDLeft");
                        TextBox txtToRDRight = (TextBox)e.Row.FindControl("txtToRDRight");
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
                                Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlDivision, divisionID);
                            }

                        }
                        if (ddlSubDivision != null)
                        {
                            if (string.IsNullOrEmpty(ddlDivision.SelectedItem.Value))
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
                            else
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt32(ddlDivision.SelectedItem.Value), (int)Constants.IrrigationDomainID);
                                Dropdownlist.SetSelectedValue(ddlSubDivision, SubDivisionID);
                            }

                        }

                        if (ddlSection != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                                Dropdownlist.DDLSections(ddlSection, true);
                            else
                            {
                                Dropdownlist.DDLSections(ddlSection, false, Convert.ToInt32(ddlSubDivision.SelectedItem.Value), (int)Constants.IrrigationDomainID);
                                Dropdownlist.SetSelectedValue(ddlSection, SectionID);
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
                BindIrrigationBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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
                BindIrrigationBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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
                DropDownList ddlSubDivision = (DropDownList)row.FindControl("ddlSubDivision");
                DropDownList ddlSection = (DropDownList)row.FindControl("ddlSection");

                TextBox txtFromRDLeft = (TextBox)row.FindControl("txtFromRDLeft");
                TextBox txtFromRDRight = (TextBox)row.FindControl("txtFromRDRight");
                TextBox txtToRDLeft = (TextBox)row.FindControl("txtToRDLeft");
                TextBox txtToRDRight = (TextBox)row.FindControl("txtToRDRight");

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
                if (ddlSection != null)
                    irrigationBoundaries.SectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);

                if (txtFromRDLeft != null & txtFromRDRight != null)
                    irrigationBoundaries.SectionRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

                if (txtToRDLeft != null & txtToRDRight != null)
                    irrigationBoundaries.SectionToRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);

                irrigationBoundaries.StructureID = Convert.ToInt64(Session[DrainID_VS]);
                irrigationBoundaries.StructureTypeID = new DrainBLL().GetDrainTypeByDrainID(Convert.ToInt64(Session[DrainID_VS]));

                if (new DrainBLL().IsSectionExists(irrigationBoundaries))
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


                if (irrigationBoundaries.SectionToRD >= irrigationBoundaries.SectionRD)
                {
                    Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //if (irrigationBoundaries.SectionRD >= irrigationBoundaries.SectionToRD)
                //{
                //    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                //    return;
                //}


                bool IsSave = new InfrastructureBLL().SaveIrrigationBoundaries(irrigationBoundaries);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(irrigationBoundariesID) == 0)
                        gvIrrigationBoundaries.PageIndex = 0;

                    gvIrrigationBoundaries.EditIndex = -1;
                    BindIrrigationBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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


                bool IsDeleted = new InfrastructureBLL().DeleteIrrigationBoundaries(Convert.ToInt64(irrigationBoundariesID));
                if (IsDeleted)
                {
                    BindIrrigationBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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
                BindIrrigationBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion "End GridView Events"

        #region "Dropdownlists Events"
        private void ResetDDLOnZoneSelectedIndexChanged(object sender, EventArgs e)
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
            // Bind empty SubDivision dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
            // Bind empty Section dropdownlist
            Dropdownlist.DDLSections(ddlSection, true);
        }
        private void ResetDDLOnCircleSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCircle = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlCircle.NamingContainer;

            DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");

            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLSections(ddlSection, true);
        }

        private void ResetDDLOnDivisionSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDivision = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;


            DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");

            // Bind empty division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLSections(ddlSection, true);
        }

        private void ResetDDLOnSubDivisionSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSubDivision = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlSubDivision.NamingContainer;

            DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");


            // Bind empty division dropdownlist
            Dropdownlist.DDLSections(ddlSection, true);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetDDLOnZoneSelectedIndexChanged(sender, e);

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
                ResetDDLOnCircleSelectedIndexChanged(sender, e);

                DropDownList ddlCircle = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlCircle.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                    Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt32(ddlCircle.SelectedItem.Value));
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
                ResetDDLOnDivisionSelectedIndexChanged(sender, e);
                DropDownList ddlDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlDivision.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt32(ddlDivision.SelectedItem.Value));
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
                ResetDDLOnSubDivisionSelectedIndexChanged(sender, e);
                DropDownList ddlSubDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubDivision.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                    Dropdownlist.DDLSections(ddlSection, false, Convert.ToInt32(ddlSubDivision.SelectedItem.Value));
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
        private void BindAdministrativeBoundariesGridView(long _DrainID)
        {
            try
            {
                List<object> lstAdminBoundaries = new DrainBLL().GetAdministrativeBoundriesByDrainID(_DrainID);
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
                    List<object> lstAdminBoundaries = new DrainBLL().GetAdministrativeBoundriesByDrainID(Convert.ToInt64(Session[DrainID_VS]));

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
                    #endregion

                    #region "Controls"
                    DropDownList ddlDistrict = (DropDownList)e.Row.FindControl("ddlDistrict");
                    DropDownList ddlTehsil = (DropDownList)e.Row.FindControl("ddlTehsil");
                    DropDownList ddlPoliceStation = (DropDownList)e.Row.FindControl("ddlPoliceStation");
                    DropDownList ddlVillage = (DropDownList)e.Row.FindControl("ddlVillage");
                    TextBox txtFromRDLeft = (TextBox)e.Row.FindControl("txtFromRDLeft");
                    TextBox txtFromRDRight = (TextBox)e.Row.FindControl("txtFromRDRight");
                    TextBox txtToRDLeft = (TextBox)e.Row.FindControl("txtToRDLeft");
                    TextBox txtToRDRight = (TextBox)e.Row.FindControl("txtToRDRight");
                    #endregion

                    if (ddlDistrict != null)
                    {
                        Dropdownlist.DDLDistrictByStructureID(ddlDistrict, Convert.ToInt64(Session[DrainID_VS]));
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
                BindAdministrativeBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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
                BindAdministrativeBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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

                TextBox txtFromRDLeft = (TextBox)row.FindControl("txtFromRDLeft");
                TextBox txtFromRDRight = (TextBox)row.FindControl("txtFromRDRight");
                TextBox txtToRDLeft = (TextBox)row.FindControl("txtToRDLeft");
                TextBox txtToRDRight = (TextBox)row.FindControl("txtToRDRight");

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

                if (txtFromRDLeft != null & txtFromRDRight != null)
                    adminBoundaries.FromRD = Calculations.CalculateTotalRDs(txtFromRDLeft.Text, txtFromRDRight.Text);

                if (txtToRDLeft != null & txtToRDRight != null)
                    adminBoundaries.ToRD = Calculations.CalculateTotalRDs(txtToRDLeft.Text, txtToRDRight.Text);

                adminBoundaries.StructureID = Convert.ToInt64(Session[DrainID_VS]);
                adminBoundaries.StructureTypeID = new DrainBLL().GetDrainTypeByDrainID(Convert.ToInt64(Session[DrainID_VS]));

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

                if (new InfrastructureBLL().IsVillageExists(adminBoundaries))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (adminBoundaries.ToRD >= adminBoundaries.FromRD)
                {
                    Master.ShowMessage("To RD should be less than From RD.", SiteMaster.MessageType.Error);
                    return;
                }

                //if (adminBoundaries.FromRD >= adminBoundaries.ToRD)
                //{

                //    Master.ShowMessage("From RD should be less than To RD.", SiteMaster.MessageType.Error);
                //    return;
                //}


                bool IsSaved = new InfrastructureBLL().SaveAdministrativeBoundaries(adminBoundaries);

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
        protected void gvAdministrativeBoundaries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string adminBoundariesID = Convert.ToString(gvAdministrativeBoundaries.DataKeys[e.RowIndex].Values[0]);

                bool IsDeleted = new InfrastructureBLL().DeleteAdministrativeBoundaries(Convert.ToInt64(adminBoundariesID));
                if (IsDeleted)
                {
                    BindAdministrativeBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RecordNotDeleted.Description);
            }
        }
        protected void gvAdministrativeBoundaries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAdministrativeBoundaries.PageIndex = e.NewPageIndex;
                gvAdministrativeBoundaries.EditIndex = -1;
                BindAdministrativeBoundariesGridView(Convert.ToInt64(Session[DrainID_VS]));
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

        #endregion "End Dropdownlists Events"

        #endregion "End Administrative Boundaries"


    }
}