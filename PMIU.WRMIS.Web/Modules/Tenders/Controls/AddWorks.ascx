<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddWorks.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Controls.AddWorks" %>
<div class="table-responsive">
    <table class="table tbl-info">
        <tr>
            <th>Domain</th>
            <th>Division</th>
        </tr>
        <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblTenderDomain" runat="server" Text="Domain" />
            </td>
            <td>
                <asp:Label ID="lblTenderDivision" runat="server" Text="Division" />
            </td>
        </tr>
         <tr>
            <th>Tender Notice</th>
        </tr>
       <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblTenderNotice" runat="server" Text="Tender Notice of Lahore Drainage Division" />
            </td>
        </tr>
    </table>
</div>