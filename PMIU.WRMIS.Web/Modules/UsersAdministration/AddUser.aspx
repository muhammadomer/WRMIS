<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AddUser" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>
                <asp:Literal runat="server" ID="ltlPageTitle">Add User</asp:Literal></h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div id="dvBasicInfo" runat="server">
                    <h3>User Basic Information</h3>
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
                                    <asp:TextBox ID="txtPassword" ClientIDMode="Static" class="form-control required" runat="server" TabIndex="5" required="true" type="Password" MaxLength="12" oninput="ValidatePassword(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Confirm PWD</label>
                                <div class="col-sm-8 col-lg-9 controls">
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
                    <asp:UpdatePanel runat="server">
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
                                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control required" required="true" Enabled="false" TabIndex="10">
                                                <asp:ListItem Text="Select" Value="" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Role</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control required" required="true" TabIndex="11"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Label ID="lblID" runat="server" Visible="false" Text="0" />
                                <asp:Button runat="server" ID="btnStep1Next" Text="Next" OnClick="btnStep1Next_Click" CssClass="btn btn-primary" ToolTip="Next" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div id="dvLocation" runat="server" visible="false">
                    <h3>User Location Association</h3>

                    <asp:GridView ID="gvLocation" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header"
                        BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvLocation_RowCommand"
                        OnRowCancelingEdit="gvLocation_RowCancelingEdit" OnRowEditing="gvLocation_RowEditing"
                        OnRowDataBound="gvLocation_RowDataBound" OnRowUpdating="gvLocation_RowUpdating" OnPageIndexChanged="gvLocation_PageIndexChanged"
                        OnPageIndexChanging="gvLocation_PageIndexChanging" OnRowDeleting="gvLocation_RowDeleting" OnRowCreated="gvLocation_RowCreated">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Zone">
                                <ItemTemplate>
                                    <asp:Label ID="lblZone" runat="server" Text='<%# Eval("zoneName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control required" required="required" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Circle">
                                <ItemTemplate>
                                    <asp:Label ID="lblCircle" runat="server" Text='<%# Eval("circleName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control required" required="required" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Division">
                                <ItemTemplate>
                                    <asp:Label ID="lblDivision" runat="server" Text='<%# Eval("divisionName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control required" required="required" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sub Division">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubDivision" runat="server" Text='<%# Eval("subDivisionName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlSubDivision" runat="server" CssClass="form-control required" required="required" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Section">
                                <ItemTemplate>
                                    <asp:Label ID="lblSection" runat="server" Text='<%# Eval("sectionName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-control required" required="required" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                        <asp:LinkButton ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                        <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit">                                   
                                        </asp:LinkButton>
                                        <asp:Button ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:Button>
                                    </asp:Panel>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button runat="server" ID="btnStep2Next" Text="Next" OnClick="btnStep2Next_Click" CssClass="btn btn-primary" ToolTip="Next" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div id="dvUserManagers" runat="server" visible="false">
                    <h3>User Manager</h3>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-4 col-lg-3 control-label">Select Manager</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlManager" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Label ID="Label1" runat="server" Visible="false" Text="0" />
                                <asp:Button runat="server" ID="btnFinalSave" Text="Save" OnClick="btnFinalSave_Click" CssClass="btn btn-primary" ToolTip="Save" />
                            </div>
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
