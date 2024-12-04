using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Data;
using PMIU.WRMIS.BLL.WaterLosses;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.Web.Modules.WaterLosses
{
    public partial class CommandWaterLosses : BasePage
    {
        List<object> lstCommand = new List<object>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                LoadCommandDDL();
                lbUnits.Visible = false;
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.CommandWaterLosses);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        void LoadCommandDDL()
        {
            lstCommand = new WaterLossesBLL().GetCommandList();
            Dropdownlist.DDLLoading(ddlCommand, false, (int)Constants.DropDownFirstOption.Select, lstCommand);
        }

        protected void ddlCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbUnits.Visible = false; ;
            try
            {
                string commandID = ddlCommand.SelectedItem.Value;
                long command_ID = Convert.ToInt64(commandID);
                List<object> lstData = new List<object>();

                if (string.IsNullOrEmpty(commandID) == true) 
                    lstData = null;
                else
                    lstData = new WaterLossesBLL().GetReachListByCommand(command_ID);
                if (lstData != null && lstData.Count > 0)
                {
                    Dropdownlist.DDLLoading(ddlReach, false, (int)Constants.DropDownFirstOption.Select, lstData);
                    ddlYear.Enabled = true;
                    ddlMonth.Enabled = true;
                    ddlReach.Enabled = true;
                }
                List<object> lstYear = new List<object>();
                if (command_ID == 1) //Indus Command
                    lstYear = new WaterLossesBLL().GetIndusCommandWLYearListByReach();
                else //Jhelum Chenab Command
                    lstYear = new WaterLossesBLL().GetJCCommandWLYearListByReach();

                Dropdownlist.DDLLoading(ddlYear, false, (int)Constants.DropDownFirstOption.Select, lstYear);
               
                pnlSearchArea.Update();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            lbUnits.Visible = false;
            lblheader.Text = "";
            pnlCharts.Visible = false;
            pnlTblurData.Visible = false;

            List<object> lstData = new List<object>();
            long command_ID, reach_ID = 0; int year = 0, month = 0;

            if (parametersProvided())
            {
                command_ID = Convert.ToInt64(ddlCommand.SelectedItem.Value);
                reach_ID = Convert.ToInt64(ddlReach.SelectedItem.Value);
                year = Convert.ToInt32(ddlYear.SelectedItem.Value);
                month = Convert.ToInt32(ddlMonth.SelectedItem.Value);

                
                lblheader.Text = ddlReach.SelectedItem.Text + " water loss for " + ddlMonth.SelectedItem.Text + " " + year;
                if (command_ID == 1) //Indus Command
                {
                    string reachName = ddlReach.SelectedItem.Text;
                    
                    lstData = new WaterLossesBLL().GetWaterLosses_Indus(reachName, reach_ID, year, month);
                    lbUnits.Visible = true;
                    gvData.DataSource = lstData;
                    gvData.DataBind(); 
                }
                else //Jhelum Chenab Command
                {
                    string reachName = ddlReach.SelectedItem.Text;
                    lbUnits.Visible = true;
                    lstData = new WaterLossesBLL().GetWaterLosses_JC(reachName, reach_ID, year, month);
                    gvData.DataSource = lstData;
                    gvData.DataBind(); 
                }
            }
            pnlTblurData.Update();
            pnlTblurData.Visible = true;
        }

        protected void btnViewGraph_Click(object sender, EventArgs e)
        {
            pnlCharts.Visible = false;
            pnlTblurData.Visible = false;

            List<string> lstData = new List<string>();
            long command_ID, reach_ID = 0; int year = 0, month = 0;

            if (parametersProvided())
            {
                command_ID = Convert.ToInt64(ddlCommand.SelectedItem.Value);
                reach_ID = Convert.ToInt64(ddlReach.SelectedItem.Value);
                year = Convert.ToInt32(ddlYear.SelectedItem.Value);
                month = Convert.ToInt32(ddlMonth.SelectedItem.Value);
                string reachName = ddlReach.SelectedItem.Text;

                if (command_ID == 1) //Indus Command
                {  
                    if (reachName.Contains("Tarbela-Chashma") || reachName.Contains("Chashma-Kotri"))
                    { 
                        double[] yAxis_Values = new double[lstData.Count];
                        string[] xAxis_Values = new string[lstData.Count];
                        double yAxis_Interval = 0;
                        double max_LG = 0;

                        if (reachName.Contains("Tarbela-Chashma"))
                        {
                            #region draw kalabah loss and gain
                            reachName = "Tarbela-Kalabagh";
                            lstData = new WaterLossesBLL().GetWaterLossesGains_Indus(reachName, reach_ID, year, month);
                            int i =0;
                            if (lstData != null && lstData.Count > 0)
                            {
                                yAxis_Values = new double[lstData.Count];
                                xAxis_Values = new string[lstData.Count];
                                for ( ; i < lstData.Count; i++)
                                {
                                    string[] LGData = lstData.ElementAt(i).ToString().Split(':');
                                    xAxis_Values[i] = LGData[0];
                                    double lg = 0.0;
                                    if (!string.IsNullOrEmpty(LGData[1]))
                                        lg = Convert.ToDouble(LGData[1]);
                                    if (lg > max_LG)
                                        max_LG = lg;
                                    yAxis_Values[i] = lg;
                                } 

                                yAxis_Interval = max_LG / 10;
                                DrawChartLine(yAxis_Values, xAxis_Values, 0, reachName);
                            }
                            #endregion

                            #region draw kalabagh chashma loss and gain
                            reachName = "Kalabagh-Chashma";
                            i = 0;
                            lstData = new WaterLossesBLL().GetWaterLossesGains_Indus(reachName, reach_ID, year, month);      

                            if (lstData != null && lstData.Count > 0)
                            {
                                yAxis_Values = new double[lstData.Count];
                                xAxis_Values = new string[lstData.Count];
                                for (;  i < lstData.Count; i++)
                                {
                                    string[] LGData = lstData.ElementAt(i).ToString().Split(':');
                                    xAxis_Values[i] = LGData[0];
                                    double lg = 0.0;
                                    if (!string.IsNullOrEmpty(LGData[1]))
                                        lg = Convert.ToDouble(LGData[1]);
                                    if (lg > max_LG)
                                        max_LG = lg;
                                    yAxis_Values[i] = lg;
                                } 
                                DrawChartLine(yAxis_Values, xAxis_Values, 1, reachName);
                                waterLossChart.Series[1].Color = Color.Green;
                            }
#endregion
                        }
                        else if (reachName.Contains("Chashma-Kotri") )
                        {
                            //draw chashma taunsa loss and gains
                            //draw Taunsa Gudda loss and gain
                            //draw Guddu sukkur loss and gain
                            //draw sukkur korti loss and gain
                        }

                        waterLossChart.ChartAreas.Add("" + 0);
                        waterLossChart.ChartAreas[0].AxisX.Interval = 1;
                        waterLossChart.ChartAreas[0].AxisY.Interval = yAxis_Interval;
                        waterLossChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                        waterLossChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray; 
                    }
                    else //single line chart
                    {
                        lstData = new WaterLossesBLL().GetWaterLossesGains_Indus(reachName, reach_ID, year, month);
                        DrawSingleChart(lstData , reachName);
                    } 
                }
                else //Jhelum Chenab Command
                {
                    if (reachName.Contains("Mangla-Panjnad"))
                    {
                    }
                    else //single line chart
                    {
                        lstData = new WaterLossesBLL().GetWaterLossesGains_Indus(reachName, reach_ID, year, month);
                        DrawSingleChart(lstData, reachName);
                    } 
                }
               
            } 
            pnlCharts.Visible = true;
            pnlCharts.Update();
        }

        private void DrawSingleChart(List<string> lstData , string reachName)
        {
            if (lstData != null && lstData.Count > 0)
            {
                double[] yAxis_Values = new double[lstData.Count];
                string[] xAxis_Values = new string[lstData.Count];
                double max_LG = 0, yAxis_Interval = 0;
                 
                for (int i = 0; i < lstData.Count; i++)
                {
                    string[] LGData = lstData.ElementAt(i).ToString().Split(':');
                    xAxis_Values[i] = LGData[0];
                    double lg = 0.0;
                    if (!string.IsNullOrEmpty(LGData[1]))
                        lg = Convert.ToDouble(LGData[1]);
                    if (lg > max_LG)
                        max_LG = lg;
                    yAxis_Values[i] = lg;
                }

                yAxis_Interval = max_LG / 10;

                waterLossChart.Legends.Add(ChartName(reachName));

                waterLossChart.Series.Add(ChartName(reachName)); 
              //  waterLossChart.Series[0].Name = ChartName(reachName);
                waterLossChart.Series[0].ChartType = SeriesChartType.Line;
                waterLossChart.Series[0].MarkerColor = Color.Black;
                waterLossChart.Series[0].MarkerStyle = MarkerStyle.Diamond;
                waterLossChart.Series[0].MarkerSize = 6;
                waterLossChart.Series[0].IsValueShownAsLabel = true;
                waterLossChart.Series[0].IsXValueIndexed = true;
                waterLossChart.Series[0].Points.DataBindXY(xAxis_Values, yAxis_Values);
                waterLossChart.Series[0].ToolTip = "#VALX| #VALY";
                waterLossChart.ChartAreas.Add("" + 0);
                waterLossChart.ChartAreas[0].AxisX.Interval = 1;
                waterLossChart.ChartAreas[0].AxisY.Interval = yAxis_Interval;
                waterLossChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                waterLossChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            }
            else
            {
                // show now record found message
            }
        }
         
        private string ChartName(string _ReachName)
        {
            string name = "";
            if (_ReachName.Contains("Tarbela-Kalabagh"))
                name = "Tar-Kal";
            else if (_ReachName.Contains("Kalabagh-Chashma"))
                name = "Kal-Cha";
            else if (_ReachName.Contains("Chashma-Taunsa"))
                name = "Cha-Tau";
            else if (_ReachName.Contains("Taunsa-Guddu"))
                name = "Tau-Gud";
            else if (_ReachName.Contains("Guddu-Sukkur"))
                name = "Gud-Suk";
            else if (_ReachName.Contains("Sukkur-Kotri"))
                name = "Suk-Kot";
            else if (_ReachName.Contains("Tarbela-Chashma"))
                name = "Tar-Cha";
            else if (_ReachName.Contains("Chashma-Kotri"))
                name = "Cha-Kot";
            else if (_ReachName.Contains("Mangla-Marala-Rasul"))
                name = "Mang-Panj";
            else if (_ReachName.Contains("Rasul-Trimmu"))
                name = "Mang-Ras";
            else if (_ReachName.Contains("Trimmu-Panjnad"))
                name = "Tri-Panj";
            else if (_ReachName.Contains("Mangla-Panjnad"))
                name = "Mang-Pan";

            return name;
        }
        
        private void DrawChartLine(double[] _yAxis_Values, string[] _xAxis_Values, int _ChartIndex, string _ReachName)
        {
            waterLossChart.Legends.Add(ChartName(_ReachName));
            //waterLossChart.Legends[_ChartIndex].Name = "";

            waterLossChart.Series.Add(ChartName(_ReachName));
            waterLossChart.Series[_ChartIndex].Name = ChartName(_ReachName);
            waterLossChart.Series[_ChartIndex].ChartType = SeriesChartType.Line;
            waterLossChart.Series[_ChartIndex].MarkerColor = Color.Black;
            waterLossChart.Series[_ChartIndex].MarkerStyle = MarkerStyle.Diamond;
            waterLossChart.Series[_ChartIndex].MarkerSize = 6;
            waterLossChart.Series[_ChartIndex].IsValueShownAsLabel = true;
            waterLossChart.Series[_ChartIndex].ToolTip = "#VALX| #VALY";
            waterLossChart.Series[_ChartIndex].Points.DataBindXY(_xAxis_Values, _yAxis_Values);

        }

        private bool parametersProvided()
        {
            if (string.IsNullOrEmpty(ddlCommand.SelectedItem.Value))
                return false;

            else if (string.IsNullOrEmpty(ddlReach.SelectedItem.Value))
                return false;

            else if (string.IsNullOrEmpty(ddlMonth.SelectedItem.Value))
                return false;

            else if (string.IsNullOrEmpty(ddlYear.SelectedItem.Value))
                return false;
            else
                return true;
        }
         
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow current = e.Row;
                current.Attributes.Add("style", "white-space:nowrap;");
                foreach (TableCell cell in current.Cells)
                {
                    cell.Attributes.Add("class", "text-right");
                    
                    if (cell.Text.Any(c => Char.IsLetter(c)))
                        cell.Attributes.Add("class", "text-center"); 
                }
            }
            if (e.Row.RowType == DataControlRowType.Header )
            { 
                GridViewRow header = e.Row;
                header.Attributes.Add("class", "text-center tnowrap");
                header.Cells.Clear();
                GenerateHeader(header);
                //Tarbeela_Kalabagh_Header(header);
            }
            //if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            //{
            //    GridViewRow header = new GridViewRow(0,0, DataControlRowType.Header, DataControlRowState.Insert);
            //    header.Attributes.Add("class", "text-center");
            //    header.Cells.Clear();
            //    Tarbeela_Kalabagh_Header(header);
            //    gvData.Controls[0].Controls.AddAt(0, header);

            //}
        }

        private void GenerateHeader(GridViewRow _HeaderRow)
        {
            string reachName = ddlReach.SelectedItem.Text;
            if (reachName.Contains("Kalabagh-Chashma"))
            {
                Kalabagh_Chashma_Header(_HeaderRow);
            }
            else if (reachName.Contains("Chashma-Taunsa"))
            {
                Chashma_Taunsa_Header(_HeaderRow);
            }
            else if (reachName.Contains("Taunsa-Guddu"))
            {
                Taunsa_Guddu_Header(_HeaderRow);
            }
            else if (reachName.Contains("Guddu-Sukkur"))
            {
                Guddu_Sukkur_Header(_HeaderRow);
            }
            else if (reachName.Contains("Sukkur-Kotri"))
            {
                Sukkur_Kotri_Header(_HeaderRow);
            }
            else if (reachName.Contains("Tarbela-Kalabagh"))
            {
                Tarbeela_Kalabagh_Header(_HeaderRow);
            }
            else if (reachName.Contains("Tarbela-Chashma"))
            {
                Tarbela_Chashma_Header(_HeaderRow);
            }
            else if (reachName.Contains("Chashma-Kotri"))
            {
                Chashma_Korti_Header(_HeaderRow);
            }
            if (reachName.Contains("Mangla-Marala-Rasul"))
            {
                Mangla_Marala_Rasul_Header(_HeaderRow);
            }
            else if (reachName.Contains("Rasul-Trimmu"))
            {
                Rasul_Trimmu_Header(_HeaderRow);
            }
            else if (reachName.Contains("Trimmu-Panjnad"))
            {
                Trimmu_Pnjnd_Header(_HeaderRow);
            }
            else if (reachName.Contains("Mangla-Panjnad"))
            {
                Mngla_Pnjnd_Header(_HeaderRow);
            }
        }
        
        private void Kalabagh_Chashma_Header(GridViewRow HeaderRow) 
        { 
            HeaderRow.Cells.Add(GetTableCell("Kalabagh Date"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh US"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh DS"));
            HeaderRow.Cells.Add(GetTableCell("Thal Canal Wdls"));
            HeaderRow.Cells.Add(GetTableCell("Chashma Date"));
            HeaderRow.Cells.Add(GetTableCell("Chashma M.Inflow"));
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));  
        }
        private void Chashma_Taunsa_Header(GridViewRow HeaderRow) 
        { 
            HeaderRow.Cells.Add(GetTableCell("Chashma Date"));
            HeaderRow.Cells.Add(GetTableCell("Chashma M.Inflow"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa Date"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa U/S"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa D/S"));
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
        }
        private void Taunsa_Guddu_Header(GridViewRow HeaderRow) 
        { 
            HeaderRow.Cells.Add(GetTableCell("Taunsa Date"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa U/S"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa D/S"));
            HeaderRow.Cells.Add(GetTableCell("Pnj. Date"));
            HeaderRow.Cells.Add(GetTableCell("Pnj. Dis D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Taunsa D/S + Pnj Dis")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu Date")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
        }
        private void Guddu_Sukkur_Header(GridViewRow HeaderRow) 
        {  
            HeaderRow.Cells.Add(GetTableCell("Guddu Date")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu D/S"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur Date"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur U/S"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur D/S"));                                              
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
        }
        private void Sukkur_Kotri_Header(GridViewRow HeaderRow) 
        { 
            HeaderRow.Cells.Add(GetTableCell("Sukkur Date"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur U/S"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur D/S"));
            HeaderRow.Cells.Add(GetTableCell("Kotri Date")); 
            HeaderRow.Cells.Add(GetTableCell("Kotri U/S"));  
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
        }
        private void Tarbeela_Kalabagh_Header(GridViewRow HeaderRow)
        {  
            HeaderRow.Cells.Add(GetTableCell("Tarbela Date"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela M.Inflow"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela M.Outflow"));
            HeaderRow.Cells.Add(GetTableCell("River Kabul"));
            HeaderRow.Cells.Add(GetTableCell("Total"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh Date"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh US"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh DS"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh Loss Gain")); 
        }
        private void Tarbela_Chashma_Header(GridViewRow HeaderRow) 
        { 
            HeaderRow.Cells.Add(GetTableCell("Tarbela Date"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela Res. Level"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela M.Inflow"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela M.Outflow")); 
            HeaderRow.Cells.Add(GetTableCell("River Kabul"));  
            HeaderRow.Cells.Add(GetTableCell("Total")); 
            HeaderRow.Cells.Add(GetTableCell("Kalabagh Date"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh U/S"));
            HeaderRow.Cells.Add(GetTableCell("Kalabagh D/S"));
            HeaderRow.Cells.Add(GetTableCell("Thal Canal Wdls")); 
            HeaderRow.Cells.Add(GetTableCell("Kalabagh Loss/Gain")); 
            HeaderRow.Cells.Add(GetTableCell("Balance for CSB"));
            HeaderRow.Cells.Add(GetTableCell("Chashma Date"));
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));
            HeaderRow.Cells.Add(GetTableCell("Chashma Res. Level")); 
            HeaderRow.Cells.Add(GetTableCell("Balance"));  
            HeaderRow.Cells.Add(GetTableCell("Chashma M.Inflow")); 
            HeaderRow.Cells.Add(GetTableCell("CRBC"));
            HeaderRow.Cells.Add(GetTableCell("CJ Link"));
            HeaderRow.Cells.Add(GetTableCell("CSB D/S"));
            HeaderRow.Cells.Add(GetTableCell("Total")); 
        }
        private void Chashma_Korti_Header(GridViewRow HeaderRow)
        { 
            HeaderRow.Cells.Add(GetTableCell("Chashma Date"));
            HeaderRow.Cells.Add(GetTableCell("Chashma M.Outflow")); 
            HeaderRow.Cells.Add(GetTableCell("Taunsa Date"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa U/S"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gain"));  
            HeaderRow.Cells.Add(GetTableCell("Pnj Date")); 
            HeaderRow.Cells.Add(GetTableCell("Pnj D/S"));
            HeaderRow.Cells.Add(GetTableCell("Taunsa D/S + Pnd D/S"));
            HeaderRow.Cells.Add(GetTableCell("Guddu Date"));
            HeaderRow.Cells.Add(GetTableCell("Guddu U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Guddu D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur Date"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur U/S"));
            HeaderRow.Cells.Add(GetTableCell("Sukkur D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));  
            HeaderRow.Cells.Add(GetTableCell("Kotri Date")); 
            HeaderRow.Cells.Add(GetTableCell("Korti U/S"));
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));
            HeaderRow.Cells.Add(GetTableCell("Tarbela-Chashma"));
            HeaderRow.Cells.Add(GetTableCell("Chashma-Korti"));
            HeaderRow.Cells.Add(GetTableCell("Korti-Tarbel")); 
        }

        private void Mangla_Marala_Rasul_Header(GridViewRow HeaderRow) 
        {
            HeaderRow.Cells.Add(GetTableCell("Mangla Date")); 
            HeaderRow.Cells.Add(GetTableCell("Mangla Res. Level"));   
            HeaderRow.Cells.Add(GetTableCell("Mangla M.Inflow")); 
            HeaderRow.Cells.Add(GetTableCell("Mangla M.Outflow"));  
            HeaderRow.Cells.Add(GetTableCell("Mean Outflow-UJC"));   
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));  
            HeaderRow.Cells.Add(GetTableCell("Marala U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Marala D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Rasul Date")); 
            HeaderRow.Cells.Add(GetTableCell("Rasul U/S"));  
            HeaderRow.Cells.Add(GetTableCell("Rasul D/S"));  
            HeaderRow.Cells.Add(GetTableCell("QBD Below"));  
        }
        private void Mngla_Pnjnd_Header(GridViewRow HeaderRow) 
        {
            HeaderRow.Cells.Add(GetTableCell("Mangla Date"));  
            HeaderRow.Cells.Add(GetTableCell("Mangla Res Level"));   
            HeaderRow.Cells.Add(GetTableCell("Mangla M.Inflow")); 
            HeaderRow.Cells.Add(GetTableCell("Magla M.Outflow")); 
            HeaderRow.Cells.Add(GetTableCell("Mangla M.Outflow - UJC")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
            HeaderRow.Cells.Add(GetTableCell("Marala U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Marala D/S"));  
            HeaderRow.Cells.Add(GetTableCell("Rasul Date"));  
            HeaderRow.Cells.Add(GetTableCell("Rasul U/S"));  
            HeaderRow.Cells.Add(GetTableCell("Rasul D/S")); 
            HeaderRow.Cells.Add(GetTableCell("QBD Below"));                                            
            HeaderRow.Cells.Add(GetTableCell("CJ Date")); 
            HeaderRow.Cells.Add(GetTableCell("CJ at Head - GTC"));  
            HeaderRow.Cells.Add(GetTableCell("Total")); 
            HeaderRow.Cells.Add(GetTableCell("Trimmu Date")); 
            HeaderRow.Cells.Add(GetTableCell("Trimmu U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Trimmu D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
            HeaderRow.Cells.Add(GetTableCell("Islam D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Sidhnai D/S")); 
            HeaderRow.Cells.Add(GetTableCell("TP Date")); 
            HeaderRow.Cells.Add(GetTableCell("TP Link")); 
            HeaderRow.Cells.Add(GetTableCell("Total")); 
            HeaderRow.Cells.Add(GetTableCell("Panjnad Date")); 
            HeaderRow.Cells.Add(GetTableCell("Panjnad U/S")); 
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains")); 
            HeaderRow.Cells.Add(GetTableCell("Total Losses/Gains"));  
        }
        private void Rasul_Trimmu_Header(GridViewRow HeaderRow) 
        {
             HeaderRow.Cells.Add(GetTableCell("Rasul Date")); 
            HeaderRow.Cells.Add(GetTableCell("Rasul U/S"));  
            HeaderRow.Cells.Add(GetTableCell("Rasul D/S"));  
            HeaderRow.Cells.Add(GetTableCell("QBD Below")); 
            HeaderRow.Cells.Add(GetTableCell("CJ at Head Date")); 
            HeaderRow.Cells.Add(GetTableCell("C-J at Head(-)GTC"));   
            HeaderRow.Cells.Add(GetTableCell("Total")); 
            HeaderRow.Cells.Add(GetTableCell("Trimmu Date"));  
            HeaderRow.Cells.Add(GetTableCell("Trimmu U/S"));   
            HeaderRow.Cells.Add(GetTableCell("Trimmu D/S"));  
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));  
        }
        private void Trimmu_Pnjnd_Header(GridViewRow HeaderRow) 
        {
            HeaderRow.Cells.Add(GetTableCell("Trimmu Date"));  
            HeaderRow.Cells.Add(GetTableCell("Trimmu U/S"));   
            HeaderRow.Cells.Add(GetTableCell("Trimmu D/S")); 
            HeaderRow.Cells.Add(GetTableCell("Islam")); 
            HeaderRow.Cells.Add(GetTableCell("Sidhnai")); 
            HeaderRow.Cells.Add(GetTableCell("TP Date ")); 
            HeaderRow.Cells.Add(GetTableCell("T.P Link")); 
            HeaderRow.Cells.Add(GetTableCell("Total"));  
            HeaderRow.Cells.Add(GetTableCell("Punjnad Date"));  
            HeaderRow.Cells.Add(GetTableCell("Punjnad U/S"));  
            HeaderRow.Cells.Add(GetTableCell("Losses/Gains"));  
        }

        private TableHeaderCell GetTableCell(string _CellValue)
        {
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = _CellValue;
            cell.Attributes.Add("class", "text-center");
            return cell;
        } 
    }
}