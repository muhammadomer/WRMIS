<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExcludeChannel.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.ExcludeChannel" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Exclude Channels from Performance Evaluation</h3>
                        </div>
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlSubDivision" runat="server" TabIndex="4">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Command Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlCommandName" runat="server" TabIndex="5">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Channel Type</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlChannelType" runat="server" TabIndex="6">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Flow Type</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlFlowType" runat="server" TabIndex="7">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Channel Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtChannelName" runat="server" placeholder="Channel Name" class="form-control" TabIndex="8" MaxLength="150" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Parent Channel</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control" ID="ddlParentChannel" runat="server" TabIndex="9">
                                                    <asp:ListItem Text="All" Value="" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">IMIS Code</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtIMISCode" runat="server" placeholder="IMIS Code" class="form-control" TabIndex="10" MaxLength="17" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-left: 0px;">Excluded Channels</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <label class="checkbox-inline" style="padding-top: 0px;">
                                                    <asp:CheckBox runat="server" ID="chkExclude" Text=" " TabIndex="11" />
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label" style="padding-left: 0px;">Zero Authorized Tail</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <label class="checkbox-inline" style="padding-top: 0px;">
                                                    <asp:CheckBox runat="server" ID="chkAuthorizedTail" Text=" " TabIndex="12" />
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button TabIndex="10" ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvExcludeChannel" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                                GridLines="None" DataKeyNames="ID,ChannelID,IsExcluded,TotalRDs" OnPageIndexChanging="gvExcludeChannel_PageIndexChanging"
                                                OnRowDataBound="gvExcludeChannel_RowDataBound" PageSize="100">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Division">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDivision" runat="server" CssClass="control-label" Text='<%# Eval("Division") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IMIS Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIMISCode" runat="server" CssClass="control-label" Text='<%# Eval("IMISCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Channel Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Total R.Ds" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalRDs" runat="server" CssClass="control-label" Text='<%# Eval("TotalRDs") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <ItemStyle CssClass="text-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Command Name" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCommandName" runat="server" CssClass="control-label" Text='<%# Eval("CommandName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CCA" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCCA" runat="server" CssClass="control-label" Text='<%# Eval("CCA") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <ItemStyle CssClass="text-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GCA" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGCA" runat="server" CssClass="control-label" Text='<%# Eval("GCA") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <ItemStyle CssClass="text-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Authorized Tail Value">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAuthorizedTail" runat="server" CssClass="control-label" Text='<%# Eval("AuthorizedTail") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                        <ItemStyle CssClass="text-right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tail Analysis">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTailAnalysis" runat="server" CssClass="control-label"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exclude" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div class="controls">
                                                                <asp:CheckBox ID="chkIsExcluded" runat="server" Checked='<%# Eval("IsExcluded") %>' OnCheckedChanged="chkIsExcluded_CheckedChanged" AutoPostBack="true" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <div class="controls">
                                                                <asp:Label ID="lblExclude" runat="server" Text="Exclude" />
                                                            </div>
                                                            <div class="controls">
                                                                <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" />
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button TabIndex="10" ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" Visible="false" />
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
