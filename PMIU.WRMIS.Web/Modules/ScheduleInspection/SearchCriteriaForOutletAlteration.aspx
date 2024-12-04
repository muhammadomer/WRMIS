<%@ Page Title="Roles" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchCriteriaForOutletAlteration.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.SearchCriteriaForOutletAlteration" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">

                    <div class="box">
                        <div class="box-title">
                            <h3>Search Criteria For Outlet Alteration</h3>
                        </div>
                        <div class="box-content" id="divMain" runat="server">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Command Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlCommandName" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Channel Type</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlChannelType" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Flow type</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlFlowType" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Channel Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlChannelName" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" Text="Search Channel" runat="server" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="table-responsive">

                                <asp:GridView ID="gvChannels" runat="server" AutoGenerateColumns="False" EmptyDataText="No Channel is found for the given criteria"
                                    ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                    CellSpacing="-1" GridLines="None" OnRowEditing="gvChannels_RowEditing" OnPageIndexChanged="gvChannels_PageIndexChanged"
                                    OnPageIndexChanging="gvChannels_PageIndexChanging">
                                    <Columns>

                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelType" runat="server" Text='<%# Eval("ChannelType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Flow type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFlowType" runat="server" Text='<%# Eval("FlowType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total R.Ds">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalRDs" runat="server" Text='<%# Eval("TotalRds") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Command Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommandName" runat="server" Text='<%# Eval("CommandName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Outlets in Sections">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlAssociation" runat="server" CssClass="btn btn-primary btn_24 outlet" ToolTip="Outlets" NavigateUrl='<%# String.Format("~/Modules/ScheduleInspection/CriteriaForSpecificOutletAlteration.aspx?ChannelID={0}", Convert.ToString(Eval("ID"))) %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
