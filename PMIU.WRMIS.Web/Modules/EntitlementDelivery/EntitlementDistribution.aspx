<%@ Page Title="Entitlements Distribution" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="EntitlementDistribution.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.EntitlementDistribution" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Entitlement Delivery</h3>
                        </div>
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Command</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlCommand" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged"/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Season</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlSeason" runat="server" Enabled="false" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Year</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" Enabled="false" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Main Canals</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlMainCanals" runat="server" Enabled="false" AutoPostBack="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Branch Canal</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlBranchCanal" runat="server" Enabled="false" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Distributary</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlDistributary" runat="server" Enabled="false" AutoPostBack="true">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Minor</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlMinor" runat="server" Enabled="false" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Sub Minor</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlSubMinor" runat="server" Enabled="false" AutoPostBack="true">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvEntitlement" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                            ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Period">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPeriod" runat="server" CssClass="control-label" Text='<%# Eval("Period") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Entitlement(Cusecs)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntitlement" runat="server" CssClass="control-label" Text='<%# Eval("Entitlement") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deliveries(Cusecs)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeliveries" runat="server" CssClass="control-label" Text='<%# Eval("Deliveries") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Difference">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDifference" runat="server" CssClass="control-label" Text='<%# Eval("Difference") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delivery(MAF)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffenceSite" runat="server" CssClass="control-label" Text='<%# Eval("DeliveryMAF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Accumulative Delivery(MAF)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccDeliveryMAF" runat="server" CssClass="control-label" Text='<%# Eval("AccDeliveryMAF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalance" runat="server" CssClass="control-label" Text='<%# Eval("Balance") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>










        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
