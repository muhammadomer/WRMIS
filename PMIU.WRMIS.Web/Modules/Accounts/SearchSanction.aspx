<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchSanction.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.SearchSanction" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3>Search Sanction</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <div class="row">

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblSanctionNo" runat="server" Text="Sanction No." CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:TextBox ID="txtSanctionNo" runat="server" CssClass="form-control" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblSanctionType" runat="server" Text="Sanction Type" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlSanctionType" ClientIDMode="Static" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSanctionType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblSanctionOn" runat="server" Text="Sanction On" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlSanctionOn" ClientIDMode="Static" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlFinancialYear" ClientIDMode="Static" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlMonth" ClientIDMode="Static" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblSanctionStatus" runat="server" Text="Sanction Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList ID="ddlSanctionStatus" ClientIDMode="Static" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="table-responsive" style="display: <%= GridDisplay %>;">
                        <asp:GridView ID="gvSanctions" runat="server" EmptyDataText="No record found" AutoGenerateColumns="false" AllowPaging="true"
                            CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" PageSize="10"
                            OnRowDataBound="gvSanctions_RowDataBound" OnPageIndexChanging="gvSanctions_PageIndexChanging"
                            OnRowCommand="gvSanctions_RowCommand" OnRowUpdating="gvSanctions_RowUpdating" OnRowEditing="gvSanctions_RowEditing"
                            OnRowCancelingEdit="gvSanctions_RowCancelingEdit" OnRowDeleting="gvSanctions_RowDeleting" ClientIDMode="Static">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" CssClass="control-label" Text='<%# Eval("Month") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlgvMonth" runat="server" CssClass="form-control required" required="true" />
                                        <asp:Label ID="lblEditMonth" runat="server" CssClass="control-label" Text='<%# Eval("Month") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanction No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanctionNo" runat="server" CssClass="control-label" Text='<%# Eval("SanctionNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtgvSanctionNo" runat="server" MaxLength="17" oninput="InputTextWithLengthValidation(this,'16');" CssClass="form-control required" Text='<%# Eval("SanctionNo") %>' required="true" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanction Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanctionType" runat="server" CssClass="control-label" Text='<%# Eval("AT_ExpenseType.ExpenseType") +" "+ Eval("AT_AssetType.AssetType") %>'></asp:Label>
                                        <asp:Label ID="lblSanctionTypeID" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("ExpenseTypeID") %>'></asp:Label>
                                        <asp:Label ID="lblSanctionTypeName" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("SanctionTypeName") %>'></asp:Label>

                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="control-label" Text='<%# Eval("AT_AssetType.AssetType") %>'></asp:Label>
                                        <asp:Label ID="lblExpenseType" runat="server" Visible="false" CssClass="control-label" Text='<%# Eval("AT_ExpenseType.ExpenseType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSanctionType" runat="server" MaxLength="20" oninput="InputTextWithLengthValidation(this,'3');" CssClass="form-control required" Text='<%# Eval("SanctionTypeName") %>' required="true" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Object Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanctionedOn" runat="server" CssClass="control-label" Text='<%# Eval("AT_ObjectClassification.AccountsCode") +" ("+Eval("AT_ObjectClassification.ObjectClassification") + ")"  %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlgvObjectClassification" runat="server" CssClass="form-control required" required="true" />
                                        <asp:Label ID="lblObjectClassification" runat="server" Visible="false" Text='<%# Eval("ObjectClassificationID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanction Amount (Rs.)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanctionAmount" runat="server" CssClass="control-label" Text='<%# Eval("SanctionAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSanctionAmount" runat="server" onfocus="RemoveCommas(this);" onblur="AddCommas(this);" MaxLength="6" oninput="InputWithLengthValidation(this,'1');" Text='<%# Eval("SanctionAmount") %>' CssClass="form-control required decimalInput" required="true" />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sanction Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanctionStatus" runat="server" CssClass="control-label" Text='<%# Eval("AT_SanctionStatus.Name") %>'></asp:Label>
                                        <asp:Label ID="lblSanctionStatusID" runat="server" Visible="false" CssClass="control-label" Text='<%# Eval("SanctionStatusID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtSanctionStatus" runat="server" CssClass="control-label" Text='<%# PMIU.WRMIS.Common.Constants.SanctionStatus.Sanctioned.ToString()%>' />
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusDate" runat="server" CssClass="control-label" Text='<%# Eval("SanctionStatusDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Token Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTokenNumber" runat="server" CssClass="control-label" Text='<%# Eval("TokenNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:LinkButton ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add"></asp:LinkButton>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                        </asp:Panel>
                                    </EditItemTemplate>

                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnChangeStatus" runat="server" CssClass="btn btn-primary btn_24 change_status" ToolTip="Change Status" OnClick="btnChangeStatus_Click" />
                                        <asp:HyperLink ID="btnTaxSheet" runat="server" NavigateUrl='<%#String.Format("~/Modules/Accounts/TaxSheet.aspx?SanctionID={0}", Eval("ID"))%>' CssClass="btn btn-primary btn_32 tax_sheet" ToolTip="Tax Sheet"></asp:HyperLink>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit"></asp:Button>
                                        <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="btn btn-danger btn_32 delete" ToolTip="delete"></asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col-md-2" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>


            <!-- START Of Audit Trail -->
            <div id="ChangeStatus" class="modal fade">
                <div class="modal-dialog" style="width: 50%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>--%>
                            <div class="row">

                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="hdnLabel" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblModalSanctionStatus" runat="server" Text="Sanction Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlModalSanctionStatus" runat="server" CssClass="form-control" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <!-- BEGIN Right Side -->
                                    <div class="form-group">
                                        <asp:Label ID="lblDate" runat="server" class="col-sm-4 col-lg-3 control-label">Status Date</asp:Label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <div class="input-group date" data-date-viewmode="years">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="disabled-future-date-picker form-control" ClientIDMode="Static"></asp:TextBox>
                                                <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- END Right Side -->
                                </div>

                            </div>
                            <div id="dvTokenNumber" runat="server" clientidmode="Static" class="row" style="display: none;">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblTokenNumberString" runat="server" Text="Token Number" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtTokenNumber" runat="server" CssClass="form-control integerInput" MaxLength="10" />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click"></asp:Button>
                            <asp:Button CssClass="btn btn-default" runat="server" ID="btnClose" OnClientClick="Close()" Text="Close" OnClick="btnClose_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Detail Modal Ends here -->
            <!-- END Of Audit Trail -->



        </div>
    </div>


    <script>
        function Close() {
            $('#txtDate').removeAttr('required');
        }

        var SentToAGOffice = <% Response.Write((long)PMIU.WRMIS.Common.Constants.SanctionStatus.SenttoAGOffice); %>

        $(document).ready(function () {

            $('#txtSanctionNo').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlSanctionType').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlSanctionOn').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlFinancialYear').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlMonth').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlSanctionStatus').change(function () {
                $('#gvSanctions').hide();
            });

            $('#ddlModalSanctionStatus').change(function () {
                if ($('#ddlModalSanctionStatus').val() == SentToAGOffice) {
                    $('#dvTokenNumber').show();
                }
                else {
                    $('#dvTokenNumber').hide();
                }
            });

        });
    </script>

</asp:Content>
