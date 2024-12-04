<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GCProtectionInfrastructure.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.GCProtectionInfrastructure" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>General Conditions for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="hidden">
                                <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnGCProtectionInfrastructureID" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnGCProtectionInfrastructureCreatedBy" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnGCProtectionInfrastructureCreatedDate" runat="server" Value="0" />
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblGaugeReaderHutCondition" runat="server" Text="Gauge Reader Hut Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:DropDownList ID="ddlGaugeReaderHutCondition" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblWatchingHutCondition" runat="server" Text="Watching Hut Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:DropDownList ID="ddlWatchingHutCondition" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblRiverGaugeCondition" runat="server" Text="River Gauge Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:DropDownList ID="ddlRiverGaugeCondition" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <asp:Label ID="lblServiceRoadCondition" runat="server" Text="Service Road Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-5 controls">
                                            <asp:DropDownList ID="ddlServiceRoadCondition" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="margin-left: 12px;">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="col-sm-4 col-lg-12 controls" />
                                    <div class="col-sm-8 col-lg-11 controls" style="margin-top: 6px;">
                                        <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Rows="4" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn" style="margin-left: 26px;">
                                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                        <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" />--%>
                                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
