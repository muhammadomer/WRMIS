using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using PMIU.WRMIS.Web.Modules.FEWS.FEWSClasses;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System.Globalization;

namespace PMIU.WRMIS.Web.Modules.FEWS
{
    public partial class FloodForecast : BasePage
    {
        int firstRow;
        string LastRiverName;
        string[] HeaderValues;
        DataTable Table = new DataTable();
        RoleRightBits bits;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (!UserRoleHasRights())
                //{
                //    Response.Redirect(Request.UrlReferrer.ToString());              
                //    return;
                //}

                if (!IsPostBack)
                {
                    //if (ValidateDate())
                    //{
                    SetPageTitle();
                    DataBindToGrid();
                    //}
                    //else
                    //{
                    //    Message.InnerText = "Results will be shown in Flood Season i.e., 15th June to 15th October";
                    //    SeasonMessage.Visible = true;
                    //    PageHeader.Visible = false;
                    //    main.Visible = false;
                    //}

                    //DataBindToGrid();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodEarlyWarningSystem);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private bool UserRoleHasRights()
        {
            try
            {

                bits = new RoleRightBits();
                bits = (RoleRightBits)Session["RoleRights"];
                System.Web.UI.WebControls.Image img_print = (System.Web.UI.WebControls.Image)Master.FindControl("imgPrint");
                System.Web.UI.WebControls.Image imgPDF = (System.Web.UI.WebControls.Image)Master.FindControl("imgExportPDF");

                if (bits != null)
                {
                    if (bits.PrintBit != true)
                    {
                        img_print.Enabled = false;
                        img_print.CssClass = "btnOpacity";
                    }
                    if (bits.ExportBit != true)
                    {
                        imgPDF.Enabled = false;
                        imgPDF.CssClass = "btnOpacity";
                    }


                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
        }

        private void DataBindToGrid()
        {
            try
            {
                if (GetFilePath() != string.Empty)
                {

                    string[] str_HtmlData = ReadHtmlFile(GetFilePath()).ToString().Split('~');
                    List<string> Lst_Data = StripHTMLTags(str_HtmlData[0].ToString());
                    List<string> Lst_Date = StripDateTags(str_HtmlData[1].ToString());
                    Session["Date"] = Lst_Date;
                    TBDataTimeOfIssue.Text = Lst_Date.ElementAt(1).ToString();
                    string str = Lst_Date.ElementAt(1).ToString();
                    string[] arr = str.Split(',');
                    string[] arr1 = arr[0].ToString().Split('.');

                    //string newDate = arr1[1] + "/" + arr1[0] + "/" + arr1[2] + arr[1].Replace("(PST)", "").TrimEnd() + ":00 PM";
                    string newDate = arr1[1] + "/" + arr1[0] + "/" + arr1[2];
                    //newDate = "5/1/2008 8:30:52 AM";
                    //newDate = "7/27/2010 12:00:00 PM";
                    DateTime parsedDateTime;
                    parsedDateTime = DateTime.Parse(newDate, CultureInfo.InvariantCulture);
                    TBDataTimeOfIssue.Text = parsedDateTime.ToString("dd-MMM-yyyy") + " ," + arr[1].ToString();
                    //TBDataTimeOfIssue.Text = Convert.ToDateTime(newDate).ToString("dd-MMM-yyyy") + " ," + arr[1].ToString();
                    //TBDataTimeOfIssue.Text = Convert.ToDateTime(newDate).ToString("dd-MMM-yyyy") + " ," + arr[1].ToString();


                    
                    //if (DateTime.TryParseExact(newDate, "dd-MM-yyyy",
                    //                           CultureInfo.InvariantCulture,
                    //                           DateTimeStyles.AssumeUniversal,
                    //                           out parsedDateTime))
                    ////if ()
                    //{
                    //    parsedDateTime = DateTime.ParseExact(newDate, "dd-MM-yyyy",CultureInfo.InvariantCulture);
                    //    TBDataTimeOfIssue.Text = parsedDateTime.ToString("dd-MMM-yyyy") + " ," + arr[1].ToString();
                    //     //Value will now be midnight UTC on the given date
                    //}

                    //string dateString = "5/1/2008 8:30:52 AM";
                    //DateTime date1 = DateTime.Parse(dateString,
                    //                          System.Globalization.CultureInfo.InvariantCulture); 
                    //string repl = arr[0].Replace(".", "/");
                    //string[] date = repl.Split('/');
                    //  int dtMonth = DateTime.ParseExact(date[0], "MM", System.Globalization.CultureInfo.CurrentCulture).Month;

                    //   DateTime dt = DateTime.ParseExact(repl, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture);
                    //string strDateTime = "06-06-2015 PST"; //Lst_Date.ElementAt(1).ToString();

                    //DateTime _date = DateTime.ParseExact("2013-01-14 21:09:06 PST".Replace("PST", "+2"), "dd/MM/yyyy mm:ss z", System.Globalization.CultureInfo.CurrentUICulture);//.CurrentUICulture); CreateSpecificCulture("ur-PK")

                    //  System.Globalization.CultureInfo culture = ); //nl-BE
                    //  DateTime dt3 = DateTime.ParseExact("24-okt-08 21:09:06 PST".Replace("PST", "+02:00"), "dd-MMM-yy HH:mm:ss zzz", System.Globalization.CultureInfo.);

                    bool IsHeader = true;
                    foreach (var item in Lst_Data)
                    {
                        if (!IsHeader)
                            AddRow(item);
                        else
                        {
                            AddColumns(item);
                            IsHeader = false;
                        }
                    }
                    GridViewFloodForecast.DataSource = Table;
                    GridViewFloodForecast.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private string GetFilePath()
        {
            return Utility.GetImagePath("OverviewBU.html");
        }

        public StringBuilder ReadHtmlFile(string htmlFileNameWithPath)
        {
            StringBuilder storeContent = new StringBuilder();
            StringBuilder StoreContent_Tbl = new StringBuilder();
            int tblCount = 0;
            bool IsContain = false;
            bool IsContain_Tbl = false;
            try
            {
                using (System.IO.StreamReader htmlReader = new System.IO.StreamReader(htmlFileNameWithPath))
                {
                    string lineStr;

                    while ((lineStr = htmlReader.ReadLine()) != null)
                    {
                        if (lineStr.Contains("<table"))
                            tblCount++;

                        if (lineStr.Contains("<table") && tblCount == 3)
                        {
                            // storeContent.Append(lineStr);
                            IsContain_Tbl = true;
                        }

                        if (IsContain_Tbl)
                        {
                            StoreContent_Tbl.Append(lineStr);
                            if (lineStr.Contains("</table>"))
                                IsContain_Tbl = false;
                        }

                        if (lineStr.Contains("<table") && tblCount == 5)
                        {
                            // storeContent.Append(lineStr);
                            IsContain = true;
                        }

                        if (IsContain)
                        {
                            storeContent.Append(lineStr);
                            if (lineStr.Contains("</table>"))
                                IsContain = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

            return storeContent.Append("~").Append(StoreContent_Tbl);
        }

        public List<string> StripHTMLTags(string str)
        {
            MatchCollection m1 = Regex.Matches(str, @"(<\s*a[^>]*>(.*?)</a>)", RegexOptions.Singleline);
            string value = "";
            int ColCount = 0;

            List<string> LstRows = new List<string>();
            foreach (Match m in m1)
            {
                ColCount++;
                string cell = m.Groups[1].Value;
                Match match = Regex.Match(cell, @"<\s*a[^>]*>(.*?)</a");
                if (match.Success)
                {
                    value += match.Groups[1].Value + ";";
                    if (ColCount == 6)
                    {
                        LstRows.Add(value);
                        ColCount = 0;
                        value = string.Empty;
                    }
                }
            }

            return LstRows;
        }

        private List<string> StripDateTags(string str)
        {
            MatchCollection m1 = Regex.Matches(str, @"(<\s*FONT[^>]*>(.*?)</FONT>)", RegexOptions.Singleline);
            string value = "";
            int ColCount = 0;

            List<string> LstRows = new List<string>();
            foreach (Match m in m1)
            {
                ColCount++;
                string cell = m.Groups[1].Value;
                Match match = Regex.Match(cell, @"<\s*FONT[^>]*>(.*?)</FONT");
                if (match.Success)
                {
                    value = match.Groups[1].Value;// +";";
                    if (ColCount == 2)
                    {
                        LstRows.Add(value);
                        ColCount = 0;
                        value = string.Empty;
                    }
                }
            }
            return LstRows;
        }

        private void AddRow(string RowData)
        {
            DataRow Row;
            string[] values = RowData.Split(';');
            if (values[0] != "")
            {
                LastRiverName = values[0];
            }
            if (values[0] == "")
            {
                values[0] = LastRiverName;
            }

            Row = Table.NewRow();
            Row["RIVERS"] = values[0];
            Row["Stations"] = values[1];
            Row["Design Capacity"] = values[2];
            Row["Forecast for next 30 days"] = values[3];
            Row["Forecast for next 30 dayss"] = values[4];
            Row["Danger Level"] = values[5];


            Table.Rows.Add(Row);
        }

        private void AddColumns(string ColHeader)
        {
            HeaderValues = ColHeader.Split(';');
            Table.Columns.Add("RIVERS", typeof(string));
            Table.Columns.Add("Stations", typeof(string));
            Table.Columns.Add("Design Capacity", typeof(string));
            Table.Columns.Add("Forecast for next 30 days", typeof(string));
            Table.Columns.Add("Forecast for next 30 dayss", typeof(string));
            Table.Columns.Add("Danger Level", typeof(string));
        }

        protected void GridViewFloodForecast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = ((DataRowView)e.Row.DataItem);

                    Label lblForecas = (Label)e.Row.FindControl("lblForecas");
                    Label lblForecast_1 = (Label)e.Row.FindControl("lblForecast_1");
                    Label lblDangerLevel = (Label)e.Row.FindControl("lblDangerLevel");

                    var thisForecas = lblForecas.Text;
                    var thisDangerLevel = lblDangerLevel.Text.Replace(",", "");

                    string str = lblForecast_1.Text;
                    string[] arr = str.Split(',');
                    string[] arr1 = arr[0].ToString().Split('.');
                    string newDate = arr1[1] + "/" + arr1[0] + "/" + arr1[2];
                    //newDate = "5/1/2008 8:30:52 AM";
                    //newDate = "7/27/2010 12:00:00 PM";
                    DateTime parsedDateTime;
                    parsedDateTime = DateTime.Parse(newDate, CultureInfo.InvariantCulture);
                    lblForecast_1.Text = parsedDateTime.ToString("dd-MMM-yyyy") + " " + Utility.GetFormattedTime(Convert.ToDateTime(arr[1].ToString()));

                    //lblForecast_1.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblForecast_1.Text)) + " " + Utility.GetFormattedTime(Convert.ToDateTime(lblForecast_1.Text));
                    lblForecas.Text = String.Format("{0:###,0}", Convert.ToInt32(lblForecas.Text));

                    if (thisForecas != "" && thisDangerLevel != "")
                    {

                        if (Convert.ToInt64(thisForecas) > Convert.ToInt64(thisDangerLevel))
                        {
                            lblForecas.Style.Add("color", "red");
                            lblForecas.Style.Add("font-weight", "bold");
                        }
                        else if (Convert.ToInt64(thisDangerLevel) - Convert.ToInt64(thisForecas) < 50000)
                        {
                            lblForecas.Style.Add("color", "#f49f04");
                            lblForecas.Style.Add("font-weight", "bold");
                        }

                    }

                    //e.Row.Cells[0].BackColor = System.Drawing.Color.LightGray;
                    if (drv["RIVERS"].ToString() == LastRiverName)
                    {

                        if (GridViewFloodForecast.Rows[firstRow].Cells[0].RowSpan == 0)

                            GridViewFloodForecast.Rows[firstRow].Cells[0].RowSpan = 2;
                        else
                            GridViewFloodForecast.Rows[firstRow].Cells[0].RowSpan += 1;

                        e.Row.Cells.RemoveAt(0);
                    }
                    else
                    {
                        e.Row.VerticalAlign = VerticalAlign.Top;
                        LastRiverName = drv["RIVERS"].ToString();
                        firstRow = e.Row.RowIndex;
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        private bool ValidateDate()
        {
            if (!WithinFloodSeason(DateTime.Now.Date))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool WithinFloodSeason(DateTime date)
        {
            int monthNumber = date.Month;
            if (monthNumber >= 6 && monthNumber <= 10)
            {
                int dayNumber = date.Day;
                if (monthNumber == 6)
                {
                    if (dayNumber >= 15)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (monthNumber == 10)
                {
                    if (dayNumber <= 15)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //public class FEWS
        //{
        //    public DateTime Date { get; set; }
        //    public TimeSpan Time { get; set; }
        //    public Decimal Value { get; set; }
        //    public int Flag { get; set; }
        //}
    }
}


