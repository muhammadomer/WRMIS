<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FundRelease.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.FundRelease" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <div class="box">
        <div class="box-title">
            <h3>Fund Release</h3>

        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <!-- BEGIN Left Side -->
                        <div class="form-group">
                            <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control required" AutoPostBack="True" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" required="true"></asp:DropDownList>
                            </div>
                        </div>
                        <!-- END Left Side -->
                    </div>
                </div>
            </div>
            <br />
            <div class="table-responsive">
                <asp:GridView ID="gvFundRelease" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                    EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" BorderWidth="0px" CellSpacing="-1" GridLines="None"
                    Visible="false" CssClass="table header"
                    OnRowCommand="gvFundRelease_RowCommand" OnRowDataBound="gvFundRelease_RowDataBound"
                    OnRowCancelingEdit="gvFundRelease_RowCancelingEdit" OnRowUpdating="gvFundRelease_RowUpdating"
                    OnRowEditing="gvFundRelease_RowEditing" OnRowDeleting="gvFundRelease_RowDeleting"
                    OnPageIndexChanging="gvFundRelease_PageIndexChanging" OnPageIndexChanged="gvFundRelease_PageIndexChanged">

                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No." Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fund Release Type">
                            <ItemTemplate>
                                <asp:Label ID="lblFundReleaseType" runat="server" CssClass="control-label" Text='<%# Eval("FundReleaseType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlFundReleaseType" runat="server" CssClass="form-control required" required="true" />
                                <asp:Label ID="lblFundReleaseTypeID" runat="server" Text='<%# Eval("FundReleaseTypeID") %>' Visible="false"></asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fund Release Date">
                            <ItemTemplate>
                                <asp:Label ID="lblFundReleaseDate" runat="server" CssClass="control-label" Text='<%# Eval("FundReleaseDate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtFundReleaseDate" runat="server" CssClass="date-picker-user form-control required" required="true" Text='<%# Eval("FundReleaseDate") %>'></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    <asp:Label ID="lblFundReleaseDateEdit" runat="server" Visible="false" CssClass="control-label" Text='<%# Eval("FundReleaseDate") %>'></asp:Label>
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-2" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDesc" runat="server" MaxLength="80" CssClass="form-control" placeholder="Enter Description" Text='<%# Eval("Description") %>' />
                            </EditItemTemplate>
                            <HeaderStyle CssClass="col-md-6" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                </asp:Panel>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:Button>
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                    <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete ?');" CssClass="btn btn-danger btn_32 delete" ToolTip="Delete"></asp:Button>
                                    <asp:HyperLink ID="btnDetail" runat="server" NavigateUrl='<%#String.Format("~/Modules/Accounts/FundReleaseDetails.aspx?FinancialYear={0}&FundReleaseID={1}",ddlFinancialYear.SelectedItem.Value, Eval("ID"))%>' CssClass="btn btn-primary btn_32 details" ToolTip="Detail"></asp:HyperLink>
                                </asp:Panel>
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
    <script type="text/javascript">

        $(document).ready(function () {

            $('.date-picker-user').datepicker({
                startDate: '<%= ViewState[StartDate].ToString() %>',
                endDate: '<%= ViewState[EndDate].ToString() %>',
                autoclose: true,
                todayHighlight: true,
                language: 'ru'
            });

        });

    </script>
</asp:Content>
