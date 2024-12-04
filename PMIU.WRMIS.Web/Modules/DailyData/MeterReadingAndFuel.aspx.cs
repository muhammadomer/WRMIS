using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class MeterReadingAndFuel : BasePage
    {
        #region Hash Table Keys

        public const string ActivityNameIDKey = "ActivityNameID";
        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string ActivityByKey = "ActivityBy";
        public const string ADMIDKey = "ADMID";
        public const string MAIDKey = "MAID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    hlGIS.NavigateUrl = "https://wrmis.irrigation.punjab.gov.pk" + new DailyDataBLL().GetGISURL();

                    BindDropdowns();
                    SetPageTitle();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    //DateTime CurrentDate = DateTime.Now;
                    //txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-1)));
                    if (SessionManagerFacade.UserInformation.DesignationID == null)
                    {
                        BindDropdowns();
                    }
                    else if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.MA)))
                    {
                        ddlActivityBy.Items.Clear();
                        ddlActivityBy.Items.Insert(0, new ListItem("MA", ""));
                        ddlADM.Enabled = false;
                        ddlMA.DataSource = new DailyDataBLL().GetUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.UserID));
                        ddlMA.DataTextField = "Name";
                        ddlMA.DataValueField = "ID";
                        ddlMA.DataBind();
                        //ddlMA.Items.Insert(0, new ListItem("All", ""));
                        //Dropdownlist.DDLMeterReading(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 13);
                        //Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.ID));
                    }
                    else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                    {
                        //ddlActivityBy.Items.Clear();
                        //ddlActivityBy.Items.Insert(0, new ListItem("ADM", ""));
                        //ddlADM.DataSource = new DailyDataBLL().GetUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.UserID));
                        //ddlADM.DataTextField = "Name";
                        //ddlADM.DataValueField = "ID";
                        //ddlADM.DataBind();
                        //  Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                        ddlActivityBy.ClearSelection();
                        ddlActivityBy.Items.FindByText("ADM").Selected = true;
                        ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                        ddlADM.SelectedItem.Value = Convert.ToString(SessionManagerFacade.UserInformation.ID);
                        ddlADM.Enabled = true;
                    }

                    // Maintain state while coming back from another page 


                    if (!string.IsNullOrEmpty(Request.QueryString["ShowSearched"]))
                    {
                        if (Session["MRAF_SC_SearchCriteria"] != null)
                        {
                            Hashtable HT = (Hashtable)Session["MRAF_SC_SearchCriteria"];

                            string ActivityName = Convert.ToString(HT[ActivityNameIDKey]);
                            string FromDate = Convert.ToString(HT[FromDateKey]);
                            string ToDate = Convert.ToString(HT[ToDateKey]);
                            string ActivityBy = Convert.ToString(HT[ActivityByKey]);
                            string ADM = Convert.ToString(HT[ADMIDKey]);
                            string MA = Convert.ToString(HT[MAIDKey]);


                            ddlactivity.Items.FindByText(ActivityName).Selected = true;

                            if (!string.IsNullOrEmpty(FromDate))
                                txtFromDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(FromDate));

                            if (!string.IsNullOrEmpty(ToDate))
                                txtToDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(ToDate));

                            if (!string.IsNullOrEmpty(ActivityBy))
                                ddlActivityBy.Items.FindByText(ActivityBy).Selected = true;

                            if (!string.IsNullOrEmpty(ActivityBy))
                            {
                                ddlActivityBy_SelectedIndexChanged(null, null);

                                if (ddlADM.Items.Count > 0)
                                    if (ddlADM.Items.Count == 1 && ddlADM.Text != "")
                                        ddlADM.Items.FindByText(ADM).Selected = true;

                                if (!string.IsNullOrEmpty(ADM))
                                    ddlADM_SelectedIndexChanged(null, null);

                                if (ddlMA.Items.Count > 0)
                                    if (ddlMA.Items.Count == 1 && ddlMA.Text != "")
                                        ddlMA.Items.FindByText(MA).Selected = true;
                            }
                            BindSearchResultsGrid();
                        }
                    }
                    // end 
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }


        }
        private void BindSearchResultsGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                string SelectedActivity = null;
                long? SelectedActivityBy = null;
                long? SelectedUserID = SessionManagerFacade.UserAssociatedLocations.UserID;
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (ddlactivity.SelectedItem.Text != "Select")
                {
                    SelectedActivity = Convert.ToString(ddlactivity.SelectedItem.Text).ToUpper();
                    SearchCriteria.Add(ActivityNameIDKey, Convert.ToString(ddlactivity.SelectedItem.Text));
                }
                else
                    SearchCriteria.Add(ActivityNameIDKey, "");

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                    SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());
                }
                else
                    SearchCriteria.Add(FromDateKey, "");

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                    SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());
                }
                else
                    SearchCriteria.Add(ToDateKey, "");

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }

                if (ddlActivityBy.SelectedItem.Value != "All" && ddlActivityBy.SelectedItem.Value != "")
                {
                    SelectedActivityBy = Convert.ToInt64(ddlActivityBy.SelectedItem.Value);
                    SearchCriteria.Add(ActivityByKey, ddlActivityBy.SelectedItem.Text);
                }
                else
                    SearchCriteria.Add(ActivityByKey, "");


                if (ddlADM.SelectedItem.Value != String.Empty && ddlADM.SelectedItem.Value != "0")
                {
                    SelectedUserID = Convert.ToInt64(ddlADM.SelectedItem.Value);
                    SearchCriteria.Add(ADMIDKey, ddlADM.SelectedItem.Text);
                }
                else
                    SearchCriteria.Add(ADMIDKey, "");


                if (ddlMA.SelectedItem.Value != "All" && ddlMA.SelectedItem.Value != "")
                {
                    SelectedUserID = Convert.ToInt64(ddlMA.SelectedItem.Value);
                    SearchCriteria.Add(MAIDKey, ddlMA.SelectedItem.Text);
                }
                else
                    SearchCriteria.Add(MAIDKey, "");

                SelectedUserID = SelectedUserID == 0 ? null : SelectedUserID;
                SelectedActivityBy = SelectedActivityBy == 0 ? null : SelectedActivityBy;


                //var ID = _FloodInspectionID == 0 ? (long?)null : _FloodInspectionID;

                //if (SessionManagerFacade.UserInformation.UA_Designations == null || (SessionManagerFacade.UserInformation.UA_Designations != null && SessionManagerFacade.UserInformation.UA_Designations.IrrigationLevelID == null))
                //    SelectedUserID = null;
                DataSet DS = new DataSet();

                if (ddlactivity.SelectedItem.Text == Constants.WaterTheft || ddlactivity.SelectedItem.Text == Constants.CutBreach || ddlactivity.SelectedItem.Text == Constants.ChannelObservation
                   || ddlactivity.SelectedItem.Text == Constants.RotationalViolation || ddlactivity.SelectedItem.Text == Constants.OutletChecking || ddlactivity.SelectedItem.Text == Constants.Leaves
                   || ddlactivity.SelectedItem.Text == Constants.ET_All)
                {
                    DS = new DailyDataBLL().GetWaterTheftSearch(FromDate, ToDate, SelectedUserID, ddlactivity.SelectedItem.Text, SelectedActivityBy);

                    if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                        gvWatertheft.DataSource = DS.Tables[0];
                    else
                        gvWatertheft.DataSource = DS;

                    gvWatertheft.DataBind();
                    gvWatertheft.Visible = true;
                    gvMeterFuelReading.Visible = false;
                }
                else
                {
                    DS = new DailyDataBLL().GetMeterReadingSearch(SelectedActivity, FromDate, ToDate, SelectedUserID, SelectedActivityBy);

                    if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                        gvMeterFuelReading.DataSource = DS.Tables[0];
                    else
                        gvMeterFuelReading.DataSource = DS;

                    gvMeterFuelReading.DataBind();
                    gvWatertheft.Visible = false;
                    gvMeterFuelReading.Visible = true;
                }

                Session["MRAF_SC_SearchCriteria"] = SearchCriteria;
                //IEnumerable<DataRow> ieMeterReading = new DailyDataBLL().GetMeterReadingSearch(SelectedActivity, FromDate, ToDate, SelectedUserID);

                //var lstMeterReadingSearch = ieMeterReading.Select(dataRow => new
                //{
                //    VehicleReadingID = dataRow.Field<long>("VehicleReadingID"),
                //    ReadingType = dataRow.Field<string>("ReadingType"),
                //    MeterReading = dataRow.Field<int>("MeterReading"),
                //    PetrolQuantity = dataRow.Field<float>("PetrolQuantity"),
                //    Remarks = dataRow.Field<string>("Remarks"),
                //    ObserveBY = dataRow.Field<string>("ObserveBY"),
                //    AttachmentFile1 = dataRow.Field<string>("AttachmentFile1"),
                //    ReadingServerDate = dataRow.Field<DateTime>("ReadingServerDate"),
                //    ServerTime = dataRow.Field<DateTime>("ServerTime"),

                //}).ToList();

                //gvMeterReadingAndFule.DataSource = lstMeterReadingSearch;
                //gvMeterReadingAndFule.DataBind();

                // SearchCriteria.Add(PageIndexKey, gvMeterReadingAndFule.PageIndex);

                //Session[SessionValues.SearchMeterReadingAndFuel] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterFuelReading_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMeterFuelReading.EditIndex = -1;
                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterFuelReading_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMeterFuelReading.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);

                if ((ToDate - FromDate).Days < 91) //(FromDate.AddMonths(1) == ToDate || (ToDate - FromDate).Days < 31)
                    BindSearchResultsGrid();
                else
                    Master.ShowMessage("Maximum allowed period for monitoring are last 90 days", PMIU.WRMIS.Web.SiteMaster.MessageType.Error);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDropdowns()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlactivity, CommonLists.GetActivity(), (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<object>>(ddlActivityBy, CommonLists.GetActivityBy(), (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.MeterReadingAndFule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void ddlActivityBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMeterFuelReading.Visible = false;
                gvWatertheft.Visible = false;
                if (ddlActivityBy.SelectedItem.Value != "")
                {
                    if (ddlActivityBy.SelectedItem.Text == "ADM")
                    {
                        if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.ADM)))
                        {

                            ddlADM.Enabled = true;
                            ddlMA.Enabled = false;
                            ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                        }
                        else
                        {
                            ddlADM.Enabled = true;
                            ddlMA.Enabled = false;
                            //ddlMA.Items.Insert(0, new ListItem("All", ""));
                            Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                            //  ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                            gvMeterFuelReading.Visible = false;
                        }

                    }
                    else if (ddlActivityBy.SelectedItem.Text == "MA")
                    {
                        if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.ADM)))
                        {
                            ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                            ddlMA.Enabled = true;
                            Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.UserID));
                            gvMeterFuelReading.Visible = false;
                        }
                        else
                        {
                            ddlMA.Enabled = true;
                            Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                            //  Dropdownlist.DDLMeterReading(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 13);
                            gvMeterFuelReading.Visible = false;
                        }

                    }
                }
                else
                {
                    ddlADM.Enabled = true;
                    ddlMA.Items.Clear();
                    ddlADM.Items.Clear();
                    ddlMA.Items.Insert(0, new ListItem("All", ""));
                    ddlADM.Items.Insert(0, new ListItem("All", ""));
                    gvMeterFuelReading.Visible = false;


                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlADM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMeterFuelReading.Visible = false;
                gvWatertheft.Visible = false;

                if (ddlADM.SelectedItem.Value != "")
                {
                    if (Convert.ToString(ddlActivityBy.SelectedItem.Text) == "ADM")
                    {
                        ddlMA.Items.Clear();
                        ddlMA.Items.Insert(0, new ListItem("All", ""));
                        ddlMA.Enabled = false;
                        //Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(ddlADM.SelectedItem.Value));
                        gvMeterFuelReading.Visible = false;
                    }
                    else
                    {
                        ddlMA.Enabled = true;
                        Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(ddlADM.SelectedItem.Value));
                        gvMeterFuelReading.Visible = false;
                    }
                }
                else
                {
                    ddlMA.Items.Clear();
                    ddlMA.Items.Insert(0, new ListItem("All", ""));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMeterFuelReading.Visible = false;
                gvWatertheft.Visible = false;

                if (ddlactivity.SelectedItem.Value != "")
                {
                    if (ddlactivity.SelectedItem.Text == "Meter")
                    {
                        // gvMeterReadingAndFule.Columns[5].Visible = false;
                        gvMeterFuelReading.Visible = false;
                    }
                    else if (ddlactivity.SelectedItem.Text == "Fuel")
                    {

                        //  gvMeterReadingAndFule.Columns[5].Visible = true;
                        gvMeterFuelReading.Visible = false;
                    }
                }
                else
                {
                    gvMeterFuelReading.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterFuelReading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblAttachment = (Label)e.Row.FindControl("lblAttachment");
                    string AttachmentPath = lblAttachment.Text;
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        List<string> lstName = new List<string>();
                        lstName.Add(AttachmentPath);
                        WebFormsTest.FileUploadControl FileUploadControl1 = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl1");
                        FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                        FileUploadControl1.Size = lstName.Count;
                        FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.VehicleReadings, lstName);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterFuelReading_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvMeterFuelReading.DataKeys[gvrow.RowIndex];

                if (e.CommandName.Equals("ViewImage"))
                {
                    lblUploadedBy.Text = Convert.ToString(key["ObservedBY"]);
                    lblDateTime.Text = Utility.GetFormattedDate(Convert.ToDateTime(Convert.ToString(key["ReadingServerDate"]))) + " " + Utility.GetFormattedTime(Convert.ToDateTime(Convert.ToString(key["ReadingServerDate"])));
                    //ReadingType
                    header.InnerText = "";
                    if (Convert.ToString(key["ReadingType"]).ToLower() == "meter")
                    {
                        header.InnerText = "Meter Reading";
                        thfuelreading.Visible = false;
                        lblFuelReading.Visible = false;
                        thMeterReading.Visible = true;
                        lblMeterReading.Visible = true;
                        lblMeterReading.Text = Convert.ToString(key["MeterReading"]);
                    }
                    else
                    {
                        header.InnerText = "Fuel Reading";
                        thMeterReading.Visible = true;
                        lblMeterReading.Visible = true;
                        lblMeterReading.Text = Convert.ToString(key["MeterReading"]);
                        thfuelreading.Visible = true;
                        lblFuelReading.Visible = true;
                        lblFuelReading.Text = Convert.ToString(key["PetrolQuantity"]);

                    }

                    imgImage.ImageUrl = Utility.GetImageURL(Configuration.VehicleReadings, Convert.ToString(key["AttachmentFile1"]));
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#viewimage').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScript", sb.ToString(), false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #region Search Water Theft

        protected void gvWatertheft_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvWatertheft.PageIndex = e.NewPageIndex;
            BindSearchResultsGrid();
        }

        protected void gvWatertheft_PageIndexChanged(object sender, EventArgs e)
        {
            gvWatertheft.EditIndex = -1;
            BindSearchResultsGrid();
        }

        protected void gvWatertheft_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                DataKey key = gvWatertheft.DataKeys[gvrow.RowIndex];

                string Activity = Convert.ToString(key["Activity"]);

                if (ddlactivity.SelectedItem.Text == Constants.RotationalViolation || Activity == Constants.RotationalViolation)
                {
                    //GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    //DataKey key = gvWatertheft.DataKeys[gvrow.RowIndex];

                    long CaseID = Convert.ToInt64(key["CaseID"]);

                    object Detail = new DailyDataBLL().GetRotationalDetail(CaseID);

                    if (Detail != null)
                    {
                        lblObservedBy.Text = Convert.ToString(Detail.GetType().GetProperty("ObservedBy").GetValue(Detail));
                        lblDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime((Detail.GetType().GetProperty("Date").GetValue(Detail)))));
                        lblHG.Text = Convert.ToString(Detail.GetType().GetProperty("HeadGauge").GetValue(Detail));
                        lblViolation.Text = Convert.ToString(Detail.GetType().GetProperty("Violation").GetValue(Detail));
                        lblGP.Text = Convert.ToString(Detail.GetType().GetProperty("GroupPreference").GetValue(Detail));
                        lblRemarks.Text = Convert.ToString(Detail.GetType().GetProperty("Remarks").GetValue(Detail));
                        lblChnlName.Text = Convert.ToString(Detail.GetType().GetProperty("ChannelName").GetValue(Detail));
                        lblDivName.Text = Convert.ToString(Detail.GetType().GetProperty("DivisionName").GetValue(Detail));
                        string Attachment = Convert.ToString(Detail.GetType().GetProperty("Attachment").GetValue(Detail));

                        imgRotational.ImageUrl = Utility.GetImageURL(Configuration.EmployeeTracking, Attachment);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#idViewRotational').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScriptRotational", sb.ToString(), false);
                    }
                }
                else if (ddlactivity.SelectedItem.Text == Constants.Leaves || Activity == Constants.Leaves)
                {
                    //GridViewRow gvrow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    //DataKey key = gvWatertheft.DataKeys[gvrow.RowIndex];

                    long CaseID = Convert.ToInt64(key["CaseID"]);

                    object Detail = new DailyDataBLL().GetLeavesDetail(CaseID);

                    if (Detail != null)
                    {
                        lblLObservedBy.Text = Convert.ToString(Detail.GetType().GetProperty("ObservedBy").GetValue(Detail));
                        lblLDate.Text = Convert.ToString(Utility.GetFormattedDate(Convert.ToDateTime((Detail.GetType().GetProperty("Date").GetValue(Detail)))));
                        lblLLeaveType.Text = Convert.ToString(Detail.GetType().GetProperty("LeaveType").GetValue(Detail));
                        lblLRainGauge.Text = Convert.ToString(Detail.GetType().GetProperty("RainGauge").GetValue(Detail));
                        lblLRemarks.Text = Convert.ToString(Detail.GetType().GetProperty("Remarks").GetValue(Detail));
                        string Attachment = Convert.ToString(Detail.GetType().GetProperty("Attachment").GetValue(Detail));

                        imgLimage.ImageUrl = Utility.GetImageURL(Configuration.EmployeeTracking, Attachment);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#idViewLeaves').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScriptLeave", sb.ToString(), false);
                    }
                }
                else if (Activity == Constants.Meter || Activity == Constants.UMeter || Activity == Constants.Fuel || Activity == Constants.UFuel)
                {
                    long CaseID = Convert.ToInt64(key["CaseID"]);
                    object Detail = new DailyDataBLL().GetFuelMeterDetail(CaseID);

                    if (Detail != null)
                    {
                        lblUploadedBy.Text = Convert.ToString(Detail.GetType().GetProperty("ObservedBy").GetValue(Detail));
                        lblDateTime.Text = Utility.GetFormattedDate(Convert.ToDateTime(Detail.GetType().GetProperty("Date").GetValue(Detail)));
                        //ReadingType

                        header.InnerText = "";


                        if (Activity == Constants.Meter)
                        {
                            header.InnerText = "Meter Reading";
                            thfuelreading.Visible = false;
                            lblFuelReading.Visible = false;
                            thMeterReading.Visible = true;
                            lblMeterReading.Visible = true;
                            lblMeterReading.Text = Convert.ToString(Detail.GetType().GetProperty("MeterReading").GetValue(Detail));
                        }
                        else
                        {
                            header.InnerText = "Fuel Reading";
                            thMeterReading.Visible = true;
                            lblMeterReading.Visible = true;
                            lblMeterReading.Text = Convert.ToString(Detail.GetType().GetProperty("MeterReading").GetValue(Detail));
                            thfuelreading.Visible = true;
                            lblFuelReading.Visible = true;
                            lblFuelReading.Text = Convert.ToString(Detail.GetType().GetProperty("Petrol").GetValue(Detail));
                        }

                        imgImage.ImageUrl = Utility.GetImageURL(Configuration.VehicleReadings, Convert.ToString(Detail.GetType().GetProperty("Attachment").GetValue(Detail)));
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#viewimage').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "viewimageScript", sb.ToString(), false);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvWatertheft_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hlink = (HyperLink)e.Row.FindControl("hlView");
                    Button btn = (Button)e.Row.FindControl("btnViewRota");
                    long CaseID = Convert.ToInt64(gvWatertheft.DataKeys[e.Row.RowIndex].Values[0]);

                    if (ddlactivity.SelectedItem.Text == Constants.WaterTheft)
                    {
                        string WT_HeaderInformationScreen = "~/Modules/WaterTheft/WTSBESDOWorking.aspx?WaterTheftID={0}&ET={1}";
                        hlink.NavigateUrl = string.Format(WT_HeaderInformationScreen, CaseID, 1);
                        hlink.Visible = true;
                        btn.Visible = false;
                    }
                    else if (ddlactivity.SelectedItem.Text == Constants.CutBreach)
                    {
                        string CaseDetail = "~/Modules/WaterTheft/ViewBreachCase.aspx?BreachCaseID={0}&ET={1}";
                        hlink.NavigateUrl = string.Format(CaseDetail, CaseID, 1);
                        hlink.Visible = true;
                        btn.Visible = false;
                    }
                    else if (ddlactivity.SelectedItem.Text == Constants.ChannelObservation)
                    {
                        bool Scheduled = Convert.ToBoolean(gvWatertheft.DataKeys[e.Row.RowIndex].Values[1]);
                        string CaseDetail = "~/Modules/ScheduleInspection/AddGaugeInspection.aspx?ScheduleDetailID={0}&IsScheduled={1}&ET={1}";
                        hlink.NavigateUrl = string.Format(CaseDetail, CaseID, Scheduled, 1);
                        hlink.Visible = true;
                        btn.Visible = false;
                    }
                    else if (ddlactivity.SelectedItem.Text == Constants.OutletChecking)
                    {
                        long OutleID = Convert.ToInt64(gvWatertheft.DataKeys[e.Row.RowIndex].Values[1]);
                        string CaseDetail = "~/Modules/ScheduleInspection/OutletChecking.aspx?OutletCheckingID={0}&Outlet={1}&ET={2}";
                        hlink.NavigateUrl = string.Format(CaseDetail, CaseID, OutleID, 1);
                        hlink.Visible = true;
                        btn.Visible = false;
                    }
                    else if (ddlactivity.SelectedItem.Text == Constants.RotationalViolation || ddlactivity.SelectedItem.Text == Constants.Leaves)
                    {
                        hlink.Visible = false;
                        btn.Visible = true;
                    }
                    else if (ddlactivity.SelectedItem.Text == Constants.ET_All)
                    {
                        String Activity = Convert.ToString(gvWatertheft.DataKeys[e.Row.RowIndex].Values[2]);

                        if (Activity == Constants.RotationalViolation || Activity == Constants.Leaves || Activity == Constants.Fuel || Activity == Constants.UFuel || Activity == Constants.Meter || Activity == Constants.UMeter)
                        {
                            hlink.Visible = false;
                            btn.Visible = true;
                        }
                        else if (Activity == Constants.WaterTheft)
                        {
                            string WT_HeaderInformationScreen = "~/Modules/WaterTheft/WTSBESDOWorking.aspx?WaterTheftID={0}&ET={1}";
                            hlink.NavigateUrl = string.Format(WT_HeaderInformationScreen, CaseID, 1);
                            hlink.Visible = true;
                            btn.Visible = false;
                        }
                        else if (Activity == Constants.CutBreach)
                        {
                            string CaseDetail = "~/Modules/WaterTheft/ViewBreachCase.aspx?BreachCaseID={0}&ET={1}";
                            hlink.NavigateUrl = string.Format(CaseDetail, CaseID, 1);
                            hlink.Visible = true;
                            btn.Visible = false;
                        }
                        else if (Activity == Constants.ChannelObservation)
                        {
                            bool Scheduled = Convert.ToBoolean(gvWatertheft.DataKeys[e.Row.RowIndex].Values[1]);
                            string CaseDetail = "~/Modules/ScheduleInspection/AddGaugeInspection.aspx?ScheduleDetailID={0}&IsScheduled={1}&ET={1}";
                            hlink.NavigateUrl = string.Format(CaseDetail, CaseID, Scheduled, 1);
                            hlink.Visible = true;
                            btn.Visible = false;
                        }
                        else if (Activity == Constants.OutletChecking)
                        {
                            long OutleID = Convert.ToInt64(gvWatertheft.DataKeys[e.Row.RowIndex].Values[1]);
                            string CaseDetail = "~/Modules/ScheduleInspection/OutletChecking.aspx?OutletCheckingID={0}&Outlet={1}&ET={2}";
                            hlink.NavigateUrl = string.Format(CaseDetail, CaseID, OutleID, 1);
                            hlink.Visible = true;
                            btn.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

    }
}