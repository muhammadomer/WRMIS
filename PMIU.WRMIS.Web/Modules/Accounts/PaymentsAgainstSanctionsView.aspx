<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentsAgainstSanctionsView.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.PaymentsAgainstSanctionsView" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>View Detail of Payments against Sanctions</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="30" required="true" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblChequeDate" runat="server" Text=" Cheque Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtChequeDate" TabIndex="5" runat="server" required="true" CssClass="form-control date-picker" size="16" type="text" Enabled="false"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblChequeAmount" runat="server" Text="Cheque Amount" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtChequeAmount" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" MaxLength="30" required="true" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblAttachBill" runat="server" Text="Attach Cheque" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <fup:FileUploadControl runat="server" ID="fupBillAttachment" Size="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCashDate" runat="server" Text="Cash Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtCashDate" TabIndex="5" runat="server" required="true" CssClass="form-control date-picker" size="16" type="text" Enabled="false"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">

                            <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                                EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" BorderWidth="0px" CellSpacing="-1" GridLines="None" CssClass="table header"
                                OnRowDataBound="gvPayments_RowDataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonth" runat="server" CssClass="control-label" Text='<%# Eval("Month") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sanction No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionNo" runat="server" CssClass="control-label" Text='<%# Eval("SanctionNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sanction Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionType" runat="server" CssClass="control-label" Text='<%# Eval("ExpenseType") %>'></asp:Label>
                                            <asp:Label ID="lblSanctionTypeName" runat="server" CssClass="control-label" Text='<%# Eval("SanctionTypeName") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Object / Classification">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedOn" runat="server" CssClass="control-label" Text='<%# Eval("AccountsCode") +" ("+Eval("ObjectClassification") + ")"  %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sanctioned Amount (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedAmount" runat="server" CssClass="control-label" Text='<%# PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("SanctionAmount"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>

                                </Columns>

                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />

                            </asp:GridView>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $("input[name*='$fupAttachBill$']").removeAttr('required');
        });

    </script>
</asp:Content>
