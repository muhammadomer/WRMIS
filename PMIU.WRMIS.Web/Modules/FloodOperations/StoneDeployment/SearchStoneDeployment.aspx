<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchStoneDeployment.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.StoneDeployment.SearchStoneDeployment" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Stone Deployment</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
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
                            <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true">
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
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvStoneDeployment" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="FFPStonePositionID,InfrastructureType,InfrastructureName"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvStoneDeployment_PageIndexChanging" OnPageIndexChanged="gvStoneDeployment_PageIndexChanged" OnRowDataBound="gvStoneDeployment_RowDataBound">
                            <Columns>
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
                                <asp:TemplateField HeaderText="RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRD" runat="server" Text='<%#Eval("RD") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Approved in FFP (‘000 cft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityApproved" runat="server" Text='<%#Eval("RequiredQty") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Deployed (‘000 cft)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityDisposed" runat="server" Text='<%#Eval("QtyOfStoneDisposed") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>

                                        <asp:HyperLink ID="hlStoneDeploymentDetails" runat="server" ToolTip="Details" CssClass="btn btn-primary" NavigateUrl='<%# Eval("FFPStonePositionID","~/Modules/FloodOperations/StoneDeployment/AddStoneDeployment.aspx?FFPStonePositionID={0}") %>' Text="Details">
                                        </asp:HyperLink>
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
            </div>
            <asp:HiddenField ID="hdnStonePositionID" runat="server" Value="0" />
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
