<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkProgressInfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls.WorkProgressInfo" %>

<asp:Table ID="tblAdd" runat="server" CssClass="table tbl-info">
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
        <asp:TableHeaderCell>Previous Progress (%)</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow> 
        <asp:TableCell><asp:Label ID="lblCW_Type" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell><asp:Label ID="lblCW_Pgrs" runat="server"></asp:Label></asp:TableCell> 
    </asp:TableRow> 
     <asp:TableRow>
        <asp:TableHeaderCell>Previous Progress Dated</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblDate" runat="server"></asp:Label> 
        </asp:TableCell> 
    </asp:TableRow> 
</asp:Table>

<asp:Table ID="tblView" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Plan Title</asp:TableHeaderCell>
        <asp:TableHeaderCell>Year</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblTitleV" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblYearV" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
    <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Division</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblNameV" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblDivV" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
     <asp:TableRow>
        <asp:TableHeaderCell>Closure Work Type</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>  
        <asp:TableCell><asp:Label ID="lblTypeV" runat="server"></asp:Label></asp:TableCell> 
    </asp:TableRow>  
</asp:Table>