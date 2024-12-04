using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using System.Data;

namespace PMIU.WRMIS.Web.Modules.WaterLosses
{
    public partial class SubDivisionalWaterLosses : BasePage
    {
        private const int ZONE = 1, CIRCLE = 2, DIVISION = 3, SUB_DIVISION = 4;

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
                string str_subDiv = ddlSubDiv.SelectedItem.Value;
                if (string.IsNullOrEmpty(str_subDiv))
                {
                    Master.ShowMessage(Message.SelectASubDivision.Description, SiteMaster.MessageType.Error);
                    rbMnthly.Checked = false;
                    return;
                }

                List<object> lstYears = new WaterLossesBLL().GetSubDivisonalWLYearList(Convert.ToInt64(str_subDiv));
                if (lstYears != null && lstYears.Count > 0)
                {
                    ddlMonthlyYear.Items.Clear();
                    ddlMonthlyYear.Items.Add(new ListItem("Select", ""));
                    ddlMonthlyYear.Enabled = false;
                    //Dropdownlist.DDLLoading(ddlMonthlyYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                    //ddlMonthlyYear.Enabled = true;

                    //ddlFrmMnth.Enabled = true;
                    //ddlToMnth.Enabled = true;
                }
                else
                {
                    ddlMonthlyYear.Items.Clear();
                    ddlMonthlyYear.Items.Add(new ListItem("Select", ""));
                    ddlMonthlyYear.Enabled = false;

                    //ddlToMnth.Enabled = false ;
                    //ddlFrmMnth.Enabled = false;
                }

