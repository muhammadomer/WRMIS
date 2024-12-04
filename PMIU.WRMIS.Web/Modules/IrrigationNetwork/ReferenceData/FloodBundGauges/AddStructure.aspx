<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStructure.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.FloodBundGauges.AddStructure" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Add Sructures</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnStructureNallahHillTorentID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                            <%--<asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />--%>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label ID="lblStructureType" Text="Structure Type" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlStructureType" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label ID="lblStructureName" Text="Structure Name" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtStructureName" runat="server" CssClass="input required form-control" required="required" pattern="[0-9a-zA-Z.\.\-_:/,!\(\)\[\]\{\}].{2,250}" title="Name must be of minim 2 characters which is upto 250 characters" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 text-left form-group">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-xs-4 col-lg-4 control-label" />
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:RadioButtonList ID="rdolStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                        <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">InActive</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 text-left form-group"></div>
                        </div>
                        <h3>Physical Location of the Gauge</h3>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblZone" class="col-xs-4 col-lg-4 control-label">Zone</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblCircle" class="col-xs-4 col-lg-4 control-label">Circle</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" AutoPostBack="True">
                                        <%--<asp:ListItem Value="">Select</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblDivision" class="col-xs-4 col-lg-4 control-label">Division</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="">Select</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblDistrict" class="col-xs-4 col-lg-4 control-label">District</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control required" required="True" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="">Select</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblTehsil" class="col-xs-4 col-lg-4 control-label">Tehsil</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlTehsil" runat="server" CssClass=" form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="">Select</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblVillage" class="col-xs-4 col-lg-4 control-label">Village</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlVillage" runat="server" CssClass="form-control ">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <label id="lblDesignedDischarge" class="col-xs-4 col-lg-4 control-label">Designed Discharge (cusec)</label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtDesignedDischarge" runat="server" CssClass=" form-control" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~\Modules\IrrigationNetwork\ReferenceData\ControlledInfrastructure\Search.aspx" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
