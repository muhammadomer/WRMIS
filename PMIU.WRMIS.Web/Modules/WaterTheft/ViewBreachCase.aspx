<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBreachCase.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.BreachCaseView" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="ucbreach" TagName="FileUploadControl" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Breach Incident</h3>
                </div>
                <div class="box-content">
                       <asp:UpdatePanel ID="ViewBreachIncident" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                 
                    <div class="form-horizontal">
                      <div class="row" id="divisionId" runat="server">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label id="lbldivision" runat="server" class="col-sm-4 col-lg-3 control-label">Division</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                       <asp:DropDownList CssClass="form-control Hide" required="true" ID="ddlDivision" runat="server"   disabled="disabled" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                          <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Channel</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control " required="true" ID="ddlChannel" runat="server" AutoPostBack="true" disabled="disabled">
                                        </asp:DropDownList>
                              <%--           <asp:Label id="lblChannel" runat="server" CssClass="col-xs-4 col-lg-3 control-label"> </asp:Label>--%>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                      <%--      <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Select Channel</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control " required="true" ID="ddlChannel" runat="server" AutoPostBack="true" disabled="disabled">
                                        </asp:DropDownList>
                              <%--           <asp:Label id="lblChannel" runat="server" CssClass="col-xs-4 col-lg-3 control-label"> </asp:Label>
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">RD</asp:Label>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDLeft" autofocus="autofocus" runat="server" placeholder="Outlet RD" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput  form-control" ReadOnly></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        +
                                    </div>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="Outlet RD" required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput  form-control" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Side</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control " required="true" ID="ddlSide" runat="server" AutoPostBack="true" disabled="disabled">
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
                                            <asp:TextBox ID="txtIncidentDate" TabIndex="5" runat="server" required="required" class=" form-control date-picker" size="16" type="text" disabled="disabled"></asp:TextBox>
                                     <%--       <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Checking Time </label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                    <%--    <div class="input-group date" data-date-viewmode="years"> </div>--%>
                                         <%--   <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>--%>
                                            <asp:TextBox ID="txtIncidentTime" TabIndex="5" runat="server" required="required" class=" form-control" type="text" ReadOnly></asp:TextBox>
                                          <%--  <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>--%>
                                       
                                       <%-- <uc1:TimePicker runat="server" ID="BreachTimePicker" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Head Discharge</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtHeadDischarge" type="text" class="form-control integerInput" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Length of Breaching Section</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtLengthofbreach" type="text" class="form-control integerInput" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtRemarks" type="text" TextMode="multiline" class="form-control multiline-no-resize" Rows="3" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Field Staff available</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlFieldStaff" runat="server" disabled="disabled">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                         <asp:LinkButton ID="lnkViewAttachments" OnClick="lnkViewAttachments_Click" runat="server">View Attachments</asp:LinkButton>
                                       
                                    </div>
                                   
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn" style="margin-bottom: 0px;">
                                   <%-- <asp:Button runat="server" ID="btnSaveBreachData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveBreachData_Click" />--%>
<%--                                    <asp:Button runat="server" ID="btnBack" value=" Back " CssClass="btn btn-primary" Text="&nbsp;Back" OnClick="btnBack_Click" />--%>
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/WaterTheft/SearchBreachIncident.aspx?RP=2" CssClass="btn">Back</asp:HyperLink>
                                    <asp:LinkButton ID="lbtnBackToET" runat="server" Visible ="false" Text="Back" class="btn" PostBackUrl="~/Modules/DailyData/MeterReadingAndFuel.aspx?ShowSearched=true"></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <asp:HiddenField ID="BreachCaseId" runat="server" /> 
                    </div>

                        </ContentTemplate>
                           </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
    <!-- Start Of view images -->
<div id="viewAttachment" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                <asp:UpdatePanel runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="gvViewAttachment" runat="server"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="false"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            DataKeyNames="AttachmentPath" OnRowDataBound="gvViewAttachment_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="File name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploadedBy" runat="server" Text='<%# Eval("UploadedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="hlImage" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.WaterTheft , Convert.ToString(Eval("AttachmentPath"))) %>' CssClass="btn btn-primary btn_24 viewimg" runat="server" />--%>
                                         <ucbreach:FileUploadControl runat="server" ID="FileUploadControl" Size="0" />
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
            </div>
        </div>
    </div>
</div>
<!-- End Of view images -->
    </asp:Content>