                divMonthly.Visible = true;
            }
            else if (rbYearly.Checked)
            {
                string str_subDiv = ddlSubDiv.SelectedItem.Value;
                if (string.IsNullOrEmpty(str_subDiv))
                {
                    Master.ShowMessage(Message.SelectASubDivision.Description, SiteMaster.MessageType.Error);
                    rbYearly.Checked = false;
                    return;
                }

                List<object> lstYears = new WaterLossesBLL().GetSubDivisonalWL_RabiYears(Convert.ToInt64(str_subDiv));
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
                DisableDD(ddlSubDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(CIRCLE, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlCircle"))
            {
                DisableDD(ddlDiv);
                DisableDD(ddlSubDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(DIVISION, Convert.ToInt64(strValue));
            }
            else if (sndr.ID.Equals("ddlDiv"))
            {
                DisableDD(ddlSubDiv);
                if (!string.IsNullOrEmpty(strValue))
                    LoadDDL(SUB_DIVISION, Convert.ToInt64(strValue));
            }

            rdDaily.Checked = false;
            rbMnthly.Checked = false;
            rbYearly.Checked = false;

            HideDivs();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            HideDataDivs();
            List<WaterLossesDAL.SubDiv_WL> lstData = new List<WaterLossesDAL.SubDiv_WL>();

            if (string.IsNullOrEmpty(ddlSubDiv.SelectedItem.Value))
            {
                Master.ShowMessage(Message.SelectASubDivision.Description, SiteMaster.MessageType.Error);
                return;
            }

            if (rbMnthly.Checked == false && rbYearly.Checked == false && rdDaily.Checked == false)
            {
                Master.ShowMessage("Select a radio button", SiteMaster.MessageType.Error);
                return;
            }

            #region Daily Water Losses
            if (rdDaily.Checked)
            {
                long ID = Convert.ToInt64(ddlSubDiv.SelectedItem.Value);
                DateTime fromDate = DateTime.Today, toDate = DateTime.Today;

                if (VerifyDateRange())
                {
                    if (!string.IsNullOrEmpty(txtFromDate.Text))
                        fromDate = Utility.GetParsedDate(txtFromDate.Text);

                    if (!string.IsNullOrEmpty(txtToDate.Text))
                        toDate = Utility.GetParsedDate(txtToDate.Text);

                    lstData = new WaterLossesBLL().GetSubDivisionalLoss_Daily(ID, fromDate, toDate);

                    if (lstData.Count > 0)
                        DrawTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

            #region Monthly Water Losses
            else if (rbMnthly.Checked)
            {
                long ID = Convert.ToInt64(ddlSubDiv.SelectedItem.Value);
                int year = 0, fromMonth = 0, toMonth = 0;

                string strValue = ddlMonthlyYear.SelectedItem.Value;
                if (string.IsNullOrEmpty(strValue))
                {
                    Master.ShowMessage("Year is a reqruied field.", SiteMaster.MessageType.Error);
                    return;
                }
                if (VerifyMonthRange())
                {
                    //strValue = ddlFrmMnth.SelectedItem.Value;
                    //if (!string.IsNullOrEmpty(strValue))
                    //    fromMonth = Convert.ToInt32(strValue);

                    //strValue = ddlToMnth.SelectedItem.Value;
                    //if (!string.IsNullOrEmpty(strValue))
                    //    toMonth = Convert.ToInt32(strValue);

                    if (ddlSeason.SelectedItem.Value == "1") //Rabi Season
                    {
                        //Need from year and from month
                        year = -11;
                        fromMonth = Convert.ToInt32((strValue.Split('-'))[0]); toMonth = Convert.ToInt32((strValue.Split('-'))[1]);
                        lstData = new WaterLossesBLL().GetSubDivisionalLosses(ID, year, fromMonth, toMonth);
                    }
                    else if (ddlSeason.SelectedItem.Value == "2")// Kharif season
                    {
                        year = Convert.ToInt32(strValue);
                        fromMonth = 4; toMonth = 9;
                        lstData = new WaterLossesBLL().GetSubDivisionalLosses(ID, year, fromMonth, toMonth);
                    }


                    if (lstData.Count > 0)
                        DrawTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

            #region Yealry Water Losses
            else if (rbYearly.Checked)
            {
                long ID = Convert.ToInt64(ddlSubDiv.SelectedItem.Value);
                int year = 0, fromYear = 0, toYear = 0;
                if (VerifyYearRange())
                {
                    string strValue = "";
                    if (!string.IsNullOrEmpty(strValue))
                        ID = Convert.ToInt64(strValue);

                    strValue = ddlFromYear.SelectedItem.Value;
                    if (!string.IsNullOrEmpty(strValue))
                        fromYear = Convert.ToInt32(strValue.Split('-')[0]);

                    strValue = ddlToYear.SelectedItem.Value;
                    if (!string.IsNullOrEmpty(strValue))
                        toYear = Convert.ToInt32(strValue.Split('-')[1]);

                    lstData = new WaterLossesBLL().GetSubDivisionalLosses(ID, year, fromYear, toYear);

                    if (lstData.Count > 0)
                        DrawTable(lstData);
                    else
                        showErrorDiv(true);
                }
            }
            #endregion

        }
        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideDataDivs();
            string str_subDiv = ddlSubDiv.SelectedItem.Value;
            if (string.IsNullOrEmpty(str_subDiv))
            {
                Master.ShowMessage(Message.SelectASubDivision.Description, SiteMaster.MessageType.Error);
                rbMnthly.Checked = false;
                return;
            }

            if (string.IsNullOrEmpty(ddlSeason.SelectedItem.Value))
                return;
            else if (ddlSeason.SelectedItem.Value == "1") // Rabi Season
            {
                ddlMonthlyYear.Items.Clear();
                List<object> lstYears = new WaterLossesBLL().GetSubDivisonalWL_RabiYears(Convert.ToInt64(str_subDiv));
                Dropdownlist.DDLLoading(ddlMonthlyYear, false, (int)Constants.DropDownFirstOption.Select, lstYears);
                ddlMonthlyYear.Enabled = true;
            }
            else if (ddlSeason.SelectedItem.Value == "2") // Kharif Season
            {
                ddlMonthlyYear.Items.Clear();
                List<object> lstYears = new WaterLossesBLL().GetSubDivisonalWLYearList(Convert.ToInt64(str_subDiv));
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

            List<object> lstChild = (List<object>)lstData.ElementAt(0);
            if (lstChild.Count > 0) // Subdivision
            {
                Dropdownlist.DDLLoading(ddlSubDiv, false, (int)Constants.DropDownFirstOption.Select, lstChild);
                ddlSubDiv.Enabled = true;
                if (lstChild.Count == 1)
                {
                    ddlSubDiv.SelectedIndex = 1;
                    ddlSubDiv.Enabled = false;
                }
            }

            lstChild = (List<object>)lstData.ElementAt(1);
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

                case SUB_DIVISION: // Subdivision
                    lstData = bll_waterLosses.GetSubDivByDivisionID(_SearchID);
                    ddlToLoad = ddlSubDiv;
                    break;

                default:
                    break;
            }
            if (lstData.Count > 0)
            {
                Dropdownlist.DDLLoading(ddlToLoad, false, (int)Constants.DropDownFirstOption.Select, lstData);
                ddlToLoad.Enabled = true;
                //if (lstData.Count == 1)
                //{
                //    ddlToLoad.SelectedIndex = 1;
                //  //  ddlToLoad.Enabled = false;
                //}
            }
        }
        private void DrawTable(List<WaterLossesDAL.SubDiv_WL> lstData)
        {
            #region show Table
            if (lstData.Count > 0)
            {
                //fill loss and gain table
                #region Table Header
                //1. fill header fields
                TableHeaderRow header_upper = new TableHeaderRow();
                header_upper.Attributes.Add("class", "text-center table-header");
                header_upper.Cells.Add(GetHeaderCell("Channel Name", 1, 1, "width:230px;text-align:left;"));
                header_upper.Cells.Add(GetHeaderCell("Design Discharge", 1, 1, "width:130px;text-align:right;"));

                string strTitle = "";
                if (rbYearly.Checked)
                    strTitle = "Yearly Loss of " + ddlSubDiv.SelectedItem.Text + " from " + ddlFromYear.SelectedItem.Text + " to " + ddlToYear.SelectedItem.Text;
                else if (rbMnthly.Checked)
                    strTitle = "Monthly Loss of " + ddlSubDiv.SelectedItem.Text + " for " + ddlSeason.SelectedItem.Text + " " + ddlMonthlyYear.SelectedItem.Text;
                //   + ddlFrmMnth.SelectedItem.Text + " " + ddlMonthlyYear.SelectedItem.Text
                //   + " to " + ddlToMnth.SelectedItem.Text + " " + ddlMonthlyYear.SelectedItem.Text;
                else if (rdDaily.Checked)
                    strTitle = "Daily Loss of " + ddlSubDiv.SelectedItem.Text + " from "
                        + Utility.GetParsedDate(txtFromDate.Text).ToString("dd-MMM-yyyy")
                        + " to " + Utility.GetParsedDate(txtToDate.Text).ToString("dd-MMM-yyyy");

                lblheader.Text = strTitle;

                List<string> keys = lstData.ElementAt(0).LstAttributes.Keys.ToList();
                foreach (var k in keys)
                    header_upper.Cells.Add(GetHeaderCell(k, 1, 1, "text-align:right;"));

                if (keys.Count == 2)
                {
                    header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
                }
                else if (keys.Count == 1)
                {
                    header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
                    header_upper.Cells.Add(GetHeaderCell("", 1, 1, "text-align:right;"));
                }
                tblLG.Rows.Add(header_upper);
                #endregion

                int rowCount = 0;

                //2. Row filling
                foreach (var d in lstData)
                {
                    if (d.ID != d.ParentID)
                    {
                        if (rowCount > 0 && d.ParentID == 0)
                        {
                            TableRow tempRow = new TableRow();
                            tempRow.Attributes.Add("style", "background-color:white");
                            tempRow.Cells.Add(GetHeaderCell("", (keys.Count + 2), 1, ""));
                            tblLG.Rows.Add(tempRow);
                        }

                        TableRow row = new TableRow();

                        if (d.Name.Equals("Total"))
                        {
                            row.Attributes.Add("class", "border-bottom");
                            row.Cells.Add(GetCell(d.Name, "text-left"));
                        }
                        else
                        {
                            if (d.ParentID == 0)
                                row.Cells.Add(GetCell(d.Name, "text-left bitext"));
                            else if (d.ParentID < 0)
                                row.Cells.Add(GetCell(d.Name, "text-left"));
                            else
                                row.Cells.Add(GetCell("&nbsp;&nbsp;&nbsp;" + d.Name, "text-left"));
                        }

                        row.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(d.DD), "text-right"));

                        foreach (var k in keys)
                        {
                            if (d.LstAttributes.ContainsKey(k))
                                row.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(d.LstAttributes[k]), "text-right"));
                            else
                                row.Cells.Add(GetCell("", "text-right"));
                        }
                        if (keys.Count == 2)
                        {
                            row.Cells.Add(GetCell("", "text-right"));
                        }
                        else if (keys.Count == 1)
                        {
                            row.Cells.Add(GetCell("", "text-right"));
                            row.Cells.Add(GetCell("", "text-right"));
                        }
                        tblLG.Rows.Add(row);
                    }

                    rowCount++;
                }

                showErrorDiv(false);
            }
            else
            {
                //show no record found div
                showErrorDiv(true);
            }

            #region Calculate Sub Division and Next Sub Division Discharge

            Dictionary<string, double?> lst_TotalSubDivDischarge = new Dictionary<string, double?>();
            Dictionary<string, double?> lst_TotalWaterToNextSubDiv = new Dictionary<string, double?>();

            List<string> DateKeys = lstData.ElementAt(0).LstAttributes.Keys.ToList();

            foreach (var k in DateKeys)
            {
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

                long SubDivisionID = Convert.ToInt64(ddlSubDiv.SelectedItem.Value);

                DataTable DischargeData = new WaterLossesBLL().GetCurrentAndNextSubDivisionalDischarge(SubDivisionID, FromDate, ToDate);

                lst_TotalSubDivDischarge.Add(k, Convert.ToDouble(DischargeData.Rows[0][0]));
                lst_TotalWaterToNextSubDiv.Add(k, Convert.ToDouble(DischargeData.Rows[0][1]));
            }

            #endregion

            #region Total Water to Next SubDivisions Row

            //Calculate Total Water to Next SubDivisions
            //List<WaterLossesDAL.SubDiv_WL> lst_parentDD1 = (from a in lstData where a.ParentID == 0 select a).ToList();
            WaterLossesDAL.SubDiv_WL mdlTotalNextSubDivDschrg = new WaterLossesDAL.SubDiv_WL();
            mdlTotalNextSubDivDschrg.Name = "Total Water to Next SubDivision(s)";
            mdlTotalNextSubDivDschrg.ID = -1;
            mdlTotalNextSubDivDschrg.ParentID = -1;
            mdlTotalNextSubDivDschrg.DD = null; // lst_parentDD.Sum(x => x.DD);
            mdlTotalNextSubDivDschrg.LstAttributes = null;
            //mdlTotaNextSubDivDschrg.LstAttributes = new Dictionary<string, double?>();

            //foreach (var b in lst_parentDD1.ElementAt(0).LstAttributes)
            //    mdlTotalSubDivDschrg1.LstAttributes.Add(b.Key, 0);

            //foreach (var c in lst_parentDD1)
            //{
            //    foreach (var d in c.LstAttributes)
            //    {
            //        double? child = (c.LstAttributes[d.Key] == null ? 0 : c.LstAttributes[d.Key]);
            //        double? total = (mdlTotalSubDivDschrg1.LstAttributes[d.Key] == null ? 0 : mdlTotalSubDivDschrg1.LstAttributes[d.Key]);
            //        total = total + child;
            //        mdlTotalSubDivDschrg1.LstAttributes[d.Key] = total;
            //    }
            //}

            // Draw Total Water to Next SubDivisions
            TableRow rowNSD = new TableRow();

            rowNSD.Cells.Add(GetCell(mdlTotalNextSubDivDschrg.Name, "text-left text-bold"));

            rowNSD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(mdlTotalNextSubDivDschrg.DD), "text-right"));

            foreach (var k in DateKeys)
            {
                rowNSD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(lst_TotalWaterToNextSubDiv[k]), "text-right"));
            }

            //foreach (var a in mdlTotalNextSubDivDschrg.LstAttributes)
            //{
            //    rowDD1.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(a.Value), "text-right"));
            //}

            if (lst_TotalWaterToNextSubDiv.Count == 2)
            {
                rowNSD.Cells.Add(GetCell("", "text-right"));
            }
            else if (lst_TotalWaterToNextSubDiv.Count == 1)
            {
                rowNSD.Cells.Add(GetCell("", "text-right"));
                rowNSD.Cells.Add(GetCell("", "text-right"));
            }
            tblLG.Rows.Add(rowNSD);
            #endregion

            #region Total Divisional Discharge Row

            //Calculate Total Subdivisional discharges
            //List<WaterLossesDAL.SubDiv_WL> lst_parentDD = (from a in lstData where a.ParentID == 0 select a).ToList();
            WaterLossesDAL.SubDiv_WL mdlTotalSubDivDschrg = new WaterLossesDAL.SubDiv_WL();
            mdlTotalSubDivDschrg.Name = "Total Sub-Divisional Discharge";
            mdlTotalSubDivDschrg.ID = -1;
            mdlTotalSubDivDschrg.ParentID = -1;
            mdlTotalSubDivDschrg.DD = null; // lst_parentDD.Sum(x => x.DD);
            mdlTotalSubDivDschrg.LstAttributes = null;
            //mdlTotalSubDivDschrg.LstAttributes = new Dictionary<string, double?>();

            //foreach (var b in lst_parentDD.ElementAt(0).LstAttributes)
            //    mdlTotalSubDivDschrg.LstAttributes.Add(b.Key, 0);

            //foreach (var c in lst_parentDD)
            //{
            //    foreach (var d in c.LstAttributes)
            //    {
            //        double? child = (c.LstAttributes[d.Key] == null ? 0 : c.LstAttributes[d.Key]);
            //        double? total = (mdlTotalSubDivDschrg.LstAttributes[d.Key] == null ? 0 : mdlTotalSubDivDschrg.LstAttributes[d.Key]);
            //        total = total + child;
            //        mdlTotalSubDivDschrg.LstAttributes[d.Key] = total;
            //    }
            //}

            // Draw Total Divisional Discharge Row
            TableRow rowDD = new TableRow();

            rowDD.Cells.Add(GetCell(mdlTotalSubDivDschrg.Name, "text-left text-bold"));

            rowDD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(mdlTotalSubDivDschrg.DD), "text-right"));
            //foreach (var a in mdlTotalSubDivDschrg.LstAttributes)
            //{
            //    rowDD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(lst_TotalSubDivDischarge[a.Key]), "text-right"));
            //}

            foreach (var k in DateKeys)
            {
                rowDD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(lst_TotalSubDivDischarge[k]), "text-right"));
            }

            if (lst_TotalSubDivDischarge.Count == 2)//(mdlTotalSubDivDschrg.LstAttributes.Count == 2)
            {
                rowDD.Cells.Add(GetCell("", "text-right"));
            }
            else if (lst_TotalSubDivDischarge.Count == 1)//(mdlTotalSubDivDschrg.LstAttributes.Count == 1)
            {
                rowDD.Cells.Add(GetCell("", "text-right"));
                rowDD.Cells.Add(GetCell("", "text-right"));
            }
            tblLG.Rows.Add(rowDD);

            #endregion

            #region Total Reported Discharge

            //Calculate Child channel Sum of discharges
            //List<WaterLossesDAL.SubDiv_WL> lst_ReportedDircharge = (from a in lstData where (a.ParentID == -11 || a.ParentID == -12) select a).ToList();
            List<WaterLossesDAL.SubDiv_WL> lst_ReportedDircharge = (from a in lstData where (a.ParentID == -11 || (a.ID == a.ParentID && a.ParentID != -12)) select a).ToList();

            List<string> keysAll = lstData.ElementAt(0).LstAttributes.Keys.ToList();

            WaterLossesDAL.SubDiv_WL mdlReportedDircharge = new WaterLossesDAL.SubDiv_WL();
            mdlReportedDircharge.Name = "Total Reported Discharge";
            mdlReportedDircharge.ID = -2;
            mdlReportedDircharge.ParentID = -2;
            mdlReportedDircharge.DD = null;// lst_ReportedDircharge.Sum(x => x.DD);
            mdlReportedDircharge.LstAttributes = new Dictionary<string, double?>();
            foreach (var k in keysAll)
                mdlReportedDircharge.LstAttributes.Add(k, 0);

            foreach (var c in lst_ReportedDircharge)
            {
                foreach (var k in keysAll)
                {
                    if (c.LstAttributes.ContainsKey(k))
                    {
                        double? value = c.LstAttributes[k];
                        mdlReportedDircharge.LstAttributes[k] = mdlReportedDircharge.LstAttributes[k] + value;
                    }
                }
            }

            // Draw Total Reported Discharge Row
            TableRow rowRD = new TableRow();

            rowRD.Cells.Add(GetCell(mdlReportedDircharge.Name, "text-left text-bold"));
            rowRD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(mdlReportedDircharge.DD), "text-right"));
            foreach (var a in mdlReportedDircharge.LstAttributes)
            {
                rowRD.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(a.Value), "text-right"));
            }

            if (mdlReportedDircharge.LstAttributes.Count == 2)
            {
                rowRD.Cells.Add(GetCell("", "text-right"));
            }
            else if (mdlReportedDircharge.LstAttributes.Count == 1)
            {
                rowRD.Cells.Add(GetCell("", "text-right"));
                rowRD.Cells.Add(GetCell("", "text-right"));
            }
            tblLG.Rows.Add(rowRD);
            #endregion

            #region Total Losses
            Dictionary<string, double?> losses = new Dictionary<string, double?>();

            foreach (var g in mdlReportedDircharge.LstAttributes)
            {
                losses[g.Key] = mdlReportedDircharge.LstAttributes[g.Key] - lst_TotalSubDivDischarge[g.Key]; //mdlTotalSubDivDschrg.LstAttributes[g.Key];
            }
            // Draw Total Reported Discharge Row
            TableRow rowLosses = new TableRow();
            TableRow rowLossesPrcntg = new TableRow();

            rowLosses.Cells.Add(GetCell("Losses", "text-left text-bold"));
            rowLossesPrcntg.Cells.Add(GetCell("% Losses", "text-left text-bold"));

            rowLosses.Cells.Add(GetCell("", "text-right"));
            rowLossesPrcntg.Cells.Add(GetCell("", "text-right"));

            int count = 0;
            double? TotalSubDivDschrg = 0, totalLosses = 0;

            foreach (var a in losses)
            {
                rowLosses.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(losses[a.Key]), "text-right"));

                double? divisional = lst_TotalSubDivDischarge[a.Key];//mdlTotalSubDivDschrg.LstAttributes[a.Key];
                double? loss = losses[a.Key];
                double? prcntgLoss = 0;

                if (divisional > 0)
                    prcntgLoss = (loss / divisional) * 100;

                rowLossesPrcntg.Cells.Add(GetCell(Utility.GetRoundOffValueOneDecimal(prcntgLoss), "text-right"));

                TotalSubDivDschrg = TotalSubDivDschrg + divisional;
                totalLosses = totalLosses + loss;
                count = count + 1;
            }

            if (losses.Count == 2)
            {
                rowLosses.Cells.Add(GetCell("", "text-right"));
                rowLossesPrcntg.Cells.Add(GetCell("", "text-right"));
            }
            else if (losses.Count == 1)
            {
                rowLosses.Cells.Add(GetCell("", "text-right"));
                rowLossesPrcntg.Cells.Add(GetCell("", "text-right"));

                rowLosses.Cells.Add(GetCell("", "text-right"));
                rowLossesPrcntg.Cells.Add(GetCell("", "text-right"));
            }
            tblLG.Rows.Add(rowLosses);
            tblLG.Rows.Add(rowLossesPrcntg);

            if (count > 0)
            {
                double? avgDschrg = TotalSubDivDschrg / count;

                {
                    double? avgLosses = totalLosses / count;
                    albl.Text = Utility.GetRoundOffValueOneDecimal(avgDschrg);
                    blbl.Text = Utility.GetRoundOffValueOneDecimal(avgLosses);
                    if (avgDschrg > 0)
                        clbl.Text = Utility.GetRoundOffValueOneDecimal((avgLosses / avgDschrg) * 100);
                }
            }


            #endregion

            #endregion
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
            //bool result = true;
            if (string.IsNullOrEmpty(ddlSeason.SelectedItem.Value))// || string.IsNullOrEmpty(ddlToMnth.SelectedItem.Value))
            {
                Master.ShowMessage("Select a season.", SiteMaster.MessageType.Error);
                return false;
            }

            //int from = Convert.ToInt32(ddlFrmMnth.SelectedItem.Value), to = Convert.ToInt32(ddlToMnth.SelectedItem.Value);

            //if (to < from)
            //{
            //    Master.ShowMessage("From month cannot be greater than To month.", SiteMaster.MessageType.Error);
            //    return false;
            //}

            //else if (to - from > 5)
            //{
            //    Master.ShowMessage("Month range cannot be more than 6 months.", SiteMaster.MessageType.Error);
            //    return false;
            //}
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