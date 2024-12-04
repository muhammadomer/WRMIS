<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TempAssignment.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.TempAssignment" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Temporary Assignments</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Delegating From</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDelegatingFrom" runat="server" CssClass="form-control required" required="true" onfocus="this.value = this.value;" ClientIDMode="Static" />
                                <asp:TextBox ID="txtFromID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                                <asp:TextBox ID="txtFromDesignationID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Delegating To</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDelegatingTo" runat="server" CssClass="form-control required" required="true" ClientIDMode="Static" />
                                <asp:TextBox ID="txtToID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                                <asp:TextBox ID="txtToDesignationID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div id="divFrom" class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFrom" runat="server" ClientIDMode="Static" CssClass="form-control date-picker required" required="true" onkeyup="return false;" onkeydown="return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div id="divTo" class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtTo" runat="server" ClientIDMode="Static" CssClass="form-control date-picker required" required="true" onkeyup="return false;" onkeydown="return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Label ID="lblID" runat="server" Text="0" Style="display: none;" />
                            <asp:Button ID="btnAssign" runat="server" ClientIDMode="Static" Text="Assign" CssClass="btn btn-primary" ToolTip="Assign" OnClick="btnAssign_Click" />
                            <asp:LinkButton ID="lbtnBack" runat="server" Text="Back" CssClass="btn" ToolTip="Back" OnClick="lbtnBack_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: scroll;
            overflow-x: hidden;
        }
    </style>
    <script src="/Scripts/jquery.mcautocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            // Sets up the multicolumn autocomplete widget.
            var columns = [{ name: 'ID', minWidth: '60px', valueField: 'ID', isVisible: 'false', hasTooltip: 'false' },
                            { name: 'Username', minWidth: '160px', valueField: 'UserName', isVisible: 'true', hasTooltip: 'false' },
                            { name: 'Full Name', minWidth: '200px', valueField: 'FullName', isVisible: 'true', hasTooltip: 'false' },
                            { name: 'Designation', minWidth: '140px', valueField: 'Designation', isVisible: 'true', hasTooltip: 'false' },
                            { name: 'DesignationID', minWidth: '60px', valueField: 'DesignationID', isVisible: 'false', hasTooltip: 'false' },
                            { name: 'Location', minWidth: '140px', valueField: 'Location', isVisible: 'true', hasTooltip: 'true' }];

            $("#txtDelegatingFrom").mcautocomplete({
                // These next two options are what this plugin adds to the autocomplete widget.
                showHeader: true,
                columns: columns,

                // Event handler for when a list item is selected.
                select: function (event, ui) {
                    if (ui.item && ui.item.FullName != 'No match found.') {
                        $('#txtFromID').val(ui.item.ID);
                        $('#txtFromDesignationID').val(ui.item.DesignationID);
                        $('#txtDelegatingFrom').val(ui.item.FullName);
                    }
                    else {
                        $('#txtFromID').val("-1");
                        $('#txtFromDesignationID').val("-1");
                        $('#txtDelegatingFrom').val("");
                    }

                    CheckValidAssignment();

                    return false;
                },
                // The rest of the options are for configuring the ajax webservice call.
                minLength: 1,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("TempAssignment.aspx/GetUserInfo") %>',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: '{_Name: "' + request.term + '" }',
                        // The success event handler will display "No match found" if no items are returned.
                        success: function (data) {
                            var result;
                            if (!data || data.length === 0 || !data.d || data.d.length === 0) {
                                result = [{
                                    ID: '',
                                    UserName: '',
                                    FullName: 'No match found.',
                                    Designation: '',
                                    DesignationID: '',
                                    Location: ''
                                }];
                            } else {
                                result = data.d;
                            }
                            response(result);
                        }
                    });
                }
            });

            $('#txtDelegatingFrom').on('input', function () {

                if ($('#txtFromID').val() != "-1") {
                    $('#txtDelegatingFrom').val("");
                }

                $('#txtFromID').val("-1");
                $('#txtFromDesignationID').val("-1");

            });

            $('#txtDelegatingFrom').on('focusout', function () {

                if ($('#txtFromID').val() == "-1") {
                    $('#txtDelegatingFrom').val("");
                }

            });

            $("#txtDelegatingTo").mcautocomplete({
                // These next two options are what this plugin adds to the autocomplete widget.
                showHeader: true,
                columns: columns,
                position: { my: "right top", at: "right bottom" },
                // Event handler for when a list item is selected.
                select: function (event, ui) {
                    if (ui.item && ui.item.FullName != 'No match found.') {
                        $('#txtToID').val(ui.item.ID);
                        $('#txtToDesignationID').val(ui.item.DesignationID);
                        $('#txtDelegatingTo').val(ui.item.FullName);
                    }
                    else {
                        $('#txtToID').val("-1");
                        $('#txtToDesignationID').val("-1");
                        $('#txtDelegatingTo').val("");
                    }
                    return false;
                },
                // The rest of the options are for configuring the ajax webservice call.
                minLength: 1,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("SearchTempAssignment.aspx/GetUserInfo") %>',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: '{_Name: "' + request.term + '" }',
                        // The success event handler will display "No match found" if no items are returned.
                        success: function (data) {
                            var result;
                            if (!data || data.length === 0 || !data.d || data.d.length === 0) {
                                result = [{
                                    ID: '',
                                    UserName: '',
                                    FullName: 'No match found.',
                                    Designation: '',
                                    DesignationID: '',
                                    Location: ''
                                }];
                            } else {
                                result = data.d;
                            }
                            response(result);
                        }
                    });
                }
            });

            $('#txtDelegatingTo').on('input', function () {

                if ($('#txtToID').val() != "-1") {
                    $('#txtDelegatingTo').val("");
                }

                $('#txtToID').val("-1");
                $('#txtToDesignationID').val("-1");
            });

            $('#txtDelegatingTo').on('focusout', function () {

                if ($('#txtToID').val() == "-1") {
                    $('#txtDelegatingTo').val("");
                }

            });

            $('#txtFrom').on('change', function () {

                CheckValidAssignment();

            });

            $('#txtTo').on('change', function () {

                CheckValidAssignment();

            });
        });

        function CheckValidAssignment() {

            var UserID = $('#txtFromID').val().trim();
            var FromDate = $('#txtFrom').val().trim();
            var ToDate = $('#txtTo').val().trim();

            if (UserID != '-1' && FromDate != '' && ToDate != '') {

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("TempAssignment.aspx/CheckValidAssignment") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_UserID: "' + UserID + '", _FromDate: "' + FromDate + '", _ToDate: "' + ToDate + '" }',
                    success: function (data) {
                        if (data.d == 1) {
                            $('#btnAssign').attr("onclick", "return confirm('The Delegating From user has been assigned another user’s responsibilities. By delegating the responsibilities, you will delegate all his responsibilities. Do you want to continue?');");
                        }
                        else {
                            $('#btnAssign').attr("onclick", "");
                        }
                    }
                });

            }
            else {

                $('#btnAssign').attr("onclick", "");
            }
        }

    </script>
</asp:Content>
