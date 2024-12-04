<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Payments.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.Controls.Payments" %>
<div class="table-responsive">
    <table class="table tbl-info">
           <tr>
            <th>Auction Notice</th>
            <th>Opening Date</th>
            <th>Submission Date</th>
        </tr>
       <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblAuctionNotice" runat="server" Text="Auction for Cars" />
            </td>
            <td style="word-break:break-all">
                <asp:Label ID="lblOpeningDate" runat="server" Text="16-March-2017" />
            </td>
            <td>
                <asp:Label ID="lblSubmissiondate" runat="server" Text="16-March-2017" />
            </td>
        </tr>
        <tr>
           <th>Asset Category</th>
            <th>Asset Sub-Category</th>
            <th>Asset Name</th>
        </tr>
        <tr>
           <td style="word-break:break-all">
                <asp:Label ID="lblCategory" runat="server" Text="Transportation" />
            </td>
            <td style="word-break:break-all">
                <asp:Label ID="lblSubCategory" runat="server" Text="Car" />
            </td>
              <td style="word-break:break-all">
                <asp:Label ID="lblAssetName" runat="server" Text="LEF" />
            </td>
        </tr>

       <tr>
           <th>Earnest Money</th>
            <th>Bidder Rate</th>
            <th>Token Money</th>
        </tr>
        <tr>
           <td style="word-break:break-all">
                <asp:Label ID="lblEarnestMoney" runat="server" Text="Transportation" />
            </td>
            <td style="word-break:break-all">
                <asp:Label ID="lblBidderRate" runat="server" Text="Car" />
            </td>
              <td style="word-break:break-all">
                <asp:Label ID="lblTokenMoney" runat="server" Text="LEF" />
            </td>
        </tr>

             <tr>
           <th>Total Token Paid</th>
            <th>Total Amount Paid</th>
            <th>Balance Amount</th>
        </tr>
        <tr>
           <td style="word-break:break-all">
                <asp:Label ID="lblTotalTokenMoneyPaid" runat="server" Text="Transportation" />
            </td>
            <td style="word-break:break-all">
                <asp:Label ID="lblTotalAmountPaid" runat="server" Text="Car" />
            </td>
              <td style="word-break:break-all">
                <asp:Label ID="lblBalanceAmount" runat="server" Text="LEF" />
            </td>
        </tr>
        <tr>
           <th>Submission of Balance Date</th>
        </tr>
        <tr>
           <td style="word-break:break-all">
                <asp:Label ID="lblBalanceSubmissionDate" runat="server" Text="Transportation" />
            </td>
           </tr>
      
    </table>
</div>