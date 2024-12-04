<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemsDivisionSummary.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary.ItemsDivisionSummary" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/FloodOperations/Controls/DivisionSummaryDetail.ascx" TagPrefix="uc1" TagName="DivisionSummaryDetail" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnItemsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDivisionSummaryID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureTypeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureID" runat="server" Value="0" />

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 runat="server" id="h3PageTitle">Items on Infrastructure for Division Summary</h3>
                </div>
                <div class="box-content">
                    <uc1:DivisionSummaryDetail runat="server" ID="DivisionSummaryDetail" />
                    <div class="tbl-info">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblInfrastructureTypeText" runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblInfrastructureNameText" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="lblInfrastructureType" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblInfrastructureName" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCat" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                                    <div class="col-sm-8 col-lg-6 controls">
                                        <asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemId,ItemName,AvailableQtyPost,DivisionStoreQty"
                                CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvItems_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" HeaderStyle-CssClass="col-md-4" />
                                    <asp:BoundField DataField="AvailableQtyPost" HeaderText="Quantity Available at Infrastructures" HeaderStyle-CssClass="col-md-3 text-right" ItemStyle-CssClass="text-right" />
                                    <asp:BoundField DataField="" HeaderText="" ItemStyle-CssClass="col-md-1" />
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
    </script>
</asp:Content>
