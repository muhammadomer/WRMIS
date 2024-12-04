<%@ Page Title="Roles" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SearchRotationalProgram.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.SearchRotationalProgram" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .CenterAlign {
            text-align: center;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Search Rotational Plan</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCircle" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Circle Name</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlCircleName" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblSubDivision" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Sub Division</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSubDivision" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDivision_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblYear" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Year</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Season</asp:Label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlSeason" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" Text="Search" CssClass="btn btn-primary" runat="server" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnAddNew" Text="Add New" CssClass="btn btn-primary" runat="server" OnClick="btnAddNew_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvRotationalProg" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                        OnRowCommand="gvRotationalProg_RowCommand" OnRowDeleting="gvRotationalProg_RowDeleting" OnPageIndexChanging="gvRotationalProg_PageIndexChanging"
                                        OnPageIndexChanged="gvRotationalProg_PageIndexChanged"
                                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Plan Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelName" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-3" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Season">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChannelType" runat="server" CssClass="control-label" Text='<%# Eval("Season") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlowType" runat="server" CssClass="control-label" Text='<%# Eval("Year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gini">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGinni" runat="server" CssClass="control-label" Text='<%# string.Format("{0:0.00}",Eval("Gini"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Imp. %">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPlanPer" runat="server" CssClass="control-label" Text='<%# string.Format("{0:0.00}",Eval("Percentage"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duration">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPlanDuration" runat="server" CssClass="control-label" Text='<%# Eval("Date") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" runat="server" CssClass="control-label" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnGini" runat="server" CssClass="btn btn-primary btn_24 gini" CommandName="CalculateGini" Visible='<%# Eval("VisibilityGini") %>' ToolTip="Calculate Gini"></asp:Button>
                                                    <asp:Button ID="btnPlanImplt" runat="server" CssClass="btn btn-primary btn_24 planImplementation" CommandName="CalculatePlanImplementation" Visible='<%# Eval("VisibilitySE") %>' ToolTip="Calculate Plan Implementation"></asp:Button>
                                                    <asp:Button ID="btnView" runat="server" CssClass="btn btn-primary btn_24 view" CommandName="NavigationView" ToolTip="View"></asp:Button>
                                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn_24 edit" Visible='<%# Eval("Visibility") %>' CommandName="NavigationEdit" ToolTip="Edit"></asp:Button>
                                                    <asp:Button ID="hlDelete" runat="server" CssClass="btn btn-primary btn_24 delete" Visible='<%# Eval("Visibility") %>' CommandName="Delete" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"></asp:Button>
                                                    <asp:Button ID="btnFootNote" runat="server" CssClass="btn btn-primary btn_24 footnote" CommandName="FootNote" ToolTip="Foot Note"></asp:Button>
                                                    <%--<asp:Button ID="hlApprove" runat="server" CssClass="btn btn-primary fa tick" Visible='<%# Eval("Visibility") %>' CommandName="Approve"></asp:Button>--%>
                                                    <asp:Button ID="lbImport" runat="server" CssClass="btn btn-primary btn_24 assign-back" CommandName="Import" ToolTip="ExportToExcel" OnClick="ExportToExcel" CommandArgument='<%#Eval("ID")%>' />
                                                    <asp:Button ID="btnClone" runat="server" CssClass="btn btn-primary btn_24 Clone" CommandName="Clone" ToolTip="Clone Plan" CommandArgument='<%#Eval("ID")%>'  Visible='<%# Eval("Visibility") %>' />
                                                    <asp:Button ID="btnGraph" runat="server" CssClass="btn btn-primary btn_24 RPGraph" CommandName="Graph" ToolTip="Graph, Difference and Frequency Bands" Visible='<%# Eval("VisibilityGini") %>' CommandArgument='<%#Eval("ID")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="CenterAlign" />
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
        </div>
    </div>

    <div class="modal fade" id="ModalCalculatePlan" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">

                    <div class="box">
                        <div class="box-title">
                            <asp:Label ID="lblName" runat="server" Style="font-size: 30px;"></asp:Label>
                        </div>
                        <div class="box-content">
                            <div class="table-responsive">
                                <div class="row" runat="server">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-content">
                                                <div class="form-horizontal">
                                                    <asp:HiddenField ID="hfRPID" runat="server" />
                                                    <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lblWaraFrom" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Wara From</asp:Label>
                                                                        <div class="col-sm-8 col-lg-9 controls">
                                                                            <asp:DropDownList ID="ddlWaraFrom" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6 ">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lblWaraTo" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Wara To</asp:Label>
                                                                        <div class="col-sm-8 col-lg-9 controls">
                                                                            <asp:DropDownList ID="ddlWaraTo" ClientIDMode="Static" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label2" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Low Limit%</asp:Label>
                                                                        <div class="col-sm-8 col-lg-9 controls">
                                                                            <asp:TextBox ID="txtStart" runat="server" ClientIDMode="Static" CssClass="form-control decimalInput" MaxLength="6" Text="80"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6 ">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label3" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Max Limit%</asp:Label>
                                                                        <div class="col-sm-8 col-lg-9 controls">
                                                                            <asp:TextBox ID="txtEnd" runat="server" ClientIDMode="Static" CssClass="form-control decimalInput" MaxLength="6" Text="100"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <asp:Button ID="btnOk" Text="Calculate" runat="server" CssClass="btn btn-primary" OnClick="btnOk_Click" />
                                                                <div class="col-sm-8 col-lg-9 controls">
                                                                    <asp:Button ID="btnCompleteSeason" Text="Calculate Complete Season" runat="server" CssClass="btn btn-primary" OnClick="btnCompleteSeason_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button id="btnCloseRemarks" class="btn btn-info" data-dismiss="modal" aria-hidden="true" onclick="return RequiredAttribute()">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="ModalFootNote" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content1">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="box">
                                <div class="box-title">
                                    <asp:Label ID="Label4" runat="server" Style="font-size: 30px;"></asp:Label>
                                </div>
                                <div class="box-content">
                                    <div class="table-responsive">
                                        <div class="row" runat="server">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="box-content">
                                                        <div class="form-horizontal">
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label5" CssClass="col-sm-4 col-lg-3 control-label" runat="server">Foot Notes</asp:Label>
                                                                        <div class="col-sm-8 col-lg-9 controls">
                                                                            <asp:TextBox ID="txtFootNotes" runat="server" onblur="TrimInput(this);" CssClass="form-control multiline-no-resize" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <asp:Button ID="btnAddComment" runat="server" CssClass="btn btn-primary" Text="Add FootNotes" OnClick="btnAddComment_Click" />
                                                        </div>
                                                    </div>
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
                    <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function RequiredAttribute() {
            RemoveRequiredAttribute(document.getElementById("ddlWaraFrom"));
            RemoveRequiredAttribute(document.getElementById("ddlWaraTo"));

            RemoveRequiredAttributeTextBox(document.getElementById("txtStart"));
            RemoveRequiredAttributeTextBox(document.getElementById("txtEnd"));
        }


        function RemoveRequiredAttribute(controlID) {
            controlID.removeAttribute("required");
            controlID.setAttribute("Class", "form-control");
        }

        function RemoveRequiredAttributeTextBox(controlID) {
            controlID.removeAttribute("required");
            controlID.setAttribute("Class", "form-control decimalInput");
        }

        function openModal() {
            $("#ModalCalculatePlan").modal("show");
        };

        //$('#ModalCalculatePlan').on('hidden.bs.modal', function () {
        //   // alert("Success");
        //});



    </script>
</asp:Content>
