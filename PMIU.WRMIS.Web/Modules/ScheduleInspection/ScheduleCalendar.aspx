<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleCalendar.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ScheduleInspection.ScheduleCalendar" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .calendar-month-header {
            height: 55px !important;
            min-height: 55px !important;
            max-height: 55px !important;
        }
    </style>
    <!-- BEGIN Main Content -->
    <div class="box">
        <div class="box-title">
            <h3>Schedule Calendar</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <asp:HiddenField ID="hdnUserID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMonth" runat="server" Value="0" />
            <asp:HiddenField ID="hdnYear" runat="server" Value="0" />
            <asp:HiddenField ID="hdDateTime" runat="server" Value="0" />
            <br />
            <div class="row">
                <div class="col-md-7">
                    <div class="box-content">
                        <div id="scheduleCalendar"></div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="box-content">
                        <table id="tblUserInspections1" class="table-calender calendar-month-header">
                            <tbody>
                                <tr>
                                    <th colspan="2" style="background-color: #248dc1; font-size: 15px; height: 47px;" align="center"><span runat="server" id="InspectionDate"></span></th>
                                </tr>
                            </tbody>
                        </table>
                        <div style="height: 381px; overflow-x: none; overflow-y: auto;">
                            <table id="tblUserInspections" class="table-calender">
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../Design/assets/zabuto-calendar/js/zabuto_calendar.js"></script>
    <link href="../../Design/assets/zabuto-calendar/css/zabuto_calendar.css" rel="stylesheet" />
    <style type="text/css">
        .currentday {
            border: solid 1px #ff9b08;
        }
    </style>
    <script type="text/javascript">
        var isCurrentDateScheduleExists = false;
        $(document).ready(function () {
            $("#scheduleCalendar").zabuto_calendar(
                {
                    action: function () {
                        return LoadScheduleInspections(this.id, false);
                    },
                    action_nav: function () {
                        return LoadScheduleInspectionsOnMonthChange(this.id);
                    },
                    cell_border: true,
                    today: true,
                    UserID: $("input[id$=hdnUserID]").val(),
                    ajax: {
                        url: '<%= ResolveUrl("ScheduleCalendar.aspx/GetUserInspectionDates") %>',
                        modal: false
                    }
                });
            var currentDate = GetTodayDate();
            var UserID = $("input[id$=hdnUserID]").val();
            LoadScheduleInspection(currentDate, UserID);
        });
        function LoadScheduleInspections(id, fromModal) {
            debugger;
                var hasEvent = $("#" + id).data("hasEvent");
                if (!hasEvent) {
                    return false;
                }
                var date = $("#" + id).data("date");
                var dayId = id + '_day';

                $(".badge").contents().unwrap();
                $("#" + dayId).wrapInner('<span class="badge badge-today"></span>');

                var UserID = $("input[id$=hdnUserID]").val();
                LoadScheduleInspection(date, UserID);
            }
            function LoadScheduleInspectionsOnMonthChange(id) {
                var to = $("#" + id).data("to");
                var date = to.month + '-' + 01 + '-' + to.year;
                var UserID = $("input[id$=hdnUserID]").val();
                LoadScheduleInspection(date, UserID);
                console.log($("#" + id));
            }
            function GetInspectionDate() {
                return $("#InspectionDate").text();
            }
            function LoadScheduleInspection(date, UserID) {
                var dataToPost = {
                    _Date: date,
                    _UserID: UserID
                };

                $("#InspectionDate").text(GetDateToDisplay(new Date(date)));
                $("input[id$=hdDateTime]").val(GetDateToDisplay(new Date(date)))
                jQuery("#tblUserInspections tbody tr").remove();

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("ScheduleCalendar.aspx/GetUserInspectionsByDate") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataToPost),
                    success: function (data) {
                        if (data.d != "") {
                            jQuery("#tblUserInspections tbody").append(data.d);
                            if (date == GetTodayDate()) {
                                $(".currentday").wrapInner('<span class="badge badge-today"></span>');
                            }
                        }
                        else {
                            jQuery("#tblUserInspections tbody").append("<tr><td colspan='3'>No Inspection found</td></tr>");
                        }
                    }
                });
            }
    </script>
    <!-- END Main Content -->
</asp:Content>
