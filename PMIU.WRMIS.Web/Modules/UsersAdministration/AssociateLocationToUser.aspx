<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssociateLocationToUser.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AssociateLocationToUser" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css" rel="stylesheet">
        .subject-info-box-1 {
            float: left;
            width: 40%;
        }

        select {
            height: 200px;
            padding: 0;
        }

        option {
            padding: 4px 10px 4px 10px;
        }

            option:hover {
                background: #EEEEEE;
            }

        .subject-info-arrows {
            float: left;
            width: 10%;
        }

        input {
            width: 70%;
            margin-bottom: 5px;
        }
    </style>
    <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div class="box">
        <div class="box-title">
            <h3>Associate Location to User</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"></a>
            </div>
        </div>
        <div class="box-content">

            <div class="table-responsive">

                <asp:Table runat="server" CssClass="table tbl-info">

                    <asp:TableRow>
                        <asp:TableHeaderCell>
                            <asp:Label ID="lblFullNamelbl" runat="server" Text="Full Name"></asp:Label>
                        </asp:TableHeaderCell>

                        <asp:TableHeaderCell>
                            <asp:Label ID="lblUserNamelbl" runat="server" Text="User Name"></asp:Label>
                        </asp:TableHeaderCell>

                        <asp:TableHeaderCell>
                            <asp:Label ID="lblDesignationlbl" runat="server" Text="Designation"></asp:Label>
                        </asp:TableHeaderCell>

                    </asp:TableRow>


                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFullName" runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell>
                            <asp:Label ID="lblUserName" runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell>
                            <asp:Label ID="lblDesignation" runat="server"></asp:Label>
                        </asp:TableCell>

                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableHeaderCell>
                            <asp:Label ID="lblCellNumberlbl" runat="server" Text="Cell Number"></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableRow>

                    <asp:TableRow>

                        <asp:TableCell>
                            <asp:Label ID="lblCellNumber" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
            </div>
            <hr />
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
                            <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Right">
                                <asp:LinkButton ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save">
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel">
                                </asp:LinkButton>
                            </asp:Panel>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right;">
                                <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbMulSlec" runat="server" Text="" CommandName="MultiAdd" CssClass="btn btn-primary btn_add plus" ToolTip="Add Multiple">
                                </asp:LinkButton>
                            </asp:Panel>

                            <div style="clear: both"></div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Right">
                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit">                                   
                                </asp:LinkButton>
                                <asp:Button ID="ccc" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:Button>
                            </asp:Panel>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-2" />
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Mode="NumericFirstLast" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
            <div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/UsersAdministration/SearchUser.aspx?ShowHistory=true" CssClass="btn">Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="AddMultipal" class="modal fade">
        <%--<div class="modal-dialog modal-lg" style="width: 1000px;">--%>
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Zone</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList CssClass="form-control" ID="ddlMZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMZone_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList CssClass="form-control" ID="ddlMCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label id="lblDivlbl" runat="server" class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Division</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList CssClass="form-control" ID="ddlMDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label id="lblSubdivlbl" runat="server" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList CssClass="form-control" ID="ddlMSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMSubDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-sm-4 col-lg-3 control-label" style="padding-right: 6px;">Section</label>
                                                    <div class="col-sm-8 col-lg-8 controls">
                                                        <asp:DropDownList CssClass="form-control" ID="ddlMSection" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <br>
                                        <div id="MultipleSlection" runat="server" visible="False">
                                            <div id="dual-list-box" class="form-group" style="margin-left: 80px;">
                                                <div class="subject-info-box-1">
                                                    <asp:Label ID="lblListleft" Text="" runat="server" CssClass="control-label"></asp:Label>
                                                    <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstLeft" runat="server" class="form-control" Style="height: 250px"></asp:ListBox>
                                                </div>
                                                <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                                    <input type="button" id="AllRight" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                                    <input type="button" id="right" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                                    <input type="button" id="left" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                                    <input type="button" id="AllLeft" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                                </div>
                                                <div class="subject-info-box-1">
                                                    <asp:Label ID="lblListright" Text="" runat="server" CssClass="control-label"></asp:Label>
                                                    <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstRight" runat="server" class="form-control" Style="height: 250px"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="Button1" runat="server" class="btn btn-primary" OnClientClick="SelectAll()" Text="Save" OnClick="btnSave_Click" Width="56px"></asp:Button>
                                        <button class="btn btn-default" data-dismiss="modal">Close</button>
                                    </div>
                                    <%-- <div class="modal-footer">
                                        <asp:Button ID="Button1" runat="server" class="btn btn-primary" OnClientClick="SelectAll()" Text="Save" OnClick="btnSave_Click"></asp:Button>
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-info" OnClick="btnClose_Click" formnovalidate="formnovalidate"></asp:Button>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <script type="text/javascript">
                    function closeModal() {
                        $("#AddMultipal").modal("hide");
                    };
                </script>
            </div>
        </div>
    </div>
    <%--<div class="form-horizontal" id="MultipleSlection" runat="server" visible="False">
        <div id="dual-list-box" class="form-group row" style="width: 926px; height: 500px; margin-left: 26px;">
            <div class="col-md-4" style="float: left;">
                <div class="col-lg-12 form-group">
                    <asp:Label ID="lblListleft" Text="" runat="server" CssClass="control-label"></asp:Label>
                    <asp:ListBox runat="server" data-title="users" ID="lstLeft" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 311px; margin-top: 5px; width: 130%;" />
                </div>
            </div>
            <div style="float: left; margin-top: 188px; margin-left: 40px;">
                <div class="col-md-8 col-lg-9 controls" style="margin-bottom: 10px;">
                    <input type="button" id="right" value=">>" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;" />
                </div>
                <div class="col-md-8 col-lg-9 controls">
                    <input type="button" id="left" value="<<" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;">
                </div>
            </div>
            <div class="col-md-4">
                <div class="col-lg-12" style="margin-left: -69px;">
                    <asp:Label ID="lblListright" Text="" runat="server" CssClass="control-label"></asp:Label>
                    <asp:ListBox runat="server" ID="lstRight" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 311px; margin-top: 5px; width: 130%;" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="fnc-btn">
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" ToolTip="Save" OnClick="btnSave_Click" />
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                </div>
            </div>
        </div>
    </div>--%>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">

        //$("#left").bind("click", function () {
        //    debugger;
        //    var options = $("[id*=lstRight] option:selected");
        //    for (var i = 0; i < options.length; i++) {
        //        var opt = $(options[i]).clone();
        //        $(options[i]).remove();
        //        $("[id*=lstLeft]").append(opt);
        //    }
        //});
        //$("#right").bind("click", function () {
        //    debugger;
        //    var options = $("[id*=lstLeft] option:selected");
        //    for (var i = 0; i < options.length; i++) {
        //        var opt = $(options[i]).clone();
        //        $(options[i]).remove();
        //        $("[id*=lstRight]").append(opt);
        //    }
        //});
        (function () {
            $('#right').bind("click", function () {
                var selectedOpts = $('#lstLeft option:selected');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#lstRight').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });
            $('#AllRight').click(function (e) {
                var selectedOpts = $('#lstLeft option');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#lstRight').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });
            $('#left').click(function (e) {
                var selectedOpts = $('#lstRight option:selected');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#lstLeft').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });
            $('#AllLeft').click(function (e) {
                var selectedOpts = $('#lstRight option');
                if (selectedOpts.length == 0) {
                    alert("Nothing to move.");
                    e.preventDefault();
                }
                $('#lstLeft').append($(selectedOpts).clone());
                $(selectedOpts).remove();
                e.preventDefault();
            });
        }(jQuery));

        function SelectAll() {
            $('#lstRight option').prop('selected', true);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(function () {
                        //$("#left").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstRight] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstLeft]").append(opt);
                        //    }
                        //});
                        //$("#right").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstLeft] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstRight]").append(opt);
                        //    }
                        //});
                        (function () {
                            $('#right').bind("click", function () {
                                var selectedOpts = $('#lstLeft option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstRight').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#AllRight').click(function (e) {
                                var selectedOpts = $('#lstLeft option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstRight').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#left').click(function (e) {
                                var selectedOpts = $('#lstRight option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstLeft').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#AllLeft').click(function (e) {
                                var selectedOpts = $('#lstRight option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstLeft').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                        }(jQuery));
                    });
                    $(function () {
                        $("[id*=btnSave]").bind("click", function () {
                            $("[id*=lstLeft] option").attr("selected", "selected");
                            $("[id*=lstRight] option").attr("selected", "selected");
                        });
                    });

                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };


    </script>
    <%-- <div class="form-horizontal" style="visibility: hidden;">
                <div id="dual-list-box" class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label id="lblZone" runat="server" class="col-sm-4 col-lg-3 control-label">Zone</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label id="lblCircle" runat="server" class="col-sm-4 col-lg-3 control-label">Circle</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label id="lblDivision" runat="server" class="col-sm-4 col-lg-3 control-label">Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label id="lblSubDevision" runat="server" class="col-sm-4 col-lg-3 control-label">Sub Division</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlSubDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <label id="lblSection" runat="server" class="col-sm-4 col-lg-3 control-label">Section</label>
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList CssClass="form-control" ID="ddlSection" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-4 col-lg-3 control-label" style="width: 25%;"></div>
                            <div class="col-sm-8 col-lg-9">
                                <asp:Button ID="btnAssign" Text="Assign" class="btn btn-success" runat="server" OnClick="btnAssign_Click"></asp:Button>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6" style="border-left: 1px solid #e5e5e5;">
                        <div class="form-group">
                            <asp:Label ID="lblLevelName" Text="" runat="server" class="col-md-12" Style="padding-bottom: 7px;"></asp:Label>
                            <div class="col-md-12 controls">
                                <asp:ListBox ID="lstBoxAssigned" runat="server" ClientIDMode="Static" SelectionMode="Single" CssClass="form-control" Style="height: 200px;" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Button ID="btnRemove" class="btn btn-danger" runat="server" Text="Remove" OnClick="btnRemove_Click" />
                            </div>
                        </div>

                    </div>
                </div>

                <br />
                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="col-sm-4 col-lg-3 control-label"></label>
                            <div class="col-sm-8 col-lg-9">
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/UsersAdministration/SearchUser.aspx?ShowHistory=true" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <hr />

            </div>--%>




    <%--<script src="../../Scripts/dual-list-box.js"></script>--%>

    <%-- <script type="text/javascript">
        debugger;
        $("select[id$='lstDivisions']").DualListBox();

        debugger;

        function SelectAll() {
            // Set all option selected to avoid validation failure
            $('#lstSelectedDivisions option').prop('selected', true);
        }
        $("form").submit(function (event) {
            debugger;
            /* stop form from submitting normally */
            event.preventDefault();
            $('#lblMsgs').html('');
            $('#lblMsgs').removeClass('ErrorMsg').removeClass('SuccessMsg');

            var channelsIDs = [];
            $('#lstSelectedDivisions > option').each(function () {
                var $this = $(this);
                channelsIDs.push($this.val());
            });

            var dataToPost = {
                _XChannelsIDs: channelsIDs,

            };

            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("ACCPExcludedChannels.aspx/SaveExcludeChannels") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(dataToPost),
                success: function (data) {
                    if (data.d == true) {
                        $('#lstBoxExcludeChannels option').prop('selected', false);

                        $('#lblMsgs').html('Record saved successfully.');
                        $('#lblMsgs').addClass('SuccessMsg').show();
                        setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                    }
                    else {
                        $('#lblMsgs').addClass('ErrorMsg').show();
                        $('#lblMsgs').html('An internal server error occurred');
                        setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
                    }
                }
            });
            setTimeout(function () { $("#lblMsgs").hide(); }, 5000);
            return false;
        });
    </script>--%>
</asp:Content>



