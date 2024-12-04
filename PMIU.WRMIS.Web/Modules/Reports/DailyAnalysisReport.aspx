<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyAnalysisReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.DailyAnalysisReport" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
            <h3>Daily Analysis Reports</h3>
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
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDailyAnalysis" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Daily Analysis" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDailyStatusOfChannels" runat="server" AutoPostBack="true" GroupName="Reports" Text="Daily Status of Channels" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbComparisonOfShortDryTails" runat="server" AutoPostBack="true" GroupName="Reports" Text="Comparison of Short and  Dry Tails" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
                                                    <label id="lblDate" class="col-sm-4 col-lg-3 control-label">Date</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <div class="input-group date" data-date-viewmode="years">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text"></asp:TextBox>
                                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label id="lblSession" class="col-sm-4 col-lg-3 control-label">Session</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPComparisonOfShortDryTails" visible="false">
                                                <div class="form-group">
                                                    <label id="lblYearsForComparison" class="col-sm-4 col-lg-3 control-label">Years</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlYearsForComparison" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPDailyChannelsStatus" visible="false">
                                                <div class="form-group">
                                                    <label for="ddlIrrigationLevel" id="lblIrrigationLevel" class="col-sm-4 col-lg-3 control-label">Irrigation Level</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlIrrigationLevel" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <%--                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" Checked="true" ID="rbZone" runat="server" GroupName="SearchType" Text="Zone" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbCircle" runat="server" GroupName="SearchType" Text="Circle" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDivision" runat="server" GroupName="SearchType" Text="Division" />
                                                    </div>
                                                </div>
                                            </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1" style="text-align:right;">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnLoadDailyData" OnClick="btnLoadDailyData_Click" CssClass="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    
                    </div>
                  
                    <div class="row">
                        <div class="col-md-12">
<%--                            <div class="box">
                                <div class="box-content">--%>
                                    <iframe id="iframestyle" src="ViewReport.aspx" frameborder="0" allowfullscreen="" style="height:1350px;" name="iframe" runat="server"></iframe>
<%--                                </div>
                            </div>--%>
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
