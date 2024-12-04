<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntitlementDeliveryDirection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.EntitlementDeliveryDirection" MaintainScrollPositionOnPostback="true" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Entitlements and Delivery Direction</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <asp:Label ID="lblHMAF5y" runat="server" Style="font-size: 20px; font-weight: 500;" CssClass="control-label" Text="Generate Entitlement based on" />
                                </div>

                                <div class="col-md-2">
                                    <asp:RadioButton ID="rbEntitlement" runat="server" Checked="true" CssClass="control-label" GroupName="Entitlement" />
                                    <asp:Label ID="Label1" runat="server" CssClass="control-label" Text="Entitlement" />
                                </div>
                                <div class="col-md-2">
                                    <asp:RadioButton ID="rbDelivery" runat="server" CssClass="control-label" GroupName="Entitlement" />
                                    <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Delivery" />
                                </div>
                                <div class="col-md-4">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnGoTo" runat="server" CssClass="btn btn-primary" Text="Go To" OnClick="btnGoTo_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br>
                        <div class="row">
                            <asp:Panel ID="pnlEdit" runat="server"  CssClass="text-primary text-center">
                                <asp:Label ID="lblEditIndus" runat="server" />
                            </asp:Panel>
                            <asp:Panel ID="Panel3" runat="server"  CssClass="text-primary text-center">
                                <asp:Label ID="lblEditJC" runat="server" />
                            </asp:Panel>
                            <asp:Panel ID="Panel1" runat="server"  CssClass="text-primary text-center">
                                 <asp:Label ID="lblEditEK" runat="server" />
                            </asp:Panel>

                            <asp:Panel ID="Panel2" runat="server"  CssClass="text-primary text-center">
                                <asp:Label ID="lblEditLk" runat="server" />
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
