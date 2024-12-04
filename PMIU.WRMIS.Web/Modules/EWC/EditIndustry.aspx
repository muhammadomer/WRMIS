<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditIndustry.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.EditIndustry" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Edit Industry</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList required="required" CssClass="required form-control" ID="ddlType" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry No.</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtNo" runat="server" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry Name</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox required="required" CssClass="required form-control" ID="txtName" runat="server" MaxLength="50" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Industry Status</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList required="required" CssClass="required form-control" ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="Functional" Value="1" />
                                            <asp:ListItem Text="Non Functional" Value="2" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">NTN No.</label>
                                    <div class="col-sm-3 col-lg-6 controls">
                                        <asp:TextBox ID="txtNTNL" autofocus="autofocus" runat="server" CssClass="form-control integerInput" MaxLength="7"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        -
                                    </div>
                                    <div class="col-sm-2 col-lg-2 controls">
                                        <asp:TextBox ID="txtNTNR" runat="server" CssClass="form-control decimalIntegerInput" MaxLength="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Address</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox required="required" CssClass="required form-control" ID="txtAdrs" runat="server" MaxLength="250" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Phone No.</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox ID="txtPhnNo" CssClass="form-control required" runat="server" required="true" MaxLength="100"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Fax</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <%--<asp:TextBox ID="txtFax" CssClass="form-control" runat="server" MaxLength="11" MinLength="11" placeholder="XXXXXXXXXXX" pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox> --%>
                                        <asp:TextBox ID="txtFax" CssClass="form-control" runat="server" MaxLength="25" placeholder="XXXXXXXXXXX"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Email</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server" TextMode="Email" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" class="form-control" MaxLength="75" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Location: Long.</label>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtX" autofocus="autofocus" runat="server" placeholder="x-axis" CssClass="form-control" MaxLength="20" TextMode="Number"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1 col-lg-1 controls">
                                        Lat.
                                    </div>
                                    <div class="col-sm-3 col-lg-4 controls">
                                        <asp:TextBox ID="txtY" runat="server" placeholder="y-axis" CssClass="form-control" MaxLength="20" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="pnlPlant" runat="server" GroupingText="Water Treatment Plant Details">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Plant Exist ?</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlPlantExists" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                <asp:ListItem Text="Select" Value="" />
                                                <asp:ListItem Text="Yes" Value="1" />
                                                <asp:ListItem Text="No" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Plant Condition</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList CssClass="form-control" ID="ddlPlantCondition" runat="server" Enabled="false" AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" />
                                                <asp:ListItem Text="Functional" Value="1" />
                                                <asp:ListItem Text="Non Functional" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlcnctDtl" runat="server" GroupingText="Contact Person Details">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Name</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtCnctName" CssClass="form-control required" runat="server" MinLenght="3" MaxLength="90" required="true" onkeyup="InputValidation(this)" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Cell No</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtCnctCellNo" CssClass="form-control required" runat="server" required="true" MinLenght="11" MaxLength="11" placeholder="XXXXXXXXXXX" pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">CNIC</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control" ID="txtCnctCNIC" runat="server" MaxLength="13" MinLenght="13" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Email</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control" ID="txtCnctEmail" runat="server" TextMode="Email" pattern="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" MaxLength="75" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlService" runat="server" GroupingText="Services">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:CheckBox CssClass="radio-inline" ID="cbSrvcEffluent" Text="&nbsp;&nbsp;Effluent Waters" runat="server" Enabled="false" Style="margin-left: 5pt;" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:CheckBox CssClass="radio-inline" ID="cbSrvcCanal" Text="&nbsp;&nbsp;Canal Special Waters" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save" OnClick="btn_Click" />
                                    <asp:Button ID="btnSave2" Visible="false" runat="server" Text="Save and Edit Serivces Next" CssClass="btn btn-primary" ToolTip="Save and Edit Serivces Next" OnClick="btn_Click" />
                                    <asp:HyperLink ID="hlCWPback" runat="server" NavigateUrl="~/Modules/EWC/Industry.aspx?RestoreState=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
