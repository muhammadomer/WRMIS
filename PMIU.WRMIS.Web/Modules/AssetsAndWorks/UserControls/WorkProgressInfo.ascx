<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkProgressInfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls.WorkProgressInfo" %>

<asp:Table ID="tblAdd" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Work Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Financial Year</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblWorkName" runat="server"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lblYear" runat="server"></asp:Label>
        </asp:TableCell> 
    </asp:TableRow> 
     <asp:TableRow>
        <asp:TableHeaderCell>Work Type</asp:TableHeaderCell>
          <asp:TableHeaderCell>Previous Progress Dated</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow> 
        <asp:TableCell><asp:Label ID="lblAW_Type" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell><asp:Label ID="lblDate" runat="server"></asp:Label> </asp:TableCell>
    </asp:TableRow> 
     <asp:TableRow>
         <asp:TableHeaderCell>Previous Physical Progress (%)</asp:TableHeaderCell>
         <asp:TableHeaderCell>Previous Financial Progress (%)</asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>

            <asp:TableCell><asp:Label ID="lblAW_Pgrs" runat="server"></asp:Label></asp:TableCell> 
             <asp:TableCell><asp:Label ID="lblFinancial" runat="server"></asp:Label></asp:TableCell>
 
    </asp:TableRow> 
</asp:Table>

<asp:Table ID="tblView" runat="server" CssClass="table tbl-info">
    <asp:TableRow>
        <asp:TableHeaderCell>Work Name</asp:TableHeaderCell>
        <asp:TableHeaderCell>Financial Year</asp:TableHeaderCell> 
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
        <asp:TableHeaderCell>Closure Work Type</asp:TableHeaderCell> 
    </asp:TableRow>
    <asp:TableRow>  
        <asp:TableCell><asp:Label ID="lblTypeV" runat="server"></asp:Label></asp:TableCell> 
    </asp:TableRow>  
</asp:Table>