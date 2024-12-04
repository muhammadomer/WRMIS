<%@ Page Title="Para2" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Para2.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.Para2" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Para2</h3>
                </div>
                <div class="box-content">
                    <asp:GridView ID="gvPara" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        OnPageIndexChanged="gvPara_PageIndexChanged" OnPageIndexChanging="gvPara_PageIndexChanging" OnRowDataBound="gvPara_RowDataBound"
                        ShowHeaderWhenEmpty="True" ShowFooter="true"  CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Period">
                                <ItemTemplate>
                                    <asp:Label ID="lblTenDaily" runat="server" Text='<%# Eval("TDaily") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="EK (MAF)" runat="server" Visible="true" />
                                    <br />
                                    <asp:Label Text="LK (MAF)" runat="server" Visible="true" />
                                    <br />
                                    <asp:Label Text="Total (MAF)" runat="server" Visible="true" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Indus Command Para2">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndus" runat="server" Text='<%#String.Format("{0:0.0}",Eval("Indus")) %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label Text="No Value" ID="lblEK" runat="server" Visible="true" /><br />
                                    <asp:Label Text="No Value" ID="lblLK" runat="server" Visible="true" /><br />
                                    <asp:Label Text="No Value" ID="lblTotal" runat="server" Visible="true" />
                                </FooterTemplate>
                                <HeaderStyle CssClass="col-md-2" />
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
