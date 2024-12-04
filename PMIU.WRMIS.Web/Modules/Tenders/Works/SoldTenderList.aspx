<%@ Page Title="SoldTenderList" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="SoldTenderList.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.Works.SoldTenderList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="ucViewWorksControl" TagName="ViewWorksUserControl" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Sold Tender List</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">

                <div class="row">
                    <div class="col-md-12">
                        <ucViewWorksControl:ViewWorksUserControl runat="server" ID="ViewWorksUserControl" />
                    </div>
                </div>
            </div>

            <%--<div class="row">--%>
            <div class="col-md-3" style="float: right;">
                <div style="float: right;">
                    <asp:LinkButton TabIndex="10" ID="btnCloseTender" runat="server" CssClass="btn btn-primary" Text="Close Tender" OnClick="btnCloseTender_Click"></asp:LinkButton>
                    <br />
                    <br />
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvSoldTenderList" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                    ShowHeaderWhenEmpty="True" AllowPaging="False" OnRowCommand="gvSoldTenderList_RowCommand" OnRowDataBound="gvSoldTenderList_RowDataBound"
                    OnRowUpdating="gvSoldTenderList_RowUpdating" OnRowCancelingEdit="gvSoldTenderList_RowCancelingEdit" OnRowEditing="gvSoldTenderList_RowEditing"
                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control required" ClientIDMode="Static" Width="70%" required="true" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" />
                                <asp:TextBox ID="txtCompanyID" runat="server" Text="-1" ClientIDMode="Static" Style="display: none;" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
                            <ItemStyle CssClass="text-left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bank Receipt">
                            <EditItemTemplate>
                                <uc1:FileUploadControl runat="server" ID="FileUpload" Size="1" Name="GridCtrl" />
                                <asp:HyperLink ID="hlBankRecieptLnk" CssClass="" Visible="false" runat="server" Text="abc" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAttachment" runat="server" CssClass="control-label" Text='<%# Eval("BankReceipt")%>' Visible="false"></asp:Label>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Name="FormCtrl" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="col-md-5" />
                            <ItemStyle CssClass="text-left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                    <asp:Button ID="btnSoldTenderGrid" runat="server" Text="" CommandName="AddSoldTenderItem" ToolTip="Add" CssClass="btn btn-success btn_add plus" OnClientClick="RemoveFormRequired()" Enabled="<%# GetEnableValue() %>" />
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlActionAdvertisement" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEditSoldTenderGrid" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditActionAdvertisement" runat="server" HorizontalAlign="Center">
                                    <asp:Button runat="server" ID="btnSaveSoldTenderItem" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                    <asp:Button ID="lbtnCancelSoldTender" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
            <%--  </div>--%>
            <div class="row" runat="server" id="divSave">
                <div class="col-md-12">
                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                </div>
            </div>

            <div id="CloseTender" class="modal fade">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="box">
                            <div class="box-title">
                                <h5>Close Tender</h5>
                            </div>
                            <div class="modal-body">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-lg-1 control-label">Reason</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <asp:TextBox ID="txtReason" required="required" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control commentsMaxLengthRow multiline-no-resize txtReason required" TabIndex="5" MaxLength="250" Enabled="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-lg-1 control-label">Upload</label>
                                                <div class="col-sm-6 col-lg-7 controls">
                                                    <uc1:FileUploadControl runat="server" ID="FileUpload1" Size="1" Name="FormCtrl" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                    <asp:Button TabIndex="10" ID="LinkButton" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal -->

            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content" style="height: 350px;">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Add Contractor</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Company Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%--<input id="txtCompanyNameP" type="text" class="form-control txtCompanyNameP required" maxlength="90" ><div class="check" id="CompanyNameDiv" style="color: red;" />--%>
                                                <input id="txtCompanyNameP" type="text" class="form-control txtCompanyNameP required" maxlength="90" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Contact Person</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%--<input id="txtContactPersonP" type="text" class="form-control txtContactPersonP required" maxlength="90" minlength="3"><div class="check" id="ContactPersonDiv" style="color: red;" />--%>
                                                <input id="txtContactPersonP" type="text" class="form-control txtContactPersonP required" maxlength="90" minlength="3" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Contact Number</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%--<input id="txtContactNumberP" type="number" class="form-control txtContactNumberP required" maxlength="15"><div class="check" id="ContactNumberDiv" style="color: red;" />--%>
                                                <input id="txtContactNumberP" type="tel" class="form-control txtContactNumberP phoneNoInput required" maxlength="15" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Address</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <%--<input id="txtAddressP" type="text" class="form-control txtAddressP required" maxlength="90"><div class="check" id="AddressDiv" style="color: red;" />--%>
                                                <input id="txtAddressP" type="text" class="form-control txtAddressP required" maxlength="90" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <%--<div class="check" id="check" style="color: red;" />--%>
                                <%-- <div class="check" id="check" style="color: red;"></div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="border-top: none;">
                        <button type="button" class="btn btn-primary" onclick="SaveNewContractor()">Save</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnTenderWorkID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTenderNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnWorkStatusID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsubmitted" runat="server" Value="false" />

    <style>
        .chosen-container.chosen-container-multi {
            width: 60% !important; /* or any value that fits your needs */
        }
    </style>

    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: scroll;
            overflow-x: hidden;
        }
    </style>
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <script src="/Scripts/jquery.mcautocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {


            var columns = [{ name: 'ContractorID', minWidth: '60px', valueField: 'ContractorID', isVisible: 'false', hasTooltip: 'false' },
                           { name: 'CompanyName', minWidth: '160px', valueField: 'CompanyName', isVisible: 'true', hasTooltip: 'false' }];





            $("#txtCompanyName").mcautocomplete({
                // These next two options are what this plugin adds to the autocomplete widget.
                showHeader: false,
                columns: columns,

                // Event handler for when a list item is selected.
                select: function (event, ui) {
                    if (ui.item && ui.item.CompanyName != 'Click to add new Contractor/Company.') {
                        $('#txtCompanyID').val(ui.item.ContractorID);
                        $('#txtCompanyName').val(ui.item.CompanyName);
                    }
                    else {
                        var CName = $('#txtCompanyName').val();
                        //console.log(CName);
                        $('.txtCompanyNameP').val(CName);
                        //$('.txtContactPersonP').val("");
                        $('.txtCompanyNameP').attr("required", "required");
                        $('.txtContactNumberP').attr("required", "required");
                        $('.txtAddressP').attr("required", "required");
                        $('.txtContactPersonP').attr("required", "required");
                        //$('.txtContactNumberP').val("");
                        //$('.txtAddressP').val("");
                        $("#myModal").modal();
                        //$('#txtCompanyID').val("-1");
                        //$('#txtCompanyName').val("");
                    }
                    return false;
                },
                // The rest of the options are for configuring the ajax webservice call.
                minLength: 1,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("SoldTenderList.aspx/GetContractors") %>',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: '{_Name: "' + request.term + '" }',
                        // The success event handler will display "No match found" if no items are returned.
                        success: function (data) {
                            var result;
                            if (!data || data.length === 0 || !data.d || data.d.length === 0) {
                                result = [{
                                    ContractorID: '',
                                    CompanyName: 'Click to add new Contractor/Company.'
                                }];
                            } else {
                                result = data.d;
                            }
                            response(result);
                        }
                    });
                }
            });

            $('#txtCompanyName').on('input', function () {

                if ($('#txtCompanyID').val() != "-1") {
                    $('#txtCompanyName').val("");
                }

                $('#txtCompanyID').val("-1");

            });

            $('#myModal').on('hidden.bs.modal', function () {
                //console.log("called");
                $('.txtCompanyNameP').removeAttr("required");
                $('.txtContactNumberP').removeAttr("required");
                $('.txtAddressP').removeAttr("required");
                $('.txtContactPersonP').removeAttr("required");
            });
            $('#txtCompanyName').on('focusout', function () {

                if ($('#txtCompanyID').val() == "-1") {
                    $('#txtCompanyName').val("");
                }

            });

            //$("input:checkbox").click(function (e) {

            //    if ($(this).is(":checked")) {

            //        if (confirm("Are you sure, you want to close the work/tender?")) {

            //            $('.txtReason').attr("required", "required");
            //            $('.txtReason').removeAttr("disabled");
            //            $('.txtReason').addClass("required");
            //        }
            //        else {
            //            e.preventDefault();
            //            $('.txtReason').removeAttr("required");
            //            $('.txtReason').attr("disabled", "disabled");
            //            $('.txtReason').removeClass("required");
            //            $('.txtReason').val("");

            //        }
            //    }
            //    else {

            //        $("#ChkBoxStatus").attr('checked', false)
            //        $('.txtReason').removeAttr("required");
            //        $('.txtReason').attr("disabled", "disabled");
            //        $('.txtReason').removeClass("required");
            //        $('.txtReason').val("");

            //    }
            //    //if ($(this).is(":checked")) {
            //    //    alert("true");
            //    //} else {
            //    //    alert("false");
            //    //}
            //});







