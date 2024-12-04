<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrainDetails.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Controls.DrainDetails" %>

<div class="table-responsive">
    <table class="table tbl-info">
        <tr>
            <th>Drain Name:</th>
            <th>Total Length(ft):</th>
            <th>Catchment Area(sq ft):</th>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDrainName" runat="server" Text="Drain" />
            </td>
            <td> <asp:Label ID="lblTotalLength" runat="server" Text="3" /></td>
            <td>
                <asp:Label ID="lblCatchmentArea" runat="server" Text="2" />
            </td>
        </tr>
        <tr>
            <th>Major Buildup Area Name:</th>
            <th>Drain Status:</th>
            
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMajorBuildUpArea" runat="server" Text="None" />
            </td>
            <td>
                <asp:Label ID="lblDrainStatus" runat="server" Text="Inactive" />
            </td>
           
        </tr>
       
    </table>
</div>