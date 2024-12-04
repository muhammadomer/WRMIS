<%@ Page Title="Division" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Division.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Division" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Division</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" required="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" Enabled="False" required="true">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvDivision" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnRowDataBound="gvDivision_RowDataBound" OnRowCommand="gvDivision_RowCommand"
                            OnRowCancelingEdit="gvDivision_RowCancelingEdit" OnRowUpdating="gvDivision_RowUpdating"
                            OnRowEditing="gvDivision_RowEditing" OnRowDeleting="gvDivision_RowDeleting" AllowPaging="True"
                            OnPageIndexChanging="gvDivision_PageIndexChanging" CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false" OnPageIndexChanged="gvDivision_PageIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="true" MaxLength="75" placeholder="Enter Division Name" Text='<%# Eval("Name") %>' Width="90%" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Domain">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDomain" runat="server" CssClass="control-label" Text='<%# Eval("CO_Domain.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblDomainID" CssClass="control-label" runat="server" Text='<%# Eval("DomainID") %>' Visible="false" />
                                        <asp:DropDownList ID="ddlDomain" runat="server" required="true" CssClass="form-control required"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="150" placeholder="Enter Division Description" Text='<%# Eval("Description") %>' Width="90%" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-7" />
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
