<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerformanceEvaluationScore.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.PerformanceEvaluation.PerformanceEvaluationScore" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Performance Evaluation Score</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Evaluation Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <asp:RadioButton ID="rbGeneral" runat="server" GroupName="evaluation" Text="General" AutoPostBack="true" TabIndex="1" OnCheckedChanged="rbGeneral_CheckedChanged" Checked="true" />
                                        </label>
                                        &nbsp;&nbsp;
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <asp:RadioButton ID="rbSpecific" runat="server" GroupName="evaluation" Text="Specific" AutoPostBack="true" TabIndex="2" OnCheckedChanged="rbSpecific_CheckedChanged" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlGeneral" runat="server">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Evaluation Level</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlLevel" runat="server" TabIndex="3" required="true" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Session</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <label class="radio-inline" style="padding-top: 1px;">
                                                <input runat="server" id="rbMorningG" type="radio" name="session" value="M" checked="true" tabindex="4" clientidmode="Static">
                                                Morning
                                            </label>
                                            &nbsp;&nbsp;
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <input runat="server" id="rbEveningG" type="radio" name="session" value="E" clientidmode="Static" tabindex="5">
                                            Evening
                                        </label>
                                            &nbsp;&nbsp;
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <input runat="server" id="rbAverageG" type="radio" name="session" value="A" clientidmode="Static" tabindex="6">
                                            Average
                                        </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Report Span</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlReportSpanG" runat="server" TabIndex="7" required="true" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlReportSpanG_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlSeasonG" runat="server" Visible="false" CssClass="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Season</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlSeasonG" runat="server" TabIndex="9" required="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlSeasonG_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Year</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlYearG" runat="server" TabIndex="8" required="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlYearG_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:Panel ID="pnlMonthG" runat="server" Visible="false" CssClass="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Month</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlMonthG" runat="server" TabIndex="9" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMonthG_SelectedIndexChanged" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlFortnightlyG" runat="server" Visible="false" CssClass="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Fortnightly</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlFortnightlyG" runat="server" TabIndex="10" required="true" Enabled="false" ClientIDMode="Static">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlSpecific" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlZone" runat="server" TabIndex="3" Required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" TabIndex="4" Enabled="false" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" TabIndex="5" Enabled="false" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlSubDivision" runat="server" TabIndex="6" ClientIDMode="Static" Enabled="false">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Session</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <label class="radio-inline" style="padding-top: 1px;">
                                                <input runat="server" id="rbMorningS" type="radio" name="session" value="M" checked="true" tabindex="7" clientidmode="Static">
                                                Morning
                                            </label>
                                            &nbsp;&nbsp;
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <input runat="server" id="rbEveningS" type="radio" name="session" value="E" tabindex="8" clientidmode="Static">
                                            Evening
                                        </label>
                                            &nbsp;&nbsp;
                                        <label class="radio-inline" style="padding-top: 1px;">
                                            <input runat="server" id="rbAverageS" type="radio" name="session" value="A" tabindex="9" clientidmode="Static">
                                            Average
                                        </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Report Span</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlReportSpanS" runat="server" TabIndex="10" required="true" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlReportSpanS_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:Panel ID="pnlSeasonS" runat="server" Visible="false" CssClass="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Season</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlSeasonS" runat="server" TabIndex="12" required="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlSeasonS_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Year</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlYearS" runat="server" TabIndex="11" required="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlYearS_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlMonthS" runat="server" Visible="false" CssClass="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Month</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlMonthS" runat="server" TabIndex="12" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMonthS_SelectedIndexChanged" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="pnlFortnightlyS" runat="server" Visible="false" CssClass="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Analysis Fortnightly</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" ID="ddlFortnightlyS" runat="server" TabIndex="13" required="true" Enabled="false" ClientIDMode="Static">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button TabIndex="12" ID="btnShow" runat="server" Text="Show Results" CssClass="btn btn-primary" OnClick="btnShow_Click" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtShowPopup" runat="server" Text="false" Style="display: none;" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                        <div id="dvGrids" style="display: <%= GridDisplay %>">
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="dvZone" runat="server" class="table-responsive" visible="false">
                                        <asp:GridView ID="gvZone" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" DataKeyNames="EvalID,BoundryID" OnPageIndexChanging="gvZone_PageIndexChanging"
                                            OnRowDataBound="gvZone_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRating" runat="server" CssClass="control-label" Text='<%# Eval("Rating") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" CssClass="control-label" Text='<%# Eval("BoundryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Channels">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalChannels" runat="server" CssClass="control-label" Text='<%# Eval("TotalChannels") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analyzed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnalyzedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AnalyzedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosedGauges" runat="server" CssClass="control-label" Text='<%# Eval("ClosedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aggregated Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAggregatedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AggregatedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Obtained Points">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObtainedPoints" runat="server" CssClass="control-label" Text='<%# Eval("Score") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                    <div id="dvCircle" runat="server" class="table-responsive" visible="false">
                                        <asp:GridView ID="gvCircle" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" DataKeyNames="EvalID,BoundryID" OnPageIndexChanging="gvCircle_PageIndexChanging"
                                            OnRowDataBound="gvCircle_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRating" runat="server" CssClass="control-label" Text='<%# Eval("Rating") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Circle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircleName" runat="server" CssClass="control-label" Text='<%# Eval("BoundryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" CssClass="control-label" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-2" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Channels">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalChannels" runat="server" CssClass="control-label" Text='<%# Eval("TotalChannels") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analyzed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnalyzedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AnalyzedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosedGauges" runat="server" CssClass="control-label" Text='<%# Eval("ClosedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aggregated Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAggregatedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AggregatedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Obtained Points">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObtainedPoints" runat="server" CssClass="control-label" Text='<%# Eval("Score") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                    <div id="dvDivision" runat="server" class="table-responsive" visible="false">
                                        <asp:GridView ID="gvDivision" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" DataKeyNames="EvalID,BoundryID" OnPageIndexChanging="gvDivision_PageIndexChanging"
                                            OnRowDataBound="gvDivision_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRating" runat="server" CssClass="control-label" Text='<%# Eval("Rating") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("BoundryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Circle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircleName" runat="server" CssClass="control-label" Text='<%# Eval("CircleName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" CssClass="control-label" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Channels">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalChannels" runat="server" CssClass="control-label" Text='<%# Eval("TotalChannels") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analyzed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnalyzedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AnalyzedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosedGauges" runat="server" CssClass="control-label" Text='<%# Eval("ClosedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aggregated Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAggregatedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AggregatedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Obtained Points">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObtainedPoints" runat="server" CssClass="control-label" Text='<%# Eval("Score") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Level">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityLevel" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityLevel") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Score">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityScore" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityScore") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                    <div id="dvSubDivision" runat="server" class="table-responsive" visible="false">
                                        <asp:GridView ID="gvSubDivision" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" DataKeyNames="EvalID,BoundryID" OnPageIndexChanging="gvSubDivision_PageIndexChanging"
                                            OnRowDataBound="gvSubDivision_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRating" runat="server" CssClass="control-label" Text='<%# Eval("Rating") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("BoundryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Circle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircleName" runat="server" CssClass="control-label" Text='<%# Eval("CircleName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" CssClass="control-label" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Channels">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalChannels" runat="server" CssClass="control-label" Text='<%# Eval("TotalChannels") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analyzed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnalyzedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AnalyzedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosedGauges" runat="server" CssClass="control-label" Text='<%# Eval("ClosedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aggregated Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAggregatedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AggregatedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Obtained Points">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObtainedPoints" runat="server" CssClass="control-label" Text='<%# Eval("Score") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Level">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityLevel" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityLevel") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Score">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityScore" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityScore") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>
                                    <div id="dvSection" runat="server" class="table-responsive" visible="false">
                                        <asp:GridView ID="gvSection" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                            ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1"
                                            GridLines="None" DataKeyNames="EvalID,BoundryID" OnPageIndexChanging="gvSection_PageIndexChanging"
                                            OnRowDataBound="gvSection_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRating" runat="server" CssClass="control-label" Text='<%# Eval("Rating") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSectionName" runat="server" CssClass="control-label" Text='<%# Eval("BoundryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Division">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDivisionName" runat="server" CssClass="control-label" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Circle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircleName" runat="server" CssClass="control-label" Text='<%# Eval("CircleName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" CssClass="control-label" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Channels">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalChannels" runat="server" CssClass="control-label" Text='<%# Eval("TotalChannels") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Analyzed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAnalyzedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AnalyzedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Closed Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClosedGauges" runat="server" CssClass="control-label" Text='<%# Eval("ClosedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aggregated Gauges">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAggregatedGauges" runat="server" CssClass="control-label" Text='<%# Eval("AggregatedGauges") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Obtained Points">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblObtainedPoints" runat="server" CssClass="control-label" Text='<%# Eval("Score") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Level">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityLevel" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityLevel") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Complexity Score">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComplexityScore" runat="server" CssClass="control-label" Text='<%# Eval("ComplexityScore") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="col-md-1" />
                                                    <ItemStyle CssClass="text-right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlDetail" runat="server" ToolTip="Detail" CssClass="btn btn-primary btn_32 audit"></asp:HyperLink>
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
                            <div class="row">
                                <div class="col-md-6">
                                    <div id="dvbutton" runat="server" class="fnc-btn" visible="false">
                                        <asp:HyperLink ID="hlSave" runat="server" NavigateUrl="#changelog" role="button" data-toggle="modal" CssClass="btn btn-primary" Text="Save" />
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back" Visible="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="changelog" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Evaluation Report Name" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:TextBox runat="server" CssClass="form-control" ClientIDMode="Static" ID="txtReportName" onblur="this.value = this.value.trim();" autocomplete="off" MaxLength="100" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-primary" ClientIDMode="Static" Text="Save" />
                                    <button id="btnClose" class="btn" data-dismiss="modal" aria-hidden="true" type="button">Close</button>
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

            if ($('#txtShowPopup').val() == "true") {
                $('#changelog').modal('show');
                $('#txtShowPopup').val("false");
            }

            $('#ddlLevel').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbMorningG').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbEveningG').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbAverageG').change(function () {
                $('#dvGrids').hide();
            });

            $('#ddlFortnightlyG').change(function () {
                $('#dvGrids').hide();
            });

            $('#ddlSeasonG').change(function () {
                $('#dvGrids').hide();
            });

            $('#ddlSubDivision').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbMorningS').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbEveningS').change(function () {
                $('#dvGrids').hide();
            });

            $('#rbAverageS').change(function () {
                $('#dvGrids').hide();
            });

            $('#ddlFortnightlyS').change(function () {
                $('#dvGrids').hide();
            });

            $('#ddlSeasonS').change(function () {
                $('#dvGrids').hide();
            });

            $('#btnSave').click(function () {
                $('#txtReportName').attr("required", "required");
            });

            $('#btnClose').click(function () {
                $('#txtReportName').removeAttr("required");
                $('#txtReportName').val("");
            });

        });

    </script>
</asp:Content>
