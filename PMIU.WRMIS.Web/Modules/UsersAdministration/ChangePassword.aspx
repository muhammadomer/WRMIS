<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.ChangePassword" %>

<%@ MasterType VirtualPath="~/Site.Master" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Change Password</asp:Literal></h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Current Password</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtoldPassword"  class="form-control required" runat="server"  autofocus="autofocus" required="true" type="Password" MaxLength="12" oninput="ValidateCurrentPassword(this)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row" id="divPassword" runat="server">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">New Password</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtPassword" ClientIDMode="Static" class="form-control required" runat="server"  required="true" type="Password" MaxLength="12" oninput="ValidatePassword(this)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Confirm New Password</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" class="form-control required" runat="server"  required="true" type="Password" MaxLength="12" oninput="ValidateConfirmPassword(this)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" ToolTip="Save" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type='text/javascript'>

        $(document).ready(function () {
            var txtPassword = document.getElementById('txtPassword');
            ValidatePassword(txtPassword);
            if (txtPassword.value.length >= 8) {
                ValidateConfirmPassword(document.getElementById('txtConfirmPassword'));
            }
        });

        function ValidateCurrentPassword(input) {
            if (input.value.length >= 8) {
                if ($("input[name$='txtoldPassword']").val().indexOf(' ') >= 0) {
                    input.setCustomValidity('Password should not contain spaces');
                }
                else {
                    // input is valid -- reset the error message
                    input.setCustomValidity('');
                }
          
            }
            //else {
            //    input.setCustomValidity('Password must have eight characters');
                
            //}
        }
        function ValidatePassword(input) {
            if (input.value.length >= 8) {
                if (($("input[name$='txtPassword']").val() != $("input[name$='txtConfirmPassword']").val()) && $("input[name$='txtConfirmPassword']").val().length > 0) {
                    input.setCustomValidity('New Password must be matching with Confirm New Password');
                }
                else if ($("input[name$='txtPassword']").val().indexOf(' ') >= 0) {
                    input.setCustomValidity('Password should not contain spaces');
                }
                else {
                    // input is valid -- reset the error message
                    input.setCustomValidity('');
                }
                $("input[name$='txtConfirmPassword']").get(0).setCustomValidity('');
            }
            else {
                input.setCustomValidity('Password must have eight characters');
                $("input[name$='txtConfirmPassword']").get(0).setCustomValidity('');
            }
        }
        function ValidateConfirmPassword(input) {
            if (input.value.length >= 8) {
                if (($("input[name$='txtPassword']").val() != $("input[name$='txtConfirmPassword']").val()) && $("input[name$='txtPassword']").val().length > 0) {
                    input.setCustomValidity('Confirm New Password must be matching with New Password');
                }
                else if ($("input[name$='txtPassword']").val().indexOf(' ') >= 0) {
                    input.setCustomValidity('Password should not contain spaces');
                }
                else {
                    // input is valid -- reset the error message
                    input.setCustomValidity('');
                }
                $("input[name$='txtPassword']").get(0).setCustomValidity('');
            }
            else {
                input.setCustomValidity('Confirm New Password must be matching with New Password');
                $("input[name$='txtPassword']").get(0).setCustomValidity('');
            }
        }

    </script>
</asp:Content>
