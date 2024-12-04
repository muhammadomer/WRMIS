<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchStructure.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.FloodBundGauges.SearchStructure" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Search Structure</h3>
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
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <asp:TextBox ID="txtStructureName" runat="server" CssClass="input form-control" pattern="[0-9a-zA-Z.\.\-_:/,!\(\)\[\]\{\}].{2,250}" title="Name must be of minim 2 characters which is upto 250 characters" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/FloodBundGauges/AddStructure.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvStructureSearch" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStructureNalaHillTorantID" runat="server" Text='<%# Eval("StructureNalaHillTorantID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionName" runat="server" Text='<%#Eval("Division") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Structure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStructureType" runat="server" Text='<%#Eval("StructureType") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3" />
                                    <%--<ItemStyle CssClass="text-right" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Structure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStructureName" runat="server" Text='<%#Eval("StructureName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 " />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlEdit" Visible="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# Eval("StructureNalaHillTorantID", "~/Modules/IrrigationNetwork/ReferenceData/FloodBundGauges/AddStructure.aspx?StructureNalaHillTorantID={0}")%>'>
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
            <asp:HiddenField ID="hdnStructureNallahHillTorentID" runat="server" Value="0" />
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

