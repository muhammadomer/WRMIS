<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDetailDivisionStore.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ViewDetailDivisionStore" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">View Details Of Division Store</h3>
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
                            <asp:Label ID="QuantityAvailableinDivisionStoree" runat="server" Text="Quantity Available in Division Store" Font-Bold="True"></asp:Label>
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
                    <div id="MajorItemGrid" runat="server">
                        <asp:GridView ID="gvItems" runat="server"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItems_PageIndexChanging">
                            <Columns>
                                <%--<asp:BoundField DataField="MajorMinor" HeaderText="Major/Minor Item" ItemStyle-CssClass="col-lg-3" />--%>
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-2" />
                                <asp:TemplateField HeaderText="Total Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ApprovedQty" runat="server" Text='<%# Eval("QuantityApproved") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Issued">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_IssuedQty" runat="server" Text='<%# Eval("IssuedQty") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Available">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_StoreQty" runat="server" Text='<%# Eval("QuantityAvailable") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue To">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_issueto" runat="server" Text='<%# Eval("Infraname") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_issuedate" runat="server" Text='<%# Eval("IssueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

                    <div id="MinorItemGrid" runat="server">
                        <asp:GridView ID="gvViewDetailMinorItem" runat="server"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvViewDetailMinorItem_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EntryDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 " />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-md-2" />
                                <asp:TemplateField HeaderText="Quantity Effected">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityEffected" runat="server" Text='<%# Eval("QuantityEffected") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCondition" runat="server" Text='<%# Eval("Condition") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

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

