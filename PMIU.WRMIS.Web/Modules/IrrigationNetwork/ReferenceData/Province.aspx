﻿<%@ Page Title="Province" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Province.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Province" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Province</h3>
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvProvince" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowCommand="gvProvince_RowCommand"
                            OnRowCancelingEdit="gvProvince_RowCancelingEdit" OnRowUpdating="gvProvince_RowUpdating" OnRowEditing="gvProvince_RowEditing"
                            OnRowDeleting="gvProvince_RowDeleting" AllowPaging="True" OnPageIndexChanging="gvProvince_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvProvince_PageIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Province Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvinceName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="true" MaxLength="30" placeholder="Enter Province Name" Text='<%# Eval("Name") %>' Width="90%" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvinceDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="100" placeholder="Enter Province Description" Text='<%# Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-8" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
