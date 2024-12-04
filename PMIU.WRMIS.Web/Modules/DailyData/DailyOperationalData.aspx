<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DailyOperationalData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.DailyOperationalData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .myAltRowClass {
            color: blue;
            background-color: #DCFFFF;
            background-image: none;
        }
    </style>
    <div class="box">
        <div class="box-title">
            <h3>Daily Operational Data</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" required="required" CssClass="form-control" data-rule-required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True" Enabled="False"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" required="required" CssClass="form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="3" ID="ddlDivision" runat="server" required="required" CssClass="form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="4" ID="ddlSubDivision" runat="server" CssClass="form-control" data-rule-required="true" Enabled="False"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->
                                <div class="form-group">
                                    <label id="lblDate" class="col-sm-4 col-lg-3 control-label">Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="5" runat="server" required="required" class="form-control disabled-future-date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="ddlSession" id="lblSession" class="col-sm-4 col-lg-3 control-label">Session</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="6" ID="ddlSession" runat="server" required="required" CssClass="form-control" data-rule-required="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="txtChannelName" id="lblChanngelName" class="col-sm-4 col-lg-3 control-label">Channel Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtChannelName" TabIndex="7" runat="server"  class="form-control" size="16" type="text"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnLoadDailyData" CssClass="btn btn-primary" Text="Show" OnClick="btnLoadDailyData_Click" />
                                    <asp:Button runat="server" ID="btnPrint" CssClass="btn btn-success" Text="Print" OnClick="btnPrint_Click" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDailyOperationalData" runat="server" DataKeyNames="DailyGaugeReadingID,GaugeID,IsCurrent,Close,channelNameForAuditTrail,GaugePhoto,DesignationID,ChannelID,ReadingDate,SubmittedBy,Designation,GIS_X,GIS_Y"
                                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="true" PageSize="10"
                                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                                        OnRowDataBound="gvDailyOperationalData_RowDataBound" OnPageIndexChanging="gvDailyOperationalData_PageIndexChanging"
                                        OnPreRender="gvDailyOperationalData_PreRender" OnRowCommand="gvDailyOperationalData_RowCommand" OnRowCreated="gvDailyOperationalData_RowCreated">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sub Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubDivisionName" runat="server" Text='<%# Eval("SubDivisionName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Channel Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannalName" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Close">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClose" runat="server" Text='<%# Eval("Close") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reading Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadingDateTime" runat="server" Text='<%# Eval("ReadingDateTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gauge Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGaugeName" runat="server" Text='<%# Eval("GaugeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RDs (ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRDs" runat="server" Text='<%# Eval("RDs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Section Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval("SectionName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gauge Value (ft)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGaugeValue" runat="server" Text='<%# Eval("GaugeValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discharge (cusec)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDailyDischarge" runat="server" Text='<%# Eval("DailyDischarge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle CssClass="text-right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Submitted by with Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubmittedBy" runat="server" Text='<%# Eval("SubmittedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlActionDailyData" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnViewGaugeImage" runat="server" Text="" CommandName="ViewGaugeImage" CssClass="btn btn-primary btn_24 viewimg" ToolTip="View Gauge Image" />
                                                        <asp:Button ID="btnAuditTrail" data-toggle="modal" runat="server" Text="" CommandName="AuditTrail"
                                                            CssClass="btn btn-primary btn_24 audit" ToolTip="View History" />
                                                        <asp:Button ID="btnEditGauge" runat="server" Text="" CommandName="EditGauge"
                                                            CssClass="btn btn-primary btn_24 edit" ToolTip="Edit Gauge" Visible="false" />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                                <ItemStyle Width="285px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnLoadDailyData" EventName="click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <!-- Start Of Edit Gauge  Value -->
            <div id="editvalue" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-horizontal">
                                        <div class="row">

                                            <div class="test">

                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 col-lg-3 control-label">Current Gauge Value</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <div style="margin-top: 5px;">
                                                                <asp:Label ID="lblCurrentGaugeValue" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" id="divNewGauge" runat="server">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 col-lg-3 control-label">New Gauge Value</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:TextBox ID="txtNewGaugeValue" ClientIDMode="Static" runat="server" MaxLength="4" placeholder="" CssClass="decimalInput required form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 col-lg-3 control-label">Reason for Change</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlReasonForChange" ClientIDMode="Static" runat="server" CssClass="required form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div>
                                                <asp:HiddenField ID="hdnGaugeID" runat="server" ClientIDMode="Static" Value="-1" />
                                                <asp:HiddenField ID="hdnGaugeReadingID" runat="server" ClientIDMode="Static" Value="-1" />

                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvDailyOperationalData" EventName="RowCommand" />
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSaveGaugeValue" OnClientClick="SetRequiredField();" CssClass="btn btn-primary" Text="&nbsp;Save changes" OnClick="btnSaveGaugeValue_Click" />
                            <asp:Button CssClass="btn" runat="server" data-dismiss="modal" aria-hidden="true" Text="Cancel" OnClientClick="testt();" />

                            <%--<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>--%>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END Of Edit Gauge  Value -->
            <!-- Start Of Audit Trail -->
            <div id="auditTrail" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Table ID="tblChannelDetail" runat="server" CssClass="table tbl-info">
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Channel Name</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Gauge Type</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblChannelName" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblGuageType" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Gauge RD</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>Section</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblGuageRD" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblSection" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Session</asp:TableHeaderCell>
                                            <asp:TableHeaderCell></asp:TableHeaderCell>
                                            <asp:TableHeaderCell></asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblSession" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                    <hr />

                                    <asp:GridView ID="gvAuditTrail" runat="server" DataKeyNames="IsCurrent"
                                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="true"
                                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                                        OnRowDataBound="gvAuditTrail_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Gauge Value">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGaugeValue" runat="server" Text='<%# Eval("GaugeValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discharge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDailyDischarge" runat="server" Text='<%# Eval("DailyDischarge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Submitted By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubmittedBy" runat="server" Text='<%# Eval("SubmittedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reading Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReadingDateTime" runat="server" Text='<%# Eval("ReadingDateTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="175px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reason For Change">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReasonForChange" runat="server" Text='<%# Eval("ReasonForChange") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvDailyOperationalData" EventName="RowCommand" />
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Detail Modal Ends here -->
            <!-- END Of Audit Trail -->
            <!-- Start Of View Gauge Image -->
            <div id="viewimage" class="modal fade">
                <div class="modal-dialog" style="max-height: 519px; max-width: 793.398px; overflow: initial !important">
                    <div class="modal-content" style="width: 730px">
                        <div class="modal-body" style="height: 500px; overflow: auto; width: 728px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Table ID="Table1" runat="server" CssClass="table tbl-info">
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>By</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>GIS_X</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:Label ID="lblUploadedBy" runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblGISX" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableHeaderCell>Dated</asp:TableHeaderCell>
                                            <asp:TableHeaderCell>GIS_Y</asp:TableHeaderCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblDateTime" runat="server"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="lblGISY" runat="server"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                    <br />
                                    <center>
                                        <asp:Image ID="imgGaugeImage" runat="server" style="display: block; max-width: 100%; height:auto; max-height:500px;"/>
				                    </center>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
                <!-- END Of View Gauge Image -->
            </div>

        </div>
        <asp:HiddenField ID="hdnSession" ClientIDMode="Static" runat="server" Value="" />
        <asp:HiddenField ID="hdnDate" runat="server" Value="" />
    </div>

    <script type="text/ecmascript" src="../../Scripts/jquery-1.10.2.min.js"></script>
    <%--<script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
    <%--<script type="text/ecmascript" src="../../Design/js/grid.locale-en.js"></script>--%>
    <!-- This is the Javascript file of jqGrid -->
    <%--<script type="text/ecmascript" src="../../Design/js/jquery.jqGrid.js"></script>--%>
    <!-- This is the localization file of the grid controlling messages, labels, etc.
    <!-- We support more than 40 localizations -->
    <%--    <script type="text/ecmascript" src="../../../js/trirand/i18n/grid.locale-en.js"></script>--%>
    <!-- A link to a jQuery UI ThemeRoller theme, more than 22 built-in and many more custom -->
    <%--    <link rel="stylesheet" type="text/css" media="screen" href="../../../css/jquery-ui.css" />--%>
    <!-- The link to the CSS that the grid needs -->
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css">--%>
    <link href="../../Design/css/ui.jqgrid-bootstrap.css" rel="stylesheet" />

    <%--    <script type="text/javascript">
        $('#<%=txtDate.ClientID%>').change(function () {
            PreSelectSession();
        });
    </script>--%>

    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerOnUpdatePanelRefresh();
                    $('#<%=txtDate.ClientID%>').change(function () {
                        PreSelectSession();
                    });
                    InitilizeNumericValidation();
                    ClearDateField();
                }
            });
        };
        $('.modal').on('hidden.bs.modal', function () {
            testt();
        });

        function testt() {
            $('#txtNewGaugeValue').removeAttr('required', 'false');
            $('#ddlReasonForChange').removeAttr('required', 'required');
        }

    </script>


    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>--%>
    <%--<script src="../../Scripts/DailyData/OperationalData.js?1"></script>--%>
</asp:Content>
