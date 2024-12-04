<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TenderOpeningProcess.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.TenderNotice.TenderOpeningProcess" %>

<%@ Register Src="~/Modules/Tenders/Controls/ViewWorks.ascx" TagPrefix="uc1" TagName="ViewWorks" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="box-title">
                <h3 id="htitle" runat="server">Evaluation Committee Attendance</h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <uc1:ViewWorks runat="server" ID="ViewWorks" />
                </div>

                <div id="divPanel" class="panel panel-default" runat="server">
                    <div id="Tabs" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li id="liCommittee" style="width: 20%; text-align: center" runat="server" class="active"><a id="anchCommittee" runat="server" aria-controls="CommitteeAttendance" role="tab">Committee Attendance</a></li>
                            <li id="liContractor" style="width: 20%; text-align: center" runat="server"><a id="anchContractor" runat="server" aria-controls="ContractorsAttendance" role="tab">Contractors Attendance</a></li>
                            <li id="liPrice" runat="server" style="width: 20%; text-align: center"><a id="anchTenderPrice" runat="server" aria-controls="TenderPrice" role="tab">Tender Price</a></li>
                            <li id="liReport" runat="server" style="width: 20%; text-align: center"><a id="anchReport" runat="server" aria-controls="ADMReport" role="tab">ADM Report</a></li>
                            <li id="liStatement" runat="server" style="width: 20%; text-align: center"><a id="anchstatement" runat="server" aria-controls="ComparativeStatement" role="tab">Comparative Statement</a></li>

                        </ul>
                    </div>
                </div>
                <div class="form-horizontal">
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Monitored By</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control " required="required" ID="ddlMonitoredBy" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlMonitoredBy_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="12">ADM</asp:ListItem>
                                                <asp:ListItem Value="13">MA</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="DivisionDiv">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Name</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control " required="required" ID="ddlName" runat="server" TabIndex="2">
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label runat="server" id="lblOpenedBy" class="col-sm-4 col-lg-3 control-label">Opened By</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox runat="server" ID="txtOpenedBy" required="required" type="text" class="form-control"> </asp:TextBox>

                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvEvalCommitteeAttend" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="True"
                                    DataKeyNames="AttendanceID" OnRowDataBound="gvEvalCommitteeAttend_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("isAttended").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" CssClass="control-label" Text='<%# Eval("AttendanceID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMemberName" runat="server" CssClass="control-label" Text='<%# Eval("MemberName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <%--<HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesignation" runat="server" CssClass="control-label" Text='<%# Eval("MemberDesignation") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alternate">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkAlternate" runat="server" ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name">
                                            <%--        <HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtAlternateName" required="required" type="text" class="form-control" Text='<%# Eval("AlternateName") %>' ClientIDMode="Static"> </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Designation">
                                            <%--<HeaderStyle CssClass="col-md-2" />--%>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtAlternateDesignation" required="required" type="text" class="form-control" Text='<%# Eval("AlternateDesignation") %>' ClientIDMode="Static"> </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>


                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="col-sm-3 col-lg-2 control-label">Attach Attendance Sheet</label>
                                <div class="col-sm-3 col-lg-4 controls">
                                    <uc1:FileUploadControl runat="server" ID="FileUploadControl" Size="1" AllowMultiple="true" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="" id="HyperLinksDiv" runat="server" visible="false">

                        <div class="col-md-12">
                            <div class="form-group">
                                <h4>Attachments</h4>
                                <%--  <table id="tblHyperlinks" runat="server">
                                        </table>--%>
                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" />
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="divSave">
                        <div class="col-md-12">
                            <%--   <asp:LinkButton TabIndex="10" ID="btnSave" runat="server" Text="Save & Proceed" CssClass="btn btn-primary" OnClick="btnSave_Click"></asp:LinkButton>--%>
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save & Proceed" OnClick="btnSave_Click" />
                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTenderWorkID" runat="server" />
    <asp:HiddenField ID="hdnWorkSourceID" runat="server" />
    <asp:HiddenField ID="hdnDivisionID" runat="server" />

    <script type="text/javascript">
        function RemoveRequired() {
            $('.CtrlClass0').removeAttr("required");
        }
        // for check all checkbox  
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvEvalCommitteeAttend.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }


        <%--            $("input:checkbox").click(function () {
                if ($(this).is(":checked")) {
                    var GridView1 = document.getElementById('<%= gvEvalCommitteeAttend.ClientID %>');

                    for (var rowId = 1; rowId < GridView1.rows.length; rowId++) {
                        var textValue = GridView1.rows[rowId].cells[0].children[0];
                        alert(textValue.value);
                    }
                } else {
                    alert("false");
                }
            });--%>


    </script>

    <script type="text/javascript">
        $(function () {


            //Enable Disable TextBoxes in a Row when the Row CheckBox is checked.
            $("[id*=chkAlternate]").bind("click", function () {

                //Find and reference the GridView.
                var grid = $(this).closest("table");



                //If the CheckBox is Checked then enable the TextBoxes in thr Row.
                if (!$(this).is(":checked")) {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#FFF" });
                    $("input[type=text]", td).attr("disabled", "disabled");
                } else {
                    var td = $("td", $(this).closest("tr"));
                    td.css({ "background-color": "#D8EBF2" });
                    $("input[type=text]", td).removeAttr("disabled");
                }


            });
        });
    </script>
</asp:Content>
