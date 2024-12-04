<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TaxRates.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.TaxRates" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Tax Rates</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblTransactionType" runat="server" Text="Transaction Type" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                    <asp:ListItem Text="Purchase" Value="Purchase" />
                                    <asp:ListItem Text="Repair" Value="Repair" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblVendorType" runat="server" Text="Vendor Type" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlVendorType" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVendorType_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                    <asp:ListItem Text="Filer" Value="Filer" />
                                    <asp:ListItem Text="Non Filer" Value="Non Filer" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvTaxRates" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID" OnRowEditing="gvTaxRates_RowEditing" OnRowCancelingEdit="gvTaxRates_RowCancelingEdit"
                                OnRowUpdating="gvTaxRates_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tax Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxType" runat="server" CssClass="control-label" Text='<%# Eval("TaxType") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-6" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax Rate in Percentage (%)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxRatePercentage" runat="server" CssClass="control-label" Text='<%# Eval("TaxRateInPercentage") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTaxRatePercentage" runat="server" CssClass="form-control decimal2PInput required" required="true" placeholder="Enter Tax Rate Percentage" Text='<%# Eval("TaxRateInPercentage") %>' MaxLength="6" autocomplete="off" oninput="javascript:ValueValidation(this, '0', '100');" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-5" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
