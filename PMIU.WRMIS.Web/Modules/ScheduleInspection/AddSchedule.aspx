<%@ Page Title="AddSchedule" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddSchedule.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.AddSchedule" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Add New Schedule</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label CssClass="col-sm-5 col-lg-4 control-label" ID="lblScheduleTitle" Text="Schedule Title" runat="server" />
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:TextBox CssClass="form-control required" required="required" Minlength="3" MaxLength="100" ID="txtScheduleTitle" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">From Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control disabled-Past-date-picker required" required="true" type="text"></asp:TextBox>
                                    <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-5 col-lg-4 control-label">To Date</label>
                            <div class="col-sm-7 col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control disabled-Past-date-picker required" required="true" type="text"></asp:TextBox>
                                    <span id="spanToDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:Label AssociatedControlID="txtTourDescripton" CssClass="col-sm-5 col-lg-4 control-label" ID="lblTuorDes" Text="Schedule Description" runat="server" />
                            <div class="col-sm-7 col-lg-8 controls">
                                <asp:TextBox CssClass="form-control commentsMaxLengthRow required multiline-no-resize" minlength="3" MaxLength="250" required="required" TextMode="MultiLine" Rows="5" Columns="50"
                                    ID="txtTourDescripton" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnScheduleID" runat="server" Value="0" />
</asp:Content>
