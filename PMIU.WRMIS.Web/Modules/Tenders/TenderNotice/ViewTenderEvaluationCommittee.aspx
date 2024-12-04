<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewTenderEvaluationCommittee.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.ViewTenderEvaluationCommittee" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="uc1" TagName="ViewWorks" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Tender Evaluation Committee</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <uc1:ViewWorks runat="server" ID="ViewWorks" />
            </div>
            <div class="row">
                <div class="col-md-12">
                    <%--<h5><b>Advertisement Source</b></h5>--   OnRowDataBound="gvViewTenderEvalCommittee_RowDataBound" --%>
                    <asp:GridView ID="gvViewTenderEvalCommittee" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                        ShowHeaderWhenEmpty="True" AllowPaging="False" OnRowCommand="gvViewTenderEvalCommittee_RowCommand"
                        OnRowUpdating="gvViewTenderEvalCommittee_RowUpdating" OnRowCancelingEdit="gvViewTenderEvalCommittee_RowCancelingEdit"
                        OnRowEditing="gvViewTenderEvalCommittee_RowEditing" OnRowDataBound="gvViewTenderEvalCommittee_RowDataBound"
                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" DataKeyNames="ID">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Member Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMemberName" runat="server" CssClass="form-control" ClientIDMode="Static" Width="100%" TabIndex="1" required="true" Minlength="3" MaxLength="90" Text='<%# Eval("MembersName") %>' />
                                    <asp:TextBox ID="txtMemberID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMemberName" runat="server" Text='<%# Eval("MembersName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDesignation" TabIndex="2" runat="server" CssClass="form-control required txtDesignation" required="true" Minlength="0" MaxLength="250" placeholder="Enter Designation" Text='<%# Eval("Designation") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>

                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation")%>'></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contact No.">
                                <EditItemTemplate>
                                    <%--<asp:TextBox ID="txtMobile" runat="server" CssClass="form-control required decimalInput integerInput txtMobile" required="true" Minlength="0" MaxLength="11" placeholder="Enter Mobile" Text='<%# Eval("Mobile") %>' />--%>
                                    <asp:TextBox ID="txtMobile" class="form-control required txtMobile" runat="server" TabIndex="3" required="true" placeholder="XXXXXXXXXXX" Text='<%# Eval("Mobile") %>' pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>

                                    <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile")%>'></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Email">
                                <EditItemTemplate>
                                    <%--<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control required txtEmail" required="true" Minlength="0" MaxLength="250" placeholder="Enter Email" Text='<%# Eval("Email") %>' />--%>
                                    <asp:TextBox ID="txtEmail" placeholder="abc@xyz.com" TextMode="Email" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" class="form-control required txtEmail" runat="server" TabIndex="4" Text='<%# Eval("Email") %>' MaxLength="75" required="true" />
                                </EditItemTemplate>
                                <ItemTemplate>

                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>

                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                        <asp:Button ID="btnEvalCommitteeGrid" runat="server" Text="" CommandName="AddEvalCommitteeItem" Visible="<%# base.CanAdd %>" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlActionAdvertisement" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnEditEvalCommitteeGrid" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionAdvertisement" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveEvalCommitteeItem" CommandName="Update" TabIndex="5" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelEvalCommittee" runat="server" Text="" CommandName="Cancel" TabIndex="6" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>

                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" />
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
            var columns = [{ name: 'MemberID', minWidth: '60px', valueField: 'MemberID', isVisible: 'false', hasTooltip: 'false' },
                           { name: 'MemberName', minWidth: '160px', valueField: 'MemberName', isVisible: 'true', hasTooltip: 'false' }];
            $("#txtMemberName").mcautocomplete({
                // These next two options are what this plugin adds to the autocomplete widget.
                showHeader: false,
                columns: columns,

                // Event handler for when a list item is selected.
                select: function (event, ui) {
                    if (ui.item && ui.item.MemberName != 'No Name found.') {

                        $('#txtMemberID').val(ui.item.MemberID);
                        $('#txtMemberName').val(ui.item.MemberName);
                        $('.txtDesignation').val(ui.item.Designation);
                        $('.txtMobile').val(ui.item.Mobile);
                        $('.txtEmail').val(ui.item.Email);
                    }
                    else {

                        $('#txtMemberID').val("-1");
                        $('#txtMemberName').val("");
                        $('#txtDesignation').val("");
                        $('#txtMobile').val("");
                        $('#txtEmail').val("");
                    }
                    return false;
                },
                // The rest of the options are for configuring the ajax webservice call.
                minLength: 1,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("ViewTenderEvaluationCommittee.aspx/GetCommitteeMembersAutoComplete") %>',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: '{_Name: "' + request.term + '" }',
                        // The success event handler will display "No match found" if no items are returned.
                        success: function (data) {
                            var result;
                            if (!data || data.length === 0 || !data.d || data.d.length === 0) {
                                result = [{
                                    MemberID: '',
                                    MemberName: 'No Name found.'

                                }];

                                var gv = document.getElementById("<%=gvViewTenderEvalCommittee.ClientID %>");
                                 for (var i = 0; i < gv.rows.length - 1; i++) {
                                     $("input[id*=txtDesignation]").val("");
                                     $("input[id*=txtMobile]").val("");
                                     $("input[id*=txtEmail]").val("");

                                 }

                             } else {
                                 result = data.d;
                             }
                             response(result);
                         }
                    });
                }
            });

            $('#txtMemberName').on('input', function () {

                if ($('#txtMemberID').val() != "-1") {
                    $('#txtMemberName').val("");
                }

                $('#txtMemberID').val("-1");

            });

            // $('#txtMemberName').on('focusout', function () {

            //     if ($('#txtMemberID').val() == "-1") {
            //        $('#txtMemberName').val("");
            //    }

            //});

        });



         //On UpdatePanel Refresh
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         if (prm != null) {
             prm.add_endRequest(function (sender, e) {
                 if (sender._postBackSettings.panelsToUpdate != null) {
                     InitilizeDatePickerStateOnUpdatePanelRefresh();
                 }
             });
         };


    </script>
</asp:Content>
