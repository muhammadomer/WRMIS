<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfrastructureDetails.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.InfrastructureDetail" %>

<div class="row">
    <div class="col-md-4"><strong>Name</strong></div>
    <div class="col-md-4"><strong>Type</strong></div>
    <div class="col-md-4"><strong>Status</strong></div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblName" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblType" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
        <%--<asp:HiddenField ID="hdnChannelTotalRDs" Value="0" runat="server" />--%>
    </div>
</div>
<br />