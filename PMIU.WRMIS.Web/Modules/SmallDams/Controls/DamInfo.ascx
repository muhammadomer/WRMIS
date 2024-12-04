<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DamInfo.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.Controls.DamInfo" %>
<div class="table tbl-info">
    <div class="tbl-info">
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="DamName" Text="Dam Name" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="DamType" Text="Dam Type" runat="server" Font-Bold="True"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="DamNameText" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="DamTypeText" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="CostProject" Text="Cost of Project" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="YearCompletion" Text="Year of Completion" runat="server" Font-Bold="True"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="CostProjectText" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="YearCompletionText" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</div>
