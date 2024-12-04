<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchReceivedStock.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.SearchReceivedStock" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Search Received Items</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="box-content">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
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
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
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
                                <asp:Label ID="lblReceivedStockType" runat="server" Text="Receive Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList runat="server" ID="ddlReceivedStockType" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlReceivedStockType_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    
                        <div  runat="server" id="Div_category" visible="false" class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblCampSite" runat="server" Text="Item Category" CssClass="col-lg-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList runat="server" ID="ddlItemCategory" CssClass="rquired form-control" AutoPostBack="True" required="required" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged"><%--OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">--%>
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
                                <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/DivisionStore/ReceivedStockReturnPurchased.aspx" CssClass="btn btn-success">&nbsp;Receive Stock</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div id="PurchaseFloodFightingPlan" runat="server" visible="False">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvPurchaseFloodFightingPlan" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvPurchaseFloodFightingPlan_PageIndexChanging"
                                    OnPageIndexChanged="gvPurchaseFloodFightingPlan_PageIndexChanged">
                                    <Columns>
                                        <%--<asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionSummaryID" runat="server" Text='<%# Eval("FFPStonePositionID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="itemName" runat="server" Text='<%#Eval("itemsName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity Approved (FFP)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityApprovedFFP" runat="server" Text='<%#Eval("QuantityApproved") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity Received">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityReceived" runat="server" Text='<%#Eval("QuantityReceived") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div id="ReturnPurchaseDuringFlood" runat="server" visible="False">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvReturnPurchaseDuringFlood" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvReturnPurchaseDuringFlood_PageIndexChanging"
                                    OnPageIndexChanged="gvReturnPurchaseDuringFlood_PageIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="itemName" runat="server" Text='<%#Eval("itemName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity Purchased">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityPurchased" runat="server" Text='<%#Eval("QuantityPurchased") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity Received">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantityReceived" runat="server" Text='<%#Eval("RQuantityReceived") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div id="ReturnInfrastructure" runat="server" visible="False">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvReturnInfrastructure" runat="server" AutoGenerateColumns="False" DataKeyNames="InfraStructureType,InfrastructureName,StructureTypeID,StructureID" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvReturnInfrastructure_PageIndexChanging"
                                    OnPageIndexChanged="gvReturnInfrastructure_PageIndexChanged" OnRowCommand="gvReturnInfrastructure_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Infrastructure Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfraStructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfraStructureType") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Infrastructure Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfrastructureName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="hlDetails" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
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

                    <div id="ReturnCampSite" runat="server" visible="False">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvReturnCampSite" runat="server" AutoGenerateColumns="False" DataKeyNames="InfraStructureType,InfrastructureName,CampSiteRD,StructureTypeID,StructureID,FFPcampSiteID" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvReturnCampSite_PageIndexChanging"
                                    OnPageIndexChanged="gvReturnCampSite_PageIndexChanged" OnRowCommand="gvReturnCampSite_RowCommand" OnRowDataBound="gvReturnCampSite_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Infrastructure Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfraStructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfrastructureType") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Infrastructure Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfrastructureName") %>'>
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
                                                <asp:LinkButton ID="hlDetailCam" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
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
            </div>
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
