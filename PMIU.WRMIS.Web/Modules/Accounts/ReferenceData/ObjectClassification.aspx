<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ObjectClassification.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.ObjectClassification" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Data - Object/Classification</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <br />
                <asp:HiddenField runat="server" ID="hfAccountHeadID" Value="0" />
                <div class="row" id="searchDiv">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Account Head</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlAccountsHead" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Text="ALL" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvObjectClassification" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvObjectClassification_RowCommand"
                                DataKeyNames="ID" OnRowEditing="gvObjectClassification_RowEditing" OnRowCancelingEdit="gvObjectClassification_RowCancelingEdit" OnRowDeleting="gvObjectClassification_RowDeleting"
                                OnRowUpdating="gvObjectClassification_RowUpdating" AllowPaging="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountHeadID" runat="server" CssClass="control-label" Text='<%# Eval("AccountHeadID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Accounts Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountsCode" runat="server" CssClass="control-label" Text='<%# Eval("AccountsCode") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control required" placeholder="" Text='<%# Eval("AccountsCode") %>'
                                                MaxLength="10" required="true" oninput="javascript:InputWithLengthValidation(this, '3');" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-6" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Object/Classification">
                                        <ItemTemplate>
                                            <asp:Label ID="lblObjectClassification" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassification") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtObjectClassification" runat="server" CssClass="form-control required" placeholder="" Text='<%# Eval("ObjectClassification") %>'
                                                MaxLength="50" required="true" oninput="javascript:InputWithLengthValidation(this, '3');" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-5" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete"></asp:Button>
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditTemplate" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                            </asp:Panel>
                                        </EditItemTemplate>


                                        <HeaderStyle CssClass="col-md-1" />
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
