<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="PMIU.WRMIS.Web.AccessDenied" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlContent" runat="server" HorizontalAlign="Center">
        <h3>
            <span style="text-align: center; color: red;" runat="server" id="msg">You don't have access to this page.</span>
        </h3>
        <br />
        <asp:Button ID="btnback" Text="Back" runat="server" CssClass="btn" OnClientClick="javascript:history.go(-1); return false;" />
    </asp:Panel>
</asp:Content>
