<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TawaanWorking.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.Controls.TawaanWorking" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>


<div class="form-horizontal">

    <h5>Tawaan Working</h5>
    <hr />



    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblDecisionType" runat="server" Text="Decision Type"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:DropDownList ID="ddlDecisiontype" ClientIDMode="Static" CssClass="form-control required" required="required" runat="server"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">Decision Date</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <div class="input-group date" data-date-viewmode="years">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtDecisionDate" ClientIDMode="Static" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                        <span id="spanDateID" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblSpecialCharges" class="col-sm-4 col-lg-3 control-label" runat="server" Text="Special Charges(Rs)"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox ID="txtSpecialCharges" ClientIDMode="Static" MaxLength="7" CssClass="integerInput form-control" onblur="TrimInput(this);" autocomplete="off" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label class="col-sm-4 col-lg-6 control-label" ID="lblAreaBooked" runat="server" Text="Area Booked"></asp:Label>
                    <div class="col-sm-8 col-lg-6 controls" style="padding-left:8px;">
                        <asp:TextBox ID="txtAreaBooked" ClientIDMode="Static" CssClass="integerInput form-control required" onblur="TrimInput(this);" autocomplete="off" required="required" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label class="col-sm-4 col-lg-6 control-label" ID="lblFine" runat="server" Text="Fine"></asp:Label>
                    <div class="col-sm-8 col-lg-6 controls" style="padding-right:0px;">
                        <asp:TextBox ID="txtFine" ClientIDMode="Static" CssClass="integerInput form-control required" onblur="TrimInput(this);" MaxLength="10" required="required" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblLetterSDOToPolice" runat="server" Text="Letter # from SDO to Police"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox ID="txtLetterSDOToPolice" ClientIDMode="Static" onblur="TrimInput(this);" CssClass="form-control required" MaxLength="12" required="required" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6" id="UploadLetter" runat="server">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">Upload Copy of Letter</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <uc1:FileUploadControl runat="server" ID="FileUploadControlCL" Size="5" />
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label ID="lblFirNo" class="col-sm-4 col-lg-3 control-label" runat="server" Text="FIR # "></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox ID="txtFirNo" ClientIDMode="Static" onblur="TrimInput(this);" CssClass="form-control required" required="required" MaxLength="12" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6" id="UploadFIR" runat="server">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">Upload Copy of FIR</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <uc1:FileUploadControl runat="server" ID="FileUploadControlCF" Name="FIRCtrl" Size="5" />
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">FIR Date</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <div class="input-group date" data-date-viewmode="years">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtFIRDate" ClientIDMode="Static" runat="server" onblur="TrimInput(this);" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                        <span id="spanFIRDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label class="col-sm-4 col-lg-6 control-label" ID="lblImprisonment" runat="server" Text="Imprisonment"></asp:Label>
                    <div class="col-sm-8 col-lg-6 controls" style="padding-left:8px;">
                        <asp:DropDownList CssClass="form-control" ID="ddlImprisonment" runat="server" ClientIDMode="Static"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label class="col-sm-4 col-lg-6 control-label" ID="lblDays" runat="server" Text="Days"></asp:Label>
                    <div class="col-sm-8 col-lg-6 controls" style="padding-right:0px;">
                        <asp:TextBox ID="txtDays" CssClass="integerInput form-control" onblur="TrimInput(this);" autocomplete="off" ClientIDMode="Static" MaxLength="5" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblCaseToXEN" runat="server" Text="Case submitted to XEN (Number)"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox CssClass="form-control required" onblur="TrimInput(this);" required="required" ID="txtCaseToXEN" ClientIDMode="Static" MaxLength="12" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">Case Submitted to XEN (Date)</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <div class="input-group date" data-date-viewmode="years">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtCaseToXENDate" ClientIDMode="Static" onblur="TrimInput(this);" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                        <span id="spanXENDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="divAppeal" runat="server" visible="false">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblAmountPaidForAppeal" runat="server" Text="Amount Paid for the Appeal (Rs)"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox CssClass="integerInput form-control required" onblur="TrimInput(this);" autocomplete="off"  required="true" ID="txtAmountPaidForAppeal" MaxLength="7" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">

                <asp:Label id="lblAttachmentProof" Text="Attachment of Proof" runat="server" class="col-sm-4 col-lg-3 control-label" ></asp:Label>
                <%--<label id="lblAttachmentProof" runat="server" class="col-sm-4 col-lg-3 control-label">Attachment of Proof</label>--%>
                <div class="col-sm-8 col-lg-9 controls">
                    <uc1:FileUploadControl runat="server" ID="FileUploadControlAppeal" Name="AttchCtrl"  Size="5"  />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblAddComments" runat="server" Text="Add Comments"></asp:Label>
                <div class="col-sm-8 col-lg-9 controls">
                    <asp:TextBox ID="taComments" ClientIDMode="Static" onblur="TrimInput(this);" runat="server" CssClass="form-control multiline-no-resize" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row" id="Attachment" runat="server">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                <div class="col-sm-8 col-lg-9 controls">
                    <uc1:FileUploadControl runat="server" ID="FileUploadControlProof" Name="DesnCtrl" Size="5" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAA" runat="server" ClientIDMode="Static"></asp:HiddenField>
     <asp:HiddenField ID="hdnDateOfChecking" runat="server" />
</div>

<script type="text/javascript">

    $(document).ready(function () {

        $("#ddlImprisonment").change(function () {
            var val = document.getElementById("ddlImprisonment").value
            if (val == 1)
                document.getElementById("txtDays").disabled = false;
            else
                document.getElementById("txtDays").disabled = true;
        });
    });

    
    $('#<%=txtFIRDate.ClientID%>').change(function () {
        ValidateWorkingDate(this);
    });

    $('#<%=txtCaseToXENDate.ClientID%>').change(function () {
        ValidateWorkingDate(this);
    });

    $('#<%=txtDecisionDate.ClientID%>').change(function () {
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

