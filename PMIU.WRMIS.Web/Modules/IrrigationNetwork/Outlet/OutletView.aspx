<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OutletView.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet.OutletView" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Import Namespace="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet" %>
<%@ Register Src="~/Modules/IrrigationNetwork/Controls/OutletChannelDetails.ascx" TagPrefix="uc1" TagName="ChannelDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- BEGIN Main Content -->
    <div class="box">
        <div class="box-title">
            <h3>Outlets</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>

        <div class="box-content">
            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
            <uc1:ChannelDetails runat="server" ID="ChannelDetails" />

            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlAdd" runat="server" CssClass="btn btn-primary">
                    <%--<i class="fa fa-check"></i>--%>&nbsp;Add New Outlet</asp:HyperLink>
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>

            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvOutlet" DataKeyNames="OutletID,AlterationID" runat="server" CssClass="table header" AutoGenerateColumns="False" AllowPaging="true"
                    AllowSorting="false" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowCustomPaging="true"
                    OnPageIndexChanging="gvOutlet_PageIndexChanging" OnRowCreated="gvOutlet_RowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="Outlet R.D & Side">
                            <ItemTemplate>
                                <asp:Label ID="lblRDSide" runat="server" Text='<%# Eval("RDandSide") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Outlet Type">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("OutletType") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Village Name">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Village") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design Discharge (Cusec)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("DesignDischarge") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design Diameter/ Width (ft)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Width") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height of Outlet (Y in ft)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Height") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Head above Crest (H in ft)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Crest") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Submergence (h in ft)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Submergence") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Crest Reduced Level (ft)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CrestRLL") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Minimum Modular Head (mmh)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("MinModHead") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlVillages" runat="server" CssClass="btn btn-primary btn_24 outlet-villages" NavigateUrl='<%# GetPageURL(PageNames.OutletVillages, Convert.ToString(Eval("OutletID")), Convert.ToString(Eval("AlterationID"))) %>' ToolTip="Outlet Villages">
                                </asp:HyperLink>
                                <asp:HyperLink ID="hlAlteration" runat="server" CssClass="btn btn-primary btn_24 outlet-alteration" NavigateUrl='<%# GetPageURL(PageNames.Alteration, Convert.ToString(Eval("OutletID")), Convert.ToString(Eval("AlterationID"))) %>' ToolTip="Alteration">
                                </asp:HyperLink>
                                <asp:HyperLink ID="hlHisory" runat="server" CssClass="btn btn-primary btn_24 outlet-alteration-history" NavigateUrl='<%# GetPageURL(PageNames.AlterationHistory, Convert.ToString(Eval("OutletID")), Convert.ToString(Eval("AlterationID"))) %>' ToolTip="History">
                                </asp:HyperLink>
                                <%--<asp:HyperLink ID="hlEditHistory" runat="server" CssClass="btn btn-primary btn_24 outlet-alteration-history" NavigateUrl='<%# GetPageURL(PageNames.EditHistory, Convert.ToString(Eval("OutletID")), Convert.ToString(Eval("AlterationID"))) %>' ToolTip="Edit History">--%>
                                <%--</asp:HyperLink>--%>
                                <asp:HyperLink ID="hlEdit" runat="server" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" NavigateUrl='<%# GetPageURL(PageNames.EditOutlet, Convert.ToString(Eval("OutletID")), Convert.ToString(Eval("AlterationID"))) %>' ToolTip="Edit">
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>

        </div>

    </div>
    <!-- END Main Content -->


</asp:Content>
