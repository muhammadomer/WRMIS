<%@ Page Title="TenderNotice" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddTenderNotice.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.AddTenderNotice" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Tender Notice</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">

                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblTenderNotice" class="col-sm-4 col-lg-3 control-label">Tender Notice</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtTenderNotice" runat="server" CssClass="form-control required" required="true" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlDomain" class="col-sm-4 col-lg-3 control-label">Domain</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDomain" CssClass="form-control required" runat="server" AutoPostBack="True" required="true" TabIndex="2" OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                       <%-- OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged" --%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lblddlDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" CssClass="form-control required" runat="server" required="true" TabIndex="3">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                           </ContentTemplate>
                </asp:UpdatePanel>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Submission Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtSubmissionDate" TabIndex="4" runat="server" required="required" class="required form-control disabled-Past-date-picker" size="16" type="text"></asp:TextBox>
                                            <span id="SubmissionDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">

                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Submission Time</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%--<div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                                <asp:TextBox ID="txtSubmissionTime" TabIndex="6" runat="server" required="true" class="required form-control timepicker-default" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>--%>

                                        <uc1:TimePicker runat="server" ID="TimePickerSubmission" />
                                    </div>
                                </div>



                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Opening Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtOpeningDate" TabIndex="5" runat="server" required="required" class="required form-control disabled-Past-date-picker" size="16" type="text"></asp:TextBox>
                                            <span id="OpeningDate" class="input-group-addon clear" style="background-color: #fff;" runat="server"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Opening Time</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%-- <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                                <asp:TextBox ID="txtOpeningTime" TabIndex="6" runat="server" required="true" class="required form-control timepicker-default" type="text"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>--%>

                                        <uc1:TimePicker runat="server" ID="TimePickerOpening" />
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
                                        <%-- <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control" ID="txtAdvertisementSource" Text='<%# Eval("AdvertisementSource") %>' runat="server"></asp:TextBox>
                                 </EditItemTemplate>--%>
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblAdvertisementSource" runat="server" Text='<%# Eval("AdvertisementSource") %>'></asp:Label>--%>
                                            <asp:TextBox CssClass="form-control required" ID="txtAdvertisementSource" Text='<%# Eval("AdvertisementSource") %>' runat="server" required="required" Enabled="<%# GetEnableValue() %>"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <%--  <EditItemTemplate>
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtAdvertisementDate" Text='<%# Eval("AdvertisementDate") %>' runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" id="spnDateOfVisit" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </EditItemTemplate>--%>
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblAdvertisementDate" runat="server" Text='<%# Eval("AdvertisementDate") %>'></asp:Label>--%>
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtAdvertisementDate" Text='<%# Eval("AdvertisementDate", "{0:d-MMM-yyyy}") %>' runat="server" CssClass="<%# GetClassValue() %>" required="true" type="text" Enabled="<%# GetEnableValue() %>"></asp:TextBox>
                                                <span class="input-group-addon clear" id="spnDateOfVisit" visible="<%# GetVisibleValue() %>" runat="server" style="background-color: #fff;"><i class="fa fa-times"></i></span>
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
                                                <%--<asp:Button ID="btnEditAdvertisement" runat="server" Text="" CommandName="Edit" Visible="true" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />--%>
                                                <asp:Button ID="lbtnDeleteAdvertisement" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate" Visible="true" Enabled="<%# GetEnableValue() %>"
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                            </asp:Panel>
                                        </ItemTemplate>
                                        <%--   <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionAdvertisement" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveAdvertisement" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelAdvertisement" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>--%>
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
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize" TabIndex="6" TextMode="MultiLine" Rows="5" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Document</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                         <%--       <asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />--%>
                                      <uc1:FileUploadControl runat="server" ID="FileUploadControl1"  />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" Visible="<%# base.CanAdd %>"  />
                                <%--OnClientClick="return RemoveRequiredKeyWord()"--%>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>


        </div>
    </div>
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsEditMode" runat="server" Value="" />
    <asp:HiddenField ID="hdnFileName" runat="server" Value="0"/>


    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
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
