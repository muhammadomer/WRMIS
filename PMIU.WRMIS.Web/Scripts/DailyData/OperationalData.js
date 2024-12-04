/// <reference path="~/design/js/jquery.jqgrid.js" />
// Enum for daily data types
var dailyDataType = {
    SubDivisional: 1,
    Divisional: 2
};
var designation = {
    ChiefIrrigation: 1,
    DeputyDirector: 2,
    XEN: 3,
    SDO: 4,
    DataAnalyst: 5
}
//Enum for Session
var session = {
    Morning: 1,
    Evening: 2
};
function GetCurrentTime() {
    var today = new Date();
    var hours = today.getHours();// > 12 ? today.getHours() - 12 : today.getHours();
    var amPM = today.getHours() >= 12 ? "PM" : "AM";
    hours = hours < 10 ? "0" + hours : hours;
    var minutes = today.getMinutes() < 10 ? "0" + today.getMinutes() : today.getMinutes();
    var seconds = today.getSeconds() < 10 ? "0" + today.getSeconds() : today.getSeconds();
    var currentTime = hours + ":" + minutes;// + " " + amPM;
    return currentTime;
}
//function GetCurrentDate() {
//    var today = new Date();
//    var dd = today.getDate();
//    var mm = today.getMonth() + 1; //January is 0!
//    var yyyy = today.getFullYear();

//    if (dd < 10) {
//        dd = '0' + dd
//    }
//    if (mm < 10) {
//        mm = '0' + mm
//    }
//    var currentDate = mm + '/' + dd + '/' + yyyy;
//    return currentDate;
//}
//function GetParsedDate(dateValue, dateFormat) {
//    dpg = $.fn.datepicker.DPGlobal;
//    //date_format = 'dd-MM-yyyy';
//    var parsedDate = dpg.parseDate(dateValue, dpg.parseFormat(dateFormat), 'en');
//    return parsedDate;
//}
function EnableDisableControl(controlID, value) {
    if (value) {// Enable
        $(controlID).removeAttr('disabled');
        $(controlID).prop('disabled', false);
    }
    else if (!value) {//Disable
        $(controlID).attr('disabled', true);
        $(controlID).prop('disabled', true);
    }
}
function GetSession() {
    var time = GetCurrentTime();
    var currentDate = new Date();
    var parts = time.split(":");
    var tim = currentDate.setHours(parseInt(parts[0]), parseInt(parts[1]), 0, 0);
    // 12:01 AM to 12:00 PM
    if (tim >= new Date().setHours(00, 01, 0, 0) && tim <= new Date().setHours(12, 00, 0, 0)) {
        return session.Morning;
    }// 12:01 PM to 12:00 AM
    else if (tim >= new Date().setHours(12, 01, 0, 0) && tim <= new Date().setHours(24, 00, 0, 0)) {
        return session.Evening;
    }
}
function PreSelectSession() {
    var today = GetParsedDate(GetCurrentDate(), 'dd-MM-yyyy');
    var searchDate = GetParsedDate($("input[id$='txtDate']").val(), 'dd-MM-yyyy');
    var ddlSesseion = $("select[id$='ddlSession']");
    if (new Date(today).getTime() == new Date(searchDate).getTime()) {
        EnableDisableControl(ddlSesseion, false); //Disable session dropDowns
        ddlSesseion.val(GetSession());
        $("#hdnSession").val(GetSession());
    }
    else {
        EnableDisableControl(ddlSesseion, true); //Disable session dropDowns
        ddlSesseion.val("");
    }
}

var id = '';

