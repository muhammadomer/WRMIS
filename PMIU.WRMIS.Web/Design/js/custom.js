ClearDateField();
function ClearDateField() {
    $(".clear").click(function () {
        var parent = $(this).parent();
        var clear = parent.find('.date-picker');

        if (clear.length < 1)
            clear = parent.find('.disabled-future-date-picker');

        clear.datepicker('setDate', null);
        var input = parent.find(':input');
        input.val('');
    })
}
function GetTodayDate() {
    var today = new Date();

    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var currentDate = mm + '/' + dd + '/' + yyyy;
    return currentDate;
}

function GetCurrentDate() {
    var today = new Date();
  
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var currentDate = dd + '/' + mm + '/' + yyyy;
    return currentDate;
}
function GetParsedDate(dateValue, dateFormat) {
    dpg = $.fn.datepicker.DPGlobal;
    //date_format = 'dd-MM-yyyy';
    var parsedDate = dpg.parseDate(dateValue, dpg.parseFormat(dateFormat), 'en');
    return parsedDate;
}
function GetFormatedDate(dateValue)
{
    dpg = $.fn.datepicker.DPGlobal;
    var formatedDate = dpg.formatDate(dateValue, "dd-M-yyyy", 'en');
    return formatedDate;
}

Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
function GetDateToDisplay(date)
{
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var dd = date.getDate();
    var yyyy = date.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }
    var formatedDate = dd + '-' + monthNames[date.getMonth()] + '-' + yyyy;
    return formatedDate;
}