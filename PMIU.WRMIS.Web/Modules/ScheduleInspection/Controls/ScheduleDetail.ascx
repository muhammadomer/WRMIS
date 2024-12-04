<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail" %>
<table class="table tbl-info">
    <tr>
        <th colspan="2" style="width: 66.6%">Title</th>
        <th style="width: 33.3%">Status</th>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblScheduleTitle" runat="server" Text="Inspection Schedule of Ahmadpur Disty" />
        </td>
        <td>
            <asp:Label ID="lblStatus" runat="server" Text="Approved" />
        </td>
    </tr>
    <tr>
        <th style="width: 33.3%">Prepared By</th>
        <th style="width: 33.3%">From Date</th>
        <th style="width: 33.3%">To Date</th>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPreparedBy" runat="server" Text="Muhammad Ali, MA" />
        </td>
        <td>
            <asp:Label ID="lblFromDate" runat="server" Text="August 15, 2015" />
        </td>
        <td>
            <asp:Label ID="lblToDate" runat="server" Text="August 16, 2015" />
        </td>
    </tr>
</table>
