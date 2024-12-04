<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewPurchaseExpense.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.NewPurchaseExpense" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add New Purchase Expense</asp:Literal>
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
                                <asp:Label ID="lblExpenseTypeName" runat="server" CssClass="control-label" Text="New Purchase" Font-Bold="true" />
                                <asp:HiddenField ID="hdnMonthlyExpenseID" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="dvBillNo" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblNPBillNoText" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label runat="server" ID="lblNPBillNo" CssClass="control-label" Font-Bold="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblNPBillDate" runat="server" Text="Bill Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtNPBillDate" runat="server" CssClass="form-control date-picker-user required" required="true" TabIndex="1" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblPurchaseItemName" runat="server" Text="Purchase Item Name" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 9px; padding-right: 8px;" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtPurchaseItemName" CssClass="form-control required" MaxLength="50" TabIndex="2" required="true" oninput="javascript:InputWithLengthValidation(this, 3)" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblNPApprovalReference" runat="server" Text="Approval Reference" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 13px; padding-right: 12px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtNPApprovalReference" CssClass="form-control" MaxLength="150" TabIndex="3" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblNPPurchaseAmount" runat="server" Text="Purchase Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 4px; padding-right: 3px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtNPPurchaseAmount" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" TabIndex="4" OnTextChanged="txtNPPurchaseAmount_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlQuotation" runat="server" Visible="false" Style="margin-top: 10px;">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblNoOfQuotation" runat="server" Text="No. of Quotations" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlNoOfQuotations" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlNoOfQuotations_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" Selected="True" />
                                                <asp:ListItem Text="3" Value="3" />
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
                                                        MaxLength="150" autocomplete="off" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" required="true" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quotation Date">
                                                <ItemTemplate>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control date-picker-user required" required="true" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quoted Price (Rs)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuotedPrice" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control decimal2PInput" placeholder="Enter Quoted Price"
                                                        MaxLength="10" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '999999');" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quoted Price with Tax (Rs)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuotedTaxPrice" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price with Tax"
                                                        MaxLength="10" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '999999');" required="true" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Attachment">
                                                <ItemTemplate>
                                                    <fup:FileUploadControl runat="server" ID="fupAttachQuotation" Size="1" Name="BidCtrl" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlQuotationEdit" runat="server" Visible="false" Style="margin-top: 10px;">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvQuotationEdit" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                        OnRowDeleting="gvQuotationEdit_RowDeleting" DataKeyNames="ID,QuotedPriceAttachment" OnRowDataBound="gvQuotationEdit_RowDataBound"
                                        OnRowCommand="gvQuotationEdit_RowCommand" OnRowCancelingEdit="gvQuotationEdit_RowCancelingEdit" OnRowUpdating="gvQuotationEdit_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Vendor Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendorName" runat="server" CssClass="control-label" Text='<%# Eval("VendorName") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="form-control required" placeholder="Enter Vendor Name"
                                                        MaxLength="150" autocomplete="off" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" required="true" />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quotation Date">
                                                <EditItemTemplate>
                                                    <div class="input-group" runat="server" id="dvQuotationDate">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control date-picker-user required" required="true" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                                    </div>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuotationDate" runat="server" CssClass="control-label" Text='<%# Eval("QuotationDate") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quoted Price (Rs)">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtQuotedPrice" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control decimal2PInput" placeholder="Enter Quoted Price"
                                                        MaxLength="10" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '999999');" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuotedPrice" runat="server" CssClass="control-label" Text='<%# Eval("QuotedPrice") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quoted Price with Tax (Rs)">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtQuotedTaxPrice" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price with Tax"
                                                        MaxLength="10" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '999999');" required="true" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuotedTaxPrice" runat="server" CssClass="control-label" Text='<%# Eval("QuotedPriceWithTax") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Attachment">
                                                <EditItemTemplate>
                                                    <fup:FileUploadControl runat="server" ID="fupAttachQuotation" Size="1" Name="BidCtrl" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <fup:FileUploadControl runat="server" ID="fupAttachQuotationEdit" Size="0" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1 text-center" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblNPAttachBill" runat="server" Text="Attach Bill" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <fup:FileUploadControl runat="server" ID="fupNPAttachBill" Size="1" tabindex="5" Name="FormCtrl" />
                                </div>
                                <div>
                                    <fup:FileUploadControl runat="server" ID="fupNPAttachBillEdit" Size="0" />
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

            $('.FormCtrl0').removeAttr('required');
            $('.BidCtrl0').removeAttr('required');

            $('.date-picker-user').datepicker({
                startDate: '<%= ViewState[StartDate].ToString() %>',
                endDate: '<%= ViewState[EndDate].ToString() %>',
                autoclose: true,
                todayHighlight: true,
                language: 'ru'
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeNumericValidation();
                        $('.FormCtrl0').removeAttr('required');
                        $('.BidCtrl0').removeAttr('required');

                        $('.date-picker-user').datepicker({
                            startDate: '<%= ViewState[StartDate].ToString() %>',
                            endDate: '<%= ViewState[EndDate].ToString() %>',
                            autoclose: true,
                            todayHighlight: true,
                            language: 'ru'
                        });
                    }
                });
            };

        });

    </script>
</asp:Content>
