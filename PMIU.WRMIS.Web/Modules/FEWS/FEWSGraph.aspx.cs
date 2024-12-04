using System;
using System.Collections.Generic;
using System.Linq;
//sing FFC.BLL;
//using FFC.Model;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Xml;
using System.Configuration;
//using FFC.Web.App_Code;
using PMIU.WRMIS.Web.Modules.FEWS.FEWSClasses;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.FEWS;
using PMIU.WRMIS.Common;
using System.Data;
using System.Text;
using PMIU.WRMIS.Web.Common;
using System.Globalization;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.Web.Modules.FEWS
{
    public partial class WebForm4 : BasePage
    {

        //  List<FEWS_XML> LstFewsGraphData;
        string LocationID;
        string RiverName;
        List<FewsXmlData> LstFewsXmlData = new List<FewsXmlData>();
        RoleRightBits bits;
        protected void Page_Load(object sender, EventArgs e)
        {
            // UserRoleHasRights();

            SetPageTitle();

            if (GetFilePath() != string.Empty)
            {
                XmlDocument xdocument = new XmlDocument();
                xdocument.Load(GetFilePath());
                GetDataFromXMl(xdocument);
            }

            if (!IsPostBack)
            {
                DateBindToControls();
                GenerateAllGraph();
            }
            else
            {
                GenerateAllGraph();
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodEarlyWarningSystem);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        //private void UserRoleHasRights()
        //{
        //    System.Web.UI.WebControls.Image img_print = (System.Web.UI.WebControls.Image)Master.FindControl("imgPrint");
        //    System.Web.UI.WebControls.Image imgPDF = (System.Web.UI.WebControls.Image)Master.FindControl("imgExportPDF");

        //    img_print.Enabled = false;
        //    img_print.CssClass = "btnOpacity";

        //    imgPDF.Enabled = false;
        //    imgPDF.CssClass = "btnOpacity";

        //}

        private string GetFilePath()
        {
            return Utility.GetImagePath("ReportBU.xml");
        }

        private void GetDataFromXMl(XmlDocument xdocument)
        {
            FewsXmlData MdlFewsXmlData;
            LstFewsXmlData = new List<FewsXmlData>();
            XmlNodeList lsst = xdocument.GetElementsByTagName("series");

            for (int i = 0; i < lsst.Count; i++)
            {
                XmlNodeList LstChildNode = lsst[i].ChildNodes;
                // MdlFewsXmlData = new FewsXmlData();

                string LocationID = string.Empty;
                string ParameterID = string.Empty;
                string StationName = string.Empty;


                for (int j = 0; j < LstChildNode.Count; j++)
                {
                    MdlFewsXmlData = new FewsXmlData();
                    if (j == 0)
                    {
                        var LstHeader = LstChildNode[j].ChildNodes;
                        LocationID = LstHeader[1].InnerText;
                        ParameterID = LstHeader[2].InnerText;
                        StationName = LstHeader[7].InnerText;
                    }
                    else
                    {
                        if (LstChildNode[j].Attributes.Item(2).InnerText != "NaN")
                        {
                            MdlFewsXmlData.locationId = LocationID;
                            MdlFewsXmlData.parameterId = ParameterID;
                            MdlFewsXmlData.stationName = StationName;
                            MdlFewsXmlData.date = DateTime.Parse(LstChildNode[j].Attributes.Item(0).InnerText + " " + LstChildNode[j].Attributes.Item(1).InnerText, CultureInfo.InvariantCulture); //Convert.ToDateTime(LstChildNode[j].Attributes.Item(0).InnerText) + TimeSpan.Parse(LstChildNode[j].Attributes.Item(1).InnerText);
                            MdlFewsXmlData.time = TimeSpan.Parse(LstChildNode[j].Attributes.Item(1).InnerText);
                            // MdlFewsXmlData.date = Convert.ToDateTime(LstChildNode[j].Attributes.Item(0).InnerText) + MdlFewsXmlData.time;
                            MdlFewsXmlData.value = LstChildNode[j].Attributes.Item(2).InnerText;
                            MdlFewsXmlData.flag = Convert.ToInt32(LstChildNode[j].Attributes.Item(3).InnerText);
                            LstFewsXmlData.Add(MdlFewsXmlData);
                        }
                    }
                }
            }
        }

        private void DateBindToControls()
        {
            try
            {
                txtDataTimeOfIssues.Value = ((List<string>)Session["Date"]).ElementAt(0).ToString();
                string[] datearr = txtDataTimeOfIssues.Value.Split(',');
                string[] datearr1 = datearr[0].ToString().Split('.');

                string newDate = datearr1[1] + "/" + datearr1[0] + "/" + datearr1[2];
                //newDate = "5/1/2008 8:30:52 AM";
                //newDate = "7/27/2010 12:00:00 PM";
                DateTime parsedDateTime;
                parsedDateTime = DateTime.Parse(newDate, CultureInfo.InvariantCulture);
                txtDataTimeOfIssue.Text = parsedDateTime.ToString("dd-MMM-yyyy") + " ," + datearr[1].ToString();

                //txtDataTimeOfIssue.Text = Convert.ToDateTime(datearr1[0] + "/" + datearr1[1] + "/" + datearr1[2]).ToString("dd-MMM-yyyy") + " ," + datearr[1].ToString();
                txtTimes.Value = ((List<string>)Session["Date"]).ElementAt(1).ToString();
                string[] datearr2 = txtTimes.Value.Split(',');
                string[] datearr3 = datearr2[0].ToString().Split('.');

                newDate = datearr3[1] + "/" + datearr3[0] + "/" + datearr3[2];
                //newDate = "5/1/2008 8:30:52 AM";
                //newDate = "7/27/2010 12:00:00 PM";
                parsedDateTime = DateTime.Parse(newDate, CultureInfo.InvariantCulture);
                txtTime.Text = parsedDateTime.ToString("dd-MMM-yyyy") + " ," + datearr2[1].ToString();

                //txtTime.Text = Convert.ToDateTime(datearr3[0] + "/" + datearr3[1] + "/" + datearr3[2]).ToString("dd-MMM-yyyy") + " ," + datearr2[1].ToString();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GenerateAllGraph()
        {
            try
            {
                //if (ValidateDate())
                //{
                GetRiverAndLocationID();
                if (IsDataExist(LocationID, "Q.simulated.forecast", "Q.simulated.historical", "Q.obs") > 0)
                {
                    //forecastHeader.InnerText = "Forecast for " + LocationID + " DownStream";
                    GenerateDischargewGraph(LocationID, "Q.simulated.forecast", "Q.simulated.historical", "Q.obs");

                }
                else
                {
                    Cusecsdiv.Visible = false;
                }

                if (IsDataExist(LocationID, "H.simulated.forecast", "H.simulated.historical", "H.obs") > 0)
                {
                    GenerateFeetGraph(LocationID, "H.simulated.forecast", "H.simulated.historical", "H.obs");
                }
                else
                {
                    Feetdiv.Visible = false;
                }

                if (Cusecsdiv.Visible == false && Feetdiv.Visible == false)
                {
                    // main.Visible = false;
                    // forecastHeader.Visible = false;
                    //Message.InnerText = "Data does not exist for selected station";
                    Master.ShowMessage("Data does not exist for selected station", PMIU.WRMIS.Web.SiteMaster.MessageType.Error);
                    //SeasonMessage.Visible = true;
                }
                //  }

                //else
                //{
                //    main.Visible = false;
                //    forecastHeader.Visible = false;
                //    Feetdiv.Visible = false;
                //    Cusecsdiv.Visible = false;
                //Message.InnerText = "Results will be shown in Flood Season i.e., 15th June to 15th October";
                //  Message.Visible = true;
                //}

            }
            catch (Exception)
            {
                throw;
            }

        }
        private void GetRiverAndLocationID()
        {
            LocationID = Convert.ToString(Request.QueryString["Stations"]);
            RiverName = Convert.ToString(Request.QueryString["RIVERS"]);
        }

        private void GenerateDischargewGraph(string _LocationID, string _SimulatedForecast, string _SimulatedHistorycal, string _Obs)
        {

            List<DateTime?> lstDate_Forecast = GetRangeOfDate(_LocationID, _SimulatedForecast);
            List<string> LstValues_Forecast = GetRangeOfValue(_LocationID, _SimulatedForecast);

            DateTime[] xAxis_Values_Forecast = new DateTime[lstDate_Forecast.Count];
            Decimal[] yAxis_Values_Forecast = new Decimal[LstValues_Forecast.Count];

            //string[] yAxis_Values_Forecast = new string[LstValues_Forecast.Count];
            for (int i = 0; i < lstDate_Forecast.Count; i++)
            {
                xAxis_Values_Forecast[i] = ExtractDateTime(lstDate_Forecast[i].ToString());
                //xAxis_Values_Forecast[i] = DateTime.Parse(lstDate_Forecast[i].ToString(), CultureInfo.InvariantCulture);
                yAxis_Values_Forecast[i] = Convert.ToDecimal(LstValues_Forecast[i]);
                //yAxis_Values_Forecast[i] = Math.Round(Convert.ToDecimal(LstValues_Forecast[i]), 2).ToString("##,0.00"); 
            }

            List<DateTime?> lstDate_Historycal = GetRangeOfDate(_LocationID, _SimulatedHistorycal);
            List<string> LstValues_Historycal = GetRangeOfValue(_LocationID, _SimulatedHistorycal);

            DateTime[] xAxis_Values_Historycal = new DateTime[lstDate_Historycal.Count];
            Decimal[] yAxis_Values_Historycal = new Decimal[LstValues_Historycal.Count];
            //string[] yAxis_Values_Historycal = new string[LstValues_Historycal.Count];
            for (int i = 0; i < lstDate_Historycal.Count; i++)
            {
                xAxis_Values_Historycal[i] = ExtractDateTime(lstDate_Historycal[i].ToString());
                //xAxis_Values_Historycal[i] = DateTime.Parse(lstDate_Historycal[i].ToString(), CultureInfo.InvariantCulture);//(Convert.ToDateTime(lstDate_Historycal[i]));
                yAxis_Values_Historycal[i] = Convert.ToDecimal(LstValues_Historycal[i]);
                //yAxis_Values_Historycal[i] = Math.Round(Convert.ToDecimal(LstValues_Historycal[i]), 2).ToString("##,0.00");
            }

            List<DateTime?> lstDate_Obs = GetRangeOfDate(_LocationID, _Obs);
            List<string> LstValues_Obs = GetRangeOfValue(_LocationID, _Obs);

            DateTime[] xAxis_Values_Obs = new DateTime[lstDate_Obs.Count];
            Decimal[] yAxis_Values_Obs = new Decimal[LstValues_Obs.Count];
            //string[] yAxis_Values_Obs = new string[LstValues_Obs.Count];
            for (int i = 0; i < lstDate_Obs.Count; i++)
            {
                xAxis_Values_Obs[i] = ExtractDateTime(lstDate_Obs[i].ToString());
                //xAxis_Values_Obs[i] = DateTime.Parse(lstDate_Obs[i].ToString(), CultureInfo.InvariantCulture); //(Convert.ToDateTime(lstDate_Obs[i]));//.ToShortDateString();
                yAxis_Values_Obs[i] = Convert.ToDecimal(LstValues_Obs[i]);
                //yAxis_Values_Obs[i] = Math.Round(Convert.ToDecimal(LstValues_Obs[i]), 2).ToString("##,0.00");
            }

            FewsGraphcusecs.Titles["TitleCusecs"].Text = LocationID + " Downstream Discharge (cusecs)";
            FewsGraphcusecs.Titles["TitleCusecs"].Font = new System.Drawing.Font("Open Sans", 16, System.Drawing.FontStyle.Regular);
            //FewsGraphcusecs.Titles["TitleCusecs"].Alignment = ContentAlignment.TopLeft;


            FewsGraphcusecs.Series["Q.Forecast"].Points.DataBindXY(xAxis_Values_Forecast, yAxis_Values_Forecast);
            FewsGraphcusecs.Series["Q.Historical"].Points.DataBindXY(xAxis_Values_Historycal, yAxis_Values_Historycal);//xAxis_Values_Historycal,
            FewsGraphcusecs.Series["Q.Obs"].Points.DataBindXY(xAxis_Values_Obs, yAxis_Values_Obs);

            //FewsGraphcusecs.Series["Q.Forecast"].ToolTip = Convert.ToDecimal("#VALY").ToString("##,0.00");// string.Format("#VALX".ToString(), "{0:F2}");// Convert.ToDecimal("#VALX", 2).ToString("##,0.00");// "#VALX  | #VALY cusecs"; //date.ToShortDateString();#VALX | #VALY cusecs<%=Math.Round(Convert.ToDecimal({0}, 2).ToString("##,0.00")%>




            //FewsGraphcusecs.BackColor = Color.Coral;

            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.IsStartedFromZero = false;
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;

            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            FewsGraphcusecs.Legends.Add("Q.Forecast");
            FewsGraphcusecs.Legends.Add("Q.Historical");
            FewsGraphcusecs.Legends.Add("Q.Obs");

            FewsGraphcusecs.Legends[0].LegendStyle = LegendStyle.Table;
            FewsGraphcusecs.Legends[0].TableStyle = LegendTableStyle.Wide;
            FewsGraphcusecs.Legends[0].Docking = Docking.Bottom;
            FewsGraphcusecs.Legends[0].Alignment = StringAlignment.Center;
            FewsGraphcusecs.Legends[0].Font = new System.Drawing.Font("Open Sans", 10, System.Drawing.FontStyle.Regular);
            FewsGraphcusecs.Legends[0].Title = "";
            FewsGraphcusecs.Legends[0].TitleForeColor = Color.Blue;

            FewsGraphcusecs.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Open Sans", 11, System.Drawing.FontStyle.Bold);
            FewsGraphcusecs.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Open Sans", 11, System.Drawing.FontStyle.Bold);

            FewsGraphcusecs.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Open Sans", 09, System.Drawing.FontStyle.Regular);
            FewsGraphcusecs.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Open Sans", 09, System.Drawing.FontStyle.Regular);

            FewsGraphcusecs.Series["Q.Forecast"].XValueType = ChartValueType.DateTime;
            FewsGraphcusecs.Series["Q.Historical"].XValueType = ChartValueType.DateTime;
            FewsGraphcusecs.Series["Q.Obs"].XValueType = ChartValueType.DateTime;

            //FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MMM-yyyy\nddd"; //hh:mm"; //"MM.dd.yyyy hh:mm\nddd";
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MMM-yyyy"; //hh:mm"; //"MM.dd.yyyy hh:mm\nddd";
            //FewsGraphcusecs.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
            FewsGraphcusecs.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "##,0";

            var xAxis_Forecast = xAxis_Values_Forecast;
            List<Graph> lSTGraph = new List<Graph>();

            for (int i = 0; i < xAxis_Values_Forecast.Length; i++)
            {
                Graph g = new Graph();
                g.date = xAxis_Values_Forecast[i];
                g.Forecast = yAxis_Values_Forecast[i];
                //g.ForecastFormatted = Math.Round(Convert.ToDecimal(yAxis_Values_Forecast[i]), 2).ToString("##,0.00");
                if (_SimulatedHistorycal.Length > i)
                {
                    g.Historical = _SimulatedHistorycal[i];
                }
                if (_Obs.Length > i)
                {
                    g.Obs = _Obs[i];
                }
                lSTGraph.Add(g);
            }
            getgraph(lSTGraph);


            string[] datearr = txtDataTimeOfIssues.Value.Split(',');
            datearr = datearr[0].ToString().Split('.');

            string str = datearr[1] + "/" + datearr[0] + "/" + datearr[2];
            DateTime date = DateTime.Parse(str, CultureInfo.InvariantCulture);// Convert.ToDateTime(str);

            StripLine stripLine = new StripLine();
            stripLine.BorderColor = Color.Red;
            stripLine.IntervalOffset = date.ToOADate();
            stripLine.ToolTip = date.ToShortDateString();
            FewsGraphcusecs.ChartAreas[0].AxisX.StripLines.Add(stripLine);

            FW_Ref_Classifications mdlLimits = new FEWSBLL().GetFlowStatus(RiverName, LocationID);
            if (mdlLimits != null)
            {
                double limitToDraw = 0;
                limitToDraw = mdlLimits.Low == null ? 0 : Convert.ToDouble(mdlLimits.Low.ToString());
                // AddStripLine(limitToDraw, ColorTranslator.FromHtml("#66FFFF"), "Low Flood");
                AddStripLine(limitToDraw, ConfigurationManager.AppSettings["low"].ToString(), "Low Flood");

                limitToDraw = mdlLimits.Medium == null ? 0 : Convert.ToDouble(mdlLimits.Medium.ToString());
                AddStripLine(limitToDraw, ConfigurationManager.AppSettings["medium"].ToString(), "Medium Flood");

                limitToDraw = mdlLimits.High == null ? 0 : Convert.ToDouble(mdlLimits.High.ToString());
                //AddStripLine(limitToDraw, ColorTranslator.FromHtml("#666600"), "High Flood");
                AddStripLine(limitToDraw, ConfigurationManager.AppSettings["high"].ToString(), "High Flood");

                limitToDraw = mdlLimits.VHigh == null ? 0 : Convert.ToDouble(mdlLimits.VHigh.ToString());
                //AddStripLine(limitToDraw, ColorTranslator.FromHtml("#FFCC00"), "V_High Flood"); 
                AddStripLine(limitToDraw, ConfigurationManager.AppSettings["vHigh"].ToString(), "V_High Flood");

                limitToDraw = mdlLimits.EHigh == null ? 0 : Convert.ToDouble(mdlLimits.EHigh.ToString());
                AddStripLine(limitToDraw, ConfigurationManager.AppSettings["exceptionalHigh"].ToString(), "Excep_High Flood");
            }

        }

        private void AddStripLine(Double _Limit, string _ColorCode, string _Level)
        {
            if (_Limit != 0)
            {
                StripLine stripLine = new StripLine();
                stripLine.BorderColor = System.Drawing.ColorTranslator.FromHtml(_ColorCode);
                stripLine.IntervalOffset = _Limit;
                stripLine.Text = _Level;
                stripLine.ForeColor = System.Drawing.ColorTranslator.FromHtml(_ColorCode);
                stripLine.ToolTip = _Level;
                stripLine.BorderDashStyle = ChartDashStyle.Dash;
                stripLine.TextLineAlignment = StringAlignment.Far;
                FewsGraphcusecs.ChartAreas[0].AxisY.StripLines.Add(stripLine);
            }
        }

        private void GenerateFeetGraph(string _LocationID, string _SimulatedForecast, string _SimulatedHistorycal, string _Obs)
        {

            List<DateTime?> lstDate_Forecast = GetRangeOfDate(_LocationID, _SimulatedForecast);
            List<string> LstValues_Forecast = GetRangeOfValue(_LocationID, _SimulatedForecast);

            DateTime[] xAxis_Values_Forecast = new DateTime[lstDate_Forecast.Count];
            Decimal[] yAxis_Values_Forecast = new Decimal[LstValues_Forecast.Count];

            for (int i = 0; i < lstDate_Forecast.Count; i++)
            {
                xAxis_Values_Forecast[i] = ExtractDateTime(lstDate_Forecast[i].ToString());
                //xAxis_Values_Forecast[i] = DateTime.Parse(lstDate_Forecast[i].ToString(), CultureInfo.InvariantCulture); //(Convert.ToDateTime(lstDate_Forecast[i]));
                yAxis_Values_Forecast[i] = Convert.ToDecimal(LstValues_Forecast[i]);
            }

            List<DateTime?> lstDate_Historycal = GetRangeOfDate(_LocationID, _SimulatedHistorycal);
            List<string> LstValues_Historycal = GetRangeOfValue(_LocationID, _SimulatedHistorycal);

            DateTime[] xAxis_Values_Historycal = new DateTime[lstDate_Historycal.Count];
            Decimal[] yAxis_Values_Historycal = new Decimal[LstValues_Historycal.Count];
            for (int i = 0; i < lstDate_Historycal.Count; i++)
            {
                xAxis_Values_Historycal[i] = ExtractDateTime(lstDate_Historycal[i].ToString());
                //xAxis_Values_Historycal[i] = DateTime.Parse(lstDate_Historycal[i].ToString(), CultureInfo.InvariantCulture); //(Convert.ToDateTime(lstDate_Historycal[i]));//.ToShortDateString();
                yAxis_Values_Historycal[i] = Convert.ToDecimal(LstValues_Historycal[i]);
            }

            List<DateTime?> lstDate_Obs = GetRangeOfDate(_LocationID, _Obs);
            List<string> LstValues_Obs = GetRangeOfValue(_LocationID, _Obs);

            DateTime[] xAxis_Values_Obs = new DateTime[lstDate_Obs.Count];
            Decimal[] yAxis_Values_Obs = new Decimal[LstValues_Obs.Count];
            for (int i = 0; i < lstDate_Obs.Count; i++)
            {
                xAxis_Values_Obs[i] = ExtractDateTime(lstDate_Obs[i].ToString());
                //xAxis_Values_Obs[i] = DateTime.Parse(lstDate_Obs[i].ToString(), CultureInfo.InvariantCulture); //(Convert.ToDateTime(lstDate_Obs[i]));
                yAxis_Values_Obs[i] = Convert.ToDecimal(LstValues_Obs[i]);
            }



            FEWSFeetGraph.Titles["TitleFeet"].Text = LocationID + " Water Level (feet)";
            FEWSFeetGraph.Titles["TitleFeet"].Font = new System.Drawing.Font("Open Sans", 16, System.Drawing.FontStyle.Regular);

            FEWSFeetGraph.Series["H.Forecast"].Points.DataBindXY(xAxis_Values_Forecast, yAxis_Values_Forecast);
            FEWSFeetGraph.Series["H.Historical"].Points.DataBindXY(xAxis_Values_Historycal, yAxis_Values_Historycal);//xAxis_Values_Historycal,
            FEWSFeetGraph.Series["H.Obs"].Points.DataBindXY(xAxis_Values_Obs, yAxis_Values_Obs);
            FEWSFeetGraph.Series["H.Forecast"].ChartType = SeriesChartType.Line;
            FEWSFeetGraph.Series["H.Historical"].ChartType = SeriesChartType.Line;
            FEWSFeetGraph.Series["H.Obs"].ChartType = SeriesChartType.Line;


            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisY.IsStartedFromZero = false;

            FEWSFeetGraph.Legends.Add("H.Forecast");
            FEWSFeetGraph.Legends.Add("H.Historical");
            FEWSFeetGraph.Legends.Add("H.Obs");

            FEWSFeetGraph.Legends[0].LegendStyle = LegendStyle.Table;
            FEWSFeetGraph.Legends[0].TableStyle = LegendTableStyle.Wide;
            FEWSFeetGraph.Legends[0].Docking = Docking.Bottom;
            FEWSFeetGraph.Legends[0].Alignment = StringAlignment.Center;
            FEWSFeetGraph.Legends[0].Font = new System.Drawing.Font("Open Sans", 10, System.Drawing.FontStyle.Regular);
            FEWSFeetGraph.Legends[0].Title = "";
            FEWSFeetGraph.Legends[0].TitleForeColor = Color.Green;

            FEWSFeetGraph.Series[0].XValueType = ChartValueType.DateTime;
            //FEWSFeetGraph.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yyyy \n ddd";
            FEWSFeetGraph.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yyyy";
            //FEWSFeetGraph.ChartAreas[0].AxisX.LabelStyle.Angle = -70;
            FEWSFeetGraph.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "###,0.00";

            FEWSFeetGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Open Sans", 11, System.Drawing.FontStyle.Bold);
            FEWSFeetGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Open Sans", 11, System.Drawing.FontStyle.Bold);

            FEWSFeetGraph.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Open Sans", 09, System.Drawing.FontStyle.Regular);
            FEWSFeetGraph.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Open Sans", 09, System.Drawing.FontStyle.Regular);
            string[] datearr = txtDataTimeOfIssues.Value.Split(',');
            datearr = datearr[0].ToString().Split('.');

            string str = datearr[1] + "/" + datearr[0] + "/" + datearr[2];
            DateTime date = DateTime.Parse(str, CultureInfo.InvariantCulture);// Convert.ToDateTime(str);

            StripLine stripLine = new StripLine();
            stripLine.BorderColor = Color.Red;
            stripLine.IntervalOffset = date.ToOADate();
            stripLine.ToolTip = date.ToShortDateString();
            FEWSFeetGraph.ChartAreas[0].AxisX.StripLines.Add(stripLine);
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

        public List<DateTime?> GetRangeOfDate(string _LocationID, string _ParameterID)
        {
            try
            {
                List<DateTime?> Lstdate = new List<DateTime?>();
                IEnumerable<DateTime?> dadt = LstFewsXmlData.Where(x => x.locationId == _LocationID && x.parameterId == _ParameterID && x.value != "NaN").Select(x => x.date).ToList();
                Lstdate = (from x in LstFewsXmlData
                           where x.locationId.Equals(_LocationID) && x.parameterId.Equals(_ParameterID)
                           select x.date).ToList();

                return Lstdate;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<string> GetRangeOfValue(string _LocationID, string _ParameterID)
        {
            List<string> Lstdate = new List<string>();
            Lstdate = (from x in LstFewsXmlData
                       where x.locationId.Equals(_LocationID) && x.parameterId.Equals(_ParameterID)
                       select x.value).ToList();
            return Lstdate;
        }

        private int IsDataExist(string _LocationID, string _SimulatedForecast, string _SimulatedHistorycal, string _Obs)
        {
            int count = 0;
            count = (from x in LstFewsXmlData
                     where x.locationId.Equals(_LocationID) && (x.parameterId.Equals(_SimulatedForecast) || x.parameterId.Equals(_SimulatedHistorycal) || x.parameterId.Equals(_Obs))
                     select x.value).Count();
            return count;
        }

        //private class FewsXmlData
        //{
        //    public string parameterId { get; set; }
        //    public string locationId { get; set; }
        //    public string stationName { get; set; }
        //    public Nullable<System.DateTime> date { get; set; }
        //    public Nullable<System.TimeSpan> time { get; set; }
        //    public string value { get; set; }
        //    public Nullable<int> flag { get; set; }           
        //}


        private class Graph
        {
            public DateTime date { get; set; }
            public decimal Forecast { get; set; }

            public string ForecastFormatted { get; set; }
            public decimal Historical { get; set; }
            public decimal Obs { get; set; }

        }

        private void getgraph(List<Graph> LstGraph)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                // dt = GetData();                      

                //            str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});
                //            google.setOnLoadCallback(drawChart);
                //            function drawChart() {
                //            var data = new google.visualization.DataTable();
                //            data.addColumn('new Date()', 'Date');
                //            data.addColumn('Number', 'Forecast');
                //            data.addColumn('Number', 'Historical');
                //            data.addColumn('Number', 'Obs');
                // 
                //            data.addRows(" + LstGraph.Count + ");");

                //            for (int i = 10; i <= LstGraph.Count - 1; i++)
                //            {
                //                str.Append("data.setValue( " + i + "," + 0 + "," + "'" +  LstGraph.ElementAt(i).date + "');");
                //                //str.Append("data.setValue(" + i + "," + 1 + "," +   LstGraph.ElementAt(i).Forecast + ");");
                //                str.Append("data.setValue(" + i + "," + 1 + ",'" + Math.Round(Convert.ToDecimal(LstGraph.ElementAt(i).Forecast), 2).ToString("##,0.00") + "');");
                //                str.Append("data.setValue(" + i + "," + 2 + "," +   LstGraph.ElementAt(i).Historical + ") ;");
                //                str.Append("data.setValue(" + i + "," + 3 + "," +   LstGraph.ElementAt(i).Obs + ");");

                //            }

                //            str.Append("var chart = new google.visualization.LineChart(document.getElementById('chart_div'));");            
                //            str.Append(" chart.draw(data, {width: 660, height: 300, title: 'Company Performance',");
                //            str.Append("hAxis: {title: 'Date', titleTextStyle: {color: 'green'}}");
                //            str.Append("}); }");
                //            str.Append("</script>");
                lt.Text = str.ToString().Replace('*', '"');
            }
            catch
            { }
        }

        public DateTime ExtractDateTime(string _StrDateTime)
        {
            //string[] arr; string[] arr1; string newDate; DateTime parsedDateTime;
            //arr = _StrDateTime.Split(' ');
            //arr1 = arr[0].ToString().Split('/');
            //newDate = arr1[1] + "/" + arr1[0] + "/" + arr1[2] + " " + arr[1];
            //parsedDateTime = DateTime.Parse(newDate, CultureInfo.InvariantCulture);
            return DateTime.Parse(_StrDateTime);
        }


    }
}





