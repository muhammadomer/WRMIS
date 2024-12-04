<%@ Page Title="RoleRights" Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RoleRights.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.RoleRights" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function AllTrueFalse(_ClassName) {            
            var CheckAll = 1;
            var chkBox = document.getElementsByClassName(_ClassName);
            if (chkBox.length == 0)
                CheckAll = 0;
            else {
                for (var i = 0 ; i < chkBox.length; i++) {
                    if (!chkBox[i].getElementsByTagName('input')[0].checked)
                        CheckAll = 0;
                }
            }
            return CheckAll;
        }

        function CheckAll(_From) {           
            var chkBox = document.getElementsByName("CheckBox");
            var checkAll = 1;

            if (chkBox.length > 0) {
                for (var i = 0 ; i < chkBox.length; i++) {
                    if (!chkBox[i].getElementsByTagName('input')[0].checked) {
                        checkAll = 0;
                    }
                }                
                if (checkAll == 1) {
                    var all = document.getElementsByName("AllCheckBox");
                    all[0].getElementsByTagName('input')[0].checked = true;

                    if (_From == "All") {
                        var all = document.getElementsByName("AllAddCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;

                        var all = document.getElementsByName("AllEditCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;

                        var all = document.getElementsByName("AllDeleteCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;

                        var all = document.getElementsByName("AllViewCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;
                    }
                    else if (_From == "Add") {
                        var all = document.getElementsByName("AllAddCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;
                    }
                    else if (_From == "Edit") {
                        var all = document.getElementsByName("AllEditCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;
                    }
                    else if (_From == "Delete") {
                        var all = document.getElementsByName("AllDeleteCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;
                    }
                    else if (_From == "View") {
                        var all = document.getElementsByName("AllViewCheckBox");
                        var res = all[0].getElementsByTagName('input')[0].checked = true;
                    }
                }
                else {

                    var all = document.getElementsByName("AllCheckBox");
                    all[0].getElementsByTagName('input')[0].checked = false;

                    if (_From == "All") {
                        var Res = AllTrueFalse('Add');
                        var all = document.getElementsByName("AllAddCheckBox");
                        if (Res == 1) {
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        }
                        else {
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                        }

                        var Res = AllTrueFalse('Edit');
                        var all = document.getElementsByName("AllEditCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;

                        var Res = AllTrueFalse('Delete');
                        var all = document.getElementsByName("AllDeleteCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;

                        var Res = AllTrueFalse('View');
                        var all = document.getElementsByName("AllViewCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                    }
                    else if (_From == "Add") {
                        var Res = AllTrueFalse('Add');
                        var all = document.getElementsByName("AllAddCheckBox");
                        if (Res == 1) {
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        }
                        else {
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                        }
                    }
                    else if (_From == "Edit") {
                        var Res = AllTrueFalse('Edit');
                        var all = document.getElementsByName("AllEditCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                    }
                    else if (_From == "Delete") {
                        var Res = AllTrueFalse('Delete');
                        var all = document.getElementsByName("AllDeleteCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                    }
                    else if (_From == "View") {
                        var Res = AllTrueFalse('View');
                        var all = document.getElementsByName("AllViewCheckBox");
                        if (Res == 1)
                            var res = all[0].getElementsByTagName('input')[0].checked = true;
                        else
                            var res = all[0].getElementsByTagName('input')[0].checked = false;
                    }
                }
            }
        }

        function SelectAll() {

            var all = document.getElementsByName("AllCheckBox");
            var res = all[0].getElementsByTagName('input')[0].checked;

            var chkBox = document.getElementsByName("CheckBox");
            for (var i = 0 ; i < chkBox.length; i++) {

                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }

            if (res == true) {
                var all = document.getElementsByName("AllAddCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = true;

                var all = document.getElementsByName("AllEditCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = true;

                var all = document.getElementsByName("AllDeleteCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = true;

                var all = document.getElementsByName("AllViewCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = true;
            }
            else {
                var all = document.getElementsByName("AllAddCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = false;

                var all = document.getElementsByName("AllEditCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = false;

                var all = document.getElementsByName("AllDeleteCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = false;

                var all = document.getElementsByName("AllViewCheckBox");
                var res = all[0].getElementsByTagName('input')[0].checked = false;
            }
        }

        function CheckAdd() {            
            var all = document.getElementsByName("AllAddCheckBox");
            var res = all[0].getElementsByTagName('input')[0].checked;

            var chkBox = document.getElementsByClassName("Add");
            for (var i = 0 ; i < chkBox.length; i++) {
                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }
        }

        function SelectAllAdd() {
            CheckAdd();
            CheckAll('Add');
        }

        function CheckEdit() {            
            var all = document.getElementsByName("AllEditCheckBox");
            var res = all[0].getElementsByTagName('input')[0].checked;

            var chkBox = document.getElementsByClassName("Edit");
            for (var i = 0 ; i < chkBox.length; i++) {
                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }
        }

        function SelectAllEdit() {
            CheckEdit();
            CheckAll('Edit');
        }

        function CheckDelete() {            
            var all = document.getElementsByName("AllDeleteCheckBox");
            var res = all[0].getElementsByTagName('input')[0].checked;

            var chkBox = document.getElementsByClassName("Delete");
            for (var i = 0 ; i < chkBox.length; i++) {
                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }
        }

        function SelectAllDelete() {
            CheckDelete();
            CheckAll('Delete');
        }

        function CheckView() {            
            var all = document.getElementsByName("AllViewCheckBox");
            var res = all[0].getElementsByTagName('input')[0].checked;

            var chkBox = document.getElementsByClassName("View");
            for (var i = 0 ; i < chkBox.length; i++) {
                if (res == true)
                    chkBox[i].getElementsByTagName('input')[0].checked = true;
                else
                    chkBox[i].getElementsByTagName('input')[0].checked = false;
            }
        }

        function SelectAllView() {
            CheckView();
            CheckAll('View');
        }


        $(document).ready(function () {
            CheckAll('All');
        });
    </script>

    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">--%>
    <%--<ContentTemplate>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="box">


                <div class="box-title">
                    <h3>Role Rights</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"></a>
                    </div>
                </div>

                <div class="box-content">

                    <div class="row">
                        <div class="form-horizontal">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="textfield1" class="col-sm-4 col-lg-3 control-label">Roles</label>
                                    <div id="roleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label for="textfield2" class="col-sm-4 col-lg-3 control-label">Modules</label>
                                    <div id="moduleDiv" class="col-sm-8 col-lg-9 controls" runat="server">
                                        <asp:DropDownList CssClass="form-control required" required="true" ID="ddlrModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrModule_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="False" />
                    <asp:GridView CssClass="table header" ID="gvRoleRights" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvRoleRights_PageIndexChanging" OnPageIndexChanged="gvRoleRights_PageIndexChanged"
                        OnRowDataBound="gvRoleRights_RowDataBound" OnRowCommand="gvRoleRights_RowCommand" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") == null ? "" :  Eval("ID") %>'></asp:Label>
                                    <asp:Label ID="lblPageID" runat="server" Text='<%#Eval("PageID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pages">
                                <ItemTemplate>
                                    <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Add">
                                <HeaderTemplate>
                                    <label class="checkbox-inline">
                                        <asp:CheckBox ID="AllAddCheckBox" name="AllAddCheckBox" runat="server" OnClick="SelectAllAdd();" Text="Check All Add" HorizontalAlign="Center" AutoPostBack="false" />
                                    </label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label class="checkbox-inline">
                                        <%--<asp:CheckBox ID="cbAdd" name="CheckBox" AutoPostBack="false" runat="server" Text="Add" OnClick="CheckAll();" Checked='<%# Eval("BAdd")%>' Visible='<%# Eval("AddVisible")%>' />--%>
                                        <asp:CheckBox ID="cbAdd" name="CheckBox" AutoPostBack="false" Class="Add" runat="server" Text="Add" OnClick="CheckAll('Add');" Checked='<%# Eval("BAdd") == null ? false : Eval("BAdd") %>' Visible='<%# Eval("AddVisible") == null ? false : Eval("AddVisible") %>' />
                                    </label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="true" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <label class="checkbox-inline">
                                        <asp:CheckBox ID="AllEditCheckBox" name="AllEditCheckBox" runat="server" OnClick="SelectAllEdit();" Text="Check All Edit" HorizontalAlign="Center" AutoPostBack="false" />
                                    </label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label class="checkbox-inline">
                                        <%--<asp:CheckBox ID="cbEdit" name="CheckBox" AutoPostBack="false" runat="server" Text="Edit" OnClick="CheckAll();" Checked='<%# Eval("BEdit")%>' Visible='<%# Eval("EditVisible")%>' />--%>
                                        <asp:CheckBox ID="cbEdit" name="CheckBox" AutoPostBack="false" Class="Edit" runat="server" Text="Edit" OnClick="CheckAll('Edit');" Checked='<%# Eval("BEdit") == null ? false : Eval("BEdit") %>' Visible='<%# Eval("EditVisible") == null ? false : Eval("EditVisible") %>' />
                                    </label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <label class="checkbox-inline">
                                        <asp:CheckBox ID="AllDeleteCheckBox" name="AllDeleteCheckBox" runat="server" OnClick="SelectAllDelete();" Text="Check All Delete" HorizontalAlign="Center" AutoPostBack="false" />
                                    </label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label class="checkbox-inline">
                                        <%--<asp:CheckBox ID="cbDelete" name="CheckBox" AutoPostBack="false" runat="server" Text="Delete" OnClick="CheckAll();" Checked='<%# Eval("BDelete")%>' Visible='<%# Eval("DeleteVisible")%>' />--%>
                                        <asp:CheckBox ID="cbDelete" name="CheckBox" AutoPostBack="false" Class="Delete" runat="server" Text="Delete" OnClick="CheckAll('Delete');" Checked='<%# Eval("BDelete") == null ? false : Eval("BDelete") %>' Visible='<%# Eval("DeleteVisible") == null ? false : Eval("DeleteVisible") %>' />
                                    </label>

                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <label class="checkbox-inline">
                                        <asp:CheckBox ID="AllViewCheckBox" name="AllViewCheckBox" runat="server" OnClick="SelectAllView();" Text="Check All View" HorizontalAlign="Center" AutoPostBack="false" />
                                    </label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label class="checkbox-inline">
                                        <%--<asp:CheckBox ID="cbView" name="CheckBox" AutoPostBack="false" runat="server" Text="View" OnClick="CheckAll();" Checked='<%# Eval("BView")%>' Visible='<%# Eval("ViewVisible")%>' />--%>
                                        <asp:CheckBox ID="cbView" name="CheckBox" AutoPostBack="false" Class="View" runat="server" Text="View" OnClick="CheckAll('View');" Checked='<%# Eval("BView") == null ? false : Eval("BView") %>' Visible='<%# Eval("ViewVisible") == null ? false : Eval("ViewVisible") %>' />
                                    </label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <label class="checkbox-inline">
                                        <asp:CheckBox ID="cbSelectAll" name="AllCheckBox" runat="server" OnClick="SelectAll('All');" Text="" HorizontalAlign="Center" AutoPostBack="false" />
                                        Check All
                                    </label>
                                </HeaderTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>


                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:LinkButton CssClass="btn btn-primary" ID="btnSave" formonvalidate="false" CommandName="Save" runat="server" Visible="false" OnClick="btnSave_Click" Text="Save">             
                                </asp:LinkButton>
                                <asp:Button CssClass="btn" ID="btnCancel" runat="server" Visible="false" OnClick="btnCancel_Click" Text="Reset" CommandName="Cancel" formonvalidate="false" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>--%>
    <%-- </asp:UpdatePanel>--%>
</asp:Content>
