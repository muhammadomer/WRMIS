<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpillwayTypeSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallSpillways.ReferenceData.SpillwayTypeSD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%@ MasterType VirtualPath="~/Site.Master" %>

    <div class="box">
        <div class="box-title">
            <h3>Spillway Type</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">

            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvSpillwayType" runat="server" DataKeyNames="SpillwayTypeID,Name,Description,IsActive,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvSpillwayType_RowCommand" OnRowDataBound="gvSpillwayType_RowDataBound"
                            OnRowEditing="gvSpillwayType_RowEditing" OnRowCancelingEdit="gvSpillwayType_RowCancelingEdit"
                            OnRowUpdating="gvSpillwayType_RowUpdating"
                            OnRowDeleting="gvSpillwayType_RowDeleting" OnPageIndexChanging="gvSpillwayType_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Spillway ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpillwayID" runat="server" Text='<%#Eval("SpillwayTypeID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Spillway Type">
                                    <HeaderStyle  CssClass="col-md-2"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpillwayName" runat="server" Text='<%# Eval("Name") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSpillwayName" runat="server" MinLength="3" MaxLength="30" required="true" class="required form-control" Text='<%# Eval("Name") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-md-4" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" class="form-control" Text='<%# Eval("Description") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                    <ItemStyle  CssClass="text-center"/>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddSpillway" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddSpillway" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddSpillway" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionSpillway" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditSpillway" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteSpillway" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionSpillway" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveSpillway" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelSpillway" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
