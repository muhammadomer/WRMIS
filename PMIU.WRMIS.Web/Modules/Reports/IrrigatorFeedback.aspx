<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IrrigatorFeedback.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.IrrigatorFeedback" %>

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
            <h3>Irrigator Feedback Report</h3>
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
                                                    <label id="lblChannel" class="col-sm-4 col-lg-3 control-label">Channel</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                        </div>
                                        <div class="row">
                                            <div runat="server" id="SPWaterTheftOutlet">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="ddlOutlet" id="lblOutlet" class="col-sm-4 col-lg-3 control-label">Tail Status</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlTailStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="ddlOutletCondition" id="lblOutletCondition" class="col-sm-4 col-lg-3 control-label">Rot. Violation</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlRotationalVoilation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div runat="server" id="SPBreach" class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlBreachLength" id="lblBreachLength" class="col-sm-4 col-lg-3 control-label">Water Theft</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlWaterTheft" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                <asp:Button runat="server" ID="btnIrrigatorFeedback" OnClick="btnIrrigatorFeedback_Click" CssClass="btn btn-primary" Text="View" />
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
</asp:Content>
