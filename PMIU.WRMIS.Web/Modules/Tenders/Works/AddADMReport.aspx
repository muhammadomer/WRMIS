<%@ Page Title="AddADMReport" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddADMReport.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.AddADMReport" %>

<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="uc1" TagName="ViewWorks" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <style type="text/css">

   .padding-right-number {
            padding-right:35px !important;
   }
    
    @media only screen and (min-width: 1300px) {
       .padding-right-number {
            padding-right:45px !important;
   }
    }
    @media only screen and (min-width: 1400px) {
        .gridReachStartingRDs {
            width: 15%;
        }
    }
    @media only screen and (min-width: 1500px) {
        .gridReachStartingRDs {
            width: 14%;
        }
    }
    @media only screen and (min-width: 1400px) {
        .padding-right-number {
            padding-right:75px !important;
   }
    }
</style>
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">ADM Report</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <uc1:ViewWorks runat="server" ID="ViewWorks" />
                </div>

                <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li id="liCommittee" style="width: 20%; text-align: center" runat="server"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Committee Attendance</a></li>
                            <li id="liContractor" style="width: 20%; text-align: center" runat="server"><a id="anchContractor" runat="server" aria-controls="ContractorsAttendance" role="tab">Contractors Attendance</a></li>
                            <li id="liPrice" runat="server" style="width: 20%; text-align: center"><a id="anchTenderPrice" runat="server" aria-controls="TenderPrice" role="tab">Tender Price</a></li>
                            <li id="liReport" runat="server" style="width: 20%; text-align: center" class="active"><a id="anchReport" runat="server" aria-controls="ADMReport" role="tab">ADM Report</a></li>
                            <li id="liStatement" runat="server" style="width: 20%; text-align: center"><a id="anchstatement" runat="server" aria-controls="ComparativeStatement" role="tab">Comparative Statement</a></li>

                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>


                            <div class="row">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="lblSubmissionTime" class="col-md-12 control-label" style="text-align:left"><strong>Submission Time</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:Label ID="lblSubmissionTime" runat="server" Text="11:30 pm"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="textfield1" class="col-md-12 control-label" style="text-align:left"><strong>Actual Submission Time</strong></label>
                                        <div class="col-md-12 controls">
                                            <div class="col-md-2" style="margin-top: -10px;">
                                                <label class="checkbox-inline" style="padding-left: 5px;">
                                                    <asp:CheckBox CssClass="checkbox" ID="chkBoxSameS" runat="server" Text="Same" OnCheckedChanged="chkBoxSameS_CheckedChanged" AutoPostBack="true" />
                                                </label>
                                            </div>
                                            <div class="col-md-10">
                                                <uc1:TimePicker runat="server" ID="TimePickerSubmission" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtAlternateSubmissionReason" class="col-md-1 control-label"><strong>Reason</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:TextBox ID="txtAlternateSubmissionReason" runat="server" CssClass="form-control required" Enabled="true" required="requied"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="lblOpeningTime" class="col-md-12 control-label" style="text-align:left"><strong>Opening Time</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:Label ID="lblOpeningTime" runat="server" Text="11:30 pm"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="textfield1" class="col-md-12 control-label" style="text-align:left" ><strong>Actual Opening Time</strong></label>
                                        <div class="col-md-12 controls">
                                            <div class="col-md-2" style="margin-top: -10px;">
                                                <label class="checkbox-inline" style="padding-left: 5px;">
                                                    <asp:CheckBox CssClass="checkbox" ID="CheckBoxA" runat="server" Text="Same" OnCheckedChanged="chkBoxSameA_CheckedChanged" AutoPostBack="true" />
                                                </label>
                                            </div>
                                            <div class="col-md-10">
                                                <uc1:TimePicker runat="server" ID="TimePickerOpening" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtAlternateOpeningReason" class="col-md-1 control-label"><strong>Reason</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:TextBox ID="txtAlternateOpeningReason" runat="server" CssClass="form-control required" Enabled="true" required="requied"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label for="lblSoldTenderCount" class="col-md-12 control-label" style="padding-right: 40px; text-align:left"><strong>Sold Count</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:Label ID="lblSoldTenderCount" runat="server" Text="6"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="lblTenderSubmittedCount" class="col-md-12 control-label" style="padding-right: 91px; text-align:left"><strong>Submitted Count</strong></label>
                                        <div class="col-md-12 controls">
                                            <asp:Label ID="lblTenderSubmittedCount" runat="server" Text="6"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>



                    <h4>List of Contractors</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvRejectedTenders" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    DataKeyNames="WorkContractorID,IsRejected,RejectedReason" OnRowDataBound="gvRejectedTenders_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("WorkContractorID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Contractor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContractorName" runat="server" CssClass="control-label" Text='<%# Eval("ContractorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Earnest Money (Rs)">
                                            <%--<HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEarnestMoney" runat="server" CssClass="control-label" Text='<%# Eval("EarnestMoney","{0:n0}") %>'></asp:Label>
                                              </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-2 text-right" />
                                                <ItemStyle CssClass="text-right padding-right-number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Call Deposit Amount (Rs)">
                                            <%--<HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeposit" runat="server" CssClass="control-label" Text='<%# Eval("Deposit","{0:n0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-3 text-right padding-right-number" />
                                                <ItemStyle CssClass="text-right padding-right-number" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <%--        <HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtRejectedReason" type="text" class="form-control" Text='<%# Eval("RejectedReason") %>' ClientIDMode="Static"> </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rejected">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRejected" runat="server" Checked='<%# bool.Parse(Eval("IsRejected").ToString()) %>' ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>


                        </div>
                    </div>


                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <h4>Observations</h4>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label class="col-sm-2 col-lg-1 control-label"></label>--%>
                                        <div class="col-sm-5 col-lg-6 controls">
                                            <%--<asp:TextBox ID="txtObservations" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize required" TabIndex="6" TextMode="MultiLine" Rows="5" MaxLength="250" required="required"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtObservations" runat="server" CssClass="form-control required" Enabled="true" required="requied"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxtenderEmpty" runat="server" Text="Tender Box was empty prior to cast the tenders." ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxRate" runat="server" Text="All the rates were noted at the time of opening." ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxTenderingProcessmembers" runat="server" Text="All the members were present in the tendering process." ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxSnaps" runat="server" Text="Snaps/Videos were taken and phptography of all the bis was controlled." ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxXEN" runat="server" Text="XEN was requested to send the comparative statement as early as possible." ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-3 col-lg-2 control-label">Upload received Tenders</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <uc1:FileUploadControl runat="server" ID="FileUpload1" Size="1" AllowMultiple="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-3 col-lg-2 control-label">Upload snap/video</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <uc1:FileUploadControl runat="server" ID="FileUpload2" Size="1" AllowMultiple="true" />
                                            <%--<asp:FileUpload ID="fileImages" AllowMultiple="true" runat="server" />--%>
                                        </div>

                                    </div>

                                </div>
                            </div>
                            <div class="" id="HyperLinksDiv" runat="server" visible="false">

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <h4>Attachments</h4>
                                        <table id="tblHyperlinks" runat="server">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">

                                        <div class="col-sm-6 col-lg-7 controls" style="padding-left: 35px;">
                                            <asp:CheckBox CssClass="checkbox" ID="ChkBoxCancel" runat="server" Text="Cancel Tender" ClientIDMode="Static" OnCheckedChanged="ChkBoxCancel_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label class="col-sm-2 col-lg-1 control-label">Reason</label>
                                        <div class="col-sm-5 col-lg-6 controls">
                                            <asp:TextBox ID="txtTenderCancelReason" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="6" TextMode="MultiLine" Rows="5" MaxLength="250" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row" runat="server" id="divSave">
                        <div class="col-md-12">
                            <asp:Button class="btn btn-primary" ID="btnSave"   Text="Save" runat="server" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            <asp:HyperLink ID="hlBackToWork" runat="server" CssClass="btn">&nbsp;Back to Tender Work</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" />
    <asp:HiddenField ID="hdnSubmissionDate" runat="server" />
    <asp:HiddenField ID="hdnOpeningDate" runat="server" />

    <script type="text/javascript">
        $(function () {



            //$('.timepicker1').timepicker({

            //    defaultTime: 'current',
            //    minuteStep: 1,
            //    disableFocus: true,
            //    template: 'dropdown'


            //});
            //Enable Disable TextBoxes in a Row when the Row CheckBox is checked.
            $("[id*=chkRejected]").bind("click", function () {

                //Find and reference the GridView.
                var grid = $(this).closest("table");



                //If the CheckBox is Checked then enable the TextBoxes in thr Row.
                if (!$(this).is(":checked")) {
                    var td = $("td", $(this).closest("tr"));
                    //td.css({ "background-color": "#FFF" });
                    $("input[type=text]", td).attr("disabled", "disabled");

                } else {
                    var td = $("td", $(this).closest("tr"));
                    //td.css({ "background-color": "#D8EBF2" });
                    $("input[type=text]", td).removeAttr("disabled");

                }


            });



        });
    </script>
</asp:Content>
