<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DivisionStoreItems.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary.DivisionItems" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/FloodOperations/Controls/DivisionSummaryDetail.ascx" TagPrefix="uc1" TagName="DivisionSummaryDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnDivisionSummaryID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnItemsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureTypeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureID" runat="server" Value="0" />
    <!-- BEGIN Main Content -->
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <div class="box">
                <div class="box-title">
                    <h3 runat="server" id="h3PageTitle">Division Items for Division Summary</h3>
                </div>
                <div class="box-content">
                    <uc1:DivisionSummaryDetail runat="server" ID="DivisionSummaryDetail" />
                    <br />
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCat" runat="server" CssClass="col-sm-4 col-lg-2 control-label" Text="Category" />
                                    <div class="col-sm-8 col-lg-6 controls">
                                        <asp:DropDownList ID="ddlItemCategory" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvItems" runat="server" AllowPaging="True" AllowSorting="false" AutoGenerateColumns="False" CssClass="table header" DataKeyNames="ItemId,ItemName,AvailableQtyPost,DivisionStoreQty" EmptyDataText="No record found" GridLines="None" OnRowDataBound="gvItems_RowDataBound" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <%--<asp:BoundField DataField="MajorMinor" HeaderText="Major/Minor Item" ItemStyle-Width="150px" />--%>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-md-1" />
                                    <asp:TemplateField HeaderText="Quantity Available at Infrastructures">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Infra_Qty" runat="server" Text='<%#Eval("AvailableQtyPost") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="integerInput text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity Available in Division Store">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Div_Store_Qty" runat="server" Text='<%#Eval("DivisionStoreQty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="integerInput text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Quantity Available in Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_total_Qty" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2 text-right" />
                                        <ItemStyle CssClass="integerInput" />
                                    </asp:TemplateField>
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
                    <br>
                    <br></br>
                    <br></br>
                    <br></br>
                    <br></br>
                    </br>
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
    <!-- END Main Content -->
</asp:Content>
