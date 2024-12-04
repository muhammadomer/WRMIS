<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCampSiteItems.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.AddCampSiteItems" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPCampSiteID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDivisionID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnyear" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureTypeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStatus" runat="server" Value="" />
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Items on Camp Sites for Flood Fighting Plan</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="tbl-info">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Infrastructure Type" Font-Bold="true"></asp:Label>

                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label2" runat="server" Text="Infrastructure Name" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            &nbsp;<asp:Label ID="Label4" runat="server" Text="RD" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_infra_type" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lbl_infrastructure" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            &nbsp;<asp:Label ID="lbl_RD" runat="server"></asp:Label>
                        </div>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="lbl_D" runat="server" Text="Description" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="lbl_Description" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
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
                    <asp:GridView ID="gvItems" runat="server" DataKeyNames="ItemId,ItemName,RequiredQty,OveralID"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <%--<asp:BoundField DataField="MajorMinor" HeaderText="Major/Minor Item" ItemStyle-CssClass="col-lg-3" />--%>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-CssClass="col-lg-3" />

                            <asp:TemplateField HeaderText="Quantity Available in Division Store">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AvailableQty" runat="server" Text='<%# Eval("DivisionQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-3 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Quantity Required at Site">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Qty" runat="server" Text='<%# Eval("RequiredQty") %>' pattern="^(0|[0-9][0-9]*)$" class="integerInput form-control" MaxLength="8"></asp:TextBox>
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

