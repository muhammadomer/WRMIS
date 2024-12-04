<%@ Page Title="Elevation Capacity" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElevationCapacity.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.SeasonalPlanning.ReferenceData.ElevationCapacity" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3>Elevation Capacity</h3>
                        </div>
                        <div class="box-content">

                            <div class="row">
                                <div class="form-horizontal">

                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblRimStation" class="col-sm-4 col-lg-3 control-label">Rim Station</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" required="true" ID="ddlRimStation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRimStation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label for="lblDate" class="col-sm-4 col-lg-3 control-label">Date</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList CssClass="form-control required" required="true" Enabled="false" ID="ddlDates" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divButtons" runat="server">
                                <asp:Button ID="btnView" runat="server" Text="View" Visible="false" CssClass="btn btn-primary"></asp:Button>
                                <%--<asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click"></asp:Button>--%>


                                <asp:LinkButton ID="lbtnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="lbtnAdd_Click"></asp:LinkButton>



                            </div>
                            <br />
                            <asp:GridView ID="gvElevation" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                                OnRowCancelingEdit="gvElevation_RowCancelingEdit" OnRowUpdating="gvElevation_RowUpdating" OnRowEditing="gvElevation_RowEditing"
                                OnRowDataBound="gvElevation_RowDataBound" ShowHeaderWhenEmpty="True" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("Level") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-2" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Capacity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%#String.Format("{0:0.000}",Eval("Capacity")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control decimalInput" MaxLength="6" Text='<%# Eval("Capacity") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="col-md-1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                <asp:Button ID="btnSave" runat="server" Text="" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save"></asp:Button>
                                                <asp:LinkButton ID="lbtnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                            </asp:Panel>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:LinkButton>
                                                <asp:LinkButton ID="lbtnHistory" runat="server" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_24 audit" ToolTip="History" OnClick="lbtnHistory_Click"></asp:LinkButton>
                                            </asp:Panel>
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


            <div class="modal fade" id="ModalHistory" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body" id="content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                                <ContentTemplate>
                                    <div class="box">
                                        <div class="box-title">
                                            <asp:Label ID="lblName" runat="server" Style="font-size: 30px;"></asp:Label>
                                        </div>
                                        <div class="box-content">
                                            <div class="table-responsive">
                                                <div class="row" id="gvOffenders" runat="server">
                                                    <div class="col-md-12">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                                                                ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="10" CssClass="table header" BorderWidth="0px"
                                                                OnPageIndexChanged="gvHistory_PageIndexChanged" OnPageIndexChanging="gvHistory_PageIndexChanging"
                                                                CellSpacing="-1" GridLines="None">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Level">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Level") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Capacity">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Capacity") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ElevationCapacityDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Modified Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Modified By">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ModifiedBy") %>'></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
