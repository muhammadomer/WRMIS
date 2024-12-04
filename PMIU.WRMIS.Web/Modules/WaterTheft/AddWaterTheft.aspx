<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddWaterTheft.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.AddWaterTheft" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Water Theft Identification</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Offence Site</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlOffenceSite" runat="server" onChange="HideShowOffenceSite(this)" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlOffenceSite_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="DivisionDiv" class="hidden">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control Hide" required="true" ID="ddlDivision" runat="server" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- BEGIN Ouotlet Content -->
                        <div id="AddOutletIncidentDiv" class="hidden">
                            <asp:UpdatePanel ID="OutletIncident" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <h3>Outlet Incident</h3>
                                        <hr />
                                    </div>

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
                                                <asp:TextBox ID="txtIncidentDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
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
                                <div class="col-md-6" id="divTheftType">
                                    <div class="form-group">
                                        <label id="lblTheftType" runat="server" class="col-sm-4 col-lg-3 control-label">Theft Type</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlTheftType" runat="server" TabIndex="7">
                                                <%--OnSelectedIndexChanged="ddlOutletCondition_SelectedIndexChanged"--%>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Outlet Condition </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlOutletCondition" runat="server" TabIndex="8" onchange="EnableDisableTheftType(this)">
                                            </asp:DropDownList>
                                            <%--OnSelectedIndexChanged="ddlOutletCondition_SelectedIndexChanged" AutoPostBack="true"--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label" id="lblValueofH" runat="server">Value of H</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtHValue" type="text" autocomplete="off" class="form-control required decimalInput " required="true" TabIndex="9" MaxLength="4" onkeyup="return ValidateValue(this);"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label" id="lblDefectiveType" runat="server">Defective Type</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlDefectiveType" runat="server" TabIndex="10" onchange="ChangeRequiredAttributes(this)">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label decimalInput" id="lblValueofB" runat="server">Value of B</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtBValue" type="text" autocomplete="off" class="form-control decimalInput" TabIndex="11" MaxLength="4" onkeyup="return ValidateValue(this);"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label decimalInput" id="lblValueofY" runat="server">Value of Y</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtYValue" type="text" autocomplete="off" class="form-control decimalInput" TabIndex="12" MaxLength="4" onkeyup="return ValidateValue(this);"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label decimalInput" id="lblValueofDIA" runat="server">Value of DIA</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtDIAValue" type="text" autocomplete="off" class="form-control decimalInput" TabIndex="13" MaxLength="4" onkeyup="return ValidateValue(this);"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="txtRemarks" type="text" MaxLength="250" onblur="TrimInput(this);" TextMode="multiline" class="form-control required multiline-no-resize" required="true" Rows="5" TabIndex="14"> </asp:TextBox>
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
                                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn" style="margin-bottom: 0px;">
                                        <asp:Button runat="server" ID="btnSaveOutletIncidentData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveOutletIncidentData_Click" />
                                        <%--          <input type="submit" class="btn" value="Cancel" id="btnCancelOutletIncidentData">--%>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Modules/WaterTheft/AddWaterTheft.aspx" CssClass="btn">Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- End Ouotlet Content -->
                        <!-- BEGIN Channel Content -->
                        <div id="AddChannelIncidentDiv" class="hidden">
                            <div>
                                <h3>Channel Incident</h3>
                                <hr />
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannels" runat="server" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">RD (ft)</asp:Label>
                                                <div class="col-sm-3 col-lg-4 controls">
                                                    <asp:TextBox ID="txtOutletRDLeft" autofocus="autofocus" runat="server" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput LeftRDsMaxLength required form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-1 col-lg-1 controls">
                                                    +
                                                </div>
                                                <div class="col-sm-3 col-lg-4 controls">
                                                    <asp:TextBox ID="txtOutletRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" MaxLength="3" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput RightRDsMaxLength required form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Side</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannelSide" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Offence Type </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control required" required="true" ID="ddlOffenceType" runat="server" onchange="EnableDisableOffenceType(this)">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                            </div>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Checking Date</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtDateofChecking" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Checking Time </label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <%--         <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                                <asp:TextBox ID="txtTimeofChecking" TabIndex="5" runat="server" required="required" class="required form-control timepicker-default" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>--%>
                                            <uc1:TimePicker runat="server" ID="ChannelTimePicker" CssClass="form-control required" />
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">



                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Condition of Cut</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList name="ddlConditionofCut" CssClass="form-control required" required="true" ID="ddlCutCondition" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox runat="server" ID="ChannelRemarks" type="text" MaxLength="250" TextMode="multiline" class="form-control required  multiline-no-resize" required="true" Rows="5"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="5" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn" style="margin-bottom: 0px;">
                                        <asp:Button runat="server" ID="btnSaveChannelData" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveChannelData_Click" />
                                        <%--OnClick="btnSaveOutletIncidentData_Click"--%>
                                        <%--  <input type="submit" class="btn" value="Cancel" id="btnCancelChannelIncidentData">--%>
                                        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/WaterTheft/AddWaterTheft.aspx" CssClass="btn">Back</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- End Channel Content -->


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
    </div>

    <!-- END Main Content -->
    <script type="text/javascript" lang="javascript">

        var ddlTheftTypeID = $('#<%= ddlTheftType.ClientID %>');
        var txtType = $('#<%= txtType.ClientID %>');
        var txtRD = $('#<%= txtRDS.ClientID %>');
        var txtSide = $('#<%= txtSide.ClientID %>');
        var ddlDefectiveType = $('#<%= ddlDefectiveType.ClientID %>');
        var txtBValue = $('#<%= txtBValue.ClientID %>');
        var txtYValue = $('#<%= txtYValue.ClientID %>');
        var txtDIAValue = $('#<%= txtDIAValue.ClientID %>');
        var ddlChannelsID = $('#<%= ddlChannels.ClientID %>');
        var ddlChannelSideID = $('#<%= ddlChannelSide.ClientID %>');
        var txtDateofCheckingID = $('#<%= txtDateofChecking.ClientID %>');
      <%--  var txtTimeofCheckingID = $('#<%= txtTimeofChecking.ClientID %>');--%>
        var ddlOffenceTypeID = $('#<%= ddlOffenceType.ClientID %>');
        var ddlCutConditionID = $('#<%= ddlCutCondition.ClientID %>');
        var ChannelRemarksID = $('#<%= ChannelRemarks.ClientID %>');
        var ddlChannelID = $('#<%= ddlChannel.ClientID %>');
        var ddlOutletID = $('#<%= ddlOutlet.ClientID %>');
        var txtIncidentDate = $('#<%= txtIncidentDate.ClientID %>');
        var ddlOutletConditionID = $('#<%= ddlOutletCondition.ClientID %>');
        var ddlTheftTypes = $('#<%= ddlTheftType.ClientID %>');
        var txtOutletRDRightID = $('#<%= txtOutletRDRight.ClientID %>');
        var txtOutletRDLeftID = $('#<%= txtOutletRDLeft.ClientID %>');
        var ChannelRemark = $('#<%= ChannelRemarks.ClientID %>');
        var txtRemark = $('#<%= txtRemarks.ClientID %>');
        var txtHValueID = $('#<%= txtHValue.ClientID %>');

        $(document).ready(function () {
            var offenceSiteID = $("#OffenceSites").val();
            OffenceSite(offenceSiteID);
            var outletInfoID = $("#OutletInfoID").val();
            OutletInfo(outletInfoID);
            var deftypeid = $("#ConditionofCut").val();
            var ddlCutId = $("select[name='ddlConditionofCut'] option:selected").index();
            EnableDisableControlbyID(deftypeid, offenceSiteID);
            if (ddlCutId == -1) {
                DisableDesignParameter(ddlCutConditionID);
            }
            hide($('#ddlDivision'));

            var outlletcontition = $("#DefectiveType").val();
            console.log(outlletcontition);
            // var ddloutCondition = $("select[id$='ddlDefectiveType'] option:selected").index();
            if (outlletcontition == TheftSiteCondition.Defective) {
                ddlTheftTypes.get(0).selectedIndex = 0;
                DisableDesignParameter(ddlTheftTypes);
            }
            else if (outlletcontition == TheftSiteCondition.Tempered || outlletcontition == TheftSiteCondition.Repaired || outlletcontition == TheftSiteCondition.OK) {
                ddlDefectiveType.get(0).selectedIndex = 0;
                EnableDesignParameter(ddlTheftTypeID);
                DisableDesignParameter(ddlDefectiveType);
                DisableDesignParameter(txtBValue);
                DisableDesignParameter(txtYValue);
                DisableDesignParameter(txtDIAValue);
                AddRequiredAttribute(ddlTheftTypes);
                txtBValue.val('');
                txtYValue.val('');
                txtDIAValue.val('');
            }


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

        function OffenceSite(offenceSiteID) {
            if (offenceSiteID == 'C') {
                hide($('#AddOutletIncidentDiv'));
                Show($('#AddChannelIncidentDiv'));
                Show($('#DivisionDiv'));
                AddRequiredAttribute(ddlChannelsID);
                AddRequiredAttribute(ddlChannelSideID);
                AddRequiredAttribute(txtDateofCheckingID);
                AddRequiredAttribute(ddlOffenceTypeID);
                AddRequiredAttribute(ddlCutConditionID);
                AddRequiredAttribute(txtOutletRDLeftID);
                AddRequiredAttribute(txtOutletRDRightID);
                AddRequiredAttribute(ChannelRemark);
                RemoveRequiredAttribute(ddlChannelID);
                RemoveRequiredAttribute(ddlOutletID);
                RemoveRequiredAttribute(txtIncidentDate);
                RemoveRequiredAttribute(ddlTheftTypes);
                RemoveRequiredAttribute(ddlOutletConditionID);
                RemoveRequiredAttribute(txtRemark);
                RemoveRequiredAttribute(txtHValueID);
                RemoveRequiredAttribute(ddlDefectiveType);
                DisableDesignParameter(ddlCutConditionID);
                hide($('#ddlDivision'));
                $("#AddChannelIncidentDiv").blur();
            }
            else if (offenceSiteID == 'O') {
                Show($('#AddOutletIncidentDiv'));
                hide($('#AddChannelIncidentDiv'));
                Show($('#DivisionDiv'));
                RemoveRequiredAttribute(ddlChannelsID);
                RemoveRequiredAttribute(ddlChannelSideID);
                RemoveRequiredAttribute(txtDateofCheckingID);
                RemoveRequiredAttribute(ddlOffenceTypeID);
                RemoveRequiredAttribute(ddlCutConditionID);
                RemoveRequiredAttribute(txtOutletRDLeftID);
                RemoveRequiredAttribute(txtOutletRDRightID);
                RemoveRequiredAttribute(ChannelRemark);
                AddRequiredAttribute(ddlChannelID);
                AddRequiredAttribute(ddlOutletID);
                AddRequiredAttribute(txtIncidentDate);
                AddRequiredAttribute(ddlOutletConditionID);
                AddRequiredAttribute(txtRemark);
                AddRequiredAttribute(txtHValueID);
                AddRequiredAttribute(ddlDefectiveType);
                AddRequiredAttribute(ddlTheftTypes);
                Show($('#ddlDivision'));
                $("#AddOutletIncidentDiv").blur();
                EnableDisableDefectiveTypebyID(OffenceType.Others);
                ChangeRequiredAttributeOnControl(OffenceType.Others);
            }
            else {
                hide($('#AddChannelIncidentDiv'));
                hide($('#AddOutletIncidentDiv'));
                hide($('#ddlDivision'));
            }
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


        function EnableDisableControlbyID(DefectiveTypeID, offenceSiteID) {
            if (offenceSiteID == 'O') {
                AddRequiredAttribute(ddlTheftTypes)
            }
            else {
                RemoveRequiredAttribute(ddlTheftTypes);
                if (DefectiveTypeID == OffenceType.Cut) {
                    EnableDesignParameter(ddlCutConditionID);
                }
                else if (DefectiveTypeID != OffenceType.Cut) {
                    DisableDesignParameter(ddlCutConditionID);
                    ddlCutConditionID.get(0).selectedIndex = 0;
                }
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

        function ChangeRequiredAttributes(DefectiveTypeID) {
            var Id = $(DefectiveTypeID).val();
            $("#DefectiveTypes").val(Id);
            ChangeRequiredAttributeOnControl(Id);
        }
        function EnableDisableTheftType(ConditionID) {

            var Id = $(ConditionID).val();
            $("#DefectiveType").val(Id);
            EnableDisableDefectiveTypebyID(Id);
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
        function EnableDisableOffenceType(ConditionID) {
            var Id = $(ConditionID).val();
            $("#ConditionofCut").val(Id);
            var offenceSiteID = $("#OffenceSites").val();
            EnableDisableControlbyID(Id, offenceSiteID);
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

