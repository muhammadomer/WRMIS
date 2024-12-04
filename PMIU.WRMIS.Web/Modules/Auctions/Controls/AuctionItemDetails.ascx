<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuctionItemDetails.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.Controls.AuctionItemDetails" %>
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
                <asp:Label ID="lblAssetName" runat="server" Text="ABC" />
            </td>
        </tr>
      
    </table>
</div>