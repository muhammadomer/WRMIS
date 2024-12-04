<%@ Page Title="AddAuctionAssets" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddAuctionAssets.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.Auctions.AddAuctionAssets" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Modules/Auctions/Controls/AuctionNotice.ascx" TagPrefix="uc1" TagName="AuctionNotice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

       <div class="box">
        <div class="box-title">
            <h3>Assets</h3>
        </div>
          
        <div class="box-content">
             <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline">
                            <ContentTemplate>
            <div class="form-horizontal">
                <uc1:AuctionNotice runat="server" id="AuctionNotice" />
                   <asp:GridView ID="gvAuctionAssets" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                       DataKeyNames="AuctionAssetID,LevelID,GroupIndividual,CategoryID,SubCategoryID,NameID,Name,AttributeTypeID,AttributeValue,CreatedBy,CreatedDate"
                       ShowHeaderWhenEmpty="True" OnRowDataBound="gvAuctionAssets_RowDataBound" OnRowCommand="gvAuctionAssets_RowCommand" OnRowEditing="gvAuctionAssets_RowEditing"
                        OnRowDeleting="gvAuctionAssets_RowDeleting" AllowPaging="True" OnPageIndexChanging="gvAuctionAssets_PageIndexChanging" OnRowUpdating="gvAuctionAssets_RowUpdating"
                        OnRowCancelingEdit="gvAuctionAssets_RowCancelingEdit" CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Level">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlLevel" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("Level") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group/Individual">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlGroupIndividual" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupIndividual_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupIndividual" runat="server" Text='<%# Eval("GroupIndividual") %>'></asp:Label>
                                    </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                    </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub-Category">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategory" runat="server" Text='<%# Eval("SubCategory") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Name">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnSelectedIndexChanged" Visible="true">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtGroupName" type="text" class="form-control" Visible="false"> </asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Attribute Type">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control" ID="ddlAttributeType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributeType_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAttributeType" runat="server" Text='<%# Eval("AttributeType") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Attribute Value/Qty">
                                <EditItemTemplate>
                                    <%--<asp:DropDownList CssClass="form-control required" required="required" ID="ddlAttributeValue" runat="server" Visible="false">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:TextBox CssClass="form-control decimalIntegerInput" ID="txtAttributeValue" runat="server" minlength="3" MaxLength="5" Enabled="false" AutoPostBack="true" OnTextChanged="txtAttributeValue_OnTextChanged"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAttributeValue" runat="server" Text='<%# Eval("AttributeValue") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                         
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                        <asp:Button ID="btnGaugeInspection" runat="server" Text="" CommandName="AddAuctionAsset" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" Visible="<%# base.CanAdd %>"  />
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlActionGaugeInspection" runat="server" HorizontalAlign="Center">
                                        <asp:Button ID="btnAssets" runat="server" Text="" CommandName="Assets" CssClass="btn btn-primary btn_24 view" ToolTip="Assets" formnovalidate="formnovalidate" Visible="false" />
                                        <asp:Button ID="btnDetails" runat="server" Text="" CommandName="ItemDetails" CssClass="btn btn-primary btn_24 details" ToolTip="Details" formnovalidate="formnovalidate" />
                                        <asp:Button ID="btnEdit" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />
                                        <asp:Button ID="lbtnDeleteGaugeInspection" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" Visible="<%# base.CanDelete %>" />
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionGaugeInspection" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveGaugeInformation" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelGaugeInspection" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                 </div>

              </ContentTemplate>
                        </asp:UpdatePanel>
               <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="col-md-12">
                                <%--<asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click"  />--%>
                                <asp:HyperLink ID="hlBack" runat="server" CssClass="btn">&nbsp;Back</asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>

            <div id="AssetsGrouped" class="modal fade">

                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">


                                    <div class="box">
                                        <div class="box-title">
                                            <h5>Assets</h5>
                                        </div>
                                        <div class="modal-body">

                                            <div class="form-horizontal">
                                                 <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                              <div class="row">

                  
                                                           <div class="col-md-12">
                                <asp:GridView ID="gvGroupAsset" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found"
                       DataKeyNames="AuctionAssetItemID,AuctionAssetID,AssetItemID,AttributeTypeID,AttributeValue,LotQuantity,CreatedBy,CreatedDate"
                       ShowHeaderWhenEmpty="True" OnRowCommand="gvGroupAsset_OnRowCommand" OnRowDataBound="gvGroupAsset_RowDataBound" OnRowUpdating="gvGroupAsset_OnRowUpdating"
                       AllowPaging="True" OnRowCancelingEdit="gvGroupAsset_RowCancelingEdit" OnRowDeleting="gvGroupAsset_RowDeleting"
                        CssClass="table header" BorderWidth="0px" CellSpacing="-1" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control required" required="required" ID="ddlNameGrouped" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNameGrouped_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Attribute Type">
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control" ID="ddlAttributeTypeGrouped" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributeTypeGrouped_OnSelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAttributeType" runat="server" Text='<%# Eval("AttributeType") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Attribute Value/Qty">
                                <EditItemTemplate>
                                    <%--<asp:DropDownList CssClass="form-control required" required="required" ID="ddlAttributeValue" runat="server" Visible="false">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:TextBox CssClass="form-control IntegerInput" ID="txtAttributeValueGrouped" runat="server" minlength="3" MaxLength="250" Enabled="false" AutoPostBack="true" OnTextChanged="txtAttributeValueGrouped_OnTextChanged"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAttributeValue" runat="server" Text='<%# Eval("AttributeValue") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                         
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Panel ID="pnlAdd" runat="server" HorizontalAlign="center">
                                        <asp:Button ID="btnGroupedAssets" runat="server" Text="" CommandName="AddGroupedAssets" ToolTip="Add" formnovalidate="formnovalidate" CssClass="btn btn-success btn_add plus" />
                                    </asp:Panel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlActionGroupedAssets" runat="server" HorizontalAlign="Center">
                                       <%--<asp:Button ID="btnEditGroupedAssets" runat="server" Text="" CommandName="Edit" CssClass="btn btn-primary btn_24 edit" ToolTip="Edit" formnovalidate="formnovalidate" />--%>
                                        <asp:Button ID="lbtnDeleteGroupedAssets" runat="server" Text="" CommandName="Delete" formnovalidate="formnovalidate"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="btn btn-danger btn_24 delete" ToolTip="Delete" />
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Panel ID="pnlEditActionGroupedAssets" runat="server" HorizontalAlign="Center">
                                        <asp:Button runat="server" ID="btnSaveGroupedAssets" CommandName="Update" ToolTip="Save" CssClass="btn btn-primary btn_24 save" />
                                        <asp:Button ID="lbtnCancelGroupedAssets" runat="server" Text="" CommandName="Cancel" CssClass="btn btn-danger btn_24 cross" ToolTip="Cancel" formnovalidate="formnovalidate" />
                                    </asp:Panel>
                                </EditItemTemplate>
                                <HeaderStyle CssClass="col-md-2" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                            </div>
                        </div>
                                                  </ContentTemplate>
                                                </asp:UpdatePanel>   
                                            </div>
                                            <div class="modal-footer">
                                                <button id="btnClose" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                                                <%--<asp:Button TabIndex="10" ID="LinkButton" runat="server" Text="Save" CssClass="btn btn-primary"></asp:Button>--%>
                                            </div>
                                        </div>
                                    </div>



                                </div>
                            </div>

                        </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnAuctionNoticeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnAuctionAssetID" runat="server" Value="0" />
</asp:Content>
