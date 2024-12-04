<%@ Page Title="Water Distribution" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WaterDistribution.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.WaterDistribution" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Water Distribution</h3>
                </div>
                <div class="box-content">
                    <asp:GridView ID="gvWaterDistribution" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        OnPageIndexChanged="gvWaterDistribution_PageIndexChanged" OnPageIndexChanging="gvWaterDistribution_PageIndexChanging"
                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Period">
                                <ItemTemplate>
                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="0%">
                                <ItemTemplate>
                                    <asp:Label ID="lblZero" runat="server" Text='<%#String.Format("{0:0.0}", Eval("Percentile0")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblFine" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile5")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTen" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile10")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblFifteen" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile15")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTwenty" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile20")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="25%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTwentyFive" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile25")) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblThirty" runat="server" Text='<%# String.Format("{0:0.0}",Eval("Percentile30")) %>'></asp:Label>
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
    </div>
</asp:Content>

