using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PublicWebSite
{
    public partial class ChannelDetail : System.Web.UI.Page
    {
        int channelID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsPostBack == false)
                {
                    txtDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd");

                    dvChannelDetail.Visible = false;
                    divNoResultFound.Visible = false;

                    channelID = Utility.GetNumericValueFromQueryString("Channel", 0);

                    if (channelID > 0)
                    {
                        hdnChannelID.Value = channelID.ToString();
                        PopulateChannelInformation();
                    }
                    else
                    {
                        divNoResultFound.Visible = true;
                        divNoResultFound.InnerText = "No channel Found";
                        dvChannelDetail.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
                divNoResultFound.Visible = true;
                //divNoResultFound.InnerText = "Please enter Complaint Number & Complain CellNumber both.";
                dvChannelDetail.Visible = false;
            }
        }

        private void PopulateChannelInformation()
        {
            try
            {
                DataSet DS = new ChannelBLL().GetChannelDetailInformation_ForPublicWebSite(channelID, null);

                string NotAvailable = "NA";

                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    string Zone = string.Empty;
                    string Circle = string.Empty;
                    string Division = string.Empty;
                    string SubDivision = string.Empty;
                    string Section = string.Empty;

                    foreach (DataRow drRow in DS.Tables[0].Rows)
                    {
                        if (drRow["Zone"].ToString().Trim() != "")
                        {
                            if (Zone == string.Empty)
                            {
                                Zone = drRow["Zone"].ToString().Trim() + ", ";
                            }
                            else
                            {
                                if (!Zone.Contains(drRow["Zone"].ToString().Trim()))
                                {
                                    Zone = Zone + drRow["Zone"].ToString().Trim() + ", ";
                                }
                            }
                        }

                        if (drRow["Circle"].ToString().Trim() != "")
                        {
                            if (Circle == string.Empty)
                            {
                                Circle = drRow["Circle"].ToString().Trim() + ", ";
                            }
                            else
                            {
                                if (!Circle.Contains(drRow["Circle"].ToString().Trim()))
                                {
                                    Circle = Circle + drRow["Circle"].ToString().Trim() + ", ";
                                }
                            }
                        }

                        if (drRow["Division"].ToString().Trim() != "")
                        {
                            if (Division == string.Empty)
                            {
                                Division = drRow["Division"].ToString().Trim() + ", ";
                            }
                            else
                            {
                                if (!Division.Contains(drRow["Division"].ToString().Trim()))
                                {
                                    Division = Division + drRow["Division"].ToString().Trim() + ", ";
                                }
                            }
                        }

                        if (drRow["SubDivision"].ToString().Trim() != "")
                        {
                            if (SubDivision == string.Empty)
                            {
                                SubDivision = drRow["SubDivision"].ToString().Trim() + ", ";
                            }
                            else
                            {
                                if (!SubDivision.Contains(drRow["SubDivision"].ToString().Trim()))
                                {
                                    SubDivision = SubDivision + drRow["SubDivision"].ToString().Trim() + ", ";
                                }
                            }
                        }

                        if (drRow["Section"].ToString().Trim() != "")
                        {
                            if (Section == string.Empty)
                            {
                                Section = drRow["Section"].ToString().Trim() + ", ";
                            }
                            else
                            {
                                if (!Section.Contains(drRow["Section"].ToString().Trim()))
                                {
                                    Section = Section + drRow["Section"].ToString().Trim() + ", ";
                                }
                            }
                        }
                    }

                    if (Zone != string.Empty)
                    {
                        lblZone.Text = Zone.Substring(0, Zone.Length - 2);
                    }
                    else
                    {
                        lblZone.Text = NotAvailable;
                    }

                    if (Circle != string.Empty)
                    {
                        lblCircle.Text = Circle.Substring(0, Circle.Length - 2);
                    }
                    else
                    {
                        lblCircle.Text = NotAvailable;
                    }

                    if (Division != string.Empty)
                    {
                        lblDivision.Text = Division.Substring(0, Division.Length - 2);
                    }
                    else
                    {
                        lblDivision.Text = NotAvailable;
                    }

                    if (SubDivision != string.Empty)
                    {
                        lblSubDivision.Text = SubDivision.Substring(0, SubDivision.Length - 2);
                    }
                    else
                    {
                        lblSubDivision.Text = NotAvailable;
                    }

                    if (Section != string.Empty)
                    {
                        lblSection.Text = Section.Substring(0, Section.Length - 2);
                    }
                    else
                    {
                        lblSection.Text = NotAvailable;
                    }

                    DataRow DR = DS.Tables[0].Rows[0];

                    if (DR["IMISCode"].ToString().Trim() != "")
                    {
                        lblIMISCode.Text = DR["IMISCode"].ToString().Trim();
                    }
                    else
                    {
                        lblIMISCode.Text = NotAvailable;
                    }

                    if (DR["ChannelName"].ToString().Trim() != "")
                    {
                        lblChannelName.Text = DR["ChannelName"].ToString().Trim();
                    }
                    else
                    {
                        lblChannelName.Text = NotAvailable;
                    }

                    if (DR["ParentChannel"].ToString().Trim() != "")
                    {
                        lblParentChannel.Text = DR["ParentChannel"].ToString().Trim();
                    }
                    else
                    {
                        lblParentChannel.Text = NotAvailable;
                    }

                    if (DR["ChannelType"].ToString().Trim() != "")
                    {
                        lblChannelType.Text = DR["ChannelType"].ToString().Trim();
                    }
                    else
                    {
                        lblChannelType.Text = NotAvailable;
                    }

                    if (DR["FlowType"].ToString().Trim() != "")
                    {
                        lblFlowType.Text = DR["FlowType"].ToString().Trim();
                    }
                    else
                    {
                        lblFlowType.Text = NotAvailable;
                    }

                    if (DR["TotalRDs"].ToString().Trim() != "")
                    {
                        lblTotalRD.Text = PMIU.WRMIS.AppBlocks.Calculations.GetRDText(Convert.ToInt64(DR["TotalRDs"].ToString().Trim())) + "  (ft)";
                    }
                    else
                    {
                        lblTotalRD.Text = NotAvailable;
                    }

                    if (DR["Distance"].ToString().Trim() != "")
                    {
                        lblDistance.Text = DR["Distance"].ToString().Trim() + "  (Miles)";
                    }
                    else
                    {
                        lblDistance.Text = NotAvailable;
                    }

                    if (DR["AuthHeadDischarge"].ToString().Trim() != "")
                    {
                        lblAuthHeadDischarge.Text = DR["AuthHeadDischarge"].ToString().Trim() + "  (Cusec)";
                    }
                    else
                    {
                        lblAuthHeadDischarge.Text = NotAvailable;
                    }

                    if (DR["AuthTailDischarge"].ToString().Trim() != "")
                    {
                        lblAuthTailDischarge.Text = DR["AuthTailDischarge"].ToString().Trim() + "  (Cusec)";
                    }
                    else
                    {
                        lblAuthTailDischarge.Text = NotAvailable;
                    }

                    if (DR["AuthorizedTailGauge"].ToString().Trim() != "")
                    {
                        lblAuthTailGuage.Text = DR["AuthorizedTailGauge"].ToString().Trim() + "  (ft)";
                    }
                    else
                    {
                        lblAuthTailGuage.Text = NotAvailable;
                    }

                    if (DR["GCAAcre"].ToString().Trim() != "")
                    {
                        lblGCA.Text = DR["GCAAcre"].ToString().Trim() + "  (Acre)";
                    }
                    else
                    {
                        lblGCA.Text = NotAvailable;
                    }

                    if (DR["CCAAcre"].ToString().Trim() != "")
                    {
                        lblCCA.Text = DR["CCAAcre"].ToString().Trim() + "  (Acre)";
                    }
                    else
                    {
                        lblCCA.Text = NotAvailable;
                    }

                    if (DR["MaxReadingDateTime"].ToString().Trim() != "")
                    {
                        txtDateTime.Value = DateTime.Parse(DR["MaxReadingDateTime"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    if (DR["CurrentHeadDischarge"].ToString().Trim() != "")
                    {
                        lblCurrHeadDis.Text = DR["CurrentHeadDischarge"].ToString().Trim() + "  (Cusec)";
                    }
                    else
                    {
                        lblCurrHeadDis.Text = NotAvailable;
                    }

                    if (DR["CurrentTailDischarge"].ToString().Trim() != "")
                    {
                        lblCurrTailDis.Text = DR["CurrentTailDischarge"].ToString().Trim() + "  (Cusec)";
                    }
                    else
                    {
                        lblCurrTailDis.Text = NotAvailable;
                    }

                    if (DR["CurrentTailGaugeValue"].ToString().Trim() != "")
                    {
                        lblCurrTailGauge.Text = DR["CurrentTailGaugeValue"].ToString().Trim() + "  (ft)";
                    }
                    else
                    {
                        lblCurrTailGauge.Text = NotAvailable;
                    }

                    if (DR["MaxReadingDateTime"].ToString().Trim() != "")
                    {
                        lblReadDate.Text = txtDateTime.Value = DateTime.Parse(DR["MaxReadingDateTime"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        lblReadDate.Text = NotAvailable;
                    }

                    if (DR["Remarks"].ToString().Trim() != "")
                    {
                        lblChnlRemrks.Text = DR["Remarks"].ToString().Trim();
                    }
                    else
                    {
                        lblChnlRemrks.Text = NotAvailable;
                    }



                    divNoResultFound.Visible = false;
                    dvChannelDetail.Visible = true;
                    //myCal.SelectedDate = DateTime.Parse(DR["MaxReadingDateTime"].ToString().Trim());
                    BindChart(DS);
                }
                else
                {
                    divNoResultFound.Visible = true;
                    dvChannelDetail.Visible = false;
                }

            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
                divNoResultFound.Visible = true;

                //divNoResultFound.InnerText = "Please enter Complaint Number & Complain CellNumber both.";
                dvChannelDetail.Visible = false;
            }
        }

        private void BindChart(DataSet DS)
        {
            //UpdatePanel1.Update();

            StringBuilder strScript = new StringBuilder();

            try
            {
                if (DS.Tables[1].Rows.Count > 0)
                {
                    dvLtScripts.InnerText = "";
                    chart_div.InnerText = "";

                    strScript.Append(@"//$(document).ready(function () {
                    function drawVisualization() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['DailyDate','Head Discharge','Tail Discharge','Indent'],");

                    //['DailyDate', 'Indent', 'HeadDischarge', 'TailDischarge'],");

                    foreach (DataRow row in DS.Tables[1].Rows)
                    {
                        string DailyDate = Utility.GetFormattedDate(Convert.ToDateTime(row["DailyDate"].ToString()));
                        //strScript.Append("['" + row["DailyDate"] + "'," + row["Indent"] + "," +
                        //    row["HeadDischarge"] + "," + row["TailDischarge"] + "],");
                        //strScript.Append("['" + row["DailyDate"] + "','" +
                        //    row["HeadDischarge"] + "','" + row["TailDischarge"] + "'],");
                        strScript.Append("['" + DailyDate + "'," +
                            float.Parse(row["HeadDischarge"].ToString()) + "," +
                            float.Parse(row["TailDischarge"].ToString()) + "," +
                            float.Parse(row["Indent"].ToString()) + "],");
                    }

                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");

                    strScript.Append("var options = { legend: {position: 'top'}, title : '', vAxis: {title: 'Discharge (Cusecs)'},  hAxis: {title: 'Recording Date'}, seriesType: 'bars', series: {4: {type: 'area'}} };");
                    strScript.Append("var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));  chart.draw(data, options);  $(document).ready(function () { $(document).scrollTop($('#scrollDiv').offset().top+250);}); } ;");

                    ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.GetType(), "scriptname", strScript.ToString(), true);
                    chart_div.Attributes.Add("style", "width: 990px; height: 600px; margin: auto; display: block;");

                    //HtmlGenericControl Include = new HtmlGenericControl("script");
                    //Include.Attributes.Add("type", "text/javascript");
                    //Include.InnerHtml = strScript.ToString();
                    //this.Page.Header.Controls.Add(Include);

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "scriptname", strScript.ToString(), true);

                    //strScript.Clear();
                    //strScript.Append(@"$(function () { 
                    //                    strScript.Append(@"function abcDatePicker() {
                    //                                                $('#datepicker12').datepicker({
                    //                                                    inline: true,
                    //                                                    sideBySide: true
                    //                                                });");

                    //                     strScript.Append(@"var yearStr, monthStr, dayStr;
                    //
                    //                                                if ($('#txtDateTime').val() != '')
                    //                                                {
                    //                                                    yearStr = new Date($('#txtDateTime').val().toString()).getFullYear();
                    //                                                    monthStr = new Date($('#txtDateTime').val().toString()).getMonth();
                    //                                                    dayStr = new Date($('#txtDateTime').val().toString()).getDate();
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    yearStr = new Date().getFullYear();
                    //                                                    monthStr = new Date().getMonth();
                    //                                                    dayStr = new Date().getDate();
                    //                                                }
                    //                                                                                              
                    //                                                $('#datepicker12').datepicker('update', new Date(yearStr, monthStr, dayStr));");

                    //                     strScript.Append(@"
                    //                                                $('#datepicker12').on('changeDate', function () {
                    //
                    //                                                    var currentDate1 = new Date($('#datepicker12').datepicker('getFormattedDate'));
                    //
                    //                                                    $('#txtDateTime').val(currentDate1.format('yyyy-MM-dd'));
                    //
                    //                                                    //$('#btnRefreshGraph').click();
                    //
                    //                                                    alert($('#txtDateTime').val());
                    //
                    //
                    //                                                    //__doPostBack('btnRefreshGraph','');
                    //
                    //                                                    return false;
                    //
                    //                                                });
                    //                                       ");
                    //                     strScript.Append("}");
                    //      });");


                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "scriptname", strScript.ToString(), true);

                }
                else
                {
                    dvLtScripts.InnerText = "";
                    Literal myLiteral = new Literal();

                    myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found.</span>";
                    myLiteral.ID = "ltScripts";
                    //  chart_div.Visible = false;
                    chart_div.Attributes.Add("style", "width: 660px; height: 400px; margin: auto; display: none;");
                    dvLtScripts.Controls.Add(myLiteral);
                }
            }
            catch (Exception ex)
            {
                dvLtScripts.InnerText = "";
                Literal myLiteral = new Literal();

                myLiteral.Text = "<br/><br/><span style='color:#FF0000;'>No data found</span>";
                myLiteral.ID = "ltScripts";
                dvLtScripts.Controls.Add(myLiteral);
                chart_div.Attributes.Add("style", "width: 660px; height: 400px; margin: auto; display: none;");
                //chart_div.Visible = false;
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
            finally
            {
                //dsChartData.Dispose();  
                strScript.Clear();
            }
        }

        protected void UpdateDischarges(DataSet _DS, string _SelectedDate)
        {
            try
            {
                string NotAvailable = "NA";
                if (_DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = _DS.Tables[0].Rows[0];

                    if (!string.IsNullOrEmpty(_SelectedDate))
                        lblReadDate.Text = DateTime.Parse(_SelectedDate).ToString("yyyy-MM-dd");
                    else
                        lblReadDate.Text = "-";

                    if (DR["CurrentHeadDischarge"].ToString().Trim() != "")
                        lblCurrHeadDis.Text = DR["CurrentHeadDischarge"].ToString().Trim() + "  (Cusec)";
                    else
                        lblCurrHeadDis.Text = NotAvailable;

                    if (DR["CurrentTailDischarge"].ToString().Trim() != "")
                        lblCurrTailDis.Text = DR["CurrentTailDischarge"].ToString().Trim() + "  (Cusec)";
                    else
                        lblCurrTailDis.Text = NotAvailable;

                    if (DR["CurrentTailGaugeValue"].ToString().Trim() != "")
                        lblCurrTailGauge.Text = DR["CurrentTailGaugeValue"].ToString().Trim() + "  (ft)";
                    else
                        lblCurrTailGauge.Text = NotAvailable;
                }
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void Date_Changed(object sender, EventArgs e)
        {
            try
            {
                DataSet DS = new ChannelBLL().GetChannelDetailInformation_ForPublicWebSite(Convert.ToInt64(hdnChannelID.Value), Convert.ToDateTime(txtDateTime.Value));// Convert.ToDateTime(myCal.SelectedDate.GetDateTimeFormats()[6]));
                BindChart(DS);
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void btnRefreshGraph_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet DS = new ChannelBLL().GetChannelDetailInformation_ForPublicWebSite(Convert.ToInt64(hdnChannelID.Value), Convert.ToDateTime(txtDateTime.Value));
                BindChart(DS);
                UpdateDischarges(DS, txtDateTime.Value);
                //PopulateChannelInformation();
            }
            catch (Exception ex)
            {
                new WRException(0, ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}