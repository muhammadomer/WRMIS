<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="SearchEntitlements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.SearchEntitlements" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Search Entitlement</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Command</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlCommand" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Canal System</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlCanalSystem" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlCanalSystem_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Main Canal</label>
                                    <div class="col-sm-4 col-lg-4 controls" style="float: left;">
                                        <asp:DropDownList CssClass="form-control" ID="ddlMainCanal" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlMainCanal_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5 col-lg-5 controls" visible="false" runat="server" id="dvMainSub">
                                        <%--<label class="col-sm-4 col-lg-3 control-label">Any Label</label>--%>
                                        <asp:DropDownList CssClass="form-control" ID="ddlMainCanalSub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMainCanal_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Branch Canal</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlBranch" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5 col-lg-5 controls" visible="false" runat="server" id="dvBranchSub">
                                        <asp:DropDownList CssClass="form-control" ID="ddlBranchSub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Distributary</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDistributry" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlDistributry_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5 col-lg-5 controls" visible="false" runat="server" id="dvDistSub">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDistributrySub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDistributry_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Minor</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlMinor" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlMinor_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5 col-lg-5 controls" visible="false" runat="server" id="dvMinorSub">
                                        <asp:DropDownList CssClass="form-control" ID="ddlMinorSub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMinor_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Sub Minor</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSubMinor" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMinor_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-5 col-lg-5 controls" visible="false" runat="server" id="dvSMinorSub">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSubMinorSub" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMinor_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Season</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlYear" Enabled="false" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 ">
                            <div class="table-responsive">
                                <asp:GridView ID="gvSearchEntitlements" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowFooter="true"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="true"
                                    OnRowDataBound="gvSearchEntitlements_RowDataBound" OnPageIndexChanging="gvSearchEntitlements_PageIndexChanging"
                                    DataKeyNames="ChannelID,Season,Year,ParentChild">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Channel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelType" runat="server" CssClass="control-label" Text='<%# Eval("ChannelType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Flow Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFlowType" runat="server" CssClass="control-label" Text='<%# Eval("FlowType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Season">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSeason" runat="server" CssClass="control-label" Text='<%# Eval("Season") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYear" runat="server" CssClass="control-label" Text='<%# Eval("Year") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 text-center" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Entitlement (MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMAFEntitlement" runat="server" CssClass="control-label" Text='<%# Eval("MAFEntitlement") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 text-right" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Delivery (MAF)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMAFDistribution" runat="server" CssClass="control-label" Text='<%# Eval("MAFDistribution") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-right" />
                                            <HeaderStyle CssClass="col-md-1 text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlView" runat="server" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# String.Format("~/Modules/EntitlementDelivery/ViewEntitlements.aspx?ChannelID={0}&SeasonID={1}&Year={2}&ParentChild={3}&CommandID={4}", Convert.ToString(Eval("ChannelID")),Convert.ToString(Eval("Season")),Convert.ToString(Eval("Year")),ParentChild,ddlCommand.SelectedItem.Value) %>' ToolTip="View"></asp:HyperLink>
                                                <asp:LinkButton ID="lbtnPrint" runat="server" CssClass="btn btn-primary btn_24 print" ToolTip="Print" OnClick="lbtnPrint_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-center" />
                                            <HeaderStyle CssClass="col-md-1 text-center" />
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
        </div>
    </div>

</asp:Content>
