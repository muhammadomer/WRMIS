<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DivisionSummaryDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.Controls.DivisionSummaryDetail" %>
<div class="tbl-info">
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="YearText" Text="Year" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="ZoneText" Text="Zone" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="CircleText" Text="Circle" runat="server" Font-Bold="True"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblYear" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblZone" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblCircle" runat="server"></asp:Label>
    </div>
</div>
<br/>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="DivisionText" Text="Division" Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        &nbsp;<asp:Label ID="DivisionSummaryStatus" Text="Status" Font-Bold="True" runat="server"></asp:Label>
    </div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblDivision" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        &nbsp;<asp:Label ID="lblDivisionSummaryStatus" runat="server"></asp:Label>
    </div>
</div>


<br />
</div>