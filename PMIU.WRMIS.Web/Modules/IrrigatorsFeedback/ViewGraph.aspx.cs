using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.IrrigatorsFeedback
{
    public partial class ViewGraph : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //BindChart();
                    SetPageTitle();
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
//        private void BindChart()
//        {
//            hdnChannelID.Value = Convert.ToString(Session["CHANNELID"]);
//            DataSet DS = new ComplaintsManagementBLL().GetTailStatusLastTenDays(Convert.ToInt64(hdnChannelID.Value));
//            //DataSet DS = new ComplaintsManagementBLL().GetTailStatusLastTenDays(2416);

//            StringBuilder strScript = new StringBuilder();

//            try
//            {

//                if (DS.Tables[0].Rows.Count > 0)
//                {
//                    dvLtScripts.InnerText = "";
//                    chart_div.InnerText = "";

//                    strScript.Append(@"//$(document).ready(function () {
//                    function drawVisualization() {  var data = google.visualization.arrayToDataTable([  
//                                                    ['ReadingDate', 'Status'],");

//                    foreach (DataRow row in DS.Tables[0].Rows)
//                    {
//                        string DailyDate = "";
//                        if (row["ReadingDateTime"].ToString() != "")
//                            DailyDate = Utility.GetFormattedDateTimeJavaScript(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
//                        strScript.Append("['" + DailyDate + "','" + row["Status"] + "'],");
//                    }

//                    strScript.Remove(strScript.Length - 1, 1);
//                    strScript.Append("]);");
//                    strScript.Append(@"var view = new google.visualization.DataView(data);");
//                    //view.hideRows([0, 1, 2, 3]);");

//                    strScript.Append("var options = {bar: {groupWidth: '20%'}, legend: {position: 'none'}, title : '', vAxis: {title: 'Status'},  hAxis: {title: 'Reading Date', slantedText: true}, seriesType: 'bars' };");
//                    strScript.Append("var chart =new google.charts.Bar(document.getElementById('MainContent_chart_div'));chart.draw(view, google.charts.Bar.convertOptions(options));}");
//                    //strScript.Append("var chart =new google.visualization.ColumnChart(document.getElementById('MainContent_chart_div'));chart.draw(view, options);}");
//                    //                    strScript.Append(@"function drawVisualization() {     
//                    //                                            var data = new google.visualization.DataTable();
//                    //                                            data.addColumn('string', 'ReadingDate');
//                    //                                            data.addColumn('string', 'Status');
//                    //                                            data.addRows([");

//                    //                    foreach (DataRow row in DS.Tables[0].Rows)
//                    //                    {
//                    //                       string DailyDate = Utility.GetFormattedDate(Convert.ToDateTime(row["ReadingDateTime"].ToString()));
//                    //                       strScript.Append("['" + DailyDate + "','" + row["Status"] + "'],");
//                    //                    }
//                    //                    strScript.Append("]);");

//                    //                    strScript.Append("var options = {bar: {groupWidth: '20%'}, legend: {position: 'none'}, title : '', vAxis: {title: 'Status'},  hAxis: {title: 'ReadingDate'}, seriesType: 'bars', series: {3: {type: 'area'}} };");
//                    //                    strScript.Append("var chart = new google.charts.Bar(document.getElementById('MainContent_chart_div'));");
//                    //                    strScript.Append("chart.draw(data, google.charts.Bar.convertOptions(options));}");



//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname", strScript.ToString(), true);

//                }
//                else
//                {
//                    dvLtScripts.InnerText = "";
//                    Literal myLiteral = new Literal();

//                    myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found.</span>";
//                    myLiteral.ID = "ltScripts";
//                    chart_div.Visible = false;
//                    dvLtScripts.Controls.Add(myLiteral);
//                }
//            }
//            catch (Exception ex)
//            {
//                dvLtScripts.InnerText = "";
//                Literal myLiteral = new Literal();

//                myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found</span>";
//                myLiteral.ID = "ltScripts";
//                dvLtScripts.Controls.Add(myLiteral);
//                chart_div.Visible = false;
//                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
//            }
//            finally
//            {
//                strScript.Clear();
//            }
//        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddIrrigatorFeedback);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}