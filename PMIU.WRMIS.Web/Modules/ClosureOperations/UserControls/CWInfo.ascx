<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CWInfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls.CWInfo" %>
 
<asp:Table ID="tblCW" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Plan Title</asp:TableHeaderCell>
        <asp:TableHeaderCell>Year</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCW_CWPTitle" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblCW_Year" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
    <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Division</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCW_Title" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblCW_DivName" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
     <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Type</asp:TableHeaderCell> 
         <asp:TableHeaderCell>Estimated Costs (Rs.)</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCW_Type" runat="server"></asp:Label>
        </asp:TableCell> 
        <asp:TableCell>
            <asp:Label ID="lblCWCost" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
</asp:Table>

<asp:Table ID="TblCWP" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Plan Title</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCWP_Title" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
    <asp:TableRow>
        <asp:TableHeaderCell>Division</asp:TableHeaderCell> 
        <asp:TableHeaderCell>Year</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCWP_Div" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblCWP_Year" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow>  
</asp:Table>
<%--<br />--%>

