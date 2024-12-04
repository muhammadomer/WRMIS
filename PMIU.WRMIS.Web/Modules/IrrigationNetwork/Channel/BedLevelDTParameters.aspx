<%@ Page Title="Bed Level DT Parameters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BedLevelDTParameters.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.BedLevelDTParameters" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Discharge Table Parameters for Gauge on Bed Level</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="table-responsive">
                    <table class="table tbl-info">
                        <tr>
                            <th>Channel Name</th>
                            <th>Channel Type</th>
                            <th>Total RDs (ft)</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblChannelName" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblChannelType" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblTotalRDs" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <th>Flow Type</th>
                            <th>Command Name</th>
                            <th>Category of Gauge</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFlowType" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblCommandName" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblCategoryOfGauge" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Date of Approval</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtReadingDate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="1" onkeyup="return false;" onkeydown="return false;" />
                                    <asp:Label ID="lblGaugeID" runat="server" CssClass="control-label" Visible="false" />
                                    <asp:Label ID="lblChannelID" runat="server" CssClass="control-label" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Date of Observation</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtObservationdate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="2" onkeyup="return false;" onkeydown="return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Value of Exponent (n)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtExponentValue" CssClass="form-control required decimalInput" MaxLength="10" required="true" TabIndex="2" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">
                                Mean Depth<br>
                                (D in ft)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtMeanDepth" CssClass="form-control required decimalInput" MaxLength="10" required="true" TabIndex="3" />
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">
                                Observed Discharge<br>
                                (Qo Cusec)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtObservedDischarge" CssClass="form-control required decimalInput" MaxLength="10" required="true" TabIndex="4" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Gauge Correction Type</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:RadioButton runat="server" CssClass="radio-inline" Text="Bed Silted" ID="rbBedSilted" GroupName="GaugeCorrectionType" AutoPostBack="True" OnCheckedChanged="rbBedSilted_CheckedChanged" />
                                <asp:RadioButton runat="server" CssClass="radio-inline" Text="Bed Scoured" ID="rbBedScoured" GroupName="GaugeCorrectionType" AutoPostBack="True" OnCheckedChanged="rbBedScoured_CheckedChanged" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Gauge Value Correction</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtCorrectionValue" CssClass="form-control" MaxLength="10" Enabled="false" required="true" TabIndex="5" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Coefficient of Discharge (K)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox runat="server" ID="txtDischargeCoefficient" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                            <asp:LinkButton runat="server" ID="lbtnBack" CssClass="btn" Text="Back" OnClick="lbtnBack_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

