<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FEWSGraph.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FEWS.WebForm4" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" runat="server">

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderButtons" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <%--<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>--%>
    <%--  <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = google.visualization.arrayToDataTable([
              ['Year', 'Sales', 'Expenses'],
              ['2004', 1000, 400],
              ['2005', 1170, 460],
              ['2006', 660, 1120],
              ['2007', 1030, 540]
            ]);

            var options = {
                title: 'Company Performance',
                curveType: 'function',
                legend: { position: 'bottom' }
            };

            var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));

            chart.draw(data, options);
        }
    </script>--%>
    <script id="CloseMsgDivScripe" type="text/javascript">
        function CloseMsg() {
            //$("#MainContent_divMessage").css("display", "none");
            $('#MainContent_divMessage').hide();
        }
    </script>

    <style>
        /*.FewsLabel {
            background-color: #EDEDED;
            Width: 70px;
            color: #494964;
            text-align: right;
            font-family: 'Times New Roman';
            font-size: 12px;
            position: relative;
            float: left;
            height: 23px;
            width: 115px;
            margin-left: 2px;
            text-align:center;
                     
           
        }*/
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="forecastHeader">Station - <%=PMIU.WRMIS.Common.Utility.GetStringValueFromQueryString("Stations", "")%></h3>
                </div>

                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-4 control-label">Time of Issue</label>
                                    <div class="col-sm-6 col-lg-6 controls">
                                        <asp:TextBox runat="server" ID="txtDataTimeOfIssue" type="text" class="form-control " ReadOnly="True"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-4 control-label">Forecast Time</label>
                                    <div class="col-sm-6 col-lg-6 controls">
                                        <asp:TextBox runat="server" ID="txtTime" type="text" class="form-control " ReadOnly="True"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>


                        </div>

                    </div>



                    <%-- style="border:1px solid; margin-right:25px; margin-left:25px;"--%>

                    <div id="wrapperGraph" runat="server" style="width: auto;">
                        <div id="SeasonMessage" style="width: auto; height: auto;" runat="server" visible="false">
                            <label id="Message" runat="server" visible="true" style="color: red; margin-left: 25px; margin-top: 15px; font-size: 18px"></label>
                        </div>
                        <%-- style=" display:inline;float:left; border:1px solid; margin-left:5px;"--%>

                        <div id="Cusecsdiv" runat="server" style="display: block; float: left; border: 0px solid; margin-left: 5px; width: 100%; margin-bottom: 25px;">
                            <asp:Chart ID="FewsGraphcusecs" runat="server" Height="580px" Width="1094px" ImageStorageMode="UseImageLocation" BackGradientStyle="None">
                                <Titles>
                                    <asp:Title Font="Open Sans, 10pt, style=Bold" Name="TitleCusecs" Text="" />
                                </Titles>

                                <Legends>
                                    <%--  <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False"  LegendStyle="Row" />--%>
                                </Legends>
                                <Series>
                                    <asp:Series Name="Q.Forecast" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} cusecs" BorderWidth="3" ChartType="Line" />
                                    <asp:Series Name="Q.Historical" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} cusecs" BorderWidth="3" ChartType="Line" />
                                    <asp:Series Name="Q.Obs" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} cusecs" BorderWidth="3" ChartType="Line" />
                                </Series>

                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BackColor="#ffffff" BackSecondaryColor="#ddddee" BackGradientStyle="TopBottom" >
                                        <AxisX IntervalAutoMode="FixedCount" TitleFont="Open Sans" LabelAutoFitMaxFontSize="18" Title="Date">
                                            <MajorGrid Enabled="false" />
                                        </AxisX>
                                        <AxisY Title="Discharge" IntervalAutoMode="FixedCount" TitleFont="Open Sans" LabelAutoFitMaxFontSize="18">
                                            <MajorGrid Enabled="false" />
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>

                            </asp:Chart>
                        </div>
                        <%-- border:1px solid; margin-right:25px; margin-left:25px; margin-top:10px;--%>

                        <%-- style="width: auto; margin-left: 0px; float: left; border:1px solid;"--%>
                        <div id="Feetdiv" style="width: 100%; margin-left: 0px; border: 0px solid;" runat="server">
                            <asp:Chart ID="FEWSFeetGraph" runat="server" Height="580px" Width="1094px" ImageStorageMode="UseImageLocation">
                                <Titles>
                                    <asp:Title Font="Open Sans, 10pt, style=Bold" Name="TitleFeet" Text="" />
                                </Titles>

                                <Legends>
                                    <%--  <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False"  LegendStyle="Row"  />--%>
                                </Legends>
                                <Series>
                                    <asp:Series Name="H.Forecast" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} feet" Color="DarkGreen" BorderWidth="3" ChartType="Line" />
                                    <asp:Series Name="H.Historical" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} feet" Color="YellowGreen" BorderWidth="3" ChartType="Line" />
                                    <asp:Series Name="H.Obs" ToolTip="#VALX{dd-MMM-yyyy hh tt} | #VALY{##,0.00} feet" Color="SpringGreen" BorderWidth="3" ChartType="Line" />
                                </Series>

                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BackColor="#ffffff" BackSecondaryColor="#ddddee" BackGradientStyle="TopBottom">
                                        <AxisX IntervalAutoMode="FixedCount" LineWidth="2" TitleFont="Open Sans" LabelAutoFitMaxFontSize="13">
                                        </AxisX>
                                        <AxisY Title="Level - Feet" IntervalAutoMode="FixedCount" LineWidth="2" TitleFont="Open Sans" LabelAutoFitMaxFontSize="13">
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn" style="margin-left: 26px;">
                                    <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" />--%>
                                    <a ID="hlBack" class="btn" onclick="window.history.go(-1); return false;">Back</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />

                    <%--    <div id="curve_chart" style="width: 900px; height: 500px"></div>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:Literal ID="lt" runat="server"></asp:Literal>
    <div id="chart_div"></div>
    <asp:HiddenField ID="txtDataTimeOfIssues" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtTimes" runat="server"></asp:HiddenField>

</asp:Content>
