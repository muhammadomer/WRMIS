<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOutletAlterationInspection.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddOutletAlterationInspection" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ScheduleInspection/Controls/OutletInspectionNotes.ascx" TagPrefix="uc1" TagName="OutletAlterationInspection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Add Inspection Notes - Outlet Alteration</h3>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOutletTypeID" runat="server" />
            <asp:HiddenField ID="hdnScheduleDetailChannelID" runat="server" Value="0" />

            <uc1:OutletAlterationInspection runat="server" ID="OutletAlterationInspection" />

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
                            <asp:Label AssociatedControlID="txtOutletHeight" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Height of Outlet/Orifice (Y in ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtOutletHeight" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Height of Outlet/Orifice" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtWorkingHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Working Head (ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtWorkingHead" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Working Head" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- END Left Side -->
                    </div>
                    <div class="col-md-6 ">
                        <!-- BEGIN Right Side -->
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtOutletCrestHead" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Head above Crest of Outlet (H in ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtOutletCrestHead" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Head above Crest of Outlet" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtDBWidth" runat="server" CssClass="col-xs-4 col-lg-3 control-label">Diameter/Breadth/ Width (Dia/B in ft)</asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDBWidth" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" placeholder="Diameter/Breadth/ Width" required="required" CssClass="decimal2PInput required form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- END Right Side -->
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label for="select" class="col-sm-4 col-lg-3 control-label">Discharge (Cusec)</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDischarge" runat="server" oninput="ValidateIntergerInputRange(this,'0','10')" onblur="CalculateEfficiency();" placeholder="Discharge" required="true" CssClass="decimal2PInput form-control required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
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
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script type="text/javascript">
        <%--        $('#<%=txtInspectionDate.ClientID%>').change(function () {
            dpg = $.fn.datepicker.DPGlobal;
            date_format = 'dd-MM-yyyy';
            console.log($('#<%=hdnAlterationDate.ClientID%>').val());
            console.log(dpg.parseDate(dpg.parseDate($('#<%=hdnAlterationDate.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en')));
            console.log(dpg.parseDate($('#<%=txtInspectionDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en'));

            var alterationDate = dpg.parseDate($('#<%=txtInspectionDate.ClientID%>').val(), dpg.parseFormat(date_format), 'en');
            var lastAlterationDate = dpg.parseDate($('#<%=hdnAlterationDate.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en');
            if (lastAlterationDate >= alterationDate) {
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Alteration date should be greater then " + GetFormatedDate(lastAlterationDate));
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                $('#<%=txtInspectionDate.ClientID%>').val("");
            }
        });--%>
    </script>
</asp:Content>
