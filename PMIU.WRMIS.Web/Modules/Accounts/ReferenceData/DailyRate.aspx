<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyRate.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.DailyRate" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Daily Rate</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvDailyRate" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                DataKeyNames="ID" OnRowEditing="gvDailyRate_RowEditing" OnRowCancelingEdit="gvDailyRate_RowCancelingEdit"
                                OnRowUpdating="gvDailyRate_RowUpdating" AllowPaging="true" OnPageIndexChanging="gvDailyRate_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="BPS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBPS" runat="server" CssClass="control-label" Text='<%# Eval("BPS") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinary Rate (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrdinaryRate" runat="server" CssClass="control-label" Text='<%# Eval("OrdinaryRate") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("OrdinaryRate"))) : Eval("OrdinaryRate") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOrdinaryRate" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control integerInput required" placeholder="Enter Ordinary Rate" Text='<%# Eval("OrdinaryRate") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("OrdinaryRate"))) : Eval("OrdinaryRate") %>'
                                                MaxLength="5" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '99999');" required="true" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-4 text-center" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Special Rate (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSpecialRate" runat="server" CssClass="control-label" Text='<%# Eval("SpecialRate") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("SpecialRate"))) : Eval("SpecialRate") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSpecialRate" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control integerInput required" placeholder="Enter Special Rate" Text='<%# Eval("SpecialRate") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("SpecialRate"))) : Eval("SpecialRate") %>'
                                                MaxLength="5" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '99999');" required="true" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-4 text-center" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
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
