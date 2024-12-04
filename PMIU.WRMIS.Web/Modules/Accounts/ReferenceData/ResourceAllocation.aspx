<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResourceAllocation.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Accounts.ReferenceData.ResourceAllocation" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="box">
        <div class="box-title">
            <h3>Resource Allocation</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="PMIU Staff" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlPMIUStaff" AutoPostBack="true" OnSelectedIndexChanged="ddlPMIUStaff_SelectedIndexChanged" runat="server" CssClass="form-control required" required="true">
                                    <asp:ListItem Text="Select" Value="" Selected="True" />
                                    <asp:ListItem Text="Field" Value="Field" />
                                    <asp:ListItem Text="Office" Value="Office" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblADM" runat="server" Text="ADM" CssClass="col-sm-4 col-lg-3 control-label" />
                            <div class="col-sm-8 col-lg-9 controls">
                                <asp:DropDownList ID="ddlADM" runat="server" CssClass="form-control">
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
            </div>
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvResourceAllocation" runat="server" DataKeyNames="ID,PMIUFieldOffice,ADMUserID,StaffUserID,StaffUserName,DesignationID,DesignationName,EmailAddress,ContactNumber,BPS,IsActive,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvResourceAllocation_RowCommand" OnRowDataBound="gvResourceAllocation_RowDataBound"
                            OnRowEditing="gvResourceAllocation_RowEditing" OnRowCancelingEdit="gvResourceAllocation_RowCancelingEdit"
                            OnRowUpdating="gvResourceAllocation_RowUpdating"
                            OnRowDeleting="gvResourceAllocation_RowDeleting" OnPageIndexChanging="gvResourceAllocation_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="ResourceAllocation ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResourceAllocationID" runat="server" Text='<%#Eval("ID")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Designation">
                                    <HeaderStyle CssClass="col-md-1" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("DesignationName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDesignation" runat="server" required="required" CssClass="form-control required" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged" AutoPostBack="true" onfocus="this.value = this.value;" Width="100%">
                                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name of Staff">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNameofStaff" runat="server" Text='<%# Eval("StaffUserName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlNameofStaff" Enabled="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNameofStaff_SelectedIndexChanged" CssClass="form-control" onfocus="this.value = this.value;" Width="100%">
                                            <asp:ListItem Value="" Text="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtNameofStaff" runat="server" oninput="javascript:AlphabetsWithLengthValidation(this, '3');" MaxLength="90" pattern="^[a-zA-Z ]+$" class="form-control commentsMaxLengthRow required" required="true" Text='<%# Eval("StaffUserName") %>' Visible="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Email Address">
                                    <HeaderStyle CssClass="col-md-2 text-left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmailAddress" runat="server" CssClass="control-label" Text='<%# Eval("EmailAddress") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="250" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" class="form-control commentsMaxLengthRow" Text='<%# Eval("EmailAddress") %>' />
                                        <asp:Label ID="lbltxtEmailAddress" runat="server" CssClass="control-label" Text='<%# Eval("EmailAddress") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact Number">
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNumber" runat="server" CssClass="control-label" Text='<%# Eval("ContactNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="20" oninput="javascript:InputWithLengthValidation(this, '10');" pattern="^[0-9]*$" class="form-control phoneNoInput required" required="true" Text='<%# Eval("ContactNumber") %>' />
                                        <asp:Label ID="lbltxtContactNumber" runat="server" CssClass="control-label" Text='<%# Eval("ContactNumber") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="BPS">
                                    <HeaderStyle CssClass="col-md-1 text-left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBPS" runat="server" CssClass="control-label" Text='<%# Eval("BPS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtBPS" runat="server" MaxLength="2" pattern="^[0-9]*$" oninput="javascript:ValueValidation(this,'1','22')" class="form-control required commentsMaxLengthRow" required="true" Text='<%# Eval("BPS") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" Visible="true">
                                    <HeaderStyle CssClass="col-md-1  text-center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" CssClass="control-label" Text='<%# Eval("IsActive") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox CssClass="radio-inline" ID="chkActive" runat="server" Text="" Checked='<%# Eval("IsActive") %>' />
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddResourceAllocation" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddResourceAllocation" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddResourceAllocation" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>

                                        <asp:Panel ID="pnlActionResourceAllocation" runat="server" HorizontalAlign="Center">
                                            <asp:HyperLink ID="hlAssets" runat="server" ToolTip="Assets" CssClass="btn btn-primary btn_24 asset_allocation" NavigateUrl='<%# String.Format("~/Modules/Accounts/ReferenceData/AssetAllocation.aspx?ResourceAllocationID={0}",Eval("ID")) %>' Text="">
                                            </asp:HyperLink>
                                            <asp:Button ID="btnEditResourceAllocation" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteResourceAllocation" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionResourceAllocation" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveResourceAllocation" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelResourceAllocation" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2 text-center" />
                                    <ItemStyle CssClass="text-center" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--onclick="history.go(-1);return false;"--%>
            <%--<div class="row">
                <div class="col-md-6">
                    <div class="fnc-btn">
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {

           

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                       
                        InitilizeNumericValidation();
                        
                    }
                });
            };

        });

    </script>
</asp:Content>
