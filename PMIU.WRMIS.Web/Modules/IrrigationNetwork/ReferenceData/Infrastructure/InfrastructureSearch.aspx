<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfrastructureSearch.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.InfrastructureSearch" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Protection Infrastructures</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" Enabled="False">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" Enabled="False">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureType" runat="server" Text="Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureName" runat="server" Text="Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtInfrastructureName" runat="server" Text="" CssClass="col-sm-8 col-lg-9 form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdolInfrastructureStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                            <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="0">InActive</asp:ListItem>
                                            <asp:ListItem Value="-1">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                                    <asp:HyperLink ID="hlAddNewInfrastructure" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureAddUpdate.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvProtectionInfrastructure" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found " DataKeyNames="InfrastructureTypeName,InfrastructureTypeID"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" OnPageIndexChanged="gvProtectionInfrastructure_PageIndexChanged" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false"
                            OnRowDeleting="gvProtectionInfrastructure_RowDeleting" OnPageIndexChanging="gvProtectionInfrastructure_PageIndexChanging" OnRowDataBound="gvProtectionInfrastructure_RowDataBound" OnRowCommand="gvProtectionInfrastructure_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureID" runat="server" Text='<%# Eval("InfrastructureID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="InfrastructureName" runat="server" Text='<%#Eval("InfrastructureName") %>' NavigateUrl='<%# Eval("InfrastructureID", "~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureInformation.aspx?InfrastructureID={0}")%>'>
                                        </asp:HyperLink>--%>
                                        <asp:Label ID="lblInfrastructureName" runat="server" Text='<%# Eval("InfrastructureName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemStyle CssClass="bold" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="InfrastructureTypeName" runat="server" Text='<%#Eval("InfrastructureTypeName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Length (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="TotalLength" runat="server" Text='<%#Eval("TotalLength") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 padding1 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designed Top Width (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="DesignedTopWidth" runat="server" Text='<%#Eval("DesignedTopWidth") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 padding1 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Design Freeboard (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="DesignedFreeBoard" runat="server" Text='<%#Eval("DesignedFreeBoard") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 nopadding text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="InfrastructureStatus" runat="server" Text='<%#Eval("InfrastructureStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 padding1 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlProtectionInfrastructurePhysicalLocation" runat="server" ToolTip="Physical Locations" CssClass="btn btn-primary btn_24 phyloc" NavigateUrl='<%# Eval("InfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/PhysicalLocation.aspx?InfrastructureID={0}") %>' Text=""></asp:HyperLink>
                                        <asp:HyperLink ID="hlInfrastructureParent" runat="server" ToolTip="Infrastructure Parent" CssClass="btn btn-primary btn_24 infrastructure_parent" NavigateUrl='<%# Eval("InfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureParent.aspx?InfrastructureID={0}") %>' Text=""></asp:HyperLink>
                                        <asp:HyperLink ID="hlInfrastructureBreachingSection" runat="server" ToolTip="Breaching Section" CssClass="btn btn-primary btn_24 pfch" NavigateUrl='<%# Eval("InfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/BreachingSection.aspx?InfrastructureID={0}") %>' Text=""></asp:HyperLink>
                                        <asp:HyperLink ID="hlInfrastructureStoneStock" runat="server" ToolTip="Stone Stock" CssClass="btn btn-primary btn_24 stone_stock" NavigateUrl='<%# Eval("InfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/StoneStock.aspx?InfrastructureID={0}") %>' Text=""></asp:HyperLink>
                                        <asp:HyperLink ID="hlInfrastructurePublicRepresentative" runat="server" ToolTip="Public Representative" CssClass="btn btn-primary btn_24 public_reps" NavigateUrl='<%# Eval("InfrastructureID","~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/PublicRepresentative.aspx?InfrastructureID={0}") %>' Text=""></asp:HyperLink>
                                        <asp:HyperLink ID="hlView" runat="server" ToolTip="View" CssClass="btn btn-primary btn_24 view" NavigateUrl='<%# Eval("InfrastructureID", "~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureInformation.aspx?InfrastructureID={0}")%>'></asp:HyperLink>
                                        <asp:Button ID="btnGauges" runat="server" ToolTip="Gauges" CssClass="btn btn-primary btn_24 gauge" CommandName="btnGauge" CommandArgument='<%# Eval("InfrastructureID") %>' Text="" Enabled="False"></asp:Button>
                                        <asp:HyperLink ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("InfrastructureID", "~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureAddUpdate.aspx?InfrastructureID={0}")%>'></asp:HyperLink>
                                        <asp:LinkButton ID="linkButtonDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3 text-center" />
                                    <ItemStyle CssClass="text-right" />
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

    <asp:HiddenField ID="hdnInfrastructureID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
    <style type="text/css">
        .nopadding {
            padding: 0 !important;
        }

        .padding1 {
            padding: 1px !important;
        }
    </style>
</asp:Content>