var DailyDataGridColumnNames = ['Sub Division Name', 'Channel Name', 'Close', 'Reading Time', 'Gauge Type', 'R.Ds. (ft)', 'Section Name', 'Gauge Value (ft)', 'Discharge (cusec)', 'Submitted by with Designation', '', '', '', '', '', ''];
var DailyDataGridModel = [{ name: 'SubDivisionName', index: 'SubDivisionName', width: 15, resizable: false, sortable: false }
                     , { name: 'ChannelName', index: 'ChannelName', width: 25, resizable: false, sortable: false }
                     , { name: 'Close', index: 'Close', width: 10, resizable: false, sortable: false }
                     , { name: 'ReadingDateTime', index: 'ReadingDateTime', resizable: false, align: 'center', width: 13, sortable: false }
                     , { name: 'GaugeName', index: 'GaugeName', width: 16, resizable: false, sortable: false }
                     , { name: 'RDs', index: 'Rds', width: 12, align: 'center', resizable: false, sortable: false }
                     , { name: 'SectionName', index: 'SectionName', width: 20, resizable: false, sortable: false }
                     , { name: 'GaugeValue', index: 'GaugeValue', width: 12, align: 'center', resizable: false, sortable: false }
                     , { name: 'DailyDischarge', index: 'DailyDischarge', width: 12, align: 'center', resizable: false, sortable: false }
                     , { name: 'SubmittedBy', index: 'SubmittedBy', width: 20, resizable: false, sortable: false }
                     , { name: 'DailyGaugeReadingID', index: 'DailyGaugeReadingID', width: 15, edittype: 'select', formatter: ActionLinks, resizable: false, align: 'right', sortable: false }
                     , { name: 'DailyGaugeReadingID', index: 'DailyGaugeReadingID', hidden: true, resizable: false, sortable: false }
                     , { name: 'GaugePhoto', index: 'GaugePhoto', hidden: true, resizable: false, sortable: false }
                     , { name: 'GaugeID', index: 'GaugeID', hidden: true, resizable: false, sortable: false }
                     , { name: 'channelNameForAuditTrail', index: 'channelNameForAuditTrail', hidden: true, resizable: false, sortable: false }
                     , { name: 'IsCurrent', index: 'IsCurrent', width: 200, hidden: true }

];

jQuery(document).ready(function () {


    //BindOperationalDataGrid();
});



