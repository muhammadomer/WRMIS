<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBreachIncident.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.AddBreachIncident" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Add Breach Incident</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="row" id="divisionId" runat="server">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label id="lbldivision" runat="server" class="col-sm-4 col-lg-3 control-label">Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control Hide" required="true" ID="ddlDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannel" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--           <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Select Channel</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannel" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label"> RD</asp:Label>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDLeft" autofocus="autofocus" runat="server" placeholder="RD" required="required" pattern="^(0|[0-9][0-9]*)$" MaxLength="3" CssClass="integerInput required form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        +
                                    </div>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="RD" required="required" pattern="^(0|[0-9][0-9]*)$" MaxLength="3" CssClass="integerInput required form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Side</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlSide" runat="server" AutoPostBack="true">
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
                                        <%--   <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                            <asp:TextBox ID="txtIncidentTime" TabIndex="5" runat="server" required="required" class="required form-control timepicker-default" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>--%>
                                        <uc1:TimePicker runat="server" ID="BreachTimePicker" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Head Discharge (cusec)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtHeadDischarge" type="text" MaxLength="99999" autocomplete="off" class="form-control decimalInput required" required="true" onkeyup="return ValidateValue(this, 99999);"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Length of Breaching Section (ft)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtLengthofbreach" type="text" MaxLength="1000" autocomplete="off" class="form-control decimalInput required" required="true" onkeyup="return ValidateValue(this, 1000);"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Field Staff available</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlFieldStaff" runat="server">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%--     <asp:FileUpload runat="server" ID="UploadImages" AllowMultiple="true" />
                                        <%--  <asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="uploadFile_Click"  /> 
                                        <asp:Label ID="listofuploadedfiles" runat="server" />--%>

                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemarks" ClientIDMode="Static" onblur="TrimInput(this);" runat="server" CssClass="form-control multiline-no-resize required" required="true" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                               <div class="col-md-6" style="visibility: hidden">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtRemark" ClientIDMode="Static" onblur="TrimInput(this);" runat="server" CssClass="form-control multiline-no-resize " TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn" style="margin-bottom: 0px;">
                                <asp:Button runat="server" ID="btnSaveBreachData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveBreachData_Click" />
                                <%--<asp:Button runat="server" ID="btnBack" value=" Back " CssClass="btn btn-primary" Text="&nbsp;Back" OnClick="btnBack_Click" />--%>
                                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/WaterTheft/SearchBreachIncident.aspx?RP=2" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- END Main Content -->
    <script>
        function ValidateValue(textboxID, maxvalue) {
            if ((textboxID.value >= 0 && textboxID.value <= maxvalue) && textboxID.value.fixedlength != 0) {
                return true;
            }
            else {

                alert('Value range is 0 to' + ' ' + maxvalue);
                textboxID.value = '';
                return false;
            }
        }

    </script>
</asp:Content>
