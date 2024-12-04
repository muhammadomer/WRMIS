<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistoryDivisionStore.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.HistoryDivisionStore" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Item History Of Division Store</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="tbl-info">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="lblZone" runat="server" Text="Zone" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblCircle" runat="server" Text="Circle" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDivision" runat="server" Text="Division" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblZonevalue" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblCirclevalue" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDivisionvalue" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="lblYear" runat="server" Text="Year" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="QuantityAvailableinDivisionStoree" runat="server" Text="Available in Division Store" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="lblYearvalue" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblQuantityAvailablevalue" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="table-responsive">
                    <br />
                    <asp:GridView ID="gvItemsHistory" runat="server" DataKeyNames="ETStatus,StructureName"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItemsHistory_PageIndexChanging" OnRowDataBound="gvItemsHistory_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EntryDate","{0:d/M/yyyy HH:mm:ss}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issued Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedStatus" runat="server" Text='<%# Eval("ETStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 " />
                                <%--<ItemStyle CssClass="text-right" />--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedStatus" runat="server" Text='<%# Eval("ETStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                                <%--<ItemStyle CssClass="text-right" />--%>
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
