<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ComplaintReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.ComplaintReport" %>

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
            <h3>Complaint Reports</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#">
                    <i class="fa fa-chevron-up"></i>
                </a>
            </div>
        </div>
        <div class="box-content" style="padding-bottom: 0px;">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbComplaintSource" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Complaint Source" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbComplaintType" runat="server" AutoPostBack="true" GroupName="Reports" Text="Complaint Type" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbComplaintStatus" runat="server" AutoPostBack="true" GroupName="Reports" Text="Complaint Status" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbDirectives" runat="server" AutoPostBack="true" GroupName="Reports" Text="Directives" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbStructure" runat="server" AutoPostBack="true" GroupName="Reports" Text="Structure" OnCheckedChanged="rbReports_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbComplaintDetails" runat="server" AutoPostBack="true" GroupName="Reports" Text="Complaint Details" OnCheckedChanged="rbReports_CheckedChanged" />
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
                                            <div runat="server" id="SPComplaintType" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblComplaintType" class="col-sm-4 col-lg-3 control-label">Type</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlComplaintType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPComplaintDetails" visible="false">
                                                <div class="form-group">
                                                    <label id="lblComplaintSource" class="col-sm-4 col-lg-3 control-label">Source</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlComplaintSource" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="SPComplaintStatus" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblComplaintStatus" class="col-sm-4 col-lg-3 control-label">Status</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlComplaintStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPAssignedTo" visible="false">
                                                <div class="form-group">
                                                    <label id="lblAssignedTo" class="col-sm-4 col-lg-3 control-label">Assigned To</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlAssignedTo" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPDirective" visible="false">
                                                <div class="form-group">
                                                    <label id="lblDirectiveTypes" class="col-sm-4 col-lg-3 control-label">Type</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlDirectiveTypes" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="SPStructure" visible="false">
                                                <div class="form-group">
                                                    <label id="lblStructure" class="col-sm-4 col-lg-3 control-label">Structure</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlStructure" runat="server"  class="required" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddlStructure_SelectedIndexChanged" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4" runat="server" id="divFirst" visible="false">
                                                <div class="form-group">
                                                    <label id="lblFirst" runat="server" class="col-sm-4 col-lg-3 control-label">Channels</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlFirst" runat="server" OnSelectedIndexChanged="ddlFirst_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4" runat="server" id="divSecond" visible="false">
                                                <div class="form-group">
                                                    <label id="lblSecond" class="col-sm-4 col-lg-3 control-label">Outlets</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlSecond" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="SPComplaintSource">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:CheckBox CssClass="radio-inline" Checked="true" ID="chkFrames" runat="server" Text="Farmers" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:CheckBox CssClass="radio-inline" Checked="true" ID="chkPMIU" runat="server" Text="PMIU" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:CheckBox CssClass="radio-inline" Checked="true" ID="chkDirectives" runat="server" Text="Directives" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3" style="visibility:hidden; width: 24%; margin-left: 8px;">
                                                <div class="form-group">
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:CheckBox CssClass="radio-inline" Checked="true" ID="chkOverall" runat="server" Text="Overall" />
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
                                <asp:Button runat="server" ID="btnComplaintViewReport"  OnClick="btnViewReport_Click" Class="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    </div>
                    <div style="clear:both"></div>
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
