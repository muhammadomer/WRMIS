<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddGaugeInspection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddGaugeInspection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucGaugeInspection" Src="~/Modules/ScheduleInspection/Controls/GaugeInspection.ascx" TagName="GaugeInspection" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Add Inspection Notes - Gauge Inspection</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <ucGaugeInspection:GaugeInspection runat="server" ID="GaugeInspection" />
                    </div>
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="form-control disabled-future-date-picker required" required="true" type="text" OnTextChanged="Check_InspectionDates" AutoPostBack="true"></asp:TextBox>
                                            <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblInspectionTime" runat="server" CssClass="col-sm-4 col-lg-3 control-label hidden"></asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtInspectionTime" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>--%>
                                <div class="row">
                                    <div class="col-md-6" <%--   style="border-bottom:1px solid grey"--%>>
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label" style="margin-left: 0px;">Gauge Fixed</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:RadioButtonList ID="RadioButtonListGaugeFixed" CssClass="My-Radio" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Check_GaugeValueEnabled" AutoPostBack="true">
                                                    <asp:ListItem class="radio-inline" Text="Yes" Value="True" Selected="True" />
                                                    <asp:ListItem class="radio-inline" Text="No" Value="False" style="margin-left: 15px;" />
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label" style="margin-left: 0px;">Gauge Painted</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:RadioButtonList ID="RadioButtonListGaugePainted" runat="server" CssClass="My-Radio" RepeatDirection="Horizontal" OnSelectedIndexChanged="Check_GaugeValueEnabled" AutoPostBack="true">
                                                    <asp:ListItem class="radio-inline" Text="Yes" Value="True" Selected="True" />
                                                    <asp:ListItem class="radio-inline" Text="No" Value="False" style="margin-left: 15px;" />
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Gauge (ft)</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtGaugeFt" runat="server" onKeyPress="return check(event,value)" onInput="ValidateGaugeValues(this)" CssClass="decimalInput form-control GuageFt" Enabled="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Discharge (Cusec)</label>
                                            <div class="col-sm-8 col-lg-9 controls" style="padding-top: 2px;">
                                                <asp:Label ID="lblDischarge" runat="server" Text="--" Font-Bold="true" ClientIDMode="Static"></asp:Label>
                                            </div>
                                        </div>
                                    </div>



                                </div>
                        <%--    </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachment</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%--<asp:FileUpload ID="FileUploadGauge" runat="server" />--%>
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemarks" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="5" MaxLength="250"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="table-responsive">
                            <asp:GridView ID="gvComplaints" runat="server" Visible="false" AutoGenerateColumns="False" DataKeyNames="ComplaintID" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" AllowPaging="true" CssClass="table header"
                                BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Complaint Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInspectionDatetime" runat="server" Text='<%# Eval("ComplaintType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInspectionType" runat="server" Text='<%# Eval("ComplaintNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>


                            <div class="row" runat="server" id="divSave">
                                <div class="col-md-12">
                                    <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" OnClientClick="return ValidateRequiredFields();" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                                    <asp:LinkButton ID="lbtnBackToET" runat="server" Visible ="false" Text="Back" class="btn" PostBackUrl="~/Modules/DailyData/MeterReadingAndFuel.aspx?ShowSearched=true"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnDivisionID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnGaugeReadingID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnGaugeID" runat="server" Value="0" />

    <asp:HiddenField ID="hdnGaugeMinValue" runat="server" Value="" />
    <asp:HiddenField ID="hdnGaugeMaxValue" runat="server" Value="" />

    <%--<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>--%>
    <script>
        gminValue = $('#<%= hdnGaugeMinValue.ClientID %>');
        gmaxValue = $('#<%= hdnGaugeMaxValue.ClientID %>');
        txtGaugeFt = $('#<%= txtGaugeFt.ClientID %>');
        lblDischarge = $('#<%= lblDischarge.ClientID %>');
        hdnGaugeID = $('#<%= hdnGaugeID.ClientID %>');

        function check(e, value) {
            //Check Charater
            var unicode = e.charCode ? e.charCode : e.keyCode;
            if (value.indexOf(".") != -1) if (unicode == 46) return false;
            if (unicode != 8) if ((unicode < 48 || unicode > 57) && unicode != 46) return false;
        };

        function ValidateRequiredFields() {
            if (Page_ClientValidate()) {

                return confirm('Once saved, you cannot edit/delete this record. Are you sure you want to save this record?');
            }
            return false;
        }

        function ValidateGaugeValues(txtInput) {
            //console.log(gminValue);
            //console.log(gmaxValue);
            var minValue = gminValue.val().trim();
            var maxValue = gmaxValue.val().trim();

            var inputValue = txtInput.value.trim();
            if (inputValue.length != 0 && !(inputValue === "")) {
                // console.log('validate input');
                if (!(minValue === "") && !(maxValue === "")) {
                    //  console.log('validate min max value');
                    inputValue = parseInt(inputValue);

                    if (inputValue < parseInt(minValue) || inputValue > parseInt(maxValue)) {
                        //console.log('input is invalid');
                        txtInput.setCustomValidity("Value should be between " + minValue + " and " + maxValue);
                        console.log('vali');
                    }
                    else {
                        txtInput.setCustomValidity("");
                    }
                }
                else {
                    txtInput.setCustomValidity("");
                }
            }
            else {
                txtInput.setCustomValidity("");
            }
        }

        function CalculateDischarge(GuageID, GuageInputID, DischargeInputID) {

            var GuageValue = $('#' + GuageInputID).val();

            if (GuageID != '0' && GuageValue.trim() != '' && $('#' + GuageInputID)[0].checkValidity()) {

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("AddGaugeInspection.aspx/CalculateDischarge") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    data: '{_GaugeID: "' + GuageID + '",_GaugeValue:"' + GuageValue + '"}',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        var a = data.d.split(';');
                        //console.log(a[0]);
                        if (a[0] != null && a[1] != "") {
                            //console.log("1");
                            //lblDischarge.text(a[0]);
                            $('#' + DischargeInputID).text(a[0]);
                            $('#lblMsgs').addClass('ErrorMsg').show();
                            $('#lblMsgs').html(a[1]);
                            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                        else if (a[0] != null && a[0] != "") {
                            //console.log("2nd");
                            //lblDischarge.text(a[0]);
                            $('#' + DischargeInputID).text(a[0]);
                            //console.log("testing");
                            //$('#lblMsgs').addClass('ErrorMsg').show();
                            //$('#lblMsgs').html("In absence of discharge table parameter, discharge cannot be calculated");
                            //setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                       else {
                            //console.log("3");
                            $('#lblMsgs').addClass('ErrorMsg').show();
                            $('#lblMsgs').html("In absence of discharge table parameter, discharge cannot be calculated");
                            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>


