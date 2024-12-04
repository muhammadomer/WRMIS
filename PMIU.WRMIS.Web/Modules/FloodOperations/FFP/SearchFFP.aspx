<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchFFP.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.SearchFFP" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <div class="box">
        <div class="box-title">
            <h3>Flood Fighting Plan</h3>
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
                            </div>
                            <div class="row">
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
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <div id="divStatus" runat="server">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">All</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                                    <asp:HyperLink ID="hlAddNewFFP" runat="server" NavigateUrl="~/Modules/FloodOperations/FFP/AddFFP.aspx" CssClass="btn btn-success">&nbsp;Add New</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvFFP" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" DataKeyNames="FFPYear,FFPDivisionID"
                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvFFP_PageIndexChanging" OnPageIndexChanged="gvFFP_PageIndexChanged" OnRowDeleting="gvFFP_RowDeleting" OnRowCommand="gvFFP_RowCommand" OnRowDataBound="gvFFP_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFFPID" runat="server" Text='<%# Eval("FFPID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Label ID="Year" runat="server" Text='<%#Eval("FFPYear") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemStyle CssClass="bold" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZone" runat="server" Text='<%#Eval("FFPZone") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCircle" runat="server" Text='<%#Eval("FFPCircle") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("FFPDivision") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFFPStatus" runat="server" Text='<%#Eval("FFPStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="HeaderAction">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlLastYearRestorationWorksFFP" runat="server" ToolTip="Last Year Restoration Works" CssClass="btn btn-primary btn_24 restoration_work" NavigateUrl='<%# String.Format("~/Modules/FloodOperations/FFP/LastYearRestorationWorksFFP.aspx?FFPID={0}&FFPYear={1}&FFPDivisionID={2}",Eval("FFPID"),Eval("FFPYear"),Eval("FFPDivisionID")) %>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlStonePositionFFP" runat="server" ToolTip="Stone Position" CssClass="btn btn-primary btn_24 stnpos_ffp" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FFP/AddFFPStonePosition.aspx?FFPID={0}&FFPYear={1}",Eval("FFPID"), Eval("FFPYear"))%>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlCampSites" runat="server" ToolTip="Camp Sites" CssClass="btn btn-primary btn_24 camp_sites" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FFP/AddCampSites.aspx?FFPID={0}&FFPYear={1}",Eval("FFPID"), Eval("FFPYear"))%>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlOverallDivisionItemsFFP" runat="server" ToolTip="Overall Division Items" CssClass="btn btn-primary btn_24 overall_d_item" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FFP/DivisionItemFFP.aspx?FFPID={0}&FFPYear={1}",Eval("FFPID"), Eval("FFPYear"))%>' Text="">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="hlArrangementsFFP" runat="server" ToolTip="Arrangements" CssClass="btn btn-primary btn_24 arrangement" NavigateUrl='<%#String.Format("~/Modules/FloodOperations/FFP/AddArrangements.aspx?FFPID={0}&FFPYear={1}",Eval("FFPID"), Eval("FFPYear"))%>' Text="">
                                        </asp:HyperLink>
                                        <asp:Button ID="btnPublish" runat="server" ToolTip="Publish" CssClass="btn btn-primary btn_24 publish" CommandName="FFPStatus" CommandArgument='<%# Eval("FFPID") %>' Text="" OnClientClick="return confirm('Are you sure you want to Publish this record?');" Enabled="false"></asp:Button>
                                        <asp:Button ID="btnPrint" Enabled="<%# base.CanPrint%>" runat="server" formnovalidate="formnovalidate" ToolTip="Print" CssClass="btn btn-primary btn_24 print" Style="height: 24px; width: 29px;" CommandName="PrintReport" />
                                        <asp:Button ID="hlEdit" Enabled="<%# base.CanEdit %>" runat="server" ToolTip="Edit" CssClass="btn btn-primary btn_24 edit" CommandName="EditFFP" CommandArgument='<%# Eval("FFPID") %>'></asp:Button>
                                        <asp:Button ID="linkButtonDelete" Enabled="<%# base.CanDelete %>" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete"></asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-3 text-center" />
                                    <ItemStyle CssClass="text-right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>

                    <%--</div>
                    </div>--%>

                </ContentTemplate>
            </asp:UpdatePanel>
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
