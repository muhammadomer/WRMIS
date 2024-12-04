<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddClosureWorkPlan.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.ClosureOperations.CWP.AddClosureWorkPlan" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
 <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="lblAdCWP" runat="server">Add Closure Work Plan</h3>
                </div>

                <div class="box-content"> 
                    <div class="form-horizontal">
                        <div class="row" >
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Title" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtCWPTitle" required="required" TabIndex="1" CssClass="required form-control" MaxLength="50"  />
                                        <asp:HiddenField ID="hdnFldID" runat="server" Value="0" ></asp:HiddenField>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Year" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlYear" runat="server" TabIndex="2" required="required" CssClass="required form-control" >
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>  
                        </div>  
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList ID="ddlDivision"  runat="server" TabIndex="3" required="required" CssClass="required form-control" >
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Comments" CssClass="col-sm-4 col-lg-3 control-label" />
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:TextBox runat="server" ID="txtComments" TabIndex="4" 
                                            TextMode="MultiLine" MaxLength="250"  
                                            Rows="5" Columns="10"  
                                            CssClass="form-control commentsMaxLengthRow multiline-no-resize" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="searchButtonsDiv">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnSaveClosureWorkPlan"  runat="server" Text="Save" CssClass="btn btn-primary" ToolTip="Save"  OnClick="btnSaveClosureWorkPlan_Click" Enabled='<%# base.CanAdd %>'/>  
                                     <asp:HyperLink ID="hlCWPback" runat="server" NavigateUrl="~/Modules/ClosureOperations/CWP/ClosureWorkPlan.aspx?RestoreState=1" CssClass="btn btn-default">&nbsp;Back</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                        
                    </div>  
        </div> 
            </div>
        </div>
    </div>
    <script src="../../../Scripts/IrrigationNetwork/BusinessValidations.js"></script>
    <script src="../../../Scripts/jquery-1.10.2.js"></script>
    
 </asp:Content>