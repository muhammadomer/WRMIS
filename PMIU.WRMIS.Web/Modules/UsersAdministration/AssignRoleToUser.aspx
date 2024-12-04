<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" CodeBehind="AssignRoleToUser.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AssignRoleToUser" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3>Assign Role to Users</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">

                <div id="dual-list-box" class="form-group row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="col-lg-2 control-label">
                                <asp:Label AssociatedControlID="ddlRoles" ID="lblRoles" Text="Roles" runat="server" CssClass="control-label"></asp:Label>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlRoles" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <h4><span>Users</span></h4>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label AssociatedControlID="txtFilterUsers" ID="lblUserFilter" Text="Unassigned Users" runat="server" CssClass="control-label"></asp:Label><small> - showing <span class="unselected-count"></span></small>
                            <asp:TextBox ID="txtFilterUsers" Style="margin-bottom: 5px;" runat="server" placeholder="Filter Users" CssClass="filter filter-unselected form-control"></asp:TextBox>
                            <asp:ListBox runat="server" data-title="users" ID="lstBoxUsers" SelectionMode="Multiple" class="unselected form-control" Style="height: 200px; width: 100%;" />
                        </div>
                    </div>
                    <div style="margin-top: 210px" class="col-md-2 center-block">
                        <div class="col-md-8 col-lg-9 controls">
                            <button id="btnAdd" runat="server" style="margin-bottom: 16px;" data-type="str" class="str btn btn-default col-md-8 col-md-offset-2" type="button"><span class="glyphicon glyphicon-chevron-right"></span></button>
                        </div>
                        <div class="col-md-8 col-lg-9 controls">
                            <button id="btnRemove" runat="server" style="margin-bottom: 10px;" data-type="stl" class="stl btn btn-default col-md-8 col-md-offset-2" type="button"><span class="glyphicon glyphicon-chevron-left"></span></button>
                        </div>
                    </div>
                    <div style="margin-top: 145px" class="col-md-4">
                        <div class="col-lg-12 form-group">
                            <asp:Label AssociatedControlID="txtFilterAssignedUsers" ID="lblAssignedUsers" Text="Assigned Users" runat="server" CssClass="control-label"></asp:Label><small> - showing <span class="selected-count"></span></small>
                            <asp:TextBox ID="txtFilterAssignedUsers" Style="margin-bottom: 5px;" runat="server" placeholder="Filter Users" CssClass="filter filter-selected form-control"></asp:TextBox>
                            <asp:ListBox runat="server" required="required" ID="lstBoxAssignedUsers" ClientIDMode="Static" SelectionMode="Multiple" class="selected form-control" Style="height: 200px; width: 100%;" />
                        </div>
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary btn_t_32 tick" Text="Save" OnClientClick="SelectAll(); return;" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn hidden">Back</asp:HyperLink>

                        </div>
                    </div>
                </div>
                <hr />

            </div>
        </div>
    </div>
    <!-- END Main Content -->
    <script src="../../Scripts/dual-list-box.js"></script>
    <script type="text/javascript">
        $("select[id$='lstBoxUsers']").DualListBox();
        function SelectAll() {
            // Set all option selected to avoid validation failure
            $('#lstBoxAssignedUsers option').prop('selected', true);
        }

        $("form").submit(function (event) {
            /* stop form from submitting normally */
            event.preventDefault();
            $('#lblMsgs').html('');
            $('#lblMsgs').removeClass('ErrorMsg').removeClass('SuccessMsg');

            var usersIDs = [];
            var roleID = $("select[id$='ddlRoles']").val();
            $('#lstBoxAssignedUsers > option').each(function () {
                var $this = $(this);
                usersIDs.push($this.val());
            });

            var dataToPost = {
                _RoleID: roleID,
                _UserIDs: usersIDs
            };

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("AssignRoleToUser.aspx/AssignRoleToUsers") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(dataToPost),
                success: function (data) {
                    if (data.d == true) {
                        $('#lstBoxAssignedUsers option').prop('selected', false);

                        $('#lblMsgs').html('Successfully assigned role to users.');
                        $('#lblMsgs').addClass('SuccessMsg').show();
                    }
                    else {
                        $('#lblMsgs').addClass('ErrorMsg').show();
                        $('#lblMsgs').html('An internal server error occurred');
                    }
                }
            });
            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            return false;
        });

    </script>
</asp:Content>