function SearchOperationalData() {

    $.jgrid.styleUI.Bootstrap.base.rowTable = "table header";

    if (designation.XEN == userDesignationID && $("select[id$='ddlSubDivision']").val() == "") {
        jQuery("#operationalDataGrid").jqGrid('showCol', ["SubDivisionName"]);
    }
    else
        jQuery("#operationalDataGrid").jqGrid('hideCol', ["SubDivisionName"]);

    $('#operationalDataGrid').trigger("reloadGrid", [{ page: 1 }]);

    BindOperationalDataGrid();
}
function BindOperationalDataGrid() {

    grid = jQuery("#operationalDataGrid"), MAX_PAGERS = 2;

    var operationalDataGrid = jQuery("#operationalDataGrid");

    operationalDataGrid.jqGrid({
        url: 'OperationalData.aspx/GetDailyGaugeReadingData',
        ajaxGridOptions: {
            contentType: 'application/json; charset=utf-8'
        , type: 'post'
        },
        datatype: "json",
        colNames: DailyDataGridColumnNames,
        colModel: DailyDataGridModel,
        sortname: '',
        height: '%100',
        rowNum: 10,
        shrinkToFit: true,
        rowList: [10, 20, 30, 40, 50],
        autowidth: true,
        viewrecords: true,
        sortorder: "",
        gridview: true,
        pager: jQuery('#pager'),
        emptyrecords: 'No record found.',
        serializeGridData: function (postData) {
            var pd = JqGridPostParams(postData);

            var ret = {};
            ret["_ZoneID"] = $("select[id$='ddlZone']").val();
            ret["_CircleID"] = $("select[id$='ddlCircle']").val();
            ret["_DivisionID"] = $("select[id$='ddlDivision']").val();
            ret["_SubDivisionID"] = $("select[id$='ddlSubDivision']").val();
            ret["_Date"] = $("input[id$='txtDate']").val();
            ret["_Session"] = $("select[id$='ddlSession']").val();

            //ret["_ZoneID"] = "6";
            //ret["_CircleID"] = "17";
            //ret["_DivisionID"] = "48";
            //ret["_SubDivisionID"] = "144";
            //ret["_Date"] = "12-Mar-2012";
            //ret["_Session"] = "12-Mar-2012";

            ret["_PageIndex"] = pd.PageIndex;
            ret["_PageSize"] = pd.PageSize;
            return JSON.stringify(ret);
        },

        jsonReader: {
            repeatitems: false,
            root: function (obj) { return obj.d.Data; },
            page: function (obj) { return obj.d.page; },
            total: function (obj) { return obj.d.total; },
            records: function (obj) { return (obj.d.Data === undefined || obj.d.Data.length === 0) ? "0" : obj.d.Data.length; }
        },
        gridComplete: function () {
            var gridName = "operationalDataGrid";

            var recs = parseInt(operationalDataGrid.getGridParam("records"), 10);
            if (isNaN(recs) || recs == 0) {
                $(".ui-jqgrid-pager").hide();
                $("#NoRecordFound").removeClass("hidden").show();
                console.log(recs);
            }
            else {
                $('.ui-jqgrid-pager').show();
                $("#NoRecordFound").addClass("hidden").hide();
            }

            TableRowSpan($('#operationalDataGrid tr:has(td)'), 0, 3);
            //$('#operationalDataGrid .deleted').remove();
        },
        rowattr: function (rd) {
            if (rd.IsCurrent == false) {
                return { "class": "myAltRowClass" };
            }
        },
        loadComplete: function () {
            console.log("load complete");
            var i, myPageRefresh = function (e) {
                var newPage = $(e.target).text();
                console.log(newPage);
                grid.trigger("reloadGrid", [{ page: newPage }]);
                e.preventDefault();
            };

            $(grid[0].p.pager + '_center td.myPager').remove();
            var pagerPrevTD = $('<td>', { class: "myPager" }), prevPagesIncluded = 0,
                pagerNextTD = $('<td>', { class: "myPager" }), nextPagesIncluded = 0,
                totalStyle = grid[0].p.pginput === false,
                startIndex = totalStyle ? this.p.page - MAX_PAGERS * 2 : this.p.page - MAX_PAGERS;
            for (i = startIndex; i <= this.p.lastpage && (totalStyle ? (prevPagesIncluded + nextPagesIncluded < MAX_PAGERS * 2) : (nextPagesIncluded < MAX_PAGERS)) ; i++) {
                if (i <= 0 || i === this.p.page) { continue; }

                var link = $('<a>', { href: '#', click: myPageRefresh });
                link.text(String(i));
                if (i < this.p.page || totalStyle) {
                    if (prevPagesIncluded > 0) { pagerPrevTD.append('<span>,&nbsp;</span>'); }
                    pagerPrevTD.append(link);
                    prevPagesIncluded++;
                } else {
                    if (nextPagesIncluded > 0 || (totalStyle && prevPagesIncluded > 0)) { pagerNextTD.append('<span>,&nbsp;</span>'); }
                    pagerNextTD.append(link);
                    nextPagesIncluded++;
                }
            }
            if (prevPagesIncluded > 0) {
                $(grid[0].p.pager + '_center td[id^="prev"]').after(pagerPrevTD);
            }
            if (nextPagesIncluded > 0) {
                $(grid[0].p.pager + '_center td[id^="next"]').before(pagerNextTD);
            }
        }
    });

    operationalDataGrid.jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: false, refresh: false });

    //$(window).on("resize", function () {
    //    var newWidth = operationalDataGrid.closest(".ui-jqgrid").parent().width();
    //    operationalDataGrid.jqGrid("setGridWidth", newWidth, true);
    //});
}
function TableRowSpan($rows, startIndex, total) {
    if (total === 0) {
        return;
    }
    var i, currentIndex = startIndex, count = 1, lst = [];
    var tds = $rows.find('td:eq(' + currentIndex + ')');
    var ctrl = $(tds[0]);
    lst.push($rows[0]);
    for (i = 1; i <= tds.length; i++) {
        if (ctrl.text() == $(tds[i]).text()) {
            count++;
            $(tds[i]).addClass('hidden');
            lst.push($rows[i]);
        }
        else {
            if (count > 1) {
                ctrl.attr('rowspan', count);
                ctrl.css('vertical-align', 'middle');
                TableRowSpan($(lst), startIndex + 1, total - 1)
            }
            count = 1;
            lst = [];
            ctrl = $(tds[i]);
            lst.push($rows[i]);
        }
    }
}
function DisableControlsForCloseChannel(cellValue, options, rowdata, action) {

}
//function Merger(gridName, CellName) { // get the display to set the interface id 
//    var Mya = $("#" + gridName + "").getDataIDs();
//    // display how many current 
//    var length = Mya.length;
//    for (var i = 0; i < length; i++) {
//        // Get a message from top to bottom 
//        var before = $("#" + gridName + "").jqGrid('getRowData', Mya[i]);
//        // define the combined number of rows
//        var rowSpanTaxCount = 1;
//        for (J = i + 1; J <= length; J++) {
//            // upper and contrast information on the merger if the value of the same number of rows + 1 and then set the current cell rowspan let hide 
//            var End = $("#" + gridName + "").jqGrid('getRowData', Mya[J]);
//            if (before[CellName] == End[CellName]) {
//                rowSpanTaxCount++;
//                $("#" + gridName + "").setCell(Mya[J], CellName, '', { display: 'none' });
//            }
//            else {
//                rowSpanTaxCount = 1;
//                BREAK;
//            }
//            $(" # " + CellName + " " + Mya[i] + "").attr("rowspan", rowSpanTaxCount);
//        }
//    }
//}
function JqGridPostParams(postData) {
    var ret = {};
    ret.PageSize = parseInt(postData.rows) || 10;
    ret.PageIndex = (postData.page - 1) * postData.rows || 0;

    //ret.IsSearch = postData._search || false;
    //ret.IsEdit = postData.oper && (postData.oper == 'edit');
    //ret.IsDelete = postData.oper && (postData.oper == 'delete');

    return ret;
}
function ActionLinks(cellValue, options, rowdata, action) {
    var today = GetParsedDate(GetCurrentDate(), 'MM-dd-yyyy');
    var searchDate = GetParsedDate($("input[id$='txtDate']").val(), 'dd-MM-yyyy');

    var disabledLink = "";
    if (rowdata.DailyGaugeReadingID == 0 || rowdata.Close == "Yes")
        disabledLink = "disabled";

    var links = '<a class="btn btn-primary btn_24 viewimg ' + disabledLink + '" role="button" href="#" onclick="ViewGaugeImage(' + options.rowId + ')"></a>&nbsp;';
    links += '<a class="btn btn-primary btn_24 audit ' + disabledLink + '" role="button" href="#" onclick="AuditTrail(' + options.rowId + ')"></a>&nbsp;';
    //If a previous day is selected along with a Session, all the relevant records will be shown in read-only mode with no Edit Icon
    if (new Date(today).getTime() == new Date(searchDate).getTime())
        links += '<a class="btn btn-primary btn_24 edit ' + disabledLink + '" role="button" href="#" onclick="EidtGauge(' + options.rowId + ')"></a>';

    return links;
}
function GetDailyDataGridColumnNames() {
    var columnNames = ['Channel Name', 'Close', 'Reading Time', 'Gauge Type', 'R.Ds. (ft)', 'Section Name', 'Gauge Value (ft)', 'Discharge (cusec)', 'Submitted by with Designation'];
    if (dailyDataType.Divisional == 1) { columnNames = columnNames.unshift('Sub Division Name'); }
    return columnNames;
}
function GetDailyDataGridModel() {
    var modelColumn = [{ name: 'ChannelName', index: 'ChannelName', width: 40 }
                     , { name: 'Close', index: 'Close', width: 10 }
                     , { name: 'ReadingDateTime', index: 'ReadingDateTime', width: 10 }
                     , { name: 'GaugeType', index: 'GaugeType', width: 20 }
                     , { name: 'RDs', index: 'Rds', width: 15 }
                     , { name: 'SectionName', index: 'SectionName', width: 30 }
                     , { name: 'GaugeValue', index: 'GaugeValue', width: 20 }
                     , { name: 'DailyDischarge', index: 'DailyDischarge', width: 20 }
                     , { name: 'SubmittedBy', index: 'SubmittedBy', width: 20 }
                     , { name: 'GaugePhoto', index: 'GaugePhoto', hidden: true }];
}
function ViewGaugeImage(rowId) {
    var rowData = $('#operationalDataGrid').jqGrid('getRowData', rowId);
    $("#gaugeImage").attr('src', rowData.GaugePhoto);
    $("#viewimage").modal('show');
}


