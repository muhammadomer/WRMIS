<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CommandWaterLosses.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterLosses.CommandWaterLosses" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="aspCharts" %>
<%@ Register  tagPrefix="ajaxToolkit" assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="box">
        <div class="box-title">
            <h3>Command  Water Losses</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal"> 
                <asp:UpdatePanel ID="pnlSearchArea"  UpdateMode="Conditional" runat="server">                     
                    <ContentTemplate>
                       <div class="row">

                            <div class="col-md-6 "> 
                                <div class="form-group">
                                    <label for="ddlCommand" id="lblZone" class="col-sm-4 col-lg-3 control-label">Command</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="1" ID="ddlCommand" runat="server" required="required" CssClass="form-control" data-rule-required="true"  AutoPostBack="True" OnSelectedIndexChanged="ddlCommand_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="ddlYear" id="lblDivision" class="col-sm-4 col-lg-3 control-label">Year</label>
                                    <div class="col-sm-8 col-lg-9 controls">
                                        <asp:DropDownList TabIndex="3" ID="ddlYear" runat="server" required="required" CssClass="form-control" data-rule-required="true" Enabled="false"></asp:DropDownList>
                                    </div>
                                </div>  
                            </div>

                            <div class="col-md-6 "> 

                            <div class="form-group">
                                <label for="ddlReach" id="lblCircle" class="col-sm-4 col-lg-3 control-label">Reach</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList TabIndex="2" ID="ddlReach" runat="server" required="required" CssClass="form-control" data-rule-required="true" Enabled="false">
                                        <asp:ListItem Text="Select" Value="" /> 
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="ddlMonth" id="lblSubDivision" class="col-sm-4 col-lg-3 control-label">Month</label>
                                <div class="col-sm-8 col-lg-9 controls">
                                    <asp:DropDownList TabIndex="4" ID="ddlMonth" runat="server" required="required" CssClass="form-control" data-rule-required="true" Enabled="false">
                                        <asp:ListItem Text="Select" Value="" /> 
                                        <asp:ListItem Text="January" Value="1" /> 
                                        <asp:ListItem Text="February" Value="2" /> 
                                        <asp:ListItem Text="March" Value="3" /> 
                                        <asp:ListItem Text="April" Value="4" /> 
                                        <asp:ListItem Text="May" Value="5" /> 
                                        <asp:ListItem Text="June" Value="6" /> 
                                        <asp:ListItem Text="July" Value="7" /> 
                                        <asp:ListItem Text="August" Value="8" /> 
                                        <asp:ListItem Text="September" Value="9" /> 
                                        <asp:ListItem Text="October" Value="10" /> 
                                        <asp:ListItem Text="November" Value="11" /> 
                                        <asp:ListItem Text="December" Value="12" /> 
                                    </asp:DropDownList>
                                </div>
                            </div> 

                        </div> 

                        </div>
                    </ContentTemplate>                    
            </asp:UpdatePanel>
                
                <br />
                <div class="row">
                    <div class="col-md-6">
                        <div class="fnc-btn">
                            <asp:Button runat="server" ID="btnView" CssClass="btn btn-primary" Text="View" OnClick="btnView_Click"/>
                            <asp:Button runat="server" ID="btnViewGraph" CssClass="btn btn-success" Text="View Graph" OnClick="btnViewGraph_Click" Visible="false" />
                        </div>
                    </div>
                </div>
                

                <asp:UpdatePanel ID="pnlTblurData" runat="server" UpdateMode="Conditional"> 
                    <ContentTemplate>
                         <div class="row">
                          <div class="col-md-12">
                          <h3><asp:Label ID="lblheader" runat="server"></asp:Label></h3>
                          </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div style="text-align:right"> <asp:Label ID="lbUnits" runat="server" style="font-size:10px;text-align:right" CssClass="text-right"> All discharges are in Cusecs</asp:Label></div>
                                    

                                <div class="table-special"> 
                                    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="true"  ShowHeader ="true" EmptyDataText="No Record Found."
                                        OnRowDataBound="gvData_RowDataBound"  CssClass="table header table-lowpadd" BorderStyle="None" BorderWidth="0" GridLines="None">
                                    </asp:GridView>
                                </div>
                            </div>
                        </div> 
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID ="pnlCharts" runat="server" UpdateMode="Conditional" Visible = "false" >
                    <ContentTemplate>
                        <aspCharts:chart ID="waterLossChart"  runat="server" Height="600px" Width="900px" ImageStorageMode="UseImageLocation" >
 
                        </aspCharts:chart> 
                    </ContentTemplate>
                     
                </asp:UpdatePanel>
            </div> 

             
        </div> 
    </div>
</asp:Content>
