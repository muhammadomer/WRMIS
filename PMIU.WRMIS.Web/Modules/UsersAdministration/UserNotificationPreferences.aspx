<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UserNotificationPreferences.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.UserNotificationPreferences" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>My Preferences</h3>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <asp:GridView ID="gvUserNotificationPreferences" runat="server" AutoGenerateColumns="false"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="UserNotificationConfigID,EventID"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No record found">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Modules">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblModuleName" runat="server" CssClass="control-label" Text='<%# Eval("ModuleName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Events">
                                            <HeaderStyle CssClass="col-md-3" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblEvent" runat="server" CssClass="control-label" Text='<%# Eval("EventName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- Alert Check Box --%>
                                        <asp:TemplateField HeaderText="Alert">
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkNotification" Enabled="false" runat="server" CssClass="control-label" Checked='<%# Eval("Alert") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- SMS Check Box --%>
                                        <asp:TemplateField HeaderText="SMS">
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSms" runat="server" Enabled='<%# Eval("IsSMSByDefaultEnabled") %>' CssClass="control-label" Checked='<%# Eval("SMS") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- Email Check Box --%>
                                        <asp:TemplateField HeaderText="Email">
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEmail" runat="server" Enabled='<%# Eval("IsEmailByDefaultEnabled") %>' CssClass="control-label" Checked='<%# Eval("Email") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click"></asp:Button>
                                            <asp:LinkButton ID="lnkCancel" OnClick="lnkCancel_Click" runat="server" Text="Reset" CssClass="btn" ToolTip="Cancel" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
