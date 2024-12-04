<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" EnableEventValidation="false" CodeBehind="SurchargeAmount.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.ReferenceData.SurchargeAmount" %>
<%@ Import Namespace="PMIU.WRMIS.Common" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/WaterTheft/Controls/FileUploadControl.ascx" TagPrefix="uc1" TagName="FileUploadControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Surcharge Amount</h3>
                </div>
                <div class="box-content">
                    <asp:Panel ID="pnlEffluent" runat="server" GroupingText="Effluent">
                        <div class="table-responsive">
                            <table class="table tbl-info">
                                <tr>
                                    <th>
                                        <div><strong>Surcharge Amount (Rs.)</strong></div>
                                    </th>
                                    <th>
                                        <div><strong>Surcharge Type</strong></div>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAmnt" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblType" runat="server"></asp:Label>
                                        <asp:HiddenField runat="server" Value="" ID="ddlTypeID" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <div><strong>Last Update Date</strong></div>
                                    </th>
                                    <th>
                                        <div><strong>Attachment</strong></div>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hdnAttachmentEffulent" runat="server" Value="" />
                                        <asp:HyperLink ID="hlAtchmntEffluent" runat="server" Text="Attachment" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnChange" runat="server" Text="Change" CssClass="btn btn-primary" ToolTip="Change" OnClick="btnChange_Click" />
                                    <asp:Button ID="btnHistry" runat="server" Text="View History" data-toggle="modal" formnovalidate="formnovalidate" CssClass="btn btn-success" ToolTip="View History" OnClick="btnHistry_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <br />
                     <br />
                    <asp:Panel ID="pnlCanal" runat="server" GroupingText="Canal Special Water">
                        <div class="table-responsive">
                            <table class="table tbl-info">
                                <tr>
                                    <th>
                                        <div>
                                            <strong>Surcharge Amount (Rs.)</strong></div>
                                    </th>
                                    <th>
                                        <div>
                                            <strong>Surcharge Type</strong></div>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAmntC" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTypeC" runat="server"></asp:Label>
                                        <asp:HiddenField runat="server" Value="" ID="ddlTypeIDC" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <div>
                                            <strong>Last Update Date</strong></div>
                                    </th>
                                    <th>
                                        <div>
                                            <strong>Attachment</strong></div>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDateC" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        
                                        <asp:HiddenField ID="hdnAttachmentCanal" runat="server" Value="" />
                                        <asp:HyperLink ID="hlAtchmntCanal" runat="server" Text="Attachment" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnChangeCanal" runat="server" CssClass="btn btn-primary" OnClick="btnChange_Click" Text="Change" ToolTip="Change" />
                                    <asp:Button ID="btnHstoryCanal" runat="server" CssClass="btn btn-success" data-toggle="modal" formnovalidate="formnovalidate" OnClick="btnHistry_Click" Text="View History" ToolTip="View History" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="Add" class="modal fade">
                        <div class="modal-dialog table-responsive" style="max-height: 419px; max-width: 893.398px;">
                            <div class="modal-content" style="width: 830px">
                                <div class="modal-body">
                                    <div class="box">
                                        <div class="box-title">
                                            <h3>
                                                <asp:Label ID="lblTitle" runat="server" Text="" />
                                            </h3>
                                        </div>
                                        <div class="box-content ">
                                            <div class="table-responsive">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <div id="divHistory" runat="server" class="table-responsive" visible="false">
                                                            <asp:GridView ID="gv" runat="server" AllowPaging="true" AutoGenerateColumns="false" BorderWidth="0px" CellSpacing="-1" CssClass="table header" EmptyDataText="No record found" GridLines="None" PageSize="30" ShowHeaderWhenEmpty="true" 
                                                                DataKeyNames="Attachment" OnRowDataBound="gv_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <HeaderStyle CssClass="col-md-2" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" CssClass="control-label" Text='<%# Eval("Date") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount (Rs.)">
                                                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                                                        <ItemStyle CssClass="text-right" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" CssClass="control-label" Text='<%# Eval("Amount")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Type">
                                                                        <HeaderStyle CssClass="col-md-1 text-center" />
                                                                        <ItemStyle CssClass="text-right" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" CssClass="control-label" Text='<%# Eval("Type")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <HeaderStyle CssClass="col-md-3 " />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesc" runat="server" CssClass="control-label" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Attachment">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="hlImage" runat="server" NavigateUrl='<%# Utility.GetImageURL(PMIU.WRMIS.Common.Configuration.EffluentWaterCharges , Convert.ToString(Eval("Attachment"))) %>'  Text="View Attachment" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="col-md-1" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerSettings Mode="NumericFirstLast" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                            </asp:GridView>
                                                        </div>
                                                        <div id="divAdd" runat="server" visible="false">
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label2" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Text="Surchage Amount " />
                                                                        <div class="col-sm-8 col-lg-8 controls">
                                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="integerInput required form-control" MaxLength="8" required="required" TabIndex="1" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label3" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Text="Surchage Type " />
                                                                        <div class="col-sm-8 col-lg-8 controls">
                                                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="required form-control" required="required" TabIndex="1" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Text="Attachment" />
                                                                        <div class="col-sm-8 col-lg-8 controls">
                                                                            <uc1:FileUploadControl ID="FileUploadControl1" runat="server" Size="1" />
                                                                            <asp:HyperLink ID="hlAtchmntAdd" runat="server" Text="Attachment" Visible="false" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label4" runat="server" CssClass="col-sm-4 col-lg-3 control-label" Text="Remarks" />
                                                                        <div class="col-sm-8 col-lg-8 controls">
                                                                            <asp:TextBox ID="txtRmrks" runat="server" Columns="10" CssClass="form-control commentsMaxLengthRow multiline-no-resize" MaxLength="50" Rows="5" TabIndex="4" TextMode="MultiLine" />
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
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save" ToolTip="Save" />
                                        <button aria-hidden="true" class="btn btn-default" data-dismiss="modal">
                                            Close
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                  <br />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
