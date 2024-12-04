<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DivisionItemFFP.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FFP.DivisionItemFFP" %>

<%@ Register Src="~/Modules/FloodOperations/Controls/FFPDetail.ascx" TagPrefix="uc1" TagName="FFPDetail" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFFPCampSiteID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnFFPStatus" runat="server" Value="0" />
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Overall Division Items for Flood Fighting Plan</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <uc1:FFPDetail runat="server" ID="FFPDetail" />
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblCat" runat="server" Text="Category" CssClass="col-sm-4 col-lg-2 control-label" />
                            <div class="col-sm-8 col-lg-6 controls">
                                <asp:DropDownList ID="ddlItemCategory" runat="server" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" CssClass="form-control required" required="true" AutoPostBack="True">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <br />
                    <asp:GridView ID="gvItems" runat="server"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ItemSubcategoryID,Item,AdditionalQty"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCreated="gvItems_RowCreated" OnRowDataBound="gvItems_RowDataBound" OnPageIndexChanged="gvItems_PageIndexChanged" OnPageIndexChanging="gvItems_PageIndexChanging">
                        <Columns>
                            <%--<asp:BoundField DataField="MajorMinor" HeaderText="Major/Minor Item" ItemStyle-Width="150px" />--%>
                            <asp:BoundField DataField="Item" HeaderText="Item Name" ItemStyle-CssClass="col-md-2" />

                            <asp:TemplateField HeaderText="Available Quantity in Division Store">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AvailableQty" runat="server" Text='<%# Eval("AvailableQuantityinDivisionStore") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lbll" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Camp Sites <br/>(A)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TotalCampSiteQty" runat="server" Text='<%# Eval("TotalCampSiteQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblUniss" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<center>Infrastructures <br/>(B)<center/>">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TotalInfrastructureQty" runat="server" Text='<%# Eval("TotalInfrastructureQty") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblUn" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<center>Additional <br/> (C)<center/>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Qty" runat="server" Text='<%# Eval("AdditionalQty") %>' pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblU" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<center>Total <br/>(A+B+C)<center/>">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TotalQuantity" runat="server" Text='<%# Eval("TotalQuantity") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                   


                    <asp:GridView ID="gvItemsAsset" runat="server"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ItemSubCategoryID,ItemSubCategoryName,AdditionalQty,AvailableQuantity"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gvItemsAsset_RowDataBound" OnPageIndexChanged="gvItemsAsset_PageIndexChanged" OnPageIndexChanging="gvItemsAsset_PageIndexChanging">
                        <Columns>
                            <%--<asp:BoundField DataField="MajorMinor" HeaderText="Major/Minor Item" ItemStyle-Width="150px" />--%>
                            <asp:BoundField DataField="ItemSubCategoryName" HeaderText="Type" ItemStyle-CssClass="col-md-4" />

                            <asp:TemplateField HeaderText="Available Quantity" >
                                <ItemTemplate>
                                    <asp:Label ID="lbl_AvailableQty" runat="server" Text='<%# Eval("AvailableQuantity") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-4 text-right" />
                                <ItemStyle CssClass="text-right" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Additional Quantity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Qty" runat="server" Text='<%# Eval("AdditionalQty") %>' pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8"></asp:TextBox>
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
                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" /><%--OnClick="btnSave_Click"--%>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <!-- END Main Content -->


</asp:Content>
