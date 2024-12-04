<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TADAExpense.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.TADAExpense" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add TADA Expense</asp:Literal>
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
                                <asp:Label ID="lblExpenseTypeName" runat="server" CssClass="control-label" Text="TA/DA" Font-Bold="true" />
                                <asp:HiddenField ID="hdnMonthlyExpenseID" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnResourceAllocationID" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="dvBillNo" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblTDABillNoText" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label runat="server" ID="lblTDABillNo" CssClass="control-label" Font-Bold="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblEndReading" runat="server" Text="End Reading (km)" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtEndReading" CssClass="form-control decimalInput" MaxLength="10" TabIndex="1" oninput="javascript:ValueValidation(this,'1','999999');" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblAttachTADAProforma" runat="server" Text="Attach TA/DA Proforma" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 1px; padding-right: 1px;" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <fup:FileUploadControl runat="server" ID="fupAttachTADAProforma" Size="1" TabIndex="2" />
                                </div>
                                <div>
                                    <fup:FileUploadControl runat="server" ID="fupAttachTADAProformaEdit" Size="0" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblMiscExpenses" runat="server" Text="Misc. Expenses (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 12px; padding-right: 12px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtMiscExpenses" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput" MaxLength="10" TabIndex="3" OnTextChanged="txtMiscExpenses_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <div class="form-group">
                                    <asp:Label ID="lblNoOfDays" runat="server" Text="No. of Days Travelled" Font-Bold="true" Font-Underline="true" CssClass="control-label" />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblOrdinaryHalfDailies" runat="server" Text="Ordinary Half Dailies" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 10px; padding-right: 9px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtOrdinaryHalfDailies" CssClass="form-control required integerInput" required="true" MaxLength="2" TabIndex="4" OnTextChanged="txtOrdinaryHalfDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblOrdinaryFullDailies" runat="server" Text="Ordinary Full Dailies" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 10px; padding-right: 9px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtOrdinaryFullDailies" CssClass="form-control required integerInput" required="true" MaxLength="2" TabIndex="5" OnTextChanged="txtOrdinaryFullDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblSpecialHalfDailies" runat="server" Text="Special Half Dailies" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtSpecialHalfDailies" CssClass="form-control required integerInput" required="true" MaxLength="2" TabIndex="6" OnTextChanged="txtSpecialHalfDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblSpecialFullDailies" runat="server" Text="Special Full Dailies" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtSpecialFullDailies" CssClass="form-control required integerInput" required="true" MaxLength="2" TabIndex="7" OnTextChanged="txtSpecialFullDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <div class="form-group">
                                    <asp:Label ID="lblTotalKMTravelled" runat="server" Text="Total KM Travelled" Font-Bold="true" Font-Underline="true" CssClass="control-label" />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblPublicTransport" runat="server" Text="Public Transport" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtPublicTransport" CssClass="form-control required decimalInput" required="true" MaxLength="10" TabIndex="8" OnTextChanged="txtPublicTransport_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblOfficialVehicle" runat="server" Text="Official Vehicle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtOfficialVehicle" CssClass="form-control required decimalInput" required="true" MaxLength="10" TabIndex="9" OnTextChanged="txtOfficialVehicle_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblTAAmount" runat="server" Text="TA Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtTAAmount" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput disabled" Enabled="false" Font-Bold="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDAAmount" runat="server" Text="DA Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtDAAmount" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput disabled" Enabled="false" Font-Bold="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblClaimAmount" runat="server" Text="Claim Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 15px; padding-right: 14px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtClaimAmount" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput disabled" Enabled="false" Font-Bold="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeNumericValidation();
                    }
                });
            };

        });

    </script>
</asp:Content>
