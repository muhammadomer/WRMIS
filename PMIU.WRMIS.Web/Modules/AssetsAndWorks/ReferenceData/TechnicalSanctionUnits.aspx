﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TechnicalSanctionUnits.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData.TechnicalSanctionUnits" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Technical Sanction Units</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="100"
                            OnRowCommand="gvUnits_RowCommand" OnPageIndexChanging="gvUnits_PageIndexChanging"
                            OnRowUpdating="gvUnits_RowUpdating" OnRowDeleting="gvUnits_RowDeleting"
                            OnRowCancelingEdit="gvUnits_RowCancelingEdit" OnRowCreated="gvUnits_RowCreated"
                            OnRowEditing="gvUnits_RowEditing" OnPageIndexChanged="gvUnits_PageIndexChanged"
                            EmptyDataText="No record found" CssClass="table header" BorderWidth="0px" 
                            CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="10" onfocus="this.value = this.value;" CssClass="form-control required" placeholder="Enter Unit" value='<%# Eval("Name") %>' Width="90%"/>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description" Visible="true">
                                    <HeaderStyle CssClass="col-md-5" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" CssClass="form-control" placeholder="Enter Description" value='<%# Eval("Description") %>' Onkeyup="InputValidationText(this);" Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="cb_Active" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-1" />
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
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </asp:Panel>
                                    </EditItemTemplate>
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

