<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AccountSetup.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.AccountSetup" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Account Setup</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAccountSetup" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID,ACRule" OnRowEditing="gvAccountSetup_RowEditing" OnRowDataBound="gvAccountSetup_RowDataBound" OnRowCancelingEdit="gvAccountSetup_RowCancelingEdit"
                                OnRowUpdating="gvAccountSetup_RowUpdating" AllowPaging="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rule">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRule" runat="server" CssClass="control-label" Text='<%# Eval("ACRule") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-6" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" CssClass="control-label" Text='<%# Eval("Amount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("Amount"))) : Eval("Amount") %>'/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control required" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" placeholder="" Text='<%# Eval("Amount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("Amount"))) : Eval("Amount") %>'
                                                required="true" MaxLength="8" oninput="javascript:ValueValidation(this, '1', '99999999');" />
                                            <asp:Label ID="hlblAmount" Visible="false" runat="server" CssClass="control-label" Text='<%# Eval("Amount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("Amount"))) : Eval("Amount") %>' />
                                        </EditItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="col-md-5 text-center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditTemplate" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnEdit" runat="server" CommandName="edit" CssClass="btn btn-primary btn_24 edit" CommandArgument='<%# Eval("ID") %>' ToolTip="Edit"></asp:Button>
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-center" />
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
