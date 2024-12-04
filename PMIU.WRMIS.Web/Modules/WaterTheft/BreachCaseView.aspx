<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BreachCaseView.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.BreachCaseView" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Breach Incident</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Select Channel</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlChannel" runat="server" AutoPostBack="true" disabled="disabled">
                                        </asp:DropDownList>
                              <%--           <asp:Label id="lblChannel" runat="server" CssClass="col-xs-4 col-lg-3 control-label"> </asp:Label>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-xs-4 col-lg-3 control-label">Outlet R.D. (ft)</asp:Label>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDLeft" autofocus="autofocus" runat="server" placeholder="Outlet R.D." required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput required form-control" ReadOnly></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        +
                                    </div>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtOutletRDRight" runat="server" oninput="CompareRDValues(this)" placeholder="Outlet R.D." required="required" pattern="^(0|[0-9][0-9]*)$" CssClass="integerInput required form-control" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Side</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlSide" runat="server" AutoPostBack="true" disabled="disabled">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Incident Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtIncidentDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text" disabled="disabled"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Time of Checking</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                                            <asp:TextBox ID="txtIncidentTime" TabIndex="5" runat="server" required="required" class="required form-control" type="text" ReadOnly></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Head Discharge</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtHeadDischarge" type="text" class="form-control" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Length of Breaching Section</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtLengthofbreach" type="text" class="form-control" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Remarks</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtRemarks" type="text" TextMode="multiline" class="form-control" Rows="3" ReadOnly> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Photos</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:FileUpload runat="server" ID="UploadImages" AllowMultiple="true" />
                                        <%--  <asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="uploadFile_Click"  /> --%>
                                        <asp:Label ID="listofuploadedfiles" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn" style="margin-bottom: 0px;">
                                   <%-- <asp:Button runat="server" ID="btnSaveBreachData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveBreachData_Click" />--%>
<%--                                    <asp:Button runat="server" ID="btnBack" value=" Back " CssClass="btn btn-primary" Text="&nbsp;Back" OnClick="btnBack_Click" />--%>
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/WaterTheft/SearchBreachIncident.aspx" CssClass="btn">Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>

                        <asp:HiddenField ID="BreachCaseId" runat="server" /> 
                    </div>

                     

                </div>
            </div>
        </div>
    </div>
    <!-- END Main Content -->
    </asp:Content>