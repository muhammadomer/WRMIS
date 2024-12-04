<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewSanction.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.AddNewSanction" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucACCP" TagName="AccpTitleYear" Src="~/Modules/Accounts/Controls/SanctionControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function RemoveValidation() {
            //debugger;
            $('#<%= ddlSanctionedMonth.ClientID %>').removeAttr('required');
            $('#<%= ddlObjectClassification.ClientID %>').removeAttr('required');
        }
        function RemoveVal() {
            //debugger;
            $('#<%= txtRejectionReason.ClientID %>').removeAttr('required');
        }
    </script>
    <div class="box">
        <div class="box-title">
            <h3>Preparing New Sanction</h3>
        </div>

        <div class="box-content">



            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Sanction Type" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlSanctionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSanctionType_SelectedIndexChanged" CssClass="form-control required" required="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Sanction On" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlSanctionOn" OnSelectedIndexChanged="ddlSanctionOn_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlFinancialYear" runat="server" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control required" required="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                <div class="row" id="searchButtonsDiv">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnSearch" runat="server" Text="Search Bills" CssClass="btn btn-primary" OnClick="btnSearch_Click" ToolTip="Search Bill" />
                            <asp:Button ID="PrintSanction" Visible="false" runat="server" Text="Print Sanction" CssClass="btn btn-success" OnClick="PrintSanction_Click" ToolTip="Print Sanction" />
                            <asp:Button ID="ShowTaxSheet" Visible="false" runat="server" Text="Show Tax Sheet" CssClass="btn btn-primary" OnClick="ShowTaxSheet_Click" ToolTip="Show Tax Sheet" />
                        </div>
                    </div>
                </div>

                <%-- <ucACCP:AccpTitleYear ID="FY" Visible="false" runat="server" />--%>

                <div class="row" id="GridRepairMaintenance" runat="server">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvRepairMaintenance" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" DataKeyNames="VendorType" CssClass="table header" OnRowCommand="gvRepairMaintenance_RowCommand"
                                BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                OnRowDataBound="gvRepairMaintenance_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetTypeID" runat="server" CssClass="control-label" Text='<%# Eval("AssetTypeID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verify All">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlVerifyAll" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" class="vAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlVerify" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkVerify" runat="server" AutoPostBack="true" OnCheckedChanged="chkVerify_CheckedChanged" CssClass="checkbox" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of Staff" FooterText="Grand Total" FooterStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate")=="01/01/0002" ? "" : Eval("BillDate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetName" runat="server" CssClass="control-label" Text='<%# Eval("AssetName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetType" runat="server" CssClass="control-label" Text='<%# Eval("AssetType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Items (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseItems" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseItems") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("PurchaseItems"))) : Eval("PurchaseItems") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Repair Items (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRepairItems" runat="server" CssClass="control-label" Text='<%# Eval("RepairItems") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("RepairItems"))) : Eval("RepairItems") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Claim (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("TotalClaim") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("TotalClaim"))) : Eval("TotalClaim") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="true" CssClass="ApprovalAmount" Text='<%# Eval("TotalClaim") %>' />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sanction Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedStatus" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejenctionReason" runat="server" CssClass="control-label" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor Type">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlVendorType" runat="server" required="required" CssClass="form-control required">
                                                <asp:ListItem Value="1">Filer</asp:ListItem>
                                                <asp:ListItem Value="2" Selected="True">Non Filer</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="btnReject" CommandName="Rejected" runat="server" CommandArgument='<%# Eval("AssetName") %>' CssClass="btn btn-danger btn_32 reject" ToolTip="Reject" />
                                                <asp:LinkButton ID="btnReConsider" runat="server" Visible="false" CommandArgument='<%# Eval("AssetName") %>' CommandName="ReConsider" CssClass="btn btn-success btn_32 plus" ToolTip="Re Consider" />
                                            </asp:Panel>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>

                </div>
                <div class="row" id="GridPolRecipts" visible="false" runat="server">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvPolRecipts" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header"
                                OnRowDataBound="gvPolRecipts_RowDataBound" OnRowCommand="gvPolRecipts_RowCommand" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetTypeID" runat="server" CssClass="control-label" Text='<%# Eval("AssetTypeID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Verify All">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlVerifyAll" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" class="vAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlVerify" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkVerify" runat="server" AutoPostBack="true" OnCheckedChanged="chkVerify_CheckedChanged" CssClass="checkbox" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of Staff">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Meter Reading">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMeterReading" runat="server" CssClass="control-label" Text='<%# Eval("MeterReading") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="POL Receipt No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOLReceiptNo" runat="server" CssClass="control-label" Text='<%# Eval("POLReceiptNo") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOLDate" runat="server" CssClass="control-label" Text='<%# Eval("POLDatetime") == "01/01/0002" ? "" : Eval("POLDatetime")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sanction Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedStatus" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejenctionReason" runat="server" CssClass="control-label" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("AmountRs") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("AmountRs"))) : Eval("AmountRs") %>' />

                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="btnReject" CommandName="Rejected" runat="server" CommandArgument='<%# Eval("BillNo") %>' CssClass="btn btn-danger btn_32 reject" ToolTip="Reject" />
                                                <asp:LinkButton ID="btnReConsider" runat="server" CommandArgument='<%# Eval("BillNo") %>' CommandName="ReConsider" Visible="false" CssClass="btn btn-success btn_32 plus" ToolTip="Re Consider" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="row" id="GridTADA" visible="false" runat="server">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvTADA" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" OnRowCommand="gvTADA_RowCommand"
                                OnRowDataBound="gvTADA_RowDataBound" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verify All">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlVerifyAll" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" class="vAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlVerify" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkVerify" runat="server" AutoPostBack="true" OnCheckedChanged="chkVerify_CheckedChanged" CssClass="checkbox" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of Staff">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNameOfStaff" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Dailies(Ordinary)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHalfDailiesOrdinary" runat="server" CssClass="control-label" Text='<%# Eval("HalfDailiesOrdinary") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Dailies(Ordinary)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullDailiesOrdinary" runat="server" CssClass="control-label" Text='<%# Eval("FullDailiesOrdinary") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Dailies (Special)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHalfDailiesSpecial" runat="server" CssClass="control-label" Text='<%# Eval("HalfDailiesSpecial") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Dailies (Special)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullDailiesSpecial" runat="server" CssClass="control-label" Text='<%# Eval("FullDailiesSpecial") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total KM (Public Transport)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalKMPublicTransport" runat="server" CssClass="control-label" Text='<%# Eval("TotalKMPublicTransport") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total KM (Irrigation Vehicle)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalKMIrrigationVehicle" runat="server" CssClass="control-label" Text='<%# Eval("TotalKMIrrigationVehicle") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Misc. Expenses (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMiscExpenses" runat="server" CssClass="control-label" Text='<%# Eval("MiscExpenses") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("MiscExpenses"))) : Eval("MiscExpenses") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total TA/DA (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("TotalTADA") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("TotalTADA"))) : Eval("TotalTADA") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejenctionReason" runat="server" CssClass="control-label" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sanction Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedStatus" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="btnReject" CommandName="Rejected" runat="server" CommandArgument='<%# Eval("BillNo") %>' CssClass="btn btn-danger btn_32 reject" ToolTip="Reject" />
                                                <asp:LinkButton ID="btnReConsider" runat="server" CommandArgument='<%# Eval("BillNo") %>' CommandName="ReConsider" Visible="false" CssClass="btn btn-success btn_32 plus" ToolTip="Re Consider" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="row" id="GridNewPurchase" visible="false" runat="server">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvNewPurchase" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                OnRowCommand="gvNewPurchase_RowCommand" OnRowDataBound="gvNewPurchase_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verify All">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlVerifyAll" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" class="vAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlVerify" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkVerify" runat="server" AutoPostBack="true" OnCheckedChanged="chkVerify_CheckedChanged" CssClass="checkbox" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of Staff">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNameOfStaff" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate")=="01/01/0002" ? "" : Eval("BillDate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchased Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchasedItemName" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseItemName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase Amount (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseItemAmount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("PurchaseItemAmount"))) : Eval("PurchaseItemAmount") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRejenctionReason" runat="server" CssClass="control-label" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sanction Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSanctionedStatus" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor Type">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlVendorType" runat="server" required="required" CssClass="form-control required">
                                                <asp:ListItem Value="1">Filer</asp:ListItem>
                                                <asp:ListItem Value="2" Selected="True">Non Filer</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="btnReject" CommandName="Rejected" runat="server" CommandArgument='<%# Eval("BillNo") %>' CssClass="btn btn-danger btn_32 reject" ToolTip="Reject" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="row" id="GridOtherExpense" visible="false" runat="server">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvOtherExpense" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header"
                                OnRowDataBound="gvOtherExpense_RowDataBound" OnRowCommand="gvOtherExpense_RowCommand" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Verify All">
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlVerifyAll" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkbox" class="vAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlVerify" runat="server" HorizontalAlign="Center">
                                                <asp:CheckBox ID="chkVerify" runat="server" AutoPostBack="true" OnCheckedChanged="chkVerify_CheckedChanged" CssClass="checkbox" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of Staff">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expense Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpenseName" runat="server" CssClass="control-label" Text='<%# Eval("ExpenseName") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("ExpenseAmount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("ExpenseAmount"))) : Eval("ExpenseAmount") %>' />
                                            <asp:Label ID="lblSanctionedStatus" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("SanctionStatus") %>' />
                                            <asp:Label ID="lblRejenctionReason" runat="server" CssClass="control-label" Visible="false" Text='' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right" />
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="btnReject" CommandName="Rejected" runat="server" CommandArgument='<%# Eval("BillNo") %>' CssClass="btn btn-danger btn_32 reject" ToolTip="Reject" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="row" runat="server" id="pnlTotalClaim" visible="false">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Sanctioned Amount (Rs)" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:Label ID="lblTotalsanctionAmount" runat="server" CssClass="form-control required">
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" Visible="false" Text="Prepare Sanction" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>







            <div id="SanctionedCreate" class="modal fade">
                <div class="modal-dialog" style="width: 36%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblRejectionReason" runat="server" Text="" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>






            <div id="RejectionPopUp" class="modal fade" style="margin-top: 400px;">
                <div class="modal-dialog" style="width: 50%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="row">

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="lblModalSanctionStatus" runat="server" Text="Reason for Rejection" CssClass="col-sm-4 col-lg-3 control-label" />
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:TextBox ID="txtRejectionReason" runat="server" MaxLength="50" CssClass="form-control" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnRejectYes" runat="server" class="btn btn-primary" Text="Save" OnClick="btnRejectYes_Click"></asp:Button>
                            <button class="btn btn-default" data-dismiss="modal" onclick="RemoveVal();">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div id="PrePairSanctionedPopUp" class="modal fade" style="margin-top: 400px;">
                <div class="modal-dialog" style="width: 50%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" Text="Sanctioned for the Month" CssClass="col-sm-4 col-lg-3 control-label" />
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList ID="ddlSanctionedMonth" runat="server" CssClass="form-control required" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br>
                                    </br>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="Label5" runat="server" Text="Object/Classification" CssClass="col-sm-4 col-lg-3 control-label" />
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList ID="ddlObjectClassification" runat="server" CssClass="form-control required" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSaveSanctioned" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSaveSanctioned_Click"></asp:Button>
                            <button class="btn btn-default" data-dismiss="modal" onclick="RemoveValidation();">Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
