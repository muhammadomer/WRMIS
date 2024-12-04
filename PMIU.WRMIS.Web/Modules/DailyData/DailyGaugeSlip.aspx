<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyGaugeSlip.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.DailyGaugeSlip" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Daily Gauge Slip</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group" style="margin-bottom: 0px;">
                                    <label class="col-sm-4 col-lg-3 control-label">Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date-picker required" required="true" ClientIDMode="Static" onkeyup="return false;" onkeydown="return false;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn" style="margin-bottom: 0px;">
                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" ToolTip="Show" OnClick="btnShow_Click" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-success" ToolTip="Print" OnClick="btnPrint_Click" />
                                    <asp:Button ID="btnCollapse" ClientIDMode="Static" runat="server" Text="Expand All" CssClass="btn btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlSlipSites" runat="server" Visible="true" ClientIDMode="Static">
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Indus - Tarbela Dam & Kabul River</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanIndusDam" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconIndusDam" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divIndusDam" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvIndusDam" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvIndusDam_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge">
                                <Columns>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxDamGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <%--onfocusin="return HighliteField(this);"--%>    <%--onfocusout="ReturnToNormal(this)"--%>
                                            <asp:Label ID="lblGauge" runat="server" CssClass="control-label" Text='<%# Eval("Gauge") %>' Visible="false" Style="margin-right: 6px;" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discharge (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxDamDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblDischarge" runat="server" CssClass="control-label" Visible="false" Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnIndusDamSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnIndusDamSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnIndusDamCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnIndusDamCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Indus - Headworks/Barrages Punjab</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanPunjabIndusBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconPunjabIndusBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divPunjabIndusBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvPunjabIndusBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvPunjabIndusBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGauge" runat="server" CssClass="control-label" Text='<%# Eval("Gauge") %>' Visible="false" Style="margin-right: 6px;"></asp:Label>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnPunjabIndusBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnPunjabIndusBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnPunjabIndusBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnPunjabIndusBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Indus - Headworks/Barrages Sindh</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanSindhIndusBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconSindhIndusBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divSindhIndusBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvSindhIndusBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvSindhIndusBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblDischarge" runat="server" CssClass="control-label" Visible="false" Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSindhIndusBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnSindhIndusBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnSindhIndusBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnSindhIndusBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Jhelum - Mangla Dam</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanManglaDam" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconManglaDam" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divManglaDam" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvManglaDam" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvManglaDam_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge">
                                <Columns>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxDamGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGauge" runat="server" CssClass="control-label" Text='<%# Eval("Gauge") %>' Visible="false" Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discharge (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxDamDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblDischarge" runat="server" CssClass="control-label" Visible="false" Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnManglaDamSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnManglaDamSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnManglaDamCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnManglaDamCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Jhelum - Barrages/Headworks</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanJhelumBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconJhelumBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divJhelumBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvJhelumBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvJhelumBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnJhelumBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnJhelumBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnJhelumBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnJhelumBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Chenab - Barrages/Headworks</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanChenabBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconChenabBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divChenabBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChenabBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvChenabBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnChenabBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnChenabBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnChenabBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnChenabBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Chenab - Other Heads</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanChenabOtherBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconChenabOtherBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divChenabOtherBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvChenabOtherBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvChenabOtherBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnChenabOtherBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnChenabOtherBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnChenabOtherBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnChenabOtherBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Ravi</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanRaviBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconRaviBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divRaviBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvRaviBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvRaviBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnRaviBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnRaviBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnRaviBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnRaviBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-bottom: 1px;">
                    <div class="box-title" style="background-color: #4ca4ee; text-shadow: 0 1px 0 #aecef4; padding: 1px; padding-left: 10px;">
                        <h4 style="color: #fff;">Sutlej</h4>
                        <div class="box-tool" style="top: 7px;">
                            <span id="spanSutlejBarrages" runat="server" class="badge badge-important">Incomplete</span>
                            <a data-action="collapse" href="#"><i id="iconSutlejBarrages" clientidmode="Static" runat="server" class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div id="divSutlejBarrages" runat="server" class="box-content" style="display: none;" clientidmode="Static">
                        <div class="table-responsive">
                            <asp:GridView ID="gvSutlejBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                                AllowPaging="false" OnRowDataBound="gvSutlejBarrages_RowDataBound" DataKeyNames="EnableGaugeDischarge,
                                MinValueGauge,MaxValueGauge,MinValueDischarge,MaxValueDischarge,GaugeCategoryID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Headwork / Barrage">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarrageName" runat="server" CssClass="control-label" Text='<%# Eval("StationName") %>'></asp:Label>
                                            <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false" />
                                            <asp:Label ID="lblSiteID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeSlipSiteID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Channel Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" CssClass="control-label" Text='<%# Eval("SiteName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAFSQHeader" runat="server" Text="A.F.S.Q (Cusec)" title="Authorized Full Supply Discharge In Cusec" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAFSQ" runat="server" CssClass="control-label" Text='<%# Eval("AFSQ") %>' Style="margin-right: 6px;"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gauge (ft)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Gauge") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                            <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Text='<%# Eval("GaugeID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent (Cusec)" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndent" runat="server" CssClass="control-label" Text='<%# Eval("Indent") %>' Style="margin-right: 6px;"></asp:Label>
                                            <asp:TextBox ID="txtIndent" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Indent") %>' MaxLength='<%# MaxBarrageGaugeLength %>' autocomplete="off" Visible="false" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Discharge (Cusec)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" Text='<%# Eval("Discharge") %>' MaxLength='<%# MaxBarrageDischargeLength %>' autocomplete="off" onfocus="HighliteField(this);" onfocusout="ReturnToNormal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSutlejBarragesSave" runat="server" CssClass="btn btn-primary" ToolTip="Save" Text="Save" OnClick="btnSutlejBarragesSave_Click" OnClientClick="DisableControl(this);" />
                                        <asp:LinkButton ID="lbtnSutlejBarragesCancel" runat="server" CssClass="btn" ToolTip="Cancel" Text="Cancel" OnClick="lbtnSutlejBarragesCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- END Main Content -->

    <br />
    <div class="box">
        <div class="box-title">
            <h3>Missing Gauge Slip Sites</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblFinancialYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                               <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control required">

                                <%--   <asp:ListItem Text="Select" Value="Select"></asp:ListItem> 
                                    <asp:ListItem Text="2014" Value="2014"></asp:ListItem> 
                                   <asp:ListItem Text="2015" Value="2015"></asp:ListItem> 
                                     <asp:ListItem Text="2016" Value="2016"></asp:ListItem> 
                                   <asp:ListItem Text="2017" Value="2017"></asp:ListItem> 
                                   <asp:ListItem Text="2018" Value="2018"></asp:ListItem> 
                                     <asp:ListItem Text="2019" Value="2019"></asp:ListItem> --%>
                                  

                               </asp:DropDownList> 
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                               <asp:DropDownList ID="ddlmonth" runat="server" CssClass="form-control required">
                                    <%-- <asp:ListItem Text="Select" Value="Select"></asp:ListItem> 
                                <asp:ListItem Text="Jan" Value="1"></asp:ListItem>  
                                    <asp:ListItem Text="Feb" Value="2"></asp:ListItem> 
                                     <asp:ListItem Text="Mar" Value="3"></asp:ListItem> 
                                   <asp:ListItem Text="Apr" Value="4"></asp:ListItem>  
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem> 
                                     <asp:ListItem Text="Jun" Value="6"></asp:ListItem> 

                                    <asp:ListItem Text="Jul" Value="7"></asp:ListItem>  
                                    <asp:ListItem Text="Aug" Value="8"></asp:ListItem> 
                                     <asp:ListItem Text="Sep" Value="9"></asp:ListItem> 
                                   <asp:ListItem Text="Oct" Value="10"></asp:ListItem>  
                                    <asp:ListItem Text="Nov" Value="11"></asp:ListItem> 
                                     <asp:ListItem Text="Dec" Value="12"></asp:ListItem> 
                                        --%> 
                               </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" id="divPMIUNExpenseMadeBy">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Gauge Slip Site" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlGaugeSite" runat="server" CssClass="form-control required"   >
                                    <asp:ListItem Text="Indus - Tarbela Dam & Kabul River (inflow)" Value="2"  />
                                    <asp:ListItem Text="kalabagh/Jinnah Barrage 	(upstream)" Value="10"  />
                                    <asp:ListItem Text="Jhelum - Mangla Dam (inflow)" Value="28"  />
                                     <asp:ListItem Text="Marala Barrage 		(Upstream)" Value="5"  />


                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                       <%-- <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Expenses made By" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlExpenseMadeBy" AutoPostBack="true" OnSelectedIndexChanged="ddlExpenseMadeBy_SelectedIndexChanged" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                    </div>
                </div>
                <br />
                <div class="row" id="searchButtonsDiv">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                      
                               <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-primary"  ToolTip="Search" OnClick="Button1_Click"  />
                      
                        </div>
                    </div>
                </div>

                <div class="row">
                <asp:GridView ID="gv_missedgaugeslip" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                 
                <Columns>

                    <asp:TemplateField HeaderText="Sr.No" HeaderStyle-Width="100px" >
                       <ItemTemplate><%# Container.DataItemIndex+1   %></ItemTemplate> 

                    </asp:TemplateField> 
                    <asp:BoundField DataField="readingdate" HeaderText="Reading Date" />   

                </Columns>                    
                 
                </asp:GridView>
                       

                </div> 
              
              
                
            </div>
        </div>






        
           


    </div>



    
    <script type="text/javascript">

        var datepickerCurrentValue = $('#txtDate').val();


        $('#btnCollapse').click(function () {
            var text = $('#btnCollapse')[0].defaultValue;

            if (text == "Expand All") {
                $('#divIndusDam').css({ 'display': 'block' });
                $('#iconIndusDam').removeClass("fa fa-chevron-down");
                $('#iconIndusDam').addClass("fa fa-chevron-up");

                $('#divPunjabIndusBarrages').css({ 'display': 'block' });
                $('#iconPunjabIndusBarrages').removeClass("fa fa-chevron-down");
                $('#iconPunjabIndusBarrages').addClass("fa fa-chevron-up");

                $('#divSindhIndusBarrages').css({ 'display': 'block' });
                $('#iconSindhIndusBarrages').removeClass("fa fa-chevron-down");
                $('#iconSindhIndusBarrages').addClass("fa fa-chevron-up");

                $('#divManglaDam').css({ 'display': 'block' });
                $('#iconManglaDam').removeClass("fa fa-chevron-down");
                $('#iconManglaDam').addClass("fa fa-chevron-up");

                $('#divJhelumBarrages').css({ 'display': 'block' });
                $('#iconJhelumBarrages').removeClass("fa fa-chevron-down");
                $('#iconJhelumBarrages').addClass("fa fa-chevron-up");

                $('#divChenabBarrages').css({ 'display': 'block' });
                $('#iconChenabBarrages').removeClass("fa fa-chevron-down");
                $('#iconChenabBarrages').addClass("fa fa-chevron-up");

                $('#divChenabOtherBarrages').css({ 'display': 'block' });
                $('#iconChenabOtherBarrages').removeClass("fa fa-chevron-down");
                $('#iconChenabOtherBarrages').addClass("fa fa-chevron-up");

                $('#divRaviBarrages').css({ 'display': 'block' });
                $('#iconRaviBarrages').removeClass("fa fa-chevron-down");
                $('#iconRaviBarrages').addClass("fa fa-chevron-up");

                $('#divSutlejBarrages').css({ 'display': 'block' });
                $('#iconSutlejBarrages').removeClass("fa fa-chevron-down");
                $('#iconSutlejBarrages').addClass("fa fa-chevron-up");

                $('#btnCollapse')[0].defaultValue = "Collapse All";
            }
            else {
                $('#divIndusDam').css({ 'display': 'none' });
                $('#iconIndusDam').removeClass("fa fa-chevron-up");
                $('#iconIndusDam').addClass("fa fa-chevron-down");

                $('#divPunjabIndusBarrages').css({ 'display': 'none' });
                $('#iconPunjabIndusBarrages').removeClass("fa fa-chevron-up");
                $('#iconPunjabIndusBarrages').addClass("fa fa-chevron-down");

                $('#divSindhIndusBarrages').css({ 'display': 'none' });
                $('#iconSindhIndusBarrages').removeClass("fa fa-chevron-up");
                $('#iconSindhIndusBarrages').addClass("fa fa-chevron-down");

                $('#divManglaDam').css({ 'display': 'none' });
                $('#iconManglaDam').removeClass("fa fa-chevron-up");
                $('#iconManglaDam').addClass("fa fa-chevron-down");

                $('#divJhelumBarrages').css({ 'display': 'none' });
                $('#iconJhelumBarrages').removeClass("fa fa-chevron-up");
                $('#iconJhelumBarrages').addClass("fa fa-chevron-down");

                $('#divChenabBarrages').css({ 'display': 'none' });
                $('#iconChenabBarrages').removeClass("fa fa-chevron-up");
                $('#iconChenabBarrages').addClass("fa fa-chevron-down");

                $('#divChenabOtherBarrages').css({ 'display': 'none' });
                $('#iconChenabOtherBarrages').removeClass("fa fa-chevron-up");
                $('#iconChenabOtherBarrages').addClass("fa fa-chevron-down");

                $('#divRaviBarrages').css({ 'display': 'none' });
                $('#iconRaviBarrages').removeClass("fa fa-chevron-up");
                $('#iconRaviBarrages').addClass("fa fa-chevron-down");

                $('#divSutlejBarrages').css({ 'display': 'none' });
                $('#iconSutlejBarrages').removeClass("fa fa-chevron-up");
                $('#iconSutlejBarrages').addClass("fa fa-chevron-down");

                $('#btnCollapse')[0].defaultValue = "Expand All";
            }

            return false;
        });

        $('#txtDate').change(function () {

            var datepickerNewValue = $('#txtDate').val();

            if (datepickerNewValue != datepickerCurrentValue) {
                $('#pnlSlipSites').hide();
            }

            datepickerCurrentValue = datepickerNewValue;
        });

        function CalculateDischarge(GuageID, GuageInputID, DischargeInputID) {

            var GuageValue = $('#' + GuageInputID).val();

            if (GuageID != '0' && GuageValue.trim() != '' && $('#' + GuageInputID)[0].checkValidity()) {

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("DailyGaugeSlip.aspx/CalculateDischarge") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_GaugeID: "' + GuageID + '",_GaugeValue:"' + GuageValue + '"}',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        if (data.d != null) {
                            $('#' + DischargeInputID).val(data.d);
                        }
                    }
                });
            }
        }

        function HighliteField(txtBox) {
            //txtBox.style.background = "pink";
            txtBox.style.borderColor = "black";
            txtBox.parentElement.parentElement.style.background = "#b5d3ec";
        }

        function ReturnToNormal(txtBox) {
            //alert("blur called");
            txtBox.style.background = "white";
            txtBox.style.borderColor = "#e5e5e5";
            var rowID = txtBox.parentElement.parentElement.sectionRowIndex;
            if (rowID % 2 == 0) {
                txtBox.parentElement.parentElement.style.background = "#FAFAFA";
            }
            else {

                txtBox.parentElement.parentElement.style.background = "#FFFFFF";
            }
        }

    </script>
</asp:Content>
