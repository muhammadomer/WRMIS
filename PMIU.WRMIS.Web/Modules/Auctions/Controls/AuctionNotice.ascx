<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuctionNotice.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.Controls.AuctionNotice" %>
<div class="table-responsive">
    <table class="table tbl-info">
           <tr>
            <th>Auction Notice</th>
        </tr>
       <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblAuctionNotice" runat="server" Text="Auction for Cars" />
            </td>
        </tr>
        <tr>
            <th>Opening Date</th>
            <th>Submission Date</th>
        </tr>
        <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblOpeningDate" runat="server" Text="16-March-2017" />
            </td>
            <td>
                <asp:Label ID="lblSubmissiondate" runat="server" Text="16-March-2017" />
            </td>
        </tr>
      
    </table>
</div>