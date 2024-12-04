<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchCheque.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.SearchCheque" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:UpdatePanel runat="server">
        <ContentTemplate>--%>
            <div class="box">
                <div class="box-title">
                    <h3>Search Cheques</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblCheckDateFrom" runat="server" Text="Cheque Date From" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtCheckDateFrom" runat="server" CssClass="form-control date-picker" TabIndex="3" onkeyup="return false;" onkeydown="return false;" autocomplete="off" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <asp:Label ID="lblCheckDateTo" runat="server" Text="To" CssClass="col-sm-2 col-lg-1 control-label" />
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtCheckDateTo" runat="server" CssClass="form-control date-picker" onkeyup="return false;" onkeydown="return false;" autocomplete="off" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblAmountBetween" runat="server" Text="Amount Between (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtAmountBetweenFrom" runat="server" CssClass="form-control decimal2PInput" autocomplete="off" ClientIDMode="Static" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" />
                                        </div>
                                        <asp:Label ID="lblAmountBetweenAnd" runat="server" Text="and" CssClass="col-sm-2 col-lg-1 control-label" />
                                        <div class="col-sm-3 col-lg-4 controls">
                                            <asp:TextBox ID="txtAmountBetweenAnd" runat="server" CssClass="form-control decimal2PInput" autocomplete="off" ClientIDMode="Static" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:HyperLink ID="hlAddNew" Enabled="<%# base.CanAdd %>" runat="server" NavigateUrl="PaymentsAgainstSanctions.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div id="dvChequeGrid" class="table-responsive" style="display: <%= GridDisplay %>;">
                                <asp:GridView ID="gvSearchCheques" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" AllowPaging="true"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" PageSize="10"
                                    OnRowDataBound="gvSearchCheques_RowDataBound" OnPageIndexChanging="gvSearchCheques_PageIndexChanging"
                                    DataKeyNames="ChequeAttachment,ID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeNoGrid" runat="server" CssClass="control-label" Text='<%# Eval("ChequeNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeDate" runat="server" CssClass="control-label" Text='<%# Eval("ChequeDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount (Rs.)" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeAmount" runat="server" CssClass="control-label" Text='<%# Eval("ChequeAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cash Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCashDate" runat="server" CssClass="control-label" Text='<%# Eval("CashDate")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server" CssClass="control-label" Text='<%# Eval("PaymentDate")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Attachment">
                                            <ItemTemplate>
                                                <fup:FileUploadControl runat="server" ID="fupBillAttachment" Size="0" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnAddPaymentDate" runat="server" CssClass="btn btn-primary btn_24 Finilized-Bill" ToolTip="Add Payment Date" OnClick="lbtnAddPaymentDate_Click" />
                                                <asp:HyperLink ID="hlDetail" runat="server" NavigateUrl='<%#String.Format("~/Modules/Accounts/PaymentsAgainstSanctionsView.aspx?SanctionPaymentID={0}", Eval("ID"))%>' CssClass="btn btn-primary btn_32 details" ToolTip="Detail"></asp:HyperLink>
                                                <asp:LinkButton ID="lbtnPrintAcquittenceRoll" runat="server" CssClass="btn btn-primary btn_24 print" ToolTip="Print Acquittence Roll" OnClick="lbtnPrintAcquittenceRoll_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- Start of Payment Date Model -->
                    <div id="AddPaymentDate" class="modal fade">
                        <div class="modal-dialog" style="width: 50%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:HiddenField ID="hdnChequeID" runat="server" />
                                                <asp:Label ID="lblPaymentDateString" runat="server" class="col-sm-4 col-lg-3 control-label" style="padding-left:3px;padding-right:2px;">Payment Date</asp:Label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="date-picker form-control"></asp:TextBox>
                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button CssClass="btn btn-default" runat="server" ID="btnClose" Text="Close" OnClick="btnClose_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End of Payment Date Model -->
                </div>
            </div>
            <script type="text/javascript">

                $(document).ready(function () {                    

                    $('#txtChequeNo').change(function () {
                        $('#dvChequeGrid').hide();
                    });

                    $('#txtCheckDateFrom').change(function () {
                        $('#dvChequeGrid').hide();
                    });

                    $('#txtCheckDateTo').change(function () {
                        $('#dvChequeGrid').hide();
                    });

                    $('#txtAmountBetweenFrom').change(function () {
                        $('#dvChequeGrid').hide();
                    });

                    $('#txtAmountBetweenAnd').change(function () {
                        $('#dvChequeGrid').hide();
                    });

                });

            </script>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
