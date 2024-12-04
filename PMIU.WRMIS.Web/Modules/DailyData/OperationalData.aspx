<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OperationalData.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.DailyData.OperationalData" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .myAltRowClass {
            color: red;
            background-color: #DCFFFF;
            background-image: none;
        }
    </style>
    <div class="box">
        <div class="box-title">
            <h3>Channel Search</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <!-- BEGIN Left Side -->

                        <div class="form-group">
                            <label for="ddlZone" id="lblZone" class="col-sm-4 col-lg-3 control-label">Zone</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="1" ID="ddlZone" runat="server" required="required" CssClass="required form-control" data-rule-required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged" AutoPostBack="True" Enabled="False"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlDivision" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="3" ID="ddlDivision" runat="server" required="required" CssClass="required form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label id="lblDate" class="col-sm-4 col-lg-3 control-label">Date</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtDate" TabIndex="5" runat="server" required="required" class="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                        <!-- END Left Side -->
                    </div>
                    <div class="col-md-6 ">
                        <!-- BEGIN Right Side -->

                        <div class="form-group">
                            <label for="ddlCircle" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Circle</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="2" ID="ddlCircle" runat="server" required="required" CssClass="required form-control" data-rule-required="true" AutoPostBack="True" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlSubDivision" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="4" ID="ddlSubDivision" runat="server" required="required" CssClass="required form-control" data-rule-required="true" Enabled="False"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ddlSession" id="lblSession" class="col-sm-4 col-lg-3 control-label">Session</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList TabIndex="6" ID="ddlSession" runat="server" required="required" CssClass="required form-control" data-rule-required="true" Enabled="False"></asp:DropDownList>
                            </div>
                        </div>
                        <!-- END Right Side -->
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <input type="submit" class="btn btn-primary" onclick="SearchOperationalData(); return false" value=" Show ">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <table id="operationalDataGrid">
                            </table>
                            <div id="pager"></div>
                            <table id="NoRecordFound" class="hidden">
                                <tbody>
                                    <tr>
                                        <td colspan="9">No record found</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnAuditDailyGaugeReadingID" runat="server" ClientIDMode="Static" Value="-1" />
                </div>

            </div>
            <!-- Start Of Edit Gauge  Value -->
            <div id="editvalue" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="row">

                                    <div class="test">

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Current Guage Value</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <div style="margin-top: 5px;"><span id="spnCurrentGaugeValue">4.88</span></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">New Guage Value</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:TextBox ID="txtNewGaugeValue" runat="server" placeholder="" required="true" CssClass="decimalInput form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-4 col-lg-3 control-label">Reason for Change</label>
                                                <div class="col-sm-8 col-lg-9 controls">
                                                    <asp:DropDownList ID="ddlReasonForChange" required="required" runat="server" CssClass="required form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div>
                                        <asp:HiddenField ID="hdnGaugeID" runat="server" ClientIDMode="Static" Value="-1" />
                                        <asp:HiddenField ID="hdnGaugeReadingID" runat="server" ClientIDMode="Static" Value="-1" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">


                            <input type="submit" class="btn btn-primary" onclick="GetDischargeValue(); return false" value=" Save changes ">

                            <%-- <button class="btn btn-primary">Save changes</button>--%>
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END Of Edit Gauge  Value -->
            <!-- Start Of Audit Trail -->
            <div id="auditTrail" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <table class="table tbl-info">
                                <tr>
                                    <th>Channel Name</th>
                                    <td><span id="spnChannelName"></span></td>
                                    <th>Guage Type</th>
                                    <td><span id="spnGaugeType"></span></td>
                                </tr>
                                <tr>
                                    <th>Guage RD</th>
                                    <td><span id="spnGaugeRD"></span></td>
                                    <th>Section</th>
                                    <td><span id="spnSection"></span></td>
                                </tr>
                                <tr>
                                    <th>Session</th>
                                    <td><span id="spnSession"></span></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <hr />
                            <table id="auditTrailGrid">
                            </table>
                            <%--<div id="auditTrailPager"></div>--%>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END Of Audit Trail -->
            <!-- Start Of View Gauge Image -->
            <div id="viewimage" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-body">
                            <center>
				               <%--<img id="gaugeImage" src="../../Design/img/noimage.jpg" />--%>
                                <img id="gaugeImage"/>
				            </center>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </div>
                <!-- END Of View Gauge Image -->
            </div>

        </div>
        <asp:HiddenField ID="hdnChannelID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnSearchCriteria" runat="server" Value="" />
    </div>
    <script type="text/ecmascript" src="../../Scripts/jquery-1.10.2.min.js"></script>
    <%--<script type="text/ecmascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>--%>
    <script type="text/ecmascript" src="../../Design/js/grid.locale-en.js"></script>
    <!-- This is the Javascript file of jqGrid -->
    <script type="text/ecmascript" src="../../Design/js/jquery.jqGrid.js"></script>
    <!-- This is the localization file of the grid controlling messages, labels, etc.
    <!-- We support more than 40 localizations -->
    <%--    <script type="text/ecmascript" src="../../../js/trirand/i18n/grid.locale-en.js"></script>--%>
    <!-- A link to a jQuery UI ThemeRoller theme, more than 22 built-in and many more custom -->
    <%--    <link rel="stylesheet" type="text/css" media="screen" href="../../../css/jquery-ui.css" />--%>
    <!-- The link to the CSS that the grid needs -->
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css">--%>
    <link href="../../Design/css/ui.jqgrid-bootstrap.css" rel="stylesheet" />
    <script type="text/javascript">

        $.jgrid.defaults.width = 780;
        $.jgrid.defaults.responsive = true;
        $.jgrid.defaults.styleUI = 'Bootstrap';;
        //$(function () {
        //    $.jgrid.no_legacy_api = true;
        //    $.jgrid.useJSON = true;
        //});

        $('#<%=txtDate.ClientID%>').change(function () {
            PreSelectSession();
        });
    </script>
    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>--%>
    <script src="../../Scripts/DailyData/OperationalData.js"></script>
</asp:Content>
