<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchIrrigatorFeedback.aspx.cs" EnableEventValidation="false" Inherits="PMIU.WRMIS.Web.Modules.IrrigatorsFeedback.SearchIrrigatorFeedback" %>


<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="box">
                <div class="box-title">
                    <h3>Search Irrigator Feedback</h3>
                </div>
                <div class="box-content">
                    <div class="form-horizontal">
                        <div class="form-horizontal">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblZone" runat="server" Text="Zone" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlZone" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblCircle" runat="server" Text="Circle" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlCircle" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlCircle_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblDivision" runat="server" Text="Division" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblChannelName" runat="server" Text="Channel Name" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlChannelName" runat="server" CssClass="form-control" AutoPostBack="True" Enabled="False">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblTailStatus" runat="server" Text="Tail Status From Daily Data" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlTailStatus" runat="server" CssClass="form-control" AutoPostBack="True">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-4 col-lg-3 control-label">Tail Side</label>
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <label class="checkbox-inline">
                                                <input runat="server" name="checkbox" id="chkFront" class="required" type="checkbox" value="Front">
                                                Front
                                            </label>
                                            &nbsp;&nbsp;
                                    <label class="checkbox-inline">
                                        <input runat="server" name="checkbox" id="chkLeft" class="required" type="checkbox" value="Left">
                                        Left
                                    </label>
                                            &nbsp;&nbsp;
                                    <label class="checkbox-inline">
                                        <input runat="server" name="checkbox" id="chkRight" class="required" type="checkbox" value="Right">
                                        Right
                                    </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblIrrigatorStatus" runat="server" Text="Status" CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:DropDownList ID="ddlIrrigatorStatus" runat="server" CssClass="form-control">
                                                <%--<asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="a">Active</asp:ListItem>
                                        <asp:ListItem Value="i">InActive</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 ">
                                    <div class="form-group">
                                        <asp:Label ID="lblIrrigatorMobileNo" runat="server" Text="Mobile No." CssClass="col-sm-4 col-lg-3 control-label" />
                                        <div class="col-sm-8 col-lg-9 controls">
                                            <asp:TextBox ID="txtIrrigatorMobileNo" runat="server" CssClass="form-control phoneNoInput" placeholder="XXXXXXXXXXX" onkeyup="PhoneNoWithLengthValidation(this, 11)" MaxLength="20" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="fnc-btn">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                        <%--<asp:HyperLink ID="hlAddIrrigator" runat="server" CssClass="btn btn-primary" Text="Add New Irrigator"></asp:HyperLink>--%>
                                        <%--<asp:HyperLink ID="hlAddIrrigator" runat="server" CssClass="btn btn-primary" Text="Add New Irrigator" NavigateUrl='<%# String.Format("~/Modules/IrrigatorsFeedback/AddIrrigator.aspx") %>'></asp:HyperLink>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="table-responsive">
                                <asp:GridView ID="gvIrrigatorFeedback" runat="server" EmptyDataText="No record found" DataKeyNames="ChannelClosed" AutoGenerateColumns="false" AllowPaging="true"
                                    CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None" ShowHeaderWhenEmpty="true" PageSize="10"
                                    OnRowDataBound="gvIrrigatorFeedback_RowDataBound" OnPageIndexChanged="gvIrrigatorFeedback_PageIndexChanged" OnPageIndexChanging="gvIrrigatorFeedback_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblIrrigatorID" runat="server" CssClass="control-label" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>--%>

                                                <asp:Label ID="lblChannelClosed" runat="server" CssClass="control-label" Text='<%# Eval("ChannelClosed") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField HeaderText="Zone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvZone" runat="server" CssClass="control-label" Text='<%# Eval("Zone") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Circle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvCircle" runat="server" CssClass="control-label" Text='<%# Eval("Circle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvDivision" runat="server" CssClass="control-label" Text='<%# Eval("Division") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Channel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvChannel" runat="server" CssClass="control-label" Text='<%# Eval("Channel") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tail Side Front">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTailFront" runat="server" CssClass="control-label" Text='<%# Eval("TF") %>'></asp:Label>
                                                <%--<asp:CheckBox ID="chkLoc" runat="server" CssClass="control-label" disabled="true"></asp:CheckBox>--%>
                                                <%--&#10003;--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tail Side Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTailLeft" runat="server" CssClass="control-label" Text='<%# Eval("TL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tail Side Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTailRight" runat="server" CssClass="control-label" Text='<%# Eval("TR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tail Status From Daily Data">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTailStatus" runat="server" CssClass="control-label" Text='<%# Eval("TailStatus") %>'></asp:Label>
                                                <asp:Label ID="lblgvTailStatusDate" runat="server" CssClass="control-label" Visible="false" Text='<%# Eval("TailStatusDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvIrrigatorName" runat="server" CssClass="control-label" Text='<%# Eval("IrrigatorName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mobile No. 1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvWaterTheft" runat="server" CssClass="control-label" Text='<%# Eval("Mobile1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mobile No. 2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvMobile2" runat="server" CssClass="control-label" Text='<%# Eval("Mobile2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvIrrigatorStatus" runat="server" CssClass="control-label" Text='<%# Eval("IrrigatorStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                                            <ItemTemplate>
                                                <%--<asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-primary btn_32 edit" ToolTip="Edit" />--%>
                                                <asp:HyperLink ID="hlAddFeedback" runat="server" CssClass="btn btn-primary btn_24 add-feedback" NavigateUrl='<%# String.Format("~/Modules/IrrigatorsFeedback/AddIrrigatorFeedback.aspx?IrrigatorID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Add Feedback"></asp:HyperLink>
                                                <asp:HyperLink ID="hlFeedbackHistory" runat="server" CssClass="btn btn-primary btn_24 history-feedback" NavigateUrl='<%# String.Format("~/Modules/IrrigatorsFeedback/IrrigatorFeedbackHistory.aspx?IrrigatorID={0}", Convert.ToString(Eval("ID"))) %>' ToolTip="Feedback History"></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-md-1" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
