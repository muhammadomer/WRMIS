<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntitlementDeliveriesReports.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.EntitlementDeliveriesReports" %>

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
            <h3>Entitlement & Deliveries Reports</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#">
                    <i class="fa fa-chevron-up"></i>
                </a>
            </div>
        </div>
        <div class="box-content" style="padding-bottom: 0px;">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-11">
                            <div class="box">
                                <div class="box-content" style="background: #EEEEEE; padding: 10px 10px 5px 10px;">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label id="lblReports" class="col-sm-5 col-lg-4 control-label">Reports</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="1" ID="ddlReports" runat="server" required="required" CssClass="form-control required" OnSelectedIndexChanged="ddlReports_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="ddlCommandTypeDiv" runat="server" visible="true">
                                                <div class="form-group">
                                                    <label id="lblCommandType" class="col-sm-5 col-lg-4 control-label">Command Type</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="2" ID="ddlCommandType" required="required" class="required" runat="server" CssClass="form-control required"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label id="lblSeason" class="col-sm-5 col-lg-4 control-label">Season</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="3" ID="ddlSeason" required="required" runat="server" CssClass="form-control required" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="ddlMonthDiv" runat="server" visible="false">
                                                <div class="form-group">
                                                    <label id="lblMonth" class="col-sm-5 col-lg-4 control-label">Month</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="4" ID="ddlMonth" required="required" runat="server" CssClass="form-control required" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="row">
                                            <div class="col-md-4" id="ddlYearDiv" runat="server" visible="false">
                                                <div class="form-group">
                                                    <label id="lblYear" class="col-sm-5 col-lg-4 control-label">Year</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="2" ID="ddlYear" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" id="ddlTenDailyDiv" runat="server" visible="false">
                                                <div class="form-group">
                                                    <label id="lblTenDaily" class="col-sm-5 col-lg-4 control-label">Ten Daily</label>
                                                    <div class="col-sm-7 col-lg-8 controls">
                                                        <asp:DropDownList TabIndex="2" ID="ddlTenDaily" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-1">
                            <div class="fnc-btn" style="text-align: right">
                                <asp:Button runat="server" ID="btnLoad" OnClick="btnLoad_Click" CssClass="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12" style="text-align: center;">
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
    </script>
    <script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>
</asp:Content>
