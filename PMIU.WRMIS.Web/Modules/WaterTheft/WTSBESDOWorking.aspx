<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WTSBESDOWorking.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.WTSBESDOWorking" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3><span runat="server" id="pageTitleID"></span></h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnCanalWireID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDateOfChecking" runat="server" />
            <asp:PlaceHolder runat="server" ID="PhWaterTheftIncidentInformation"></asp:PlaceHolder>
            <h3 id="hWorkingTitle" runat="server">SBE Working</h3>
            <div class="form-horizontal" id="WorkingFieldsID" runat="server">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCanalWireNo" ID="lblCanalWireNo" Text="Canal Wire #" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtCanalWireNo" ClientIDMode="Static" autofocus="autofocus" onblur="TrimInput(this);" runat="server" required="required" MaxLength="12" CssClass="required form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div id="DivCanalWireDate" runat="server" class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtCanalWireDate" ID="lblCanalWireDate" Text="Canal Wire Date" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtCanalWireDate" ClientIDMode="Static" runat="server" onblur="TrimInput(this);" required="required" CssClass="required form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon clear disabled" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div id="DivClosingRepairDate" runat="server" class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtClosingRepairDate" ID="lblClosingRepairDate" Text="Date of Closing/Repair" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtClosingRepairDate" ClientIDMode="Static" onblur="TrimInput(this);" runat="server" CssClass="form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtComments" ID="lblComments" Text="Add Comments" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtComments" runat="server" onblur="TrimInput(this);" ClientIDMode="Static" CssClass="commentsMaxLengthRow form-control multiline-no-resize" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="lblAttachments" ID="lblAttachments" Text="Attachments" runat="server" CssClass="col-sm-4 col-lg-3 control-label"></asp:Label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:Button runat="server" OnClick="btnAssignToSDO_Click" ID="btnAssignToSDO" CssClass="btn btn-primary" ClientIDMode="Static" OnClientClick="RemoveRequired()" Text="&nbsp;Assign To SDO" />
                        <asp:PlaceHolder runat="server" ID="pnlIsSDOWorking" Visible="false">
                            <asp:Button runat="server" ID="btnMarkCaseNA" OnClick="btnMarkCaseNA_Click" OnClientClick="return ValidateComments();" ClientIDMode="Static" CssClass="btn btn-primary" Text="&nbsp;Mark Case as N/A" />&nbsp;
                            <asp:Button runat="server" ID="btnAssignToSBE" OnClick="btnAssignToSBE_Click" OnClientClick="return ValidateComments();" CssClass="btn btn-primary" Text="&nbsp;Assign Back to SBE" />&nbsp;
                            <asp:Button runat="server" ID="btnAssignToZiladar" OnClick="btnAssignToZiladar_Click" CssClass="btn btn-primary" ClientIDMode="Static" OnClientClick="RemoveRequired()" Text="&nbsp;Assign to Ziladar" />&nbsp;
                             <%--OnClientClick="return AddWorkingFieldsValidation();"--%>
                        </asp:PlaceHolder>
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/ecmascript" src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        function ValidateComments() {
            debugger;
            $('.CtrlClass0').removeAttr("required");
            var comments = document.getElementById("txtComments");
            comments.value = comments.value.trim();
            if (comments) {
                RemoveWorkingFieldsValidation();
                SetRequiredAttribute(comments);
                comments.setAttribute("Class", "form-control multiline-no-resize required");
                comments.get(0).setCustomValidity('ok');
                return false;
            }
            else
                return true;
        }
        function SetRequiredAttribute(controlID) {
            controlID.setAttribute("required", "required");
        }
        function RemoveRequiredAttribute(controlID) {
            controlID.removeAttribute("required");
            controlID.setAttribute("Class", "form-control");
        }
        function AddRequiredAttribute(controlID) {
            SetRequiredAttribute(controlID);
            controlID.setAttribute("Class", "required form-control");
        }
        function RemoveWorkingFieldsValidation() {
            RemoveRequiredAttribute(document.getElementById("txtCanalWireNo"));
            RemoveRequiredAttribute(document.getElementById("txtCanalWireDate"));
            for (var i = 0; i < 5; i++) {
                $('.CtrlClass' + i).blur();
                $('.CtrlClass' + i).removeAttr('required');
            }
        }
        //function AddWorkingFieldsValidation() {
        //    AddRequiredAttribute(document.getElementById("txtCanalWireNo"));
        //    AddRequiredAttribute(document.getElementById("txtCanalWireDate"));

        //    console.log($('input[type=file]').val());

        //    $('.CtrlClass' + 0).blur();
        //    $('.CtrlClass' + 0).attr("required", "required");

        //    var comments = document.getElementById("txtComments");
        //    comments.removeAttribute("required");
        //    comments.setAttribute("Class", "form-control multiline-no-resize");
        //    return true;
        //}

        function RemoveRequired() {           
            $('.CtrlClass0').removeAttr("required");
        }

        $('#<%=txtCanalWireDate.ClientID%>').change(function () {
            ValidateWorkingDate(this);
        });
        $('#<%=txtClosingRepairDate.ClientID%>').change(function () {
            ValidateWorkingDate(this);
        });
        function ValidateWorkingDate(datePickerDate) {
            dpg = $.fn.datepicker.DPGlobal;
            date_format = 'dd-MM-yyyy';
            console.log($('#<%=hdnDateOfChecking.ClientID%>').val());
            console.log(dpg.parseDate(dpg.parseDate($('#<%=hdnDateOfChecking.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en')));
            console.log(dpg.parseDate(datePickerDate.value, dpg.parseFormat(date_format), 'en'));

            var workingDate = dpg.parseDate(datePickerDate.value, dpg.parseFormat(date_format), 'en');
            var lastWorkingDate = dpg.parseDate($('#<%=hdnDateOfChecking.ClientID%>').val(), dpg.parseFormat("MM-dd-yyyy"), 'en');
            if (lastWorkingDate > workingDate) {
                $('#lblMsgs').addClass('ErrorMsg').show();
                $('#lblMsgs').html("Date should be greater then " + GetFormatedDate(lastWorkingDate));
                setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                datePickerDate.value = "";
            }
        }
    </script>
</asp:Content>
