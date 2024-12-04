<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutletInspectionNotes.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.OutletInspectionNotes" %>
<asp:Table ID="tblChannelDetail" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2" Width="66.6%">Title</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">Status</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" style="max-width:100px;word-break:break-all">
            <asp:Label ID="lblScheduleTitle" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableHeaderCell Width="33.3%">Prepared By</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">From Date</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">To Date</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblPreparedBy" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblFromDate" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblToDate" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableHeaderCell>Channel Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Outlet Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Outlet Type</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblChannelName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblOutletName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblOutletType" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>

     <asp:TableRow ID="InspectedByLabel" runat="server" Visible="false">
        <asp:TableHeaderCell>Inspected By</asp:TableHeaderCell>
        
    </asp:TableRow>
    <asp:TableRow ID="InspectedByValue" runat="server" Visible ="false">
        <asp:TableCell>
            <asp:Label ID="lblInspectedBy" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
<br />
