<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewWorks.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Controls.ViewWorks" %>
<div class="table-responsive">
    <table class="table tbl-info">
        <tr>
            <th>Tender Notice</th>
        </tr>
        <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblTenderNotice" runat="server" Text="Tender Notice" />
            </td>
        </tr>
         <tr>
            <th>Work/Tender Name</th>
             <th>Work Type</th>
        </tr>
       <tr>
            <td style="word-break:break-all">
                <asp:Label ID="lblWorkName" runat="server" Text="Work" />
            </td>
            <td>
                <asp:Label ID="lblWorkType" runat="server" Text="Type" />
            </td>
        </tr>
    </table>
</div>