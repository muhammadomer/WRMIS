<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TenderEvaluationCommittee.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Tenders.ReferenceData.TenderEvaluationCommittee" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../../Scripts/IrrigationNetwork/InputValidations.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Tender Evaluation Committee</h3>

                </div>
                <div class="box-content">
                    <div class="table-responsive">
                        <asp:GridView ID="gvTenderEvaluationCommittee" runat="server" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvTenderEvaluationCommittee_RowCommand"
                            OnRowCancelingEdit="gvTenderEvaluationCommittee_RowCancelingEdit" OnRowUpdating="gvTenderEvaluationCommittee_RowUpdating"
                            OnRowEditing="gvTenderEvaluationCommittee_RowEditing" OnRowDeleting="gvTenderEvaluationCommittee_RowDeleting"
                            OnPageIndexChanging="gvTenderEvaluationCommittee_PageIndexChanging" OnPageIndexChanged="gvTenderEvaluationCommittee_PageIndexChanged"
                            CssClass="table header"
                            BorderWidth="0px" CellSpacing="-1" GridLines="None">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr. No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="control-label" Text='<%#Eval("ID") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%#Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" onfocus="this.value = this.value;" MaxLength="90" CssClass="form-control required" placeholder="Enter Name" Text='<%#Eval("Name") %>' Onkeyup="InputValidation(this);" ClientIDMode="Static" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Designation">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation"  runat="server" CssClass="control-label " Text='<%#Eval("Designation") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesignation"  required="required" runat="server" MaxLength="250" CssClass="form-control required" placeholder="Enter Designation" Text='<%#Eval("Designation") %>' Width="90%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact No.">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobilePhone" runat="server" CssClass="control-label" Text='<%#Eval("MobilePhone") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:TextBox ID="txtMobilePhone" runat="server" required="required" MaxLength="11" CssClass="form-control required decimalInput integerInput" placeholder="Enter Mobile" Text='<%#Eval("MobilePhone") %>' Width="90%" />--%>
                                         <asp:TextBox ID="txtMobilePhone" class="form-control required" runat="server"  required="true" placeholder="XXXXXXXXXXX" Text='<%# Eval("MobilePhone") %>' pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                
                                <asp:TemplateField HeaderText="Email">
                                    <HeaderStyle CssClass="col-md-3" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" CssClass="control-label" Text='<%#Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:TextBox ID="txtEmail" runat="server" MaxLength="250" CssClass="form-control" placeholder="Enter Email" Text='<%#Eval("Email") %>' Width="90%" />--%>
                                        <asp:TextBox ID="txtEmail" placeholder="abc@xyz.com" TextMode="Email" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" class="form-control required" runat="server"  Text='<%# Eval("Email") %>'  MaxLength="75" required="true" />
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-1" />

                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-success btn_add plus" ToolTip="Add" Visible="<%# base.CanAdd %>"></asp:Button>
                                        </asp:Panel>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" Visible="<%# base.CanEdit %>"></asp:Button>
                                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="btn btn-danger btn_32 delete" ToolTip="delete" Visible="<%# base.CanDelete %>"></asp:Button>
                                        </asp:Panel>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnSave" runat="server" CommandName="Update" CssClass="btn btn-primary btn_32 save" ToolTip="Save" />
                                            <asp:LinkButton ID="lbtnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger btn_32 cross" ToolTip="Cancel"></asp:LinkButton>
                                        </asp:Panel>
                                    </EditItemTemplate>

                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
