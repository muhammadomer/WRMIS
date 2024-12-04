<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DamTypeSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.ReferenceData.DamTypeSD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%@ MasterType VirtualPath="~/Site.Master" %>

    <div class="box">
        <div class="box-title">
            <h3>Dam Type</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvDamType" runat="server" DataKeyNames="DamTypeID,Name,Description,IsActive,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvDamType_RowCommand" OnRowDataBound="gvDamType_RowDataBound"
                            OnRowEditing="gvDamType_RowEditing" OnRowCancelingEdit="gvDamType_RowCancelingEdit"
                            OnRowUpdating="gvDamType_RowUpdating"
                            OnRowDeleting="gvDamType_RowDeleting" OnPageIndexChanging="gvDamType_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Dam ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDamID" runat="server" Text='<%#Eval("DamTypeID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dam Type">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDamName" runat="server" Text='<%# Eval("Name") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDamName" runat="server" MinLength="3" MaxLength="50" required="true" class="required form-control" Text='<%# Eval("Name") %>' />
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-md-4" />
                                    <ItemStyle CssClass="col-md-4" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" class="form-control" Text='<%# Eval("Description") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-1  text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="col-md-1  text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddDam" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddDam" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddDam" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionDam" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditDam" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteDam" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionDam" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveDam" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelDam" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--onclick="history.go(-1);return false;"--%>
            <%--<div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
</asp:Content>
