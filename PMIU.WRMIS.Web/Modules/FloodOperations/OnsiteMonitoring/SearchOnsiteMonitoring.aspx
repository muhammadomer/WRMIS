<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchOnsiteMonitoring.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring.SearchOnsiteMonitoring" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Onsite Monitoring</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass=" rquired form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
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
                                <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control">
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
                            <%--  <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/FloodInspection/IndependentInspection/AddIndependent.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvOnsiteMonitoring" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" ShowHeaderWhenEmpty="True" AllowPaging="True"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanging="gvOnsiteMonitoring_PageIndexChanging"
                            OnPageIndexChanged="gvOnsiteMonitoring_PageIndexChanged" DataKeyNames="FFPID,Year,Zone,Circle,Division,InfrastructureType,InfrastructureName,DivisionID,StructureTypeID,StructureID" OnRowCommand="gvOnsiteMonitoring_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionSummaryID" runat="server" Text='<%# Eval("FFPID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Label ID="Year" runat="server" Text='<%#Eval("Year") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle CssClass="bold" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZone" runat="server" Text='<%#Eval("Zone") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircle" runat="server" Text='<%#Eval("Circle") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureType" runat="server" Text='<%#Eval("InfrastructureType").ToString()=="Control Structure1"?"Barrage/Headwork":Eval("InfrastructureType") %>'>
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
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hlStonePosition" runat="server" ToolTip="Stone Position" CommandName="StonePosition" CssClass="btn btn-primary btn_24 stnpos">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="hlCampSite" runat="server" ToolTip="Camp Site" CommandName="CampSite" CssClass="btn btn-primary btn_24 camp_sites">
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
            <asp:HiddenField ID="hdnFFPID" runat="server" Value="0" />
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
