<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.EWC.Payments" %>
<%@ MasterType VirtualPath="~/Site.Master" %>  

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
                    <h3>Payments</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group"> 
                                    <label class="col-sm-4 col-lg-3 control-label">Search By</label>
                                    <div class="col-sm-8 col-lg-3 controls">
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbIndName" runat="server" AutoPostBack="true" GroupName="ViewType" Text="Industry No." OnCheckedChanged="rb_CheckedChanged" Checked="true" style="margin-top:-5px;"/>
                                    </div>
                                    <div class="col-sm-8 col-lg-3">
                                        <asp:RadioButton CssClass="radio-inline" required="required" ID="rbBillNo" runat="server" AutoPostBack="true" GroupName="ViewType" Text ="Bill No." OnCheckedChanged="rb_CheckedChanged" style="margin-top:-5px;"/> 
                                    </div> 
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Number</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox CssClass="form-control" ID="txtNo" runat="server" MaxLength="100" required="required"  />
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSearch" Visible="<%# base.CanView %>" runat="server" Text="Search" CssClass="btn btn-primary" ToolTip="Search" OnClick="btn_Click" formnovalidate="formnovalidate"/> 
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblNoSerivce" Text="" runat="server"></asp:Label>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                            <div class="table-responsive" >
                                <table class="table tbl-info">
                                    <tr>
                                        <th> <div><strong>Industry Name</strong></div> </th>
                                        <th> <div ><strong>Industry ID</strong></div> </th>
                                    </tr>
                                    <tr> 
                                        <td> <asp:Label ID="lblName" runat="server" ></asp:Label> </td>
                                        <td> <asp:Label ID="lblID" runat="server"></asp:Label> </td>
                                    </tr>
                                    <tr>
                                        <th> <div><strong>Industry Type</strong></div> </th> 
                                    </tr>
                                    <tr>
                                        <td> <asp:Label ID="lblType" runat="server" ></asp:Label> </td> 
                                    </tr>
                                     <tr>
                                        <th> <div><strong>Effluent Water Balance (Rs.)</strong></div> </th>
                                        <th> <div ><strong>Canal Special Water Balance (Rs.)</strong></div> </th>
                                    </tr>
                                    <tr> 
                                        <td> <asp:Label ID="lblEffBlnc" runat="server" ></asp:Label> </td>
                                        <td> <asp:Label ID="lblCnlBlnc" runat="server"></asp:Label> </td>
                                    </tr>
                                </table>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group"> 
                                        <label class="col-sm-4 col-lg-3 control-label">Service Type</label>
                                        <div class="col-sm-8 col-lg-4 controls">
                                            <asp:RadioButton CssClass="radio-inline" required="required" ID="rbEffluent" runat="server" AutoPostBack="true" GroupName="ServiceType" Text="Effluent Waters" OnCheckedChanged="rbService_CheckedChanged"  style="margin-top:-5px;"/>
                                        </div>
                                        <div class="col-sm-8 col-lg-5">
                                            <asp:RadioButton CssClass="radio-inline" required="required" ID="rbCanal" runat="server" AutoPostBack="true" GroupName="ServiceType" Text ="Canal Special Waters" OnCheckedChanged="rbService_CheckedChanged"  style="margin-top:-5px;" /> 
                                        </div> 
                                         
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="pnlPayment" runat="server" Visible="false" GroupingText="Deposit Details"> 
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Amount (Rs.)</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox CssClass="integerInput form-control required" required="required" ID="txtDepositAmount" runat="server" MaxLength="9"/>
                                            </div>
                                        </div>
                                    </div>  
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Date</label>
                                             <div class="col-sm-8 col-lg-9 controls">
                                                <div class="input-group date" data-date-viewmode="years">
                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txtDepositDate" runat="server" CssClass="form-control date-picker required" required="required" size="16" type="text"></asp:TextBox>
                                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                </div>

                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Cheque # / Challan #</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:TextBox CssClass="form-control required" required="required" ID="txtChequeNo" runat="server" MaxLength="100" />
                                            </div>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Bank</label>
                                             <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control required" required="required" />
                                            </div>
                                        </div>
                                    </div>  
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="fnc-btn">
                                            <asp:Button ID="btnSubmit" Visible="<%# base.CanView %>" runat="server" Text="Submit" CssClass="btn btn-primary" ToolTip="Submit" OnClick="btn_Click"/> 
                                            <asp:Button ID="btnCancel" Visible="<%# base.CanView %>" runat="server" Text="Cancel" CssClass="btn btn-default" ToolTip="Cancel" OnClick="btn_Click" formnovalidate="formnovalidate"/> 
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel> 
                        </asp:Panel>
  <%--                      <div id="divSearchCriteria"> 

                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Industry Name</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox CssClass="form-control" ID="txtIndName" runat="server" MaxLength="100" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" id="searchButtonsDiv">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSubmit" Visible="<%# base.CanView %>" runat="server" Text="Submit" CssClass="btn btn-primary" ToolTip="Submit"/> 
                                    </div>
                                </div>
                            </div>

                        </div>--%>

                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
