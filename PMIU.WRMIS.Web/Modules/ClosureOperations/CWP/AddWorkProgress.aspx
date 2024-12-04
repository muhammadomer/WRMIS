<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddWorkProgress.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.AddWorkProgress" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/ClosureOperations/UserControls/WorkProgressInfo.ascx" TagPrefix="ucWPrgrsInfo" TagName="ucPrgrsInfoCntrl" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>
                        <asp:Label ID="lblPageTitle" runat="server" Text="Add Work Progress"></asp:Label></h3>
                </div>
                <div class="box-content">
                    <ucWPrgrsInfo:ucPrgrsInfoCntrl runat="server" ID="ucInfo" />
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hdnF_CWID" Value="0" runat="server" />
                        <asp:Label ID="lblMode" runat="server" Text="" Visible="false" />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                    <div class="col-sm-8 col-lg-9 controls" runat="server" id="divDate">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" TabIndex="4" required="required" runat="server" CssClass="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Work Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlWrkStatus" runat="server">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Approximate Progress (%)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="required form-control decimal2PInput" MaxLength="5" ID="txtPrg" required="required" runat="server" oninput="PercentageValidation(this)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divDesilting" runat="server" visible="false">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Quantity of Silt Removed (cft)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="required form-control integerInput" ID="txtSiltToRmv" MaxLength="6" required="required" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Length of Channel Desilted (ft)</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="required form-control integerInput" ID="txtChnlLnght" MaxLength="6" required="required" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachment</label>
                                    <div class="col-sm-8 col-lg-9 controls" id="divAttchmentAdd" runat="server">

                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="5" />
                                    </div>
                                    <div class="col-sm-8 col-lg-9 controls" id="divAttchmentView" runat="server" visible="false">
                                        <%--<asp:HyperLink ID="HyperLink1"  Text="Attachment"  runat="server" Visible="false" /> <br>
                                        <asp:HyperLink ID="HyperLink2"  Text="Attachment"  runat="server" Visible="false" /> <br>
                                        <asp:HyperLink ID="HyperLink3"  Text="Attachment"  runat="server" Visible="false" /> <br>
                                        <asp:HyperLink ID="HyperLink4"  Text="Attachment"  runat="server" Visible="false" /> <br>
                                        <asp:HyperLink ID="HyperLink5"  Text="Attachment"  runat="server" Visible="false" />--%>
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl2" Size="0" Visible="false" />
                                        <br />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl3" Size="0" Visible="false" />
                                        <br />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl4" Size="0" Visible="false" />
                                        <br />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl5" Size="0" Visible="false" />
                                        <br />
                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl6" Size="0" Visible="false" />

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                                    <div class="col-sm-7 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control commentsMaxLengthRow multiline-no-resize"
                                            minlength="3" MaxLength="250" TextMode="MultiLine" Rows="5" Columns="50"
                                            ID="txtRmrks" runat="server" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSave_Click" />
                                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnRefMonitoringID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsScheduled" runat="server" Value="false" />
    <asp:HiddenField ID="hdnScheduleDetailID" runat="server" Value="0" />
    <script type="text/javascript">
        $(document).ready(function () {
            var title = $('#<%=lblPageTitle.ClientID%>').text();
            if (title.toLowerCase() === 'work progress') {
                $('input').removeClass('required');

            }
        });
    </script>

</asp:Content>
