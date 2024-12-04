<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyWaterPosition.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.DailyWaterPosition" %>

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
            <h3>Daily Water Position</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#">
                    <i class="fa fa-chevron-up"></i>
                </a>
            </div>
        </div>
        <div class="box-content" style="padding-bottom: 0px;">
            <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-content" style="background: #EEEEEE; padding: 10px 10px 5px 10px;">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbCanalWithdrawals" OnCheckedChanged="rb_CheckedChanged" runat="server" AutoPostBack="true" GroupName="Reports" Text="Canal Withdrawals" Checked="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbChannelWithdrawals" OnCheckedChanged="rb_CheckedChanged" runat="server" AutoPostBack="true" GroupName="Reports" Text="Channel Withdrawals" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDailyGauge" OnCheckedChanged="rb_CheckedChanged" runat="server" AutoPostBack="true" GroupName="Reports" Text="Daily Gauge" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDailyWaterAvailabilityPosition" OnCheckedChanged="rb_CheckedChanged" runat="server" AutoPostBack="true" GroupName="Reports" Text="Daily Water Availability Position" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbSuppyPosition" runat="server" AutoPostBack="true" GroupName="Reports" Text="Supply Position" OnCheckedChanged="rb_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbCanalWireIndus" runat="server" AutoPostBack="true" GroupName="Reports" Text="Canal Wire Indus" OnCheckedChanged="rb_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbCanalWireJC" runat="server" AutoPostBack="true" GroupName="Reports" Text="Canal Wire JC" OnCheckedChanged="rb_CheckedChanged" />
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

                                        <div runat="server" id="divDailyWaterAvailabilityPosition">
                                            <div class="row">

                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblFromDate" class="col-sm-4 col-lg-3 control-label">Date</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <div class="input-group date" data-date-viewmode="years">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4" id="Scenario" runat="server" visible="false">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 col-lg-3 control-label">Scenario</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlScenario" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnDailyWaterPositionReports" OnClick="btnDailyWaterPositionReports_Click" CssClass="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <iframe id="iframestyle" src="ViewReport.aspx" frameborder="0" allowfullscreen="" style="height: 1350px;" name="iframe" runat="server"></iframe>
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


        var ddlScenario = $('#<%= ddlScenario.ClientID %>');
        $(function () {
            //$('#rbCanalWithdrawals').click(function () {
            //    $('#Scenario').hide();
            //    DisableDesignParameter(ddlScenario);
            //})

            $('#rbCanalWithdrawals, #rbDailyGauge, #rbChannelWithdrawals').each(function () {
                $('#Scenario').hide();
                DisableDesignParameter(ddlScenario);
            })

            function DisableDesignParameter(fieldID) {
                fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");
            }
            function EnableDesignParameter(fieldID) {
                fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
            }
        });

    </script>
</asp:Content>
