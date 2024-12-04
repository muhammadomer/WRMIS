<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddPlanBasicInfo.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.AddPlanBasicInfo" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc" TagName="FileUploadControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="hName" runat="server">Add Rotational Program</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButton ID="RBCircle" GroupName="rbtnSelection" CssClass="radio-inline" runat="server" Text="Circle" />
                                        <asp:RadioButton ID="RBDivision" CssClass="radio-inline" GroupName="rbtnSelection" runat="server" Text="Division" />
                                        <asp:RadioButton ID="RBSubDivision" CssClass="radio-inline" GroupName="rbtnSelection" runat="server" Text="Sub Division" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label5" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Plan Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtName" MaxLength="250" runat="server" CssClass="form-control required" required="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Circle</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircle" CssClass="form-control required" required="true" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="divShowHide" runat="server">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Sub division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label2" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Season</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSeason" CssClass="form-control required" required="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label3" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Year</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtYear" runat="server" MaxLength="9" CssClass="form-control required" required="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="divClosure" runat="server" visible="false">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Closure Start Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureStartDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="Closure End Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureEndDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDate" runat="server" Text="Plan Start Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" required="required" CssClass="disabled-todayPast-date-picker form-control required"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label4" CssClass="col-sm-4 col-lg-3 control-label" runat="server">No. of Groups</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlGroups" CssClass="form-control required" required="true" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Attachments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <uc:FileUploadControl runat="server" ID="FileUploadControl" Size="1" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 " id="divPriority" runat="server">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-sm-4 col-lg-3 control-label">Priority (RL/LR)</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:CheckBox ID="cbPriority" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" ClientIDMode="Static" OnClientClick="RemoveRequired()" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function RemoveRequired() {
            $('.CtrlClass0').removeAttr("required");
        }

        $(function () {
            $('.CtrlClass0').removeAttr("required");
        });

    </script>


</asp:Content>
