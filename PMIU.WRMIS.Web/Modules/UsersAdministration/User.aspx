<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AddEditUser" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add User</asp:Literal></h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">First Name</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtFirstName" class="form-control required" runat="server" TabIndex="1" MaxLength="45" required="true" onfocus="this.value = this.value;" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Last Name</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtLastName" class="form-control required" runat="server" TabIndex="2" MaxLength="45" required="true" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Username</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtUserName" class="form-control required" runat="server" TabIndex="3" MaxLength="30" required="true" onblur="AlphabetsWithLengthValidations(this,3)" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Email Address</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtEmailAddress" placeholder="abc@xyz.com" TextMode="Email" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" class="form-control" runat="server" TabIndex="4" MaxLength="75" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="divPassword" runat="server">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Password</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <%--<asp:TextBox ID="txtPassword" ClientIDMode="Static" class="form-control required" runat="server" TabIndex="5" required="true" type="Password" MaxLength="12"></asp:TextBox>--%>
                                <asp:TextBox ID="txtPassword" ClientIDMode="Static" class="form-control required" runat="server" TabIndex="5" required="true" type="Password" MaxLength="12" oninput="ValidatePassword(this)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Confirm Password</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <%--<asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" class="form-control required" runat="server" TabIndex="6" required="true" type="Password" MaxLength="12"></asp:TextBox>--%>
                                <asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" class="form-control required" runat="server" TabIndex="6" required="true" type="Password" MaxLength="12" oninput="ValidateConfirmPassword(this)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Landline Number</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <%--<asp:TextBox ID="txtLandlineNumber" class="form-control" runat="server" oninvalid="setCustomValidity('Please enter valid landline number')"  Changes From Iqbal Sb
                                    onchange="try{setCustomValidity('')}catch(e){}" TabIndex="7" placeholder="XXXXXXXXXXX" pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>--%>
                                <asp:TextBox ID="txtLandlineNumber" class="form-control" runat="server" TabIndex="7" MaxLength="18"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">Mobile Number</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:TextBox ID="txtMobileNumber" class="form-control phoneNoInput required" runat="server" MaxLength="11" TabIndex="8" oninvalid="setCustomValidity('Please enter valid mobile number. e.g. 03XXXXXXXXX')"
                                    onchange="try{setCustomValidity('')}catch(e){}" required="true" placeholder="XXXXXXXXXXX" pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Organization</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control required" required="true" TabIndex="9" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Designation</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control required" required="true" Enabled="false" TabIndex="10" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">User Manager</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlManager" runat="server" CssClass="form-control required" required="true" TabIndex="11" Enabled="false">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Role</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control required" required="true" TabIndex="12"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label">User Status</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <label class="radio-inline">
                                    <input runat="server" id="rbStatusActive" type="radio" name="UserStatus" value="Active" checked="true" required>
                                    Active
                                </label>
                                &nbsp;&nbsp;<label class="radio-inline"><input runat="server" id="rbStatusInactive" type="radio" name="UserStatus" value="Inactive">
                                    Inactive
                                </label>
                                &nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Label ID="lblID" runat="server" Visible="false" Text="0" />
                            <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" ToolTip="Save" />
                            <asp:LinkButton runat="server" ID="lbtnBack" CssClass="btn" Text="Back" OnClick="lbtnBack_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type='text/javascript'>

        //$(document).ready(function () {
        //    var txtPassword = document.getElementById('txtPassword');
        //    ValidatePassword(txtPassword);
        //    if (txtPassword.value.length >= 8) {
        //        ValidateConfirmPassword(document.getElementById('txtConfirmPassword'));
        //    }
        //});

        function ValidatePassword(input) {
            if (input.value.length >= 8) {
                if (($("input[name$='txtPassword']").val() != $("input[name$='txtConfirmPassword']").val()) && $("input[name$='txtConfirmPassword']").val().length > 0) {
                    input.setCustomValidity('Password must be matching');
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
                    input.setCustomValidity('Password must be matching');
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
                input.setCustomValidity('Password must have eight characters');
                $("input[name$='txtPassword']").get(0).setCustomValidity('');
            }
        }

        function AlphabetsWithLengthValidations(input, minLength) {
            debugger;
            var str = input.value;
            //   var regex = /^[a-zA-Z]{1}/;
            //  var regex = /^[a-zA-Z0-9]+$/;
            var regex = /^[A-Za-z][a-zA-Z0-9-_.]+$/;
            var regex1 = /^[a-zA-Z0-9](.*[a-zA-Z0-9])?$/;
            //if (str.length != 0) {

            //    str = str.trim();

            //    if (str.length < parseInt(minLength)) {
            //        input.setCustomValidity("Text should have at least " + minLength + " characters.");
            //    }
            //    else if (str === "" || regex.test(str) == false) {
            //        input.setCustomValidity("First character must be alphabet.");
            //    }
            //    else {
            //        input.setCustomValidity("");
            //    }
            //}
            //else {
            //    input.setCustomValidity("");
            //}

            if (str.length != 0) {

                str = str.trim();

                if (str.length < parseInt(minLength)) {
                    input.setCustomValidity("Text should have at least " + minLength + " characters.");
                }
                else if (str === "" || regex.test(str) == false) {
                    input.setCustomValidity("Allowed Characters in Username are [Alphabets, Numbers, . and _ ]. Username must start with Alphabet and can’t end with . and _");
                }
                else if (str === "" || regex1.test(str) == false) {
                    input.setCustomValidity("Allowed Characters in Username are [Alphabets, Numbers, . and _ ]. Username must start with Alphabet and can’t end with. and _");
                }
                else {
                    input.setCustomValidity("");
                }
            }
            else {
                input.setCustomValidity("");
            }

        }

    </script>
</asp:Content>
