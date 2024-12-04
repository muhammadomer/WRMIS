<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BillDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.BillDetail" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Billing Details</h3>
                </div>
                <div class="box-content">
                    <asp:HiddenField ID="hdnIndustryID" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnBillID" Value="0" runat="server" />
                    <div class="form-horizontal">
                        <div class="tbl-info">
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="Label9" runat="server" Text="Industry Name" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="Label11" runat="server" Text="Industry No." Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="Label13" runat="server" Text="Division" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblIndustryName" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblIndustryNo" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                </div>

                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="row">
                            <%--  <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdServiceType" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rdServiceType_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Selected="True">&nbsp;Effluent Water</asp:ListItem>
                                            <asp:ListItem Value="2">&nbsp;Canal Special Water</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                    <div class="col-sm-8 col-lg-4 controls">
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbEffluent" runat="server" AutoPostBack="true" GroupName="ServiceType" Text="Effluent Waters" OnCheckedChanged="rbService_CheckedChanged" />
                                    </div>
                                    <div class="col-sm-8 col-lg-4">
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbCanal" runat="server" AutoPostBack="true" GroupName="ServiceType" Text="Canal Special Waters" OnCheckedChanged="rbService_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Financial Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlfinancialyear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlfinancialyear_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <asp:Panel ID="pnlWaterBilling" runat="server" GroupingText="Effluent Water Bill Information" Visible="false">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblChargesText" class="col-sm-4 col-lg-5 control-label" Text="Effluent Charges (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <%-- <asp:Label runat="server" ID="lblCharges" ></asp:Label>--%>
                                            <asp:Label runat="server" ID="lblRate"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label1" class="col-sm-4 col-lg-5 control-label" Text="Bill No." Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblBillNo"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label3" class="col-sm-4 col-lg-5 control-label" Text="Applicable Taxes (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblApplicableTax"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label5" class="col-sm-4 col-lg-5 control-label" Text="Bill Issue Date" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblBillIssueDate"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label2" class="col-sm-4 col-lg-5 control-label" Text="Adjustments (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblAdjustment"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label6" class="col-sm-4 col-lg-5 control-label" Text="Bill Due Date" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblBillDueDate"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblTotalBillText" class="col-sm-4 col-lg-5 control-label" Text="Total Bill (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblTotalBill" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblBillStatusText" class="col-sm-4 col-lg-5 control-label" Text="Bill Status" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblBillStatus" style="text-transform:capitalize;"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                        
                            </div>

                            <div class="row" runat="server" id="DivPTotal" visible="false">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label7" class="col-sm-4 col-lg-5 control-label" Text="Total Bill for previous year (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblPtotalBill"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label12" class="col-sm-4 col-lg-5 control-label" Text="Arrears (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblArrears"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblSanctionedDischargeText" class="col-sm-4 col-lg-5 control-label" Text="Discharge (Cusec)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblSanctionedDischarge"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label16" class="col-sm-4 col-lg-5 control-label" Text="Advance Paid (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblAdvancePaid"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblRateText" class="col-sm-4 col-lg-5 control-label" Text="Effluent Rate (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">

                                            <asp:Label runat="server" ID="lblCharges"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label20" class="col-sm-4 col-lg-5 control-label" Text="Payable Amount before Due Date (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblPayablebeforeDueDate" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label8" class="col-sm-4 col-lg-5 control-label" Text="Surcharge (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblSrchg"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label4" class="col-sm-4 col-lg-5 control-label" Text="Payable Amount after Due Date (Rs.)" Font-Bold="true"></asp:Label>
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:Label runat="server" ID="lblPayableAfterDueDate"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlBilldetail" runat="server" GroupingText="Bill Details" Visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="50"
                                            OnPageIndexChanging="gv_PageIndexChanging"
                                            OnPageIndexChanged="gv_PageIndexChanged" EmptyDataText="No record found"
                                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                            ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Charges Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFactorName" runat="server" Text='<%# Eval("FactorCategory") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFactorName" runat="server" Text='<%# Eval("FactorName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment Type" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount (Rs.)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBillAmount" runat="server" Text='<%# Utility.GetRoundOffValue(  Convert.ToString( Eval ("BillFactorAmount")) ) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1 text-right" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                               <%-- <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/EWC/Industry.aspx?RestoreState=1" CssClass="btn">Back</asp:HyperLink>--%>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
