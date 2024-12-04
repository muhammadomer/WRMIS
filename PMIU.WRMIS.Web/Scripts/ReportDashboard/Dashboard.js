// Global variable to hold data
google.load('visualization', '1', { packages: ['corechart'] });

google.charts.load('current', { packages: ['corechart'] });

var reportTypes = {
    TailStatusFieldStaff: 1,
    TailStatusPMIUStaff: 2,
    Complaints: 3,
    WaterTheft: 4,
    PerformanceEvaluation: 5
};

$(document).ready(function () {
    DrawDashboard();

    $("#txtDateTSFS").on("changeDate", function () {
        TailStatusFieldStaffPieChart();
    });
    $("#txtDateTSPS").on("changeDate", function () {
        TailStatusPMIUStaffPieChart();
    });
    $("#txtDateCSFrom").on("changeDate", function () {
        ComplaintStatusPieChart();
    });
    $("#txtDateCSTo").on("changeDate", function () {
        ComplaintStatusPieChart();
    });
    $("#txtDateWTCFrom").on("changeDate", function () {
        WaterTheftStatuse();
    });
    $("#txtDateWTCTo").on("changeDate", function () {
        WaterTheftStatuse();
    });
    $("#ddlPE").change(function () {
        PerformanceEvaluation();
    });
    $("#ddlSession").change(function () {
        PerformanceEvaluation();
    });

    SetDatePickerRange('txtDateCSFrom', 'txtDateCSTo');
    SetDatePickerRange('txtDateWTCFrom', 'txtDateWTCTo');

});

function DrawDashboard() {
    TailStatusPMIUStaffPieChart();
    TailStatusFieldStaffPieChart();
    ComplaintStatusPieChart();
    WaterTheftStatuse();
    PerformanceEvaluation();
};

function TailStatusPMIUStaffPieChart() {
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();
    var ToDate = $("#txtDateTSPS").val();

    var DataToPost = JSON.stringify({
        _ZoneID: ZoneID == "" ? "-1" : ZoneID,
        _CircleID: CircleID == "" ? "-1" : CircleID,
        _DivisionID: DivisionID == "" ? "-1" : DivisionID,
        _ToDate: ToDate
    });
    $.ajax({
        type: "POST",
        url: 'Dashboard.aspx/GetTailStatusPMIUStaff',
        contentType: "application/json; charset=utf-8",
        data: DataToPost,
        success: function (result) {
            DrawTailStatusPMIUStaffPieChart(result.d[0]);
        },
        error: function (req, status, error) {
            //error                        
        },
        beforeSend: function () {
            // show image here
            //$("#dvLoading").text('Loading ... ');
            $("#dvLoading").show();
            //ShowErrorMsg('before');
        },
        complete: function () {
            // hide image here
            //ShowErrorMsg('complete');
            $("#dvLoading").hide();
        }
    });
};

function TailStatusFieldStaffPieChart() {
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();
    var ToDate = $("#txtDateTSFS").val();

    var DataToPost = JSON.stringify({
        _ZoneID: ZoneID == "" ? "-1" : ZoneID,
        _CircleID: CircleID == "" ? "-1" : CircleID,
        _DivisionID: DivisionID == "" ? "-1" : DivisionID,
        _ToDate: ToDate
    });
    $.ajax({
        type: "POST",
        url: 'Dashboard.aspx/GetTailStatusFieldStaff',
        contentType: "application/json; charset=utf-8",
        data: DataToPost,
        success: function (result) {
            DrawTailStatusFieldStaffPieChart(result.d[0]);
        },
        error: function (req, status, error) {
            //error                        
        },
        beforeSend: function () {
            // show image here
            //$("#dvLoading").text('Loading ... ');
            $("#dvLoading").show();
            //ShowErrorMsg('before');
        },
        complete: function () {
            // hide image here
            //ShowErrorMsg('complete');
            $("#dvLoading").hide();
        }
    });
};
function ComplaintStatusPieChart() {
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();
    var ToDate = $("#txtDateCSTo").val();
    var FromDate = $("#txtDateCSFrom").val();
    var DataToPost = JSON.stringify({
        _ZoneID: ZoneID == "" ? "-1" : ZoneID,
        _CircleID: CircleID == "" ? "-1" : CircleID,
        _DivisionID: DivisionID == "" ? "-1" : DivisionID,
        _FromDate: FromDate,
        _ToDate: ToDate
    });
    $.ajax({
        type: "POST",
        url: 'Dashboard.aspx/GetComplaintStatus',
        contentType: "application/json; charset=utf-8",
        data: DataToPost,
        success: function (result) {
            DrawComplaintStatusPieChart(result.d[0]);
        },
        error: function (req, status, error) {
            //error                        
        },
        beforeSend: function () {
            // show image here
            //$("#dvLoading").text('Loading ... ');
            $("#dvLoading").show();
            //ShowErrorMsg('before');
        },
        complete: function () {
            // hide image here
            //ShowErrorMsg('complete');
            $("#dvLoading").hide();
        }
    });
};

