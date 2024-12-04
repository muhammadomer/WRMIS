<%@ Page Title="Roles" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CriteriaForSpecificOutlet.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.CriteriaForSpecificOutlet" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Search Criteria For Locate Specific Outlet</h3>
                        </div>
                        <div class="box-content" id="divMain" runat="server">
                            <div class="table-responsive">
                                <asp:Table ID="tableInfo" runat="server" CssClass="table tbl-info">

                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblChnlNamelbl" runat="server" Text="Channel Name"></asp:Label>
                                        </asp:TableHeaderCell>

                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblChnlTypelbl" runat="server" Text="Channel Type"></asp:Label>
                                        </asp:TableHeaderCell>

                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblTotalRDlbl" runat="server" Text="Total R.Ds. (ft)"></asp:Label>
                                        </asp:TableHeaderCell>

                                    </asp:TableRow>


                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblChnlName" runat="server"></asp:Label>
                                        </asp:TableCell>

                                        <asp:TableCell>
                                            <asp:Label ID="lblChnlType" runat="server"></asp:Label>
                                        </asp:TableCell>

                                        <asp:TableCell>
                                            <asp:Label ID="lblTotalRD" runat="server"></asp:Label>
                                        </asp:TableCell>

                                    </asp:TableRow>


                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblFlowTypelbl" runat="server" Text="Flow type"></asp:Label>
                                        </asp:TableHeaderCell>

                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblCommandNamelbl" runat="server" Text="Command Name"></asp:Label>
                                        </asp:TableHeaderCell>

                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblIMISCodelbl" runat="server" Text="IMIS code"></asp:Label>
                                        </asp:TableHeaderCell>

                                    </asp:TableRow>


                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblFlowType" runat="server"></asp:Label>
                                        </asp:TableCell>

                                        <asp:TableCell>
                                            <asp:Label ID="lblCommandName" runat="server"></asp:Label>
                                        </asp:TableCell>

                                        <asp:TableCell>
                                            <asp:Label ID="lblIMISCode" runat="server"></asp:Label>
                                        </asp:TableCell>

                                    </asp:TableRow>


                                    <asp:TableRow>
                                        <asp:TableHeaderCell>
                                            <asp:Label ID="lblSectionNamelbl" runat="server" Text="Section Name"></asp:Label>
                                        </asp:TableHeaderCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblSectionName" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>

                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="fnc-btn">
                                                <asp:Button ID="btnBack" runat="server" OnClientClick="window.location.href='SearchCriteriaForOutletPerformance.aspx?History=true'; return false;" Text="Back" class="btn" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView ID="gvOutlets" runat="server" AutoGenerateColumns="False" EmptyDataText="No Outlet is available for the selected Channel under Jurisdiction"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10"  
                                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvOutlets_PageIndexChanged"
                                        OnPageIndexChanging="gvOutlets_PageIndexChanging" OnRowDataBound="gvOutlets_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='hello' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outlet R.D & Side">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("RDSide") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outlet type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("OutletType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Village Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("VillageName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Design discharge (Cusec)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("DesignDischarge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Design Diameter/ Width (ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("DesignDiameterWidth") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Height of Outlet (Y in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" Text='<%# Eval("HeightOfOutlet") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Head Above Crest (H in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelType" runat="server" Text='<%# Eval("HeadAboveCrest") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Sub mergence (h in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlowType" runat="server" Text='<%# Eval("Submergence") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Crest Reduced Level (ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalRDs" runat="server" Text='<%# Eval("CrestReducedLevel") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Minimum Modular Head (mmh in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommandName" runat="server" Text='<%# Eval("MMH") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Working Head (wh in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommandName" runat="server" Text='<%# Eval("WorkingHead") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outlet Performance">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlOutletPerformance" runat="server" CssClass="btn btn-primary btn_24 outlet-performance" ToolTip="Outlet Performance" NavigateUrl='<%# String.Format("~/Modules/DailyData/AddOutletPerformanceData.aspx?OutletID={0}", Convert.ToString(Eval("ID"))) %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Outlet Performance History">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlOutletPerformanceHist" runat="server" CssClass="btn btn-primary btn_24 performance-history" ToolTip="Outlet Performance History" NavigateUrl='<%# String.Format("~/Modules/DailyData/OutletPerformanceHistory.aspx?OutletID={0}", Convert.ToString(Eval("ID"))) %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
