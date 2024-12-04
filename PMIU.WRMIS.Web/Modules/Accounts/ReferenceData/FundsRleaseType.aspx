<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FundsRleaseType.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.FundsRleaseType" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Funds Release Types</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvFundsReleaseType" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvFundsReleaseType_RowCommand"
                                DataKeyNames="ID,IsEdit" OnRowEditing="gvFundsReleaseType_RowEditing" OnRowCancelingEdit="gvFundsReleaseType_RowCancelingEdit" OnRowDeleting="gvFundsReleaseType_RowDeleting"
                                OnRowUpdating="gvFundsReleaseType_RowUpdating" AllowPaging="true" OnRowDataBound="gvFundsReleaseType_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRule" runat="server" CssClass="control-label" Text='<%# Eval("TypeName") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTypeName" runat="server" CssClass="form-control required" placeholder="" Text='<%# Eval("TypeName") %>'
                                                MaxLength="50" required="true" oninput="javascript:ValueValidation(this, '3', '50');" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter Description" Text='<%# Eval("Description") %>'
                                                MaxLength="150" oninput="javascript:ValueValidation(this, '0', '150');" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-8" />
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
