<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPunjabIndent.aspx.cs" MasterPageFile="~/Site.Master" Inherits="PMIU.WRMIS.Web.Modules.EntitlementDelivery.AddPunjabIndent" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">

        <div class="box">
            <div class="box-title">
                <h3>
                    <asp:Label runat="server" ID="lblHeader" Text="Add Punjab Indent"></asp:Label></h3>
            </div>
            <div class="box-content">
                <div class="form-horizontal table-striped">

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="addlblFromDate" class="col-sm-6 col-lg-5 control-label">From</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtFromDateAdd" ClientIDMode="Static" required="required" TabIndex="1" runat="server" ValidationGroup="P_I" class=" form-control required date-picker" size="16" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="server" ID="hfPunjabIndentID" />
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="addlblToDate" class="col-sm-6 col-lg-5 control-label">To</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <div class="input-group date" data-date-viewmode="years">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtToDateAdd" ClientIDMode="Static" TabIndex="2" required="required" ValidationGroup="P_I" runat="server" class=" form-control required date-picker" size="16" type="text"></asp:TextBox>
                                        <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="Thal" class="col-sm-6 col-lg-5 control-label">Thal</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtThal" ClientIDMode="Static" TabIndex="3" required="required" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '12.0');" runat="server" class=" form-control required decimalInput left" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblCJLinkatHead" class="col-sm-6 col-lg-5 control-label">C-J Link at Head</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtCJ" ClientIDMode="Static" TabIndex="4" required="required" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '25.0');" runat="server" class=" form-control required decimalInput" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblBelowChashmaBarrage" class="col-sm-6 col-lg-5 control-label">Below Chashma Barrage</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtBelowChashmaBarrage" ClientIDMode="Static" required="required" TabIndex="5" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '30.0');" class=" form-control required decimalInput" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblCRBC" class="col-sm-6 col-lg-5 control-label">CRBC</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtCRBC" ClientIDMode="Static" TabIndex="6" required="required" runat="server" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '2.0');" class=" form-control required decimalInput" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblGreaterThal" class="col-sm-6 col-lg-5 control-label">GreaterThal</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtGreaterThal" ClientIDMode="Static" required="required" TabIndex="7" ValidationGroup="P_I" oninput="javascript:NumberValueValidation(this, '10.0');" runat="server" class=" form-control required decimalInput" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblMangla" class="col-sm-6 col-lg-5 control-label">Mangla</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtMangla" ClientIDMode="Static" TabIndex="8" required="required" ValidationGroup="P_I" runat="server" oninput="javascript:NumberValueValidation(this, '70.0');" class=" form-control required decimalInput" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label id="lblRemarks" class="col-sm-6 col-lg-5 control-label">Remarks</label>
                                <div class="col-sm-6 col-lg-7 controls">
                                    <asp:TextBox ID="txtRemarks" ClientIDMode="Static" TabIndex="9" ValidationGroup="P_I" Style="resize: none" TextMode="MultiLine" MaxLength="250" Columns="10" Rows="5" runat="server" class=" form-control  text" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                     <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="&nbsp;Save" OnClick="btnSaveAccp_Click"/>
                                    <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="~/Modules/EntitlementDelivery/PunjabIndent.aspx?" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
