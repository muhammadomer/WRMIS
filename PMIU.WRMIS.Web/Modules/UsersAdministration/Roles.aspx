<%@ Page Title="Roles" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.Roles" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Roles</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">

                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="False" />

                        <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                            ShowHeaderWhenEmpty="True" OnPageIndexChanged="gvRoles_PageIndexChanged" 
                            OnRowCommand="gvRoles_RowCommand" OnRowCancelingEdit="gvRoles_RowCancelingEdit" OnRowUpdating="gvRoles_RowUpdating"
                            OnRowEditing="gvRoles_RowEditing" OnRowDeleting="gvRoles_RowDeleting" AllowPaging="True" PageSize="10" 
                            OnPageIndexChanging="gvRoles_PageIndexChanging" OnRowCreated="gvRoles_RowCreated"  CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Roles">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="true" MaxLength="30" Width="90%" placeholder="Enter Role Name" onkeyup="InputValidation(this)" Text='<%# Eval("Name") %>' ClientIDMode="Static" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="50" Width="90%" placeholder="Enter Role Description" onkeyup="InputValidation(this)" Text='<%# Eval("Description") %>' />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-8" />
                                </asp:TemplateField>


                                <asp:TemplateField>


                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:LinkButton ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:LinkButton>
                                            <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                        </asp:Panel>
                                    </EditItemTemplate>


                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:LinkButton>
                                        </asp:Panel>
                                    </HeaderTemplate>


                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:LinkButton>
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
