<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailedScreen.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.DetailedScreen" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Detailed Screen</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-12">
                        <div id="divPanel" class="panel panel-default" runat="server">
                            <div id="Tabs" role="tabpanel">
                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist">
                                    <li id="liActivity" runat="server" class="active"><a href="#Activity" runat="server" onserverclick="anchActivity_ServerClick" id="anchActivity" aria-controls="Activity" role="tab">Activity</a></li>
                                    <li id="liDetails" runat="server"><a href="#Details" id="anchDetails" runat="server" onserverclick="anchDetails_ServerClick" aria-controls="Details" role="tab">Details</a></li>
                                    <li id="liAccessibility" runat="server"><a href="#Accessibility" id="anchAccessibility" runat="server" onserverclick="anchAccessibility_ServerClick" aria-controls="Accessibility" role="tab">Accessibility</a></li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content" style="padding-top: 20px">

                                    <div role="tabpanel" class="tab-pane active" runat="server" id="Activity" clientidmode="Static">
                                    </div>


                                    <div role="tabpanel" class="tab-pane" runat="server" id="Details" clientidmode="Static">

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblComplaintSource" runat="server" Text="Complaint Source" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlComplaintSource" runat="server" CssClass="form-control required" TabIndex="1" AutoPostBack="true" required="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblComplaintID" runat="server" Text="Complaint ID" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtComplaintID" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblFirstName" runat="server" Text="First Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control required" required="true" TabIndex="3" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblLastName" runat="server" Text="Last Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control required" required="true" TabIndex="4" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblMobileNo" runat="server" Text="Mobile No." CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control phoneNoInput required" TabIndex="5" onkeyup="PhoneNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" required="true" ClientIDMode="Static" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblPhoneNo" runat="server" Text="Landline No." CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtPhoneNo" runat="server" onkeyup="PhoneNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" ClientIDMode="Static" CssClass="form-control phoneNoInput" TabIndex="6" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblComplaintDate" runat="server" class="col-sm-4 col-lg-3 control-label">Complaint Date</asp:Label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <div class="input-group date" data-date-viewmode="years">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txtComplaintDate" runat="server" CssClass="date-picker form-control" TabIndex="7"></asp:TextBox>
                                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control multiline-no-resize" MaxLength="250" TabIndex="8" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblDomain" runat="server" Text="Domain" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlDomain" runat="server" CssClass="form-control required" TabIndex="9" required="true" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblDivisionByDomain" runat="server" Text="Division" Visible="false" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <asp:Label ID="lblStructure" runat="server" Text="Structure" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlDivisionByDomain" runat="server" Visible="false" CssClass="form-control required" TabIndex="10" required="true" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:DropDownList ID="ddlStructure" runat="server" CssClass="form-control required" TabIndex="10" required="true" AutoPostBack="true" Enabled="false">
                                                            <asp:ListItem Value="">Select</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                                     <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" TabIndex="10" required="true">
                                                            <asp:ListItem Value="">Select</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblComplaintType" runat="server" Text="Complaint Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:DropDownList ID="ddlComplaintType" runat="server" CssClass="form-control required" TabIndex="11" required="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblResponseDuration" runat="server" Text="Response Duration" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtResponseDuration" runat="server" CssClass="form-control integerInput" TabIndex="12" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblPmiuFileNo" runat="server" Text="PMIU File No." CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtPmiuFileNo" runat="server" CssClass="form-control" TabIndex="13" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-lg-3 control-label">Complaint File</label>
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 ">
                                                <div class="form-group">
                                                    <asp:Label ID="lblComplaintDetails" runat="server" Text="Complaint Details" CssClass="col-sm-4 col-lg-3 control-label" />
                                                    <div class="col-sm-8 col-lg-9 controls">
                                                        <asp:TextBox ID="txtComplaintDetails" runat="server" CssClass="form-control multiline-no-resize" MaxLength="250" TabIndex="8" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>


                                    <div role="tabpanel" class="tab-pane" runat="server" id="Accessibility" clientidmode="Static">

                                        <div class="row">
                                            <div class="col-md-12 ">
                                                <div class="form-group">
                                                    <asp:CheckBoxList ID="chkAdditionalAccessibility" RepeatColumns="2" CssClass="table" runat="server">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $('.CtrlClass0').blur();
        $('.CtrlClass0').removeAttr('required');
    </script>

</asp:Content>
