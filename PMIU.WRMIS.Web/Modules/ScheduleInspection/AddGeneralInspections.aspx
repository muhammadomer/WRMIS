<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddGeneralInspections.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddGeneralInspections" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucScheduleDetail" Src="~/Modules/ScheduleInspection/Controls/ScheduleDetail.ascx" TagName="ScheduleDetail" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>General Inspection</h3>
                </div>
                <div class="box-content">
                    <div id="USerControl" class="form-horizontal" runat="server" visible="true">
                        <ucScheduleDetail:ScheduleDetail runat="server" ID="ScheduleDetail" />
                    </div>
                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Inspection Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%--<asp:TextBox ID="txtInspectionType" runat="server" CssClass="form-control required" required="required" Enabled="false"></asp:TextBox>--%>
                                        <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGeneralInspectionType" runat="server" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtInspectionDate" runat="server" CssClass="form-control disabled-future-date-picker required" required="required" type="text" OnTextChanged="Check_InspectionDates" AutoPostBack="true"></asp:TextBox>
                                                    <span id="spanInspectionDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="txtInspectionDate" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>

                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Location</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control required" required="required" Enabled="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Inspection Details</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtDetails" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize required" required="required" MaxLength="250"></asp:TextBox>
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
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="divSave">
                            <div class="col-md-12">
                                <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" CausesValidation="true" OnClientClick="return ValidateRequiredFields();" OnClick="btnSave_Click" />
                            </div>
                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnScheduleDetailID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnGID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnSource" Value="A" />
    <asp:HiddenField runat="server" ID="hdnScheduleID" Value="0" />
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerOnUpdatePanelRefresh();
                    ClearDateField();
                }
            });
        };
        function ValidateRequiredFields() {
            if (Page_ClientValidate()) {

                return confirm('Are you sure you want to save this record?');
            }
            return false;
        }
    </script>
</asp:Content>
