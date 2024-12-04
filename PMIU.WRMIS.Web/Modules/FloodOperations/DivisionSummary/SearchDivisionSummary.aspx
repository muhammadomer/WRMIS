<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchDivisionSummary.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary.SearchDivisionSummary" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <div class="box">
        <div class="box-title">
            <h3>Division Summary</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">

            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                <ContentTemplate>


                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <%--<div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblDivisionSummaryStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:RadioButtonList ID="rdolDivisionSummaryStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                                <asp:ListItem Selected="True" Value="Draft">Draft</asp:ListItem>
                                                <asp:ListItem Value="Published">Published</asp:ListItem>
                                                <asp:ListItem Value="">All</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                                        <asp:HyperLink ID="hlAddNewDivisionSummary" runat="server" NavigateUrl="~/Modules/FloodOperations/DivisionSummary/AddDivisionSummary.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <%--<asp:GridView 
              OnRowCommand="gvSubDivision_RowCommand"
              >--%>
                            <asp:GridView ID="gvDivisionSummary" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" DataKeyNames="Year,DivisionID" BorderWidth="0px" CellSpacing="-1" GridLines="None" Visible="false"
                                OnPageIndexChanging="gvDivisionSummary_PageIndexChanging" OnPageIndexChanged="gvDivisionSummary_PageIndexChanged"
                                OnRowDeleting="gvDivisionSummary_RowDeleting" OnRowCommand="gvDivisionSummary_RowCommand" OnRowDataBound="gvDivisionSummary_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivisionSummaryID" runat="server" Text='<%# Eval("DivisionSummaryID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year">
                                        <ItemTemplate>
                                            <asp:Label ID="Year" runat="server" Text='<%#Eval("Year") %>'>
                                            </asp:Label>
                                            <%--<asp:HyperLink ID="Year" runat="server" Text='<%#Eval("Year") %>' NavigateUrl='<%# Eval("DivisionSummaryID", "~/Modules/FloodOperations/DivisionSummary/ViewDivisionSummary.aspx?DivisionSummaryID={0}")%>'>
                                            </asp:HyperLink>--%>
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
                                    <asp:TemplateField HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivisionSummaryStatus" runat="server" Text='<%#Eval("Status") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlInfrastructureBasedItems" runat="server" ToolTip="Infrastructure Based Items" CssClass="btn btn-primary btn_24 infr_based_item" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/DivisionSummary/InfrastructureBasedDivisionSummary.aspx?DivisionSummaryID={0}&DivisionSummaryYear={1}" ,Eval("DivisionSummaryID"), Eval("Year")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="hlDivisionStoreItems" runat="server" ToolTip="Division Store Items" CssClass="btn btn-primary btn_24 division_store_item" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/DivisionSummary/DivisionStoreItems.aspx?DivisionSummaryID={0}&DivisionSummaryYear={1}" ,Eval("DivisionSummaryID"), Eval("Year")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="hlStonePosition" runat="server" ToolTip="Stone Position" CssClass="btn btn-primary btn_24 stnpos" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/DivisionSummary/StonePosition.aspx?DivisionSummaryID={0}&DivisionSummaryYear={1}" ,Eval("DivisionSummaryID"), Eval("Year")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:Button ID="btnPrint" Enabled="<%# base.CanPrint%>" runat="server" formnovalidate="formnovalidate" ToolTip="Print" CssClass="btn btn-primary btn_24 print" Style="height: 24px; width: 29px;" CommandName="PrintReport" />
                                            <%--<asp:Button ID="btnPublish" runat="server" ToolTip="Publish" CssClass="btn btn-primary btn_24 publish" CommandName="Published" CommandArgument='<%# Eval("DivisionSummaryID") %>' Text="" OnClientClick="return confirm('Are you sure you want to Publish this record?');" Enabled="false"></asp:Button>--%>
                                            <asp:Button ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditDivisionSummary" CommandArgument='<%# Eval("DivisionSummaryID") %>'></asp:Button>
                                            <asp:Button ID="btnButtonDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
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

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:HiddenField ID="hdnDivisionSummaryID" runat="server" Value="0" />
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
