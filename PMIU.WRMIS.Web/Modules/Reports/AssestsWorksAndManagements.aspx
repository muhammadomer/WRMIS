<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssestsWorksAndManagements.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Reports.AssestsWorksAndManagements" %>

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
            <h3>Asset Works And Management</h3>
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
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbWorkProgress" Checked="true" runat="server" AutoPostBack="true" GroupName="Reports" Text="Work Progress" OnCheckedChanged="rb_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbAssetsInspections" runat="server" AutoPostBack="true" GroupName="Reports" Text="Assets Inspections " OnCheckedChanged="rb_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:RadioButton CssClass="radio-inline" ID="rbAssetsDetailsID" runat="server" AutoPostBack="true" GroupName="Reports" Text="Assets Details" OnCheckedChanged="rb_CheckedChanged" />
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
                                            <div id="divlevel" runat="server" visible="False">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="ddlLevel" id="lblLevel" class="col-sm-4 col-lg-3 control-label">Level</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged" AutoPostBack="True">
                                                                <asp:ListItem>All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="SPWorkProgress" runat="server">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="ddlYear" id="lblYear" class="col-sm-4 col-lg-3 control-label">Fin. Year</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblWorkType" class="col-sm-4 col-lg-3 control-label">Work Type</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlWorkType" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblProgressStatus" class="col-sm-4 col-lg-3 control-label">Progress Status</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlProgressStatus" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="SPAssetsInspection" visible="false">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblOffice" class="col-sm-4 col-lg-3 control-label">Office</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlOffice" runat="server" CssClass="form-control">
                                                                <asp:ListItem>All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblCategory" class="col-sm-4 col-lg-3 control-label">Category</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblSubCategory" class="col-sm-4 col-lg-3 control-label">Sub Category</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label id="lblStatuts" class="col-sm-4 col-lg-3 control-label">Status</label>
                                                        <div class="col-sm-8 col-lg-9 controls">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-md-offset-1 ">
                                                    <br />
                                                    <div runat="server" id="rbIndividualAndLot" visible="false">
                                                        <asp:RadioButtonList ID="rdoIndividualLot" runat="Server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                                            <asp:ListItem Selected="True" Text="Individual" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Lot" Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div runat="server" id="rbnAssetsDetails" visible="false">
                                                        <asp:RadioButtonList ID="rbAssetsDetails" runat="Server" RepeatDirection="Horizontal" CssClass="My-Radio" AutoPostBack="True" OnSelectedIndexChanged="rbAssetsDetails_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Text="Work on Assets" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Value of Attributes" Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <br />
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div id="DivWorkonAssets" runat="server" visible="False">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblAssetName" class="col-sm-4 col-lg-3 control-label">Asset Name</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlAssetName" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblFinancialYearWork" class="col-sm-4 col-lg-3 control-label">Financial Year</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblWorkTypeW" class="col-sm-4 col-lg-3 control-label">Work Type</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlWorkTypeW" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblProgressStatusW" class="col-sm-4 col-lg-3 control-label">Progress Status</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlProgressStatusw" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div id="DivValueOfAttribute" runat="server" visible="False">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblAttribute" class="col-sm-4 col-lg-3 control-label">Attribute</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlAttribute" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlAttribute_SelectedIndexChanged">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label id="lblAttributeValue" class="col-sm-4 col-lg-3 control-label">Attribute Value</label>
                                                                    <div class="col-sm-8 col-lg-9 controls">
                                                                        <asp:DropDownList ID="ddlAttributeValue" runat="server" CssClass="form-control">
                                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="fnc-btn" style="text-align: right">
                                <asp:Button runat="server" ID="btnComplaintViewReport" OnClick="btnViewReport_Click" Class="btn btn-primary" Text="View" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <iframe id="iframestyle" src="ViewReport.aspx" frameborder="0" allowfullscreen="" style="height: 1350px;" name="iframe" runat="server"></iframe>
                        </div>
                    </div>

                    <%--<div class="col-md-1" style="text-align: right;">--%>
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
