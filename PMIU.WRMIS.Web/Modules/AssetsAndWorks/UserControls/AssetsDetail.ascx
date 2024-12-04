<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetsDetail.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.UserControls.AssetsDetail" %>
<div class="table tbl-info">
    <div class="tbl-info">
        <div class="row">
            <div class="col-md-4">

                <asp:Label ID="lblNameText" Text="Asset Name" Font-Bold="True" runat="server"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblLevelText" Text="Asset Level" runat="server" Font-Bold="True"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblAssetTypeText" Text="Asset Type" Font-Bold="True" runat="server"></asp:Label>

            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblName" runat="server"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblLevel" runat="server"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblAssetType" runat="server"></asp:Label>

            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lbllocationText" Text="Location" runat="server" Font-Bold="True"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblCategoryText" Text="Asset Category" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblSubcategoryText" Text="Asset Sub-Category" Font-Bold="True" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lbllocation" runat="server"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblCategory" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblSubcategory" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblQuantityText" Text="Lot Quantity" Font-Bold="True" runat="server" Visible="false"></asp:Label>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblAvailableQuantityText" Text="Available Quantity" Font-Bold="True" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblUnitsText" Text="Units" Font-Bold="True" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblQuantity" runat="server" Visible="false"></asp:Label>

            </div>

            <div class="col-md-4">
                <asp:Label ID="lblAvailableQuantity" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblUnits" runat="server" Visible="false"></asp:Label>
            </div>

        </div>
    </div>

    <br />
</div>
