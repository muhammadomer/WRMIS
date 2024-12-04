<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberDetailsJoint.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Joint.MemberDetailsJoint" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucJointInspectionDetail" TagName="JointInspectionDetail" Src="~/Modules/FloodOperations/Controls/JointInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>Member Details for Joint Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucJointInspectionDetail:JointInspectionDetail runat="server" ID="JointInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvMemberDetailsJointInspection" runat="server" DataKeyNames="ID,Name,Department,CreatedDate,CreatedBy"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" GridLines="None" ShowHeaderWhenEmpty="True"
                            OnRowCommand="gvMemberDetailsJointInspection_RowCommand" OnRowDataBound="gvMemberDetailsJointInspection_RowDataBound"
                            OnRowEditing="gvMemberDetailsJointInspection_RowEditing" OnRowCancelingEdit="gvMemberDetailsJointInspection_RowCancelingEdit"
                            OnRowUpdating="gvMemberDetailsJointInspection_RowUpdating"
                            OnRowDeleting="gvMemberDetailsJointInspection_RowDeleting" OnPageIndexChanging="gvMemberDetailsJointInspection_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" CssClass="required form-control" MaxLength="150" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="140px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDepartment" runat="server" required="required" CssClass="required form-control" MaxLength="150" Style="max-width: 80%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="140px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddMemberDetailsJointInspection" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddMemberDetailsJointInspection" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddMemberDetailsJointInspection" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionMemberDetailsJointInspection" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditMemberDetailsJointInspection" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteMemberDetailsJointInspection" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionMemberDetailsJointInspection" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveMemberDetailsJointInspection" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelMemberDetailsJointInspection" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="form-group">
                <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 margin-10"></div>
                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes();
                }
            });
        };
    </script>
</asp:Content>
