<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DivisionalWaterLosses.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterLosses.DivisionalWaterLosses" %> 
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3>Divisional Water Losses</h3>
                </div>
                <div class="box-content">

                    <div class="form-horizontal"> 
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Zone</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList required="required"  CssClass="form-control" ID="ddlZone" runat="server" AutoPostBack="true"  Enabled="false" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value=""/>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Circle</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList required="required"  CssClass="form-control" ID="ddlCircle" runat="server" AutoPostBack="true"  Enabled="false" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value=""/>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">                            
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Division</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList required="required"  CssClass="form-control" ID="ddlDiv" runat="server" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Select" Value=""/>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div> 
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label"></label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="rbYearly" runat="server" AutoPostBack="true" GroupName="ViewType" Text="Yearly" OnCheckedChanged="rb_CheckedChanged"/>
                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="rbMnthly" runat="server" AutoPostBack="true" GroupName="ViewType" Text ="Monthly" OnCheckedChanged="rb_CheckedChanged"/>
                                                <asp:RadioButton CssClass="radio-inline" required="required" ID="rdDaily" runat="server" AutoPostBack="true" GroupName="ViewType" Text="Daily" OnCheckedChanged="rb_CheckedChanged"/>
                                            </div>
                                        </div>
                                    </div>
                                </div> 
                         
                            
                                <div class="row"  id ="divYearly" runat="server" Visible="false">
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">From Year</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList  required="required" CssClass="form-control" ID="ddlFromYear" runat="server" Enabled="false" >
                                                    <asp:ListItem Text="Select" Value="" /> 
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">To Year</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList  required="required" CssClass="form-control" ID="ddlToYear" runat="server" Enabled="false" >
                                                    <asp:ListItem Text="Select" Value="" /> 
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id ="divMonthly" runat="server" Visible="false">
                                <div class="row"  >
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Season</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList required="required" CssClass="form-control" ID="ddlSeason" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSeason_SelectedIndexChanged" >
                                                    <asp:ListItem Text="Select" Value="" /> 
                                                    <asp:ListItem Text="Rabi" Value="1" />
                                                    <asp:ListItem Text="Kharif" Value="2" /> 
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 ">
                                        <div class="form-group">
                                            <label class="col-sm-4 col-lg-3 control-label">Year</label>
                                            <div class="col-sm-8 col-lg-9 controls">
                                                <asp:DropDownList required="required" CssClass="form-control" ID="ddlMonthlyYear" runat="server"  Enabled="false" >
                                                    <asp:ListItem Text="Select" Value="" />  
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                  </div> 
                            </div>

                                <div class="row" id="divDaily" runat="server" Visible="false">
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
                            <div class="col-md-9">
                                <h3><asp:Label ID="lblheader" runat="server" style="float: left;" ></asp:Label></h3>
                            </div> 
                            <div class="col-md-3" >
                                <div class="fnc-btn"  style="margin-top:0px;">
                                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" ToolTip="View" OnClick="btnView_Click" style="float: right;" />  
                                </div>
                            </div>
                        </div> 
                    </div>
                    <%--<div class="row">
                          <div class="col-md-12">
                          <h3><asp:Label ID="lblheader" runat="server"></asp:Label></h3>
                          </div>
                    </div>
                    --%>
                    <div class="row">
                        <div class="col-md-12"> 
                                <div id="dailyLG" runat="server"  visible ="false"  style="overflow-x: auto;overflow-y: hidden;-webkit-overflow-scrolling: touch;-ms-overflow-style: -ms-autohiding-scrollbar;"   > 
                                    <div style="text-align:right"> <asp:Label ID="lbUnits" runat="server" style="font-size:10px;text-align:right" CssClass="text-right"> All discharges are in Cusecs</asp:Label></div>
                                    <asp:Table ID="tblLG" runat="server"  CssClass="table header table-lowpad" />
                                </div>
                                <br />
                               
                        </div>
                         <div class="col-md-12 text-bold" id="lgTotal" runat="server"  visible ="false" >
                             <div class="row">
                                    <div class="col-md-3 text-right">Average Divisional Discharge</div><div class="col-md-9"><asp:Label ID="albl" runat="server"></asp:Label></div>
                                 </div><div class="row">
                                    <div class="col-md-3 text-right">Average Loss</div><div class="col-md-9"><asp:Label ID="blbl" runat="server"></asp:Label></div>
                                     </div><div class="row">
                                    <div class="col-md-3 text-right">Percentange Loss</div><div class="col-md-9"><asp:Label ID="clbl" runat="server"></asp:Label> %</div>
                                         </div>
                                </div> 
                             <div class="table-responsive" id="divError" runat="server" visible ="false"> 
                                <label style="margin-left:10px;">  No record found.</label>
                            </div>
                    </div>

                </div>
            </div>
        </div>
    </div> 
</asp:Content>
