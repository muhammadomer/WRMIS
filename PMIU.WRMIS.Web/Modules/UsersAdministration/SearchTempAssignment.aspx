<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchTempAssignment.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.TemporaryAssignment" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Search Temporary Assignments</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Delegating From</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDelegatingFrom" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                <asp:TextBox ID="txtFromID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Delegating To</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtDelegatingTo" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                <asp:TextBox ID="txtToID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">From</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control date-picker" onkeyup="return false;" onkeydown="return false;" />
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">To</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control date-picker" onkeyup="return false;" onkeydown="return false;" />
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add Temporary Assignment" CssClass="btn btn-success" ToolTip="Add Temporary Assignment" OnClick="btnAdd_Click" />
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvAssignment" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" OnRowDeleting="gvAssignment_RowDeleting" AllowPaging="True"
                    OnPageIndexChanging="gvAssignment_PageIndexChanging" CssClass="table header"
                    BorderWidth="0px" CellSpacing="-1" GridLines="None" OnPageIndexChanged="gvAssignment_PageIndexChanged"
                    OnRowDataBound="gvAssignment_RowDataBound" OnRowCreated="gvAssignment_RowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblFromName" runat="server" CssClass="control-label" Text='<%# Eval("ActingRole.UA_Users.FirstName") + " " + Eval("ActingRole.UA_Users.LastName") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblFromDesignation" runat="server" CssClass="control-label" Text='<%# Eval("ActingRole.UA_Users.UA_Designations.Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblFromLocation" runat="server" CssClass="control-label" ToolTip='<%# Eval("FromLocation") %>' Text='<%# Eval("FromLocation") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblToName" runat="server" CssClass="control-label" Text='<%# Eval("ToUser.FirstName") + " " + Eval("ToUser.LastName") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblToDesignation" runat="server" CssClass="control-label" Text='<%# Eval("ToUser.UA_Designations.Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblToLocation" runat="server" CssClass="control-label" ToolTip='<%# Eval("ToLocation") %>' Text='<%# Eval("ToLocation") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date From">
                            <ItemTemplate>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="control-label" Text='<%# Eval("ActingRole.FromDate") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date To">
                            <ItemTemplate>
                                <asp:Label ID="lblToDate" runat="server" CssClass="control-label" Text='<%# Eval("ActingRole.ToDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                    <asp:Label runat="server" ID="lblID" Visible="false" Text='<%# Eval("ActingRole.ID") %>' />
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" OnClick="btnEdit_Click" Visible="<%# base.CanEdit %>" />
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ActingRole.ID") %>' CssClass="btn btn-danger btn_32 delete" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" Visible="<%# base.CanDelete %>" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
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
                            { name: 'Location', minWidth: '140px', valueField: 'Location', isVisible: 'true', hasTooltip: 'true' }];

            $("#txtDelegatingFrom").mcautocomplete({
                // These next two options are what this plugin adds to the autocomplete widget.
                showHeader: true,
                columns: columns,

                // Event handler for when a list item is selected.
                select: function (event, ui) {
                    if (ui.item && ui.item.FullName != 'No match found.') {
                        $('#txtFromID').val(ui.item.ID);
                        $('#txtDelegatingFrom').val(ui.item.FullName);
                    }
                    else {
                        $('#txtFromID').val("-1");
                        $('#txtDelegatingFrom').val("");
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
                        $('#txtDelegatingTo').val(ui.item.FullName);
                    }
                    else {
                        $('#txtToID').val("-1");
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
            });

            $('#txtDelegatingTo').on('focusout', function () {

                if ($('#txtToID').val() == "-1") {
                    $('#txtDelegatingTo').val("");
                }

            });
        });

    </script>
</asp:Content>
