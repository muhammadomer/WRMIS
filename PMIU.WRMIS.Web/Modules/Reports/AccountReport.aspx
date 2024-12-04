<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.AccountReport" %>

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
            <h3>Accounts Reports</h3>
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
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-content" style="background: #EEEEEE; padding: 10px 10px 5px 10px;">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbBudgetUtilization" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Budget Utilization" OnCheckedChanged="rbBudgetUtilization_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbTaxSheet" runat="server" AutoPostBack="true" GroupName="Reports" Text="Tax Sheet" OnCheckedChanged="rbTaxSheet_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbHeadWiseExpenditure" runat="server" AutoPostBack="true" GroupName="Reports" Text="Head Wise Expenditure" OnCheckedChanged="rbHeadWiseExpenditure_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbHeadWiseBudgetUtilizationDetails" runat="server" AutoPostBack="true" GroupName="Reports" Text="HeadWise Budget Utilization Details" OnCheckedChanged="rbHeadWiseBudgetUtilizationDetails_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12 col-md-offset-1">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbPrintSanction" runat="server" AutoPostBack="true" GroupName="Reports" Text="Print Sanction" OnCheckedChanged="rbPrintSanction_CheckedChanged" />
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
                                                    <label id="lblRecoveryFinYear" class="col-sm-4 col-lg-4 control-label">Financial Year</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8"></div>
                                        </div>
                                        <div runat="server" id="BudgetUtilization" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblUptoMonth" class="col-sm-4 col-lg-4 control-label">Upto Month</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlUptoMonth" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="TaxSheet" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblMonth" class="col-sm-4 col-lg-4 control-label">Month</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlMonthTax" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblTaxFor" class="col-sm-4 col-lg-4 control-label">Tax for</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlTaxFor" runat="server" AutoPostBack="true" CssClass="form-control"  OnSelectedIndexChanged="ddlTaxFor_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4" id="RepairTypeCol" runat="server">
                                                    <div class="form-group">
                                                        <label id="lblRepairType" class="col-sm-4 col-lg-4 control-label">Repair Type</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlRepairType" runat="server" CssClass="form-control " ></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblTaxOn" class="col-sm-4 col-lg-4 control-label">Tax on</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlTaxOn" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div runat="server" id="HeadWisExpenditure" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblMonthHead" class="col-sm-4 col-lg-4 control-label">Month</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlMonthHead" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblAccountHead" class="col-sm-4 col-lg-4 control-label">Account Head</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlAccountHead" runat="server" AutoPostBack="true" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlAccountHead_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblObjectClassification" class="col-sm-4 col-lg-4 control-label">Object/Class.</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlObjectClassification" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="HWBUDetails" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblObjectClassificationCode" class="col-sm-4 col-lg-4 control-label">Object/Class.</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlObjectClassificationCode" runat="server" AutoPostBack="true" CssClass="form-control required" required="true"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="PrintSanction" visible="false">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblSanctionMonth" class="col-sm-4 col-lg-4 control-label">Month</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlSanctionMonth" runat="server" AutoPostBack="true" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlSanctionMonth_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblSanctionOn" class="col-sm-4 col-lg-4 control-label">Sanction ON</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlSanctionOn" runat="server" AutoPostBack="true" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlSanctionOn_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblSanctionNo" class="col-sm-4 col-lg-4 control-label">Sanction No</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlSanctionNO" runat="server" AutoPostBack="true" CssClass="form-control required" required="true"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblSanctionStatus" class="col-sm-4 col-lg-4 control-label">Sanction Status</label>
                                                        <div class="col-sm-8 col-lg-8 controls">
                                                            <asp:DropDownList ID="ddlSanctionStatus" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                        </div>
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
        <%--        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-content">
                        <iframe id="iframestyle" src="ViewReport.aspx" frameborder="0" allowfullscreen="" name="iframe" runat="server"></iframe>
                    </div>
                </div>
            </div>
        </div>--%>
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
