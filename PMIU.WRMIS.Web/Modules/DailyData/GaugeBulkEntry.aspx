<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GaugeBulkEntry.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.GaugeBulkEntry" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Gauge Bulk Entry</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" Enabled="false" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" Enabled="false" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" Enabled="false" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" ID="ddlSubDivision" runat="server" Enabled="false" required="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Section</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSection" runat="server" Enabled="false" ClientIDMode="Static">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Session</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" ID="ddlSession" runat="server" Enabled="false" required="true" ClientIDMode="Static">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Date</label>
                                    <div class="col-sm-8 col-lg-9 controls" style="margin-top: 3px;">
                                        <asp:Label ID="lblDate" runat="server" CssClass="control-label" Text="09-03-2017" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button TabIndex="10" ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" Enabled="false" OnClick="btnSearch_Click" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="display: none;" runat="server" clientidmode="Static" id="dvGrid">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvGaugeReading" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="False"
                                    OnRowDataBound="gvGaugeReading_RowDataBound" DataKeyNames="ReadingID,MinValue,MaxValue">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Section">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSection" runat="server" CssClass="control-label" Text='<%# Eval("SectionName") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channel">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannel" runat="server" CssClass="control-label" Text='<%# Eval("ChannelName") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reduced Distance (RD)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReducedDistance" runat="server" CssClass="control-label" Text='<%# Eval("RDs") %>' />
                                                <asp:HiddenField ID="hdnGaugeID" runat="server" Value='<%# Eval("GaugeID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gauge Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGaugeCategory" runat="server" CssClass="control-label" Text='<%# Eval("GaugeCategory") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTime" runat="server" CssClass="control-label" Visible="false" />
                                                <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" Text='<%# Eval("ReadingDateTime") %>' MaxLength="5" placeholder="HH:MM" pattern="^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" oninput="javascript:ValidateTime(this)" autocomplete="off" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gauge Value (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGaugeValue" runat="server" CssClass="control-label" Text='<%# Eval("GaugeValue") %>' Visible="false" />
                                                <asp:TextBox ID="txtGaugeValue" runat="server" CssClass="form-control decimalInput" Text='<%# Eval("GaugeValue") %>' autocomplete="off" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discharge (Cusecs)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDischarge" runat="server" CssClass="control-label" Text='<%# Eval("Discharge") %>' />
                                                <asp:HiddenField ID="hdnDischarge" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" />
                                            <ItemStyle CssClass="text-right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="col-md-12 text-right">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ClientIDMode="Static" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#ddlSection').change(function () {
                $('#dvGrid').hide();
            });

            $('#ddlSession').change(function () {
                $('#dvGrid').hide();
            });

            $('#btnSearch').click(function () {
                $("input[name*='txtTime']").removeAttr("pattern");
                var i = 0;
                while ($("input[name*='txtGauge']")[i] != null) {
                    $("input[name*='txtGauge']")[i].setCustomValidity("");
                    $("input[name*='txtTime']")[i].setCustomValidity("");
                    i = i + 1
                }
            });

        });

        function CalculateDischarge(GuageID, GuageInputID, DischargeInputID, HiddenInputID, TimeInputID) {

            var GuageValue = $('#' + GuageInputID).val();

            if (GuageID != '0' && GuageValue.trim() != '' && $('#' + GuageInputID)[0].checkValidity()) {

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("GaugeBulkEntry.aspx/CalculateDischarge") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_GaugeID: "' + GuageID + '",_GaugeValue:"' + GuageValue + '"}',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        if (data.d != null) {
                            $('#' + DischargeInputID).text(data.d);
                            $('#' + HiddenInputID).val(data.d);
                            $('#' + TimeInputID).attr("required", "true");
                        }
                        else {
                            $('#' + DischargeInputID).text("N/A");
                            $('#' + HiddenInputID).val("");
                            $('#' + TimeInputID).removeAttr("required");
                        }
                    }
                });
            }
        }

        function ValidateTime(TxtTimeID) {
            var Time = TxtTimeID.value;

            if (Time.indexOf(':') != -1) {
                var SplittedTime = Time.split(':');

                if ($.isNumeric(SplittedTime[0]) && $.isNumeric(SplittedTime[1])) {
                    var Hours = parseInt(SplittedTime[0]);
                    var Minutes = parseInt(SplittedTime[1]);

                    if ((Hours >= 0 && Hours <= 11) && (Minutes >= 0 && Minutes <= 59)) {
                        TxtTimeID.setCustomValidity("");
                    }
                    else {
                        TxtTimeID.setCustomValidity("Time should be between 00:00 and 11:59");
                    }

                }
                else {
                    TxtTimeID.setCustomValidity("");
                }
            }
            else {
                TxtTimeID.setCustomValidity("");
            }
        }

    </script>
</asp:Content>
