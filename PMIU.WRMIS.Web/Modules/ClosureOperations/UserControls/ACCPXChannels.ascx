<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ACCPXChannels.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.UserControls.ACCPXChannels" %>

<div class="table-responsive" >
    <table class="table tbl-info">
        <tr>
            <th>
            <div><strong>Annual Canal Closure Programme Title</strong></div>
                </th>
            <th>
                <div ><strong>Year</strong></div>
           </th>
            </tr>
        <tr>
    <td>
        <asp:Label ID="lblACCP" runat="server" ></asp:Label>
        </td>
    <td>
        <asp:Label ID="lblACCPYear" runat="server"></asp:Label>
        </td>
       </tr>
        <tr>
             <th>
                <div ><strong>Command Name</strong></div>
           </th>
             <th>
                <div ><strong>Channel Name</strong></div>
           </th>
        </tr>
        <tr>
        <td>
            
        <asp:Label ID="lblCommandName" runat="server"></asp:Label>
        </td>
    <td>
        <asp:Label ID="lblChannelName" runat="server"></asp:Label>
        </td>
</tr>        

    </table>
    </div>
<br />