<%--            $(".chzn-select").chosen({
                max_selected_options: 1,
                no_results_text: 'Press Enter to add new entry:'
              
            });

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("SoldTenderList.aspx/GetContractors") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".chzn-select").empty();
                }
            }).done(function (data) {
                //console.log(data);
                $.each(data.d, function (index, item) {
                    //console.log(item);
                    $('.chzn-select').append('<option value="' + item.id + '">' + item.name + '</option>');

                });
                $(".chzn-select").trigger("chosen:updated");
                $(".chzn-select").trigger("liszt:updated");

});


            $('.chzn-select').change(function () {
                $('#SelectedCompanyID').val($(".chzn-select option:selected").text());
                $('#SelectedCompanyText').val($(".chzn-select option:selected").val());
            });

            


            // cache the select element as we'll be using it a few times
            var select = $(".chzn-select");
           // console.log(select);
            // init the chosen plugin
            
            // get the chosen object
            var chosen = select.data('chosen');
            //console.log(chosen);
            // Bind the keyup event to the search box input
            $(".chosen-container-multi :input").on('keyup', function (e) {
                // if we hit Enter and the results list is empty (no matches) add the option
                if (e.which == 13 && $(".chosen-container-multi").find('li.no-results').length > 0) {
                    //var option = $("<option>").val(this.value).text(this.value);
                    $('.txtCompanyNameP').val(this.value);
                    $("#myModal").modal();
                }
            });--%>




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

        function AddFormRequired() {
            $('.FormCtrl0').attr("required", "required");
            $('.GridCtrl0').attr("required", "required");
        }
        function RemoveFormRequired() {
            $('.FormCtrl0').removeAttr("required");
            $('.txtReason').removeAttr("required");
            $('.GridCtrl0').removeAttr("required");
        }

        function AddGridRequired() {
            $('.GridCtrl0').attr("required", "required");
        }
        function RemoveGridRequired() {
            $('.GridCtrl0').removeAttr("required");
        }

        function RemoveFormRequiredforadd() {
            $('.FormCtrl0').removeAttr("required");
            $('.txtReason').removeAttr("required");
        }

        function SaveNewContractor() {
            var Validated = true;
            var CompanyName = document.getElementById("txtCompanyNameP");
            var ContactPerson = document.getElementById("txtContactPersonP");
            var ContactNumber = document.getElementById("txtContactNumberP");
            var Address = document.getElementById("txtAddressP");

            if (CompanyName.checkValidity() == false) {
                //document.getElementById("CompanyNameDiv").innerHTML = CompanyName.validationMessage;
                //setTimeout(function () {
                //    document.getElementById("CompanyNameDiv").innerHTML = "";
                //}, 2000);
                alert("Invalid company name");
                Validated = false;
            }

            if (ContactPerson.checkValidity() == false) {
                //document.getElementById("ContactPersonDiv").innerHTML = ContactPerson.validationMessage;
                //setTimeout(function () {
                //    document.getElementById("ContactPersonDiv").innerHTML = "";
                //}, 2000);
                alert("Invalid contact person detail");
                Validated = false;
            }

            if (ContactNumber.checkValidity() == false) {
                //document.getElementById("ContactNumberDiv").innerHTML = ContactNumber.validationMessage;
                //setTimeout(function () {
                //    document.getElementById("ContactNumberDiv").innerHTML = "";
                //}, 2000);
                alert("Invalid contact number");
                Validated = false;
            }

            if (Address.checkValidity() == false) {
                //document.getElementById("AddressDiv").innerHTML = Address.validationMessage;
                //setTimeout(function () {
                //    document.getElementById("AddressDiv").innerHTML = "";
                //}, 2000);
                alert("Invalid address");
                Validated = false;
            }

            if (Validated) {
                var C = $('.txtCompanyNameP').val();
                var CP = $('.txtContactPersonP').val();
                var CN = $('.txtContactNumberP').val();
                var CA = $('.txtAddressP').val();

                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("SoldTenderList.aspx/SaveContractorNew") %>',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{_CompanyName: "' + C + '" , _ContactPerson: "' + CP + '" , _Number: "' + CN + '", _Address: "' + CA + '" }',
                    // The success event handler will display "No match found" if no items are returned.
                    success: function (data) {
                        //console.log(data);
                        if (data.d == 0) {
                            alert("Record not saved");
                            //document.getElementById("check").style.color = "Red";
                            //document.getElementById("check").innerHTML = "Record not Saved!";
                            //setTimeout(function () {
                            //    document.getElementById("check").innerHTML = "";
                            //}, 1000);
                        }
                        else {
                            var value = $('.txtCompanyNameP').val();

                            //Chosen section start
                            //$('.chzn-select').append('<option value="' + data.d + '">' + value + '</option>');
                            //var Select = $('.chzn-select').append('<option value="' + data.d + '">' + value + '</option>');

                            //var option = $("<option>").val(data.d).text(value);

                            //// add the new option
                            //$('.chzn-select').prepend(option);
                            //// automatically select it
                            //$('.chzn-select').find(option).prop('selected', true);

                            //$(".chzn-select").trigger("chosen:updated");
                            //$(".chzn-select").trigger("liszt:updated");

                            //$('#SelectedCompanyID').val(data.d);
                            //$('#SelectedCompanyText').val(value);

                            //chosen end
                            $('#txtCompanyName').val(value);
                            $('#txtCompanyID').val(data.d);

                            //document.getElementById("check").style.color = "Green";
                            //document.getElementById("check").innerHTML = "Record Saved Successfully!";

                            setTimeout(function () {
                                //document.getElementById("check").innerHTML = "";
                                $('#myModal').modal('hide');
                                alert("Record saved successfully");
                            }, 1000);
                        }

                    }
                });
            }
        }
    </script>
</asp:Content>
