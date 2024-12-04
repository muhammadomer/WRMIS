<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutletVillage.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Outlet.OutletVillage" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><%--<i class="fa fa-file"></i>--%>Outlet Villages</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAlterationID" runat="server" Value="0" />

            <asp:Table ID="tblChannelDetail" runat="server" CssClass="table tbl-info">
                <asp:TableRow>
                    <asp:TableHeaderCell>Channel Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Channel Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Outlet R.D</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblChannelName" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblChannelType" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblOutletRD" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnChannelTotalRDs" Value="0" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Outlet Side</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Design Discharge (Cusec)</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Outlet Type</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblOutletSide" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblDesignDischarge" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblOutletType" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell>Height of Outlet/Orifice (Y in ft)</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Head above Crest of Outlet (H in ft)</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Minimum Modular Head (ft)</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblOutletHeight" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblOutletCrest" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblMMHead" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <hr>

            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvOutletVillage" runat="server" DataKeyNames="ID,VillageID,LocatedIn" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" GridLines="None" AllowSorting="false" AllowPaging="True"
                                    OnRowCommand="gvOutletVillage_RowCommand" OnRowDataBound="gvOutletVillage_RowDataBound" OnRowEditing="gvOutletVillage_RowEditing"
                                    OnRowCancelingEdit="gvOutletVillage_RowCancelingEdit" OnRowUpdating="gvOutletVillage_RowUpdating" OnRowDeleting="gvOutletVillage_RowDeleting"
                                    OnPageIndexChanging="gvOutletVillage_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Village Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVillageName" runat="server" Text='<%#Eval("VillageName") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlOutletVillage" runat="server" Width="300px" required="required" CssClass="required form-control"></asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-4" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Outlet installed at Village">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsOutletInsalled" runat="server" Text='<%# Eval("LocatedIn") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlOutletLocatedIn" runat="server" Width="200px" required="required" CssClass="required form-control"></asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-7" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="pnlAddOutletVillage" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnAddOutletVillage" visible="<%# base.CanAdd %>" runat="server" Text="" CommandName="AddOutletVillage" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlOutletVillage" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="btnEditOutletVillage" runat="server" visible="<%# base.CanEdit %>" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                                    <asp:Button ID="btnDeleteOutletVillage" runat="server" visible="<%# base.CanDelete %>" Text="" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Panel ID="pnlEditOutletVillage" runat="server" HorizontalAlign="Center">
                                                    <asp:Button runat="server" ID="btnSaveOutletVillage" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                    <asp:Button ID="btnCancelOutletVillage" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                </asp:Panel>
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
