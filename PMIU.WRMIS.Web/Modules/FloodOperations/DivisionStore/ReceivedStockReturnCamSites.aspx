<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivedStockReturnCamSites.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ReceivedStockReturnCamSites" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Division Store Received Stock</h3>
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
                                <asp:Label ID="lblZone" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="rquired form-control" AutoPostBack="True" required="required" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="Received Stock Type" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlStockType" runat="server" CssClass=" rquired form-control" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddlStockType_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="table-responsive">

                        <asp:GridView ID="gvCampSite" runat="server" DataKeyNames="InfraStructureName,InfraStructureType,RD,StructureTypeID,StructureID"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvCampSite_RowDataBound" OnRowCommand="gvCampSite_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%# (Convert.ToString(Eval("InfraStructureType"))) == "Control Structure1" ? "Barrage/Headwork": Eval("InfraStructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureName" runat="server" Text='<%#Eval("InfraStructureName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="RD">
                                   <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"></ItemStyle>
                                    <HeaderStyle CssClass="col-md-3 text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hlDetails" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary"  Text="Items">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                </div>
            </div>
            <asp:HiddenField ID="hdnID" runat="server" Value="0" />
        </div>

    </div>
</asp:Content>