function WaterTheftStatuse() {
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();
    var ToDate = $("#txtDateWTCTo").val();
    var FromDate = $("#txtDateWTCFrom").val();

    var DataToPost = JSON.stringify({
        _ZoneID: ZoneID == "" ? "-1" : ZoneID,
        _CircleID: CircleID == "" ? "-1" : CircleID,
        _DivisionID: DivisionID == "" ? "-1" : DivisionID,
        _FromDate: FromDate,
        _ToDate: ToDate
    });
    $.ajax({
        type: "POST",
        url: 'Dashboard.aspx/GetWaterTheftStatuse',
        contentType: "application/json; charset=utf-8",
        data: DataToPost,
        success: function (result) {
            DrawWaterTheftStatuse(result.d[0]);
        },
        error: function (req, status, error) {
            //error                        
        },
        beforeSend: function () {
            // show image here
            //$("#dvLoading").text('Loading ... ');
            $("#dvLoading").show();
            //ShowErrorMsg('before');
        },
        complete: function () {
            // hide image here
            //ShowErrorMsg('complete');
            $("#dvLoading").hide();
        }
    });
};

function PerformanceEvaluation() {
    var Period = $("select[id$='ddlPE']").val();
    var PeriodArray = Period.split("|");
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();
    var Session = $("select[id$='ddlSession']").val();
    var FromDate = PeriodArray[0];
    var ToDate = PeriodArray[1];

    var DataToPost = JSON.stringify({
        _ZoneID: ZoneID == "" ? "-1" : ZoneID,
        _CircleID: CircleID == "" ? "-1" : CircleID,
        _DivisionID: DivisionID == "" ? "-1" : DivisionID,
        _FromDate: FromDate,
        _ToDate: ToDate,
        _Session: Session
    });

    $.ajax({
        type: "POST",
        url: 'Dashboard.aspx/GetPerformanceEvaluation',
        contentType: "application/json; charset=utf-8",
        data: DataToPost,
        success: function (result) {
            DrawPerformanceEvaluation(result.d);
        },
        error: function (req, status, error) {
            //error                        
        }
    });
};

function WrapperDrawCharts(dataList) {
    DrawTailStatusFieldStaffPieChart(dataList.FieldStaff[0]);
    DrawTailStatusPMIUStaffPieChart(dataList.PMIUStaff[0]);
    DrawComplaintStatusPieChart(dataList.ComplaintStatus[0]);
    DrawWaterTheftStatuse(dataList.WaterTheftStatuse[0]);
    DrawPerformanceEvaluation(dataList.PerformanceEvaluation);
}
function DrawTailStatusFieldStaffPieChart(dataList) {
    console.log(dataList);
    var chartData = [
      ['Authorized', dataList.Authorized],
      ['Dry', dataList.DryCount],
      ['Short', dataList.ShortCount],
      ['Excessive', dataList.Excessive]
    ];
    DrawPieChart(dataList, chartData, 'TailStatusFieldStaffChart', reportTypes.TailStatusFieldStaff);
}
function DrawTailStatusPMIUStaffPieChart(dataList) {
    console.log(dataList);
    var chartData = [
      ['Authorized', dataList.Authorized],
      ['Dry', dataList.DryCount],
      ['Short', dataList.ShortCount],
      ['Excessive', dataList.Excessive]
    ];
    DrawPieChart(dataList, chartData, 'TailStatusPMIUStaffChart', reportTypes.TailStatusPMIUStaff);
}
function DrawComplaintStatusPieChart(dataList) {
    console.log(dataList);
    var chartData = [
      ['Inbox', dataList.NewCount],
      ['InProgress', dataList.InProgressCount],
      ['Resolved', dataList.ResolvedCount]
    ];
    DrawPieChart(dataList, chartData, 'ComplaintStatusChart', reportTypes.Complaints);
    var TotalComplaints = dataList.NewCount + dataList.InProgressCount + dataList.ResolvedCount;
    $('#lblTotalComplaints').text("Total Complaints: " + TotalComplaints);
}

