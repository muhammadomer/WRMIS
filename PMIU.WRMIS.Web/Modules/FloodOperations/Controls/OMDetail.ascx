<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OMDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.Controls.OMDetail" %>
<div class="tbl-info">
    
<div class="row">
    <div class="col-md-3">
        <asp:Label ID="YearText" Text="Year" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <div class="col-md-3">
        <asp:Label ID="ZoneText" Text="Zone" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <div class="col-md-3">
        <asp:Label ID="CircleText" Text="Circle" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <div class="col-md-3">
        <asp:Label ID="DivisionText" Text="Division" Font-Bold="True" runat="server"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-md-3">
        <asp:Label ID="lblYear" runat="server"></asp:Label>
    </div>
    <div class="col-md-3">
        <asp:Label ID="lblZone" runat="server"></asp:Label>
    </div>
    <div class="col-md-3">
        <asp:Label ID="lblCircle" runat="server"></asp:Label>
    </div>
     <div class="col-md-3">
        <asp:Label ID="lblDivision" runat="server"></asp:Label>
    </div>
</div>
<br/>
<div class="row">
    
    <div class="col-md-3">
        <asp:Label ID="lblinf" Text="Infrastructure Name" Font-Bold="True" runat="server"></asp:Label>
    </div>
</div>
<div class="row">
   
    <div class="col-md-3">
        <asp:Label ID="lblinfraname" runat="server"></asp:Label>
    </div>
</div>
<br />
</div>