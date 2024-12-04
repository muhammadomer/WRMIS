<%@ Page Title="Structure" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Structure.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Structure" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Structure</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblProvince" runat="server" Text="Province" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlProvince" runat="server" required="true" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblRiver" runat="server" Text="River" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlRiver" runat="server" required="true" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlRiver_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="display: none;">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:Button runat="server" ID="btnLoad" Text="Search" OnClick="btnLoad_Click" CssClass="btn btn-primary btn_t_32 search" ToolTip="Search" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <b>
                        <asp:Literal ID="litGridTitle" runat="server" Visible="false">Structure at River RD</asp:Literal>
                    </b>
                    <hr>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvStructure" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowDataBound="gvStructure_RowDataBound" OnRowCommand="gvStructure_RowCommand"
                            OnRowCancelingEdit="gvStructure_RowCancelingEdit" OnRowUpdating="gvStructure_RowUpdating"
                            OnRowEditing="gvStructure_RowEditing" OnRowDeleting="gvStructure_RowDeleting" AllowPaging="True"
                            OnPageIndexChanging="gvStructure_PageIndexChanging" CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false" OnPageIndexChanged="gvStructure_PageIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Structure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStructureName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="true" MaxLength="30" CssClass="form-control required" placeholder="Enter Structure Name" Text='<%# Eval("Name") %>' Width="90%" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-6" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Structure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStructureType" runat="server" CssClass="control-label" Text='<%# Eval("CO_StructureType.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 0px;">
                                            <asp:Label ID="lblStructureTypeID" CssClass="control-label" runat="server" Text='<%# Eval("StructureTypeID") %>' Visible="false" />
                                            <asp:DropDownList ID="ddlStructureType" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                        </div>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-5" />
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
