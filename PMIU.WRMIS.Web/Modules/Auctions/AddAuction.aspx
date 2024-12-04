<%@ Page Title="AuctionNotice" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddAuction.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.AddAuction" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Auction Notice</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                 <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                <div class="row">

                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblTenderNotice" class="col-sm-4 col-lg-3 control-label">Auction Title/Notice</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtAuctionNoticetitle" runat="server" CssClass="form-control required" required="true" MaxLength="100" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <%--    <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlZone" CssClass="form-control required" runat="server" AutoPostBack="True" required="true" TabIndex="2">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                      </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" CssClass="form-control required" runat="server" required="true" TabIndex="3">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>--%>


                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblddlDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlDivision" CssClass="form-control required" runat="server" required="true" TabIndex="4">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblddlApprovalAuthority" class="col-sm-4 col-lg-3 control-label">Approval Authority</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlApprovalAuthority" CssClass="form-control required" runat="server" required="true" TabIndex="5">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblddlAuctionType" class="col-sm-4 col-lg-3 control-label">Auction Type</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlAuctionType" CssClass="form-control required" runat="server" required="true" TabIndex="6">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblddlAuctionCategory" class="col-sm-4 col-lg-3 control-label">Auction Category</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlAuctionCategory" CssClass="form-control required" runat="server" required="true" TabIndex="7" AutoPostBack="true" OnSelectedIndexChanged="ddlAuctionCategory_OnSelectedIndexChanged">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>


                <h5><b>Duration</b></h5>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" TabIndex="8" runat="server" class="form-control disabled-Past-date-picker disbaled" size="16" type="text" Disabled="Disabled"></asp:TextBox>
                                    <span id="FromDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" TabIndex="9" runat="server" class="form-control disabled-Past-date-picker disbaled" size="16" type="text" Disabled="Disabled"></asp:TextBox>
                                    <span id="ToDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </ContentTemplate>
                </asp:UpdatePanel>


                <h5><b>Date and Time</b></h5>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Opening Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtOpeningDate" TabIndex="10" runat="server" required="required" class="required form-control disabled-Past-date-picker" size="16" type="text"></asp:TextBox>
                                    <span id="OpeningDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Opening Time</label>
                            <div class="col-sm-7 col-lg-8 controls">

                                <uc1:TimePicker runat="server" ID="TimePickerOpening" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Opening Location</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:TextBox ID="txtOpeningPlace" runat="server" CssClass="form-control required" required="true" MaxLength="100" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Alternate Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtAlternateOpeningDate" TabIndex="12" runat="server" class="form-control disabled-Past-date-picker" size="16" type="text"></asp:TextBox>
                                    <span id="AlternateOpeningDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Alternate Time</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <uc1:TimePicker runat="server" ID="TimePickerOpeningAlternate" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Alternate Location</label>
                            <div class="col-sm-7 col-lg-8 controls">

                                <asp:TextBox ID="txtAlternateOpeningPlace" runat="server" CssClass="form-control" MaxLength="100" TabIndex="13"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Submission Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtSubmissionDate" TabIndex="14" runat="server" class="form-control disabled-Past-date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                    <span id="SubmissionDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Submission Time</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <uc1:TimePicker runat="server" ID="TimePickerSubmission" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">Submission Fee</label>
                            <div class="col-sm-7 col-lg-8 controls">

                                <asp:TextBox ID="txtSubmissionFee" runat="server" CssClass="form-control integerInput text-left" MaxLength="10" TabIndex="15"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Auction Details</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtAuctionDetails" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="16" TextMode="MultiLine" Rows="5" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
               <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="col-md-12">
                            <h5><b>Advertisement Source</b></h5>
                            <asp:GridView ID="gvAdvertisementSource" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="gvAdvertisementSource_RowDataBound"
                                OnRowDeleting="gvAdvertisementSource_RowDeleting" AllowPaging="False"
                                CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Advertisement Source">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAdvertisementSource" Text='<%# Eval("AdvertisementSource") %>' runat="server" required="required" MaxLength="100" Enabled="<%# GetEnableValue() %>" CssClass='<%# GetClassValue("t") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtAdvertisementDate" Text='<%# Eval("AdvertisementDate", "{0:d-MMM-yyyy}") %>' runat="server" required="true" CssClass='<%# GetClassValue("d") %>' type="text" Enabled="<%# GetEnableValue() %>"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" visible="<%# GetVisibleValue() %>" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                                <asp:Button ID="btnAdvertisementGrid" runat="server" Text="" CommandName="AddAdvertisement" Visible="true" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" OnClick="AddRow_Grid" Enabled="<%# GetEnableValue() %>" />
                                            </asp:Panel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlActionAdvertisement" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="lbtnDeleteAdvertisement" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="true" Enabled="<%# GetEnableValue() %>"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>

                        </div>
                
                         </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-12">
                        <div id="DivToChange" class="col-md-6" runat="server">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Document(s)</label>
                            <div class="col-sm-4 col-lg-5 controls">
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="5" />
                                <%--<asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />--%>
                            </div>
                            </div>
                            </div>
                        <div class="col-md-6">
                            <div>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <%--OnClientClick="return RemoveRequiredKeyWord()"--%>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>


        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsEditMode" runat="server" Value="" />
    <asp:HiddenField ID="hdnFileName" runat="server" Value="0" />


    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizePastDatePickerOnUpdatePanelRefresh();
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                }
            });
        };


        function RemoveRequiredKeyWord() {
            $('.CtrlClass0').blur();
            $('.CtrlClass0').removeAttr('required');
            return true;
        }

    </script>

</asp:Content>
