<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateBill.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.GenerateBill" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Generate Bill</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:RadioButton CssClass="radio-inline" required="required" Checked="true" ID="rbEffluentWater" runat="server" GroupName="ViewType" Text="Effluent Water" />
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbCanalSpecialWater" runat="server" GroupName="ViewType" Text="Canal Special Water" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Financial Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlFinancialYear" runat="server" disabled="true" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Bill Issue Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtBillIssueDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Bill Due Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtBillDueDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Taxes</label>
                                    <div class="col-sm-8 col-lg-9 controls" >
                                        <asp:CheckBox CssClass="radio-inline" ID="cbApplicableTaxes" Text="&nbsp;&nbsp;Include Applicable Taxes"  runat="server" Style="margin-left: -12px;"  />
                                       
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <h3>
                                        <label class="col-sm-4 col-lg-3 control-label">Adjustments</label></h3>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <asp:Label ID="lblAdjustment" Text="Adjustment" runat="server" CssClass="col-xs-4 col-lg-3 control-label"></asp:Label>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:DropDownList ID="ddlAdjustment1" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Select" Value="" />
                                            <asp:ListItem Text="+" Value="Add" />
                                            <asp:ListItem Text="-" Value="Sub" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:DropDownList ID="ddlAdjustment2" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Select" Value="" />
                                            <asp:ListItem Text="% age of bill (excluding taxes)" Value="2" />
                                            <asp:ListItem Text="(Rs.) Fix" Value="1" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-lg-3 controls">
                                        <asp:TextBox ID="txtAdjustment" runat="server" ClientIDMode="Static" CssClass="form-control integerInput" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Reason</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtReason" runat="server" MaxLength="250" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnBillGeneration" ClientIDMode="Static" OnClick="btnBillGeneration_Click" runat="server" CssClass="btn btn-primary" align="center" Text="Generate Bill" OnClientClick="return confirm('Are you sure you want to continue ?');"></asp:Button>
                                </div>
                            </div>
                        </div>

                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
