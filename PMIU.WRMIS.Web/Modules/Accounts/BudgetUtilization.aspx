<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetUtilization.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.BudgetUtilization" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Budget Utilization</h3>
        </div>
        <div class="box-content">
            
            <div class="form-horizontal">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" AutoPostBack="true" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="true" TabIndex="2">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Object Classification" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlObjectClassification" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlAccountHead_SelectedIndexChanged" AutoPostBack="true" TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" ToolTip="Search" TabIndex="4" />
                                </div>
                            </div>
                        </div>


                       
                            <div class="table-responsive" runat="server"  ID="tableSelection" visible="false">
                                 <table class="table tbl-info">
                    <tr>
                        <th>Account Code</th>
                        <th>Object Classification</th>
                        <th>Budgetary Provision (Rs.)</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAccountCode" runat="server"  />
                        </td>
                        <td>
                            <asp:Label ID="lblObjectClassification" runat="server"  />
                        </td>
                        <td>
                            <asp:Label ID="lblBudgetaryProvision" runat="server"  />
                        </td>
                    </tr>
                    <tr>
                        <th>Amount Released (Rs.)</th>
                        <th>Total Expense (Rs.)</th>
                        <th>Remaining Amount (Rs.)</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAmountReleased" runat="server"  />
                        </td>
                        <td>
                            <asp:Label ID="lblTotalExpenses" runat="server"  />
                        </td>
                        <td>
                            <asp:Label ID="lblRemainingAmount" runat="server" />
                        </td>
                    </tr>
                </table>
                             </div>
                        

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvBudgetUtilization" runat="server"
                                        DataKeyNames="AccountHeadID,ObjectClassificationID,AccountsCode,ObjectClassification,BudetoryProvision,AmountReleased"
                                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                        EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                                        OnRowDataBound="gvBudgetUtilization_RowDataBound" OnPageIndexChanged="gvBudgetUtilization_PageIndexChanged"
                                        OnPageIndexChanging ="gvBudgetUtilization_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ObjectClassificationID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObjectClassificationID" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassificationID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountCode" runat="server" CssClass="control-label" Text='<%# Eval("AccountsCode") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-left" />
                                                <HeaderStyle CssClass="text-center col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Object/Classification">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblObjectClassification" runat="server" CssClass="control-label" Text='<%# Eval("ObjectClassification") %>' />
                                                </ItemTemplate>
                                                  <HeaderStyle CssClass="text-center col-md-2" />
                                                <ItemStyle CssClass="text-left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Budgetary Provision (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBudetoryProvision" runat="server" CssClass="control-label" Text='<%# Eval("BudetoryProvision") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-right" />
                                                <HeaderStyle CssClass="text-center col-md-2"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount Released (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountReleased" runat="server" CssClass="control-label" Text='<%# Eval("AmountReleased") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-right" />
                                                <HeaderStyle CssClass="text-center col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Expenses (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalExpense" runat="server" CssClass="control-label" Text='<%# Eval("TotalSanctionAmount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("TotalSanctionAmount"))) : Eval("TotalSanctionAmount") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-right" />
                                                <HeaderStyle CssClass="text-center col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remaining Balance (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemainingBalance" runat="server" CssClass="control-label" Text='<%# Eval("RemainingBalance") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("RemainingBalance"))) : Eval("RemainingBalance") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-right" />
                                                <HeaderStyle CssClass="text-center col-md-2" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>



                                <div class="table-responsive">
                                    <asp:GridView ID="gvBUShowCase" runat="server" Visible="false"
                                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                        EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Expense Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExpenseDate" runat="server" CssClass="control-label" Text='<%#  PMIU.WRMIS.Common.Utility.GetFormattedDate(Convert.ToDateTime(Eval("SanctionDate"))) %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expense Amount (Rs.)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExpenseAmount" runat="server" CssClass="control-label" Text='<%# Eval("SancionAmount") != null ? PMIU.WRMIS.Common.Utility.GetRoundOffValueAccounts(Convert.ToDouble(Eval("SancionAmount"))) : Eval("SancionAmount") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center col-md-4" />
                                                <ItemStyle CssClass="text-center " />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Expense Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExpenseStatus" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatus") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="text-center" />
                                                <HeaderStyle CssClass="text-center col-md-4" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                   
                                </div>
                            </div>
                        </div>



                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeDatePickerStateOnUpdatePanelRefresh();
                        InitilizeNumericValidation();
                    }
                });
            };

        });

    </script>
</asp:Content>
