using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain
{
    public partial class SearchDrain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindZoneDropdown();


                    if (Session["DrainSearchCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["DrainSearchCriteria"]);

                        Int64 DrainID = 0;

                        if (Convert.ToInt64(searchCriteria["_DrainID"]) == 0 || Convert.ToInt64(searchCriteria["_DrainID"]) == -1)
                        {
                            if (Convert.ToInt64(hdnDrainID.Value) != 0 && Convert.ToInt64(hdnDrainID.Value) != -1)
                            {
                                DrainID = Convert.ToInt64(hdnDrainID.Value);
                            }
                        }
                        else
                        {
                            DrainID = Convert.ToInt64(searchCriteria["_DrainID"]);
                        }

                        this.LoadSearchCriteria(DrainID, Convert.ToInt64(searchCriteria["_ZoneID"]), Convert.ToInt64(searchCriteria["_CircleID"]), Convert.ToInt64(searchCriteria["_DivisionID"]), Convert.ToInt64(searchCriteria["_SubDivisionID"]), Convert.ToString(searchCriteria["_DrainName"]));

                        LoadSearchCriteria(searchCriteria);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 01-09-2016
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 01-09-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 01-09-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindDivisionDropdown(long _CircleID)
        {
            //Dropdownlist.DDLDivisionsForDFAndIrrigation(ddlDivision, false, _CircleID, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID, -1, (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 01-09-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        private void BindSubDivisionDropdown(long _DivisionID)
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, _DivisionID, (int)Constants.DropDownFirstOption.All);
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;
                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;

                //gvProtectionInfrastructure.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    ddlDivision.Enabled = false;

                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    ddlDivision.Enabled = true;
                }
                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;

                // gvProtectionInfrastructure.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == String.Empty)
                {
                    ddlSubDivision.SelectedIndex = 0;
                    ddlSubDivision.Enabled = false;
                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    BindSubDivisionDropdown(DivisionID);
                    ddlSubDivision.Enabled = true;
                }

                // gvProtectionInfrastructure.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDrainSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string DrainID = Convert.ToString(gvDrainSearch.DataKeys[e.RowIndex].Values["DrainID"]);
                if (new DrainBLL().IsDrainDependencyExists(Convert.ToInt64(DrainID)))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool isDeleted = new DrainBLL().DeleteDrainDataByID(Convert.ToInt64(DrainID));
                if (isDeleted == true)
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                BindDrainSearchGrid(0);
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// This function performs searching algorithm for drains.
        /// Created on 01-09-2016
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvDrainSearch.PageIndex = 0;
                BindDrainSearchGrid(0);
                gvDrainSearch.Visible = true;
            }
            catch (Exception ex)
            {
                gvDrainSearch.Visible = false;
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function Loads the search criteria
        /// Created on 01-09-2016
        /// </summary>
        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(searchCriteria["_ZoneID"]), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(searchCriteria["_CircleID"]), -1, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(searchCriteria["_DivisionID"]), (int)Constants.DropDownFirstOption.All);
            // Populate search criteria fields
            Dropdownlist.SetSelectedValue(ddlZone, searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]));
            Dropdownlist.SetSelectedValue(ddlCircle, searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]));
            Dropdownlist.SetSelectedValue(ddlDivision, searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]));
            Dropdownlist.SetSelectedValue(ddlSubDivision, searchCriteria["_SubDivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_SubDivisionID"]));
            txtDrainName.Text = Convert.ToString(searchCriteria["_DrainName"]);

        }

        /// <summary>
        /// This function binds the search grid to data source.
        /// Created on 01-09-2016
        /// </summary>
        private void BindDrainSearchGrid(long _DrainID)
        {
            try
            {
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                string DrainName = txtDrainName.Text.ToString().Trim();
                List<object> lstDrainSearch = new DrainBLL().GetDrainSearchCriteria(_DrainID
                , ZoneID
                , CircleID
                , DivisionID
                , SubDivisionID
                , DrainName
                );

                gvDrainSearch.DataSource = lstDrainSearch;
                gvDrainSearch.DataBind();

                dynamic searchCriteria = new
                {
                    _DrainID = _DrainID,
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _SubDivisionID = SubDivisionID,
                    _DrainName = DrainName

                };
                Session["DrainSearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
                gvDrainSearch.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void LoadSearchCriteria(long _DrainID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivision, string _DrainName)
        {

            try
            {
                List<object> lstDraineSearch = new DrainBLL().GetDrainSearchCriteria(_DrainID
               , _ZoneID
               , _CircleID
               , _DivisionID
               , _SubDivision
               , _DrainName.Trim()
              );

                gvDrainSearch.DataSource = lstDraineSearch;
                gvDrainSearch.DataBind();

                gvDrainSearch.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 01-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvDrainSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDrainSearch.PageIndex = e.NewPageIndex;
                //BindDrainSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDrainSearch_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDrainSearch.EditIndex = -1;
                BindDrainSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}