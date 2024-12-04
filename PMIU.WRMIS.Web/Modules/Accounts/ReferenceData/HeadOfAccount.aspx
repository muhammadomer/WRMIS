<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HeadOfAccount.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.HeadOfAccount" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Head of Account</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAccountHead" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID" OnRowEditing="gvAccountHead_RowEditing" OnRowCancelingEdit="gvAccountHead_RowCancelingEdit"
                                OnRowUpdating="gvAccountHead_RowUpdating" AllowPaging="true" OnPageIndexChanging="gvAccountHead_PageIndexChanging"
                                OnRowCommand="gvAccountHead_RowCommand" OnRowDeleting="gvAccountHead_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Head Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeadName" runat="server" CssClass="control-label" Text='<%# Eval("HeadName") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtHeadName" runat="server" CssClass="form-control required" placeholder="Enter Head Name" Text='<%# Eval("HeadName") %>'
                                                MaxLength="50" autocomplete="off" onfocus="this.value = this.value;" oninput="javascript:InputWithLengthValidation(this, '3');" required="true" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-5" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter Description" Text='<%# Eval("Description") %>'
                                                MaxLength="150" autocomplete="off" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-6" />
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
