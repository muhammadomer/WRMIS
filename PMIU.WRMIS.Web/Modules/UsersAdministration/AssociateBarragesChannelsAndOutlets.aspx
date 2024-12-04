<%@ Page Title="Associate Channel Location And Barrages" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AssociateBarragesChannelsAndOutlets.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.UsersAdministration.AssociateBarragesChannelsAndOutlets" %>

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
    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="box">

                <div class="box-title">
                    <h3>Associate Barrages, Channels and Outlets to User</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"></a>
                    </div>
                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:Table runat="server" CssClass="table tbl-info">

                            <asp:TableRow>
                                <asp:TableHeaderCell Width="33.33%">
                                    <asp:Label ID="lblFullNamelbl" runat="server" Text="Full Name"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell Width="33.33%">
                                    <asp:Label ID="lblUserNamelbl" runat="server" Text="User Name"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell Width="33.33%">
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

                                <asp:TableHeaderCell ID="thZone" Visible="false">
                                    <asp:Label ID="lblZonelbl" runat="server" Text="Zone"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell ID="thCircle" Visible="false">
                                    <asp:Label ID="lblCirclelbl" runat="server" Text="Circle"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell ID="thDivision" Visible="false">
                                    <asp:Label ID="lblDivisionlbl" runat="server" Text="Division"></asp:Label>
                                </asp:TableHeaderCell>

                            </asp:TableRow>

                            <asp:TableRow>

                                <asp:TableCell ID="tdZone" Visible="false">
                                    <asp:Label ID="lblZone" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell ID="tdCircle" Visible="false">
                                    <asp:Label ID="lblCircle" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell ID="tdDivision" Visible="false">
                                    <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>


                            <asp:TableRow>
                                <asp:TableHeaderCell ID="thSubDivision" Visible="false">
                                    <asp:Label ID="lblSubDivisionlbl" runat="server" Text="Sub Division"></asp:Label>
                                </asp:TableHeaderCell>

                                <asp:TableHeaderCell ID="thSection" Visible="false">
                                    <asp:Label ID="lblSectionlbl" runat="server" Text="Section"></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell ID="tdSubDivision" Visible="false">
                                    <asp:Label ID="lblSubDivision" runat="server"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell ID="tdSection" Visible="false">
                                    <asp:Label ID="lblSection" runat="server"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>


                        </asp:Table>
                    </div>
                    <hr />

                    <asp:Label ID="lblNotAllowed" runat="server" Visible="false"></asp:Label>

                    <div class="form-horizontal">
                        <div id="dual-list-box" class="row">
                        </div>
                    </div>



                    <b id="hBarrages" runat="server" style="margin-top: 30px;">Barrages</b>

                    <asp:GridView ID="gvBarrages" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header"
                        BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvBarrages_RowCommand"
                        OnRowUpdating="gvBarrages_RowUpdating" OnRowCancelingEdit="gvBarrages_RowCancelingEdit"
                        OnRowDataBound="gvBarrages_RowDataBound" OnRowDeleting="gvBarrages_RowDeleting"
                        OnRowEditing="gvBarrages_RowEditing" OnPageIndexChanged="gvBarrages_PageIndexChanged"
                        OnPageIndexChanging="gvBarrages_PageIndexChanging" OnRowCreated="gvBarrages_RowCreated">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Barrage">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlBarrage" runat="server" CssClass="form-control required" required="true" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Site">
                                <ItemTemplate>
                                    <asp:Label ID="lblStation" runat="server" Text='<%# Eval("StationSite") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlSites" runat="server" CssClass="form-control required" required="true" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-8" />
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
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Right">
                                        <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Right">
                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit">                                   
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete">                                    
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>

                    <b id="hChannels" runat="server" style="margin-top: 30px;">Channels</b>

                    <asp:GridView ID="gvChannels" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header"
                        BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvChannels_RowCommand" OnRowDataBound="gvChannels_RowDataBound"
                        OnRowCancelingEdit="gvChannels_RowCancelingEdit" OnRowDeleting="gvChannels_RowDeleting" OnRowEditing="gvChannels_RowEditing"
                        OnRowUpdating="gvChannels_RowUpdating" OnPageIndexChanged="gvChannels_PageIndexChanged"
                        OnPageIndexChanging="gvChannels_PageIndexChanging" OnRowCreated="gvChannels_RowCreated">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChnlID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Channel">
                                <ItemTemplate>
                                    <asp:Label ID="lblChnlName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlChannel" runat="server" CssClass="form-control required" required="true" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Gauges at RDs">
                                <ItemTemplate>
                                    <asp:Label ID="lblRD" runat="server" Text='<%# Eval("GuageRD") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlRD" runat="server" CssClass="form-control required" required="true" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-7" />
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

                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center" Style="float: right;">
                                        <asp:LinkButton ID="lbMul" runat="server" Text="" CommandName="MultiAdd" CssClass="btn btn-primary btn_add plus plus" ToolTip="Add Multiple RDs">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right; margin-right: 5px;">
                                        <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Right">
                                        <asp:LinkButton ID="lbtnEdit" runat="server" Text="" Style="float: right;" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" Style="float: right; margin-right: 5px;" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>



                    <b id="hOutlets" runat="server" style="margin-top: 30px;">Outlets</b>

                    <asp:GridView ID="gvOutlets" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                        ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header"
                        BorderWidth="0px" CellSpacing="-1" GridLines="None" OnRowCommand="gvOutlets_RowCommand"
                        OnRowCancelingEdit="gvOutlets_RowCancelingEdit" OnRowDeleting="gvOutlets_RowDeleting"
                        OnRowEditing="gvOutlets_RowEditing" OnRowDataBound="gvOutlets_RowDataBound"
                        OnRowUpdating="gvOutlets_RowUpdating" OnPageIndexChanged="gvOutlets_PageIndexChanged"
                        OnPageIndexChanging="gvOutlets_PageIndexChanging" OnRowCreated="gvOutlets_RowCreated">
                        <Columns>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutletlID" runat="server" Text='<%# Eval("ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Channel">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutletChnlName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlOutletChannel" runat="server" CssClass="form-control required" required="true" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletChannel_SelectedIndexChanged"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Outlets">
                                <ItemTemplate>
                                    <asp:Label ID="lblOutlet" runat="server" Text='<%# Eval("Outlet") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <asp:DropDownList ID="ddlOutlet" runat="server" CssClass="form-control required" required="true" AutoPostBack="true"></asp:DropDownList>

                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-7" />
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
                                    <asp:Panel ID="pnlAdd" runat="server" Style="float: right;">
                                        <asp:LinkButton ID="lbMulSlec" runat="server" Text="" CommandName="MultiAdd" CssClass="btn btn-primary btn_add plus" ToolTip="Add Multiple Outlet">
                                        </asp:LinkButton>
                                    </asp:Panel>
                                    <asp:Panel runat="server" Style="text-align: right;">
                                        <asp:Panel ID="PanelMultiSelection" runat="server" Style="float: right; margin-right: 5px;">
                                            <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add">
                                            </asp:LinkButton>

                                        </asp:Panel>

                                    </asp:Panel>
                                    <div style="clear: both"></div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Right">
                                        <asp:LinkButton ID="lbtnEdit" runat="server" Style="float: right;" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit">
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="" Style="float: right; margin-right: 5px;" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete">
                                        </asp:LinkButton>
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
                            <div class="fnc-btn" style="margin-top: 20px;">
                                <%--<label for="select" class="col-sm-4 col-lg-3 control-label"></label>--%>
                                <%--<asp:Button Text="Back" ID="btnback" runat="server" CssClass="btn" OnClick="btnback_Click" />--%>
                                <asp:LinkButton Text="Back" ID="btnback" runat="server" CssClass="btn" OnClick="btnback_Click" />
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <div id="AddMultipaleOutLetPopUp" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 68%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="lblModalSanctionStatus" runat="server" Text="Channel" CssClass="col-sm-4 col-lg-2 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlOutletChannelCopy" ClientIDMode="Static" ValidationGroup="outlet" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlOutletChannelCopy_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <br>
                                <div class="form-horizontal" id="MultipleSlection">
                                    <div id="dual-list-box-Outlet" class="form-group" style="margin-left: 64px;">
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="lblOutlet" Text="Outlets" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstOutLets" runat="server" class="form-control" Style="height: 200px"></asp:ListBox>
                                        </div>
                                        <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                            <input type="button" id="AllRight" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="right" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="left" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="AllLeft" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                        </div>
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="lblSelectedOutlets" Text="Selected Outlets" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstSelectedOutlet" runat="server" class="form-control" Style="height: 200px"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>

                                    <%--<div id="dual-list-box-Outlet" class="form-group row" style="height: 336px; margin-left: 26px;">--%>
                                    <%-- <div class="col-md-4" style="float: left;">
                                        <div class="col-lg-12 form-group">
                                            <asp:Label ID="lblOutlet" Text="Outlets" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" data-title="users" ID="lstOutLets" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 311px; margin-top: 5px; width: 130%;" />
                                        </div>
                                    </div>--%>
                                    <%--<div style="float: left; margin-top: 188px; margin-left: 40px;">
                                        <div class="col-md-8 col-lg-9 controls" style="margin-bottom: 10px;">
                                            <input type="button" id="right" value=">>" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;" />
                                        </div>
                                        <div class="col-md-8 col-lg-9 controls">
                                            <input type="button" id="left" value="<<" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;">
                                        </div>
                                    </div>--%>
                                    <%-- <div class="col-md-4">
                                        <div class="col-lg-12" style="margin-left: -69px;">
                                            <asp:Label ID="lblSelectedOutlets" Text="Selected Outlets" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" ID="lstSelectedOutlet" ValidationGroup="outlet" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 311px; margin-top: 5px; width: 130%;" />
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnRejectYes" runat="server" class="btn btn-primary" OnClientClick="SelectAll()" ValidationGroup="outlet" Text="Save" OnClick="btnSaveOutlet_Click" Width="56px"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <script type="text/javascript">
             function closeModal() {
                 $("#AddMultipaleOutLetPopUp").modal("hide");
            };
        </script>
    </div>
    <div id="AddMultipalChannelRDs" class="modal fade" style="margin-top: 100px;">
        <div class="modal-dialog" style="width: 60%;">
            <div class="modal-content">
                <div class="box">
                    <div class="box-title">
                        <h5>Add Multiple</h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <br />
                                            <asp:Label ID="Label1" runat="server" Text="Channel" CssClass="col-sm-2 col-lg-2 control-label" Style="text-align: right" />
                                            <div class="col-sm-10 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlChannelCopy" runat="server" ValidationGroup="rds" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlChannelCopy_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br>
                                <div class="form-horizontal" id="Multselection">
                                    <div id="dual-list" class="form-group" style="margin-left: 64px;">
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label2" Text="RDs" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstChannelRDs" runat="server" class="form-control" Style="height: 200px"></asp:ListBox>
                                        </div>
                                        <div class="subject-info-arrows text-center" style="margin-top: 22px;">
                                            <input type="button" id="btnAllRight" value=">>" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnRight" value=">" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnLeft" value="<" class="btn btn-default" style="margin-bottom: 5px" /><br />
                                            <input type="button" id="btnAllLeft" value="<<" class="btn btn-default" style="margin-bottom: 5px" />
                                        </div>
                                        <div class="subject-info-box-1">
                                            <asp:Label ID="Label4" Text="Selected RDs" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox ClientIDMode="Static" SelectionMode="Multiple" ID="lstSelectedChannelRDs" ValidationGroup="rds" runat="server" class="form-control" Style="height: 200px"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <%--<div id="dual-list" class="form-group row" style="height: 336px; margin-left: 26px;">
                                    <div class="col-md-4" style="float: left;">
                                        <div class="col-lg-12 form-group">
                                            <asp:Label ID="Label3" Text="Selected RDs" runat="server" CssClass="control-label"></asp:Label>
                                            <asp:ListBox runat="server" data-title="users" ID="lstChannelRDs" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 100px; margin-top: 5px; width: 50%;" />
                                        </div>
                                    </div>
                                    <div style="float: left; margin-top: 188px; margin-left: 40px;">
                                        <div class="col-md-8 col-lg-9 controls" style="margin-bottom: 10px;">
                                            <input type="button" id="cright" value=">>" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;" />
                                        </div>
                                        <div class="col-md-8 col-lg-9 controls">
                                            <input type="button" id="cleft" value="<<" class="str btn btn-default col-md-8 col-md-offset-2" style="margin-top: 10px; width: 90px;">
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="col-lg-12" style="margin-left: -69px;">
                                            <asp:ListBox runat="server" ID="lstSelectedChannelRDs" ValidationGroup="rds" ClientIDMode="Static" SelectionMode="Multiple" class="form-control" Style="height: 100px; margin-top: 5px; width: 50%;" />
                                        </div>
                                    </div>
                                </div>--%>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <%--  <asp:Button ID="btnSaveChannelRDs" runat="server" class="btn btn-primary" OnClientClick="SelectAllRDs()" ValidationGroup="rds" Text="Save" OnClick="btnSaveChannelRDs_Click"></asp:Button>--%>
                                <asp:Button ID="btnSaveChannelRDs" runat="server" class="btn btn-primary" OnClientClick="SelectAllRDs()" Text="Save" OnClick="btnSaveChannelRDs_Click" Width="56px"></asp:Button>
                                <button class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function closeModalRDs() {
                $("#AddMultipalChannelRDs").modal("hide");
            };
            //$("#left").bind("click", function () {
            //    debugger;
            //    var options = $("[id*=lstSelectedOutlet] option:selected");
            //    for (var i = 0; i < options.length; i++) {
            //        var opt = $(options[i]).clone();
            //        $(options[i]).remove();
            //        $("[id*=lstOutLets]").append(opt);
            //    }
            //});
            //$("#right").bind("click", function () {
            //    debugger;
            //    var options = $("[id*=lstOutLets] option:selected");
            //    for (var i = 0; i < options.length; i++) {
            //        var opt = $(options[i]).clone();
            //        $(options[i]).remove();
            //        $("[id*=lstSelectedOutlet]").append(opt);
            //    }
            //});

            /////////////////////////////////////
            //$("#cleft").bind("click", function () {
            //    debugger;
            //    var options = $("[id*=lstSelectedChannelRDs] option:selected");
            //    for (var i = 0; i < options.length; i++) {
            //        var opt = $(options[i]).clone();
            //        $(options[i]).remove();
            //        $("[id*=lstChannelRDs]").append(opt);
            //    }
            //});
            //$("#cright").bind("click", function () {
            //    debugger;
            //    var options = $("[id*=lstChannelRDs] option:selected");
            //    for (var i = 0; i < options.length; i++) {
            //        var opt = $(options[i]).clone();
            //        $(options[i]).remove();
            //        $("[id*=lstSelectedChannelRDs]").append(opt);
            //    }
            //});
            (function () {
                $('#btnRight').bind("click", function () {
                    var selectedOpts = $('#lstChannelRDs option:selected');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstSelectedChannelRDs').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#btnAllRight').click(function (e) {
                    var selectedOpts = $('#lstChannelRDs option');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstSelectedChannelRDs').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#btnLeft').click(function (e) {
                    var selectedOpts = $('#lstSelectedChannelRDs option:selected');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstChannelRDs').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#btnAllLeft').click(function (e) {
                    var selectedOpts = $('#lstSelectedChannelRDs option');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstChannelRDs').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
            }(jQuery));


            (function () {
                $('#right').bind("click", function () {
                    var selectedOpts = $('#lstOutLets option:selected');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstSelectedOutlet').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#AllRight').click(function (e) {
                    var selectedOpts = $('#lstOutLets option');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstSelectedOutlet').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#left').click(function (e) {
                    var selectedOpts = $('#lstSelectedOutlet option:selected');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstOutLets').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
                $('#AllLeft').click(function (e) {
                    var selectedOpts = $('#lstSelectedOutlet option');
                    if (selectedOpts.length == 0) {
                        alert("Nothing to move.");
                        e.preventDefault();
                    }
                    $('#lstOutLets').append($(selectedOpts).clone());
                    $(selectedOpts).remove();
                    e.preventDefault();
                });
            }(jQuery));
            //function closeModal() {
            //    $("#AddMultipaleOutLetPopUp").modal("hide");
            //};
        </script>
    </div>



    <script type="text/javascript">
        function SelectAllRDs() {
            $('#lstSelectedChannelRDs option').prop('selected', true);
        }

        function SelectAll() {
            $('#lstSelectedOutlet option').prop('selected', true);
        }
        $(function () {

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        //$("#left").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstSelectedOutlet] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstOutLets]").append(opt);
                        //    }
                        //});
                        //$("#right").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstOutLets] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstSelectedOutlet]").append(opt);
                        //    }
                        //});

                        (function () {
                            $('#btnRight').bind("click", function () {
                                var selectedOpts = $('#lstChannelRDs option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstSelectedChannelRDs').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#btnAllRight').click(function (e) {
                                var selectedOpts = $('#lstChannelRDs option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstSelectedChannelRDs').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#btnLeft').click(function (e) {
                                var selectedOpts = $('#lstSelectedChannelRDs option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstChannelRDs').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#btnAllLeft').click(function (e) {
                                var selectedOpts = $('#lstSelectedChannelRDs option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstChannelRDs').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                        }(jQuery));
                        //////////////////////////////////////
                        (function () {
                            $('#right').bind("click", function () {
                                var selectedOpts = $('#lstOutLets option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstSelectedOutlet').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#AllRight').click(function (e) {
                                var selectedOpts = $('#lstOutLets option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstSelectedOutlet').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#left').click(function (e) {
                                var selectedOpts = $('#lstSelectedOutlet option:selected');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstOutLets').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                            $('#AllLeft').click(function (e) {
                                var selectedOpts = $('#lstSelectedOutlet option');
                                if (selectedOpts.length == 0) {
                                    alert("Nothing to move.");
                                    e.preventDefault();
                                }
                                $('#lstOutLets').append($(selectedOpts).clone());
                                $(selectedOpts).remove();
                                e.preventDefault();
                            });
                        }(jQuery));

                        /////////////////////////////////////
                        //$("#cleft").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstSelectedChannelRDs] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstChannelRDs]").append(opt);
                        //    }
                        //});
                        //$("#cright").bind("click", function () {
                        //    debugger;
                        //    var options = $("[id*=lstChannelRDs] option:selected");
                        //    for (var i = 0; i < options.length; i++) {
                        //        var opt = $(options[i]).clone();
                        //        $(options[i]).remove();
                        //        $("[id*=lstSelectedChannelRDs]").append(opt);
                        //    }
                        //});
                    }
                });
            };
        });
    </script>
</asp:Content>
