using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure
{
    public partial class InfrastructureSearch : BasePage
    {
        List<CO_SubDivision> lstSubDivision = new List<CO_SubDivision>();
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindZoneDropdown();
                    BindActiveInfrastructureTypeDropdown();


                    this.Form.DefaultButton = btnShow.UniqueID;
                    if (!string.IsNullOrEmpty(Request.QueryString["InfrastructureID"]))
                    {
                        string InfrastructureID = Convert.ToString(Request.QueryString["InfrastructureID"]);
                        hdnInfrastructureID.Value = InfrastructureID;

                        if (_IsSaved)
                        {
                            txtInfrastructureName.Text =
                                Convert.ToString(HttpContext.Current.Session["InfrastructureName"]);
                            rdolInfrastructureStatus.SelectedIndex = 2;
                            BindProtectionInfrastructureSearchGrid(Convert.ToInt32(InfrastructureID));

                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                    }

                    if (Session["ProtectionInfrastructureSearchCriteria"] != null)
                    {
                        dynamic searchCriteria =
                            new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>(
                                (string)Session["ProtectionInfrastructureSearchCriteria"]);

                        Int64 infrastructureID = 0;

                        if (Convert.ToInt64(searchCriteria["_InfrastructureID"]) == 0 ||
                            Convert.ToInt64(searchCriteria["_InfrastructureID"]) == -1)
                        {
                            if (Convert.ToInt64(hdnInfrastructureID.Value) != 0 &&
                                Convert.ToInt64(hdnInfrastructureID.Value) != -1)
                            {
                                infrastructureID = Convert.ToInt64(hdnInfrastructureID.Value);
                            }
                        }
                        else
                        {
                            infrastructureID = Convert.ToInt64(searchCriteria["_InfrastructureID"]);
                        }

                        if (infrastructureID > 0)
                        {
                            this.LoadSearchCriteria(infrastructureID, Convert.ToInt64(searchCriteria["_ZoneID"]),
                                Convert.ToInt64(searchCriteria["_CircleID"]),
                                Convert.ToInt64(searchCriteria["_DivisionID"]),
                                Convert.ToInt16(searchCriteria["_InfrastructureTypeID"]),
                                Convert.ToString(searchCriteria["_InfrastructureName"]),
                                Convert.ToInt64(searchCriteria["_InfrastructureStatus"]));
                        }

                        LoadSearchCriteria(searchCriteria);
                    }

                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat",
                "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>",
                false);
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 01-09-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
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
            Dropdownlist.DDLDivisionsForDFAndIrrigation(ddlDivision, false, _CircleID,
                (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds protection infrastructure type to the ProtectionInfrastructureType dropdown
        /// Created on 29-08-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindActiveInfrastructureTypeDropdown()
        {
            Dropdownlist.DDLActiveInfrastructureType(ddlInfrastructureType, false,
                (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _InfrastructureID)
        {
            InfrastructureBLL bllInfrastructure = new InfrastructureBLL();

            bool IsExist = bllInfrastructure.IsInfrastructureIDExists(_InfrastructureID);

            //To do Wajahat
            ////if (!IsExist)
            ////{
            ////  long ZoneIrrigationLevelID = 4;

            ////  IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _InfrastructureID);
            ////}

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        ///// <summary>
        ///// This function check whether data is valid for add/edit operation.
        ///// Created on 29-10-2015
        ///// </summary>
        ///// <param name="_DivisionID"></param>
        ///// <param name="_SubDivisionID"></param>
        ///// <param name="_SubDivisionName"></param>
        ///// <returns>bool</returns>
        //private bool IsValidAddEdit(long _DivisionID, long _SubDivisionID, string _SubDivisionName)
        //{
        //  SubDivisionBLL bllSubDivision = new SubDivisionBLL();

        //  CO_SubDivision mdlSearchedSubDivision = bllSubDivision.GetSubDivisionByName(_SubDivisionName, _DivisionID);

        //  if (mdlSearchedSubDivision != null && _SubDivisionID != mdlSearchedSubDivision.ID)
        //  {
        //    Master.ShowMessage(Message.SubDivisionNameExists.Description, SiteMaster.MessageType.Error);

        //    return false;
        //  }

        //  return true;
        //}

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

                gvProtectionInfrastructure.Visible = false;
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

                gvProtectionInfrastructure.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadSearchCriteria(dynamic searchCriteria)
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(searchCriteria["_ZoneID"]),
                (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(searchCriteria["_CircleID"]), -1,
                (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLActiveInfrastructureType(ddlInfrastructureType, false,
                (int)Constants.DropDownFirstOption.All);
            // Populate search criteria fields
            if (searchCriteria["_ZoneID"] != -1)
            {
                ddlZone.Enabled = true;
            }
            Dropdownlist.SetSelectedValue(ddlZone,
                searchCriteria["_ZoneID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_ZoneID"]));

            if (searchCriteria["_CircleID"] != -1)
            {
                ddlCircle.Enabled = true;
            }
            Dropdownlist.SetSelectedValue(ddlCircle,
                searchCriteria["_CircleID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_CircleID"]));

            if (searchCriteria["_DivisionID"] != -1)
            {
                ddlDivision.Enabled = true;
            }
            Dropdownlist.SetSelectedValue(ddlDivision,
                searchCriteria["_DivisionID"] == -1 ? string.Empty : Convert.ToString(searchCriteria["_DivisionID"]));

            if (searchCriteria["_InfrastructureTypeID"] != -1)
            {
                ddlInfrastructureType.Enabled = true;
            }
            Dropdownlist.SetSelectedValue(ddlInfrastructureType,
                searchCriteria["_InfrastructureTypeID"] == -1
                    ? string.Empty
                    : Convert.ToString(searchCriteria["_InfrastructureTypeID"]));

            txtInfrastructureName.Text = Convert.ToString(searchCriteria["_InfrastructureName"]);
            rdolInfrastructureStatus.SelectedIndex =
                rdolInfrastructureStatus.Items.IndexOf(
                    rdolInfrastructureStatus.Items.FindByValue(Convert.ToString(searchCriteria["_InfrastructureStatus"])));
        }

        private void BindProtectionInfrastructureSearchGrid(long _InfrastructureID)
        {
            try
            {
                long ZoneID = ddlZone.SelectedItem.Value == string.Empty
                    ? -1
                    : Convert.ToInt64(ddlZone.SelectedItem.Value);
                long CircleID = ddlCircle.SelectedItem.Value == string.Empty
                    ? -1
                    : Convert.ToInt64(ddlCircle.SelectedItem.Value);
                long DivisionID = ddlDivision.SelectedItem.Value == string.Empty
                    ? -1
                    : Convert.ToInt64(ddlDivision.SelectedItem.Value);

                Int16 InfrastructureTypeID = -1;
                if (ddlInfrastructureType.SelectedItem.Value != string.Empty)
                {
                    InfrastructureTypeID = Convert.ToInt16(ddlInfrastructureType.SelectedItem.Value);
                }

                string InfrastructureName = txtInfrastructureName.Text.Trim();
                Int64 InfrastructureStatus = -1;

                if (rdolInfrastructureStatus.SelectedItem != null && rdolInfrastructureStatus.SelectedItem.Value != "")
                {
                    InfrastructureStatus = int.Parse(rdolInfrastructureStatus.SelectedItem.Value);
                }

                List<object> lstProtectionInfrastructureSearch =
                    new InfrastructureBLL().GetProtectionInfrastructureBySearchCriteria(_InfrastructureID
                        , ZoneID
                        , CircleID
                        , DivisionID
                        , InfrastructureTypeID
                        , InfrastructureName
                        , InfrastructureStatus);

                gvProtectionInfrastructure.DataSource = lstProtectionInfrastructureSearch;
                gvProtectionInfrastructure.DataBind();

                dynamic searchCriteria = new
                {
                    _InfrastructureID = _InfrastructureID,
                    _ZoneID = ZoneID,
                    _CircleID = CircleID,
                    _DivisionID = DivisionID,
                    _InfrastructureTypeID = InfrastructureTypeID,
                    _InfrastructureName = InfrastructureName,
                    _InfrastructureStatus = InfrastructureStatus
                };

                Session["ProtectionInfrastructureSearchCriteria"] =
                    new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
                gvProtectionInfrastructure.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }


        //private void BindProtectionInfrastructureSearchGrid()
        //{
        //  try
        //  {
        //    long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
        //    long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
        //    long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);

        //    Int16 InfrastructureTypeID = -1;
        //    if (ddlInfrastructureType.SelectedItem.Value != string.Empty)
        //    {
        //      InfrastructureTypeID = Convert.ToInt16(ddlInfrastructureType.SelectedItem.Value);
        //    }

        //    string InfrastructureName = txtInfrastructureName.Text.ToString().Trim();
        //    int InfrastructureStatus = 0;

        //    if (rdolInfrastructureStatus.SelectedItem != null && rdolInfrastructureStatus.SelectedItem.Value != "")
        //    {
        //      InfrastructureStatus = int.Parse(rdolInfrastructureStatus.SelectedItem.Value);
        //    }

        //    List<object> lstProtectionInfrastructureSearch = new InfrastructureBLL().GetProtectionInfrastructureBySearchCriteria(0
        //    , ZoneID
        //    , CircleID
        //    , DivisionID
        //    , InfrastructureTypeID
        //    , InfrastructureName
        //    , InfrastructureStatus);

        //    gvProtectionInfrastructure.DataSource = lstProtectionInfrastructureSearch;
        //    gvProtectionInfrastructure.DataBind();

        //    dynamic searchCriteria = new
        //    {
        //      _ZoneID = ZoneID,
        //      _CircleID = CircleID,
        //      _DivisionID = DivisionID,
        //      _InfrastructureTypeID = InfrastructureTypeID,
        //      _InfrastructureName = InfrastructureName,
        //      _InfrastructureStatus = InfrastructureStatus
        //    };

        //    Session["IS_SC_SearchCriteria"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(searchCriteria);
        //    gvProtectionInfrastructure.Visible = true;
        //  }
        //  catch (Exception ex)
        //  {
        //    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //  }

        //}

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindProtectionInfrastructureSearchGrid(0);
                gvProtectionInfrastructure.Visible = true;
            }
            catch (Exception ex)
            {
                gvProtectionInfrastructure.Visible = false;
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProtectionInfrastructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProtectionInfrastructure.PageIndex = e.NewPageIndex;

                BindProtectionInfrastructureSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProtectionInfrastructure_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvProtectionInfrastructure.EditIndex = -1;

                BindProtectionInfrastructureSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProtectionInfrastructure_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long InfrastructureID =
                    Convert.ToInt64(
                        ((Label)gvProtectionInfrastructure.Rows[e.RowIndex].FindControl("lblInfrastructureID")).Text);

                if (!IsValidDelete(InfrastructureID))
                {
                    return;
                }

                InfrastructureBLL bllInfrastructure = new InfrastructureBLL();

                bool IsDeleted = bllInfrastructure.DeleteInfrastructure(InfrastructureID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                    BindProtectionInfrastructureSearchGrid(InfrastructureID);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvProtectionInfrastructure_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvProtectionInfrastructure.EditIndex = -1;

                BindProtectionInfrastructureSearchGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void LoadSearchCriteria(Int64 _InfrastructureID, Int64 _ZoneID, Int64 _CircleID, Int64 _DivisionID,
            Int16 _InfrastructureTypeID, string _InfrastructureName, Int64 _InfrastructureStatus)
        {

            try
            {
                List<object> lstProtectionInfrastructureSearch =
                    new InfrastructureBLL().GetProtectionInfrastructureBySearchCriteria(_InfrastructureID
                        , _ZoneID
                        , _CircleID
                        , _DivisionID
                        , _InfrastructureTypeID
                        , _InfrastructureName.Trim()
                        , _InfrastructureStatus);

                gvProtectionInfrastructure.DataSource = lstProtectionInfrastructureSearch;
                gvProtectionInfrastructure.DataBind();

                gvProtectionInfrastructure.Visible = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProtectionInfrastructure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region DataKey
                    DataKey key = gvProtectionInfrastructure.DataKeys[e.Row.RowIndex];
                    string StructureType = Convert.ToString(key.Values["InfrastructureTypeName"]);
                    #endregion DataKey

                    #region Control
                    Button btnGauges = (Button)e.Row.FindControl("btnGauges");
                    #endregion Control

                    btnGauges.Enabled = false;
                    if (StructureType == "Bund / Guide Bund")
                    {
                        btnGauges.Enabled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProtectionInfrastructure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "btnGauge")
                {
                    long infrastructureID = Convert.ToInt64(e.CommandArgument);
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    DataKey key = gvProtectionInfrastructure.DataKeys[row.RowIndex];
                    Response.Redirect("Gauges.aspx?infrastructureID=" + infrastructureID + "&InfrastructureTypeID=" + Convert.ToInt64(key["InfrastructureTypeID"]), false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}