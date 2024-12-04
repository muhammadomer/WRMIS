<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchDivisionStore.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.SearchDivisionStore" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <div class="box">
        <div class="box-title">
            <h3>Division Store</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="box-content">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" AutoPostBack="True" required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required " AutoPostBack="True" required="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control required" required="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblItemCategory" runat="server" Text="Item Category" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control required" required="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <%--<div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblMajorMinor" runat="server" Text="Major / Minor" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList runat="server" ID="ddlMajorMinor" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnShow_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <asp:GridView ID="gvDivisionStore" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID,QuantityAvailable" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                OnPageIndexChanging="gvDivisionStore_PageIndexChanging" OnPageIndexChanged="gvDivisionStore_PageIndexChanged" OnRowDeleting="gvDivisionStore_RowDeleting" OnRowCommand="gvDivisionStore_RowCommand" OnRowDataBound="gvDivisionStore_RowDataBound">
                                <Columns>
                                    <%-- <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivisionStoreID" runat="server" Text='<%# Eval("DivisionStoreID") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--  <asp:TemplateField HeaderText="Major / Minor Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMajorMinorItem" runat="server" Text='<%#Eval("MajorMinor")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">

                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity Available in Division Store">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampSite" runat="server" Text='<%#Eval("QuantityAvailable")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 text-right" />
                                        <ItemStyle CssClass="text-right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <%--<ItemTemplate>
                                            <asp:Label ID="lblCampSite" runat="server" Text='<%#Eval("QuantityAvailable")%>'>
                                            </asp:Label>
                                        </ItemTemplate>--%>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Status">
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="hlDivisionStoreDetails" runat="server" ToolTip="Details" CssClass="btn btn-primary" NavigateUrl="~/Modules/FloodOperations/DivisionStore/DivisionStoreDetails.aspx" Text="Details"></asp:HyperLink>--%>
                                            <asp:LinkButton ID="hlDetails" runat="server" ToolTip="Details" CommandName="ItemDetail" CssClass="btn btn-primary" Text="Details">
                                            </asp:LinkButton>&nbsp;
                                             <asp:LinkButton ID="hlUpdateItemStatus" runat="server" ToolTip="Update Item Status" CommandName="UpdateItem" CssClass="btn btn-primary" Text="Update">
                                             </asp:LinkButton>
                                            <asp:LinkButton ID="lnkDSHistory" runat="server" ToolTip="History" CommandName="ItemHistory" CssClass="btn btn-primary" Text="History" Visible="False">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="col-md-2 text-center" />
                                        <HeaderStyle CssClass="col-md-4 text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnDivisionStoreID" runat="server" Value="0" />
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