function DrawWaterTheftStatuse(dataList) {
    console.log(dataList);
    var chartData = [
      ['Outlet (PMIU)', dataList.OutletTemperedPMIU],
      ['Outlet (Field)', dataList.OutletTemperedField],
      ['Channel (Field)', dataList.WaterTheftCasesField],
      ['Channel (PMIU)', dataList.WaterTheftCasesPMIU]
    ];
    DrawColumnChart(chartData, 'WaterTheftChart', reportTypes.WaterTheft, '', 'Number of Cases', false, '70%', 'red', 225, 10);
}

function DrawPerformanceEvaluation(dataList) {
    console.log(dataList);
    var chartData = [];
    $.each(dataList, function (i, dataList) {
        console.log(i.ObtainedPoints);
        var ObtainedPoints = dataList.ObtainedPoints;
        var Name = dataList.Name;
        chartData.push([Name, ObtainedPoints]);
    });
    DrawColumnChart(chartData, 'PerformanceEvaluationChart', reportTypes.PerformanceEvaluation, 'Irrigation Levels', 'Scores', true, '60%', 'blue', 330, 10);
}

function DrawPieChart(dataList, chartData, ChartDivID, ReportType) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Name');
    data.addColumn('number', 'Type');
    data.addRows(chartData);

    var options = {
        //title: 'Hello World',
        //width: 375,
        //height: 425,
        // legend: { position: 'bottom', alignment: 'end' },
        pieSliceText: 'value-and-percentage'//,        
        , chartArea: { left: 0, top: 10, width: "95%", height: "95%" }
        //colors: ['86C157', '4BCAAD', '2FA3EE', '4BCAAD']
    };

    var pieChart = new google.visualization.PieChart(document.getElementById(ChartDivID));
    pieChart.draw(data, options);
    google.visualization.events.addListener(pieChart, 'select', selectHandler);
    function selectHandler() {
        var selectedItem = pieChart.getSelection()[0];
        NavigationEventHandler(selectedItem, data, ReportType);
    }
}
function NavigationEventHandler(selectedItem, data, ReportType) {
    if (selectedItem) {
        var Status = data.getValue(selectedItem.row, 0);
        //alert('The user selected ' + topping + '  ' + ReportType);
        if (ReportType == reportTypes.Complaints) {
            DisplayComplaintsStatusReportDetail(Status);
        }
        else if (ReportType == reportTypes.TailStatusFieldStaff) {
            DisplayTailStatusReportDetail(Status, ReportType);
        }
        else if (ReportType == reportTypes.TailStatusPMIUStaff) {
            DisplayTailStatusPMIUReportDetail(Status, ReportType);
        }
        else if (ReportType == reportTypes.WaterTheft) {
            DisplayWaterTheftReportDetail(Status, ReportType);
        }
    }
}
function DisplayWaterTheftReportDetail(Status) {
    var ToDate = $("#txtDateWTCTo").val();
    var FromDate = $("#txtDateWTCFrom").val();
    console.log(Status);
    var StatusID = 0;
    var ReportedBy = 0;
    if (Status == 'Outlet (Field)') {
        StatusID = 1;
        ReportedBy = 3;
    }
    else if (Status == 'Outlet (PMIU)') {
        StatusID = 2;
        ReportedBy = 1;
    }
    else if (Status == 'Channel (Field)') {
        StatusID = 3;
        ReportedBy = 3;
    }
    else if (Status == 'Channel (PMIU)') {
        StatusID = 4;
        ReportedBy = 1;
    }

    var URL = "ChartDetailedReport.aspx?ReportType=4&StatusID=" + StatusID + "&ReportedBy=" + ReportedBy + "&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
}
function DisplayComplaintsStatusReportDetail(Status) {
    var ToDate = $("#txtDateCSTo").val();
    var FromDate = $("#txtDateCSFrom").val();
    var StatusID = 0;
    if (Status == 'Inbox')
        StatusID = 1;
    else if (Status == 'InProgress')
        StatusID = 2;
    else if (Status == 'Resolved')
        StatusID = 3;

    var URL = "ChartDetailedReport.aspx?ReportType=3&StatusID=" + StatusID + "&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
}
function DisplayTailStatusReportDetail(Status, ReportType) {
    var ToDate = $("#txtDateTSFS").val();
    var FromDate = $("#txtDateTSFS").val();
    var StatusID = 0;
    if (Status == 'Dry')
        StatusID = 1;
    else if (Status == 'Short')
        StatusID = 2;
    else if (Status == 'Authorized')
        StatusID = 3;
    else if (Status == 'Excessive')
        StatusID = 4;

    var URL = "ChartDetailedReport.aspx?ReportType=" + ReportType + "&StatusID=" + StatusID + "&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
}
function DisplayTailStatusPMIUReportDetail(Status, ReportType) {
    var ToDate = $("#txtDateTSPS").val();
    var FromDate = $("#txtDateTSPS").val();
    var StatusID = 0;
    if (Status == 'Dry')
        StatusID = 1;
    else if (Status == 'Short')
        StatusID = 2;
    else if (Status == 'Authorized')
        StatusID = 3;
    else if (Status == 'Excessive')
        StatusID = 4;

    var URL = "ChartDetailedReport.aspx?ReportType=" + ReportType + "&StatusID=" + StatusID + "&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
}
function DrawColumnChart(chartData, ChartDivID, ReportType, hAxisTitle, VAxisTitle, IsslantedText, chartHeight, chartColor, height, fontSize) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Name');
    data.addColumn('number', '');
    data.addRows(chartData);

    var options = {
        // title: ComplaintName,
        //width: 375,
        height: height,
        legend: { position: 'none' },
        //bar: { groupWidth: "50%" },
        //backgroundColor: '#385723',
        pieSliceText: 'value-and-percentage',
        colors: [chartColor, chartColor],
        is3D: true,
        chartArea: {
            top: 20,
            height: chartHeight
        },
        hAxis: {
            title: hAxisTitle,
            textStyle: {
                fontSize: fontSize,
                fontName: 'Open Sans'
            }
            , slantedText: IsslantedText
        },
        vAxis: {
            title: VAxisTitle,
            textStyle: {
                fontSize: fontSize,
                fontName: 'Open Sans'
            }
        },
        annotations: {
            textStyle: {
                fontName: 'Open Sans',
                fontSize: 14,
                bold: true,
                color: '#ffffff',
            }
        },
    };

    var columnChart = new google.visualization.ColumnChart(document.getElementById(ChartDivID));
    columnChart.draw(data, options);
    google.visualization.events.addListener(columnChart, 'select', selectHandler);
    function selectHandler() {
        var selectedItem = columnChart.getSelection()[0];
        NavigationEventHandler(selectedItem, data, ReportType);
    }
}

