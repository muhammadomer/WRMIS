<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeasonalPlanning.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.SeasonalPlanning" %>

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
            <h3>Seasonal Planning Reports</h3>
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
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbSeasonalPlanning" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Seasonal Planning" OnCheckedChanged="rbSeasonalPlanning_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbRiverFlows" runat="server" AutoPostBack="true" GroupName="Reports" Text="River Flows at RIM Stations" OnCheckedChanged="rbRiverFlows_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="divSeasonalPlanning">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="ddlZone" id="lblSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="1" ID="ddlSeason" required="required" runat="server" CssClass="form-control required" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="SPYear" visible="false" runat="server">
                                                    <div class="form-group">
                                                        <label for="ddlCircle" id="lblYear" class="col-sm-4 col-lg-3 control-label">Year</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="2" ID="ddlYear" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="SPReports" visible="false" runat="server">
                                                    <div class="form-group">
                                                        <label for="ddlDivision" id="lblReports" class="col-sm-4 col-lg-3 control-label">Reports</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="3" ID="ddlReports" required="required" class="required" runat="server" CssClass="form-control required"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div  runat="server" id="divRiverFlow" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblRiverSeason" class="col-sm-4 col-lg-3 control-label">Season</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="1" ID="ddlRiverSeason" required="required" runat="server" CssClass="form-control required" OnSelectedIndexChanged="ddlRiverSeason_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="RFFromYear" visible="false" runat="server">
                                                    <div class="form-group">
                                                        <label id="lblFromYear" class="col-sm-4 col-lg-3 control-label">From Year</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="2" ID="ddlFromYear" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="RFToYear" visible="false" runat="server">
                                                    <div class="form-group">
                                                        <label id="lblToYear" class="col-sm-4 col-lg-3 control-label">To Year</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="3" ID="ddlToYear" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4" id="divTDaily" visible="false" runat="server">
                                                    <div class="form-group">
                                                        <label id="lblTDaily" class="col-sm-4 col-lg-3 control-label">Ten Daily</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="4" ID="ddlTDaily" runat="server" required="required" CssClass="form-control required"></asp:DropDownList>
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
                                <asp:Button runat="server" ID="btnSeasonalPlanningReport" OnClick="btnSeasonalPlanningReport_Click" CssClass="btn btn-primary" Text="View" />
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
    </script>
</asp:Content>
