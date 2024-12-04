<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DamReadingsData.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.Controls.DamReadingsData" %>
<div class="table tbl-info">
    <div class="tbl-info">
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="Division" Text="Division" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="SubDivision" Text="Sub Division" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="DamName" Text="DamName" runat="server" Font-Bold="True"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="DivisionText" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="SubDivisionText" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="DamNameText" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="Date" Text="Date" runat="server" Font-Bold="True"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="DateText" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <br />
</div>
