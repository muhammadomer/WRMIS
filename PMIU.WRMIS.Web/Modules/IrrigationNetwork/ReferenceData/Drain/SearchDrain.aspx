<%@ Page Title="Search Drain" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SearchDrain.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain.SearchDrain" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Search Drain</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                    </div>
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
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" Enabled="False">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDrainName" runat="server" Text="Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDrainName" runat="server" Text="" CssClass="col-sm-8 col-lg-9 form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:HyperLink ID="hlAddNewInfrastructure" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/Drain/AddDrain.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvDrainSearch" runat="server" DataKeyNames="DrainID" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" OnRowDeleting="gvDrainSearch_RowDeleting" OnPageIndexChanging="gvDrainSearch_PageIndexChanging"
                            OnPageIndexChanged="gvDrainSearch_PageIndexChanged" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDrainID" runat="server" Text='<%# Eval("DrainID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="DrainName" runat="server" Text='<%#Eval("DrainName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Length (ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="DrainLength" runat="server" Text='<%#Eval("DrainLength") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Catchment Area (sq.ft)">
                                    <ItemTemplate>
                                        <asp:Label ID="CatchmentArea" runat="server" Text='<%#Eval("CatchmentArea") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Major Build up Area Name">
                                    <ItemTemplate>
                                        <asp:Label ID="MajorBuildUpArea" runat="server" Text='<%#Eval("MajorBuildUpArea") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <%--   <asp:TemplateField HeaderText="Drain Status">
                                    <ItemTemplate>
                                        <asp:Label ID="DrainStatus" runat="server" Text='<%#Eval("DrainStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />

                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlProtectionInfrastructurePhysicalLocation" runat="server" ToolTip="Physical Locations" CssClass="btn btn-primary btn_24 phyloc" NavigateUrl='<%# Eval("DrainID","~/Modules/IrrigationNetwork/ReferenceData/Drain/DrainPhysicalLocation.aspx?DrainID={0}") %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlOutfallDrainDetails" runat="server" ToolTip="Outfall" CssClass="btn btn-primary btn_24 outfall" NavigateUrl='<%# Eval("DrainID","~/Modules/IrrigationNetwork/ReferenceData/Drain/OutfallDetailsDrain.aspx?DrainID={0}") %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("DrainID", "~/Modules/IrrigationNetwork/ReferenceData/Drain/AddDrain.aspx?DrainID={0}")%>'>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnDrainID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />

</asp:Content>
