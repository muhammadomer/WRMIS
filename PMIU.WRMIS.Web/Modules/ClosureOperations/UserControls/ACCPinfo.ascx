<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ACCPinfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls.ACCPinfo" %>
<asp:Table ID="tblACCP" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Title</asp:TableHeaderCell>
        <asp:TableHeaderCell>Year</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblTitle" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblYear" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
</asp:Table>
<br />
