using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.WaterLosses
{
    public partial class DivisionalWaterLosses : BasePage
    {
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;
        private const int DAILY = 1, MONTHLY_KHAREEF = 2, MONTHLY_RABI = 3, YEALRY = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();

                long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                long? boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                if (userID > 0 && boundryLvlID != null) // Irrigation Staff
                {
                    LoadAllRegionDDByUser(userID, boundryLvlID);
                }
                else
                    LoadDDL(ZONE, 0);
            }
        }
        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            HideDataDivs();
            HideDivs();
            if (rdDaily.Checked)
            {
                divDaily.Visible = true;
            }
            else if (rbMnthly.Checked)
            {
                string str_Div = ddlDiv.SelectedItem.Value;
                if (string.IsNullOrEmpty(str_Div))
                {
                    Master.ShowMessage(Message.SelectASubDivision.Description, SiteMaster.MessageType.Error);
                    rbMnthly.Checked = false;
                    return;
                }

                List<object> lstYears = new WaterLossesBLL().GetDivisonalWLYearList(Convert.ToInt64(str_Div));
                if (lstYears != null && lstYears.Count > 0)
                {
                    ddlMonthlyYear.Items.Clear();
                    ddlMonthlyYear.Items.Add(new ListItem("Select", ""));
                    ddlMonthlyYear.Enabled = false;
                }
                else
                {
                    ddlMonthlyYear.Items.Clear();
                    ddlMonthlyYear.Items.Add(new ListItem("Select", ""));
                    ddlMonthlyYear.Enabled = false;
                }

                divMonthly.Visible = true;
            }
            else if (rbYearly.Checked)
            {
                string str_Div = ddlDiv.SelectedItem.Value;
                if (string.IsNullOrEmpty(str_Div))
                {
                    Master.ShowMessage(Message.SelectADivision.Description, SiteMaster.MessageType.Error);
                    rbYearly.Checked = false;
                    return;
                }

                List<object> lstYears = new WaterLossesBLL().GetDivisonalWL_RabiYears(Convert.ToInt64(str_Div));
                if (lstYears != null && lstYears.Count > 0)
                {
                    ddlFromYear.Items.Clear();
                    ddlToYear.Items.Clear();

                    Dropdownlist.DDLLoading(ddlFromYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                    ddlFromYear.Enabled = true;

                    Dropdownlist.DDLLoading(ddlToYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                    ddlToYear.Enabled = true;
                }
                else
                {
                    ddlToYear.Items.Clear();
                    ddlToYear.Items.Add(new ListItem("Select", ""));
                    ddlToYear.Enabled = false;

                    ddlFromYear.Items.Clear();
                    ddlFromYear.Items.Add(new ListItem("Select", ""));
                    ddlFromYear.Enabled = false;
                }
                divYearly.Visible = true;
            }
            HideDataDivs();
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideDivs();
            HideDataDivs();
            DropDownList sndr = (DropDownList)sender;

            string strValue = sndr.SelectedItem.Value;
            if (sndr.ID.Equals("ddlZone"))
            {
                DisableDD(ddlCircle);
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlCircle"))
            {
                DisableDD(ddlDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(DIVISION, Convert.ToInt64(strValue));
            }

            rdDaily.Checked = false;
            rbMnthly.Checked = false;
            rbYearly.Checked = false;
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            HideDataDivs();

            if (string.IsNullOrEmpty(ddlDiv.SelectedItem.Value))
            {
                Master.ShowMessage(Message.SelectADivision.Description, SiteMaster.MessageType.Error);
                return;
            }
            if (rbMnthly.Checked == false && rbYearly.Checked == false && rdDaily.Checked == false)
            {
                Master.ShowMessage("Select a radio button", SiteMaster.MessageType.Error);
                return;
            }
            long ID = Convert.ToInt64(ddlDiv.SelectedItem.Value);
            List<WaterLossesDAL.SubDiv_WL> lstData = new List<WaterLossesDAL.SubDiv_WL>();

            #region Daily Water Losses
            if (rdDaily.Checked)
            {
                DateTime fromDate = DateTime.Today, toDate = DateTime.Today;

                if (VerifyDateRange())
                {
                    fromDate = Utility.GetParsedDate(txtFromDate.Text);
                    toDate = Utility.GetParsedDate(txtToDate.Text);

                    string day = (fromDate.Day < 10 ? ("0" + fromDate.Day) : "" + fromDate.Day);
                    string month = (fromDate.Month < 10 ? ("0" + fromDate.Month) : "" + fromDate.Month);
                    string from = fromDate.Year + "-" + month + "-" + day;

                    day = (toDate.Day < 10 ? ("0" + toDate.Day) : "" + toDate.Day);
                    month = (toDate.Month < 10 ? ("0" + toDate.Month) : "" + toDate.Month);
                    string to = toDate.Year + "-" + month + "-" + day;
                    lstData = new WaterLossesBLL().GetDivisionalLosses(ID, DAILY, from, to, 0, 0, 0);

                    if (lstData.Count > 0)
                        DrawDivisionalDataTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

            #region Monthly Water Losses
            else if (rbMnthly.Checked)
            {
                int year = 0, fromMonth = 0, toMonth = 0;
                string strValue = ddlMonthlyYear.SelectedItem.Value;

                if (string.IsNullOrEmpty(strValue))
                {
                    Master.ShowMessage(Message.YearRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (VerifyMonthRange())
                {
                    if (ddlSeason.SelectedItem.Value == "1") //Rabi Season
                    {
                        year = -11;
                        fromMonth = Convert.ToInt32((strValue.Split('-'))[0]); toMonth = Convert.ToInt32((strValue.Split('-'))[1]);
                        lstData = new WaterLossesBLL().GetDivisionalLosses(ID, MONTHLY_RABI, "", "", year, fromMonth, toMonth);
                    }
                    else if (ddlSeason.SelectedItem.Value == "2")// Kharif season
                    {
                        year = Convert.ToInt32(strValue);
                        fromMonth = 4; toMonth = 9;
                        lstData = new WaterLossesBLL().GetDivisionalLosses(ID, MONTHLY_KHAREEF, "", "", year, fromMonth, toMonth);
                    }

                    if (lstData.Count > 0)
                        DrawDivisionalDataTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

            #region Yealry Water Losses
            else if (rbYearly.Checked)
            {
                int year = 0, fromYear = 0, toYear = 0;
                if (VerifyYearRange())
                {
                    string strValue = "";

                    strValue = ddlFromYear.SelectedItem.Value;
                    fromYear = Convert.ToInt32(strValue.Split('-')[0]);

                    strValue = ddlToYear.SelectedItem.Value;
                    toYear = Convert.ToInt32(strValue.Split('-')[1]);

                    lstData = new WaterLossesBLL().GetDivisionalLosses(ID, YEALRY, "", "", year, fromYear, toYear);

                    if (lstData.Count > 0)
                        DrawDivisionalDataTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

        }
        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideDataDivs();
            string str_Div = ddlDiv.SelectedItem.Value;
            if (string.IsNullOrEmpty(str_Div))
            {
                Master.ShowMessage(Message.SelectADivision.Description, SiteMaster.MessageType.Error);
                rbMnthly.Checked = false;
                return;
            }

            if (string.IsNullOrEmpty(ddlSeason.SelectedItem.Value))
                return;

            else if (ddlSeason.SelectedItem.Value == "1") // Rabi Season
            {
                ddlMonthlyYear.Items.Clear();
                List<object> lstYears = new WaterLossesBLL().GetDivisonalWL_RabiYears(Convert.ToInt64(str_Div));
                Dropdownlist.DDLLoading(ddlMonthlyYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                ddlMonthlyYear.Enabled = true;
            }
            else if (ddlSeason.SelectedItem.Value == "2") // Kharif Season
            {
                ddlMonthlyYear.Items.Clear();
                List<object> lstYears = new WaterLossesBLL().GetDivisonalWLYearList(Convert.ToInt64(str_Div));
                Dropdownlist.DDLLoading(ddlMonthlyYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                ddlMonthlyYear.Enabled = true;
            }
        }

        private void DisableDD(DropDownList DDL)
        {
            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("Select", ""));
            DDL.Enabled = false;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivisionWaterLosses);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadAllRegionDDByUser(long _UserID, long? _BoundryLevelID)
        {
            if (_BoundryLevelID == null)
                return;

            List<object> lstData = new WaterLossesBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_BoundryLevelID));

            List<object> lstChild = (List<object>)lstData.ElementAt(1);
            if (lstChild.Count > 0) // Division
            {
                Dropdownlist.DDLLoading(ddlDiv, false, (int)Constants.DropDownFirstOption.Select, lstChild);
                ddlDiv.Enabled = true;
                if (lstChild.Count == 1)
                {
                    ddlDiv.SelectedIndex = 1;
                    ddlDiv.Enabled = false;
                }
            }

            lstChild = (List<object>)lstData.ElementAt(2);
            if (lstChild.Count > 0) // Circle
            {
                Dropdownlist.DDLLoading(ddlCircle, false, (int)Constants.DropDownFirstOption.Select, lstChild);
                ddlCircle.Enabled = true;
                if (lstChild.Count == 1)
                {
                    ddlCircle.SelectedIndex = 1;
                    ddlCircle.Enabled = false;
                }
            }

            lstChild = (List<object>)lstData.ElementAt(3);
            if (lstChild.Count > 0) // Zone
            {
                Dropdownlist.DDLLoading(ddlZone, false, (int)Constants.DropDownFirstOption.Select, lstChild);
                ddlZone.Enabled = true;
                if (lstChild.Count == 1)
                {
                    ddlZone.SelectedIndex = 1;
                    ddlZone.Enabled = false;
                }
            }
        }
        private void LoadDDL(int _DDLType, long _SearchID)
        {
            WaterLossesBLL bll_waterLosses = new WaterLossesBLL();
            List<object> lstData = new List<object>();
            DropDownList ddlToLoad = null;

            switch (_DDLType)
            {
                case ZONE: // Zone
                    lstData = bll_waterLosses.GetAllZones();
                    ddlToLoad = ddlZone;
                    break;

                case CIRCLE: // Circle
                    lstData = bll_waterLosses.GetCirclesByZoneID(_SearchID);
                    ddlToLoad = ddlCircle;
                    break;

                case DIVISION: // Division
                    lstData = bll_waterLosses.GetDivisionsByCircleID(_SearchID);
                    ddlToLoad = ddlDiv;
                    break;

                default:
                    break;
            }
            if (lstData.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.Select, lstData);
                ddlToLoad.Enabled = true;
            }
        }

        private void DrawDivisionalDataTable(List<WaterLossesDAL.SubDiv_WL> _LstData)
        {
            string strTitle = "";
            if (rbYearly.Checked)
                strTitle = "Yearly Loss of " + ddlDiv.SelectedItem.Text + " from " + ddlFromYear.SelectedItem.Text + " to " + ddlToYear.SelectedItem.Text;
            else if (rbMnthly.Checked)
                strTitle = "Monthly Loss of " + ddlDiv.SelectedItem.Text + " for " + ddlSeason.SelectedItem.Text + " " + ddlMonthlyYear.SelectedItem.Text;
            else if (rdDaily.Checked)
                strTitle = "Daily Loss of " + ddlDiv.SelectedItem.Text + " from "
                    + Utility.GetParsedDate(txtFromDate.Text).ToString("dd-MMM-yyyy")
                    + " to " + Utility.GetParsedDate(txtToDate.Text).ToString("dd-MMM-yyyy");

            lblheader.Text = strTitle;

            List<long> lstSubDivIDs = _LstData.Where(x => x.ParentID > 0).Select(x => x.ParentID).Distinct().ToList();
            List<string> keys = _LstData.ElementAt(0).LstAttributes.Keys.ToList();
            int ColCount = keys.Count;

            #region Generate Divional Water Losses Table Header
            TableHeaderRow header_upper = new TableHeaderRow();
            header_upper.Attributes.Add("class", "text-center table-header");
            header_upper.Cells.Add(GetHeaderCell("", 1, 1, "width:230px;text-align:left;"));
            header_upper.Cells.Add(GetHeaderCell("Design Discharge", 1, 1, "width:130px;text-align:left;"));
            foreach (var k in keys)
            {
                header_upper.Cells.Add(GetHeaderCell(k, 1, 1, "text-align:right;"));
            }

            if (ColCount == 1)
            {
                header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
                header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
            }
            else if (ColCount == 2)
            {
                header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
            }
            tblLG.Rows.Add(header_upper);
            #endregion

            Dictionary<string, double?> lst_TotalDivDschrg = new Dictionary<string, double?>();
            Dictionary<string, double?> lst_TotalDiversion = new Dictionary<string, double?>();
            Dictionary<string, double?> lst_TotalWaterToNextDiv = new Dictionary<string, double?>();

            foreach (var k in keys)
            {
                #region Divisional Discharge

                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;

                if (rdDaily.Checked)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text);
                    ToDate = Utility.GetParsedDate(txtToDate.Text);

                    string Date = k;

                    if (FromDate.Year == ToDate.Year)
                    {
                        Date = k + "-" + ToDate.Year;
                    }
                    else
                    {
                        if (FromDate.Month == DateTime.Parse(k).Month)
                        {
                            Date = k + "-" + FromDate.Year;
                        }
                        else
                        {
                            Date = k + "-" + ToDate.Year;
                        }
                    }

                    FromDate = DateTime.Parse(Date);
                    ToDate = FromDate;
                }
                else if (rbMnthly.Checked)
                {
                    string strYear = ddlMonthlyYear.SelectedItem.Value;

                    if (ddlSeason.SelectedItem.Value == ((long)Constants.Seasons.Rabi).ToString()) //Rabi Season
                    {
                        string fromYear = (strYear.Split('-'))[0];

                        FromDate = DateTime.Parse(string.Format("{0}-{1}-{2}", 1, k, fromYear));

                        if (FromDate.Month < 4)
                        {
                            FromDate = FromDate.AddYears(1);
                        }

                        ToDate = FromDate.AddMonths(1).AddDays(-1);
                    }
                    else if (ddlSeason.SelectedItem.Value == ((long)Constants.Seasons.Kharif).ToString()) // Kharif Season
                    {
                        FromDate = DateTime.Parse(string.Format("{0}-{1}-{2}", 1, k, strYear));
                        ToDate = FromDate.AddMonths(1).AddDays(-1);
                    }
                }
                else if (rbYearly.Checked)
                {
                    string fromYear = (k.Split('-'))[0];
                    FromDate = DateTime.Parse(string.Format("{0}-{1}-{2}", 1, "Apr", fromYear));
                    ToDate = FromDate.AddYears(1).AddDays(-1);
                }

                long DivisionID = Convert.ToInt64(ddlDiv.SelectedItem.Value);

                DataTable DischargeData = new WaterLossesBLL().GetCurrentAndNextDivisionalDischarge(DivisionID, FromDate, ToDate);

                #endregion

                lst_TotalDivDschrg.Add(k, Convert.ToDouble(DischargeData.Rows[0][0]));
                lst_TotalDiversion.Add(k, 0);
                lst_TotalWaterToNextDiv.Add(k, Convert.ToDouble(DischargeData.Rows[0][1]));
            }

            #region Generate Data Rows
            foreach (var subDivID in lstSubDivIDs)
            {
                string str_subDivName = _LstData.Where(x => x.ParentID > 0 && x.ParentID == subDivID).Select(x => x.ParentName).ToList().ElementAt(0);

                TableRow row_subDivName = new TableRow();
                TableCell cell = GetCell(str_subDivName, "text-left bitext");
                row_subDivName.Cells.Add(cell);

                TableCell cell2 = GetCell("", "");
                cell2.ColumnSpan = keys.Count + 1;
                row_subDivName.Cells.Add(cell2);

                if (ColCount == 1)
                {
                    row_subDivName.Cells.Add(GetCell("", "text-right"));
                    row_subDivName.Cells.Add(GetCell("", "text-right"));
                }
                else if (ColCount == 2)
                {
                    row_subDivName.Cells.Add(GetCell("", "text-right"));
                }
                tblLG.Rows.Add(row_subDivName);

                List<WaterLossesDAL.SubDiv_WL> lstSubDivData = _LstData.Where(x => x.ParentID > 0 && x.ParentID == subDivID).ToList();
                foreach (var chnl in lstSubDivData)
                {
                    double? offtakeDischarge = null;

                    if (chnl.LstAttributes_offtakes.Count != 0 && keys.Count != 0)
                    {
                        offtakeDischarge = chnl.LstAttributes_offtakes[keys[0]];
                    }

                    TableRow row_MainChannel = new TableRow();
                    row_MainChannel.Cells.Add(GetCell("&nbsp;&nbsp;&nbsp;" + chnl.Name, "text-left"));
                    row_MainChannel.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.DD), "text-right"));
                    //offtakes discharges
                    TableRow row_offtakes = new TableRow();
                    row_offtakes.Cells.Add(GetCell("&nbsp;&nbsp;&nbsp;" + "Offtakes Discharge", "text-left"));
                    row_offtakes.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.TotalOfftakeDD), "text-right"));
                    //offtakes discharges
                    TableRow row_outlets = new TableRow();
                    row_outlets.Cells.Add(GetCell("&nbsp;&nbsp;&nbsp;" + "Direct Outlets Discharge", "text-left"));
                    //row_outlets.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.TotalOutletDD), "text-right"));
                    row_outlets.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.LstAttributes_Outlets[keys[0]]), "text-right"));

                    foreach (var k in keys)
                    {
                        //chnl.LstAttributes_Outlets[k] = chnl.TotalOutletDD; Getting outlet discharge based on calculated values.
                        // Sub divisional discharge
                        if (chnl.LstAttributes.ContainsKey(k))
                        {
                            //if (chnl.isOutlet)//is divisional gauge check
                            //{
                            //    double? value = chnl.LstAttributes[k];
                            //    if (value != null)
                            //        lst_TotalDivDschrg[k] = lst_TotalDivDschrg[k] + value;
                            //}

                            if (offtakeDischarge == null && chnl.LstAttributes[k] != null)
                            {
                                lst_TotalDiversion[k] = lst_TotalDiversion[k] + chnl.LstAttributes[k];
                            }

                            row_MainChannel.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.LstAttributes[k]), "text-right"));
                        }
                        else
                            row_MainChannel.Cells.Add(GetCell("", "text-right"));

                        //Offtake discharges
                        if (chnl.LstAttributes_offtakes.ContainsKey(k))
                        {
                            double? value = chnl.LstAttributes_offtakes[k];
                            if (value != null)
                                lst_TotalDiversion[k] = lst_TotalDiversion[k] + value;
                            row_offtakes.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.LstAttributes_offtakes[k]), "text-right"));
                        }
                        else
                            row_offtakes.Cells.Add(GetCell("", "text-right"));

                        //direct outlets discharges
                        if (chnl.LstAttributes_Outlets.ContainsKey(k))
                        {
                            double? value = chnl.LstAttributes_Outlets[k];
                            if (value != null)
                                lst_TotalDiversion[k] = lst_TotalDiversion[k] + value;
                            row_outlets.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(chnl.LstAttributes_Outlets[k]), "text-right"));
                        }
                        else
                            row_outlets.Cells.Add(GetCell("", "text-right"));
                    }

                    if (ColCount == 1)
                    {
                        row_MainChannel.Cells.Add(GetCell("", "text-right"));
                        row_MainChannel.Cells.Add(GetCell("", "text-right"));

                        row_offtakes.Cells.Add(GetCell("", "text-right"));
                        row_offtakes.Cells.Add(GetCell("", "text-right"));

                        row_outlets.Cells.Add(GetCell("", "text-right"));
                        row_outlets.Cells.Add(GetCell("", "text-right"));
                    }
                    else if (ColCount == 2)
                    {
                        row_MainChannel.Cells.Add(GetCell("", "text-right"));
                        row_offtakes.Cells.Add(GetCell("", "text-right"));
                        row_outlets.Cells.Add(GetCell("", "text-right"));
                    }

                    tblLG.Rows.Add(row_MainChannel);

                    if (offtakeDischarge != null)
                    {
                        tblLG.Rows.Add(row_offtakes);
                        tblLG.Rows.Add(row_outlets);
                    }
                }
            }
            #endregion

            #region Generate Total Rows

            // Total Water to Next Divisions
            TableRow row_TDD = new TableRow();
            row_TDD.Cells.Add(GetCell("Total Water to Next Division(s)", "text-left text-bold"));
            row_TDD.Cells.Add(GetCell("", "text-left text-bold"));
            //Dictionary<string, double?> lst_TotalWaterToNextDiv = _LstData.Where(x => x.ParentID == -111).ToList().ElementAt(0).LstAttributes;
            foreach (var a in lst_TotalWaterToNextDiv)
            {
                row_TDD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(a.Value), "text-right"));
            }
            tblLG.Rows.Add(row_TDD);

            // Total Divisional Discharge Row 
            TableRow row_DD = new TableRow();
            row_DD.Cells.Add(GetCell("Total Divisional Discharge", "text-left text-bold"));
            row_DD.Cells.Add(GetCell("", "text-left text-bold"));
            foreach (var a in lst_TotalDivDschrg)
            {
                row_DD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(a.Value), "text-right"));
            }
            tblLG.Rows.Add(row_DD);

            // Total Diversion Row
            TableRow row_TD = new TableRow();
            row_TD.Cells.Add(GetCell("Total Diversion", "text-left text-bold"));
            row_TD.Cells.Add(GetCell("", "text-left text-bold"));
            foreach (var a in lst_TotalDiversion)
            {
                row_TD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(a.Value), "text-right"));
            }
            tblLG.Rows.Add(row_TD);

            // Total loss
            Dictionary<string, double?> losses = new Dictionary<string, double?>();

            foreach (var g in lst_TotalDiversion)
            {
                losses[g.Key] = lst_TotalDiversion[g.Key] - lst_TotalDivDschrg[g.Key];
            }
            TableRow row_losses = new TableRow();
            TableRow row_lossesPrcntg = new TableRow();

            row_losses.Cells.Add(GetCell("Losses", "text-left text-bold"));
            row_losses.Cells.Add(GetCell("", "text-left text-bold"));
            row_lossesPrcntg.Cells.Add(GetCell("% Losses", "text-left text-bold"));
            row_lossesPrcntg.Cells.Add(GetCell("", "text-left text-bold"));
            int count = 0;
            double? totalDivDschrg = 0, totalLosses = 0;

            foreach (var a in losses)
            {
                row_losses.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(losses[a.Key]), "text-right"));

                double? divisional = lst_TotalDivDschrg[a.Key];
                double? loss = losses[a.Key];
                double? prcntgLoss = 0;

                if (divisional > 0)
                    prcntgLoss = (loss / divisional) * 100;

                row_lossesPrcntg.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(prcntgLoss), "text-right"));

                totalDivDschrg = totalDivDschrg + divisional;
                totalLosses = totalLosses + loss;
                count = count + 1;
            }

            if (losses.Count == 2)
            {
                row_DD.Cells.Add(GetCell("", "text-right"));
                row_TD.Cells.Add(GetCell("", "text-right"));
                row_losses.Cells.Add(GetCell("", "text-right"));
                row_lossesPrcntg.Cells.Add(GetCell("", "text-right"));
            }
            else if (losses.Count == 1)
            {
                row_TDD.Cells.Add(GetCell("", "text-right"));
                row_TDD.Cells.Add(GetCell("", "text-right"));

                row_DD.Cells.Add(GetCell("", "text-right"));
                row_TD.Cells.Add(GetCell("", "text-right"));

                row_DD.Cells.Add(GetCell("", "text-right"));
                row_TD.Cells.Add(GetCell("", "text-right"));

                row_losses.Cells.Add(GetCell("", "text-right"));
                row_lossesPrcntg.Cells.Add(GetCell("", "text-right"));

                row_losses.Cells.Add(GetCell("", "text-right"));
                row_lossesPrcntg.Cells.Add(GetCell("", "text-right"));
            }
            tblLG.Rows.Add(row_losses);
            tblLG.Rows.Add(row_lossesPrcntg);

            if (count > 0)
            {
                double? avgDschrg = totalDivDschrg / count;

                {
                    double? avgLosses = totalLosses / count;
                    albl.Text = Utility.GetRoundOffValueOneDecimal(avgDschrg);
                    blbl.Text = Utility.GetRoundOffValueOneDecimal(avgLosses);
                    if (avgDschrg > 0)
                        clbl.Text = Utility.GetRoundOffValueOneDecimal((avgLosses / avgDschrg) * 100);
                }
            }
            #endregion

            showErrorDiv(false);
        }

        private TableCell GetHeaderCell(string _CellValue, int _ColSpan, int _RowSpan, string _Style)
        {
            TableCell cell = new TableCell();
            cell.Text = (string.IsNullOrEmpty(_CellValue) ? "" : _CellValue);
            cell.Attributes.Add("style", _Style);
            cell.ColumnSpan = _ColSpan;
            cell.RowSpan = _RowSpan;

            return cell;
        }
        private TableCell GetCell(string _CellValue, string _CssClass)
        {
            TableCell cell = new TableCell();
            cell.Text = (string.IsNullOrEmpty(_CellValue) ? "" : _CellValue);
            cell.Attributes.Add("class", _CssClass);
            return cell;
        }

        private bool VerifyDateRange()
        {
            if (string.IsNullOrEmpty(txtFromDate.Text) || string.IsNullOrEmpty(txtToDate.Text))
            {
                Master.ShowMessage(Message.FromToDateBothRequired.Description, SiteMaster.MessageType.Error);
                return false;
            }
            DateTime fromDate = Utility.GetParsedDate(txtFromDate.Text), toDate = Utility.GetParsedDate(txtToDate.Text);

            //Future date check
            if (fromDate > DateTime.Now || toDate > DateTime.Now)
            {
                Master.ShowMessage(Message.FutureDateNotAllowed.Description, SiteMaster.MessageType.Error);
                return false;
            }

            if (fromDate > toDate)
            {
                Master.ShowMessage(Message.FromDateCannotBeGreaterThanToDate.Description, SiteMaster.MessageType.Error);
                return false;
            }
            //6 month date duration check 
            if ((toDate.Subtract(fromDate)).Days > 30)
            {
                Master.ShowMessage(Message.DateRangeCannotBeMoreThan15Days.Description, SiteMaster.MessageType.Error);
                return false;
            }
            return true;
        }
        private bool VerifyMonthRange()
        {
            if (string.IsNullOrEmpty(ddlSeason.SelectedItem.Value))
            {
                Master.ShowMessage("Select a season.", SiteMaster.MessageType.Error);
                return false;
            }
            return true;
        }
        private bool VerifyYearRange()
        {
            bool result = true;
            if (string.IsNullOrEmpty(ddlFromYear.SelectedItem.Value) || string.IsNullOrEmpty(ddlToYear.SelectedItem.Value))
            {
                Master.ShowMessage("From year and To year both are required.", SiteMaster.MessageType.Error);
                return false;
            }

            int from = Convert.ToInt32((ddlFromYear.SelectedItem.Value.Split('-'))[0]), to = Convert.ToInt32((ddlToYear.SelectedItem.Value.Split('-'))[0]);

            if (to < from)
            {
                Master.ShowMessage("From year cannot be greater than To year.", SiteMaster.MessageType.Error);
                return false;
            }

            else if (to - from > 6)
            {
                Master.ShowMessage("Year range cannot be more than 6 years.", SiteMaster.MessageType.Error);
                return false;
            }
            return true;
        }

        private void showErrorDiv(bool show)
        {
            divError.Visible = show;
            dailyLG.Visible = !show;
            lgTotal.Visible = !show;
            tblLG.Visible = !show;
            if (show)
                albl.Text = blbl.Text = clbl.Text = "";

        }
        private void HideDataDivs()
        {
            divError.Visible = false;
            dailyLG.Visible = false;
            lgTotal.Visible = false;

            albl.Text = blbl.Text = clbl.Text = "";
            lblheader.Text = "";

        }
        private void HideDivs()
        {
            divDaily.Visible = false;
            divMonthly.Visible = false;
            divYearly.Visible = false;

            txtFromDate.Text = "";
            txtToDate.Text = "";

            ddlToYear.Items.Clear();
            ddlFromYear.Items.Clear();

            ddlMonthlyYear.Items.Clear();
            ddlSeason.SelectedIndex = 0;
            //ddlFrmMnth.SelectedIndex = 0;
            //ddlToMnth.SelectedIndex = 0;

            lblheader.Text = "";
            lgTotal.Visible = false;

        }

    }

}