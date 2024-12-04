<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewSmallDamReadingSD.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SmallDams.SmallDamReading.ViewSmallDamReadingSD" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnDamID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnDate" runat="server" Value="0" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>View Daily Readings</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" runat="server" AutoPostBack="true" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged">
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
                                        <asp:DropDownList ID="ddlDamName" runat="server" AutoPostBack="true" CssClass="required  form-control" required="True" OnSelectedIndexChanged="ddlDamName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Channel" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="true" CssClass=" form-control" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="5" runat="server" CssClass="form-control date-picker required" required="true" size="16" type="text" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" CssClass="form-control date-picker required" required="true" size="16" type="text" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-success" Text="Print" OnClick="btnPrint_Click" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvChannelReading" runat="server"
                                    DataKeyNames="SmallChannelID,ID,Capacity,Gauge,Discharge,ReadingDate,FromTime,ToTime,ReaderName,Remarks,CreatedDate,CreatedBy"
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

                                        <asp:TemplateField HeaderText="Reading Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReadingDate" runat="server" CssClass="control-label" Text='<%# Eval("ReadingDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" width="7%"/>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Capacity(Cusec)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelCapacity" runat="server" CssClass="control-label" Text='<%# Eval("Capacity","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2" width="1%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gauge (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGauge" runat="server" Text='<%# Eval("Gauge") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 "  width="5%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discharge (Cusec)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDischarge" runat="server" Text='<%# Eval("Discharge") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-1 "  width="1%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Running Time From">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("FromTime") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 text-center" width="12%"/>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Running Time To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToTime" runat="server" Text='<%# Eval("ToTime") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 text-center" width="12%"/>
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gauge Reader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReaderName" runat="server" Text='<%# Eval("ReaderName") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-1 " width="11%"/>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 " width="11%"/>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="table-responsive">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvDamReadings" runat="server"
                                    DataKeyNames="SmallDamID,ID,ReadingDate,DamLevel,LiveStorage,Discharge,ReaderName,Remarks,CreatedDate,CreatedBy"
                                    CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                                    EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                                    OnRowDataBound="gvDamReadings_RowDataBound" OnPageIndexChanged="gvDamReadings_PageIndexChanged" OnPageIndexChanging="gvDamReadings_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Channel ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannelID" runat="server" Text='<%#Eval("ID")%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Reading Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReadingDate" runat="server" CssClass="control-label" Text='<%# Eval("ReadingDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1 text-center" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Level (ft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDamLevel" runat="server" CssClass="control-label" Text='<%# Eval("DamLevel","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" width="2%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Live Storage (Aft)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLiveStorage" runat="server" Text='<%# Eval("LiveStorage","{0:n0}") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" width="6%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discharge (cusec)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDischarge" runat="server" Text='<%# Eval("Discharge","{0:n0}") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-1 "  width="5%"/>
                                            <ItemStyle CssClass="integerInput text-right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gauge Reader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReaderName" runat="server" Text='<%# Eval("ReaderName") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-1 " />

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-md-2 " />
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>

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

    </script>

</asp:Content>
