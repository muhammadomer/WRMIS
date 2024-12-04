<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure.Search" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Controls/DataPaging.ascx" TagPrefix="uc1" TagName="DataPaging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <div class="box">
                    <div class="box-title">
                        <h3>Barrage/Headwork Search</h3>
                    </div>
                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-12">
                                    <!-- BEGIN Left Side -->
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" CssClass="form-control" data-rule-required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" CssClass="form-control" data-rule-required="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="True" Enabled="false">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" data-rule-required="true" AutoPostBack="True" Enabled="false">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <label for="txtName" id="lblName" class="col-xs-4 col-lg-3 control-label">Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtName" runat="server" placeholder="Control Infrastructure Name" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 text-left form-group">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-xs-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                                    <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                                    <asp:ListItem Value="0">InActive</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="fnc-btn">
                                                <asp:LinkButton TabIndex="10" ID="btnSearch" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="StoreSearchCriteria()" OnClick="btnControlInfrastructureSearch_Click"><%--<i class="fa fa-search"></i>--%>&nbsp;Search</asp:LinkButton>
                                                <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/AddNew.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvControlInfrastructure" runat="server" DataKeyNames="ControlInfrastructureID" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                    ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                                    OnRowDataBound="gvControlInfrastructure_RowDataBound" OnRowDeleting="gvControlInfrastructure_RowDeleting" OnPageIndexChanging="gvControlInfrastructure_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblControlInfrastructureID" runat="server" Text='<%# Eval("ControlInfrastructureID") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Barrage Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="hplBarrageName" runat="server" Text='<%#Eval("ControlInfrastructureName") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Barrage Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBarrageType" runat="server" Text='<%#Eval("InfrastructureTypeName") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Province">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProvince" runat="server" Text='<%#Eval("ProvinceName") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="River">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRiver" runat="server" Text='<%#Eval("RiverName") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Barrage Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBarrageStatus" runat="server" Text='<%#Eval("ControlInfrastructureStatus") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction" >
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hlControlInfrastructurePhysicalLocation" runat="server" ToolTip="Physical Locations" CssClass="btn btn-primary btn_24 phyloc" NavigateUrl='<%# Eval("ControlInfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/PhysicalLocation.aspx?ControlInfrastructureID={0}") %>' Text="">
                                                                </asp:HyperLink>
                                                                <asp:HyperLink ID="hlTechnicalParameters" runat="server" ToolTip="Technical Parameters" CssClass="btn btn-primary btn_24 technical_parameters" NavigateUrl='<%# Eval("ControlInfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/TechnicalParameters.aspx?ControlInfrastructureID={0}") %>' Text="">
                                                                </asp:HyperLink>
                                                                <asp:HyperLink ID="hlGauges" runat="server" ToolTip="Gauges" CssClass="btn btn-primary btn_24 gauge" NavigateUrl='<%# Eval("ControlInfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/Gauges.aspx?ControlInfrastructureID={0}") %>' Text="">
                                                                </asp:HyperLink>
                                                                <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("ControlInfrastructureID", "~/Modules/IrrigationNetwork/ReferenceData/ControlledInfrastructure/AddNew.aspx?ControlInfrastructureID={0}")%>'>
                                                                </asp:HyperLink>
                                                                <asp:LinkButton ID="linkButtonDelete" Visible="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-2 text-center" />
                                                            <ItemStyle CssClass="text-right" />
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
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnControlInfrastructuresID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
    </div>
</asp:Content>
