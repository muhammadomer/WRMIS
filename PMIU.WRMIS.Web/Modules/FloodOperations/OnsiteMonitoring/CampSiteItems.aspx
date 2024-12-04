<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CampSiteItems.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring.CampSiteItems" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnOMCampSiteID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDivisionID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCampsiteID" runat="server" Value="0" />
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Items On Camp Sites for Onsite Monitoring</h3>
        </div>
        <div class="box-content">
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
                        <asp:Label ID="InfrastructureTypeText" Text="Infrastructure Type" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="InfrastructureNameText" Text="Infrastructure Name" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="RDText" Text="RD" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="DescriptionText" Text="Description" Font-Bold="True" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <asp:Label ID="lblInfrastructureType" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblInfrastructureName" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblRD" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
            </div>
            <br />
            <div class="form-horizontal">


                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCat" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                            <div class="col-sm-8 col-lg-6 controls">
                                <asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="form-control required" required="true" AutoPostBack="True"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <br />
                    <asp:GridView ID="gvItems" runat="server" DataKeyNames="OverallDivItemID,ItemName,OMQty,OMID,CreatedDate,CreatedBy,OMCamsiteID"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-3" />

                            <asp:TemplateField HeaderText="Quantity Approved">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ApprovedQty" runat="server" Text='<%# Eval("QuantityApprovedFFP") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Onsite Quantity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOnsiteQuantity" runat="server" Text='<%# Eval("OMQty") %>' pattern="^(0|[0-9][0-9]*)$" class="integerInput form-control" MaxLength="8"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>

            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>

