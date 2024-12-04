<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfrastructureBasedDivisionSummary.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary.InfrastructureBasedDivisionSummary" %>

<%@ Register Src="~/Modules/FloodOperations/Controls/DivisionSummaryDetail.ascx" TagPrefix="uc1" TagName="DivisionSummaryDetail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>

    <asp:HiddenField ID="hdnDivisionSummaryID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Infrastructure Based Items for Division Summary</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <uc1:DivisionSummaryDetail runat="server" ID="DivisionSummaryDetail" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvInfrastructureBasedItems" runat="server" DataKeyNames="StructureTypeID,StructureID,StructureType,StructureName"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCommand="gvInfrastructureBasedItems_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Infrastructure Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureType" runat="server" Text='<%# Eval("StructureType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-5" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Infrastructure Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInfrastructureName" runat="server" Text='<%# Eval("StructureName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-5" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="hlinfrastructureItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" Text=" Items " NavigateUrl='<%# Eval("ID", "~/Modules/FloodOperations/DivisionSummary/ItemsDivisionSummary.aspx?ItemsID={0}")%>'></asp:HyperLink>--%>
                                         <asp:LinkButton ID="hlinfrastructureItems" runat="server" ToolTip="Items" CssClass="btn btn-primary" Text=" Items" CommandName="InfrastructureItem" ></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
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
