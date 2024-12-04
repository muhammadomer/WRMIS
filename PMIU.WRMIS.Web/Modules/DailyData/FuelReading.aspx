<%@ Page MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="FuelReading.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.FuelReading" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register Src="~/Common/Controls/TimePicker.ascx" TagPrefix="uc1" TagName="TimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Fuel Refilling</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label runat="server" class="col-sm-4 col-lg-3 control-label">Meter Reading</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtReading" type="text" class="form-control integerInput" MaxLength="7"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label runat="server" class="col-sm-4 col-lg-3 control-label">Petrol Quantity</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtQuantity" type="text" class="form-control decimalInput" MaxLength="7"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Date of Observation</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtIncidentDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Time of Observation</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <uc1:TimePicker runat="server" ID="TimePicker" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Comments</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtRemarks" type="text" MaxLength="250" onblur="TrimInput(this);" TextMode="multiline" class="form-control required multiline-no-resize" required="true" Rows="5" TabIndex="14"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button runat="server" ID="btnSaveOutletIncidentData" value=" Save " CssClass="btn btn-primary" Text="&nbsp;Save" />
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Modules/DailyData/FuelAndMeterReading.aspx" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" lang="javascript">

        $(document).ready(function () {
            alert("In Ready");
        });

    </script>
</asp:Content>


