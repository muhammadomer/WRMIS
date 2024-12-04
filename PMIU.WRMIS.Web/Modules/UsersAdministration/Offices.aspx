<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Offices.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.Offices" EnableEventValidation="false"%>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Organization</h3>
                    
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvOffice" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            ShowHeaderWhenEmpty="True" OnRowCommand="gvOffice_RowCommand"
                            OnRowCancelingEdit="gvOffice_RowCancelingEdit" OnRowUpdating="gvOffice_RowUpdating" OnRowEditing="gvOffice_RowEditing"
                            OnRowDeleting="gvOffice_RowDeleting" AllowPaging="True" OnPageIndexChanging="gvOffice_PageIndexChanging"
                            OnPageIndexChanged="gvOffice_PageIndexChanged" OnRowCreated="gvOffice_RowCreated"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Organization Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficeName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOfficeName" runat="server" CssClass="form-control required" onfocus="this.value = this.value;" MaxLength="30" required="true" placeholder="Enter Organization Name" Text='<%# Eval("Name") %>' Width="90%" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficeDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOfficeDesc" runat="server" CssClass="form-control" MaxLength="100" placeholder="Enter Organization Description" Text='<%# Eval("Description") %>' Width="90%" />
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
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
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

