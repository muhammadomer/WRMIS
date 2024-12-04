<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddDischargeTableCalcBL.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddDischargeTableCalcBL" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucGaugeInspection" Src="~/Modules/ScheduleInspection/Controls/GaugeInspection.ascx" TagName="GaugeInspection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="hidden">
                <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                <asp:HiddenField ID="hdnSource" runat="server" Value="0" />
            </div>
            <div class="box">
                <div class="box-title">
                    <h3>Add Inspection Notes - Discharge Table Calculation for Bed Level</h3>
                </div>

                <div class="box-content">

                    <div class="form-horizontal">
                        <ucGaugeInspection:GaugeInspection runat="server" ID="GaugeInspection" />
                    </div>

                    <div class="form-horizontal">


                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Approval Date</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="form-control date-picker" ReadOnly="true" type="text" TabIndex="1" onkeyup="return false;" onkeydown="return false;"></asp:TextBox>
                                            <%--<span id="spantoDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>--%>
                                            <%--<span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="form-control" type="text" TabIndex="1" ReadOnly="true"></asp:TextBox>--%>
                                            <%--<span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblInspectionTime" runat="server" CssClass="col-sm-5 col-lg-4 control-label hidden"></asp:Label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtInspectionTime" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Value of Exponent(n)</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtValExpN" runat="server" CssClass="form-control required decimal2PInput" required="true" TabIndex="2" oninput="ValueValidation(this,'0','5')"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 col-lg-4 control-label">Mean Depth(D in ft)</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtMeanDepth" runat="server" CssClass="form-control required decimal2PInput" required="true" TabIndex="3" oninput="ValueValidation(this,'0','30')"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Observed Discharge(Qo in Cusec)</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtObserveDischarge" runat="server" CssClass="form-control required decimal2PInput" required="true" TabIndex="4" oninput="ValueValidation(this,'0','99999')"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 col-lg-4 control-label">Coefficient of Discharge(K)</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtCoefficientDischarge" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="ddlGaugeCorrectionType" id="lblGaugeCorrectionType" class="col-sm-4 col-lg-3 control-label">Gauge Correction Type</label>
                                            <div class="col-sm-6 col-lg-7 controls">
                                                <asp:DropDownList ID="ddlGaugeCorrectionType" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGaugeCorrectionType_SelectedIndexChanged">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 col-lg-4 control-label">Gauge Value Correction</label>
                                            <div class="col-sm-6 col-lg-7 controls">
                                                <asp:TextBox ID="txtGaugeValCorrection" runat="server" CssClass="form-control decimal2PInput" oninput="ValueValidation(this,'0','30')"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="6" TextMode="MultiLine" Rows="5" MaxLength="250"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtRemarks" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control" TabIndex="6" MaxLength="250"></asp:TextBox>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-5 col-lg-4 control-label">Date of Observation</label>
                                    <div class="col-sm-6 col-lg-7 controls">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtObservationdate" runat="server" CssClass="form-control date-picker required" required="true" TabIndex="7" onkeyup="return false;" onkeydown="return false;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="divSave">
                            <div class="col-md-12">
                                <asp:Button class="btn btn-primary" ID="btnSave" Text="Save & Send Approval" runat="server" OnClick="btnSave_Click" />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                }
            });
        };
    </script>
</asp:Content>
