﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewReceivedStockCampSite.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ViewReceivedStockCampSite" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <asp:HiddenField ID="hdnFFPCampSiteID" runat="server" Value="0" />--%>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Camp Site Returned Items</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="table tbl-info">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>

                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label2" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            &nbsp;<asp:Label ID="lblRdName" runat="server" Text="RD" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_infra_type" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_infrastructure" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            &nbsp;<asp:Label ID="lbl_RD" runat="server"></asp:Label>
                        </div>

                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCat" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                            <div class="col-sm-8 col-lg-6 controls">
                                <asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="required form-control" AutoPostBack="True"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <br />
                    <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemId,ItemName,AvailableIssuedCount"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-3" />
                            <asp:TemplateField HeaderText="Quantity Issued at Camp Site">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_StoreQty" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity Received from Camp Site">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_IssuedQty" runat="server" Text='<%# Eval("ReceivedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>

            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
