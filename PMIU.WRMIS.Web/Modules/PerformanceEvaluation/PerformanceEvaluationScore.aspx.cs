using PMIU.WRMIS.BLL.PerformanceEvaluation;
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
using System.Transactions;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class PerformanceEvaluationScore : BasePage
    {
        #region Page Constants and Variables

        public string GridDisplay = "block";

        #region View State Keys

        public const string ReportIDString = "ReportID";

        #endregion

        #region GridIndex

        public const int EvalIDIndex = 0;
        public const int BoundryIDIndex = 1;

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindLevelDropdown();
                    BindReportSpans();
                    BindZoneDropdown();
                    BindYears();
                    BindMonths();
                    BindSeasons();

                    #region Load Data

                    PerformanceEvaluationBLL bllPerformanceEvaluation = new PerformanceEvaluationBLL();

                    if (!string.IsNullOrEmpty(Request.QueryString["ReportID"]))
                    {
                        long ReportID = Convert.ToInt64(Request.QueryString["ReportID"]);

                        ViewState[ReportIDString] = ReportID;

                        PE_EvaluationReports mdlEvaluationReports = bllPerformanceEvaluation.GetReportByID(ReportID);

                        if (mdlEvaluationReports.EvaluationType == "G")
                        {
                            rbSpecific.Checked = false;
                            rbGeneral.Checked = true;

                            ddlLevel.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlLevel, mdlEvaluationReports.IrrigationLevelID.ToString());

                            if (mdlEvaluationReports.Session.ToUpper().Trim() == "A")
                            {
                                rbAverageG.Checked = true;
                            }
                            else if (mdlEvaluationReports.Session.ToUpper().Trim() == "E")
                            {
                                rbEveningG.Checked = true;
                            }
                            else
                            {
                                rbMorningG.Checked = true;
                            }

                            ddlReportSpanG.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlReportSpanG, mdlEvaluationReports.ReportSpan);

                            ddlReportSpanG_SelectedIndexChanged(null, null);

                            int Year = mdlEvaluationReports.FromDate.Value.Year;
                            ddlYearG.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlYearG, Year.ToString());

                            if (mdlEvaluationReports.ReportSpan != Constants.ReportSpan.S.ToString())
                            {
                                int Month = mdlEvaluationReports.FromDate.Value.Month;
                                ddlMonthG.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlMonthG, Month.ToString());

                                if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.F.ToString())
                                {
                                    ddlMonthG_SelectedIndexChanged(null, null);

                                    string Fortnightly = string.Format("{0} -> {1}", Utility.GetFormattedDate(mdlEvaluationReports.FromDate), Utility.GetFormattedDate(mdlEvaluationReports.ToDate));
                                    ddlFortnightlyG.ClearSelection();
                                    Dropdownlist.SetSelectedValue(ddlFortnightlyG, Fortnightly);
                                }
                            }
                            else
                            {
                                string Season = string.Format("{0} -> {1}", mdlEvaluationReports.FromDate.Value.Month, mdlEvaluationReports.ToDate.Value.Month);
                                ddlSeasonG.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlSeasonG, Season);
                            }

                            GridDisplay = "block";
                        }
                        else
                        {
                            rbGeneral.Checked = false;
                            rbSpecific.Checked = true;

                            rbSpecific_CheckedChanged(null, null);

                            if (mdlEvaluationReports.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                            {
                                ddlZone.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlZone, mdlEvaluationReports.IrrigationBoundaryID.ToString());
                            }
                            else if (mdlEvaluationReports.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                            {
                                CO_Circle mdlCircle = new CircleBLL().GetByID(mdlEvaluationReports.IrrigationBoundaryID.Value);

                                ddlZone.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlZone, mdlCircle.ZoneID.ToString());
                                ddlZone_SelectedIndexChanged(null, null);

                                ddlCircle.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlCircle, mdlCircle.ID.ToString());
                            }
                            else if (mdlEvaluationReports.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                            {
                                CO_Division mdlDivision = new DivisionBLL().GetByID(mdlEvaluationReports.IrrigationBoundaryID.Value);

                                ddlZone.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlZone, mdlDivision.CO_Circle.ZoneID.ToString());
                                ddlZone_SelectedIndexChanged(null, null);

                                ddlCircle.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlCircle, mdlDivision.CircleID.ToString());
                                ddlCircle_SelectedIndexChanged(null, null);

                                ddlDivision.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlDivision, mdlDivision.ID.ToString());
                            }
                            else if (mdlEvaluationReports.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                            {
                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID(mdlEvaluationReports.IrrigationBoundaryID.Value);

                                ddlZone.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlZone, mdlSubDivision.CO_Division.CO_Circle.ZoneID.ToString());
                                ddlZone_SelectedIndexChanged(null, null);

                                ddlCircle.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlCircle, mdlSubDivision.CO_Division.CircleID.ToString());
                                ddlCircle_SelectedIndexChanged(null, null);

                                ddlDivision.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlDivision, mdlSubDivision.DivisionID.ToString());
                                ddlDivision_SelectedIndexChanged(null, null);

                                ddlSubDivision.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlSubDivision, mdlSubDivision.ID.ToString());
                            }

                            ddlReportSpanS.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlReportSpanS, mdlEvaluationReports.ReportSpan);

                            ddlReportSpanS_SelectedIndexChanged(null, null);

                            int Year = mdlEvaluationReports.FromDate.Value.Year;
                            ddlYearS.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlYearS, Year.ToString());

                            if (mdlEvaluationReports.ReportSpan != Constants.ReportSpan.S.ToString())
                            {
                                int Month = mdlEvaluationReports.FromDate.Value.Month;
                                ddlMonthS.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlMonthS, Month.ToString());

                                if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.F.ToString())
                                {
                                    ddlMonthS_SelectedIndexChanged(null, null);

                                    string Fortnightly = string.Format("{0} -> {1}", Utility.GetFormattedDate(mdlEvaluationReports.FromDate), Utility.GetFormattedDate(mdlEvaluationReports.ToDate));
                                    ddlFortnightlyS.ClearSelection();
                                    Dropdownlist.SetSelectedValue(ddlFortnightlyS, Fortnightly);
                                }
                            }
                            else
                            {
                                string Season = string.Format("{0} -> {1}", mdlEvaluationReports.FromDate.Value.Month, mdlEvaluationReports.ToDate.Value.Month);
                                ddlSeasonS.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlSeasonS, Season);
                            }

                            GridDisplay = "block";
                        }

                        hlSave.Visible = false;

                        List<dynamic> lstBoundryData = GetGridData(ReportID);

                        BindGrid(lstBoundryData);

                        #region In Report Mode

                        if (!string.IsNullOrEmpty(Request.QueryString["ReportMode"]))
                        {
                            bool ReportMode = Convert.ToBoolean(Request.QueryString["ReportMode"]);

                            if (ReportMode)
                            {
                                rbGeneral.Enabled = false;
                                rbSpecific.Enabled = false;

                                if (mdlEvaluationReports.EvaluationType == "G")
                                {
                                    ddlLevel.Enabled = false;
                                    ddlLevel.Attributes.Remove("required");
                                    ddlLevel.CssClass = "form-control";

                                    rbMorningG.Attributes.Add("disabled", "disabled");
                                    rbEveningG.Attributes.Add("disabled", "disabled");
                                    rbAverageG.Attributes.Add("disabled", "disabled");

                                    ddlReportSpanG.Enabled = false;
                                    ddlReportSpanG.Attributes.Remove("required");
                                    ddlReportSpanG.CssClass = "form-control";

                                    ddlYearG.Enabled = false;
                                    ddlYearG.Attributes.Remove("required");
                                    ddlYearG.CssClass = "form-control";

                                    if (mdlEvaluationReports.ReportSpan != Constants.ReportSpan.S.ToString())
                                    {
                                        ddlMonthG.Enabled = false;
                                        ddlMonthG.Attributes.Remove("required");
                                        ddlMonthG.CssClass = "form-control";

                                        if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.F.ToString())
                                        {
                                            ddlFortnightlyG.Enabled = false;
                                            ddlFortnightlyG.Attributes.Remove("required");
                                            ddlFortnightlyG.CssClass = "form-control";
                                        }
                                    }
                                    else
                                    {
                                        ddlSeasonG.Enabled = false;
                                        ddlSeasonG.Attributes.Remove("required");
                                        ddlSeasonG.CssClass = "form-control";
                                    }

                                }
                                else
                                {
                                    ddlZone.Enabled = false;
                                    ddlZone.Attributes.Remove("required");
                                    ddlZone.CssClass = "form-control";

                                    ddlCircle.Enabled = false;
                                    ddlDivision.Enabled = false;
                                    ddlSubDivision.Enabled = false;

                                    rbMorningS.Attributes.Add("disabled", "disabled");
                                    rbEveningS.Attributes.Add("disabled", "disabled");
                                    rbAverageS.Attributes.Add("disabled", "disabled");

                                    ddlReportSpanS.Enabled = false;
                                    ddlReportSpanS.Attributes.Remove("required");
                                    ddlReportSpanS.CssClass = "form-control";

                                    ddlYearS.Enabled = false;
                                    ddlYearS.Attributes.Remove("required");
                                    ddlYearS.CssClass = "form-control";

                                    if (mdlEvaluationReports.ReportSpan != Constants.ReportSpan.S.ToString())
                                    {
                                        ddlMonthS.Enabled = false;
                                        ddlMonthS.Attributes.Remove("required");
                                        ddlMonthS.CssClass = "form-control";

                                        if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.F.ToString())
                                        {
                                            ddlFortnightlyS.Enabled = false;
                                            ddlFortnightlyS.Attributes.Remove("required");
                                            ddlFortnightlyS.CssClass = "form-control";
                                        }
                                    }
                                    else
                                    {
                                        ddlSeasonS.Enabled = false;
                                        ddlSeasonS.Attributes.Remove("required");
                                        ddlSeasonS.CssClass = "form-control";
                                    }
                                }

                                btnShow.Visible = false;
                                hlBack.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/PerformanceEvaluationReports.aspx?ShowHistory={0}", true);
                                hlBack.Visible = true;
                            }
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 28-10-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the Level dropdown.
        /// Created on 28-10-2016
        /// </summary>
        private void BindLevelDropdown()
        {
            Dropdownlist.DDLIrrigationLevel(ddlLevel);
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 23-11-2016
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 23-11-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 23-11-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID);
        }

        /// <summary>
        /// This function binds sub divisions to the sub division dropdown
        /// Created on 23-11-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        private void BindSubDivisionDropdown(long _DivisionID)
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, _DivisionID);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ResetGrids();

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;
                long ReportID = 0;
                List<dynamic> lstScreenData = null;

                if (rbGeneral.Checked)
                {
                    long IrrigationLevelID = Convert.ToInt64(ddlLevel.SelectedItem.Value);
                    string Session = (rbEveningG.Checked ? rbEveningG.Value : (rbAverageG.Checked ? rbAverageG.Value : rbMorningG.Value));
                    string ReportSpan = ddlReportSpanG.SelectedItem.Value;

                    DateTime? FromDate = null;
                    DateTime? ToDate = null;

                    if (ReportSpan == Constants.ReportSpan.F.ToString())
                    {
                        string AnalysisPeriod = ddlFortnightlyG.SelectedItem.Value;

                        string[] Dates = AnalysisPeriod.Split(new string[] { " -> " }, StringSplitOptions.None);

                        FromDate = Utility.GetParsedDate(Dates[0]);
                        ToDate = Utility.GetParsedDate(Dates[1]);
                    }
                    else if (ReportSpan == Constants.ReportSpan.M.ToString())
                    {
                        int Year = Convert.ToInt32(ddlYearG.SelectedItem.Value);
                        int Month = Convert.ToInt32(ddlMonthG.SelectedItem.Value);

                        FromDate = new DateTime(Year, Month, 1);
                        ToDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
                    }
                    else if (ReportSpan == Constants.ReportSpan.S.ToString())
                    {
                        int Year = Convert.ToInt32(ddlYearG.SelectedItem.Value);
                        string Season = ddlSeasonG.SelectedItem.Value;

                        string[] Months = Season.Split(new string[] { " -> " }, StringSplitOptions.None);
                        int StartMonth = Convert.ToInt32(Months[0]);
                        int EndMonth = Convert.ToInt32(Months[1]);

                        if (StartMonth < EndMonth)
                        {
                            FromDate = new DateTime(Year, StartMonth, 1);
                            ToDate = new DateTime(Year, EndMonth, DateTime.DaysInMonth(Year, EndMonth));
                        }
                        else
                        {
                            FromDate = new DateTime(Year, StartMonth, 1);
                            Year++;
                            ToDate = new DateTime(Year, EndMonth, DateTime.DaysInMonth(Year, EndMonth));
                        }
                    }

                    lstScreenData = new PerformanceEvaluationBLL().GetGeneralEvaluationScore(IrrigationLevelID, Session, FromDate, ToDate, mdlUsers.ID, ReportSpan, out ReportID);
                }
                else
                {
                    long IrrigationLevelID = 0;
                    long IrrigationBoundaryID = 0;

                    if (ddlSubDivision.SelectedItem.Value != string.Empty)
                    {
                        IrrigationLevelID = (long)Constants.IrrigationLevelID.SubDivision;
                        IrrigationBoundaryID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                    }
                    else if (ddlDivision.SelectedItem.Value != string.Empty)
                    {
                        IrrigationLevelID = (long)Constants.IrrigationLevelID.Division;
                        IrrigationBoundaryID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    }
                    else if (ddlCircle.SelectedItem.Value != string.Empty)
                    {
                        IrrigationLevelID = (long)Constants.IrrigationLevelID.Circle;
                        IrrigationBoundaryID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    }
                    else
                    {
                        IrrigationLevelID = (long)Constants.IrrigationLevelID.Zone;
                        IrrigationBoundaryID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    }

                    string Session = (rbEveningS.Checked ? rbEveningS.Value : (rbAverageS.Checked ? rbAverageS.Value : rbMorningS.Value));
                    string ReportSpan = ddlReportSpanS.SelectedItem.Value;

                    DateTime? FromDate = null;
                    DateTime? ToDate = null;

                    if (ReportSpan == Constants.ReportSpan.F.ToString())
                    {
                        string AnalysisPeriod = ddlFortnightlyS.SelectedItem.Value;

                        string[] Dates = AnalysisPeriod.Split(new string[] { " -> " }, StringSplitOptions.None);

                        FromDate = Utility.GetParsedDate(Dates[0]);
                        ToDate = Utility.GetParsedDate(Dates[1]);
                    }
                    else if (ReportSpan == Constants.ReportSpan.M.ToString())
                    {
                        int Year = Convert.ToInt32(ddlYearS.SelectedItem.Value);
                        int Month = Convert.ToInt32(ddlMonthS.SelectedItem.Value);

                        FromDate = new DateTime(Year, Month, 1);
                        ToDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
                    }
                    else if (ReportSpan == Constants.ReportSpan.S.ToString())
                    {
                        int Year = Convert.ToInt32(ddlYearS.SelectedItem.Value);
                        string Season = ddlSeasonS.SelectedItem.Value;

                        string[] Months = Season.Split(new string[] { " -> " }, StringSplitOptions.None);
                        int StartMonth = Convert.ToInt32(Months[0]);
                        int EndMonth = Convert.ToInt32(Months[1]);

                        if (StartMonth < EndMonth)
                        {
                            FromDate = new DateTime(Year, StartMonth, 1);
                            ToDate = new DateTime(Year, EndMonth, DateTime.DaysInMonth(Year, EndMonth));
                        }
                        else
                        {
                            FromDate = new DateTime(Year, StartMonth, 1);
                            Year++;
                            ToDate = new DateTime(Year, EndMonth, DateTime.DaysInMonth(Year, EndMonth));
                        }
                    }

                    lstScreenData = new PerformanceEvaluationBLL().GetSpecificEvaluationScore(IrrigationLevelID, IrrigationBoundaryID, Session, FromDate, ToDate, mdlUsers.ID, ReportSpan, out ReportID);
                }

                if (lstScreenData != null && lstScreenData.Count() > 0)
                {
                    ViewState[ReportIDString] = ReportID;
                    hlSave.Visible = true;
                }
                else
                {
                    ViewState[ReportIDString] = null;
                    hlSave.Visible = false;
                }

                BindGrid(lstScreenData);

                txtReportName.Text = string.Empty;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the grids according to the Irrigation Level
        /// Created On 01-11-2016
        /// </summary>
        /// <param name="_LstBoundryData"></param>
        private void BindGrid(List<dynamic> _LstBoundryData)
        {
            long IrrigationLevelID = 0;

            if (rbGeneral.Checked)
            {
                IrrigationLevelID = Convert.ToInt64(ddlLevel.SelectedItem.Value);
            }
            else
            {
                if (ddlSubDivision.SelectedItem.Value != string.Empty)
                {
                    IrrigationLevelID = (long)Constants.IrrigationLevelID.Section;
                }
                else if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    IrrigationLevelID = (long)Constants.IrrigationLevelID.SubDivision;
                }
                else if (ddlCircle.SelectedItem.Value != string.Empty)
                {
                    IrrigationLevelID = (long)Constants.IrrigationLevelID.Division;
                }
                else if (ddlZone.SelectedItem.Value != string.Empty)
                {
                    IrrigationLevelID = (long)Constants.IrrigationLevelID.Circle;
                }
            }

            if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                dvZone.Visible = true;
                gvZone.DataSource = _LstBoundryData;
                gvZone.DataBind();

                gvZone.PageIndex = 0;
            }
            else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                dvCircle.Visible = true;
                gvCircle.DataSource = _LstBoundryData;
                gvCircle.DataBind();

                gvCircle.PageIndex = 0;
            }
            else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                dvDivision.Visible = true;
                gvDivision.DataSource = _LstBoundryData;
                gvDivision.DataBind();

                gvDivision.PageIndex = 0;
            }
            else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                dvSubDivision.Visible = true;
                gvSubDivision.DataSource = _LstBoundryData;
                gvSubDivision.DataBind();

                gvSubDivision.PageIndex = 0;
            }
            else if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                dvSection.Visible = true;
                gvSection.DataSource = _LstBoundryData;
                gvSection.DataBind();

                gvSection.PageIndex = 0;
            }

            dvbutton.Visible = true;
            btnSave.Enabled = true;
        }

        /// <summary>
        /// This function gets data for the scores grid
        /// Created On 25-11-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        private List<dynamic> GetGridData(long _ReportID)
        {
            return new PerformanceEvaluationBLL().GetPEScoreDetailByReportID(_ReportID);
        }

        protected void gvZone_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvZone.PageIndex = e.NewPageIndex;

                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                List<dynamic> lstBoundryData = GetGridData(ReportID);

                BindGrid(lstBoundryData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvZone_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblObtainedPoints = (Label)e.Row.FindControl("lblObtainedPoints");
                    double ObtainedPoints = Convert.ToDouble(lblObtainedPoints.Text);
                    lblObtainedPoints.Text = String.Format("{0:0.00}", ObtainedPoints);

                    string ReportID = ViewState[ReportIDString].ToString();
                    string BoundryID = gvZone.DataKeys[e.Row.RowIndex].Values[BoundryIDIndex].ToString();

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPICategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}", ReportID, BoundryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCircle.PageIndex = e.NewPageIndex;

                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                List<dynamic> lstBoundryData = GetGridData(ReportID);

                BindGrid(lstBoundryData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblObtainedPoints = (Label)e.Row.FindControl("lblObtainedPoints");
                    double ObtainedPoints = Convert.ToDouble(lblObtainedPoints.Text);
                    lblObtainedPoints.Text = String.Format("{0:0.00}", ObtainedPoints);

                    string ReportID = ViewState[ReportIDString].ToString();
                    string BoundryID = gvCircle.DataKeys[e.Row.RowIndex].Values[BoundryIDIndex].ToString();

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPICategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}", ReportID, BoundryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDivision.PageIndex = e.NewPageIndex;

                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                List<dynamic> lstBoundryData = GetGridData(ReportID);

                BindGrid(lstBoundryData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblObtainedPoints = (Label)e.Row.FindControl("lblObtainedPoints");
                    double ObtainedPoints = Convert.ToDouble(lblObtainedPoints.Text);
                    lblObtainedPoints.Text = String.Format("{0:0.00}", ObtainedPoints);

                    Label lblComplexityScore = (Label)e.Row.FindControl("lblComplexityScore");
                    double ComplexityScore = Convert.ToDouble(lblComplexityScore.Text);
                    lblComplexityScore.Text = String.Format("{0:0.00}", ComplexityScore);

                    string ReportID = ViewState[ReportIDString].ToString();
                    string BoundryID = gvDivision.DataKeys[e.Row.RowIndex].Values[BoundryIDIndex].ToString();

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPICategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}", ReportID, BoundryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSubDivision.PageIndex = e.NewPageIndex;

                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                List<dynamic> lstBoundryData = GetGridData(ReportID);

                BindGrid(lstBoundryData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblObtainedPoints = (Label)e.Row.FindControl("lblObtainedPoints");
                    double ObtainedPoints = Convert.ToDouble(lblObtainedPoints.Text);
                    lblObtainedPoints.Text = String.Format("{0:0.00}", ObtainedPoints);

                    Label lblComplexityScore = (Label)e.Row.FindControl("lblComplexityScore");
                    double ComplexityScore = Convert.ToDouble(lblComplexityScore.Text);
                    lblComplexityScore.Text = String.Format("{0:0.00}", ComplexityScore);

                    string ReportID = ViewState[ReportIDString].ToString();
                    string BoundryID = gvSubDivision.DataKeys[e.Row.RowIndex].Values[BoundryIDIndex].ToString();

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPICategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}", ReportID, BoundryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSection.PageIndex = e.NewPageIndex;

                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                List<dynamic> lstBoundryData = GetGridData(ReportID);

                BindGrid(lstBoundryData);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblObtainedPoints = (Label)e.Row.FindControl("lblObtainedPoints");
                    double ObtainedPoints = Convert.ToDouble(lblObtainedPoints.Text);
                    lblObtainedPoints.Text = String.Format("{0:0.00}", ObtainedPoints);

                    Label lblComplexityScore = (Label)e.Row.FindControl("lblComplexityScore");
                    double ComplexityScore = Convert.ToDouble(lblComplexityScore.Text);
                    lblComplexityScore.Text = String.Format("{0:0.00}", ComplexityScore);

                    string ReportID = ViewState[ReportIDString].ToString();
                    string BoundryID = gvSection.DataKeys[e.Row.RowIndex].Values[BoundryIDIndex].ToString();

                    HyperLink hlDetail = (HyperLink)e.Row.FindControl("hlDetail");

                    if (hlDetail != null)
                    {
                        hlDetail.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/KPICategoryWiseWeightage.aspx?ReportID={0}&IrrigationBoundaryID={1}", ReportID, BoundryID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function hides the evaluation results grid and save button
        /// Created On 01-11-2016
        /// </summary>
        private void ResetGrids()
        {
            dvZone.Visible = false;
            dvCircle.Visible = false;
            dvDivision.Visible = false;
            dvSubDivision.Visible = false;
            dvSection.Visible = false;
            dvbutton.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long ReportID = Convert.ToInt64(ViewState[ReportIDString]);

                string ReportName = txtReportName.Text.Trim();

                PerformanceEvaluationBLL bllPerformanceEvaluation = new PerformanceEvaluationBLL();

                if (ReportName == string.Empty)
                {
                    txtReportName.Text = string.Empty;
                    txtShowPopup.Text = "true";
                    return;
                }

                if (bllPerformanceEvaluation.GetEvaluationByName(ReportName))
                {
                    Master.ShowMessage(Message.ReportNameExists.Description, SiteMaster.MessageType.Error);
                    txtShowPopup.Text = "true";
                    return;
                }

                PE_EvaluationReports mdlEvaluationReports = bllPerformanceEvaluation.GetReportByID(ReportID);

                mdlEvaluationReports.ReportName = ReportName;
                mdlEvaluationReports.ModifiedDate = DateTime.Now;

                bllPerformanceEvaluation.UpdateReport(mdlEvaluationReports);

                txtReportName.Text = string.Empty;

                hlSave.Visible = false;

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbGeneral_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlGeneral.Visible = true;
                pnlSpecific.Visible = false;

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlGeneral.Visible = false;
                pnlSpecific.Visible = true;

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
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

                GridDisplay = "none";
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

                GridDisplay = "none";
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

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds values to the Report Span dropdowns
        /// Created on 28-11-2016
        /// </summary>
        private void BindReportSpans()
        {
            ddlReportSpanG.Items.Add(new ListItem { Text = "Select", Value = "" });
            ddlReportSpanG.Items.Add(new ListItem { Text = "Weekly", Value = Constants.ReportSpan.W.ToString(), Enabled = false });
            ddlReportSpanG.Items.Add(new ListItem { Text = "Fortnightly", Value = Constants.ReportSpan.F.ToString() });
            ddlReportSpanG.Items.Add(new ListItem { Text = "Monthly", Value = Constants.ReportSpan.M.ToString() });
            ddlReportSpanG.Items.Add(new ListItem { Text = "Seasonal", Value = Constants.ReportSpan.S.ToString() });

            ddlReportSpanS.Items.Add(new ListItem { Text = "Select", Value = "" });
            ddlReportSpanS.Items.Add(new ListItem { Text = "Weekly", Value = Constants.ReportSpan.W.ToString(), Enabled = false });
            ddlReportSpanS.Items.Add(new ListItem { Text = "Fortnightly", Value = Constants.ReportSpan.F.ToString() });
            ddlReportSpanS.Items.Add(new ListItem { Text = "Monthly", Value = Constants.ReportSpan.M.ToString() });
            ddlReportSpanS.Items.Add(new ListItem { Text = "Seasonal", Value = Constants.ReportSpan.S.ToString() });
        }

        protected void ddlReportSpanG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindYears('G');
                ddlYearG.SelectedIndex = 0;
                ddlYearG.Enabled = true;

                if (ddlReportSpanG.SelectedItem.Value == string.Empty)
                {
                    pnlMonthG.Visible = false;
                    pnlFortnightlyG.Visible = false;
                    pnlSeasonG.Visible = false;
                }
                else if (ddlReportSpanG.SelectedItem.Value == Constants.ReportSpan.F.ToString())
                {
                    ddlMonthG.SelectedIndex = 0;
                    ddlFortnightlyG.SelectedIndex = 0;

                    pnlMonthG.Visible = true;
                    pnlFortnightlyG.Visible = true;
                    pnlSeasonG.Visible = false;
                }
                else if (ddlReportSpanG.SelectedItem.Value == Constants.ReportSpan.M.ToString())
                {
                    ddlMonthG.SelectedIndex = 0;

                    pnlMonthG.Visible = true;
                    pnlFortnightlyG.Visible = false;
                    pnlSeasonG.Visible = false;
                }
                else if (ddlReportSpanG.SelectedItem.Value == Constants.ReportSpan.S.ToString())
                {
                    ddlSeasonG.SelectedIndex = 0;

                    pnlMonthG.Visible = false;
                    pnlFortnightlyG.Visible = false;
                    pnlSeasonG.Visible = true;

                    ddlYearG.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlReportSpanS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindYears('S');
                ddlYearS.SelectedIndex = 0;
                ddlYearS.Enabled = true;

                if (ddlReportSpanS.SelectedItem.Value == string.Empty)
                {
                    pnlMonthS.Visible = false;
                    pnlFortnightlyS.Visible = false;
                    pnlSeasonS.Visible = false;
                }
                else if (ddlReportSpanS.SelectedItem.Value == Constants.ReportSpan.F.ToString())
                {
                    ddlMonthS.SelectedIndex = 0;
                    ddlFortnightlyS.SelectedIndex = 0;

                    pnlMonthS.Visible = true;
                    pnlFortnightlyS.Visible = true;
                    pnlSeasonS.Visible = false;
                }
                else if (ddlReportSpanS.SelectedItem.Value == Constants.ReportSpan.M.ToString())
                {
                    ddlMonthS.SelectedIndex = 0;

                    pnlMonthS.Visible = true;
                    pnlFortnightlyS.Visible = false;
                    pnlSeasonS.Visible = false;
                }
                else if (ddlReportSpanS.SelectedItem.Value == Constants.ReportSpan.S.ToString())
                {
                    ddlSeasonS.SelectedIndex = 0;

                    pnlMonthS.Visible = false;
                    pnlFortnightlyS.Visible = false;
                    pnlSeasonS.Visible = true;

                    ddlYearS.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the last ten years to year dropdown
        /// Created On 21-12-2016
        /// </summary>
        private void BindYears(char _EvaluationType = 'B')
        {
            int CurrentYear = DateTime.Now.Year;

            List<ListItem> Years = new List<ListItem>();

            for (int Year = CurrentYear; Year > CurrentYear - 15; Year--)
            {
                Years.Add(new ListItem { Text = Year.ToString(), Value = Year.ToString() });
            }

            if (_EvaluationType == 'B' || _EvaluationType == 'G')
            {
                ddlYearG.DataSource = Years;
                ddlYearG.DataValueField = "Value";
                ddlYearG.DataTextField = "Text";
                ddlYearG.DataBind();

                ddlYearG.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }

            if (_EvaluationType == 'B' || _EvaluationType == 'S')
            {
                ddlYearS.DataSource = Years;
                ddlYearS.DataValueField = "Value";
                ddlYearS.DataTextField = "Text";
                ddlYearS.DataBind();

                ddlYearS.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }
        }

        /// <summary>
        /// This function binds the twelve months of year to months dropdown
        /// Created On 21-12-2016
        /// </summary>
        private void BindMonths()
        {
            List<ListItem> Months = new List<ListItem>();

            for (int Month = 1; Month <= 12; Month++)
            {
                DateTime FirstDay = Convert.ToDateTime(string.Format("{0}-{1}-{2}", Month, 1, DateTime.Now.Year));
                Months.Add(new ListItem { Text = FirstDay.ToString("MMMM"), Value = Month.ToString() });
            }

            ddlMonthG.DataSource = Months;
            ddlMonthG.DataTextField = "Text";
            ddlMonthG.DataValueField = "Value";
            ddlMonthG.DataBind();

            ddlMonthG.Items.Insert(0, new ListItem { Text = "Select", Value = "" });

            ddlMonthS.DataSource = Months;
            ddlMonthS.DataTextField = "Text";
            ddlMonthS.DataValueField = "Value";
            ddlMonthS.DataBind();

            ddlMonthS.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
        }

        protected void ddlYearG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlYearG.SelectedItem.Value != string.Empty && ddlMonthG.SelectedItem.Value != string.Empty)
                {
                    ddlFortnightlyG.Enabled = true;

                    int Year = Int32.Parse(ddlYearG.SelectedItem.Value);
                    int Month = Int32.Parse(ddlMonthG.SelectedItem.Value);

                    BindFortnightlyDateDropdown("G", Year, Month);
                }
                else
                {
                    ddlFortnightlyG.SelectedIndex = 0;
                    ddlFortnightlyG.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlMonthG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlYearG.SelectedItem.Value != string.Empty && ddlMonthG.SelectedItem.Value != string.Empty)
                {
                    ddlFortnightlyG.Enabled = true;

                    int Year = Int32.Parse(ddlYearG.SelectedItem.Value);
                    int Month = Int32.Parse(ddlMonthG.SelectedItem.Value);

                    BindFortnightlyDateDropdown("G", Year, Month);
                }
                else
                {
                    ddlFortnightlyG.SelectedIndex = 0;
                    ddlFortnightlyG.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds fortnightly values to dropdown based on year and month
        /// Created On 22-12-2016
        /// </summary>
        /// <param name="_EvaluationType"></param>
        /// <param name="_Year"></param>
        /// <param name="_Month"></param>
        private void BindFortnightlyDateDropdown(string _EvaluationType, int _Year, int _Month)
        {
            List<ListItem> lstDates = new List<ListItem>();

            lstDates.Add(new ListItem { Text = "1st Fortnightly", Value = string.Format("{0} -> {1}", Utility.GetFormattedDate(new DateTime(_Year, _Month, 1)), Utility.GetFormattedDate(new DateTime(_Year, _Month, 15))) });
            lstDates.Add(new ListItem { Text = "2nd Fortnightly", Value = string.Format("{0} -> {1}", Utility.GetFormattedDate(new DateTime(_Year, _Month, 16)), Utility.GetFormattedDate(new DateTime(_Year, _Month, DateTime.DaysInMonth(_Year, _Month)))) });

            if (_EvaluationType == "G")
            {
                ddlFortnightlyG.DataSource = lstDates;
                ddlFortnightlyG.DataTextField = "Text";
                ddlFortnightlyG.DataValueField = "Value";
                ddlFortnightlyG.DataBind();

                ddlFortnightlyG.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }
            else
            {
                ddlFortnightlyS.DataSource = lstDates;
                ddlFortnightlyS.DataTextField = "Text";
                ddlFortnightlyS.DataValueField = "Value";
                ddlFortnightlyS.DataBind();

                ddlFortnightlyS.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }
        }

        protected void ddlYearS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlYearS.SelectedItem.Value != string.Empty && ddlMonthS.SelectedItem.Value != string.Empty)
                {
                    ddlFortnightlyS.Enabled = true;

                    int Year = Int32.Parse(ddlYearS.SelectedItem.Value);
                    int Month = Int32.Parse(ddlMonthS.SelectedItem.Value);

                    BindFortnightlyDateDropdown("S", Year, Month);
                }
                else
                {
                    ddlFortnightlyS.SelectedIndex = 0;
                    ddlFortnightlyS.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMonthS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlYearS.SelectedItem.Value != string.Empty && ddlMonthS.SelectedItem.Value != string.Empty)
                {
                    ddlFortnightlyS.Enabled = true;

                    int Year = Int32.Parse(ddlYearS.SelectedItem.Value);
                    int Month = Int32.Parse(ddlMonthS.SelectedItem.Value);

                    BindFortnightlyDateDropdown("S", Year, Month);
                }
                else
                {
                    ddlFortnightlyS.SelectedIndex = 0;
                    ddlFortnightlyS.Enabled = false;
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the seasons to dropdown
        /// Created On 23-12-2016
        /// </summary>
        private void BindSeasons()
        {
            List<ListItem> lstDates = new List<ListItem>();

            lstDates.Add(new ListItem { Text = "Kharif", Value = string.Format("{0} -> {1}", 4, 9) });
            lstDates.Add(new ListItem { Text = "Rabi", Value = string.Format("{0} -> {1}", 10, 3) });

            ddlSeasonG.DataSource = lstDates;
            ddlSeasonG.DataTextField = "Text";
            ddlSeasonG.DataValueField = "Value";
            ddlSeasonG.DataBind();

            ddlSeasonG.Items.Insert(0, new ListItem { Text = "Select", Value = "" });

            ddlSeasonS.DataSource = lstDates;
            ddlSeasonS.DataTextField = "Text";
            ddlSeasonS.DataValueField = "Value";
            ddlSeasonS.DataBind();

            ddlSeasonS.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
        }

        protected void ddlSeasonS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeasonS.SelectedValue == string.Empty)
                {
                    ddlYearS.SelectedIndex = 0;
                    ddlYearS.Enabled = false;
                }
                else
                {
                    if (ddlSeasonS.SelectedItem.Text == "Rabi")
                    {
                        BindRabiYears('S');
                        ddlYearS.SelectedIndex = 0;
                        ddlYearS.Enabled = true;
                    }
                    else
                    {
                        BindYears('S');
                        ddlYearS.SelectedIndex = 0;
                        ddlYearS.Enabled = true;
                    }
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the last ten Rabi years to year dropdown
        /// Created On 21-02-2017
        /// </summary>
        private void BindRabiYears(char _EvaluationType)
        {
            int CurrentYear = DateTime.Now.Year;

            List<ListItem> Years = new List<ListItem>();

            for (int Year = CurrentYear; Year > CurrentYear - 15; Year--)
            {
                string YearString = string.Format("{0} - {1}", Year, (Year + 1));
                Years.Add(new ListItem { Text = YearString, Value = Year.ToString() });
            }

            if (_EvaluationType == 'G')
            {
                ddlYearG.DataSource = Years;
                ddlYearG.DataValueField = "Value";
                ddlYearG.DataTextField = "Text";
                ddlYearG.DataBind();

                ddlYearG.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }

            if (_EvaluationType == 'S')
            {
                ddlYearS.DataSource = Years;
                ddlYearS.DataValueField = "Value";
                ddlYearS.DataTextField = "Text";
                ddlYearS.DataBind();

                ddlYearS.Items.Insert(0, new ListItem { Text = "Select", Value = "" });
            }
        }

        protected void ddlSeasonG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSeasonG.SelectedValue == string.Empty)
                {
                    ddlYearG.SelectedIndex = 0;
                    ddlYearG.Enabled = false;
                }
                else
                {
                    if (ddlSeasonG.SelectedItem.Text == "Rabi")
                    {
                        BindRabiYears('G');
                        ddlYearG.SelectedIndex = 0;
                        ddlYearG.Enabled = true;
                    }
                    else
                    {
                        BindYears('G');
                        ddlYearG.SelectedIndex = 0;
                        ddlYearG.Enabled = true;
                    }
                }

                GridDisplay = "none";
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}