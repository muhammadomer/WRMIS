<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddReceivedStock.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.AddReceivedStock" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Add Received Stock</h3>

                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblReceivedStockType" runat="server" Text="Received Stock Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlReceivedStockType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlReceivedStockType_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvReceivedStock" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvReceivedStock_PageIndexChanging" OnPageIndexChanged="gvReceivedStock_PageIndexChanged">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                              <%--  <asp:TemplateField HeaderText="Major / Minor Item">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMajorMinorItem" runat="server" CssClass="control-label" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Item Name">
                                    <HeaderStyle CssClass="col-md-4" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" CssClass="control-label" Text='<%#Eval("ContactPerson") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity Purchased">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityPurchased" runat="server" CssClass="control-label" Text='<%#Eval("ContactNo") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Quantity Received">
                                    <HeaderStyle CssClass="col-md-1" />


                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantityReceived" runat="server" required="required" MaxLength="10" onfocus="this.value = this.value;" CssClass="form-control decimalInput required" placeholder="Enter Value" value='<%# Eval("ChannelIndent") %>' ClientIDMode="Static" />
                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" />
                                <asp:HyperLink ID="btnBack" Text="Back" CssClass="btn" runat="server"></asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