function GetDischargeValue() {
    var ret = {};
    ret["_NewGuageValue"] = $("input[id$='txtNewGaugeValue']").val();
    ret["_GaugeID"] = $("#hdnGaugeID").val();
    ret["_ResonForChange"] = $("select[id$='ddlReasonForChange']").val();
    ret["_DailyGaugeReadingID"] = $("#hdnGaugeReadingID").val();
    $.ajax({
        url: 'OperationalData.aspx/SaveGaugeValue',
        data: JSON.stringify(ret), //'{_Date: "' + date + '", _Session: "' + session + '", _DailyGaugeReadingID: "' + 3249168 + '", _PageIndex: "' + 0 + '", _PageSize: "' + 10 + '"}',
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: EditSuccess
    });
}

function EditSuccess(Result) {
    if (Result.d == true) {
        $("#editvalue").modal('hide');
        //$("#editvalue").html("");
        $('#operationalDataGrid').trigger("reloadGrid", [{ page: 1 }]);
    }
}

function EidtGauge(rowId) {

    debugger;
    var rowData = $('#operationalDataGrid').jqGrid('getRowData', rowId);   // this is to show value from grid to pop up 
    $("#spnCurrentGaugeValue").text(rowData.GaugeValue);
    $("#hdnGaugeID").val(rowData.GaugeID);
    $("#hdnGaugeReadingID").val(rowData.DailyGaugeReadingID);
    $("#editvalue").modal('show');
}

