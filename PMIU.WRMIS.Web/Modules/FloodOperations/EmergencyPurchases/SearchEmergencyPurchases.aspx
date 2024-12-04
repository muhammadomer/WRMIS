<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchEmergencyPurchases.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.SearchEmergencyPurchases" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Emergency Purchases</h3>
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
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
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
                                <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass=" rquired form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
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
                                <asp:Label ID="lblCampSite" runat="server" Text="Camp Site" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList runat="server" ID="ddlCampSite" CssClass="form-control">
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
                                <asp:HyperLink ID="hlAddNew" runat="server" NavigateUrl="~/Modules/FloodOperations/EmergencyPurchases/AddEmergencyPurchases.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvEmergencyPurchases" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="EmergencyPurchaseID,InfrastructureName,InfrastructureType,CompSite,Year"
                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                OnPageIndexChanging="gvEmergencyPurchases_PageIndexChanging" OnPageIndexChanged="gvEmergencyPurchases_PageIndexChanged" OnRowDeleting="gvEmergencyPurchases_RowDeleting" OnRowDataBound="gvEmergencyPurchases_RowDataBound" OnRowCommand="gvEmergencyPurchases_RowCommand">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmergencyPurchasesID" runat="server" Text='<%# Eval("EmergencyPurchaseID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmergencyCreatedDate" runat="server" Text='<%#Eval("Year") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Infrastructure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInfrastructureType" runat="server" Text='<%# (Convert.ToString(Eval("InfrastructureType"))) == "Control Structure1" ? "Barrage/Headwork": Eval("InfrastructureType") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1 text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Camp Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampSite" runat="server" Text='<%#(Boolean.Parse(Eval("CompSite").ToString())) ? "Yes" : "No" %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1 text-left" />
                                        <ItemStyle CssClass="text-left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlWorks" runat="server" ToolTip="Works" CssClass="btn btn-primary btn_24 work" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/EmergencyPurchases/WorksEmergencyPurchases.aspx?EmergencyPurchaseID={0}&EmergencyPurchaseYear={1}",Eval("EmergencyPurchaseID"),Eval("Year")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="hlItemPurchasing" runat="server" ToolTip="Item Purchasing" CssClass="btn btn-primary btn_24 purchase" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/EmergencyPurchases/ItemPurchasingEmergencyPurchases.aspx?EmergencyPurchaseID={0}&EmergencyPurchaseYear={1}",Eval("EmergencyPurchaseID"),Eval("Year")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:Button ID="btnEditEmergencyPurchase" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditEmergencyPurchase" CommandArgument='<%# Eval("EmergencyPurchaseID") %>'></asp:Button>
                                            <asp:Button ID="btnDeleteEmergencyPurchase" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
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
            </div>
            <asp:HiddenField ID="hdnEmergencyPurchasesID" runat="server" Value="0" />
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
