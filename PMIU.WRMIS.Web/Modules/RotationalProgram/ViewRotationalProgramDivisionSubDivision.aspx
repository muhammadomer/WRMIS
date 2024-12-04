<%@ Page Title="Rotational Program Level" MasterPageFile="~/Site.Master" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ViewRotationalProgramDivisionSubDivision.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.RotationalProgram.ViewRotationalProgramDivisionSubDivision" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .asdf {
            text-align: center;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>View Rotational Plan</h3>
                </div>

                <div class="box-content">
                    <div id="Print">
                        <div class="form-horizontal">

                            <h2 id="RPTitle" runat="server" style="text-align: center"></h2>
                            <div>
                                <h4 id="RPDates" runat="server" style="text-align: center"></h4>
                                <div style="float: right">
                                    <asp:HyperLink ID="hlAttachment" runat="server" CssClass="btn btn-link hidden-print" Visible="true"></asp:HyperLink>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvChannels" runat="server" Visible="true" AutoGenerateColumns="False" EmptyDataText="No record found"
                                    ShowHeaderWhenEmpty="True" AllowPaging="false" CssClass="table header" OnDataBound="gvChannels_OnDataBound"
                                    BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Group" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="asdf" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Group" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubGroupName" runat="server" Text='<%# Eval("SubGroupName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="asdf" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Channels">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChannels" runat="server" Text='<%# Eval("ChannelName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-9" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="asdf" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>

                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvPreferences" runat="server" AutoGenerateColumns="true" EmptyDataText="No Record Found"
                                    ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" AllowPaging="false"
                                    ShowHeader="false" OnRowCreated="gvPreferences_RowCreated" OnRowDataBound="gvPreferences_OnRowDataBound">
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <asp:TextBox ID="txtComments" Visible="false" runat="server" onblur="TrimInput(this);" Width="600px" CssClass="form-control multiline-no-resize required" required="required" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                    <br />
                    <%--<input type='button' class="btn btn-primary" id='btn' value='Print' onclick='printDiv();' />--%>
                    <asp:Button ID="btnApprove" Visible="false" runat="server" CssClass="btn btn-primary" Text="Approve" OnClick="btnApprove_Click" />
                    <asp:Button ID="btnSendBack" Visible="false" runat="server" CssClass="btn  btn-primary" Text="Send back" OnClick="btnSendBack_Click" />
                    <asp:LinkButton ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click"></asp:LinkButton>
                    <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                    <asp:LinkButton ID="lbtnHistory" runat="server" CssClass="btn btn-primary" Text="Comments History" OnClick="lbtnHistory_Click"></asp:LinkButton>
                </div>

                <%--<input type='button' id='btn' value='Print' onclick='printDiv();'>--%>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnHidePriority" runat="server" Value="false" />
    <asp:HiddenField ID="hdnGroupsQuantity" runat="server" Value="0" />

    <div class="modal fade" id="ModalComments" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body" id="content">
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="box">
                                <div class="box-title">
                                    <asp:Label ID="lblName" Text="Comments History" runat="server" Style="font-size: 30px;"></asp:Label>
                                </div>
                                <div class="box-content">
                                    <div class="table-responsive">
                                        <div class="row" id="gvOffenders" runat="server">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvComments" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                        ShowHeaderWhenEmpty="True" AllowPaging="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-1" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comment">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%# Eval("Comments") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-md-2" />
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

    <script>
        function printDiv() {
            //debugger;
            var printContents = document.getElementById('Print').innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }

        function OpenModal() {
            $("#ModalComments").modal("show");
        };
    </script>

</asp:Content>

