<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RDWiseConditions.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.RDWiseConditions" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>RD Wise Condition for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h3>Stone Pitching</h3>
                        <asp:GridView ID="gvStonePitching" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,SideID,ConditionID,ConditionName,Remarks,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvStonePitching_RowCommand" OnRowDataBound="gvStonePitching_RowDataBound"
                            OnRowEditing="gvStonePitching_RowEditing" OnRowCancelingEdit="gvStonePitching_RowCancelingEdit"
                            OnRowUpdating="gvStonePitching_RowUpdating" OnRowDeleting="gvStonePitching_RowDeleting" OnPageIndexChanging="gvStonePitching_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStonePitchingFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtStonePitchingFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtStonePitchingFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStonePitchingToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtStonePitchingToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtStonePitchingToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Side">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStonePitchingSide" runat="server" Text='<%# Eval("SideName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStonePitchingSide" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%;"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStonePitchingCondition" runat="server" Text='<%# Eval("ConditionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStonePitchingCondition" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStonePitchingRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtStonePitchingRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddStonePitching" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddStonePitching" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddStonePitching" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionStonePitching" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditStonePitching" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteStonePitching" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionStonePitching" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveStonePitching" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelStonePitching" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>Pitch Stone Apron</h3>
                        <asp:GridView ID="gvPitchStoneApron" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,ConditionID,ConditionName,Remarks,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvPitchStoneApron_RowCommand" OnRowDataBound="gvPitchStoneApron_RowDataBound"
                            OnRowEditing="gvPitchStoneApron_RowEditing" OnRowCancelingEdit="gvPitchStoneApron_RowCancelingEdit"
                            OnRowUpdating="gvPitchStoneApron_RowUpdating" OnRowDeleting="gvPitchStoneApron_RowDeleting" OnPageIndexChanging="gvPitchStoneApron_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPitchStoneApronFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPitchStoneApronFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtPitchStoneApronFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPitchStoneApronToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPitchStoneApronToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtPitchStoneApronToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPitchStoneApronCondition" runat="server" Text='<%# Eval("ConditionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPitchStoneApronCondition" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPitchStoneApronRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPitchStoneApronRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddPitchStoneApron" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddPitchStoneApron" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddPitchStoneApron" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionPitchStoneApron" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditPitchStoneApron" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeletePitchStoneApron" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionPitchStoneApron" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSavePitchStoneApron" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelPitchStoneApron" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>Rain Cuts</h3>
                        <asp:GridView ID="gvRainCuts" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,ConditionID,ConditionName,Remarks,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvRainCuts_RowCommand" OnRowDataBound="gvRainCuts_RowDataBound"
                            OnRowEditing="gvRainCuts_RowEditing" OnRowCancelingEdit="gvRainCuts_RowCancelingEdit"
                            OnRowUpdating="gvRainCuts_RowUpdating" OnRowDeleting="gvRainCuts_RowDeleting" OnPageIndexChanging="gvRainCuts_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRainCutsFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRainCutsFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtRainCutsFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRainCutsToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRainCutsToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtRainCutsToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRainCutsCondition" runat="server" Text='<%# Eval("ConditionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlRainCutsCondition" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRainCutsRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRainCutsRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddRainCuts" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddRainCuts" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddRainCuts" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionRainCuts" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditRainCuts" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteRainCuts" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionRainCuts" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveRainCuts" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelRainCuts" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>Eroding Animal Holes</h3>
                        <asp:GridView ID="gvErodingAnimalHoles" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,ConditionName,Remarks,CreatedBy,CreatedDate,ConditionID"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvErodingAnimalHoles_RowCommand" OnRowDataBound="gvErodingAnimalHoles_RowDataBound"
                            OnRowEditing="gvErodingAnimalHoles_RowEditing" OnRowCancelingEdit="gvErodingAnimalHoles_RowCancelingEdit"
                            OnRowUpdating="gvErodingAnimalHoles_RowUpdating" OnRowDeleting="gvErodingAnimalHoles_RowDeleting" OnPageIndexChanging="gvErodingAnimalHoles_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblErodingAnimalHolesFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtErodingAnimalHolesFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtErodingAnimalHolesFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblErodingAnimalHolesToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtErodingAnimalHolesToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtErodingAnimalHolesToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblErodingAnimalHolesCondition" runat="server" Text='<%# Eval("ConditionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlErodingAnimalHolesCondition" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblErodingAnimalHolesRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtErodingAnimalHolesRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddErodingAnimalHoles" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddErodingAnimalHoles" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddErodingAnimalHoles" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionErodingAnimalHoles" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditErodingAnimalHoles" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteErodingAnimalHoles" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionErodingAnimalHoles" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveRainCuts" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelErodingAnimalHoles" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>RD Marks</h3>
                        <asp:GridView ID="gvRDMarks" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,ConditionName,Remarks,CreatedBy,CreatedDate,ConditionID"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvRDMarks_RowCommand" OnRowDataBound="gvRDMarks_RowDataBound"
                            OnRowEditing="gvRDMarks_RowEditing" OnRowCancelingEdit="gvRDMarks_RowCancelingEdit"
                            OnRowUpdating="gvRDMarks_RowUpdating" OnRowDeleting="gvRDMarks_RowDeleting" OnPageIndexChanging="gvRDMarks_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRDMarksFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRDMarksFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtRDMarksFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRDMarksToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRDMarksToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtRDMarksToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Condition">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRDMarksCondition" runat="server" Text='<%# Eval("ConditionName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlRDMarksCondition" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRDMarksRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRDMarksRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddRDMarks" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddRDMarks" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddRDMarks" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionRDMarks" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditRDMarks" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteRDMarks" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionRDMarks" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveRDMarks" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelRDMarks" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="col-md-1" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <h3>Encroachment</h3>
                        <asp:GridView ID="gvEncroachment" runat="server" DataKeyNames="ID,FromRDTotal,ToRDTotal,EncroachmentTypeId,EncroachmentTypeName,Remarks,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvEncroachment_RowCommand" OnRowDataBound="gvEncroachment_RowDataBound"
                            OnRowEditing="gvEncroachment_RowEditing" OnRowCancelingEdit="gvEncroachment_RowCancelingEdit"
                            OnRowUpdating="gvEncroachment_RowUpdating" OnRowDeleting="gvEncroachment_RowDeleting" OnPageIndexChanging="gvEncroachment_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="From RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncroachmentFromRDs" runat="server" Text='<%# Eval("FromRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEncroachmentFromRDLeft" runat="server" required="required" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtEncroachmentFromRDRight" runat="server" required="required" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To RD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncroachmentToRD" runat="server" Text='<%# Eval("ToRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEncroachmentToRDLeft" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                        +
                                        <asp:TextBox ID="txtEncroachmentToRDRight" runat="server" required="required" pattern="^(0|[0-9][0-9]*)$" oninput="CompareRDValues(this)" class="integerInput RDMaxLength required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Encroachment Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncroachmentType" runat="server" Text='<%# Eval("EncroachmentTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEncroachmentType" runat="server" CssClass="form-control required" required="true" Style="max-width: 90%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncroachmentRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEncroachmentRemarks" runat="server" CssClass="form-control" Style="max-width: 90%; display: inline;" pattern=".{0,50}" MaxLength="50"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-md-2"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlAddEncroachment" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddEncroachment" runat="server" Text="" Visible="<%# base.CanAdd %>" CommandName="AddEncroachment" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionEncroachment" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditEncroachment" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteEncroachment" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionEncroachment" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveEncroachment" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelEncroachment" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </EditItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
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
