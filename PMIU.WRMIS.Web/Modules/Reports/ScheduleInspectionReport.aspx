<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleInspectionReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.ScheduleInspectionReport" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        iframe {
            margin-left: auto;
            margin-right: auto;
            display: block;
            width: 100%;
            border: 0px;
            height: 915px;
        }

        .form-horizontal .control-label {
            padding: 0px;
        }

        .fnc-btn {
            margin: 0px 0px 0px 10px;
        }

        .form-horizontal .radio, .form-horizontal .checkbox, .form-horizontal .radio-inline, .form-horizontal .checkbox-inline {
            padding-top: 0px;
        }

        .form-group {
            margin-bottom: 5px;
        }

        .box {
            margin-bottom: 10px;
        }
    </style>
    <div class="box">
        <div class="box-title">
            <h3>Schedule and Inspections Reports</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#">
                    <i class="fa fa-chevron-up"></i>
                </a>
            </div>
        </div>
        <div class="box-content" style="padding-bottom: 0px;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-content" style="background: #EEEEEE; padding: 10px 10px 0px 10px;">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbGaugeInspection" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Gauge Inspection" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbOutletPerformance" runat="server" AutoPostBack="true" GroupName="Reports" Text="Outlet Performance" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDischargeTableCalculations" runat="server" AutoPostBack="true" GroupName="Reports" Text="Discharge Table Parameters" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbInspectionOfOutletAlteration" runat="server" AutoPostBack="true" GroupName="Reports" Text="Inspection of Outlet Alteration" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                     <%--   <div class="row" visible="false">
                                            <div class="col-md-3" visible="false">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbTenders" runat="server" AutoPostBack="true" GroupName="Reports" Text="Tenders" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3" visible="false">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbWorks" runat="server" AutoPostBack="true" GroupName="Reports" Text="Works" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-11">
                            <div class="box">
                                <div class="box-content" style="background: #EEEEEE; padding: 10px 10px 5px 10px;">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="3" ID="ddlDivision" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Div.</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="1" ID="ddlSubDivision" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlChannel" id="lblChannel" class="col-sm-4 col-lg-3 control-label">Channel</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="2" ID="ddlChannel" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPOutlet" visible="false">
                                                <div class="form-group">
                                                    <label id="lblOutlet" class="col-sm-4 col-lg-3 control-label">Outlet</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlInspectedBy" id="lblInspectedBy" class="col-sm-4 col-lg-3 control-label">Inspected By</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="3" ID="ddlInspectedBy" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label id="lblFromDate" class="col-sm-4 col-lg-3 control-label">From</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <div class="input-group date" data-date-viewmode="years">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label id="lblToDate" class="col-sm-4 col-lg-3 control-label">To</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <div class="input-group date" data-date-viewmode="years">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlInspectionType" id="lblInspectionType" class="col-sm-4 col-lg-3 control-label">Inspect Type</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList TabIndex="4" ID="ddlInspectionType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPGaugeCategory">
                                                <div class="form-group">
                                                    <label id="lblGaugeCategory" class="col-sm-4 col-lg-3 control-label">Gauge Cat.</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlGaugeCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="SPDischargeTable" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblGaugeRD" class="col-sm-4 col-lg-3 control-label">Gauge RD</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlGaugeRD" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblDTCalculation" class="col-sm-4 col-lg-3 control-label">DT Calculation</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlDTCalculation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPGaugeStatus">
                                                <div class="form-group">
                                                    <label id="lblGaugeStatus" class="col-sm-4 col-lg-3 control-label">Gauge Status</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlGaugeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="SPOutletPerformance" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblEfficiency" class="col-sm-4 col-lg-3 control-label">Efficiency</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlEfficiency" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="SPAlteredFor" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblAlteredFor" class="col-sm-4 col-lg-3 control-label">Altered for</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlAlteredFor" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1" style="text-align:right;">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnLoadScheduleInspection" OnClick="btnLoadScheduleInspection_Click" CssClass="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <iframe id="iframestyle" src="ViewReport.aspx" frameborder="0" allowfullscreen="" style="height:1350px;" name="iframe" runat="server"></iframe>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                    ClearDateField();
                }
            });
        };
    </script>
    <script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>
</asp:Content>