function AuditTrail(rowId) {
    var rowData = $("#operationalDataGrid").jqGrid('getRowData', rowId);
    $("#spnChannelName").text(rowData.channelNameForAuditTrail);
    $("#spnGaugeType").text(rowData.GaugeName);
    $("#spnGaugeRD").text(rowData.RDs);
    $("#spnSection").text(rowData.SectionName);
    $("#spnSession").text($("select[id$='ddlSession']").text());
    $("#hdnAuditDailyGaugeReadingID").val(rowData.DailyGaugeReadingID);
    id = rowData.ID;


    $('#auditTrailGrid').trigger("reloadGrid", [{ page: 1 }]);

    BindAuditTrailGrid();

    $("#auditTrail").modal('show');
    return false;
}
function GetAuditTrail() {
    var ret = {};
    ret["_Date"] = $("input[id$='txtDate']").val();
    ret["_Session"] = $("select[id$='ddlSession']").val();
    ret["_DailyGaugeReadingID"] = $("#hdnAuditDailyGaugeReadingID").val();
    ret["_PageIndex"] = 0;
    ret["_PageSize"] = 10;

    $.ajax({
        url: 'OperationalData.aspx/GetAuditTrail',
        data: JSON.stringify(ret), //'{_Date: "' + date + '", _Session: "' + session + '", _DailyGaugeReadingID: "' + 3249168 + '", _PageIndex: "' + 0 + '", _PageSize: "' + 10 + '"}',
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: successFunction
    });
}
function successFunction(jsondata) {
    var thegrid = jQuery("#auditTrailGrid")[0];
    thegrid.addJSONData(jsondata.d.Data);
}
function BindAuditTrailGrid() {
    jQuery("#auditTrailGrid").jqGrid({
        datatype: GetAuditTrail,
        colNames: ['Guage Value', 'Discharge', 'Submitted By', 'Reading Time', 'Designation', 'Reason For Change', ''],//, 'Reason for Change'],
        colModel: [{ name: 'GaugeValue', index: 'GaugeValue', width: 100, align: 'center' },
        { name: 'DailyDischarge', index: 'DailyDischarge', width: 100, align: 'center' },
                    { name: 'SubmittedBy', index: 'SubmittedBy', width: 150, align: 'left' },
                    { name: 'ReadingDateTime', index: 'ReadingDateTime', width: 100, align: 'center' },
                    { name: 'Designation', index: 'Designation', width: 150, align: 'left' },
                    { name: 'ReasonForChange', index: 'ReasonForChange', width: 200, align: 'left' },
                    { name: 'IsCurrent', index: 'IsCurrent', width: 200, hidden: true }
        ],
        rowNum: 10,
        width: 550,
        rowList: [5, 10, 20, 50, 100],
        sortname: 'ID',
        pager: jQuery('#auditTrailPager'),
        sortorder: 'asc',
        viewrecords: true,
        rowattr: function (rd) {
            if (rd.IsCurrent == false) {
                return { "class": "myAltRowClass" };
            }
        }
    });
}

function SetRequiredField() {
    $('#txtNewGaugeValue').attr('required', 'required');
    $('#ddlReasonForChange').attr('required', 'required');
}