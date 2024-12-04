<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelAddition.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel.ChannelAddition" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js?1"></script>
    <%--<script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>--%>
    <%--<script type="text/javascript" src="https://cloud.github.com/downloads/javanto/civem.js/civem-0.0.7.min.js"></script>--%>
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3><span runat="server" id="pageTitleID"></span></h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">

            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <!-- BEGIN Left Side -->
                        <div class="hidden">
                            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIsEdit" runat="server" Value="false" />
                            <asp:HiddenField ID="hdnCurrentTotalRD" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnIsGaugeCalculated" runat="server" Value="false" />
                        </div>
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtChannelName" ID="lblChannelName" Text="Channel Name" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtChannelName" autofocus="autofocus" runat="server" placeholder="Channel name" required="required" data-errormessage-value-missing="This field is required" CssClass="required form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtTotalRDLeft" ID="lblTotalRDs" Text="Total RDs (ft)" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-3 col-lg-4 controls">
                                <asp:TextBox ID="txtTotalRDLeft" runat="server" placeholder="Total RD" CssClass="integerInput RDMaxLength required form-control" required="required" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-sm-1 col-lg-1 controls">
                                +
                            </div>
                            <div class="col-sm-3 col-lg-4 controls">
                                <asp:TextBox ID="txtTotalRDRight" runat="server" placeholder="Total RD" CssClass="integerInput RDMaxLength required form-control" required="required" oninput="CompareRDValues(this)" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="ddlChannelCommandType" ID="lblChannelCommandType" Text="Command Name" runat="server" class="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlChannelCommandType" runat="server" CssClass="required form-control" required="required" TabIndex="6"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtGrossCommandArea" ID="lblGrossCommandArea" Text="GCA - Acre" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtGrossCommandArea" runat="server" placeholder="Gross Command Area" class="decimalInput form-control" MaxLength="12" TabIndex="8" oninput="ValidateGCA(this)"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtChannelNameAbbreviation" ID="lblChannelNameAbbreviation" Text="Channel ABBR" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtChannelNameAbbreviation" runat="server" placeholder="Channel Name Abbreviation" class="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <!-- END Left Side -->
                    </div>
                    <div class="col-md-6 ">
                        <!-- BEGIN Right Side -->

                        <div class="form-group">
                            <asp:Label AssociatedControlID="" ID="lblChannelType" Text="Channel Type" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlChannelType" runat="server" class="required form-control" required="required" TabIndex="2"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="ddlChannelFlowType" ID="lblChannelFlowType" Text="Flow Type" runat="server" class="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlChannelFlowType" runat="server" class="required form-control" required="required" TabIndex="5"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtAuthorizedTailGauge" ID="lblAuthorizedTailGauge" Text="Auth Tail Gauge (ft)" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>                                    
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtAuthorizedTailGauge" runat="server" placeholder="Authorized Tail Gauge" class="decimalInput required form-control" required="required" TabIndex="7"></asp:TextBox>
                                    </div>
                        </div>

                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCultureableCommandArea" ID="lblCultureableCommandArea" Text="CCA - Acre" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCultureableCommandArea" runat="server" placeholder="Cultureable Command Area" class="decimalInput form-control" MaxLength="12" oninput="ValidateCCA(this)" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtRemarks" ID="lblRemarks" Text="Remarks" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <%--<asp:TextBox ID="TextBox1" runat="server" placeholder="Cultureable Command Area" class="decimalInput form-control" MaxLength="12" oninput="ValidateCCA(this)" TabIndex="9"></asp:TextBox>--%>
                                <asp:TextBox ID="txtRemarks" runat="server" onblur="TrimInput(this);" CssClass="form-control multiline-no-resize" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="visibility:hidden;">
                            <asp:Label AssociatedControlID="txtIMISCode" ID="lblIMISCode" Text="IMIS Code" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtIMISCode" runat="server" placeholder="IMIS Code" class="integerInput IMISCodeLength form-control" TabIndex="11" oninput="ValidateIMISCodeLength(this)"></asp:TextBox>
                            </div>
                        </div>
<%--                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtIMISCode" ID="lblIMISCode" Text="IMIS Code" runat="server" class="col-xs-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtIMISCode" runat="server" placeholder="IMIS Code" pattern=".{17,}" MaxLength="17" class="integerInput form-control" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>--%>

                        <!-- END Right Side -->
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/IrrigationNetwork/Channel/ChannelSearch.aspx" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
</asp:Content>
