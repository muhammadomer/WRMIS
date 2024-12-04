<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GaugeInspection.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.GaugeInspection" %>

<div class="table-responsive">
    <table class="table tbl-info">
        <tr>
            <th colspan="2" style="width:66.6%;">Title</th>
            <th style="width: 33.3%">Status</th>
        </tr>
        <tr>
            <td colspan="2" style="max-width:100px;word-break:break-all">
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
        <tr>
            <th>Channel Name</th>
            <th>Inspection Area</th>
            <th>Inspected By</th>
        </tr>

        <tr>
            <td>
                <asp:Label ID="lblChannelName" runat="server" Text="Label"></asp:Label></td>
            <td>
                <asp:Label ID="lblInspectionArea" runat="server" Text="Label"></asp:Label></td>
            <td>
                <asp:Label ID="lblInspectedBy" runat="server" Text="Label"></asp:Label></td>

        </tr>
    </table>
</div>
