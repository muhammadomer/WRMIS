<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GCBarrageHW.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.IndependentInspection.GCBarrageHW" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="ucFloodInspectionDetail" TagName="FloodInspectionDetail" Src="~/Modules/FloodOperations/Controls/FloodInspectionDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnInspectionStatus" runat="server" Value="0" />
    <div class="box">
        <div class="box-title">
            <h3>General Conditions for Flood Inspections</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <ucFloodInspectionDetail:FloodInspectionDetail runat="server" ID="FloodInspectionDetail1" />
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <h3>Gates Information</h3>
                        <div class="table-responsive">
                            <div>
                                <table class="table header" id="tblGatesInformation" style="border-collapse: collapse; border-spacing: 0;">
                                    <tbody>
                                        <tr>
                                            <th class="col-md-2" scope="col">Gate Type</th>
                                            <th class="col-md-2" scope="col">Gates Working</th>
                                            <th class="col-md-2 text-right" scope="col">Gates Not Working</th>
                                            <th class="col-md-2 text-right" scope="col">Total Gates</th>
                                        </tr>
                                        <tr>
                                            <td><span>Electrical</span><asp:HiddenField ID="hdnElectricalWorkingGateID" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtElectricalWorkingGate" runat="server" CssClass="form-control text-right" min="0" pattern="[0-9]*"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="lblElectricalNotWorkingGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                            <td>
                                                <asp:Label ID="lblElectricalTotalGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                        </tr>
                                        <tr>
                                            <td><span>Electronics</span><asp:HiddenField ID="hdnElectronicsWorkingGateID" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtElectronicsWorkingGate" runat="server" CssClass="form-control text-right" min="0" pattern="[0-9]*"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="lblElectronicsNotWorkingGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                            <td>
                                                <asp:Label ID="lblElectronicsTotalGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                        </tr>
                                        <tr>
                                            <td><span>Manual</span><asp:HiddenField ID="hdnManualWorkingGateID" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtManualWorkingGate" runat="server" CssClass="form-control text-right" min="0" pattern="[0-9]*"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="lblManualNotWorkingGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                            <td>
                                                <asp:Label ID="lblManualTotalGate" runat="server" Text="" CssClass="col-md-12 text-right" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <%--<div class="table-responsive">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvGatesInformation" runat="server" DataKeyNames="ID,Name,Details,ContactNumber,CreatedBy,CreatedDate"
                            CssClass="table header" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvGatesInformation_RowCommand" OnRowDataBound="gvGatesInformation_RowDataBound"
                            OnRowEditing="gvGatesInformation_RowEditing" OnRowCancelingEdit="gvGatesInformation_RowCancelingEdit"
                            OnRowUpdating="gvGatesInformation_RowUpdating"
                            OnRowDeleting="gvGatesInformation_RowDeleting" OnPageIndexChanging="gvGatesInformation_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Gate Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGateType" runat="server" Text='<%# Eval("GateType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" required="required" pattern="[a-zA-Z\-:].{3,90}" MaxLength="90" class="required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Gates Working">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetail" runat="server" Text='<%# Eval("Details") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDetail" runat="server" MaxLength="80" class="form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Gates Not Working">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNumber" runat="server" Text='<%# Eval("ContactNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContactNumber" runat="server" required="required" pattern="[0-9]{10,20}" MaxLength="20" class="integerInput required form-control" Style="max-width: 40%; display: inline;"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Panel ID="pnlInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnAddInfrastructureRepresentative" runat="server" Visible="<%# base.CanAdd %>" Text="" CommandName="AddInfrastructureRepresentative" CssClass="btn btn-success btn_add plus" ToolTip="Add" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlActionInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button ID="btnEditInfrastructureRepresentative" runat="server" Text="" CommandName="Edit" Visible="<%# base.CanEdit %>" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                            <asp:Button ID="btnDeleteInfrastructureRepresentative" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Panel ID="pnlEditActionInfrastructureRepresentative" runat="server" HorizontalAlign="Center">
                                            <asp:Button runat="server" ID="btnSaveInfrastructureRepresentative" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                            <asp:Button ID="btnCancelInfrastructureRepresentative" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
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
            </div>--%>

                        <h3>CCTV Camera Information</h3>
                        <div class="table-responsive">
                            <div>
                                <table class="table header" id="tblCCTVCameraInformation" style="border-collapse: collapse; border-spacing: 0;">
                                    <tbody>
                                        <tr>
                                            <th class="col-md-2" scope="col">Total Cameras</th>
                                            <th class="col-md-2" scope="col">Operational</th>
                                            <th class="col-md-2 text-right" style="padding-right:24px!important;" scope="col">Not Operational</th>
                                            <th class="col-md-2" scope="col">CCTV Incharge Name</th>
                                            <th class="col-md-2" scope="col">CCTV Incharge Phone</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtTotalCameras" runat="server" CssClass="form-control text-right"  min="0" pattern="[0-9]*"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtOperationalCameras" runat="server" CssClass="form-control text-right"  min="0" pattern="[0-9]*"></asp:TextBox>
                                            </td>
                                            <td class="text-right">
                                                <asp:Label ID="lblNotOperationalCameras" runat="server" Text="" CssClass="col-md-12 control-label" /></td>
                                            <td>
                                                <asp:TextBox ID="txtCCTVIncharge" runat="server" CssClass="form-control" pattern="[a-zA-Z\-:].{3,90}" MaxLength="90"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtCCTVInchargePhone" runat="server" pattern="^[0-9]{11,12}$" MaxLength="11" class="integerInput form-control" ></asp:TextBox></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <h3>Miscellaneous Information</h3>
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="hidden">
                                    <asp:HiddenField ID="hdnFloodInspectionsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCBarrageHWID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCBarrageHWCreatedBy" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnIGCBarrageHWCreatedDate" runat="server" Value="0" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblPoliceMonitoryCondition" runat="server" Text="Police Monitory Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlPoliceMonitoryCondition" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblLightingCondition" runat="server" Text="Lighting Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlLightingCondition" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblDataBoardCondition" runat="server" Text="Data Board Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlDataBoardCondition" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblTollHutCondition" runat="server" Text="Toll Hut Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlTollHutCondition" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblArmyCheckPostCondition" runat="server" Text="Army Check Post Condition" CssClass="col-sm-4 col-lg-5 control-label" />
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:DropDownList ID="ddlArmyCheckPostCondition" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblOperationalDeckElevated" runat="server" Text="Operational Deck Elevated (ft)" CssClass="col-sm-4 col-lg-5 control-label"/>
                                            <div class="col-sm-8 col-lg-5 controls">
                                                <asp:TextBox ID="txtOperationalDeckElevated" runat="server" CssClass="form-control text-right" min="1" pattern="[0-9].{0,5}"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" style="margin-left: 12px;">
                                        <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="col-sm-4 col-lg-12 controls" />
                                        <div class="col-sm-8 col-lg-11 controls" style="margin-top: 6px;">
                                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Rows="4" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn" style="margin-left: 26px;">
                                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                            <%--<asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" />--%>
                                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
