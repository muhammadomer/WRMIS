<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="AddComplaint.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ComplaintsManagement.AddComplaint" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="lblAddComplaint">Add Complaint</h3>
            <h3 runat="server" id="lblEditComplaint">Edit Complaint</h3>
            <h3 runat="server" id="lblViewComplaint">Complaint Details</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">



                <div id="divForDDH" runat="server">




                    <h3>Complainant Information</h3>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblComplaintSource" runat="server" Text="Complaint Source" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlComplaintSource" runat="server" CssClass="form-control required" TabIndex="1" AutoPostBack="true" required="true" OnSelectedIndexChanged="ddlComplaintSource_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblComplaintID" runat="server" Text="Complaint ID" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtComplaintID" runat="server" CssClass="form-control required" required="true" TabIndex="2" MaxLength="8" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblFirstName" runat="server" Text="First Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control required" required="true" TabIndex="3" MaxLength="75" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblLastName" runat="server" Text="Last Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control required" required="true" TabIndex="4" MaxLength="75" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblMobileNo" runat="server" Text="Mobile No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control phoneNoInput required" TabIndex="5" onkeyup="PhoneNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" required="true" ClientIDMode="Static" MaxLength="13"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblPhoneNo" runat="server" Text="Landline No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtPhoneNo" runat="server" onkeyup="LandlineNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" ClientIDMode="Static" CssClass="form-control phoneNoInput" TabIndex="6" MaxLength="15"></asp:TextBox>
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
                                        <asp:TextBox ID="txtComplaintDate" runat="server" CssClass="disabled-future-date-picker form-control required" required="true" TabIndex="7"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtAddress" runat="server" required="true" CssClass="form-control multiline-no-resize required" MaxLength="250" TabIndex="8" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr>
                    <h3>Complaint Information</h3>

                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDomain" runat="server" Text="Domain" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDomain" runat="server" CssClass="form-control required" TabIndex="9" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDivisionByDomain" runat="server" Text="Division" Visible="false" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <asp:Label ID="lblStructure" runat="server" Text="Structure" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDivisionByDomain" runat="server" Visible="false" CssClass="form-control required" TabIndex="10" required="true" AutoPostBack="true"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlStructure" runat="server" CssClass="form-control required" TabIndex="10" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlStructure_SelectedIndexChanged" Enabled="false">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>



                            <div class="row">
                                <div class="col-md-12">
                                    <div id="divPanel" visible="false" class="panel panel-default" runat="server">
                                        <div id="Tabs" role="tabpanel">
                                            <!-- Nav tabs -->
                                            <ul class="nav nav-tabs" role="tablist">
                                                <li id="liVillage" runat="server" class="active"><a href="#Village" id="anchVillage" onserverclick="anchVillage_ServerClick" onclick="TabNameInLabel('Village');" runat="server" aria-controls="Village" role="tab">Village</a></li>
                                                <li id="liDivision" runat="server"><a href="#Division" id="anchDivision" onserverclick="anchDivision_ServerClick" onclick="TabNameInLabel('Division');" runat="server" aria-controls="Division" role="tab">Division</a></li>
                                            </ul>
                                            <asp:TextBox ID="lblValueForTab" runat="server" Style="display: none;" ClientIDMode="Static" Text=""></asp:TextBox>
                                            <!-- Tab panes -->
                                            <div class="tab-content" style="padding-top: 20px">

                                                <div role="tabpanel" class="tab-pane active" runat="server" id="Village" clientidmode="Static">

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <!-- BEGIN Left Side -->
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabDistrict" runat="server" Text="District" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabDistrict" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlVillageTabDistrict_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabTehsil" runat="server" Text="Tehsil" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabTehsil" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlVillageTabTehsil_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <!-- END Left Side -->
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <!-- BEGIN Left Side -->
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabVillage" runat="server" Text="Village" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabVillage" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlVillageTabVillage_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabChannel" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label ID="lblVillageTabProtectionStructure" Visible="false" runat="server" Text="Protection Structure" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label ID="lblVillageTabDrain" Visible="false" runat="server" Text="Drain" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabChannel" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlVillageTabChannel_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlVillageTabProtectionStructure" Visible="false" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" Enabled="False">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlVillageTabDrain" Visible="false" runat="server" CssClass="form-control required" required="true" AutoPostBack="True" Enabled="False">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <!-- END Left Side -->
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabOutlet" Visible="false" runat="server" Text="Outlet" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label AssociatedControlID="txtVillageTabTotalRDLeft" ID="lblVillageTabTotalRDs" Text="RD" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabOutlet" Visible="false" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlVillageTabOutlet_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>


                                                                    <div class="col-sm-3 col-lg-4 controls" style="padding-left: 0px;">
                                                                        <asp:TextBox ID="txtVillageTabTotalRDLeft" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="form-control integerInput" MaxLength="3"></asp:TextBox>
                                                                    </div>
                                                                    <div id="lblVillageTabPlusSign" runat="server" class="col-sm-1 col-lg-1 controls" style="padding-left: 0px;">
                                                                        +
                                                                    </div>
                                                                    <div class="col-sm-3 col-lg-4 controls" style="padding-left: 0px;">
                                                                        <asp:TextBox ID="txtVillageTabTotalRDRight" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="integerInput form-control" MaxLength="3"></asp:TextBox>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblVillageTabDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlVillageTabDivision" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div role="tabpanel" class="tab-pane" runat="server" id="Division" clientidmode="Static">

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabZone" runat="server" CssClass="form-control required" AutoPostBack="True" required="true" OnSelectedIndexChanged="ddlDivisionTabZone_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabCircle" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlDivisionTabCircle_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabDivision" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlDivisionTabDivision_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabSubDivision" runat="server" Text="Sub Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label ID="lblDivisionTabProtectionStructure" Visible="false" runat="server" Text="Protection Structure" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabSubDivision" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlDivisionTabSubDivision_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlDivisionTabProtectionStructure" Visible="false" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabChannel" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label ID="lblDivisionTabDrain" Visible="false" runat="server" Text="Drain" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabChannel" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true" OnSelectedIndexChanged="ddlDivisionTabChannel_SelectedIndexChanged">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlDivisionTabDrain" Visible="false" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="form-group">
                                                                <asp:Label ID="lblDivisionTabOutlet" Visible="false" runat="server" Text="Outlet" CssClass="col-sm-4 col-lg-3 control-label" />
                                                                <asp:Label AssociatedControlID="txtDivisionTabTotalRDLeft" ID="lblDivisionTabTotalRDs" Text="RD" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:DropDownList ID="ddlDivisionTabOutlet" Visible="false" runat="server" CssClass="form-control required" AutoPostBack="True" Enabled="False" required="true">
                                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                                    </asp:DropDownList>


                                                                    <div class="col-sm-3 col-lg-4 controls" style="padding-left: 0px;">
                                                                        <asp:TextBox ID="txtDivisionTabTotalRDLeft" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="form-control integerInput" MaxLength="3"></asp:TextBox>
                                                                    </div>
                                                                    <div id="lblDivisionTabPlusSign" runat="server" class="col-sm-1 col-lg-1 controls" style="padding-left: 0px;">
                                                                        +
                                                                    </div>
                                                                    <div class="col-sm-3 col-lg-4 controls" style="padding-left: 0px;">
                                                                        <asp:TextBox ID="txtDivisionTabTotalRDRight" runat="server" placeholder="RD" ClientIDMode="Static" CssClass="integerInput form-control" MaxLength="3"></asp:TextBox>
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



                            <div id="divWithoutTab" runat="server" visible="false">

                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlCircle" runat="server" Enabled="false" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
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
                                                <asp:DropDownList ID="ddlDivision" runat="server" Enabled="false" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <asp:Label ID="lblBarrage" runat="server" Text="Barrage/Headwork" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <asp:Label ID="lblSmallDams" Visible="false" runat="server" Text="Small Dams" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <asp:Label ID="lblChannel" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlBarrage" runat="server" Enabled="false" CssClass="form-control required" required="true" ClientIDMode="Static">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlSmallDams" Visible="false" Enabled="false" runat="server" CssClass="form-control required" required="true">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlChannel" runat="server" Enabled="false" CssClass="form-control required" required="true">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblComplaintType" runat="server" Text="Complaint Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlComplaintType" runat="server" CssClass="form-control required" TabIndex="11" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlComplaintType_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblResponseDuration" runat="server" Text="Response Duration" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtResponseDuration" runat="server" CssClass="form-control integerInput" TabIndex="12" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblPmiuFileNo" runat="server" Text="PMIU File No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtPmiuFileNo" runat="server" CssClass="form-control" TabIndex="13" MaxLength="100" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Complaint File</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />

                                    <%--  <asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />--%>
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="0" />
                                    <%-- <asp:FileUpload ID="fuComplaintFile" runat="server" CssClass="form-control" />
                                <asp:HyperLink ID="lnkFile" Text="" runat="server"></asp:HyperLink>--%>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblComplaintDetails" runat="server" Text="Complaint Details" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtComplaintDetails" runat="server" CssClass="form-control multiline-no-resize" MaxLength="250" TabIndex="14" ClientIDMode="Static" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>



                </div>

                <div id="DivForOthers" runat="server" style="display: none">
                    <asp:Table ID="tblChannelWaterTheftIncident" runat="server" CssClass="table tbl-info">
                        <asp:TableRow>
                            <asp:TableHeaderCell Width="33.3%">Complaint ID </asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Complaint Source</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="33.3%">Domain</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintNumber" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintSource" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersDomain" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Division</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Complaint Date</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Complainant Name</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersDivision" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintDate" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintName" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Complaint Type</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Mobile No</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Address</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintType" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersCell" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersAddress" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Complaint Details</asp:TableHeaderCell>
                            <asp:TableHeaderCell>PMIU File No.</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Response Duration</asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintDetails" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersPMIUFileNo" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersResponseDuration" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableHeaderCell>Attachment</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Complaint Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell></asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <%--  <asp:HyperLink ID="hlAttachment" runat="server" Visible="false" CssClass="btn btn-primary btn_24 viewimg"></asp:HyperLink>--%>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl2" Size="0" />
                                <asp:LinkButton ID="lnkViewAttachments" Visible="false" OnClick="lnkViewAttachments_Click" runat="server">View Attachments</asp:LinkButton>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthersComplaintStatus" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDivForOthers2" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>

                    </asp:Table>
                </div>

                <hr>
                <h3 id="lblAdditionalAccessibility" runat="server">Additional Accessibility</h3>

                <div class="row">
                    <div class="col-md-6" style="padding-left: 50px">
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkAdditionalAccessibility" RepeatColumns="2" TabIndex="15" CssClass="table  CheckboxAligned CheckboxAlligne" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" TabIndex="16" Text="Save" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" TabIndex="17" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnRefCode" />
    <!-- Start Of view images -->
    <div id="viewAttachment" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <%--   <asp:UpdatePanel runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                    <asp:GridView ID="gvViewAttachment" runat="server"
                        CssClass="table header" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="false"
                        EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                        OnPageIndexChanging="gvViewAttachment_PageIndexChanging" OnRowDataBound="gvViewAttachment_RowDataBound"
                        DataKeyNames="AttachmentPath">
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
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl4" Size="0" />
                                    <%-- <asp:HyperLink ID="hlImage" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.WaterTheft , Convert.ToString(Eval("AttachmentPath"))) %>' CssClass="btn btn-primary btn_24 viewimg" runat="server" />--%>
                                </ItemTemplate>
                                <ItemStyle Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <%--   </ContentTemplate>
                </asp:UpdatePanel>--%>
                </div>
                <div class="modal-footer">
                    <button id="btnCloseAttachment" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- End Of view images -->

    <script>

        $('.CtrlClass0').blur();
        $('.CtrlClass0').removeAttr('required');

        function TabNameInLabel(tabname) {
            $("#lblValueForTab").val(tabname);
        }

    </script>

</asp:Content>
