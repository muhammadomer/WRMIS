<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemPurchasingEmergencyPurchases.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.EmergencyPurchasesItem" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnEmergencyPurchaseID" runat="server" Value="0" />
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Emergency purchases of items on Infrastructure</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="tbl-info">
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label ID="Label1" runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>

                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="Label2" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:Label ID="lblRDName" runat="server" Text="RD" Font-Bold="true"></asp:Label>
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
                <br />
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCat" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                            <div class="col-sm-8 col-lg-6 controls">
                                <asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="form-control required" required="true" AutoPostBack="True">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <br />
                    <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemID,ItemName,CreatedBy,CreatedDate,PurchasedQty"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="12%" />

                            <asp:TemplateField HeaderText="Quantity Purchased <br/>(in current season)">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_flood" runat="server" Text='<%# Eval("Purchased_Flood_seasonQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-right" />
                                <ItemStyle CssClass="integerInput" Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity Purchased">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Purchased_Qty" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8" Width="100%"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-right" Width="7%" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
