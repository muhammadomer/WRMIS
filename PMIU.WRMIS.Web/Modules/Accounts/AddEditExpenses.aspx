<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditExpenses.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.AddEditExpenses" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add Expenses</asp:Literal>
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
                                <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlExpenseType_SelectedIndexChanged" TabIndex="1">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnMonthlyExpenseID" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="dvObjectClassification" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblObjectClassification" runat="server" Text="Object/Classification" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlObjectClassification" runat="server" CssClass="form-control required" required="true" TabIndex="2">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlRepairMaintainence" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRMBillNo" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtRMBillNo" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRMBillDate" runat="server" Text="Bill Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtRMBillDate" runat="server" CssClass="form-control date-picker" TabIndex="3" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblAsset" runat="server" Text="Asset" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlAsset" runat="server" CssClass="form-control required" required="true" TabIndex="4">
                                        <asp:ListItem Text="Select" Value="" Selected="True" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRMPurchaseAmount" runat="server" Text="Purchase Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 3px; padding-right: 4px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtRMPurchaseAmount" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="5" OnTextChanged="txtRMPurchaseAmount_TextChanged" AutoPostBack="true" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRepairAmount" runat="server" Text="Repair Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-right: 8px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtRepairAmount" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="6" OnTextChanged="txtRepairAmount_TextChanged" AutoPostBack="true" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtTotalAmount" CssClass="form-control decimal2PInput disabled" Enabled="false" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRMApprovalReference" runat="server" Text="Approval Reference" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-right: 10px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtRMApprovalReference" CssClass="form-control" MaxLength="10" TabIndex="7" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblRMAttachBill" runat="server" Text="Attach Bill" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <fup:FileUploadControl runat="server" ID="fupRMAttachBill" Size="1" TabIndex="8" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row" style="margin-top: 10px;">
                        <div class="col-md-1"></div>
                        <div class="col-md-4">
                            <div class="col-lg-12 form-group">
                                <span>Key Parts</span>
                            </div>
                            <div class="col-lg-12 form-group">
                                <asp:ListBox runat="server" data-title="keyparts" ID="lbKeyPart" TabIndex="9" SelectionMode="Multiple" class="unselected form-control" Style="height: 200px; width: 100%;" />
                            </div>
                        </div>
                        <div style="margin-top: 85px" class="col-md-2 center-block">
                            <div class="col-md-8 col-lg-9 controls">
                                <button id="btnAdd" runat="server" style="margin-bottom: 16px;" data-type="str" class="str btn btn-default col-md-8 col-md-offset-2" type="button" onserverclick="btnAdd_ServerClick"><span class="glyphicon glyphicon-chevron-right"></span></button>
                            </div>
                            <div class="col-md-8 col-lg-9 controls">
                                <button id="btnRemove" runat="server" style="margin-bottom: 10px;" data-type="stl" class="stl btn btn-default col-md-8 col-md-offset-2" type="button" onserverclick="btnRemove_ServerClick"><span class="glyphicon glyphicon-chevron-left"></span></button>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="col-lg-12 form-group">
                                <span>Assigned Key Parts</span>
                            </div>
                            <div class="col-lg-12 form-group">
                                <asp:ListBox runat="server" data-title="assignedkeyparts" TabIndex="10" ID="lbAssignedKeyPart" SelectionMode="Multiple" class="selected form-control" Style="height: 200px; width: 100%;" />
                            </div>
                        </div>
                        <div class="col-md-1"></div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlPOLReceipts" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblPRBillNo" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtPRBillNo" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblVehicle" runat="server" Text="Vehicle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="form-control required" required="true" TabIndex="3">
                                        <asp:ListItem Text="Select" Value="" Selected="True" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblAmount" runat="server" Text="Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="4" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblReceiptNo" runat="server" Text="POL Receipt No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtReceiptNo" CssClass="form-control required integerInput" required="true" MaxLength="10" TabIndex="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="6" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblMeterReading" runat="server" Text="Meter Reading (KM)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtMeterReading" CssClass="form-control required decimalInput" required="true" MaxLength="10" TabIndex="7" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
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
                </asp:Panel>
                <asp:Panel ID="pnlTADA" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblTDABillNo" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtTDABillNo" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblEndReading" runat="server" Text="End Reading (km)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtEndReading" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="3" />
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
                                    <asp:TextBox runat="server" ID="txtOrdinaryHalfDailies" CssClass="form-control required integerInput" required="true" MaxLength="10" TabIndex="4" OnTextChanged="txtOrdinaryHalfDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblOrdinaryFullDailies" runat="server" Text="Ordinary Full Dailies" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 10px; padding-right: 9px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtOrdinaryFullDailies" CssClass="form-control required integerInput" required="true" MaxLength="10" TabIndex="5" OnTextChanged="txtOrdinaryFullDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblSpecialHalfDailies" runat="server" Text="Special Half Dailies" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtSpecialHalfDailies" CssClass="form-control required integerInput" required="true" MaxLength="10" TabIndex="6" OnTextChanged="txtSpecialHalfDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblSpecialFullDailies" runat="server" Text="Special Full Dailies" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtSpecialFullDailies" CssClass="form-control required integerInput" required="true" MaxLength="10" TabIndex="7" OnTextChanged="txtSpecialFullDailies_TextChanged" AutoPostBack="true" autocomplete="off" />
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
                                <asp:Label ID="lblMiscExpenses" runat="server" Text="Misc. Expenses (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 12px; padding-right: 12px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtMiscExpenses" CssClass="form-control decimal2PInput" MaxLength="10" TabIndex="10" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblAttachTADAProforma" runat="server" Text="Attach TA/DA Proforma" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 1px; padding-right: 1px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <fup:FileUploadControl runat="server" ID="fupAttachTADAProforma" Size="1" TabIndex="11" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblTAAmount" runat="server" Text="TA Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtTAAmount" CssClass="form-control decimal2PInput disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblDAAmount" runat="server" Text="DA Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtDAAmount" CssClass="form-control decimal2PInput disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblClaimAmount" runat="server" Text="Claim Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 15px; padding-right: 14px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtClaimAmount" CssClass="form-control decimal2PInput disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlNewPurchase" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNPBillNo" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtNPBillNo" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNPBillDate" runat="server" Text="Bill Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtNPBillDate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="3" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblPurchaseItemName" runat="server" Text="Purchase Item Name" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 4px; padding-right: 3px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtPurchaseItemName" CssClass="form-control" MaxLength="100" TabIndex="4" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNPPurchaseAmount" runat="server" Text=" Purchase Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 9px; padding-right: 8px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtNPPurchaseAmount" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNPApprovalReference" runat="server" Text="Approval Reference" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtNPApprovalReference" CssClass="form-control" MaxLength="50" TabIndex="6" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNPAttachBill" runat="server" Text="Attach Bill" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <fup:FileUploadControl runat="server" ID="fupNPAttachBill" Size="1" TabIndex="7" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlOtherExpense" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblOEBillNo" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtOEBillNo" CssClass="form-control disabled" Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblOEBillDate" runat="server" Text="Bill Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtOEBillDate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="3" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblExpenseName" runat="server" Text="Expense Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtExpenseName" CssClass="form-control" TabIndex="4" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblExpenseAmount" runat="server" Text="Expense Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 6px; padding-right: 6px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtExpenseAmount" CssClass="form-control required decimal2PInput" required="true" TabIndex="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblOEApprovalReference" runat="server" Text="Approval Reference" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 13px; padding-right: 12px;" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtOEApprovalReference" CssClass="form-control" TabIndex="6" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblOEAttachBill" runat="server" Text="Attach Receipt" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <div class="input-group">
                                        <fup:FileUploadControl runat="server" ID="fupOEAttachBill" Size="1" TabIndex="7" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlQuotation" runat="server" Visible="false" Style="margin-top: 10px;">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblNoOfQuotation" runat="server" Text="No. of Quotations" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlNoOfQuotations" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlNoOfQuotations_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="3" Value="3" Selected="True" />
                                        <asp:ListItem Text="4" Value="4" />
                                        <asp:ListItem Text="5" Value="5" />
                                        <asp:ListItem Text="6" Value="6" />
                                        <asp:ListItem Text="7" Value="7" />
                                        <asp:ListItem Text="8" Value="8" />
                                        <asp:ListItem Text="9" Value="9" />
                                        <asp:ListItem Text="10" Value="10" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="gvQuotation" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Vendor Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVendorName" runat="server" CssClass="form-control required" placeholder="Enter Vendor Name"
                                                MaxLength="200" autocomplete="off" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" required="true" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quotation Date">
                                        <ItemTemplate>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control date-picker required" required="true" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quoted Price (Rs)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuotedPrice" runat="server" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price"
                                                MaxLength="6" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '99999');" required="true" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quoted Price with Tax (Rs)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuotedTaxPrice" runat="server" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price with Tax"
                                                MaxLength="6" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '99999');" required="true" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Attachment">
                                        <ItemTemplate>
                                            <fup:FileUploadControl runat="server" ID="fupAttachQuotation" Size="1" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlSave" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnSave_Click" Visible="false" />
                                <asp:LinkButton runat="server" ID="lbtnBack" CssClass="btn" Text="Back" PostBackUrl="~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $("input[name*='$fupRMAttachBill$']").removeAttr('required');
            $("input[name*='$fupAttachReceipt$']").removeAttr('required');
            $("input[name*='$fupAttachReceiptEdit$']").removeAttr('required');
            $("input[name*='$fupAttachTADAProforma$']").removeAttr('required');
            $("input[name*='$fupNPAttachBill$']").removeAttr('required');
            $("input[name*='$fupOEAttachBill$']").removeAttr('required');
        });

    </script>
</asp:Content>
