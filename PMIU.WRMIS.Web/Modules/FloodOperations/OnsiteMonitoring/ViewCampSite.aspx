<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCampSite.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring.ViewCampSite" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/FloodOperations/Controls/OMDetail.ascx" TagPrefix="uc1" TagName="OMDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>View Camp Sites</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="tbl-info">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="YearText" Text="Year" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="ZoneText" Text="Zone" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="CircleText" Text="Circle" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="DivisionText" Text="Division" Font-Bold="True" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblYear" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblZone" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblCircle" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="lblinf" Text="Infrastructure Name" Font-Bold="True" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-md-3">
                                    <asp:Label ID="lblinfraname" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%--  <uc1:OMDetail runat="server" ID="OMDetail" />--%>
                        <div class="table-responsive">
                            <br />
                            <asp:GridView ID="gvCampSite" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10" DataKeyNames="RD,ID"
                                EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                OnPageIndexChanging="gvCampSite_PageIndexChanging" OnPageIndexChanged="gvCampSite_PageIndexChanged" OnRowDataBound="gvCampSite_RowDataBound"
                                OnRowCancelingEdit="gvCampSite_RowCancelingEdit" OnRowUpdating="gvCampSite_RowUpdating" OnRowEditing="gvCampSite_RowEditing" OnRowCommand="gvCampSite_RowCommand">
                                <Columns>

                                    <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RD">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRD" runat="server" CssClass="control-label" Text='<%#Eval("RD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"></ItemStyle>
                                        <HeaderStyle CssClass="col-md-1 text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            
                                        </ItemTemplate>
                                        <ItemStyle CssClass="text-right"></ItemStyle>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <HeaderStyle CssClass="col-md-4" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" CssClass="control-label" Text='<%#Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Camp Site Available">
                                        <HeaderStyle CssClass="col-md-4" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantityEntered" runat="server" CssClass="control-label" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCampSiteAvailable" runat="server" required="true" CssClass="form-control required">
                                                <asp:ListItem Value="" Text="Select" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="true" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="false" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="hdnQuantityEntered" runat="server" Visible="false"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <%--   <asp:TemplateField HeaderText="Items">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" Text="Items" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/OnsiteMonitoring/CampSiteItems.aspx?ID={0}") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <HeaderStyle CssClass="col-md-1" />
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <%--<asp:HyperLink ID="hlItems" runat="server" ToolTip="Items" CssClass="btn btn-primary btn_32 view-feedback"  Enabled="false" NavigateUrl='<%# Eval("ID","~/Modules/FloodOperations/OnsiteMonitoring/CampSiteItems.aspx?ID={0}") %>'></asp:HyperLink>--%>
                                                <asp:LinkButton ID="hlItems" runat="server" ToolTip="Items" CommandName="ItemDetail" CssClass="btn btn-primary btn_32 view-feedback" Enabled="false">
                                                </asp:LinkButton>
                                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>"></asp:Button>
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">

                                                <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                            </asp:Panel>
                                        </EditItemTemplate>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
