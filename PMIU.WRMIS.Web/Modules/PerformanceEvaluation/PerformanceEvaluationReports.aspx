<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerformanceEvaluationReports.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.PerformanceEvaluationReports" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Performance Evaluation Report</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="1" runat="server" class=" form-control date-picker" size="16" type="text" onkeyup="return false;" onkeydown="return false;" ClientIDMode="Static" />
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="2" runat="server" class=" form-control date-picker" size="16" type="text" onkeyup="return false;" onkeydown="return false;" ClientIDMode="Static" />
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Report Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtReportName" type="text" class="form-control" TabIndex="3" ClientIDMode="Static" MaxLength="100" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button TabIndex="4" ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="dvGrid" runat="server" class="col-md-12" style="display: none" clientidmode="Static">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                        GridLines="None" DataKeyNames="ID" OnPageIndexChanging="gvReport_PageIndexChanging"
                                        OnRowDataBound="gvReport_RowDataBound" Visible="false" OnRowDeleting="gvReport_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Evaluation Report Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportName" runat="server" CssClass="control-label" Text='<%# Eval("ReportName") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Report Generation Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGenerationDate" runat="server" CssClass="control-label" Text='<%# Eval("ModifiedDate") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Evaluation Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEvaluationType" runat="server" CssClass="control-label" Text='<%# Eval("EvaluationType") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Report Span">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportSpan" runat="server" CssClass="control-label" Text='<%# Eval("ReportSpan") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlShow" runat="server" CssClass="btn btn-primary btn_32 view" ToolTip="Show" />
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                                    <asp:LinkButton ID="lbtnPrint" runat="server" CssClass="btn btn-primary btn_24 print" ToolTip="Print" OnClick="lbtnPrint_Click" />
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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#txtFromDate').change(function () {
                $('#dvGrid').hide();
            });

            $('#txtToDate').change(function () {
                $('#dvGrid').hide();
            });

            $('#txtReportName').change(function () {
                $('#dvGrid').hide();
            });

        });

    </script>
</asp:Content>
