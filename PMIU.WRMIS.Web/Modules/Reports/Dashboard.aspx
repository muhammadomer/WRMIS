<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.Dashboard" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Dashboard</h3>
        </div>
        <div class="box-content" style="padding: 5px;">
            <div class="row">
                <div class="col-md-12">
                    <div class="box" style="margin-bottom: 0px;">
                        <div class="box-content" style="background: #EEEEEE; padding: 5px;">
                            <div class="form-horizontal">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-lg-11">
                                                <div class="col-md-4">
                                                    <div class="form-group" style="margin-bottom: 0px;">
                                                        <label for="ddlZone" id="lblZone" class="col-sm-5 col-lg-3 control-label">Zone</label>
                                                        <div class="col-sm-6 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" class="form-control input-sm" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group" style="margin-bottom: 0px;">
                                                        <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                                        <div class="col-sm-6 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" class="form-control input-sm" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group" style="margin-bottom: 0px;">
                                                        <label for="ddlDivision" id="lblDivision" class="col-sm-5 col-lg-3 control-label">Division</label>
                                                        <div class="col-sm-6 col-lg-9 controls">
                                                            <asp:DropDownList TabIndex="3" ID="ddlDivision" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1" style="float: right;">
                                                <button style="float: right;" id="lnkShow" onclick="DrawDashboard();" class="right btn btn-primary btn-sm">Refresh</button>
                                                <%--<i class="fa fa-refresh"></i>--%>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top: 5px;">
                <div class="col-md-6" style="padding-right: 2.5px;">
                    <div class="box" style="margin-bottom: 0px;">
                        <div class="box-title" style="padding: 5px;">
                            <h3>Tail Status (Field Staff)</h3>
                            <div style="width: 90px; float: right; margin-right: 60px;">
                                <div style="text-align: right; white-space: nowrap;">
                                    <asp:TextBox ID="txtDateTSFS" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-tool" style="top: 7px;">
                                <a href="javascript:void(0);" onclick="TailStatusFieldStaffPieChart();"><i class="fa fa-refresh"></i></a>
                                <a href="javascript:void(0);" onclick="ShowTailStatusField();"><i class="fa fa-external-link"></i></a>
                            </div>
                        </div>
                        <div class="box-content" style="padding: 5px;">
                            <div id="TailStatusFieldStaffChart">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="padding-left: 2.5px;">
                    <div class="box" style="margin-bottom: 0px;">
                        <div class="box-title" style="padding: 5px;">
                            <h3>Tail Status (PMIU Staff)</h3>
                            <div style="width: 90px; float: right; margin-right: 54px;">
                                <div style="text-align: right; white-space: nowrap;">
                                    <asp:TextBox ID="txtDateTSPS" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-tool" style="top: 7px;">
                                <a href="javascript:void(0);" onclick="TailStatusPMIUStaffPieChart();"><i class="fa fa-refresh"></i></a>
                                <a href="javascript:void(0);" onclick="ShowTailStatusPMIU();"><i class="fa fa-external-link"></i></a>
                            </div>
                        </div>
                        <div class="box-content" style="padding: 5px;">
                            <div id="TailStatusPMIUStaffChart"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top: 5px;">
                <div class="col-md-6" style="padding-right: 2.5px;">
                    <div class="box" style="margin-bottom: 0px;">
                        <div class="box-title" style="padding: 5px;">
                            <h3>Complaints Status</h3>
                            <div style="width: 175px; float: right; margin-right: 60px;">
                                <div style="text-align: left; white-space: nowrap;">
                                    <asp:TextBox ID="txtDateCSFrom" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                    <asp:TextBox ID="txtDateCSTo" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-tool" style="top: 7px;">
                                <a href="javascript:void(0);" onclick="ComplaintStatusPieChart();"><i class="fa fa-refresh"></i></a>
                                <a href="javascript:void(0);" onclick="ShowComplaints();"><i class="fa fa-external-link"></i></a>
                            </div>
                        </div>
                        <div class="box-content" style="padding: 5px; text-align:center;">                            
                            <label id="lblTotalComplaints" style="font-weight:bold;"></label>
                            <div id="ComplaintStatusChart"></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6" style="padding-left: 2.5px;">
                    <div class="box" style="margin-bottom: -25px;">
                        <div class="box-title" style="padding: 5px;">
                            <h3>Water Theft Cases</h3>
                            <div style="width: 175px; float: right; margin-right: 60px;">
                                <div style="text-align: left; white-space: nowrap;">
                                    <asp:TextBox ID="txtDateWTCFrom" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                    <asp:TextBox ID="txtDateWTCTo" TabIndex="5" ClientIDMode="Static" runat="server" class="form-control date-picker" size="8" Style="display: inline; width: 85px;" type="text"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-tool" style="top: 7px;">
                                <a href="javascript:void(0);" onclick="WaterTheftStatuse();"><i class="fa fa-refresh"></i></a>
                                <a href="javascript:void(0);" onclick="ShowWaterTheft();"><i class="fa fa-external-link"></i></a>
                            </div>
                        </div>
                        <div class="box-content" style="padding: 5px;">
                            <div id="WaterTheftChart"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top: 5px;">
                <div class="col-md-12" style="padding-right: 14.5px; height: 350px">
                    <div class="box">
                        <div class="box-title" style="padding: 5px;">
                            <h3>Performance Evaluation</h3>
                            <div style="width: 250px; float: right; margin-right: 75px;">
                                <div style="text-align: left; white-space: nowrap;">
                                    <asp:DropDownList Style="display: inline; width: 75%;" ClientIDMode="Static" ID="ddlPE" runat="server"></asp:DropDownList>
                                    <asp:DropDownList Style="display: inline; width: 30%;" ClientIDMode="Static" ID="ddlSession" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="box-tool" style="top: 7px;">
                                <a href="javascript:void(0);" onclick="PerformanceEvaluation();"><i class="fa fa-refresh"></i></a>
                                <%--<a href="javascript:void(0);" onclick="javascript:alert('Under Construction');">
                                    <i class="fa fa-external-link"></i>
                                </a>--%>
                            </div>
                        </div>
                        <div class="box-content" style="padding: 5px;">
                            <div id="PerformanceEvaluationChart"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../Scripts/Complaints/loader.js"></script>
    <script src="../../Scripts/Complaints/jsapi.js"></script>
    <script src="../../Scripts/ReportDashboard/Dashboard.js?0"></script>
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
