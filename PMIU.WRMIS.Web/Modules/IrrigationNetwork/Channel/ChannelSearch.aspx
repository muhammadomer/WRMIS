<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelSearch.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.ChannelSearch" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Controls/DataPaging.ascx" TagPrefix="uc1" TagName="DataPaging" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Channel Search</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <%--        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <!-- BEGIN Left Side -->

                        <div class="form-group">
                            <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" CssClass="form-control" data-rule-required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="3" ID="ddlDivision" runat="server" CssClass="form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="ddlCommanName" id="lblCommandName" class="col-sm-4 col-lg-3 control-label">Command Name</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="5" ID="ddlCommanName" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlFlowType" id="lblFlowType" class="col-sm-4 col-lg-3 control-label">Flow Type</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="7" ID="ddlFlowType" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlParentChannel" id="lblParentChannel" class="col-sm-4 col-lg-3 control-label">Parent Channel</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="9" ID="ddlParentChannel" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                            </div>
                        </div>

                        <!-- END Left Side -->
                    </div>
                    <div class="col-md-6 ">
                        <!-- BEGIN Right Side -->

                        <div class="form-group">
                            <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" CssClass="form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="4" ID="ddlSubDivision" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlChannelType" id="lblChannelType" class="col-sm-4 col-lg-3 control-label">Channel Type</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="6" ID="ddlChannelType" runat="server" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtChannelName" id="lblChannelName" class="col-xs-4 col-lg-3 control-label">Channel Name</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox TabIndex="8" ID="txtChannelName" runat="server" placeholder="Channel Name" class="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtIMISCode" id="lblIMISCode" class="col-xs-4 col-lg-3 control-label">IMIS Code</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox TabIndex="10" ID="txtIMISCode" runat="server" placeholder="IMIS Code" class="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- END Right Side -->
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:LinkButton TabIndex="10" ID="btnChannelSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="StoreSearchCriteria()" OnClick="btnChannelSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                            <asp:HyperLink ID="hlAddNewChannel" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/Channel/ChannelAddition.aspx" CssClass="btn btn-success">&nbsp;Add New Channel</asp:HyperLink>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChannel" runat="server" DataKeyNames="ChannelID,ParentFeederID" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                OnRowDataBound="gvChannel_RowDataBound" OnRowDeleting="gvChannel_RowDeleting" OnPageIndexChanging="gvChannel_PageIndexChanging" OnRowCommand="gvChannel_RowCommand">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelID" runat="server" Text='<%# Eval("ChannelID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelName" runat="server" Text='<%#Eval("ChannelName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="160px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Type">
                                        <ItemTemplate>
                                            <asp:Label ID="ChannelType" runat="server" Text='<%# Eval("ChannelType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flow Type">
                                        <ItemTemplate>
                                            <asp:Label ID="FlowType" runat="server" Text='<%# Eval("FlowType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total RDs">
                                        <ItemTemplate>
                                            <asp:Label ID="TotalRDs" runat="server" Text='<%# Eval("TotalRDs") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Command Name">
                                        <ItemTemplate>
                                            <asp:Label ID="CommandName" runat="server" Text='<%# Eval("CommandName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CCA">
                                        <ItemTemplate>
                                            <asp:Label ID="CCAAcre" runat="server" Text='<%# Eval("CCAAcre") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GCA">
                                        <ItemTemplate>
                                            <asp:Label ID="GCAAcre" runat="server" Text='<%# Eval("GCAAcre") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlChannelPhysicalLocation" runat="server" ToolTip="Physical Locations" CssClass="btn btn-primary btn_24 phyloc" NavigateUrl='<%# Eval("ChannelID","~/Modules/IrrigationNetwork/Channel/ChannelPhysicalLocation.aspx?ChannelID={0}") %>' Text="">
                                            </asp:HyperLink>

                                            <asp:HyperLink ID="hlGauges" runat="server" ToolTip="Gauges" CssClass="btn btn-primary btn_24 gauge" NavigateUrl='<%# Eval("ChannelID","~/Modules/IrrigationNetwork/Channel/GaugeInformation.aspx?ChannelID={0}") %>' Text="">
                                                        
                                            </asp:HyperLink>

                                            <asp:HyperLink ID="hlParentChannelsChannelFeeders" runat="server" ToolTip="Parent/Feeders" CssClass="btn btn-primary btn_24 pfch" NavigateUrl='<%# Eval("ChannelID","~/Modules/IrrigationNetwork/Channel/ChannelParentFeeder.aspx?ChannelID={0}") %>' Text="">
                                                        
                                            </asp:HyperLink>

                                            <asp:HyperLink ID="hlOutlets" runat="server" ToolTip="Outlets" CssClass="btn btn-primary btn_24 outlet" NavigateUrl='<%# Eval("ChannelID","~/Modules/IrrigationNetwork/Outlet/OutletView.aspx?ChannelID={0}") %>' Text="">
                                                        
                                            </asp:HyperLink>

                                            <asp:HyperLink ID="hlReach" runat="server" ToolTip="Reaches" CssClass="btn btn-primary btn_24 reach" NavigateUrl='<%# Eval("ChannelID","~/Modules/IrrigationNetwork/Reach/DefineChannelReach.aspx?ChannelID={0}") %>' Text="">
                                            </asp:HyperLink>
                                            <asp:Button ID="btnlIMIS" runat="server" ToolTip="IMIS" CssClass="btn btn-primary btn_24 tick" CommandName="IMISCode" CommandArgument='<%# Eval("ChannelID") %>' Text="" Enabled="False"></asp:Button>

                                            <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("ChannelID", "~/Modules/IrrigationNetwork/Channel/ChannelAddition.aspx?ChannelID={0}")%>'>
                                            </asp:HyperLink>
                                            <asp:LinkButton ID="linkButtonDelete" Visible="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete">
                                                        
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="24px" />
                                        <ItemStyle Width="285px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                        <uc1:DataPaging runat="server" Visible="false" ID="pgrGrid" PageIndex="1" RecordsPerPage="10" TotalRecords="0" />
                    </div>
                </div>
            </div>
        </div>
        <%--            </ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
    </div>
</asp:Content>
