<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotificationsLimit.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.NotificationsLimit" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Notifications Limit</h3>

                        </div>
                        <div class="box-content">

                            <div class="form-horizontal">

                                <div class="row">
                                    <div class="col-md-6">

                                        <div class="form-group">
                                            <asp:Label ID="lblNotificationLimit" runat="server" CssClass="col-sm-4 col-lg-3 control-label">Show Notifications of last</asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls">

                                                <asp:DropDownList ID="ddlNotificationLimit" CssClass="form-control required" required="true" AutoPostBack="true" runat="Server">
                                                </asp:DropDownList>

                                            </div>
                                        </div>

                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <asp:Label ID="Days" runat="server" class="col-lg-1 control-label">days.</asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Label ID="lblID" runat="server" Visible="false" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" ClientIDMode="Static"></asp:Button>
                                            <asp:LinkButton ID="lbtnCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="lbtnCancel_Click" ToolTip="Cancel" />
                                        </div>
                                    </div>
                                </div>

                                <br>
                                <br>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
