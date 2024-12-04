<%@ Page Title="Roles" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CriteriaForSpecificOutletAlteration.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.CriteriaForSpecificOutletAlteration" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Locate Specific Outlet</h3>
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
                                                <asp:Button ID="btnBack" runat="server" OnClientClick="window.location.href='SearchCriteriaForOutletAlteration.aspx?History=true'; return false;" Text="Back" class="btn" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView ID="gvOutlets" DataKeyNames="OutletID" runat="server" CssClass="table header" AutoGenerateColumns="False" AllowPaging="true"
                                        AllowSorting="false" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowCustomPaging="true"
                                        OnPageIndexChanging="gvOutlets_PageIndexChanging" OnRowCreated="gvOutlets_RowCreated">
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
                                            <asp:TemplateField HeaderText="Working Head (wh in ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommandName" runat="server" Text='<%# Eval("OutletWorkingHead") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Outlet Alteration">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlOutletAlteration" runat="server" CssClass="btn btn-primary btn_24 outlet-alteration" ToolTip="Outlet Alteration" NavigateUrl='<%# String.Format("~/Modules/ScheduleInspection/AddOutletAlterationData.aspx?OutletID={0}", Convert.ToString(Eval("OutletID"))) %>'></asp:HyperLink>
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
