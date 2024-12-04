<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FloodInspectionDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.Controls.FloodInspectionDetail" %>
<div class="tbl-info">
<div class="row">
    <div class="col-md-4"><strong>Inspection Date</strong></div>
    <div class="col-md-4"><strong>Division</strong></div>
    <div class="col-md-4"><strong>Inspection Type</strong></div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblInspectionDate" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblDivision" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblInspectionType" runat="server"></asp:Label>
    </div>
</div>
<br />
<div class="row">
    <div class="col-md-4"><strong>Infrastructure Type</strong></div>
    <div class="col-md-4"><strong>Infrastructure Name</strong></div>
    <div class="col-md-4">
        <strong runat="server" id="strInspectionStatus">Inspection Status</strong>
        <strong runat="server" id="strInspectionCategory" visible="false">Inspection Category</strong>
    </div>
</div>
<div class="row">
    <div class="col-md-4">
        <asp:Label ID="lblInfrastructureType" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblInfrastructureName" runat="server"></asp:Label>
    </div>
    <div class="col-md-4">
        <asp:Label ID="lblInspectionStatus" runat="server"></asp:Label>
        <asp:Label ID="lblInspectionCategory" runat="server" Visible="false"></asp:Label>
    </div>
</div>
<br />
<div id="dvDrain" runat="server" visible="false">
    <div class="row">
        <div class="col-md-4"><strong>Outfall Type</strong></div>
        <div class="col-md-4"><strong>Outfall Name</strong></div>
        <div class="col-md-4"><strong>Inspection Status</strong></div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <asp:Label ID="lblOutfallType" runat="server"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblOutfallName" runat="server"></asp:Label>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblInspectionStatusDrain" runat="server"></asp:Label>
        </div>
    </div>
    <br />
</div>
</div>