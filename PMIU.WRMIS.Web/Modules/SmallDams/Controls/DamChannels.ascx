<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DamChannels.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.Controls.DamChannels" %>
<div class="table tbl-info">


    <div class="tbl-info">
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="Channel" Text="Channel" runat="server" Font-Bold="True"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="ChannelCapacity" Text="Channel Capacity" runat="server" Font-Bold="True"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="ChannelText" runat="server"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:Label ID="ChannelCapacityText" runat="server"></asp:Label>
            </div>
        </div>

    </div>

    <br />
</div>
