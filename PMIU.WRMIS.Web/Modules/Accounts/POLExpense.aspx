<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POLExpense.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.POLExpense" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add POL Expense</asp:Literal>
            </h3>
        </div>
        <div class="box-content">
            <div class="table-responsive">
                <table class="table tbl-info">
                    <tr>
                        <th>Financial Year</th>
                        <th>Month</th>
                        <th>Total Claim (Rs.)</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFinancialYear" runat="server" Text="2016-17" />
                        </td>
                        <td>
                            <asp:Label ID="lblMonth" runat="server" Text="January" />
                        </td>
                        <td>
                            <asp:Label ID="lblTotalClaim" runat="server" Text="12,256" />
                        </td>
                    </tr>
                    <tr>
                        <th>Resource Type</th>
                        <th>Designation</th>
                        <th>Name of Staff</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblResourceType" runat="server" Text="PMIU Field Staff" />
                        </td>
                        <td>
                            <asp:Label ID="lblDesignation" runat="server" Text="ADM" />
                        </td>
                        <td>
                            <asp:Label ID="lblNameOfStaff" runat="server" Text="Muhammad Umer" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblExpenseType" runat="server" Text="Expense Type" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label ID="lblExpenseTypeName" runat="server" CssClass="control-label" Text="POL Receipts" Font-Bold="true" />
                                <asp:HiddenField ID="hdnMonthlyExpenseID" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="dvBillNo" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblPRBillNoText" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label runat="server" ID="lblPRBillNo" CssClass="control-label" Font-Bold="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblVehicle" runat="server" Text="Vehicle" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="form-control required" required="true" TabIndex="1">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblAmount" runat="server" Text="Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control required decimal2PInput" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" required="true" MaxLength="10" TabIndex="2" oninput="javascript:ValueValidation(this,'1','999999');" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblReceiptNo" runat="server" Text="POL Receipt No." CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtReceiptNo" CssClass="form-control required" required="true" MaxLength="10" TabIndex="3" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date-picker-user required" required="true" TabIndex="4" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblMeterReading" runat="server" Text="Meter Reading (KM)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 12px; padding-right: 12px;" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtMeterReading" CssClass="form-control required decimalInput" required="true" MaxLength="10" TabIndex="5" oninput="javascript:ValueValidation(this,'1','999999');" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblReceipt" runat="server" Text="Attach Receipt" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <fup:FileUploadControl runat="server" ID="fupAttachReceipt" Size="1" TabIndex="8" />
                                </div>
                                <div>
                                    <fup:FileUploadControl runat="server" ID="fupAttachReceiptEdit" Size="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnSave_Click" />
                            <asp:LinkButton runat="server" ID="lbtnBack" CssClass="btn" Text="Back" PostBackUrl="~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $('.CtrlClass0').removeAttr('required');

            $('.date-picker-user').datepicker({
                startDate: '<%= ViewState[StartDate].ToString() %>',
                endDate: '<%= ViewState[EndDate].ToString() %>',
                autoclose: true,
                todayHighlight: true,
                language: 'ru'
            });

        });

    </script>
</asp:Content>
