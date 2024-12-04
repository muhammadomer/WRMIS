<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutletChannelDetails.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.OutletChannelDetails" %>
<asp:Table ID="tblChannelData" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>
                    Channel Name
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
                    Channel Type
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
                     Total R.Ds. (ft)
        </asp:TableHeaderCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblChannelName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblChannelType" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblTotalRD" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnChannelTotalRDs" Value="0" runat="server" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell>
                    Flow Type
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
                   Command Name
        </asp:TableHeaderCell>
        <asp:TableHeaderCell>
                    IMIS Code
        </asp:TableHeaderCell>

    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblFlowType" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblCommandName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblIMISCode" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>

</asp:Table>
