using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.OutletData;
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
using Antlr.Runtime.Tree;
using PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class GaugeInformation : BasePage
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
                        hlBack.NavigateUrl =
                            string.Format("~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx?ChannelID={0}",
                                channelID);
                        LoadGaugeInformation(channelID);
                        //  LoadAllDropdownlistsData(Convert.ToInt64(hdnChannelID.Value));



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
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.GaugeInformation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadGaugeInformation(long _ChannelID)
        {
            try
            {
                ChannelDetails.ChannelID = _ChannelID;
                bool isCalculated = new ChannelBLL().AutoDetermineGaguesFromIrrigationBoundaries(_ChannelID);
                if (isCalculated)
                    new ChannelBLL().UpdateIsCalculated(_ChannelID, false);
                BindGaugeInformationGridView(_ChannelID);
            }
            catch (Exception ex)
            {
                Master.ShowMessage("Gauges can not be refreshed due to child enteries exists.",
                    SiteMaster.MessageType.Error);
                BindGaugeInformationGridView(_ChannelID);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region "GridView Events"

        private void BindGaugeInformationGridView(long _ChannelID)
        {
            try
            {
                List<object> lstGaugeInformation = new ChannelBLL().GetGaugeInformationsByChannelID(_ChannelID);
                gvGaugeInformation.DataSource = lstGaugeInformation;
                gvGaugeInformation.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddGaugeInformation")
                {
                    List<object> lstGaugeInformation =
                        new ChannelBLL().GetGaugeInformationsByChannelID(Convert.ToInt64(hdnChannelID.Value));
                    lstGaugeInformation.Add
                    (
                        new
                        {
                            ID = 0,
                            SubDivisionID = string.Empty,
                            SubDivisionName = string.Empty,
                            SectionID = string.Empty,
                            SectionName = string.Empty,
                            GaugeTypeID = string.Empty,
                            GaugeTypeName = string.Empty,
                            GaugeCategoryID = string.Empty,
                            GaugeCategoryName = string.Empty,
                            GaugeRD = string.Empty,
                            TotalGaugeRD = string.Empty,
                            DesignDischarge = string.Empty,
                            GaugeAtBedID = string.Empty,
                            GaugeAtBedName = string.Empty,
                            DivisionName = string.Empty,
                            DivisionID = string.Empty,
                            CircleID = string.Empty

                        }
                    );
                    gvGaugeInformation.PageIndex = gvGaugeInformation.PageCount;
                    gvGaugeInformation.DataSource = lstGaugeInformation;
                    gvGaugeInformation.DataBind();

                    gvGaugeInformation.EditIndex = gvGaugeInformation.Rows.Count - 1;
                    gvGaugeInformation.DataBind();
                }
                else if (e.CommandName == "GaugeMapping")
                {
                    GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    DataKey key = gvGaugeInformation.DataKeys[gvrow.RowIndex];
                    long ID = Convert.ToInt32(key["ID"]);
                    long CircleID = Convert.ToInt32(key["CircleID"]);
                    long DivisionID = Convert.ToInt32(key["DivisionID"]);
                    long SubDivID = Convert.ToInt32(key["SubDivisionID"]);
                    long SectioniID = Convert.ToInt32(key["SectionID"]);
                    hdnID.Value = Convert.ToString(ID);

                    hdnSectionID.Value = Convert.ToString(SectioniID);
                    ddlDivisionalGL1.ClearSelection();
                    ddlDivisionalGL2.ClearSelection();
                    ddlDivisionalGL3.ClearSelection();
                    ddlSubDivisionalGL1.ClearSelection();
                    ddlSubDivisionalGL2.ClearSelection();
                    ddlSection.ClearSelection();
                    ddlSubDivisionalGL1.Enabled = false;
                    ddlSubDivisionalGL2.Enabled = false;
                    ddlSection.Enabled = false;
                    ddlSubDivisionalGL1.Attributes.Remove("required");
                    ddlSubDivisionalGL1.CssClass = "form-control";
                    ddlSubDivisionalGL2.Attributes.Remove("required");
                    ddlSubDivisionalGL2.CssClass = "form-control";
                    ddlSection.Attributes.Remove("required");
                    ddlSection.CssClass = "form-control";
                    //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL1, Convert.ToInt64(CircleID));
                    //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL2, Convert.ToInt64(CircleID));
                    //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL3, Convert.ToInt64(CircleID));


                    Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL1);
                    Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL2);
                    Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL3);
                    //Dropdownlist.DDLDivisionByID(ddlDivisionalGL1, Convert.ToInt64(hdnDivID.Value));
                    //Dropdownlist.DDLDivisionByID(ddlDivisionalGL2, Convert.ToInt64(hdnDivID.Value));
                    //Dropdownlist.DDLDivisionByID(ddlDivisionalGL3, Convert.ToInt64(hdnDivID.Value));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddOpeningOffices",
                        "$('#AddGaugeLocation').modal();", true);
                    LoadAllDropdownlistsData(Convert.ToInt64(ID));
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private bool IsAutoDetermindGauge(long _GaugeCategoryID)
        {
            bool flag = (long)Constants.GaugeCategory.SectionalGauge == _GaugeCategoryID || (long)Constants.GaugeCategory.CriticalGauge == _GaugeCategoryID;
            return flag;
        }

        private void HideShowDeleteButton(GridViewRowEventArgs _e, DataKey _key, string _GaugeCatergoryID)
        {

            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteGaugeInformation");

            if (btnDelete != null && IsAutoDetermindGauge(Convert.ToInt64(_GaugeCatergoryID)))
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
            else if (btnDelete != null)
            {
                hdnNoDeletionMessage.Value = Message.RecordNotDeleted.Description;
                btnDelete.OnClientClick = "DisplayAutoGeneratedMessage(); return false;";
            }
        }

        private void HideControlsForAutoDetermindGaugeCategories(GridViewRowEventArgs e)
        {
            Label lblEditSubDivisionName = (Label)e.Row.FindControl("lblEditSubDivisionName");
            Label lblEditSectionName = (Label)e.Row.FindControl("lblEditSectionName");
            Label lblEditGaugeCategoryName = (Label)e.Row.FindControl("lblEditGaugeCategoryName");
            Label lblEditGaugeRD = (Label)e.Row.FindControl("lblEditGaugeRD");

            DropDownList ddlSubDivision = (DropDownList)e.Row.FindControl("ddlSubDivision");
            DropDownList ddlSection = (DropDownList)e.Row.FindControl("ddlSection");
            DropDownList ddlGaugeCategory = (DropDownList)e.Row.FindControl("ddlGaugeCategory");


            Panel panelRDs = (Panel)e.Row.FindControl("panelRDs");

            if (ddlSubDivision != null && lblEditSubDivisionName != null)
            {
                ddlSubDivision.CssClass = "hidden";
                lblEditSubDivisionName.CssClass = "block";
            }
            if (ddlSection != null && lblEditSectionName != null)
            {
                ddlSection.CssClass = "hidden";
                lblEditSectionName.CssClass = "block";
            }
            if (ddlGaugeCategory != null && lblEditGaugeCategoryName != null)
            {
                ddlGaugeCategory.CssClass = "hidden";
                lblEditGaugeCategoryName.CssClass = "block";
            }
            //if (panelRDs != null && lblEditGaugeRD != null)
            //{
            //    panelRDs.CssClass = "hidden";
            //    lblEditGaugeRD.CssClass = "block";
            //}
        }

        /// <summary>
        /// In case of Tail Gauge this function disabled corresponding DT Parameter Button and DT History
        /// Created 19-11-2015
        /// </summary>
        /// <param name="_e"></param>
        /// <param name="_key"></param>
        private void DisableDTParameterHistoryButton(GridViewRowEventArgs _e, DataKey _key)
        {
            HyperLink hlDTParameters = (HyperLink)_e.Row.FindControl("hlDTParameters");
            HyperLink hlDTHistory = (HyperLink)_e.Row.FindControl("hlDTHistory");

            if ((int)Constants.GaugeCategory.TailGauge == Convert.ToInt32(_key.Values["GaugeCategoryID"]) ||
                Convert.ToString(_key[0]) == "0")
            {
                if (hlDTParameters != null)
                    hlDTParameters.Enabled = false;
                if (hlDTHistory != null)
                    hlDTHistory.Enabled = false;
            }
        }

        protected void gvGaugeInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string leftGaugeRD = string.Empty;
            string rightGaugeRD = string.Empty;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvGaugeInformation.DataKeys[e.Row.RowIndex];
                    string gaugeCategoryID = Convert.ToString(key.Values[4]);

                    // Hide delete button for auto determind Gauge Categories.
                    if (!string.IsNullOrEmpty(gaugeCategoryID))
                    {
                        HideShowDeleteButton(e, key, gaugeCategoryID);
                        // In Case of Tail Gauge the corresponding DT Parameter Button and DT History are disabled
                        DisableDTParameterHistoryButton(e, key);
                    }
                    if (gvGaugeInformation.EditIndex == e.Row.RowIndex)
                    {

                        #region "Data Keys"

                        string subDivisionID = Convert.ToString(key.Values[1]);
                        string sectionID = Convert.ToString(key.Values[2]);
                        string gaugeTypeID = Convert.ToString(key.Values[3]);

                        string gaugeRD = Convert.ToString(key.Values[5]);
                        string TotalGaugeRD = Convert.ToString(key.Values[5]);
                        string designDischarge = Convert.ToString(key.Values[6]);
                        string gaugeAtBedID = Convert.ToString(key.Values[7]);

                        #endregion

                        #region "Controls"

                        DropDownList ddlSubDivision = (DropDownList)e.Row.FindControl("ddlSubDivision");
                        DropDownList ddlSection = (DropDownList)e.Row.FindControl("ddlSection");
                        DropDownList ddlGaugeType = (DropDownList)e.Row.FindControl("ddlGaugeType");
                        DropDownList ddlGaugeCategory = (DropDownList)e.Row.FindControl("ddlGaugeCategory");
                        DropDownList ddlGaugeAtBed = (DropDownList)e.Row.FindControl("ddlGaugeAtBed");


                        TextBox txtGaugeRDLeft = (TextBox)e.Row.FindControl("txtGaugeRDLeft");
                        TextBox txtGaugeRDRight = (TextBox)e.Row.FindControl("txtGaugeRDRight");
                        TextBox txtDesignDischarge = (TextBox)e.Row.FindControl("txtDesignDischarge");

                        #endregion

                        if (!string.IsNullOrEmpty(gaugeCategoryID) &&
                            !IsAutoDetermindGauge(Convert.ToInt64(gaugeCategoryID)))
                        {
                            HideControlsForAutoDetermindGaugeCategories(e);
                        }

                        if (ddlSubDivision != null)
                        {
                            Dropdownlist.DDLSubDivisionsByChannelID(ddlSubDivision, Convert.ToInt64(hdnChannelID.Value),
                                (int)Constants.DropDownFirstOption.Select, true);
                            Dropdownlist.SetSelectedValue(ddlSubDivision, subDivisionID);
                        }

                        if (ddlSection != null)
                        {
                            if (!string.IsNullOrEmpty(ddlSubDivision.SelectedItem.Value))
                            {
                                Dropdownlist.DDLSectionsBySubDivisionChannelID(ddlSection,
                                    Convert.ToInt32(ddlSubDivision.SelectedItem.Value),
                                    Convert.ToInt64(hdnChannelID.Value), (int)Constants.DropDownFirstOption.Select,
                                    true);
                                Dropdownlist.SetSelectedValue(ddlSection, sectionID);
                            }
                            else
                                Dropdownlist.DDLSectionsBySubDivisionChannelID(ddlSection, -1, -1,
                                    (int)Constants.DropDownFirstOption.Select, true);

                        }

                        if (ddlGaugeType != null)
                        {
                            Dropdownlist.DDLGaugeTypes(ddlGaugeType);

                            if (!string.IsNullOrEmpty(gaugeTypeID))
                                Dropdownlist.SetSelectedValue(ddlGaugeType, gaugeTypeID);
                        }

                        if (ddlGaugeCategory != null)
                        {
                            // In case of System determine Gauges Display all guage categories
                            if (!string.IsNullOrEmpty(gaugeCategoryID) && !IsAutoDetermindGauge(Convert.ToInt64(gaugeCategoryID)))
                            {
                                Dropdownlist.DDLGetAllGaugeCategories(ddlGaugeCategory, false, (int)Constants.DropDownFirstOption.Select, true);
                                Dropdownlist.SetSelectedValue(ddlGaugeCategory, gaugeCategoryID);
                            }
                            else if (!string.IsNullOrEmpty(gaugeCategoryID) && IsAutoDetermindGauge(Convert.ToInt64(gaugeCategoryID)))
                            {
                                Dropdownlist.DDLGaugeCategories(ddlGaugeCategory);
                                Dropdownlist.SetSelectedValue(ddlGaugeCategory, gaugeCategoryID);
                            }
                            else
                                Dropdownlist.DDLGaugeCategories(ddlGaugeCategory);
                        }
                        if (ddlGaugeAtBed != null)
                        {
                            Dropdownlist.DDLGaugeLevels(ddlGaugeAtBed);

                            if (!string.IsNullOrEmpty(gaugeAtBedID))
                                Dropdownlist.SetSelectedValue(ddlGaugeAtBed, gaugeAtBedID);
                        }

                        if (txtDesignDischarge != null)
                        {
                            txtDesignDischarge.Text = Convert.ToString(designDischarge);
                        }

                        if (!string.IsNullOrEmpty(TotalGaugeRD))
                        {
                            Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(TotalGaugeRD));
                            leftGaugeRD = tupleRD.Item1;
                            rightGaugeRD = tupleRD.Item2;
                        }

                        if (txtGaugeRDLeft != null)
                            txtGaugeRDLeft.Text = leftGaugeRD;
                        if (txtGaugeRDRight != null)
                            txtGaugeRDRight.Text = rightGaugeRD;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInformation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvGaugeInformation.EditIndex = e.NewEditIndex;
                BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInformation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGaugeInformation.EditIndex = -1;
                BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInformation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string gaugeInformationID = Convert.ToString(gvGaugeInformation.DataKeys[e.RowIndex].Values["ID"]);

                GridViewRow row = gvGaugeInformation.Rows[e.RowIndex];

                #region "Controls"

                DropDownList ddlSubDivision = (DropDownList)row.FindControl("ddlSubDivision");
                DropDownList ddlSection = (DropDownList)row.FindControl("ddlSection");
                DropDownList ddlGaugeType = (DropDownList)row.FindControl("ddlGaugeType");
                DropDownList ddlGaugeCategory = (DropDownList)row.FindControl("ddlGaugeCategory");
                DropDownList ddlGaugeAtBed = (DropDownList)row.FindControl("ddlGaugeAtBed");


                TextBox txtGaugeRDLeft = (TextBox)row.FindControl("txtGaugeRDLeft");
                TextBox txtGaugeRDRight = (TextBox)row.FindControl("txtGaugeRDRight");
                TextBox txtDesignDischarge = (TextBox)row.FindControl("txtDesignDischarge");

                #endregion

                CO_ChannelGauge gaugeInformation = new CO_ChannelGauge();
                gaugeInformation.ID = Convert.ToInt64(gaugeInformationID);
                gaugeInformation.ChannelID = Convert.ToInt64(hdnChannelID.Value);
                if (ddlSection != null)
                    gaugeInformation.SectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);

                if (ddlGaugeType != null && !string.IsNullOrEmpty(ddlGaugeType.SelectedItem.Value))
                    gaugeInformation.GaugeTypeID = Convert.ToInt64(ddlGaugeType.SelectedItem.Value);

                if (ddlGaugeCategory != null)
                    gaugeInformation.GaugeCategoryID = Convert.ToInt64(ddlGaugeCategory.SelectedItem.Value);

                if (txtGaugeRDLeft != null & txtGaugeRDRight != null)
                    gaugeInformation.GaugeAtRD = Calculations.CalculateTotalRDs(txtGaugeRDLeft.Text,
                        txtGaugeRDRight.Text);

                if (txtDesignDischarge != null && !string.IsNullOrEmpty(txtDesignDischarge.Text.Trim()))
                    gaugeInformation.DesignDischarge = Convert.ToDouble(txtDesignDischarge.Text.Trim());

                if (ddlGaugeAtBed != null)
                    gaugeInformation.GaugeLevelID = Convert.ToInt64(ddlGaugeAtBed.SelectedItem.Value);

                Tuple<int, int> tupleRDs =
                    new ChannelBLL().GetIrrigationBoundariesRDs(Convert.ToInt64(hdnChannelID.Value));

                if (!(tupleRDs.Item1 <= gaugeInformation.GaugeAtRD && tupleRDs.Item2 >= gaugeInformation.GaugeAtRD))
                {
                    Master.ShowMessage(
                        "Gauge at  R.D is out of range of Starting and Ending R.Ds in physical location.",
                        SiteMaster.MessageType.Error);
                    return;
                }

                if (new ChannelBLL().IsGaugeRDExists(gaugeInformation))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool isSaved = new ChannelBLL().SaveGaugeInformation(gaugeInformation);

                if (isSaved)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(gaugeInformationID) == 0)
                        gvGaugeInformation.PageIndex = 0;

                    gvGaugeInformation.EditIndex = -1;
                    BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeInformation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Int64 gaugeInformationID = Convert.ToInt64(gvGaugeInformation.DataKeys[e.RowIndex].Values["ID"]);
                if (new ChannelBLL().IsGaugeDependanceExists(gaugeInformationID))
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool isDeleted = new ChannelBLL().DeleteGaugeInformation(gaugeInformationID);
                if (isDeleted)
                {
                    BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvGaugeInformation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGaugeInformation.PageIndex = e.NewPageIndex;
                gvGaugeInformation.EditIndex = -1;
                BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion "End GridView Events"

        #region "Dropdownlists Events"

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                DropDownList ddlSubDivision = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubDivision.NamingContainer;
                Label lblEditDivisionName = gvRow.FindControl("lblEditDivisionName") as Label;
                if (gvRow != null)
                {
                    if (ddlSubDivision.SelectedItem.Value != "")
                    {
                        object _DivisionNameBySubDivID =
                            new ChannelBLL().GetDivisionNameBySubDivisonID(Convert.ToInt64(ddlSubDivision.SelectedValue));
                        if (_DivisionNameBySubDivID != null)
                        {
                            lblEditDivisionName.Text =
                                Convert.ToString(Utility.GetDynamicPropertyValue(_DivisionNameBySubDivID, "DivisionName"));
                        }
                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        Dropdownlist.DDLSectionsBySubDivisionChannelID(ddlSection,
                            Convert.ToInt32(ddlSubDivision.SelectedItem.Value), Convert.ToInt64(hdnChannelID.Value),
                            (int)Constants.DropDownFirstOption.Select, true);
                    }
                    else
                    {
                        lblEditDivisionName.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion "End Dropdownlists Events"

        public string GetDTParameterPageURL(string _GaugeAtBedID, string _GaugeID)
        {
            string URL = string.Empty;
            try
            {
                int gaugeAtBedOrCrest = Convert.ToInt32(_GaugeAtBedID);

                if (gaugeAtBedOrCrest != 0)
                {
                    URL = "~/Modules/IrrigationNetwork/Channel/";
                    if (gaugeAtBedOrCrest == (int)Constants.GaugeLevel.BedLevel)
                        URL += "BedLevelDTParameters.aspx?ChannelGaugeID=" + _GaugeID;

                    else if (gaugeAtBedOrCrest == (int)Constants.GaugeLevel.CrestLevel)
                        URL += "CrestLevelDTParameters.aspx?ChannelGaugeID=" + _GaugeID;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return URL;
        }

        public string GetDTHistoryPageURL(string _GaugeAtBedID, string _GaugeID)
        {
            string URL = string.Empty;
            try
            {
                int gaugeAtBedOrCrest = Convert.ToInt32(_GaugeAtBedID);
                if (gaugeAtBedOrCrest != 0)
                {
                    URL = "~/Modules/IrrigationNetwork/Channel/";
                    if (gaugeAtBedOrCrest == (int)Constants.GaugeLevel.BedLevel)
                        URL += "ViewBedLevelDTHistory.aspx?ChannelGaugeID=" + _GaugeID;

                    else if (gaugeAtBedOrCrest == (int)Constants.GaugeLevel.CrestLevel)
                        URL += "ViewCrestLevelDTHistory.aspx?ChannelGaugeID=" + _GaugeID;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return URL;
        }

        protected void ddlGaugeCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlGaugeCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlGaugeCategory.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                    TextBox txtGaugeRDLeft = (TextBox)gvRow.FindControl("txtGaugeRDLeft");
                    TextBox txtGaugeRDRight = (TextBox)gvRow.FindControl("txtGaugeRDRight");

                    Label lblEditGaugeRD = (Label)gvRow.FindControl("lblEditGaugeRD");
                    Panel panelRDs = (Panel)gvRow.FindControl("panelRDs");

                    if (!string.IsNullOrEmpty(ddlGaugeCategory.SelectedItem.Value) && Convert.ToInt16(ddlGaugeCategory.SelectedItem.Value) == (Int16)Constants.GaugeCategory.SectionalGauge)
                    {
                        Int64 sectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);
                        CO_ChannelIrrigationBoundaries irrigationBoundary = new ChannelBLL().GetIrrigationBoundaryBySection(Convert.ToInt64(hdnChannelID.Value), sectionID);
                        if (irrigationBoundary != null)
                        {
                            Tuple<string, string> tupleRD = Calculations.GetRDValues(irrigationBoundary.SectionRD);
                            if (txtGaugeRDLeft != null)
                                txtGaugeRDLeft.Text = tupleRD.Item1;
                            if (txtGaugeRDRight != null)
                                txtGaugeRDRight.Text = tupleRD.Item2;
                            if (lblEditGaugeRD != null)
                                lblEditGaugeRD.Text = Calculations.GetRDText(irrigationBoundary.SectionRD);

                            if (panelRDs != null && lblEditGaugeRD != null)
                            {
                                panelRDs.CssClass = "hidden";
                                lblEditGaugeRD.CssClass = "block";
                            }
                        }

                    }
                    else if (string.IsNullOrEmpty(ddlGaugeCategory.SelectedItem.Value) || Convert.ToInt16(ddlGaugeCategory.SelectedItem.Value) == (Int16)Constants.GaugeCategory.CriticalGauge)
                    {
                        if (txtGaugeRDLeft != null)
                            txtGaugeRDLeft.Text = string.Empty;
                        if (txtGaugeRDRight != null)
                            txtGaugeRDRight.Text = string.Empty;

                        if (panelRDs != null && lblEditGaugeRD != null)
                        {
                            panelRDs.CssClass = "block";
                            lblEditGaugeRD.CssClass = "hidden";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSection = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSection.NamingContainer;
                if (gvRow != null)
                {
                    DropDownList ddlGaugeCategory = (DropDownList)gvRow.FindControl("ddlGaugeCategory");
                    Dropdownlist.DDLGaugeCategories(ddlGaugeCategory);
                    Dropdownlist.SetSelectedValue(ddlGaugeCategory, string.Empty);
                }
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionalGL2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionalGL2.SelectedItem.Value != "")
                {
                    //ddlSubDivisionalGL1.Enabled = true;
                    ddlSubDivisionalGL1.Attributes.Add("required", "true");
                    ddlSubDivisionalGL1.CssClass = "form-control required";
                    ddlSubDivisionalGL1.Enabled = true;
                    hdnDivID.Value = Convert.ToString(ddlDivisionalGL2.SelectedItem.Value);
                    Dropdownlist.DDLSubDivisionByChannelID(ddlSubDivisionalGL1, Convert.ToInt64(ddlDivisionalGL2.SelectedItem.Value));

                }
                else
                {
                    ddlSubDivisionalGL1.Attributes.Remove("required");
                    ddlSubDivisionalGL1.CssClass = "form-control";
                    //ddlSubDivisionalGL1.ClearSelection();
                    ddlSubDivisionalGL1.ClearSelection();
                    ddlSubDivisionalGL1.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlDivisionalGL3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivisionalGL3.SelectedItem.Value != "")
                {
                    // ddlSubDivisionalGL2.Enabled = true;
                    ddlSubDivisionalGL2.Attributes.Add("required", "true");
                    ddlSubDivisionalGL2.CssClass = "form-control required";
                    ddlSubDivisionalGL2.Enabled = true;
                    ddlSection.SelectedItem.Text = "Select";
                    ddlSection.Enabled = false;

                    Dropdownlist.DDLSubDivisionByChannelID(ddlSubDivisionalGL2, Convert.ToInt64(ddlDivisionalGL3.SelectedItem.Value));
                    //Dropdownlist.DDLSectionByChannelID(ddlSection, Convert.ToInt64(hdnChannelID.Value), Convert.ToInt64(ddlSubDivisionalGL2.SelectedItem.Value));

                }
                else
                {
                    ddlSubDivisionalGL2.Attributes.Remove("required");
                    ddlSubDivisionalGL2.CssClass = "form-control";
                    ddlSection.Attributes.Remove("required");
                    ddlSection.CssClass = "form-control";
                    ddlSubDivisionalGL2.ClearSelection();
                    ddlSection.ClearSelection();
                    ddlSubDivisionalGL2.Enabled = false;
                    ddlSection.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlSubDivisionalGL2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubDivisionalGL2.SelectedItem.Value != "")
                {
                    // ddlSection.Enabled = true;
                    ddlSection.Attributes.Add("required", "true");
                    ddlSection.CssClass = "form-control required";
                    ddlSection.Enabled = true;
                    hdnSubDivID.Value = Convert.ToString(ddlSubDivisionalGL2.SelectedItem.Value);
                    Dropdownlist.DDLSectionByChannelID(ddlSection, Convert.ToInt64(ddlSubDivisionalGL2.SelectedItem.Value));


                }
                else
                {
                    ddlSection.Attributes.Remove("required");
                    ddlSection.CssClass = "form-control";
                    ddlSection.ClearSelection();
                    ddlSection.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long? _GaugeDivID = (ddlDivisionalGL1.SelectedValue) == ""
                    ? (long?)null
                    : Convert.ToInt64(ddlDivisionalGL1.SelectedValue);
                long? _GaugeSubDivID = (ddlSubDivisionalGL1.SelectedValue) == ""
                    ? (long?)null
                    : Convert.ToInt64(ddlSubDivisionalGL1.SelectedItem.Value);
                long? _GaugeSectionID = (ddlSection.SelectedValue) == ""
                    ? (long?)null
                    : Convert.ToInt64(ddlSection.SelectedItem.Value);


                bool IsRecordUpdated = new ChannelBLL().UpdateChannelGaugeByChannelID(Convert.ToInt64(hdnID.Value),
                    _GaugeDivID, _GaugeSubDivID, _GaugeSectionID);
                if (IsRecordUpdated)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindGaugeInformationGridView(Convert.ToInt64(hdnChannelID.Value));
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadAllDropdownlistsData(long _ID)
        {
            try
            {
                CO_ChannelGauge ChannelGauge = new ChannelBLL().GetChannelGaugeByID(_ID);
                if (ChannelGauge != null)
                {
                    long? CircleID = new ChannelBLL().GetCircleIDByDivID(Convert.ToInt64(ChannelGauge.GaugeDivID));
                    if (ChannelGauge.GaugeDivID != null)
                    {

                        //Dropdownlist.DDLDivisionByID(ddlDivisionalGL1, Convert.ToInt64(hdnDivID.Value));
                        //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL1, Convert.ToInt64(CircleID));
                        Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL1);

                        Dropdownlist.SetSelectedValue(ddlDivisionalGL1, Convert.ToString(ChannelGauge.GaugeDivID));
                    }
                    if (ChannelGauge.GaugeSubDivID != null)
                    {
                        long? DivisionID = new ChannelBLL().GetDivisionIDBySubDivID(Convert.ToInt64(ChannelGauge.GaugeSubDivID));
                        Dropdownlist.DDLSubDivisionByChannelID(ddlSubDivisionalGL1, Convert.ToInt64(DivisionID));
                        Dropdownlist.SetSelectedValue(ddlSubDivisionalGL1, Convert.ToString(ChannelGauge.GaugeSubDivID));
                        ddlSubDivisionalGL1.Enabled = true;
                        ddlSubDivisionalGL1.Attributes.Add("required", "true");
                        ddlSubDivisionalGL1.CssClass = "form-control required";
                        //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL2, Convert.ToInt64(Convert.ToInt64(CircleID)));
                        Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL2);

                        Dropdownlist.SetSelectedValue(ddlDivisionalGL2, Convert.ToString(DivisionID));
                    }
                    if (ChannelGauge.GaugeSecID != null)
                    {
                        long? SubDivisionID =
                            new ChannelBLL().GetSubDivIDBySectionID(Convert.ToInt64(ChannelGauge.GaugeSecID));
                        Dropdownlist.DDLSectionByChannelID(ddlSection, Convert.ToInt64(SubDivisionID));
                        Dropdownlist.SetSelectedValue(ddlSection, Convert.ToString(ChannelGauge.GaugeSecID));
                        ddlSection.Enabled = true;
                        ddlSection.Attributes.Add("required", "true");
                        ddlSection.CssClass = "form-control required";
                        long? DivID = new ChannelBLL().GetDivisionIDBySubDivID(Convert.ToInt64(SubDivisionID));
                        Dropdownlist.DDLSubDivisionByChannelID(ddlSubDivisionalGL2, Convert.ToInt64(DivID));
                        Dropdownlist.SetSelectedValue(ddlSubDivisionalGL2, Convert.ToString(SubDivisionID));
                        ddlSubDivisionalGL2.Enabled = true;
                        ddlSubDivisionalGL2.Attributes.Add("required", "true");
                        ddlSubDivisionalGL2.CssClass = "form-control required";
                        //Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL3, Convert.ToInt64(Convert.ToInt64(CircleID)));
                        Dropdownlist.DDLDivisionByChannelID(ddlDivisionalGL3);

                        Dropdownlist.SetSelectedValue(ddlDivisionalGL3, Convert.ToString(DivID));
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ddlDivisionalGL1.Attributes.Remove("required");
                ddlDivisionalGL1.CssClass = "form-control";
                ddlDivisionalGL2.Attributes.Remove("required");
                ddlDivisionalGL2.CssClass = "form-control";
                ddlDivisionalGL3.Attributes.Remove("required");
                ddlDivisionalGL3.CssClass = "form-control";
                ddlSubDivisionalGL1.Attributes.Remove("required");
                ddlSubDivisionalGL1.CssClass = "form-control";
                ddlSubDivisionalGL2.Attributes.Remove("required");
                ddlSubDivisionalGL2.CssClass = "form-control";
                ddlSection.Attributes.Remove("required");
                ddlSection.CssClass = "form-control";
                //LoadGaugeInformation(Convert.ToInt64(hdnChannelID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


    }
}