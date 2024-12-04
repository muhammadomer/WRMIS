<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddIndependent.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.AddIndependent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Independent Flood Inspections</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnFloodInspectionID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                            <%--<asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />--%>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="True">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInspectionType" runat="server" Text="Type" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInspectionType" runat="server" CssClass=" required form-control" required="True">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureType" runat="server" Text="Infrastructure Type" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="form-control required" required="True" AutoPostBack="true" OnSelectedIndexChanged="ddlInfrastructureType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblInfrastructureName" runat="server" Text="Infrastructure Name" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <asp:DropDownList ID="ddlInfrastructureName" runat="server" CssClass="form-control required" required="true">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-lg-4 control-label" />
                                    <div class="col-lg-8 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control required date-picker" size="16" type="text" required="True"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
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
    <script type="text/javascript">

        $(document).ready(function () {
            
            if ($('#MainContent_txtDate').attr("readonly") == "readonly"){

                if ($('#MainContent_txtDate').hasClass("date-picker"))
                {
                    $('#MainContent_txtDate').removeClass("date-picker")
                }
            } else {
                if (!$('#MainContent_txtDate').hasClass("date-picker")) {
                    $('#MainContent_txtDate').addClass("date-picker")
                }
            }
            
        });
        
    </script>
</asp:Content>
