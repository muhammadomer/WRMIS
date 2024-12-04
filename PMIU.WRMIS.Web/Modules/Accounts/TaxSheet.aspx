<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TaxSheet.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.TaxSheet" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Tax Sheet</h3>
        </div>
        <div class="box-content">

            <div class="table-responsive">
                <table class="table tbl-info">

                    <tr>

                        <th width="33.3%">Month</th>
                        <th width="33.3%">Sanction Type</th>
                        <th width="33.3%">Sanction Amount</th>
                        <th></th>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblMonth" runat="server" Text="" /></td>
                        <td>
                            <asp:Label ID="lblSanctionType" runat="server" Text="" /></td>
                        <td>
                            <asp:Label ID="lblSanctionAmount" runat="server" Text="" /></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <br />
            <br />

            <div class="table-responsive">
                <asp:GridView ID="gvTaxSheet" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" Visible="true" OnRowDataBound="gvTaxSheet_RowDataBound"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <%-- <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name Of Staff">
                            <ItemTemplate>
                                <asp:Label ID="lblNameOfStaff" runat="server" CssClass="control-label" Text='<%# Eval("NameOfStaff") %>'></asp:Label>
                                <asp:Label ID="lblVendorTpye" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("VendorType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label Text="Grand Total" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("Designation") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vehical/Item No.">
                            <ItemTemplate>
                                <asp:Label ID="lblVehicalOrItemNo" runat="server" CssClass="control-label" Text='<%# Eval("VehicleNo") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total Bill (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTotalBill" runat="server" CssClass="control-label" Text='<%# Eval("TotalBill") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrTotalBill" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase Items Amount (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseAmount" runat="server" CssClass="control-label" Text='<%# Eval("PurchaseItem") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrPurchaseItemAmount" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Repair Items Amount (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblRepairAmount" runat="server" CssClass="control-label" Text='<%# Eval("RepairItem") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrRepairItemAmount" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="GST (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGST" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrGST" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="I.Tax On Purchase (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblIncomeTaxPurchase" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrIncomeTaxPurchase" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="I.Tax On Service (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblIncomeTaxService" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterStyle CssClass="text-right" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrIncomeTaxService" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PST On Services (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPSTService" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrPSTService" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total Tax (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTotalTax" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrTotalTax" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount (Rs.)">
                            <ItemStyle CssClass="text-right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetAmount" runat="server" CssClass="control-label"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblFtrNetAmount" runat="server" Visible="true" /></b>
                            </FooterTemplate>
                            <FooterStyle CssClass="text-right" />
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:LinkButton ID="lbtnPrint" runat="server" CssClass="btn btn-primary" Text="Print Tax Sheet" OnClick="lbtnPrint_Click" />
                        <button id="hlBack" onclick="javascript:history.go(-1)" class="btn">Back</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
