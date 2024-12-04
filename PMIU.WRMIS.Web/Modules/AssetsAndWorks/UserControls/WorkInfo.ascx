<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkInfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls.WorkInfo" %>
 
<asp:Table ID="tblCW" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Financial Year</asp:TableHeaderCell>
        <asp:TableHeaderCell>Division</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblFinancialYear" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblDivision" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
    <asp:TableRow>
      <asp:TableHeaderCell>Work Type</asp:TableHeaderCell> 
        <asp:TableHeaderCell>Work Name</asp:TableHeaderCell></asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblWorkType" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblWorkName" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
        <asp:TableRow>
        <asp:TableHeaderCell>Estimated Cost (Rs.)</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblECost" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>  
</asp:Table>





