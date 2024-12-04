<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InfrastructureAddUpdate.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Infrastructure.InfrastructureAddUpdate" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Add/Edit Protection Infrastructure</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <div class="hidden">
                            <asp:HiddenField ID="hdnInfrastructureID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCurrentTotalRD" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnInfrastructureCreatedDate" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />
                        </div>
                        <h3>Protection Infrastructures</h3>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="ddlInfrastructureType" ID="lblType" Text="Type" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:DropDownList ID="ddlInfrastructureType" runat="server" CssClass="required form-control" required="required" data-errormessage-value-missing="This field is required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtName" ID="lblName" Text="Name" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="input required form-control" required="required" pattern="[0-9a-zA-Z.\.\-_:/,!\(\)\[\]\{\}].{2,250}" title="Name must be of minim 2 characters which is upto 250 characters" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtTotalLength" ID="lblTotalLength" Text="Total Length (ft)" runat="server" CssClass="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtTotalLength" runat="server" CssClass="integerInput required form-control" required="required" pattern="[0-9]{1,8}" MaxLength="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtInitialCost" ID="lblInitialCost" Text="Initial Cost (Rs)" runat="server" class="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtInitialCost" runat="server" CssClass="integerInput form-control" pattern="[0-9]{0,20}" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 text-left form-group">
                                <asp:Label ID="lblInfrastructureStatus" runat="server" Text="Status" CssClass="col-xs-4 col-lg-4 control-label" />
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:RadioButtonList ID="rdolInfrastructureStatus" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                        <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">InActive</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 text-left form-group"></div>
                        </div>

                        <h3>Designed Parameters Details</h3>
                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtDesignedTopWidth" ID="lblDesignedTopWidth" Text="Top Width (ft)" runat="server" class="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtDesignedTopWidth" runat="server" class="integerInput required form-control" required="required" pattern="[0-9]{1,4}" MaxLength="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtDesignedFreeBoard" ID="lblDesignedFreeBoard" Text="Free Board (ft)" runat="server" class="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-8 col-lg-8 controls">
                                    <asp:TextBox ID="txtDesignedFreeBoard" runat="server" class="integerInput required form-control" required="required" pattern="[0-9]{1,4}" MaxLength="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtCountrySideSlope1" ID="lblCountrySideSlope" Text="Country Side Slope" runat="server" class="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-2 col-lg-2 controls">
                                    <asp:TextBox ID="txtCountrySideSlope1" runat="server" class="integerInput form-control" MaxLength="1" pattern="[0-9].{0,1}"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 col-lg-1 controls text-center">
                                    : 
                                </div>
                                <div class="col-sm-2 col-lg-2 controls">
                                    <asp:TextBox ID="txtCountrySideSlope2" runat="server" class="integerInput form-control" MaxLength="1" pattern="[0-9].{0,1}"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-12 form-group">
                                <asp:Label AssociatedControlID="txtDesignedRiverSideSlope1" ID="lbltxtDesignedRiverSideSlope" Text="River Side Slope" runat="server" class="col-xs-4 col-lg-4 control-label"></asp:Label>
                                <div class="col-sm-2 col-lg-2 controls">
                                    <asp:TextBox ID="txtDesignedRiverSideSlope1" runat="server" class="integerInput form-control" MaxLength="1" pattern="[0-9].{0,1}"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 col-lg-1 controls text-center">
                                    : 
                                </div>
                                <div class="col-sm-2 col-lg-2 controls">
                                    <asp:TextBox ID="txtDesignedRiverSideSlope2" runat="server" class="integerInput form-control" MaxLength="1" pattern="[0-9].{0,1}"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12 text-left form-group">
                                <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="col-xs-2 col-lg-2 control-label" Style="padding: 0px 1.9% 0 0;" />

                                <div class="col-lg-10 col-sm-10 controls" style="padding: 0px 4% 0 1%;">
                                    <asp:TextBox TextMode="MultiLine" Height="100" ID="txtDescription" runat="server" class="input form-control" pattern=".{0,250}" MaxLength="250"></asp:TextBox>
                                </div>

                            </div>
                        </div>

                    </div>
                    <br />

                    <div class="col-md-12">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- END Main Content -->
</asp:Content>

