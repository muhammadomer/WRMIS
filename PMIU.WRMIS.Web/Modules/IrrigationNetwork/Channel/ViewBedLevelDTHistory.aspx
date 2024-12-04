<%@ Page Title="View Bed Level DT History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBedLevelDTHistory.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.ViewBedLevelDTParameters" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Discharge Table Parameters for Gauge on Bed Level History</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="table-responsive">
                    <table class="table tbl-info">
                        <tr>
                            <th>Channel Name</th>
                            <th>Channel Type</th>
                            <th>Total R.Ds.(ft)</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChannelName" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblChannelType" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblTotalRDs" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <th>Flow Type</th>
                            <th>Command Name</th>
                            <th>Category of Gauge</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFlowType" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblCommandName" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblCategoryOfGauge" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <th>Gauge Type</th>
                            <th></th>
                            <th></th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblGaugeType" runat="server" Text="" />
                                <asp:Label ID="lblGaugeID" runat="server" Visible="false" />
                                <asp:Label ID="lblChannelID" runat="server" Visible="false" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date-picker" onkeyup="return false;" onkeydown="return false;" />
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date-picker" onkeyup="return false;" onkeydown="return false;" />
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnShow" CssClass="btn btn-primary" Text="Show History" OnClick="btnShow_Click" />
                            <asp:Button runat="server" ID="btnBack" CssClass="btn" Text="Back" OnClick="btnBack_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <hr>
            <div class="table-responsive">
                <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="True" OnPageIndexChanging="gvHistory_PageIndexChanging"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                    OnRowDataBound="gvHistory_RowDataBound" DataKeyNames="GaugeID,ReadingDate,ID">
                    <Columns>
                        <asp:TemplateField HeaderText="Date of Approval">
                            <ItemTemplate>
                                <asp:Label ID="lblDateOfApproval" runat="server" CssClass="control-label" Text='<%# Eval("ReadingDate", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date of Observation">
                            <ItemTemplate>
                                <asp:Label ID="lblDateOfObservation" runat="server" CssClass="control-label" Text='<%# Eval("ObservationDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mean Depth (D in ft)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblMeanDepth" runat="server" CssClass="control-label" Text='<%# Eval("MeanDepth") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value of Exponent (n)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblExponentValue" runat="server" CssClass="control-label" Text='<%# Eval("ExponentValue") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observed Discharge (Qo in Cusec)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblObservedDischarge" runat="server" CssClass="control-label" Text='<%# Eval("DischargeObserved") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Coefficient of Discharge (K)" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblDischargeCoefficient" runat="server" CssClass="control-label" Text='<%# Math.Round(Convert.ToDouble(Eval("DischargeCoefficient")), 1) %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gauge Value Correction" ItemStyle-CssClass="numberAlign">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeValueCorrection" runat="server" CssClass="control-label" Text='<%# Eval("GaugeValueCorrection") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gauge Correction Type">
                            <ItemTemplate>
                                <asp:Label ID="lblGaugeCorrectionType" runat="server" CssClass="control-label" Text='<%# Eval("GaugeCorrectionType") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show Discharge Table">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnDischargeTable" CssClass="btn btn-primary btn-sm" Text="Discharge Table" OnClick="btnDischargeTable_Click" />
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
</asp:Content>
