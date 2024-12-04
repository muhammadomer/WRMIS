<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploadControl.ascx.cs" Inherits="WebFormsTest.FileUploadControl" %>
<table id="tblFileUpload" runat="server">
</table>
<asp:HiddenField ID="AttachFileCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="FIRCtrlCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="DesnCtrlCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="AttchCtrlCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="GridCtrlCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="FormCtrlCount" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="BidCtrlCount" runat="server" ClientIDMode="Static" />

<script>
    function checkfile(name) {
        var filename = $(name).val();
        if (filename != "") {
            var lastIndex = filename.lastIndexOf("\\");
            if (lastIndex >= 0) {
                filename = filename.substring(lastIndex + 1);
            }
            var FileExt = filename.split('.').pop();

            if (FileExt == "JPG" || FileExt == "JPEG" || FileExt == "PNG" || FileExt == "GIF" ||
                FileExt == "jpg" || FileExt == "jpeg" || FileExt == "png" || FileExt == "gif" ||
                FileExt == "docx" || FileExt == "doc" || FileExt == "pdf" ||
                FileExt == "DOCX" || FileExt == "DOC" || FileExt == "PDF") {
                if (name.files[0].size > 5242880) {
                    alert("Maximum 5 MB length is allowed");
                    name.value = '';
                    return false;
                }
                if (name.className.substring(0, 9) == "CtrlClass") {
                    $('.CtrlClass0').blur();
                    $('.CtrlClass0').removeAttr('required');
                    if ($('#AttachFileCount').val() == '') {
                        $('#AttachFileCount').val(1);
                    }
                    else {
                        var count = $('#AttachFileCount').val();
                        count++;
                        $('#AttachFileCount').val(count);
                    }
                }
                else if (name.className.substring(0, 7) == "FIRCtrl") {
                    $('.FIRCtrl0').blur();
                    $('.FIRCtrl0').removeAttr('required');
                    if ($('#FIRCtrlCount').val() == '') {
                        $('#FIRCtrlCount').val(1);
                    }
                    else {
                        var count = $('#FIRCtrlCount').val();
                        count++;
                        $('#FIRCtrlCount').val(count);
                    }
                }
                else if (name.className.substring(0, 8) == "DesnCtrl") {
                    $('.DesnCtrl0').blur();
                    $('.DesnCtrl0').removeAttr('required');
                    if ($('#DesnCtrlCount').val() == '') {
                        $('#DesnCtrlCount').val(1);
                    }
                    else {
                        var count = $('#DesnCtrlCount').val();
                        count++;
                        $('#DesnCtrlCount').val(count);
                    }
                }
                else if (name.className.substring(0, 9) == "AttchCtrl") {
                    $('.AttchCtrl0').blur();
                    $('.AttchCtrl0').removeAttr('required');
                    if ($('#AttchCtrlCount').val() == '') {
                        $('#AttchCtrlCount').val(1);
                    }

                    else {
                        var count = $('#AttchCtrlCount').val();
                        count++;
                        $('#AttchCtrlCount').val(count);
                    }
                }
                else if (name.className.substring(0, 8) == "GridCtrl") {
                    $('.GridCtrl0').blur();
                    $('.GridCtrl0').removeAttr('required');
                    if ($('#GridCtrlCount').val() == '') {
                        $('#GridCtrlCount').val(1);

                    } else {
                        var count = $('#GridCtrlCount').val();
                        count++;
                        $('#GridCtrlCount').val(count);
                    }

                    return true;
                }
                else if (name.className.substring(0, 8) == "FormCtrl") {
                    $('.FormCtrl0').blur();
                    $('.FormCtrl0').removeAttr('required');
                    if ($('#FormCtrlCount').val() == '') {
                        $('#FormCtrlCount').val(1);

                    } else {
                        var count = $('#FormCtrlCount').val();
                        count++;
                        $('#FormCtrlCount').val(count);
                    }

                    return true;
                }
                else if (name.className.substring(0, 7) == "BidCtrl") {
                    $('.BidCtrl0').blur();
                    $('.BidCtrl0').removeAttr('required');
                    if ($('#BidCtrlCount').val() == '') {
                        $('#BidCtrlCount').val(1);

                    } else {
                        var count = $('#BidCtrlCount').val();
                        count++;
                        $('#BidCtrlCount').val(count);
                    }

                    return true;
                }
                else if (filename == '') {
                }
                else {
                    alert("This types of file not allowed");
                    name.value = '';
                    return false;
                }
            }
            else {
                if (filename == "") {
                    if (name.className.substring(0, 9) == "CtrlClass") {
                        var AttachFileCount = $('#AttachFileCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.CtrlClass0').blur();
                                $('.CtrlClass0').prop('required', true);
                            }
                            $('#AttachFileCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 7) == "FIRCtrl") {
                        var AttachFileCount = $('#FIRCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.FIRCtrl0').blur();
                                $('.FIRCtrl0').prop('required', true);
                            }
                            $('#FIRCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 8) == "DesnCtrl") {
                        var AttachFileCount = $('#DesnCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.DesnCtrl0').blur();
                                $('.DesnCtrl0').prop('required', true);
                            }
                            $('#DesnCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 9) == "AttchCtrl") {
                        var AttachFileCount = $('#AttchCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.AttchCtrl0').blur();
                                $('.AttchCtrl0').prop('required', true);
                            }
                            $('#AttchCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 8) == "GridCtrl") {
                        var AttachFileCount = $('#GridCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.GridCtrl0').blur();
                                $('.GridCtrl0').prop('required', true);
                            }
                            $('#GridCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 8) == "FormCtrl") {
                        var AttachFileCount = $('#FormCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.FormCtrl0').blur();
                                $('.FormCtrl0').prop('required', true);
                            }
                            $('#FormCtrlCount').val(AttachFileCount);
                        }
                    }
                    else if (name.className.substring(0, 8) == "BidCtrl") {
                        var AttachFileCount = $('#BidCtrlCount').val();
                        if (AttachFileCount > 0) {
                            AttachFileCount--;
                            if (AttachFileCount == 0) {
                                $('.BidCtrl0').blur();
                                $('.BidCtrl0').prop('required', true);
                            }
                            $('#BidCtrlCount').val(AttachFileCount);
                        }
                    }
                }
                else {
                    alert("This types of file not allowed");
                    name.value = '';
                    return false;
                }
            }
        }
    }
</script>
<style>
    .imagePreview {
        width: 88px;
        height: 50px;
        background-position: center center;
        background-size: cover;
        -webkit-box-shadow: 0 0 1px 1px rgba(0, 0, 0, .3);
        display: inline-block;
        background-image: url(../img/icons_24/tender-works.png);
    }
</style>
