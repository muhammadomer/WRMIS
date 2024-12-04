<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeasuringBookStatusPreFlood.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.MeasuringBookStatusPreFlood" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Measuring Book Status for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="hidden">
                                FloodInspectionsID=15
                                <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                                        <div class="col-sm-8 col-lg-6 controls">
                                            <asp:DropDownList ID="ddlCategory" runat="server" required="required" CssClass="required form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="table-responsive">
                                    <%--<asp:GridView 
                                      OnRowCommand="gvSubDivision_RowCommand"
                                      >--%>
                                    <asp:GridView ID="gvMeasuringBookStatus" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header"
                                        CellSpacing="-1" GridLines="None" OnRowDataBound="gvMeasuringBookStatus_RowDataBound" DataKeyNames="PreMBStatusID,ItemID,LYQty,DIVIssueQty,AvaQty">

                                        <%--<asp:GridView ID="gvMeasuringBookStatus" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True"
                                        CssClass="table header"
                                        CellSpacing="-1" GridLines="None" OnRowDataBound="gvMeasuringBookStatus_RowDataBound">--%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("PreMBStatusID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--                                            <asp:TemplateField HeaderText="Major/Minor">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMajorMinor" runat="server" Text='<%#Eval("MajorMinor") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Previous Year Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreviousYearQuantity" runat="server" Text='<%#Eval("LYQty") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2 text-right" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity Issued From Division Store">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantityIssuedFromDivisionStore" runat="server" Text='<%#Eval("DIVIssueQty") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2 text-right" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity Available">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantityAvalible" runat="server" class="integerInput form-control" pattern="[0-9]{0,7}" Text='<%#Eval("AvaQty") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2 text-right" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceQuantity" runat="server" Text='<%#Eval("BalanceQty") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2 text-right" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" align="center" Text="Save" OnClick="btnSave_Click" />
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" align="center" Text="Save" OnClick="btnSave_Click" />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-danger" Text="Back"></asp:HyperLink>
                            </div>--%>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
