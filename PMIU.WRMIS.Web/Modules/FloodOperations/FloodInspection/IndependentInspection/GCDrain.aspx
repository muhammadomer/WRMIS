<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GCDrain.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.GCDrain" %>

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
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="hidden">
                                    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCDrainID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCDrainCreatedBy" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCDrainCreatedDate" runat="server" Value="0" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExistingCapacity" runat="server" Text="Existing Capacity(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtExistingCapacity" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,4}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblImprovedCapacity" runat="server" Text="Improved Capacity(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtImprovedCapacity" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,4}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblCurrentLevel" runat="server" Text="Current Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtCurrentLevel" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,4}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblDrainWaterEnvironmentallyTreated" runat="server" Text="Drain Water Environmentally Treated" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlDrainWaterEnvironmentallyTreated" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <%--<div class="row">
                                   <div class="col-md-6">
                                       <asp:Label ID="Label1" runat="server" Text="<u>Outfall Current Paramters</u>"  CssClass="col-sm-4 col-lg-5 control-label" />
                                       </div>
                                <div class="col-md-6">
                                    </div>
                                    </div>--%>
                                <div class="row">
                    <div class="col-md-12">
                        <h3>Outfall Current Paramters</h3>

                    </div>

                </div>
                               
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblBedWidth" runat="server" Text="Bed Width (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtBedWidth" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,2}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblFullSupplyDepth" runat="server" Text="Full Supply Width (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtFullSupplyWidth" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,2}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                
                                <%--<div class="row">
                                <div class="col-md-6">
                                       <asp:Label ID="Label2" runat="server" Text="<u>Constructed Bridges</u>" CssClass="col-sm-4 col-lg-5 control-label" />
                                       </div>
                                   <div class="col-md-6">
                                    </div>
                                    </div>--%>
                                             <div class="row">
                    <div class="col-md-12">
                        <h3>Constructed Bridges</h3>

                    </div>

                </div>
                 
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblNumberOfGovernment" runat="server" Text="No. of Government" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtNumberOfGovernment" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,2}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblNumberOfPrivate" runat="server" Text="No. of Private" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtNumberOfPrivate" runat="server" CssClass="form-control text-right" pattern="[0-9].{0,2}"></asp:TextBox>
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
    </div>
</asp:Content>