function GetIrrigationLevels() {
    var ZoneID = $("select[id$='ddlZone']").val();
    var CircleID = $("select[id$='ddlCircle']").val();
    var DivisionID = $("select[id$='ddlDivision']").val();

    return "ZoneID=" + ZoneID + "&CircleID=" + CircleID + "&DivisionID=" + DivisionID;
}
function ShowTailStatusField() {
    var ToDate = $("#txtDateTSFS").val();
    var FromDate = $("#txtDateTSFS").val();
    var URL = "DashBoardReports.aspx?ReportType=1&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
    //window.location.href = URL;
}
function ShowTailStatusPMIU() {
    var ToDate = $("#txtDateTSPS").val();
    var FromDate = $("#txtDateTSPS").val();
    var URL = "DashBoardReports.aspx?ReportType=2&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
    //window.location.href = URL;
}
function ShowComplaints() {
    var ToDate = $("#txtDateCSTo").val();
    var FromDate = $("#txtDateCSFrom").val();
    var URL = "DashBoardReports.aspx?ReportType=3&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
    //window.location.href = URL;
}
function ShowWaterTheft() {
    var ToDate = $("#txtDateWTCTo").val();
    var FromDate = $("#txtDateWTCFrom").val();
    var URL = "DashBoardReports.aspx?ReportType=4&" + GetIrrigationLevels() + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
    window.open(URL, '_blank');
    //window.location.href = URL;
}
function ShowPerformanceEvaluation() {
    var URL = "PerformanceEvaluationReports.aspx";
    window.open(URL, '_blank');

    //window.location.href = URL;
}