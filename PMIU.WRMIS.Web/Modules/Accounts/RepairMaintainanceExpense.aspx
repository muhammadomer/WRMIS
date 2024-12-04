<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepairMaintainanceExpense.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.RepairMaintainanceExpense" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="fup" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add Repair & Maintainence Expense</asp:Literal>
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
                                <asp:Label ID="lblExpenseTypeName" runat="server" CssClass="control-label" Text="Repair & Maintainance" Font-Bold="true" />
                                <asp:HiddenField ID="hdnMonthlyExpenseID" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="dvBillNo" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblRMBillNoText" runat="server" Text="Bill No" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label runat="server" ID="lblRMBillNo" CssClass="control-label" Font-Bold="true" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblRMBillDate" runat="server" Text="Bill Date" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtRMBillDate" runat="server" CssClass="form-control date-picker-user required" required="true" TabIndex="1" onkeyup="return false;" onkeydown="return false;" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblRMApprovalReference" runat="server" Text="Approval Reference" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-right: 10px;" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtRMApprovalReference" CssClass="form-control" MaxLength="150" TabIndex="2" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblRMPurchaseAmount" runat="server" Text="Purchase Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 7px; padding-right: 0px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtRMPurchaseAmount" CssClass="form-control required decimal2PInput" required="true" MaxLength="10" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" TabIndex="3" OnTextChanged="txtRMPurchaseAmount_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblRepairAmount" runat="server" Text="Repair Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 12px; padding-right: 12px;" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtRepairAmount" CssClass="form-control required decimal2PInput" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" required="true" MaxLength="10" TabIndex="4" OnTextChanged="txtRepairAmount_TextChanged" AutoPostBack="true" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblTotalAmountText" runat="server" Text="Total Amount (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:Label runat="server" ID="lblTotalAmount" CssClass="control-label" Font-Bold="true" />
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
                                                    <asp:TextBox ID="txtQuotedPrice" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput" placeholder="Enter Quoted Price"
                                                        MaxLength="10" autocomplete="off" oninput="javascript:ValueValidation(this, '1', '999999');" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quoted Price with Tax (Rs)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuotedTaxPrice" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price with Tax"
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
                                                    <asp:TextBox ID="txtQuotedPrice" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" CssClass="form-control decimal2PInput" placeholder="Enter Quoted Price"
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
                                                    <asp:TextBox ID="txtQuotedTaxPrice" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" runat="server" CssClass="form-control decimal2PInput required" placeholder="Enter Quoted Price with Tax"
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
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblAsset" runat="server" Text="Asset" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlAsset" runat="server" CssClass="form-control required" required="true" TabIndex="5" OnSelectedIndexChanged="ddlAsset_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Select" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlKeyParts" runat="server" Visible="false">
                            <div class="form-group row" style="margin-top: 10px;">
                                <div class="col-md-1"></div>
                                <div class="col-md-4">
                                    <div class="col-lg-12 form-group">
                                        <span>Key Parts</span>
                                    </div>
                                    <div class="col-lg-12 form-group">
                                        <asp:ListBox runat="server" data-title="keyparts" ID="lbKeyPart" TabIndex="6" SelectionMode="Multiple" class="unselected form-control" Style="height: 200px; width: 100%;" />
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
                                        <asp:ListBox runat="server" data-title="assignedkeyparts" TabIndex="7" ID="lbAssignedKeyPart" SelectionMode="Multiple" class="selected form-control" Style="height: 200px; width: 100%;" />
                                    </div>
                                </div>
                                <div class="col-md-1"></div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label ID="lblRMAttachBill" runat="server" Text="Attach Bill" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <fup:FileUploadControl runat="server" ID="fupRMAttachBill" Size="1" tabindex="8" Name="FormCtrl" />
                                </div>
                                <div>
                                    <fup:FileUploadControl runat="server" ID="fupRMAttachBillEdit" Size="0" />
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
