<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchMonthlyExpenses.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.SearchMonthlyExpenses" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Monthly Expenses</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlFinancialYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" CssClass="form-control required" required="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" CssClass="form-control required" required="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" id="divPMIUNExpenseMadeBy">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="PMIU Staff" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlPMIUStaff" runat="server" CssClass="form-control required" AutoPostBack="true" OnSelectedIndexChanged="ddlPMIUStaff_SelectedIndexChanged" required="true">
                                    <asp:ListItem Text="Select" Value=""  />
                                    <asp:ListItem Text="Field" Value="F" Selected="True" />
                                    <asp:ListItem Text="Office" Value="O" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Expenses made By" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlExpenseMadeBy" AutoPostBack="true" OnSelectedIndexChanged="ddlExpenseMadeBy_SelectedIndexChanged" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row" id="searchButtonsDiv">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" ToolTip="Search" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvMonthlyExpense" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="true" OnPageIndexChanging="gvMonthlyExpense_PageIndexChanging" OnRowDataBound="gvMonthlyExpense_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Resource Allocation ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResourceAllocationID" runat="server" CssClass="control-label" Text='<%# Eval("ResourceAllocationID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="control-label" Text='<%# Eval("VehicleNo") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TA/DA (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTADA" runat="server" CssClass="control-label" Text='<%# Eval("TADA") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("TADA"))) : Eval("TADA") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"/>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Repair & Maintenance (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRepairAndMaintenance" runat="server" CssClass="control-label" Text='<%# Eval("RepairMaintainance") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("RepairMaintainance"))) : Eval("RepairMaintainance") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"/>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="POL (if any)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOL" runat="server" CssClass="control-label" Text='<%# Eval("POL") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("POL"))) : Eval("POL") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="New Purchase">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewPurchase" runat="server" CssClass="control-label" Text='<%# Eval("NewPurchase") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("NewPurchase"))) : Eval("NewPurchase") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Expenses">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOtherExpense" runat="server" CssClass="control-label" Text='<%# Eval("OtherExpenses") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("OtherExpenses"))) : Eval("OtherExpenses") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Claim (Rs.)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalClaim" runat="server" CssClass="control-label" Text='<%# Eval("TotalClaim") != "-" ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("TotalClaim"))) : Eval("TotalClaim") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"/>
                                        <HeaderStyle CssClass="col-md-2" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:HyperLink ID="btnDetail" runat="server" NavigateUrl='<%#string.Format("~/Modules/Accounts/ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}", ddlFinancialYear.SelectedItem.Value, ddlMonth.SelectedItem.Value, Eval("ResourceAllocationID"), Eval("TotalClaim"))%>' CssClass="btn btn-primary btn_32 details" ToolTip="Detail" />
                                                <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click" CssClass="btn btn-success btn_32 plus" ToolTip="Add" />
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
                <div class="row" id="divBtnSubmitExpense" Visible="false" runat="server">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnSubmitExpenses" runat="server"  Text="Submit Expenses" CssClass="btn btn-primary" OnClick="btnSubmitExpenses_Click" ToolTip="Submit Expenses" />
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hdnUrl" runat="server" />
                <div id="dvExpenseType" class="modal fade">
                    <div class="modal-dialog" style="width: 30%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="lblExpenseType" runat="server" Text="Expense Type" CssClass="col-sm-4 col-lg-3 control-label" Style="padding-left: 0px; padding-right: 0px;" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="Add" OnClick="btnAdd_Click"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>






        
            <div id="ExpenseSaved" class="modal fade">
                <div class="modal-dialog" style="width: 36%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblRecordSaved" runat="server" Text="" />
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


    </div>
</asp:Content>
