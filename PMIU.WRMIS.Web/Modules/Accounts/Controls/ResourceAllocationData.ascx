<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceAllocationData.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.Controls.ResourceAllocationData" %>


    <%--<div class="tbl-info">
        <div class="row">
            <div class="col-md-4">

                <asp:Label ID="lblResourceType" Text="Resource Type" runat="server" Font-Bold="True"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="lblDesignation" Text="Designation" runat="server" Font-Bold="True"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="lblNameOfStaff" Text="Name of Staff" runat="server" Font-Bold="True"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="lblBPS" Text="BPS" runat="server" Font-Bold="True"></asp:Label>
            </div>

        </div>
        <br />
        <div class="row">
            <div class="col-md-4">

                <asp:Label ID="ResourceTypeText" runat="server"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="DesignationText" runat="server"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="NameofStaffText" runat="server"></asp:Label>
            </div>

            <div class="col-md-4">
                <asp:Label ID="BPSText" runat="server"></asp:Label>
            </div>

        </div>

    </div>--%>


    <table class="table tbl-info">

        <tr>

            <th width="33.3%">Resource Type</th>
            <th width="33.3%">Designation</th>
            <th width="33.3%">Name of Staff</th>
            <th></th>
        </tr>

        <tr>
            <td>
                <asp:Label ID="ResourceTypeText" runat="server" Text="" /></td>
            <td>
                <asp:Label ID="DesignationText" runat="server" Text="" /></td>
            <td>
                <asp:Label ID="NameofStaffText" runat="server" Text="" /></td>
            <td>
                <asp:Label ID="lbl" runat="server" Text="" /></td>
        </tr>

        <tr>

            <th>BPS</th> 
        </tr>

        <tr>
            <td>
                <asp:Label ID="BPSText" runat="server" Text="" /></td>
        </tr>

    </table>
