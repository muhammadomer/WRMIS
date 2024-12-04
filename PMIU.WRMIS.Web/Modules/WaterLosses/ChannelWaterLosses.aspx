<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelWaterLosses.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterLosses.ChannelWaterLosses" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Channel Water Losses</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Canal System</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlMainCanal" runat="server" AutoPostBack="true"  Enabled="false" OnSelectedIndexChanged="ddlMainCanal_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value=""/>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Branch Canal</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlBranch" runat="server" AutoPostBack="true"  Enabled="false" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value=""/>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Distributary</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlDistributry" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlDistributry_SelectedIndexChanged" >
                                            <asp:ListItem Text="Select" Value=""/>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Minor</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlMinor" runat="server"  AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlMinor_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="" /> 
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Sub Minor</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList CssClass="form-control" ID="ddlSubMinor" runat="server" Enabled="false">
                                            <asp:ListItem Text="Select" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">From Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtFromDate" TabIndex="4" required="required" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 ">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">To Date</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <div class="input-group date" data-date-viewmode="years">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtToDate" TabIndex="4" required="required" runat="server" class=" form-control date-picker" size="16" type="text"></asp:TextBox>
                                            <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="fnc-btn">
                                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" ToolTip="View" OnClick="btnView_Click"/> 
                                    <asp:Button ID="btnReset" formnovalidate="formnovalidate" runat="server" Text="Reset" CssClass="btn btn-success" ToolTip="Reset page" OnClick="btnReset_Click" OnClientClick="return;"/>
                                </div>
                            </div>
                        </div> 
                    </div>
                    
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive"  ID ="divData" runat="server">
                                <div style="text-align:right"> <asp:Label ID="lbUnits" runat="server" style="font-size:10px;text-align:right" CssClass="text-right"> All discharges are in Cusecs</asp:Label></div>
                                    
                                <asp:Table id="tblChnlWtrLoses" runat="server" CssClass="table header"/>
                            </div> 
                            <div class="table-responsive" id="divError" runat="server" visible ="false"> 
                                No record found.
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div> 
</asp:Content>
