<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemStatusDivisionStore.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore.ItemStatusDivisionStore" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <%--  <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
        <ContentTemplate>--%>
    <div class="box">
        <div class="box-title">
            <h3>Update Item of Division Store</h3>
            <div class="box-tool">
                <a data-action="collapse" href="#"><%--<i class="fa fa-chevron-up"></i>--%></a>
            </div>
        </div>
        <div class="box-content">
            <div class="box-content">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-6 ">
                        <div class="form-group">
                            <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="col-lg-4 control-label" />
                            <div class="col-lg-8 controls">
                                <div class="input-group date" data-date-viewmode="years">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox ID="txtDate" TabIndex="5" runat="server" class="form-control date-picker required" size="16" type="text" required="true" AutoPostBack="false" OnTextChanged="txtDate_TextChanged" ></asp:TextBox>
                                    <span class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblZone" runat="server" Text="Division" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" AutoPostBack="True" required="required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lbl" runat="server" Text="Item Category" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" required="required" AutoPostBack="false" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                   <%--     <asp:ListItem Value="">Select</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="lblitm" runat="server" Text="Item Name" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" required="required" AutoPostBack="false" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                  <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Available Quantity" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:TextBox ID="txtAvailableQty" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control" MaxLength="8" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="Quantity Effected" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                     <asp:TextBox ID="txteffectedQty" runat="server" pattern="^(0|[0-9][0-9]*)$" class="integerInput  form-control required" required="required" MaxLength="8" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                      <div class="row">
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="Condition" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:DropDownList ID="ddlcondition" runat="server" CssClass="form-control required" required="required">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 ">
                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="Reason" CssClass="col-lg-4 control-label" />
                                <div class="col-lg-8 controls">
                                    <asp:TextBox ID="txtreason" runat="server" CssClass="form-control commentsMaxLengthRow required multiline-no-resize" TextMode="MultiLine" MaxLength="256" pattern="^[a-zA-Z0-9 ]+$"  Width="100%" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="fnc-btn">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"  />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn btn-default" Text="Back"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
      
            </div>
            <asp:HiddenField ID="hdnDivisionStoreID" runat="server" Value="0" />
        </div>

    </div>
<%--                    </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    InitilizeNumericValidation();
                    AddInputTextboxAttributes()
                }
            });
        };
    </script>--%>
</asp:Content>
