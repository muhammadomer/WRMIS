<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SBESDOWorking.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.Controls.SBESDOWorking" %>

<h3 runat="server" id="SBEWorkingTitle">SBE Working</h3>
<asp:Table ID="tblSBEWorking" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell Width="33.3%">Sub Engineer Canal Wire #</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">Sub Engineer Canal Wire Date</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">Date of Closing/Repair</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblSBECanalWireNo" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblSBECanalWireDate" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblDateOfClosingRepair" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

<br />

<h3 runat="server" id="SDOWorkingTitle">SDO Working</h3>
<asp:Table ID="tblSDOWorking" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell Width="33.3%">SDO Canal wire #</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%">SDO Canal Wire Date</asp:TableHeaderCell>
        <asp:TableHeaderCell Width="33.3%"></asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblSDOCanalWireNo" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblSDOCanalWireDate" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>

        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="3">Remarks</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="3">
            <asp:Label ID="lblRemarks" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>






