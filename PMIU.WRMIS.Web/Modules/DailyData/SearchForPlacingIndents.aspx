<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchForPlacingIndents.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.DailyData.SearchForPlacingIndents" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Search For Placing Indents</h3>

                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label ID="lblCommandName" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Command Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCommandName" class="form-control" runat="server" TabIndex="1"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblFlowType" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Flow Type</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFlowType" class="form-control" runat="server" TabIndex="3"></asp:DropDownList>
                                    </div>
                                </div>

                                <!-- END Left Side -->
                            </div>

                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->

                                <div class="form-group">
                                    <asp:Label ID="lblChannelType" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Channel Type</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlChannelType" class="form-control" runat="server" TabIndex="2"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblChannelName" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Channel Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlChannelName" class="form-control" runat="server" TabIndex="4"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearchChannel" Text="Search Channel" CssClass="btn btn-primary" runat="server" OnClick="btnSearchChannel_Click" />
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive">

                                    <asp:GridView ID="gvPlacingIndents" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" OnPageIndexChanging="gvPlacingIndents_PageIndexChanging" OnRowDataBound="gvPlacingIndents_RowDataBound"
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("Channel.ID") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("Channel.Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelType" runat="server" CssClass="control-label" Text='<%# Eval("Channel.CO_ChannelType.Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flow Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlowType" runat="server" CssClass="control-label" Text='<%# Eval("Channel.CO_ChannelFlowType.Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total RDs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalRD" runat="server" CssClass="control-label" Text='<%# Eval("Channel.TotalRDs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Command Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommandName" runat="server" CssClass="control-label" Text='<%# Eval("Channel.CO_ChannelComndType.Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Placement Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentPlacementDate" runat="server" CssClass="control-label" Text='<%# Eval("Date") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate >
                                                    <asp:HyperLink ID="hlIndent" runat="server" CssClass="btn btn-primary btn_24 indents" NavigateUrl='<%# String.Format("~/Modules/DailyData/AddIndent.aspx?ChannelID={0}&SubDivID={1}", Convert.ToString(Eval("Channel.ID")),Convert.ToString(Eval("SubDivID"))) %>' ToolTip="Indent"></asp:HyperLink>
                                                    <asp:HyperLink ID="hlIndentHistory" runat="server" CssClass="btn btn-primary btn_24 indents-history" NavigateUrl='<%# String.Format("~/Modules/DailyData/IndentHistory.aspx?ChannelID={0}&SubDivID={1}", Convert.ToString(Eval("Channel.ID")),Convert.ToString(Eval("SubDivID"))) %>' ToolTip="Indent History"></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
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
    </div>
</asp:Content>
