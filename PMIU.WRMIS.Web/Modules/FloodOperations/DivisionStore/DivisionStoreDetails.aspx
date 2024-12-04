<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DivisionStoreDetails.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.DivisionStoreDetails" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Details of Division Store</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvDivisionStore" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                            OnPageIndexChanging="gvDivisionStore_PageIndexChanging" OnPageIndexChanged="gvDivisionStore_PageIndexChanged">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Major / Minor Item">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMajorMinorItem" runat="server" CssClass="control-label" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Item Name">
                                    <HeaderStyle CssClass="col-md-4" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" CssClass="control-label" Text='<%#Eval("ContactPerson") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity Entered">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityEntered" runat="server" CssClass="control-label" Text='<%#Eval("ContactNo") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Quantity Issued">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityIssued" runat="server" CssClass="control-label" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity Effected">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityEffected" runat="server" CssClass="control-label" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Condition">
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCondition" runat="server" CssClass="control-label" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quantity Available in Division ">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantityAvailable" runat="server" CssClass="control-label" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>

                                </asp:TemplateField>


                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
