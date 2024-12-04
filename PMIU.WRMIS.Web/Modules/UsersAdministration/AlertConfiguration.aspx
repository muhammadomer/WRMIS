<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertConfiguration.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AlertConfiguration" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Alert Configuration</h3>
                        </div>
                        <div class="box-content">
                            <div class="row">
                                <div class="form-horizontal">
                                    <div class="col-md-6 ">
                                        <!-- BEGIN Left Side -->
                                        <div class="form-group">
                                            <asp:Label ID="lblModule" runat="server" Text="Module" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlModule" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" required="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- END Left Side -->
                                    </div>
                                    <div class="col-md-6 ">
                                        <!-- BEGIN Left Side -->
                                        <div class="form-group">
                                            <asp:Label ID="lblEvent" runat="server" Text="Events" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlEvent_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- END Left Side -->
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvAlertConfiguration" runat="server" AutoGenerateColumns="false"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="DesignationID,NotificationConfigID">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Organization">
                                            <HeaderStyle CssClass="col-md-3" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrganization" runat="server" CssClass="control-label" Text='<%# Eval("OrganizationName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <HeaderStyle CssClass="col-md-3" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("DesignationName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- Alert Check Box --%>
                                        <asp:TemplateField HeaderText="Alert">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkNotification" Enabled="<%# base.CanEdit %>" runat="server" CssClass="control-label" Checked='<%# Eval("NotificationName") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- SMS Check Box --%>
                                        <asp:TemplateField HeaderText="SMS">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSms" runat="server" Enabled="<%# base.CanEdit %>" CssClass="control-label" Checked='<%# Eval("SMS") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- Email Check Box --%>
                                        <asp:TemplateField HeaderText="Email">
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEmail" runat="server" Enabled="<%# base.CanEdit %>" CssClass="control-label" Checked='<%# Eval("Email") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" ClientIDMode="Static"></asp:Button>
                                            <asp:LinkButton ID="lbtnCancel" runat="server" Text="Reset" CssClass="btn" OnClick="lbtnCancel_Click" ToolTip="Cancel" />
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
