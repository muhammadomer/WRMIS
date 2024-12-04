using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class GaugeBulkEntry : BasePage
    {
        #region DataKeys Indexes

        public int ReadingIDIndex = 0;
        public int MinValueIndex = 1;
        public int MaxValueIndex = 2;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    btnSave.Enabled = base.CanAdd;
                    SetPageTitle();
                    BindZoneDropdown();
                    SetUserLocation();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 09-03-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OperationalData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 09-03-2017
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone);
        }

        /// <summary>
        /// This function sets the location dropdown for Level 3 User
        /// Created On 14-03-2017
        /// </summary>
        private void SetUserLocation()
        {
            UA_AssociatedLocation mdlAssociatedLocations = SessionManagerFacade.UserAssociatedLocations;

            if (mdlAssociatedLocations != null && mdlAssociatedLocations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                CO_Division mdlDivision = new DivisionBLL().GetByID(mdlAssociatedLocations.IrrigationBoundryID.Value);

                if (mdlDivision != null)
                {
                    CO_Circle mdlCircle = new CircleBLL().GetByID(mdlDivision.CircleID.Value);

                    if (mdlCircle != null)
                    {
                        ddlZone.ClearSelection();
                        Dropdownlist.SetSelectedValue(ddlZone, mdlCircle.ZoneID.ToString());

                        ddlZone_SelectedIndexChanged(null, null);

                        ddlCircle.ClearSelection();
                        Dropdownlist.SetSelectedValue(ddlCircle, mdlCircle.ID.ToString());

                        ddlCircle_SelectedIndexChanged(null, null);

                        ddlDivision.ClearSelection();
                        Dropdownlist.SetSelectedValue(ddlDivision, mdlDivision.ID.ToString());

                        ddlDivision_SelectedIndexChanged(null, null);

                        Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetSession());
                        ddlSession.Enabled = true;

                        lblDate.Text = Utility.GetFormattedDate(DateTime.Now);

                        btnSearch.Enabled = true;
                    }
                }
            }
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                Dropdownlist.DDLCircles(ddlCircle, false, ZoneID);
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
                long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                Dropdownlist.DDLDivisions(ddlDivision, false, CircleID);
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
                long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID);

                ddlSubDivision.Enabled = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSubDivision.SelectedItem.Value != string.Empty)
                {
                    long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);

                    Dropdownlist.DDLSections(ddlSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.All);

                    ddlSection.Enabled = true;
                }
                else
                {
                    ddlSection.ClearSelection();
                    ddlSection.Enabled = false;
                }

                dvGrid.Attributes["style"] = "display:none;";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds data to the grid.
        /// Created On 14-03-2017
        /// </summary>
        private void BindGrid()
        {
            long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);

            long SectionID = -1;

            if (ddlSection.SelectedItem.Value != string.Empty)
            {
                SectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);
            }

            int Session = Convert.ToInt32(ddlSession.SelectedItem.Value);
            DateTime ReadingDate = DateTime.Now.Date;

            List<DD_GetGaugesBulkEntry_Result> lstGaugesBulkEntry = new DailyDataBLL().GetGaugesBulkData(SubDivisionID, SectionID, Session, ReadingDate);

            gvGaugeReading.DataSource = lstGaugesBulkEntry;
            gvGaugeReading.DataBind();

            dvGrid.Attributes["style"] = "display:block;";
        }

        protected void gvGaugeReading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblReducedDistance = (Label)e.Row.FindControl("lblReducedDistance");
                    double ReducedDistance = Convert.ToDouble(lblReducedDistance.Text);
                    lblReducedDistance.Text = Calculations.GetRDText(ReducedDistance);

                    HiddenField hdnGaugeID = (HiddenField)e.Row.FindControl("hdnGaugeID");
                    long GaugeID = Convert.ToInt64(hdnGaugeID.Value);

                    double MinValue = Convert.ToDouble(gvGaugeReading.DataKeys[e.Row.RowIndex].Values[MinValueIndex]);
                    double MaxValue = Convert.ToDouble(gvGaugeReading.DataKeys[e.Row.RowIndex].Values[MaxValueIndex]);

                    TextBox txtGaugeValue = (TextBox)e.Row.FindControl("txtGaugeValue");
                    TextBox txtTime = (TextBox)e.Row.FindControl("txtTime");
                    Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                    HiddenField hdnDischarge = (HiddenField)e.Row.FindControl("hdnDischarge");

                    if (gvGaugeReading.DataKeys[e.Row.RowIndex].Values[ReadingIDIndex] == null)
                    {
                        txtGaugeValue.Attributes.Add("onblur", "javascript:CalculateDischarge('" + GaugeID + "','" + txtGaugeValue.ClientID + "','" + lblDischarge.ClientID + "','" + hdnDischarge.ClientID + "','" + txtTime.ClientID + "');");
                        txtGaugeValue.Attributes.Add("oninput", "javascript:ValueValidation(this,'" + MinValue + "','" + MaxValue + "');");
                        txtGaugeValue.Attributes.Add("placeholder", string.Format("{0} - {1}", MinValue, MaxValue));

                        if (Convert.ToInt32(ddlSession.SelectedItem.Value) == (int)Constants.SessionOrShift.Morning)
                        {
                            txtTime.Text = "09:00";
                        }
                        else
                        {
                            txtTime.Text = "04:00";
                        }
                    }
                    else
                    {
                        Label lblTime = (Label)e.Row.FindControl("lblTime");

                        DateTime ReadingDateTime = Convert.ToDateTime(txtTime.Text);
                        lblTime.Text = Utility.GetParsedTime(ReadingDateTime);

                        txtTime.Visible = false;
                        lblTime.Visible = true;

                        Label lblGaugeValue = (Label)e.Row.FindControl("lblGaugeValue");

                        txtGaugeValue.Visible = false;
                        lblGaugeValue.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function calculates the discharge based on the Gauge ID and Gauge Value
        /// Created On 13-03-2017
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns>double?</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static double? CalculateDischarge(long _GaugeID, double _GaugeValue)
        {
            double? Discharge = new DailyDataBLL().CalculateDischarge(_GaugeID, _GaugeValue);

            return Discharge;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool? Success = null;

                DateTime ReadingDateTime = DateTime.Now.Date;
                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                foreach (GridViewRow Row in gvGaugeReading.Rows)
                {
                    HiddenField hdnDischarge = (HiddenField)Row.FindControl("hdnDischarge");
                    TextBox txtTime = (TextBox)Row.FindControl("txtTime");

                    if (hdnDischarge.Value.Trim() != string.Empty && txtTime.Visible == true)
                    {
                        string Time = txtTime.Text;

                        string[] SplitedTime = Time.Split(':');
                        int Hours = Convert.ToInt32(SplitedTime[0]);
                        int Minutes = Convert.ToInt32(SplitedTime[1]);

                        if (Convert.ToInt32(ddlSession.SelectedItem.Value) == (int)Constants.SessionOrShift.Evening)
                        {
                            Hours = Hours + 12;
                        }

                        ReadingDateTime = ReadingDateTime.AddHours(Hours);
                        ReadingDateTime = ReadingDateTime.AddMinutes(Minutes);

                        HiddenField hdnGaugeID = (HiddenField)Row.FindControl("hdnGaugeID");
                        long GaugeID = Convert.ToInt64(hdnGaugeID.Value);

                        TextBox txtGaugeValue = (TextBox)Row.FindControl("txtGaugeValue");
                        double GaugeValue = Convert.ToDouble(txtGaugeValue.Text.Trim());

                        string Result = new DailyDataBLL().AddGaugeReading(mdlUsers.ID, GaugeID, true, true, GaugeValue, null, null, null, null, "W", ReadingDateTime);

                        if (Result.StartsWith("SUCCESS"))
                        {
                            string[] Response = Result.Split('-');
                            string GaugeReadingID = Response[1];

                            NotifyEvent Event = new NotifyEvent();
                            Event.Parameters.Add("DailyGaugeReadingID", GaugeReadingID);
                            Event.AddNotifyEvent((long)NotificationEventConstants.DailyData.AddDailyDate, mdlUsers.ID);

                            Success = true;
                        }
                        else
                        {
                            Success = false;
                        }

                        ReadingDateTime = DateTime.Now.Date;
                    }
                }

                if (Success != null)
                {
                    if (Success.Value)
                    {
                        Master.ShowMessage(Message.ValidRecordsSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.SomeRecordsNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}