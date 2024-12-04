<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlledInfrastructureDetails.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.ControlledInfrastructureDetails" %>

<asp:Table ID="tblInfrastructureDetails" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Barrage/Headwork Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Barrage/Headwork Type</asp:TableHeaderCell>
        <asp:TableHeaderCell>Barrage/Headwork Status</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblType" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
