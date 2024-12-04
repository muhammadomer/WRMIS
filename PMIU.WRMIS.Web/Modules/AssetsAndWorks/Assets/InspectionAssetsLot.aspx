<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionAssetsLot.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.AssetsAndWorks.Assets.InspectionAssetsLot" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<%@ Register TagPrefix="ucAssets" TagName="Assets" Src="~/Modules/AssetsAndWorks/UserControls/AssetsDetail.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .integerInput {
            text-align: left;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Inspection of Asset</h3>
                    <div class="box-tool">
                        <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
                    </div>
                </div>
                <div class="box-content">
                    <ucAssets:Assets runat="server" ID="AssetsDetail" />
                    <div class="table-responsive">
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="hidden">
                                    <asp:HiddenField ID="hdnAssetsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedBy" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCreatedDate" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnLotQuantity" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnAvailableQuantity" runat="server" Value="0" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Inspection Date</label>
                                            <div class="col-sm-8 col-lg-9 controls" runat="server" id="divDate">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    <asp:TextBox ID="txtDate" TabIndex="4" required="required" runat="server" CssClass="required form-control date-picker" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <h3>Status of Asset</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblNumber" runat="server" Text="Number" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtNumber" runat="server" CssClass="required integerInput form-control" required="required" MaxLength="8"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">

                                        <div class="form-group">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" class="col-sm-4 col-lg-3 control-label" Text="Remarks"></asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox CssClass="form-control commentsMaxLengthRow multiline-no-resize"
                                                    MaxLength="250" TextMode="MultiLine" Rows="5" Columns="50"
                                                    ID="txtRemarks" runat="server" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblCurrentAssetValue" runat="server" Text="Current Asset Value (Rs.)" CssClass="col-sm-4 col-lg-3 control-label" />
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox ID="txtCurrentAssetValue" runat="server" CssClass="form-control integerInput" MaxLength="15"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <h3>Condition of Asset</h3>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="DivNewCondition">
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:GridView ID="gvAssetCondition" runat="server"
                                                CssClass="table header" AutoGenerateColumns="False"
                                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true"
                                                OnRowCommand="gvAssetCondition_RowCommand" OnRowDeleting="gvAssetCondition_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Number">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNumberGrid" runat="server" required="required" MaxLength="8" class="required integerInput form-control"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Condition">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlCondition" runat="server" required="required" CssClass="required form-control"></asp:DropDownList>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="50" class="form-control" Style="max-width: 100%; display: inline;"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>

                                                            <asp:Panel ID="pnlItem" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnAdd" runat="server" Text="" CommandName="AddCondition" Visible="<%# base.CanAdd %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlAction" runat="server" HorizontalAlign="Center">

                                                                <asp:Button ID="btnDelete" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Panel ID="pnlEditAction" runat="server" HorizontalAlign="Center">
                                                                <asp:Button runat="server" ID="btnSave" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                                <asp:Button ID="btnCancel" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="DivViewCondition" visible="false">
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:GridView ID="gvViewCondition" runat="server"
                                                CssClass="table header" AutoGenerateColumns="False"
                                                EmptyDataText="No record found" AllowSorting="false" GridLines="None" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNature_work" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-2" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Condition">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNature_work" runat="server" Text='<%# Eval("Condition") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNature_work" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="col-md-3" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>

                                                            <asp:Panel ID="pnlItemAtrribute" runat="server" HorizontalAlign="Center">
                                                                <asp:Button ID="btnAddAtrribute" runat="server" Text="" CommandName="AddCondition" Visible="<%# base.CanAdd %>" CssClass="btn btn-success btn_add plus" ToolTip="Add" />
                                                            </asp:Panel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlActionAtrribute" runat="server" HorizontalAlign="Center">

                                                                <asp:Button ID="btnDeleteAtrribute" runat="server" Text="" CommandName="Delete" Visible="<%# base.CanDelete %>" OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Panel ID="pnlEditActionAtrribute" runat="server" HorizontalAlign="Center">
                                                                <asp:Button runat="server" ID="btnSaveAtrribute" CommandName="Update" ToolTip="Save" class="btn btn-primary btn_24 save" />
                                                                <asp:Button ID="btnCancelAtrribute" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                                            </asp:Panel>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="col-md-1" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" Text="Attachment" class="col-sm-4 col-lg-3 control-label"></asp:Label>
                                            <div class="col-sm-8 col-lg-9 controls" id="divAttchmentAdd" runat="server">

                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl1" Size="5" />
                                            </div>
                                            <div class="col-sm-8 col-lg-9 controls" id="divAttchmentView" runat="server" visible="false">
                                               <%-- <asp:HyperLink ID="HyperLink1" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink2" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink3" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink4" Text="Attachment" runat="server" Visible="false" />
                                                <br>
                                                <asp:HyperLink ID="HyperLink5" Text="Attachment" runat="server" Visible="false" />--%>
                                                 <uc1:FileUploadControl runat="server" ID="FileUploadControl2" Size="0" Visible="false" />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl3" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl4" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl5" Size="0" Visible="false"  />
                                                <br>
                                                <uc1:FileUploadControl runat="server" ID="FileUploadControl6" Size="0" Visible="false"  />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn" style="margin-left: 26px;">
                                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" Text="&nbsp;Save" />
                                            <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">Back</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
