<%@ Page  Language="C#" AutoEventWireup="true" CodeBehind="ViewChildEntitlements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PublicWebSite.ViewChildEntitlements" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Entitlement</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link rel="stylesheet" href="/Design/assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/Design/assets/font-awesome/css/font-awesome.min.css" />

    <!--page specific css styles-->
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="/Design/assets/bootstrap-timepicker/css/bootstrap-timepicker.min.css" />
    <!--flaty css styles-->
    <link rel="stylesheet" href="/Design/css/flaty.css?v=0" />
    <link rel="stylesheet" href="/Design/css/flaty-responsive.css" />

    <link rel="shortcut icon" href="/Design/img/favicon.html" />

    <link href="/Design/css/jquery-ui.css" rel="stylesheet" />

    <link href="/Design/css/custom.css" rel="stylesheet" />

    <script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <div class="col-md-12">
            <div class="box">
                <div class="box-content">
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlMainDesc" runat="server" HorizontalAlign="Center">
                                <h5>
                                    <asp:Label ID="lblMainDesc" runat="server" Text="" Font-Bold="true" /></h5>
                            </asp:Panel>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <b>
                                <asp:Label ID="lbl7782AverageText" runat="server" Text="Average System Usage 1977-1982 (MAF):" /></b>
                        </div>
                        <div class="col-md-1 text-right">
                            <asp:Label ID="lbl7782Average" runat="server" Text="" />
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <b>
                                <asp:Label ID="lblEntitlementText" runat="server" Text="" /></b>
                        </div>
                        <div class="col-md-1 text-right">
                            <asp:Label ID="lblEntitlement" runat="server" Text="" />
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <b>
                                <asp:Label ID="lblPercentChangeText" runat="server" Text="Entitlement Change w.r.t 1977-1982 ( % ):" /></b>
                        </div>
                        <div class="col-md-1 text-right">
                            <asp:Label ID="lblPercentChange" runat="server" Text="" />
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <b>
                                <asp:Label ID="lblDeleveriesChangeText" runat="server" Text="Deliveries Change w.r.t 1977-1982 ( % ):" /></b>
                        </div>
                        <div class="col-md-1 text-right">
                            <asp:Label ID="lblDeleveriesChange" runat="server" Text="" />
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <b>
                                <asp:Label ID="lblDesignDischargeText" runat="server" Text="Design Discharge (Cusecs):" /></b>
                        </div>
                        <div class="col-md-1 text-right">
                            <asp:Label ID="lblDesignDischarge" runat="server" Text="" />
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
                        <asp:GridView ID="gvEntitlements" runat="server" AutoGenerateColumns="false"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" ShowFooter="true"
                            OnRowDataBound="gvEntitlements_RowDataBound" DataKeyNames="EntitlementMAF,DeliveriesMAF"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ten Day">
                                    <HeaderStyle CssClass="col-md-1 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTenDaily" runat="server" CssClass="control-label" Text='<%#Eval("TenDaily") %>'></asp:Label>
                                        <asp:Label ID="lblTenDailyID" runat="server" Visible="false" CssClass="control-label" Text='<%#Eval("TenDailyID") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label Text="Total" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-center" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Entitlement (Cusec)">
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblEntitlementCs" runat="server" CssClass="control-label" Text='<%#Eval("EntitlementCs") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrEntitlement" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Deliveries (Cusec)">
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveriesCs" runat="server" CssClass="control-label" Text='<%#Eval("DeliveriesCs") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrDeliveriesCS" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Difference (Cusec)">
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDifferenceCs" runat="server" CssClass="control-label" Text=""></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrDifference" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Deliveries (MAF)">
                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveriesMAF" runat="server" CssClass="control-label" Text='<%#Eval("DeliveriesMAF") %>'></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrDeliveriesMAF" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Accumulative Deliveries (MAF)">
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccumulativeDeliveriesMAF" runat="server" CssClass="control-label" Text=""></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrAccumulativeDeliveries" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Balance Entitlement (MAF)">
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalanceEntitlementMAF" runat="server" CssClass="control-label" Text=""></asp:Label>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrBalanceEntitlement" runat="server" Visible="true" /></b><br>
                                        <br>
                                    </FooterTemplate>
                                    <FooterStyle CssClass="text-right" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Shortage w.r.t Entitlement (%)">
                                    <HeaderStyle CssClass="col-md-2 text-right" />
                                    <ItemStyle CssClass="text-right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblShortage" runat="server" CssClass="control-label" Text="" />
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="ftrShortageEntitlement" runat="server" Visible="true" /></b><br>
                                        <br>
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
                                <asp:HyperLink ID="hlBack" runat="server" onclick="goBack()" CssClass="btn" Text="Back" />
                                <%--<asp:HyperLink ID="btnPlaceIndent" Text="Place Indent" CssClass="btn btn-success" runat="server"></asp:HyperLink>
                                <asp:LinkButton ID="btnReset" runat="server" CssClass="btn" Text="Reset"/>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script>
                function goBack() {
                    window.history.back();
                }
            </script>
        </div>
    </form>
</body>
</html>

