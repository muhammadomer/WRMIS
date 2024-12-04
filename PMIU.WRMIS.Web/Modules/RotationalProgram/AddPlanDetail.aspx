<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddPlanDetail.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.AddPlanDetail" %>

<%@ Import Namespace="PMIU.WRMIS.Common" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc" TagName="FileUploadControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="hName" runat="server">Add Rotational Program Detail</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="table-responsive">
                            <asp:Table runat="server" CssClass="table tbl-info">
                                <asp:TableRow>
                                    <asp:TableHeaderCell CssClass="col-md-5">
                                        <asp:Label runat="server" Text="Rotational Program"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="col-md-4">
                                        <asp:Label ID="lblNamelbl" runat="server" Text="Name"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="col-md-3">
                                        <asp:Label runat="server" Text="Season"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblRotationalProgram" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                    </asp:TableCell>

                                    <asp:TableCell>
                                        <asp:Label ID="lblSeason" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Plan Start Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Year"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="No. of Groups"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblGroups" runat="server"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>

                                <asp:TableRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Closure Start Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label runat="server" Text="Closure End Date"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell>
                                        <asp:Label ID="lblProoflbl" runat="server" Text="View/Add Attachment"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblClosureStart" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lblClosureEnd" runat="server"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableHeaderCell>
                                        <asp:HyperLink ID="hlImage" CssClass="btn btn-primary btn_24 viewimg" Visible="false" runat="server" />
                                        <uc:FileUploadControl runat="server" CssClass="CtrlClass0" required="false" ID="FileUploadControl" Size="1" />
                                    </asp:TableHeaderCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>

                        <div class="row" id="divClosure" runat="server" visible="false">
                            <div class="box-title">
                                <h4 runat="server">Closure Dates</h4>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Start Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureStartDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="End Date" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtClosureEndDate" ClientIDMode="Static" runat="server" CssClass="disabled-todayPast-date-picker form-control"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <asp:Button ID="btnClosureSave" Text="Save Closure" CssClass="btn btn-primary" runat="server" OnClick="btnClosureSave_Click" ClientIDMode="Static" Style="margin-top: -3px;" />

                                </div>
                            </div>
                        </div>
                        <br />
                        <br />

                        <div class="row">
                            <div class="col-md-12" style="margin-left: 20px;">
                                <div class="table-responsive">
                                    <asp:Repeater ID="rptrGroups" runat="server" OnItemDataBound="rptrGroups_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Text='<%# Eval("Name")%>'></asp:Label>
                                            <asp:Repeater ID="rptrDivisions" runat="server" OnItemDataBound="rptrDivisions_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="table-responsive" style="margin-left: 25px;">
                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID")%>' Visible="false"></asp:HiddenField>
                                                        <asp:CheckBox ID="cbDiv" runat="server" />
                                                        <asp:LinkButton ID="lbtnChannels" runat="server" OnClick="lbtnChannels_Click" CommandArgument='<%# Eval("ID")%>' OnClientClick="Success" Text='<%# Eval("Name")%>'></asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <asp:Button ID="btnMapDiv" runat="server" Style="margin-left: 20px; margin-top: 15px;" Text="Save" OnClick="btnMapDiv_Click" />
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvPreferance" runat="server" EmptyDataText="No record found"
                                                    DataKeyNames="PrefID1,PrefID2,PrefID3,PrefID4,PrefID5,StartDate,EndDate"
                                                    ShowHeaderWhenEmpty="True" OnRowDataBound="gvPreferance_RowDataBound" OnRowCommand="gvPreferance_RowCommand"
                                                    OnRowCancelingEdit="gvPreferance_RowCancelingEdit" OnRowUpdating="gvPreferance_RowUpdating"
                                                    OnRowEditing="gvPreferance_RowEditing" OnRowCreated="gvPreferance_RowCreated" OnRowDeleting="gvPreferance_RowDeleting"
                                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AutoGenerateColumns="False">
                                                    <Columns>

                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-1" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="date-picker form-control required" required="true" type="text"></asp:TextBox>
                                                                    <span id="spanFromDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("EndDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="input-group date" data-date-viewmode="years">
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="date-picker form-control required" required="true" type="text" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                                                    <span id="spanToDate" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                                </div>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="1st Preference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfirstPref" runat="server" Text='<%# Eval("Pref1")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlFirst" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="2nd Preference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSeconfPref" runat="server" Text='<%# Eval("Pref2")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlSecond" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="3rd Preference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblThirdPref" runat="server" Text='<%# Eval("Pref3")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlThird" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="4th Preference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFourthPref" runat="server" Text='<%# Eval("Pref4")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlFourth" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="5th Preference">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFifthPref" runat="server" Text='<%# Eval("Pref5")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlFifth" runat="server" CssClass="form-control required" required="true"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <HeaderStyle CssClass="" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                                            <EditItemTemplate>
                                                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                                    <asp:LinkButton ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                                                </asp:Panel>
                                                            </EditItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                                                    <asp:LinkButton ID="lbtnAdd" runat="server" Text="" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:LinkButton>
                                                                </asp:Panel>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="right">
                                                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />
                                                                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:LinkButton>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-md-1" />

                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="ModalChannels" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="box">
                                <div class="box-title">
                                    <asp:Label ID="Label1" runat="server" Style="font-size: 30px;"></asp:Label>
                                </div>
                                <div class="box-content">
                                    <div class="table-responsive">
                                        <div class="row" id="divChannels" runat="server">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvChannels" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                        ShowHeaderWhenEmpty="True" AllowPaging="true" PageSize="50" CssClass="table header" BorderWidth="0px"
                                                        CellSpacing="-1" GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Channel Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Design Discharge">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOffenderName" runat="server" Text='<%# Eval("DesignDischarge")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="PagerStyle" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeDatePickerStateOnUpdatePanelRefresh();
                }
            });
        };

        $(function () {
            $('.CtrlClass0').removeAttr("required");
        });


        $("#btnClosureSave").click(function () {
            debugger;
            var FromDate = $("#txtClosureStartDate").val();
            var ToDate = $("#txtClosureEndDate").val();

            if (FromDate == "" || ToDate == "") {
                alert("Select both closure start and end date");
                return false;
            }
            else if (new Date(FromDate) > new Date(ToDate)) {
                alert("Closure Start date must be less than End date");
                return false;
            }
            else if (new Date(FromDate) < new Date($("#MainContent_lblStartDate").text()) || new Date(ToDate) < new Date($("#MainContent_lblStartDate").text())) {
                alert("Closure Start and End dates must be greater than Program Start date");
                return false;
            }
                //else if (FromDate < $("#MainContent_lblStartDate").text() || ToDate <= $("#MainContent_lblStartDate").text()) {
                //    alert("Closure Start and End dates must be greater than Program Start date");
                //    return false;
                //}
            else {
                return true;
                $('.CtrlClass0').removeAttr("required");
            }
        });

        //$("#btnClosureSave").click(function () {
        //    debugger;
        //    var FromDate = $("#txtClosureStartDate").val();
        //    var ToDate = $("#txtClosureEndDate").val();
        //    if (FromDate == "" || ToDate == "") {
        //        alert("Select both closure start and end date");
        //        return false;
        //    }
        //    else if (FromDate > ToDate) {
        //        alert("Closure Start date must be less than End date");
        //        return false;
        //    }
        //    else if (FromDate < $("#MainContent_lblStartDate").text() || ToDate <= $("#MainContent_lblStartDate").text()) {
        //        alert("Closure Start and End dates must be greater than Program Start date");
        //        return false;
        //    }
        //    else {
        //        return true;
        //        $('.CtrlClass0').removeAttr("required");
        //    }
        //});

    </script>

</asp:Content>
