<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchIssuedStock.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.SearchIssuedStock" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3 id="htitle" runat="server">Search Issued Item </h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control required" required="true">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control required" required="true">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDivisionStoreissue" runat="server" Text="Issued Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivisionStoreissueType" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlDivisionStoreissueType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            <asp:Button ID="btnIssuenewStock" runat="server" CssClass="btn btn-success" Text="Issue new Stock" UseSubmitBehavior="False" OnClick="btnIssuenewStock_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="IssuedStockInfrastructure" runat="server" visible="False">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvIssuedStockInfrastructure" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="InfraStructureType,InfraStructureName,StructureTypeID" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvIssuedStockInfrastructure_PageIndexChanging"
                                OnPageIndexChanged="gvIssuedStockInfrastructure_PageIndexChanged" OnRowCommand="gvIssuedStockInfrastructure_RowCommand1"><%-- OnRowCommand="gvIssuedStockInfrastructure_RowCommand">--%>
                            <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblDivisionSummaryID" runat="server" Text='<%# Eval("FFPStonePositionID") %>'>
                                        </asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfraStructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfraStructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfraStructureName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="hlInfrastructureItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" NavigateUrl="~/Modules/FloodOperations/DivisionStore/IssuedStockViewCampSiteItems.aspx" Text="Items"></asp:HyperLink>--%>
                                            <asp:LinkButton ID="hlInItems" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center col-md-1" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="IssuedStockCampSite" runat="server" visible="False">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvIssuedStockCampSite" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="InfraStructureType,InfraStructureName,CampSiteRD,FFPCampSiteID,StructureTypeID,RD" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvIssuedStockCampSite_PageIndexChanging"
                                OnPageIndexChanged="gvIssuedStockCampSite_PageIndexChanged" OnRowCommand="gvIssuedStockCampSite_RowCommand" OnRowDataBound="gvIssuedStockCampSite_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblDivisionSummaryID" runat="server" Text='<%# Eval("FFPStonePositionID") %>'>
                                        </asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfraStructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfrastructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfraStructureName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%#Eval("CampSiteRD") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="hlInfrastructureItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" NavigateUrl="~/Modules/FloodOperations/DivisionStore/IssuedStockViewCampSiteItems.aspx" Text="Items"></asp:HyperLink>--%>
                                            <asp:LinkButton ID="hlInfrastructureItems" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="text-center col-md-1" />
                                        <ItemStyle CssClass="text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnStonePositionID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
        </div>
    </div>

    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };
    </script>
</asp:Content>
