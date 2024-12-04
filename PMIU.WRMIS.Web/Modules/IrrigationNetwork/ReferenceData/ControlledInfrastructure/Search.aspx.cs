using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure
{
    public partial class Search : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindZoneDropdown();
                    this.Form.DefaultButton = btnSearch.UniqueID;
                    if (!string.IsNullOrEmpty(Request.QueryString["ControlInfrastructureID"]))
                    {
                        string ControlInfrastructuresID = Convert.ToString(Request.QueryString["ControlInfrastructureID"]);
                        hdnControlInfrastructuresID.Value = ControlInfrastructuresID;

                        if (_IsSaved)
                        {
                            //txtName.Text = Convert.ToString(HttpContext.Current.Session["ControlInfrastructuresName"]);
                            rdolStatus.SelectedIndex = 2;
                            BindControlledInfrastructureSearchGrid(Convert.ToInt64(ControlInfrastructuresID));

                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                    }
                    if (Session["ControlledInfrastructureSearchCriteria"] != null)
                    {
                        dynamic searchCriteria = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>((string)Session["ControlledInfrastructureSearchCriteria"]);
                        Int64 ControlInfrastructuresID = 0;

                        if (Convert.ToInt64(searchCriteria["_ControlInfrastructureID"]) == 0 || Convert.ToInt64(searchCriteria["_ControlInfrastructureID"]) == -1)
                        {
                            if (Convert.ToInt64(hdnControlInfrastructuresID.Value) != 0 && Convert.ToInt64(hdnControlInfrastructuresID.Value) != -1)
                            {
                                ControlInfrastructuresID = Convert.ToInt64(hdnControlInfrastructuresID.Value);
                            }
                        }
                        else
                        {
                            ControlInfrastructuresID = Convert.ToInt64(searchCriteria["_ControlInfrastructureID"]);
                        }
                        this.LoadSearchCriteria(ControlInfrastructuresID, searchCriteria["_ZoneID"], searchCriteria["_CircleID"], searchCriteria["_DivisionID"], searchCriteria["_ControlInfrastructuresName"], (searchCriteria["_ControlInfrastructureStatus"]));
                        LoadSearchCriteria(searchCriteria);
                    }
                    hlAddNew.Visible = base.CanAdd;
                    BindControlledInfrastructureSearchGrid(0);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindControlledInfrastructureSearchGrid(long _ControlInfrastructureID)
        {
            try
            {
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);

                string ControlInfrastructureName = txtName.Text.Trim();
                Int64 ControlInfrastructureStatus = -1;

                if (rdolStatus.SelectedItem != null && rdolStatus.SelectedItem.Value != "")
                {
                    ControlInfrastructureStatus = int.Parse(rdolStatus.SelectedItem.Value);
                }

                List<object> lstProtectionInfrastructureSearch = new ControlledInfrastructureBLL().GetControlledInfrastructureBySearch(_ControlInfrastructureID
                , ZoneID
                , CircleID
                , DivisionID
                , ControlInfrastructureName
                , ControlInfrastructureStatus);

                gvControlInfrastructure.DataSource = lstProtectionInfrastructureSearch;
                gvControlInfrastructure.DataBind();

                dynamic searchCriteria = new
                {
                    _ControlInfrastructureID = _ControlInfrastructureID,
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _ControlInfrastructuresName = ControlInfrastructureName,
                    _ControlInfrastructureStatus = ControlInfrastructureStatus
                };

                Session["ControlledInfrastructureSearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
                gvControlInfrastructure.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #region SearchCriteria
        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(searchCriteria["_ZoneID"]), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(searchCriteria["_CircleID"]), -1, (int)Constants.DropDownFirstOption.All);

            // Populate search criteria fields
            Dropdownlist.SetSelectedValue(ddlZone, searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]));
            Dropdownlist.SetSelectedValue(ddlCircle, searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]));
            Dropdownlist.SetSelectedValue(ddlDivision, searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]));
            txtName.Text = Convert.ToString(searchCriteria["_ControlInfrastructuresName"]);
            rdolStatus.SelectedIndex = rdolStatus.Items.IndexOf(rdolStatus.Items.FindByValue(Convert.ToString(searchCriteria["_ControlInfrastructureStatus"])));

        }
        public void LoadSearchCriteria(Int64 _ControlInfrastructureID, Int64 _ZoneID, Int64 _CircleID, Int64 _DivisionID, string _ControlInfrastructureName, Int64 _ControlInfrastructureStatus)
        {
            try
            {
                List<object> lstControlInfrastructureSearch = new ControlledInfrastructureBLL().GetControlledInfrastructureBySearch(_ControlInfrastructureID, _ZoneID, _CircleID, _DivisionID, _ControlInfrastructureName, _ControlInfrastructureStatus);

                gvControlInfrastructure.DataSource = lstControlInfrastructureSearch;
                gvControlInfrastructure.DataBind();

                gvControlInfrastructure.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion SearchCriteria

        #region GridView Events
        protected void gvControlInfrastructure_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ControlInfrastructureID = Convert.ToString(gvControlInfrastructure.DataKeys[e.RowIndex].Values["ControlInfrastructureID"]);

            if (new ControlledInfrastructureBLL().IsControlInfrastructureDependencyExists(Convert.ToInt64(ControlInfrastructureID)))
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                return;
            }

            bool isDeleted = new ControlledInfrastructureBLL().DeleteControlInfrastructure(Convert.ToInt64(ControlInfrastructureID));
            if (isDeleted == true)
                BindControlledInfrastructureSearchGrid(Convert.ToInt64(hdnControlInfrastructuresID.Value));

        }

        protected void gvControlInfrastructure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    long ControlInfrastructureID = Convert.ToInt64(gvControlInfrastructure.DataKeys[e.Row.RowIndex].Value);
            //}
        }

        protected void gvControlInfrastructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvControlInfrastructure.PageIndex = e.NewPageIndex;

                BindControlledInfrastructureSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion GridView Events

        #region Button Events
        protected void btnControlInfrastructureSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindControlledInfrastructureSearchGrid(0);
                gvControlInfrastructure.Visible = true;
            }
            catch (Exception ex)
            {
                gvControlInfrastructure.Visible = false;
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Button Events

        #region BindDropdown
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
        }
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.All);
        }
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisionsForDFAndIrrigation(ddlDivision, false, _CircleID, (int)Constants.DropDownFirstOption.All);
        }
        #endregion BindDropdown

        #region Dropdown Event
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

                gvControlInfrastructure.Visible = false;
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

                gvControlInfrastructure.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion Dropdown Event
    }
}