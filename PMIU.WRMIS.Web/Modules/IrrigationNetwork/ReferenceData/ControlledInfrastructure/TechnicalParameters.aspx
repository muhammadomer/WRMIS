<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TechnicalParameters.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.ControlledInfrastructure.TechnicalParameters" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucControlInfrastructureDetails" TagName="ControlInfrastructureDetail" Src="~/Modules/IrrigationNetwork/Controls/ControlledInfrastructureDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:HiddenField ID="hdnControlInfrastructureID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
    <asp:HiddenField ID="hdnStructureTechParaID" runat="server" Value="0" />

    <div class="box">
        <div class="box-title">
            <h3>Technical Parameters of Barrage/Headwork</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucControlInfrastructureDetails:ControlInfrastructureDetail runat="server" />
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="box-content">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-16">
                                    <h3>Design Parameters</h3>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblDesignedDischarge" runat="server" Text="Designed Discharge (cusec)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtDesignedDischarge" runat="server" placeholder="Designed Discharge" CssClass="decimalInput required form-control" required="required"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblNormalPondLevel" runat="server" Text="Normal Pond Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtNormalPondLevel" runat="server" placeholder="Normal Pond Level (ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblUpstreamFloorLevel" runat="server" Text="Upstream Floor Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtUpstreamFloorLevel" runat="server" placeholder="Upstream Floor Level (ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblDownstreamFloorLevel" runat="server" Text="Downstream Floor Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtDownstreamFloorLevel" runat="server" placeholder="Downstream Floor Level (ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblNoOfDivideWalls" runat="server" Text="No. of Divide Walls" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtNoOfDivideWalls" runat="server" placeholder="No. of Divide Walls" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblNoOfTurbinesn" runat="server" Text="No. of Turbines" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtNoOfTurbines" runat="server" placeholder="No. of Turbines" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-12 form-group">
                                            <asp:Label ID="lblWidthbetweenAbutments" runat="server" Text="Width b/w Abutments(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-7 controls">
                                                <asp:TextBox ID="txtWidthbetweenAbutments" runat="server" placeholder="Width between Abutments(ft)" CssClass="integerInput  form-control" MaxLength="99"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <h3>Gates Details</h3>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblWidthofeachWeirGate" runat="server" Text="Width of each Weir Gate(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtWidthOfEachWeirGate" runat="server" placeholder="Width of each Weir Gate(ft)" CssClass="decimalInput required form-control" required="required"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">

                                        <asp:Label ID="lblNoOfUndersluiceGates" runat="server" Text="No. of Undersluice Gates" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfUndersluiceGates" runat="server" placeholder="No. of Undersluice Gates" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblUndersluiceCrestLevel" runat="server" Text="Undersluice Crest Level(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtUndersluiceCrestLevel" runat="server" placeholder="Undersluice Crest Level(ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfLeftUndersluiceGates" runat="server" Text="No. of Left Undersluice" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfLeftUndersluiceGates" runat="server" placeholder="No. of Left Undersluice Gates" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">

                                        <asp:Label ID="lblWidthofeachUndersluiceGate" runat="server" Text="Each Undersluice(ft) Width" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtWidthOfEachUndersluiceGate" runat="server" placeholder="Width of each Undersluice Gate(ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfRightUndersluiceGates" runat="server" Text="No. of Right Undersluice" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfRightUndersluiceGates" runat="server" placeholder="No. of Right Undersluice Gates" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblHeightofeachUndersluiceGate" runat="server" Text="Each Undersluice(ft) Height " CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtHeightOfEachUndersluiceGate" runat="server" placeholder="Height of each Undersluice Gate(ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblType" runat="server" Text="Type" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" data-rule-required="true">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfManualGates" runat="server" Text="No. of Manual Gates" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfManualGates" runat="server" placeholder="No. of Manual Gates" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfElectronicGates" runat="server" Text="No. of Electronic Gates" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNOofElectronicGates" runat="server" placeholder="No. of Electronic Gates" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfElectricalGates" runat="server" Text="No. of Electrical Gates" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfElectricalGates" runat="server" placeholder="No. of Electrical Gates" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfMainWeirGates" runat="server" Text="No. of Main Weir Gates" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfMainWeirGates" runat="server" placeholder="No. of Main Weir Gates" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblMainWeirCrestLevel" runat="server" Text="Main Weir Crest Level (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtMainWeirCrestLevel" runat="server" placeholder="Main Weir Crest Level (ft)" CssClass="decimalInput form-control" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblHeightofeachWeirGate" runat="server" Text="Height of each Weir Gate (ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtHeightOfEachWeirGate" runat="server" placeholder="Height of each Weir Gate (ft)" CssClass="integerInput form-control" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <h3>Fish Ladder Details</h3>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfFishLadders" runat="server" Text="No. of Fish Ladders" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfFishLadders" runat="server" placeholder="No. of Fish Ladders" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblWidthofeachFishLadder" runat="server" Text="Width of each Fish Ladder(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtWidthofeachFishLadder" runat="server" placeholder="Width of each Fish Ladder(ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <h3>Piers Details</h3>
                                <div class="row">
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblNoOfPiers" runat="server" Text="No. of Piers" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtNoOfPiers" runat="server" placeholder="No. of Piers" CssClass="integerInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-sm-12 form-group">
                                        <asp:Label ID="lblWidthofeachPier" runat="server" Text="Width of each Pier(ft)" CssClass="col-sm-4 col-lg-5 control-label" />
                                        <div class="col-sm-8 col-lg-7 controls">
                                            <asp:TextBox ID="txtWidthOfEachPier" runat="server" placeholder="Width of each Pier(ft)" CssClass="decimalInput required form-control" required="required" MaxLength="99"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/ReferenceData/Infrastructure/InfrastructureSearch.aspx" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
