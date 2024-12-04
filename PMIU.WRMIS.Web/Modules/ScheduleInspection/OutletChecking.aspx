<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutletChecking.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.OutletChecking" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Outlet Checking</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <!-- BEGIN Ouotlet Content -->
                        <div id="AddOutletIncidentDiv">
                            <asp:UpdatePanel ID="OutletIncident" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannel" runat="server" TabIndex="3" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Outlet</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control required" required="true" ID="ddlOutlet" runat="server" TabIndex="4" OnSelectedIndexChanged="ddlOutlet_SelectedIndexChanged" AutoPostBack="true" OnChange="ShowHideOutletInfo(this)">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="OutletInfos" runat="server">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label runat="server" id="lblType" class="col-sm-4 col-lg-3 control-label">Type</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox runat="server" ID="txtType" type="text" class="form-control" ReadOnly> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label runat="server" id="lblRD" class="col-sm-4 col-lg-3 control-label">RD</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox runat="server" ID="txtRDS" type="text" class="form-control" ReadOnly> </asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label runat="server" id="lblSide" class="col-sm-4 col-lg-3 control-label">Side</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox runat="server" ID="txtSide" type="text" class="form-control" ReadOnly> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Checking Date</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtIncidentDate" TabIndex="5" runat="server" Enabled="false" required="required" class="required form-control" size="16" type="text"></asp:TextBox>
                                                <%--<span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Checking Time </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <%--    <div class="input-group date" data-date-viewmode="years">
                                                        <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                                        <asp:TextBox ID="txtIncidentTime" TabIndex="5" runat="server" required="required" class="required form-control timepicker-default" type="text"></asp:TextBox>
                                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                    </div>--%>
                                            <uc1:TimePicker runat="server" ID="TimePicker" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Outlet Condition </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlOutletCondition" runat="server" TabIndex="8" onchange="EnableDisableTheftType(this)">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label" id="lblValueofH" runat="server">Value of H</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtHValue" type="text" autocomplete="off" class="form-control  decimalInput "  TabIndex="9" MaxLength="4" onkeyup="return ValidateValue(this);"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtRemarks"  type="text" MaxLength="250"  onblur="TrimInput(this);"  Style="resize:none;" TextMode="multiline" class="form-control required multiline-no-resize" required="true" Rows="5" Columns="83" TabIndex="14"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6 ">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                                    <asp:HyperLink runat="server"  ID="fileAttachment" Visible="false"></asp:HyperLink>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn" style="margin-bottom: 0px;">
                                        <asp:Button runat="server" ID="btnSaveOutletIncidentData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveOutletCheckingData_Click" />
                                        <%--          <input type="submit" class="btn" value="Cancel" id="btnCancelOutletIncidentData">--%>
                                        
                                        <asp:HyperLink ID="hlBack" runat="server" onclick="history.go(-1);return false;" CssClass="btn">Back</asp:HyperLink>
                                        <asp:LinkButton ID="lbtnBackToET" runat="server" Visible ="false" Text="Back" class="btn" PostBackUrl="~/Modules/DailyData/MeterReadingAndFuel.aspx?ShowSearched=true"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- End Ouotlet Content -->
                </div>
                <!-- END Main Content -->
            </div>
            <asp:HiddenField ID="OffenceSites" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="OutletInfoID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="DefectiveType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="DefectiveTypes" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="ConditionofCut" runat="server" ClientIDMode="Static" />
        </div>
    </div>


    <!-- END Main Content -->
    <script type="text/javascript" lang="javascript">


        var txtType = $('#<%= txtType.ClientID %>');
        var txtRD = $('#<%= txtRDS.ClientID %>');
        var txtSide = $('#<%= txtSide.ClientID %>');


        var ddlChannelID = $('#<%= ddlChannel.ClientID %>');
        var txtHValueID = $('#<%= txtHValue.ClientID %>');
        var ddlOutletID = $('#<%= ddlOutlet.ClientID %>');
        var txtIncidentDate = $('#<%= txtIncidentDate.ClientID %>');
        var ddlOutletConditionID = $('#<%= ddlOutletCondition.ClientID %>');
        var txtHValueID = $('#<%= txtHValue.ClientID %>');
        $(document).ready(function () {
            //var offenceSiteID = $("#OffenceSites").val();
            //OffenceSite(offenceSiteID);
            //var outletInfoID = $("#OutletInfoID").val();
            //OutletInfo(outletInfoID);
            //var deftypeid = $("#ConditionofCut").val();
            //var ddlCutId = $("select[name='ddlConditionofCut'] option:selected").index();
            //EnableDisableControlbyID(deftypeid, offenceSiteID);
            //if (ddlCutId == -1) {
            //    DisableDesignParameter(ddlCutConditionID);
            //}
            //hide($('#ddlDivision'));

            //var outlletcontition = $("#DefectiveType").val();
            //console.log(outlletcontition);
            //if (outlletcontition == TheftSiteCondition.Defective) {
            //    ddlTheftTypes.get(0).selectedIndex = 0;
            //    DisableDesignParameter(ddlTheftTypes);
            //}
            //else if (outlletcontition == TheftSiteCondition.Tempered || outlletcontition == TheftSiteCondition.Repaired || outlletcontition == TheftSiteCondition.OK) {
            //    ddlDefectiveType.get(0).selectedIndex = 0;
            //    EnableDesignParameter(ddlTheftTypeID);
            //    DisableDesignParameter(ddlDefectiveType);
            //    DisableDesignParameter(txtBValue);
            //    DisableDesignParameter(txtYValue);
            //    DisableDesignParameter(txtDIAValue);
            //    AddRequiredAttribute(ddlTheftTypes);
            //    txtBValue.val('');
            //    txtYValue.val('');
            //    txtDIAValue.val('');
            //}


        });

        var TheftSiteCondition = {
            APM: 'APM',
            FreshlyClosedCut: '1',
            RunningCut: '2',
            Tempered: '3',
            Repaired: '4',
            Defective: '5',
            OK: '6'
        };

        var OffenceType = {

            CrestTempered: '1',
            Gurlu: '2',
            RoofBlockTempered: '3',
            SocketRemoved: '4',
            MachineRemoved: '5',
            Daff: '6',
            Pipe: '7',
            Takki: '8',
            Others: '9',
            Takki: '10',
            Daff: '11',
            Pipe: '12',
            Cut: '13'
        };

        var OutletDefType = {
            DuetohighHasDOH: '1',
            DuetohighBasDOB: '2',
            DuetohighYasDOY: '3',
            DuetohighDiaasDOD: '4',
            DuetolowHasDOH: '5',
            DuetolowBasDOB: '6',
            DuetolowYasDOY: '7',
            DuetolowDiaasDOD: '8'
        }
        var OutletConditionType = {
            OK: '2',
            WaterTheft: '1'
        }

   
        function OutletInfo(outletinfo) {

            if (outletinfo == -1 || outletinfo < 0 || outletinfo == "") {
                hide($('#OutletInfo'));
            }
            else {
                Show($('#OutletInfo'));
            }
        }
        function ShowHideOutletInfo(OutletID) {
            var outletInfo = $(OutletID).val();
            $("#OutletInfoID").val(outletInfo);
            OutletInfo(outletInfo);
        }
        function hide(ControlID) { ControlID.addClass("hidden"); }
        function Show(ControlID) { ControlID.removeClass("hidden"); }
        function HideShowOffenceSite(OffenceID) {
            var value = $(OffenceID).val();
            $("#OffenceSites").val(value);
            OffenceSite(value);

        }
        function EnableDisableDefectiveTypebyID(DefectiveTypeID) {

            if (DefectiveTypeID == TheftSiteCondition.Defective) {
                DisableDesignParameter(ddlTheftTypes);
                ddlTheftTypes.get(0).selectedIndex = 0;
                EnableDesignParameter(ddlDefectiveType);
                EnableDesignParameter(txtBValue);
                EnableDesignParameter(txtYValue);
                EnableDesignParameter(txtDIAValue);

            }

            else {
                ddlDefectiveType.get(0).selectedIndex = 0;
                EnableDesignParameter(ddlTheftTypeID);
                DisableDesignParameter(ddlDefectiveType);
                DisableDesignParameter(txtBValue);
                DisableDesignParameter(txtYValue);
                DisableDesignParameter(txtDIAValue);
                //AddRequiredAttribute(ddlTheftTypes);
                txtBValue.val('');
                txtYValue.val('');
                txtDIAValue.val('');

            }
        }
        function ChangeRequiredAttributeOnControl(DefectiveTypeID) {

            if (DefectiveTypeID == OutletDefType.DuetohighBasDOB || DefectiveTypeID == OutletDefType.DuetolowBasDOB) {

                AddRequiredAttribute(txtBValue)
                RemoveRequiredAttribute(txtYValue)
                RemoveRequiredAttribute(txtDIAValue)
            }
            else if (DefectiveTypeID == OutletDefType.DuetohighYasDOY || DefectiveTypeID == OutletDefType.DuetolowYasDOY) {
                AddRequiredAttribute(txtYValue)
                RemoveRequiredAttribute(txtBValue)
                RemoveRequiredAttribute(txtDIAValue)
            }
            else if (DefectiveTypeID == OutletDefType.DuetohighDiaasDOD || DefectiveTypeID == OutletDefType.DuetolowDiaasDOD) {
                AddRequiredAttribute(txtDIAValue)
                RemoveRequiredAttribute(txtBValue)
                RemoveRequiredAttribute(txtYValue)
            }
            else {
                RemoveRequiredAttribute(txtYValue)
                RemoveRequiredAttribute(txtDIAValue)
                RemoveRequiredAttribute(txtBValue)
            }
        }
        function EnableDisableHValue(OutletConditionID) {

            if (OutletConditionID == OutletConditionType.WaterTheft) {
                DisableDesignParameter(txtHValueID);
                txtHValueID.val('');
            }
            else {
                
                EnableDesignParameter(txtHValueID);
            }
        }
        function ChangeRequiredAttributes(DefectiveTypeID) {
            var Id = $(DefectiveTypeID).val();
            $("#DefectiveTypes").val(Id);
            ChangeRequiredAttributeOnControl(Id);
        }
        function EnableDisableTheftType(ConditionID) {

            var Id = $(ConditionID).val();
            EnableDisableHValue(Id);
        }
        function EnableDesignParameter(fieldID) {
            fieldID.removeAttr("disabled").attr("required", "required").addClass("required");
        }
        function DisableDesignParameter(fieldID) {
            fieldID.attr('disabled', 'disabled').removeAttr("required").removeClass("required");

        }
        function RemoveRequiredAttribute(fieldID) {
            fieldID.removeAttr("required").removeClass("required");
        }
        function AddRequiredAttribute(fieldID) {
            fieldID.attr("required", "required").addClass("required");
        }
        function RemoveRequiredatViewTimeAndDisableDateCrossIcone() {
            debugger;
            $(function () {
                //$('.input-group-addon').css('visibility', 'hidden');
                $('select').removeClass('required');
                //$(txtIncidentDate).width(569);
                //$(txtIncidentDate).addClass('form-control');
            });
        }
        function RemoveRequiredatViewTimeAndDisableDateCrossIcone() {
            debugger;
            $(function () {
                $('select').removeClass('required');
            });
        }
        function RemoveRequiredAtAddTime() {
            debugger;
            $(function () {
                $(ddlChannelID).removeClass('required');
                $(ddlOutletID).removeClass('required');
            });
        }
        function ValidateValue(textboxID) {
            if ((textboxID.value >= 0 && textboxID.value <= 10) && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {
                alert('Value range is 0 to' + ' ' + 10);
                textboxID.value = '';
                return false;
            }
        }
    </script>
</asp:Content>
