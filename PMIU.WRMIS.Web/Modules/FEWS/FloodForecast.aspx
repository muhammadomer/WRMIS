<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FloodForecast.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FEWS.FloodForecast" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  
   

   
              
       
       
</asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderButtons" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /*.alignFewstext
        {      
           padding-left:5px;
           text-align: left;          
        }

     .alignFewsvalue
        {
         float:right;   
         text-align: right;       
         padding-right: 5px;        
         vertical-align: middle;
         font-size: 12px;   
        }

        .FewsLabel {
            background-color: #EDEDED;
            Width: 70px;
            color: #494964;
            text-align: right;
            font-family: 'Times New Roman';
            font-size: 12px;
            position: relative;
            float: left;
            height: 23px;
            width: 115px;
            margin-left: 2px;
            text-align:center;
                     
           
        }
         .GridViewFont
         {
            
             margin-top: 0px;
             font-family:'Times New Roman';
             font-size:12px;
            

         }*/
    </style>

    <div class="row">
        <div class="col-md-12">
            <div class="box">
                <div class="box-title">
                    <h3 id="forecastHeader">Flood Forecast for Rim and Downstream Stations</h3>
                </div>

                <div class="box-content">

                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-8">
                                <div class="form-group">
                                    <label class="col-sm-4 col-lg-3 control-label">Date/Time of Issue</label>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <asp:TextBox runat="server" ID="TBDataTimeOfIssue" type="text" class="form-control" ReadOnly="True"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>

                    <div id="main" runat="server">
                        <%-- <div class="InfrastructureRow">
            <div class="InfrastructureColumn" >
           
                <asp:Label ID="LblDateTimeOfIssue" CssClass="FewsLabel"   runat="server">Date/Time of Issue</asp:Label>
                <asp:TextBox ID="TBDataTimeOfIssue" CssClass="TextBoxGeneral TextBoxXmlDate" runat="server" ReadOnly="True"></asp:TextBox>
            </div>
        </div>--%>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="GridViewFloodForecast"
                                        runat="server" AutoGenerateColumns="False" AllowPaging="false" PageSize="15"
                                        CellPadding="4" GridLines="None"
                                        EmptyDataText="No Record Found" AllowSorting="true" CssClass="table header" OnRowDataBound="GridViewFloodForecast_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="River">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastRiver" />--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRiver" runat="server" Text='<%# Eval("RIVERS")  %>' CssClass="alignFewstext" Font-Bold="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Station">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastStations" />--%>
                                                <ItemTemplate>
                                                    <%-- <asp:Label ID="lblStation" runat="server" Text='<%# Eval("<a>Stations</a>")  %>'></asp:Label>--%>
                                                    <%-- <asp:HyperLink ID="lblStation" runat="server" Text='<%# Eval("Stations") %>'></asp:HyperLink>--%>

                                                    <asp:HyperLink ID="lblStation" runat="server" Text='<%# Eval("Stations") %>' CssClass="alignFewstext" ForeColor="Blue" NavigateUrl='<%# String.Format("~/Modules/FEWS/FEWSGraph.aspx?Stations={0}&RIVERS={1}", Eval("Stations"), Eval("RIVERS")) %>'>
                                 
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Design Capacity (Cusec)">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastDesignCapacity" />--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDCapacity" runat="server" Text='<%# Eval("Design Capacity")  %>' CssClass="alignFewsvalue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="text-right" />
                                                <HeaderStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Forecast for next 30 days - Max. Downstream (Cusec)">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastMaxDownstream" />--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblForecas" runat="server" Text='<%# Eval("Forecast for next 30 days")  %>' CssClass="alignFewsvalue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" CssClass="text-right" />
                                                <HeaderStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Forecast for next 30 days - Time of Max. Downstream (Cusec)">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastTimeOfMaxDownstream" />--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblForecast_1" runat="server" Text='<%# Eval("Forecast for next 30 dayss")  %>' CssClass="alignFewsvalue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" CssClass="text-left" />
                                                <HeaderStyle CssClass="text-left" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Forecasted Flood Level">
                        <HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastLevel" />
                        <ItemTemplate>
                              <asp:Label ID="lblForecasted" runat="server" Text='<%# Eval("Forecasted Flood Level")  %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Danger Level - Very High Flood (Cusec)">
                                                <%--<HeaderStyle CssClass="GridViewHeaderGeneral GridViewHeaderFloodForecastDangerLevel" />--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDangerLevel" runat="server" Text='<%# Eval("Danger Level")  %>' CssClass="alignFewsvalue"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" CssClass="text-right" />
                                                <HeaderStyle CssClass="text-right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                    </div>

                    <div id="SeasonMessage" style="width: auto; height: auto; margin-top: 250px" runat="server" visible="false">
                        <label id="Message" runat="server" visible="true" style="margin-left: 200px; margin-top: 300px; font-size: 18px;"></label>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
