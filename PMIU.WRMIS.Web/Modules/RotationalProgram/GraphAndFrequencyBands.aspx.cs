






using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.RotaionalProgram;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.RotationalProgram
{
    public partial class GraphAndFrequencyBands : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetTitle();
            if (!string.IsNullOrEmpty(Request.QueryString["RP"]))
            {
                BindChart(Convert.ToInt64((Request.QueryString["RP"])));
                BindFrequencyTable(Convert.ToInt64((Request.QueryString["RP"])));
                lblDiffDPR.Text = "Difference Between Max DPR and Min DPR = " + Convert.ToString(new RotationalProgramBLL().GetMaxMinDPR(Convert.ToInt64((Request.QueryString["RP"]))));
            }
            else
            {
                dvMainChart.InnerText = "";
                Literal myLiteral = new Literal();

                myLiteral.Text = "<br/><br/><span style='color:#FF0000; margin-left: 30px;'>No data found.</span>";
                myLiteral.ID = "ltScripts";
                dvChart.Visible = false;
                dvMainChart.Controls.Add(myLiteral);

                gvFreqTable.DataSource = null;
                gvFreqTable.DataBind();

                lblDiffDPR.Text = "Difference Between Max DPR and Min DPR = -";
            }
        }


        public void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddRotationalPlan);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void BindChart(long _RPID)
        {
            List<RP_GetAvgDPRChannelName_Result> Result = new RotationalProgramBLL().GetGraphData(_RPID);

            System.Text.StringBuilder strScript = new System.Text.StringBuilder();
            try
            {
                if (Result != null && Result.Count() > 0)
                {
                    dvMainChart.InnerText = "";
                    dvChart.InnerText = "";

                    strScript.Append(@"//$(document).ready(function () {
                    function drawVisualization() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['Channel', 'DPR'],");

                    foreach (RP_GetAvgDPRChannelName_Result res in Result)
                        strScript.Append("['" + res.Channel + "'," + res.AvgDPR + "],");

                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");

                    strScript.Append("var options = { legend: {position: 'top'}, title : '', vAxis: {title: 'Average DPR(%)'},  hAxis: {title: 'Channels'}, seriesType: 'bars', series: {2: {type: 'line'}} };");
                    strScript.Append("var chart = new google.visualization.ComboChart(document.getElementById('MainContent_dvChart'));  chart.draw(data, options); $(document).ready(function () { $(document).scrollTop($('#scrollDiv').offset().top+250);}); } ;");
                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "scriptname", strScript.ToString(), true);
                }
                else
                {
                    dvMainChart.InnerText = "";
                    Literal myLiteral = new Literal();

                    myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found.</span>";
                    myLiteral.ID = "ltScripts";
                    dvChart.Visible = false;
                    dvMainChart.Controls.Add(myLiteral);
                }
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
            finally
            {
                strScript.Clear();
            }
        }

        public void BindFrequencyTable(long _RPID)
        {
            try
            {
                gvFreqTable.DataSource = new RotationalProgramBLL().GetFrequencyTable(_RPID);
                gvFreqTable.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}