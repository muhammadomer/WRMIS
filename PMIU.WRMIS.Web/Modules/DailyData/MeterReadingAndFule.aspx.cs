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
    public partial class MeterReadingAndFule : BasePage
    {
        #region Hash Table Keys

        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string ActivityNameIDKey = "ActivityNameID";
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
                    BindDropdowns();
                    SetPageTitle();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    DateTime CurrentDate = DateTime.Now;
                    txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(CurrentDate.AddDays(-1)));
                    if (SessionManagerFacade.UserInformation.DesignationID == null)
                    {
                        BindDropdowns();
                    }
                    else if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.MA)))
                    {
                        ddlActivityBy.Items.Clear();
                        ddlActivityBy.Items.Insert(0, new ListItem("MA", ""));
                        ddlADM.Enabled = false;
                        Dropdownlist.DDLMeterReading(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 13);
                    }
                    else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                    {
                        ddlActivityBy.Items.Clear();
                        ddlActivityBy.Items.Insert(0, new ListItem("ADM", ""));
                        Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                        ddlADM.Enabled = true;
                    }

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
                long? SelectedUserID = null;
                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }

                if (ddlactivity.SelectedItem.Text != "Select")
                {
                    SelectedActivity = Convert.ToString(ddlactivity.SelectedItem.Text).ToUpper();
                }
                SearchCriteria.Add(ActivityNameIDKey, SelectedActivity);

                if (ddlADM.SelectedItem.Value != String.Empty && ddlADM.SelectedItem.Value != "0")
                {
                    SelectedUserID = Convert.ToInt64(ddlADM.SelectedItem.Value);
                }
                SearchCriteria.Add(ADMIDKey, ddlADM.SelectedItem.Value);

                if (ddlMA.SelectedItem.Value != String.Empty && ddlMA.SelectedItem.Value != "0")
                {
                    SelectedUserID = Convert.ToInt64(ddlMA.SelectedItem.Value);
                }
                SearchCriteria.Add(MAIDKey, ddlMA.SelectedItem.Value);

                //var ID = _FloodInspectionID == 0 ? (long?)null : _FloodInspectionID;

                DataSet DS = new DailyDataBLL().GetMeterReadingSearch(SelectedActivity, FromDate, ToDate, SelectedUserID);

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvMeterReadingAndFule.DataSource = DS.Tables[0];
                }
                gvMeterReadingAndFule.DataBind();

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

        protected void gvMeterReadingAndFule_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMeterReadingAndFule.EditIndex = -1;
                BindSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterReadingAndFule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMeterReadingAndFule.PageIndex = e.NewPageIndex;
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
                BindSearchResultsGrid();
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
                Dropdownlist.BindDropdownlist<List<object>>(ddlactivity, CommonLists.GetActivity(), (int)Constants.DropDownFirstOption.Select);
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
                if (ddlActivityBy.SelectedItem.Value != "")
                {
                    if (ddlActivityBy.SelectedItem.Text == "ADM")
                    {
                        ddlADM.Enabled = true;
                        ddlMA.Items.Clear();
                        ddlADM.Items.Insert(0, new ListItem("All", ""));
                        ddlMA.Items.Insert(0, new ListItem("All", ""));
                        gvMeterReadingAndFule.Columns[5].Visible = false;
                        Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                        gvMeterReadingAndFule.DataSource = "";
                        gvMeterReadingAndFule.DataBind();
                    }
                    else if (ddlActivityBy.SelectedItem.Text == "MA")
                    {
                        ddlADM.SelectedItem.Text = "All";
                        ddlADM.Enabled = false;
                        gvMeterReadingAndFule.Columns[5].Visible = true;
                        Dropdownlist.DDLMeterReading(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 13);
                        gvMeterReadingAndFule.DataSource = "";
                        gvMeterReadingAndFule.DataBind();
                    }
                }
                else
                {
                    ddlADM.Enabled = true;
                    ddlMA.Items.Clear();
                    ddlADM.Items.Clear();
                    ddlMA.Items.Insert(0, new ListItem("All", ""));
                    ddlADM.Items.Insert(0, new ListItem("All", ""));
                    gvMeterReadingAndFule.DataSource = "";
                    gvMeterReadingAndFule.DataBind();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlActivity.SelectedItem.Value != "")
                {
                    if (ddlActivityBy.SelectedItem.Text == "METER")
                    {
                        gvMeterReadingAndFule.Columns[5].Visible = false;
                        //gvMeterReadingAndFule.DataSource = "";
                        //gvMeterReadingAndFule.DataBind();
                    }
                    else if (ddlActivityBy.SelectedItem.Text == "FUEL")
                    {

                        gvMeterReadingAndFule.Columns[5].Visible = true;
                        gvMeterReadingAndFule.DataSource = "";
                        gvMeterReadingAndFule.DataBind();
                    }
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
                if (ddlADM.SelectedItem.Value != "")
                {
                    Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(ddlADM.SelectedItem.Value));
                    gvMeterReadingAndFule.DataSource = "";
                    gvMeterReadingAndFule.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMeterReadingAndFule_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }
}