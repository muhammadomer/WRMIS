<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="GraphAndFrequencyBands.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.GraphAndFrequencyBands" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../Scripts/Complaints/loader.js"></script>
    <script src="../../Scripts/Complaints/jsapi.js"></script>
    <script type="text/javascript">
        google.load('visualization', '1.0', { 'packages': ['corechart'] });
        google.setOnLoadCallback(drawVisualization);
    </script>

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="hName" runat="server">Rotational Program Graph, Performance Table and DPR Difference</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Label runat="server" CssClass="control-label" Style="font-size: large; margin-left: 20px;" Text="Average DPRs"></asp:Label>
                                            <div runat="server" id="dvMainChart"></div>
                                            <div id="dvChart" runat="server" style="width: 800px; height: 400px; margin-left: 5px;">
                                            </div>
                                            <div id="scrollDiv">&nbsp;</div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="row">
                            <asp:Label runat="server" CssClass="control-label" Style="font-size: large; margin-left: 20px;" Text="Performance Frequency Table"></asp:Label>
                            <div class="col-md-12 ">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvFreqTable" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Frequency Band">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFrequency" runat="server" CssClass="control-label" Text='<%# Eval("FrequencyBand") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Band Count">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCount" runat="server" CssClass="control-label" Text='<%# Eval("BandCount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <%-- <asp:Label runat="server" CssClass="control-label" Style="font-size: large; margin-left: 20px;" Text="Difference Between Max DPR and Min DPR"></asp:Label>
                            <br />
                            <br />--%>
                            <asp:Label ID="lblDiffDPR" runat="server" Style="font-size: large; margin-left: 20px;" CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
