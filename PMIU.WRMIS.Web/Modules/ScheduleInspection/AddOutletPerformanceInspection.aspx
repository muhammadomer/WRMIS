<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOutletPerformanceInspection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddOutletPerformanceInspection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ScheduleInspection/Controls/OutletInspectionNotes.ascx" TagPrefix="uc1" TagName="OutletPerformanceInspection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Add Inspection Notes - Outlet Performance Inspection</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletTypeID" runat="server" />
            <asp:HiddenField ID="hdnScheduleDetailChannelID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDischarge" runat="server" Value="" />

            <uc1:OutletPerformanceInspection runat="server" ID="OutletPerformanceInspection" />

            <asp:UpdatePanel runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtInspectionDate" runat="server" required="required" CssClass="required form-control disabled-future-date-picker" OnTextChanged="Check_InspectionDates" AutoPostBack="true"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <!-- BEGIN Left Side -->
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtHeadAboveCrest" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Head above Crest of Outlet (H in ft)</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtHeadAboveCrest" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Head above Crest of Outlet" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtOutletHeight" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Height of Outlet/Orifice (Y in ft)</asp:Label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtOutletHeight" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Height of Outlet/Orifice" CssClass="decimal2PInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtDischarge" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Discharge (cusec)</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDischarge" runat="server" oninput="ValidateIntergerInputRange(this,'0','99999')" onblur="CalculateEfficiency();" required="true" CssClass="decimal2PInput form-control required"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- END Left Side -->
                            </div>
                            <div class="col-md-6 ">
                                <!-- BEGIN Right Side -->

                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtWorkingHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Working Head (wh in ft)</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtWorkingHead" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Working Head" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtDBWidth" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Diameter/Breadth/ Width (Dia/B in ft)</asp:Label>

                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDBWidth" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Diameter/Breadth/ Width" CssClass="decimal2PInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtEfficiency" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Efficiency (Observed Discharge/Design Discharge x 100)</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtEfficiency" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" ReadOnly="true" placeholder="" CssClass="decimal2PInput form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- END Right Side -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label AssociatedControlID="txtComments" ID="lblComments" Text="Remarks" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtComments" Rows="5" Columns="50" MaxLength="250" runat="server" ClientIDMode="Static" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" OnClick="btnSave_Click" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:HiddenField runat="server" Value="" ID="hdnDesignDischarge" />
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    InitilizeDatePickerOnUpdatePanelRefresh();
                }
            });
        };

        txtDischarge = $('#<%= txtDischarge.ClientID %>');
        txtEfficiency = $('#<%= txtEfficiency.ClientID %>');
        hdnDesignDischarge = $('#<%= hdnDesignDischarge.ClientID %>');

        function CalculateEfficiency() {
            if (txtDischarge[0].checkValidity()) {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("AddOutletPerformanceInspection.aspx/CalculateEfficiency") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_Discharge: "' + txtDischarge.val() + '",_DesignDischarge:"' + hdnDesignDischarge.val() + '"}',
                    success: function (data) {
                        if (data.d != null && data.d != "") {
                            txtEfficiency.val(data.d);
                        }
                        else {
                            console.log("testing");
                            $('#lblMsgs').addClass('ErrorMsg').show();
                            $('#lblMsgs').html("Cannot divide by Zero");
                            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
