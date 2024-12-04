<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SanctionControl.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.Controls.SanctionControl" %>
<div class="table-responsive">
    <table class="table tbl-info">
        <tr>
            <th>
                <div><strong>Budgetary Provision</strong></div>
            </th>
            <th>
                <div><strong>Amount Released</strong></div>
            </th>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBudgetProvision" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAmountReleased" runat="server"></asp:Label>
            </td>
        </tr>



        <tr>
            <th>
                <div><strong>Balance Available</strong></div>
            </th>
            <th>
                <div><strong>Expenditure Upto Previous Bill</strong></div>
            </th>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBalanceAvailable" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblExpenditureUptpPerviousBill" runat="server"></asp:Label>
            </td>
        </tr>

        <tr>
            <th>
                <div><strong>Balance after this Bill</strong></div>
            </th>
            <th>
                <div><strong>This Bill</strong></div>
            </th>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBalanceAfterThisBill" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblThisBill" runat="server"></asp:Label>
            </td>
        </tr>

    </table>
</div>
