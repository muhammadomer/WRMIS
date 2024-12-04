<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewGraph.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.ViewGraph" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../Scripts/Complaints/jsapi.js"></script>
    <script type="text/javascript" src="../../Design/js/loader-graph.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['bar'] });
        //google.load("visualization", "1", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawVisualization);
    </script>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Literal runat="server" ID="ltlPageTitle"> Irrigator Feedback Graph</asp:Literal>
                    </h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="form-group">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <asp:Label runat="server" CssClass="control-label" Style="font-size: large; margin-left: 30px;" Text="Last 10 days Status"></asp:Label>
                                        <br />
                                        <br />
                                        <div runat="server" id="dvLtScripts" style="margin-left: 35px;"></div>
                                        <div id="chart_div" runat="server" style="width: 1650px; height: 500px; margin-left: 5px;">
                                        </div>
                                        <div id="scrollDiv">&nbsp;</div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnChannelID" runat="server" ClientIDMode="Static" />
    </div>
</asp:Content>
