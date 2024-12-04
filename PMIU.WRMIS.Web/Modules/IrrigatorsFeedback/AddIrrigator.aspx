<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddIrrigator.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.AddIrrigator" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Add/Edit Irrigator</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" AutoPostBack="True" TabIndex="1" required="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required" AutoPostBack="True" TabIndex="2" Enabled="False" required="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" AutoPostBack="True" TabIndex="3" Enabled="False" required="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblChannelName" runat="server" Text="Channel Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlChannelName" runat="server" CssClass="form-control required" TabIndex="4" Enabled="False" required="true">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblIrrigatorName" runat="server" Text="Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtIrrigatorName" runat="server" CssClass="form-control required" TabIndex="5" MaxLength="45" onkeyup="AlphabetsWithLengthValidation(this, 3)" required="true" onfocus="this.value = this.value;" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblIrrigatorMobileNo" runat="server" Text="Mobile No. 1" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtIrrigatorMobileNo" runat="server" onkeyup="PhoneNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" ClientIDMode="Static" CssClass="form-control phoneNoInput required" TabIndex="6" required="true" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblIrrigatorMobileNo2" runat="server" Text="Mobile No. 2" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtIrrigatorMobileNo2" runat="server" onkeyup="PhoneNoWithLengthValidation(this, 11)" placeholder="XXXXXXXXXXX" autocomplete="off" ClientIDMode="Static" CssClass="form-control phoneNoInput" TabIndex="7" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Tail Side</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <label class="checkbox-inline">
                                        <input runat="server" name="checkbox" id="chkFront" type="checkbox" value="Front" tabindex="8" clientidmode="Static">
                                        Front
                                    </label>
                                    &nbsp;&nbsp;
                                    <label class="checkbox-inline">
                                        <input runat="server" name="checkbox" id="chkLeft" type="checkbox" value="Left" tabindex="9">
                                        Left
                                    </label>
                                    &nbsp;&nbsp;
                                    <label class="checkbox-inline">
                                        <input runat="server" name="checkbox" id="chkRight" type="checkbox" value="Right" tabindex="10">
                                        Right
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Status</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <label class="radio-inline">
                                        <input runat="server" id="rbStatusActive" type="radio" name="UserStatus" value="Active" tabindex="11" checked="true" required>
                                        Active
                                    </label>
                                    &nbsp;&nbsp;
                                    <label class="radio-inline">
                                        <input runat="server" id="rbStatusInactive" type="radio" name="UserStatus" tabindex="12" value="Inactive">
                                        Inactive
                                    </label>
                                    &nbsp;&nbsp;
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control multiline-no-resize" MaxLength="250" TabIndex="13" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClientClick="CheckBoxValidation()" TabIndex="14" OnClick="btnSave_Click" />
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" Text="Back" TabIndex="15" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function CheckBoxValidation() {


            if (($("#txtIrrigatorMobileNo").val() == $("#txtIrrigatorMobileNo2").val())) {
                document.getElementById("txtIrrigatorMobileNo2").setCustomValidity('Mobile No. 2 should not be same as Mobile No. 1');
            }
            else {
                if ($("#txtIrrigatorMobileNo2").val().length > 0 && $("#txtIrrigatorMobileNo2").val().length < 11) {
                    document.getElementById("txtIrrigatorMobileNo2").setCustomValidity('Mobile No. cannot be less than 11 digits');
                }
                else {
                    document.getElementById("txtIrrigatorMobileNo2").setCustomValidity("");
                }

            }

            var atLeastOneIsChecked = $('input:checkbox').is(':checked');


            if (atLeastOneIsChecked == false) {
                $('#chkFront')[0].setCustomValidity("Please check atleast one checkbox.");
            }
            else {
                $('#chkFront')[0].setCustomValidity("");
            }
        }


        $(document).ready(function () {
            var txtIrrigatorName = document.getElementById('txtIrrigatorName');
            var txtIrrigatorNumber1 = document.getElementById('txtIrrigatorMobileNo');
            AlphabetsWithLengthValidation(txtIrrigatorName, 3);
            PhoneNoWithLengthValidation(txtIrrigatorNumber1, 11)
        });
    </script>
</asp:Content>
