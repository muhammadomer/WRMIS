<%@ Page Title="SearchUser" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SearchUser.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.SearchUser" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>User Information</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">User Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtUserName" runat="server" class="form-control" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtFullName" runat="server" class="form-control" type="text" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server" AutoPostBack="true">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                            <asp:ListItem Value="true">Active</asp:ListItem>
                                            <asp:ListItem Value="false">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Role</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlRole" runat="server">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Organization</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlOrganization" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Designation</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDesignation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged" Enabled="false">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="divZone" runat="server" class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="divCircle" runat="server" class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="divDivision" runat="server" class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="divSubDivision" runat="server" class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divSection" runat="server" class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Section</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSection" runat="server">
                                            <asp:ListItem Text="All" Value="" Selected="True" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary"></asp:Button>
                                    <asp:Button ID="btnAddUser" runat="server" OnClick="btnAddUser_Click" Text="Add User" CssClass="btn btn-success"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView CssClass="table header" ID="gvSearchUser" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvSearchUser_PageIndexChanging" OnRowEditing="gvSearchUser_RowEditing"
                            AllowPaging="True" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowDataBound="gvSearchUser_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("LoginName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Full Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                        <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Organization">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrganization" runat="server" Text='<%# Eval("UA_Designations.UA_Organization.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("UA_Designations.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("UA_Roles.Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLevelID" runat="server" Text='<%# Eval("UA_Designations.IrrigationLevelID") %>' Visible="false"></asp:Label>
                                        <asp:HyperLink ID="hlAssociation" runat="server" CssClass="btn btn-primary btn_32 phyloc" NavigateUrl='<%# String.Format("~/Modules/UsersAdministration/AssociateLocationToUser.aspx?UserID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Location"></asp:HyperLink>
                                        <asp:HyperLink ID="hlBarrage" runat="server" CssClass="btn btn-primary btn_32 barrage" NavigateUrl='<%# String.Format("~/Modules/UsersAdministration/AssociateBarragesChannelsAndOutlets.aspx?UserID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Barrage Channel Outlets"></asp:HyperLink>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>" />
                                        <asp:HyperLink ID="btnResetPassword" runat="server" CommandName="ResetPassword" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 Clone" ToolTip="Reset Password" Visible="<%# base.CanEdit %>" NavigateUrl='<%# String.Format("~/Modules/UsersAdministration/ReSetPassword.aspx?UserID={0}", Convert.ToString(Eval("ID"))) %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
