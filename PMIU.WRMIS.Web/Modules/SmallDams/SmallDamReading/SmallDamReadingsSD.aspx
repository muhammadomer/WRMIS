<%@ Page Title="SmallDamReadingsSD" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SmallDamReadingsSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading.SmallDamReadingsSD" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnDamID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDate" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Add Daily Readings</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" ValidationGroup="Search">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" runat="server" AutoPostBack="true" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged" ValidationGroup="Search">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDamName" runat="server" Text="Dam Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDamName" runat="server" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlDamName_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Search">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label" style="padding-top:1%;">Entry Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButtonList ID="rdbtnEntryType" runat="server" RepeatDirection="Horizontal" CssClass="My-Radio">
                                            <asp:ListItem class="radio-inline" Selected="True" Value="1" Text="Channel" />
                                            <asp:ListItem class="radio-inline" Value="0" style="margin-left: 15px;" Text="Dam" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="5" runat="server" required="true" CssClass="required form-control date-picker" size="16" type="text" OnTextChanged="txtDate_TextChanged" AutoPostBack="true" ValidationGroup="Search"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                     

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" ValidationGroup="Search" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvChannelReading" runat="server"
                                    DataKeyNames="SmallChannelID,ID,ChannelCode,Name,Capacity,Gauge,Discharge,ReadingDate,FromTime,ToTime,ReaderName,Remarks,CreatedDate,CreatedBy,MaxGauge,MaxDischarge"
                                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                    EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                                    OnRowDataBound="gvChannelReading_RowDataBound" OnPageIndexChanged="gvChannelReading_PageIndexChanged" OnPageIndexChanging="gvChannelReading_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Channel ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelID" runat="server" Text='<%#Eval("ID")%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelCode" runat="server" CssClass="control-label" Text='<%# Eval("ChannelCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Capacity (Cusec)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelCapacity" runat="server" CssClass="control-label" Text='<%# Eval("Capacity","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" Width="7%" />
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gauge (ft)">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblGauge" runat="server" Text='<%# Eval("Gauge") %>' />--%>
                                                <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control integerInput" MaxLength="5" Style="max-width: 100%; display: inline;" Text='<%# Eval("Gauge","{0:0.##}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 " />
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discharge (Cusec)">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblDischarge" runat="server" Text='<%# Eval("Discharge") %>' />--%>
                                                <asp:TextBox ID="txtDischarge" runat="server" CssClass="form-control decimal2PInput" MaxLength="8" Style="max-width: 100%; display: inline;" Text='<%# Eval("Discharge","{0:0.00}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Running Time From">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("FromTime") %>' />--%>
                                                    <div style="float:left; width:90px;" class="input-group date" data-date-viewmode="years">
                                                        <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" class="form-control" size="16" type="text" placeholder="DD-MM-YYYY" Text='<%#Eval("FromTime","{0:dd-MMM-yyyy}") %>' Width="100%" onfocus="this.value = this.value;" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox Visible="false" ID="txtFromTime" runat="server" CssClass="form-control " MaxLength="5" Text='<%#Eval("FromTime","{0:t}") %>' placeholder="HH:MM" pattern="^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" oninput="javascript:ValidateTime(this)" Width="44%" autocomplete="off" />
                                                    </div>
                                                <div style="float:right">
                                                    <uc1:TimePicker Width="70px" ClockIcon="false" CrossIcon="false"  runat="server" ID="tpFrom" />
                                                    </div>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 text-left" />
                                            <ItemStyle CssClass="integerInput" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Running Time To">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblToTime" runat="server" Text='<%# Eval("ToTime") %>' />--%>
                                                <div>
                                                    <div class="input-group date" style="float:left; width:90px;" data-date-viewmode="years">
                                                        <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" class="form-control date-picker" size="16" type="text" placeholder="DD-MM-YYYY" Text='<%#Eval("ToTime","{0:dd-MMM-yyyy}") %>' Width="100%" onfocus="this.value = this.value;" ValidationGroup="Search" OnTextChanged="txtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:TextBox Visible="false" ID="txtToTime" runat="server" CssClass="form-control" MaxLength="5" Text='<%#Eval("ToTime","{0:t}") %>' placeholder="HH:MM" pattern="^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" Width="40%" oninput="javascript:ValidateTime(this)" ValidationGroup="Search" autocomplete="off" />
                                                    </div>
                                                    <div style="float:right">
                                                        <uc1:TimePicker Width="70px" ClockIcon="false" CrossIcon="false" runat="server" ID="tpTo" />
                                                    </div>
                                                </div>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 text-left" />
                                            <ItemStyle CssClass="integerInput" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gauge Reader">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblReaderName" runat="server" Text='<%# Eval("ReaderName") %>' />--%>
                                                <asp:TextBox ID="txtReaderName" runat="server" CssClass="form-control" MaxLength="75" Text='<%# Eval("ReaderName") %>' oninput="AlphabetValidation(this)" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2" width="13%" />

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />--%>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" MaxLength="500" Text='<%# Eval("Remarks") %>' oninput="InputValidation(this)" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 " />
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" Visible="false" ValidationGroup="Save" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    
    <script type="text/javascript">

        $(document).ready(function () {

            //On UpdatePanel Refresh
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitilizeDatePickerStateOnUpdatePanelRefresh();
                        InitilizeTimePickerOnUpdatePanelRefresh();
                        ClearDateField();
                    }
                });
            };
        });

        function ValidateTime(TxtTimeID) {
            var Time = TxtTimeID.value;

            if (Time.indexOf(':') != -1) {
                var SplittedTime = Time.split(':');

                if ($.isNumeric(SplittedTime[0]) && $.isNumeric(SplittedTime[1])) {
                    var Hours = parseInt(SplittedTime[0]);
                    var Minutes = parseInt(SplittedTime[1]);

                    if ((Hours >= 0 && Hours <= 23) && (Minutes >= 0 && Minutes <= 59)) {
                        TxtTimeID.setCustomValidity("");
                    }
                    else {
                        TxtTimeID.setCustomValidity("Time should be between 00:00 and 23:59");
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

        function ValueValidation(input, minValue, maxValue) {
            debugger;
            var str = input.value;
            str = str.trim();

            if (str.length != 0 && !(str === "")) {

                if (!(minValue.trim() === "") && !(maxValue.trim() === "")) {

                    var value = parseFloat(str);

                    if (value < parseFloat(minValue) || value > parseFloat(maxValue)) {
                        input.setCustomValidity("Value should be between " + minValue + " and " + maxValue);
                    }
                    else {
                        input.setCustomValidity("");
                    }
                }
                else {
                    input.setCustomValidity("");
                }
            }
            else {
                input.setCustomValidity("");
            }
        }

    </script>
</asp:Content>
