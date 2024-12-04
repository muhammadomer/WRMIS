<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpenseDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ExpenseDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Expense Details</h3>
                </div>
                <div class="box-content">

                    <div class="table-responsive">
                        <table class="table tbl-info">

                            <tr>

                                <th width="33.3%">Name of Staff</th>
                                <th width="33.3%">Designation</th>
                                <th width="33.3%">Resource Type</th>
                                <th></th>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblNameofStaff" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblDesignation" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblResourceType" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                            <tr>
                                <th>Financial Year</th>
                                <th>Month</th>
                                <th>Total Claim (Rs.)</th>

                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblFinancialYear" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblMonth" runat="server" Text="" /></td>
                                <td>
                                    <asp:Label ID="lblTotalClaim" runat="server" Text="" /></td>
                                <td></td>
                            </tr>

                        </table>
                    </div>
                    <br />

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblExpenseType" runat="server" Text="Expense Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlExpenseType_SelectedIndexChanged" required="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="table-responsive">
                        <asp:GridView ID="gvRepairMaintenance" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvRepairMaintenance_RowDataBound"
                            OnRowDeleting="gvRepairMaintenance_RowDeleting" OnPageIndexChanging="gvRepairMaintenance_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total Claim For the Month" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Asset Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetName" runat="server" CssClass="control-label" Text='<%# Eval("AT_AssetAllocation.AM_AssetItems.AssetName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Purchase Items (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseAmount" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Repair Items (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRepairAmount" runat="server" CssClass="control-label" Text='<%# Eval("RepairAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Claim (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" CssClass="control-label"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblTotalClaimForTheMonth" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                </asp:TemplateField>



                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <asp:GridView ID="gvPOLRecipts" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvPOLRecipts_RowDataBound"
                            OnRowDeleting="gvPOLRecipts_RowDeleting" OnPageIndexChanging="gvPOLRecipts_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total Claim For the Month" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Meter Reading (Km)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOLMeterReading" runat="server" CssClass="control-label" Text='<%# Eval("POLMeterReading") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Receipt No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOLReceiptNo" runat="server" CssClass="control-label" Text='<%# Eval("POLReceiptNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOLDatetime" runat="server" CssClass="control-label" Text='<%# Eval("POLDatetime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Amount (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOLAmount" runat="server" CssClass="control-label" Text='<%# Eval("POLAmount") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblTotalClaimForTheMonth" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <asp:GridView ID="gvTADA" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvTADA_RowDataBound"
                            OnRowDeleting="gvTADA_RowDeleting" OnPageIndexChanging="gvTADA_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                            OnRowCreated="gvTADA_RowCreated" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total Claim For the Month" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ordinary">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrdinaryDailiesHalf" runat="server" CssClass="control-label" Text='<%# Eval("OrdinaryDailiesHalf") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Special">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecialDailiesHalf" runat="server" CssClass="control-label" Text='<%# Eval("SpecialDailiesHalf") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ordinary">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrdinaryDailiesFull" runat="server" CssClass="control-label" Text='<%# Eval("OrdinaryDailiesFull") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Special">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecialDailiesFull" runat="server" CssClass="control-label" Text='<%# Eval("SpecialDailiesFull") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Public Transport">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalKMPublicTransport" runat="server" CssClass="control-label" Text='<%# Eval("TotalKMPublicTransport") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Official Vehicle">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalKMOfficialVehicle" runat="server" CssClass="control-label" Text='<%# Eval("TotalKMOfficialVehicle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Misc. Expense (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMiscExpenditures" runat="server" CssClass="control-label" Text='<%# Eval("MiscExpenditures") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Claim (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAAmount" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("TAAmount") %>'></asp:Label>
                                        <asp:Label ID="lblDAAmount" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("DAAmount") %>'></asp:Label>
                                        <asp:Label ID="lblTADASum" runat="server" CssClass="control-label"></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblTotalClaimForTheMonth" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <asp:GridView ID="gvNewPurchase" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvNewPurchase_RowDataBound"
                            OnRowDeleting="gvNewPurchase_RowDeleting" OnPageIndexChanging="gvNewPurchase_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total Claim For the Month" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Purchased Item Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchasedItemName" runat="server" CssClass="control-label" Text='<%# Eval("PurchasedItemName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Approval Reference">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalReference" runat="server" CssClass="control-label" Text='<%# Eval("ApprovalReference") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Purchase Items Amount (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseAmount" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseAmount") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblTotalClaimForTheMonth" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <asp:GridView ID="gvOtherExpenses" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvOtherExpenses_RowDataBound"
                            OnRowDeleting="gvOtherExpenses_RowDeleting" OnPageIndexChanging="gvOtherExpenses_PageIndexChanging"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" CssClass="control-label" Text='<%# Eval("BillNo") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total Claim For the Month" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bill Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" runat="server" CssClass="control-label" Text='<%# Eval("BillDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Expense Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpenseItemName" runat="server" CssClass="control-label" Text='<%# Eval("ExpenseName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Approval Reference">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalReference" runat="server" CssClass="control-label" Text='<%# Eval("ApprovalReference") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Expense Amount (Rs.)">
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseAmount" runat="server" CssClass="control-label" Text='<%# Eval("ExpenseAmount") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblTotalClaimForTheMonth" runat="server" Visible="true" />
                                        </b>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Object/Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblObjectClassification" runat="server" CssClass="control-label" Text='<%# Eval("AT_ObjectClassification.ObjectClassification") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" />
                                            <asp:Label ID="lblStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
