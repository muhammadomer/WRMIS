<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEmergencyPurchases.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases.AddEmergencyPurchases" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Emergency Purchases</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnEmergencyPurchasesID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCurrentTotalRD" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnTotalRD" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnStructureType" runat="server" Value="0" />
                            
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-4 control-label" />
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="True">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-sm-4 col-lg-4 control-label" />
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control required" required="True" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-sm-4 col-lg-4 control-label" />
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control required" required="true">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblCampSite" runat="server" Text="Camp Site" CssClass="col-sm-4 col-lg-4 control-label" />
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <asp:DropDownList runat="server" ID="ddlCampSite" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlCampSite_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblTotalRDs" Text="RD" runat="server" CssClass="col-sm-4 col-lg-4 control-label"></asp:Label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtRDLeft" runat="server" CssClass="integerInput RDMaxLength form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1 controls">
                                        +
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtRDRight" runat="server" CssClass="integerInput RDMaxLength form-control" oninput="CompareRDValues(this)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblYear" runat="server" Text="Year" CssClass="col-sm-4 col-lg-4 control-label" />
                                    <div class="col-sm-8 col-lg-8 controls">
                                        <asp:TextBox ID="txtCurrentyear" runat="server" CssClass="integerInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
