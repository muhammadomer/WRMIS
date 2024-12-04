<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeyParts.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.KeyParts" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Key Parts</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblVehicleType" runat="server" Text="Vehicle Type" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleType_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvKeyPart" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID" OnRowEditing="gvKeyPart_RowEditing" OnRowCancelingEdit="gvKeyPart_RowCancelingEdit"
                                OnRowUpdating="gvKeyPart_RowUpdating" AllowPaging="true" OnPageIndexChanging="gvKeyPart_PageIndexChanging"
                                OnRowCommand="gvKeyPart_RowCommand" OnRowDeleting="gvKeyPart_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Part Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartName" runat="server" CssClass="control-label" Text='<%# Eval("PartName") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPartName" runat="server" CssClass="form-control required" placeholder="Enter Part Name" Text='<%# Eval("PartName") %>'
                                                MaxLength="50" autocomplete="off" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" required="true" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-11" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                        <ItemStyle CssClass="text-center" />
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
</asp:Content>
