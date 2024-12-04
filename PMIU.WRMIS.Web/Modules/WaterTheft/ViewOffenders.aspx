<%@ Page Title="View Offenders" EnableEventValidation="false" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="ViewOffenders.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ViewOffenders" %>

<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>View Offenders</h3>
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <div class="row" id="gvOffenders" runat="server">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvOffender" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                        CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvOffender_PageIndexChanged" OnPageIndexChanging="gvOffender_PageIndexChanging"
                                        OnRowCommand="gvOffender_RowCommand" OnRowCancelingEdit="gvOffender_RowCancelingEdit" OnRowCreated="gvOffender_RowCreated"
                                        OnRowEditing="gvOffender_RowEditing" OnRowDeleting="gvOffender_RowDeleting" OnRowUpdating="gvOffender_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Offender Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("OffenderName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control required" required="true" MaxLength="30" Width="90%" Text='<%# Eval("OffenderName") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NIC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCNIC" runat="server" Text='<%# Eval("CNIC") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control" MaxLength="30" Width="90%" Text='<%# Eval("CNIC") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control required" required="true" MaxLength="30" Width="90%" Text='<%# Eval("Address") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
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

                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
