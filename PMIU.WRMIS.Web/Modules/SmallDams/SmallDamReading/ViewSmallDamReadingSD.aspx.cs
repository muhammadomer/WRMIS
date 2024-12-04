using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common;

using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using System.Collections;
using Microsoft.Reporting.WebForms;

namespace PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading
{
    public partial class ViewSmallDamReadingSD : BasePage
    {

        #region Initialize
        long LoggedUser = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    SmallDamsBLL smallDamBll = new SmallDamsBLL();
                    LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
                    InitialBind();

                }
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Events

        #region Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                Master.ShowMessage("Enter dates are invalid.", SiteMaster.MessageType.Error);
            }
            else
            {
                BindViewDamChannelReadingGrid();
            }    
            

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

                //call dam report
                if (ddlDivision.Items.Count > 0)
                {
                    long DamID = ddlDamName.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDamName.SelectedItem.Value);
                    long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                    DateTime FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                    DateTime ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());

                    long ChannelID = 0;
                    if (ddlChannel.SelectedIndex != 0)
                    {
                        ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlChannel.SelectedItem.Value);
                    }


                    ReportData mdlReportData = new ReportData();

                    ReportParameter ReportParameter = new ReportParameter("DamID", DamID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("DivisionID", DivisionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("SubDivisionID", SubDivisionID.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("FromDate", FromDate.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    ReportParameter = new ReportParameter("ToDate", ToDate.ToString());
                    mdlReportData.Parameters.Add(ReportParameter);

                    if (ddlChannel.SelectedIndex != 0)
                    {
                        ReportParameter = new ReportParameter("channelID", ChannelID.ToString());
                        mdlReportData.Parameters.Add(ReportParameter);
                    }

                    if (ddlChannel.SelectedIndex != 0)
                    {
                        mdlReportData.Name = Constants.SDChannelReading;

                    }
                    else
                    {
                        mdlReportData.Name = Constants.SDDamReading;
                    }
                    Session[SessionValues.ReportData] = mdlReportData;
                    string ReportViwerurl = "../" + Constants.ReportsUrl;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + ReportViwerurl + "','_blank');</script>", false);
                }




            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Grid
        
        protected void gvChannelReading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Key
                //SmallChannelID,ID,Capacity,Gauge,Discharge,ReadingDate,FromTime,ToTime,ReaderName,Remarks,CreatedDate,CreatedBy"

                DataKey key = gvChannelReading.DataKeys[e.Row.RowIndex];
                string ID = key.Values["ID"].ToString();
                string Capacity = Convert.ToString(key.Values["Capacity"]);
                string Gauge = Convert.ToString(key.Values["Gauge"]);
                string Discharge = Convert.ToString(key.Values["Discharge"]);
                string ReadingDate = Convert.ToString(key.Values["ReadingDate"]);
                string FromTime = Convert.ToString(key.Values["FromTime"]);
                string ToTime = Convert.ToString(key.Values["ToTime"]);
                string ReaderName = Convert.ToString(key.Values["ReaderName"]);
                string Remarks = Convert.ToString(key.Values["Remarks"]);
                #endregion
                #region Control
                Label lblChannelCapacity = (Label)e.Row.FindControl("lblChannelCapacity");
                Label lblGauge = (Label)e.Row.FindControl("lblGauge");
                Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                Label lblReadingDate = (Label)e.Row.FindControl("lblReadingDate");
                Label lblFromDate = (Label)e.Row.FindControl("lblFromDate");
                Label lblFromTime = (Label)e.Row.FindControl("lblFromTime");
                Label lblToDate = (Label)e.Row.FindControl("lblToDate");
                Label lblToTime = (Label)e.Row.FindControl("lblToTime");
                Label lblReaderName = (Label)e.Row.FindControl("lblReaderName");
                Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");

                #endregion
                if (ReadingDate != "")
                {
                    lblReadingDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ReadingDate));
                }
                else
                {
                    lblFromTime.Text = "";
                }

                if (Capacity != "")
                    lblChannelCapacity.Text = Capacity;

                if (Gauge != "")
                    lblGauge.Text = String.Format("{0:0.00}", Convert.ToDouble(Gauge));
                else lblGauge.Text = "";

                if (Discharge != "")
                    lblDischarge.Text = String.Format("{0:0.00}",Convert.ToDouble(Discharge));
                else lblDischarge.Text = "";

                if (FromTime != "")
                {
                    lblFromTime.Text = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(FromTime));
                    
                }
                else
                {
                    lblFromTime.Text = "";
                }

                if (ToTime != "")
                {
                    lblToTime.Text = String.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(ToTime));
                    
                }
                else
                {
                    lblToTime.Text = "";
                }

                if (ReaderName != "")
                    lblReaderName.Text = ReaderName;
                else lblReaderName.Text = "";

                if (Remarks != "")
                    lblRemarks.Text = Remarks;
                else lblRemarks.Text = "";

            }
        }
        protected void gvChannelReading_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvChannelReading.EditIndex = -1;
                BindViewDamChannelReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvChannelReading_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChannelReading.PageIndex = e.NewPageIndex;
                gvChannelReading.EditIndex = -1;
                BindViewDamChannelReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        

        protected void gvDamReadings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Key
                //ID,ReadingDate,DamLevel,LiveStorage,Discharge,ReaderName,Remarks,CreatedDate,CreatedBy"

                DataKey key = gvDamReadings.DataKeys[e.Row.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string ReadingDate = Convert.ToString(key.Values["ReadingDate"]);
                string DamLevel = Convert.ToString(key.Values["DamLevel"]);
                string LiveStorage = Convert.ToString(key.Values["LiveStorage"]);
                string Discharge = Convert.ToString(key.Values["Discharge"]);
                string ReaderName = Convert.ToString(key.Values["ReaderName"]);
                string Remarks = Convert.ToString(key.Values["Remarks"]);
                #endregion
                #region Control
                Label lblReadingDate = (Label)e.Row.FindControl("lblReadingDate");
                Label lblDamLevel = (Label)e.Row.FindControl("lblDamLevel");
                Label lblLiveStorage = (Label)e.Row.FindControl("lblLiveStorage");
                Label lblDischarge = (Label)e.Row.FindControl("lblDischarge");
                Label lblReaderName = (Label)e.Row.FindControl("lblReaderName");
                Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");


                #endregion
                if (ReadingDate != "")
                    lblReadingDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ReadingDate));

                if (DamLevel != "")
                    lblDamLevel.Text = String.Format("{0:0.00}",Convert.ToDouble(DamLevel));
                if (LiveStorage != "")
                    lblLiveStorage.Text = String.Format("{0:0.00}",Convert.ToDouble(LiveStorage));

                if (Discharge != "")
                    lblDischarge.Text = String.Format("{0:0.00}",Convert.ToDouble(Discharge));
                else lblDischarge.Text = "";

                if (ReaderName != "")
                    lblReaderName.Text = ReaderName;
                else lblReaderName.Text = "";
                if (Remarks != "")
                    lblRemarks.Text = Remarks;
                else lblRemarks.Text = "";

            }
        }
        protected void gvDamReadings_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDamReadings.EditIndex = -1;
                BindViewDamChannelReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvDamReadings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDamReadings.PageIndex = e.NewPageIndex;
                gvDamReadings.EditIndex = -1;
                BindViewDamChannelReadingGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region DropDown
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDivision.SelectedIndex == 0)
            {
                EmptyDropdown();
            }
            else
            {
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.DDLChannelsByDamID(ddlChannel, true, (int)Constants.DropDownFirstOption.Select);
            }
            btnPrint.Visible = false;
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubDivision.SelectedIndex == 0)
            {
                Dropdownlist.DDLDamName(ddlDamName, false, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLDamName(ddlDamName, false, Convert.ToInt64(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
            Dropdownlist.DDLChannelsByDamID(ddlChannel, true, (int)Constants.DropDownFirstOption.Select);
            btnPrint.Visible = false;
        }

        protected void ddlDamName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDamName.SelectedIndex == 0)
            {
                Dropdownlist.DDLChannelsByDamID(ddlChannel, false, -1, (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                Dropdownlist.DDLChannelsByDamID(ddlChannel, false, Convert.ToInt64(ddlDamName.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
            }
            btnPrint.Visible = false;
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnPrint.Visible = false;
        }
        

        #endregion

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            btnPrint.Visible = false;
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            btnPrint.Visible = false;
        }



        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        void InitialBind()
        {
            long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
            long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
            if (boundryLvlID == null)
            {
                boundryLvlID = 0;
            }
            if (userID > 0)
            {
                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                if (ddlDivision.Items.Count == 2)
                {
                    ddlDivision.SelectedIndex = 1;
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    EmptyDropdown();
                }
            }
            else
            {
                Dropdownlist.DDLSDDivisionsByUserID(ddlDivision, userID, (long)boundryLvlID, (int)Constants.DropDownFirstOption.Select);
                if (ddlDivision.Items.Count == 2)
                {
                    ddlDivision.SelectedIndex = 1;
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    EmptyDropdown();
                }
            }

            txtFromDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
            txtToDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
            if (!string.IsNullOrEmpty(Request.QueryString["ShowHistroy"]))
            {
                HistoryValues();
            }
        }

        private void EmptyDropdown()
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLDamName(ddlDamName, true, -1, (int)Constants.DropDownFirstOption.Select);
            Dropdownlist.DDLChannelsByDamID(ddlChannel, true, (int)Constants.DropDownFirstOption.Select);
        }

        protected void SaveHistoryValues()
        {
            string DivisionID = "0";
            string SubDivisionID = "0";
            string DamNameID = "0";
            string ChannelID = "0";
            string fromDate = "0";
            string toDate = "0";

            if (ddlDivision.SelectedIndex != 0)
            {
                DivisionID = ddlDivision.SelectedIndex.ToString();
            }
            if (ddlSubDivision.SelectedIndex != 0)
            {
                SubDivisionID = ddlSubDivision.SelectedIndex.ToString();
            }
            if (ddlDamName.SelectedIndex != 0)
            {
                DamNameID = ddlDamName.SelectedIndex.ToString();
            }

            if (ddlChannel.SelectedIndex != 0)
            {
                ChannelID = ddlChannel.SelectedIndex.ToString();
            }

            fromDate = txtFromDate.Text;

            toDate = txtToDate.Text;

            Session["SearchValuesSD"] = null;
            object obj = new
            {
                DivisionID,
                SubDivisionID,
                DamNameID,
                ChannelID,
                fromDate,
                toDate

            };
            Session["SearchValuesSD"] = obj;
        }

        protected void HistoryValues()
        {
            object currentObj = Session["SearchValuesSD"] as object;
            if (currentObj != null)
            {
                ddlDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DivisionID").GetValue(currentObj));
                if (ddlDivision.SelectedIndex != 0)
                {
                    Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);
                }

                ddlSubDivision.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("SubDivisionID").GetValue(currentObj));
                if (ddlSubDivision.SelectedIndex != 0)
                {
                    Dropdownlist.DDLDamName(ddlDamName, false, Convert.ToInt64(ddlSubDivision.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);

                }

                ddlDamName.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("DamNameID").GetValue(currentObj));
                if (ddlDamName.SelectedIndex != 0)
                {
                    Dropdownlist.DDLChannelsByDamID(ddlChannel, false, Convert.ToInt64(ddlDamName.SelectedItem.Value), (int)Constants.DropDownFirstOption.Select);

                }
                ddlChannel.SelectedIndex = Convert.ToInt32(currentObj.GetType().GetProperty("ChannelID").GetValue(currentObj));
                txtFromDate.Text = Convert.ToString(currentObj.GetType().GetProperty("fromDate").GetValue(currentObj));
                txtToDate.Text = Convert.ToString(currentObj.GetType().GetProperty("toDate").GetValue(currentObj));


                //BindSearchReadingGrid();
            }

        }

        private void BindViewDamChannelReadingGrid()
        {
            try
            {
                btnPrint.Visible = true;
                gvDamReadings.Visible = false;
                gvChannelReading.Visible = false;

                Int64? SelectedDivisionID = null;
                Int64? SelectedSubDivisionID = null;
                Int64? SelectedDamID = null;
                Int64? SelectedChannelID = null;

                DateTime FromDate = System.DateTime.Now;
                DateTime ToDate = System.DateTime.Now;

                if (ddlDivision.SelectedIndex != 0)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedDivisionID = 0;
                }

                if (ddlSubDivision.SelectedIndex != 0)
                {
                    SelectedSubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                }
                else
                {
                    SelectedSubDivisionID = 0;
                }

                if (ddlDamName.SelectedIndex != 0)
                {
                    SelectedDamID = Convert.ToInt64(ddlDamName.SelectedItem.Value);
                }
                else
                {
                    SelectedDamID = 0;
                }

                if (ddlChannel.SelectedIndex != 0)
                {
                    SelectedChannelID = Convert.ToInt64(ddlChannel.SelectedItem.Value);
                }
                else
                {
                    SelectedChannelID = 0;
                }


                FromDate = Convert.ToDateTime(txtFromDate.Text);
                ToDate = Convert.ToDateTime(txtToDate.Text);

                SaveHistoryValues();

                if (SelectedChannelID == 0)
                {
                    List<SDViewDamReading_Result> lstDamType = new SmallDamsBLL().GetDamReadingView(SelectedDamID, SelectedDivisionID, SelectedSubDivisionID, FromDate, ToDate);
                    gvDamReadings.DataSource = lstDamType;
                    gvDamReadings.DataBind();
                    gvDamReadings.Visible = true;

                }
                else
                {
                    List<SDViewChannelReading_Result> lstChannelType = new SmallDamsBLL().GetChannelReadingView(SelectedDamID, SelectedDivisionID, SelectedSubDivisionID, SelectedChannelID, FromDate, ToDate);
                    gvChannelReading.DataSource = lstChannelType;
                    gvChannelReading.DataBind();
                    gvChannelReading.Visible = true;
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