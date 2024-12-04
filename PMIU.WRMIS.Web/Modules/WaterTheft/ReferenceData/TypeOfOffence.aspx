<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TypeOfOffence.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.ReferenceData.TypeOfOffence" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- BEGIN Main Content -->
    <asp:HiddenField ID="hdnOffenceID" runat="server" Value="0" />

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3 >Type of offence</h3>
                </div>
                <div class="box-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <div id="offenceType" runat="server">
                                    <asp:GridView ID="gvOffenceType" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
                                        EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True" AllowSorting="false" OnRowCommand="gvOffenceType_RowCommand"
                                        OnRowCancelingEdit="gvOffenceType_RowCancelingEdit" OnRowEditing="gvOffenceType_RowEditing"
                                        OnRowUpdating="gvOffenceType_RowUpdating" OnRowDeleting="gvOffenceType_RowDeleting"  OnPageIndexChanging="gvOffenceType_PageIndexChanging"
                                        OnRowDataBound="gvOffenceType_RowDataBound"
                                        CssClass="table header"
                                        BorderWidth="0px" CellSpacing="-1" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                                <HeaderStyle CssClass="col-md-3" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <%-- Offence Type --%>
                                            <asp:TemplateField HeaderText="Type of Offence" Visible="true">
                                                <HeaderStyle CssClass="col-md-11" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffenceType" runat="server" CssClass="control-label" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtOffenceType" Width="20%" runat="server" CssClass="form-control required" onfocus="this.value = this.value;" required="true" Minlength="3" MaxLength="25"   placeholder="Enter OffenceType" Text='<%# Eval("Name") %>' ClientIDMode="Static" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>



                                            <%-- Add, Edit, Delete Buttons --%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Panel ID="pnlAddType" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnAddType" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddTypeOffenceType" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                                    </asp:Panel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" Visible="<%# base.CanEdit %>" ToolTip="Edit"></asp:Button>
                                                        <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn_32 delete" OnClientClick="return confirm('Are you sure you want to delete ?');" ToolTip="Delete" Visible="<%# base.CanDelete %>"></asp:Button>
                                                    </asp:Panel>
                                                </ItemTemplate>

                                                <EditItemTemplate>
                                                    <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                        <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                                        <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel" />
                                                    </asp:Panel>
                                                </EditItemTemplate>
                                                <HeaderStyle CssClass="col-md-1" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="PagerStyle" />
                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                    </div>
                    <br />
       <%--             <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END Main Content -->
    <script>
        function ValidateValue(textbox) {
            var value = textbox.value;
            var letters = /^[a-z0-9]+$/i;
            var result = letters.test(value);
            if (result == true) {
                return true;
            }
            else {
                //alert('only alphanumeric is allowed');
                //textbox.value = '';
                //var res = textbox.value + '_';
                //var index = res.lastIndexOf('_');
                //res = res.substring(0, index - 1);
                //textbox.value = res;
                return true;
            }
        }
    </script>
</asp:Content>
