<%@ Page Title="AddDrain" MasterPageFile="~/Site.Master" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="AddDrain.aspx.cs" Inherits="PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData.Drain.AddDrain" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="box-title">
            <h3 runat="server" id="h3PageTitle">Add Drain</h3>
        </div>
        <div class="box-content">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-12">
                        <h3>Drain</h3>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Name</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtDrainName" runat="server" required="required" CssClass="form-control required" TabIndex="1" MaxLength="150"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Length(ft)</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtLength" runat="server" CssClass="form-control decimalInput required" required="required" Type="number" onkeypress="return this.value.length<=7" TabIndex="2"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Catchment Area(sq ft)</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtCatchmentArea" runat="server" Type="Number" CssClass="form-control decimalInput" TabIndex="3" onkeypress="return this.value.length<=8"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Build Up Area Name</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtMajorBuildUpArea" runat="server" CssClass="form-control" TabIndex="4" MaxLength="150"></asp:TextBox>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Status</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:RadioButtonList ID="RadioButtonListStatus" runat="server" RepeatDirection="Horizontal" TabIndex="5">
                                        <asp:ListItem Text="Active" Value="1" Selected="True" style="margin-left: 13px;" />
                                        <asp:ListItem Text="Inactive" Value="0" style="margin-left: 25px;" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <h3>At 0 RD</h3>

                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Designed Discharge(Cs)</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtDesignedDischarg" runat="server" Type="Number" CssClass="form-control decimalInput required" required="required" TabIndex="6" onkeypress="return this.value.length<=4"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Bed Width(ft)</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtBedWidth" runat="server" CssClass="form-control decimalInput" Type="Number" TabIndex="7" onkeypress="return this.value.length<=2"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Full Supply Depth(ft)</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtFullCapacityDepth" runat="server" Type="Number" CssClass="form-control decimalInput" TabIndex="8" onkeypress="return this.value.length<=2"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 col-lg-4 control-label">Description</label>
                                <div class="col-sm-7 col-lg-8 controls">
                                    <asp:TextBox ID="txtDescription" TextMode="multiline" Columns="30" Rows="5" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" id="divSave">
                    <div class="col-md-12">
                        <asp:Button class="btn btn-primary" ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" TabIndex="10" />
                        <asp:HyperLink ID="hlBack" runat="server" CssClass="btn" TabIndex="11">&nbsp;Back</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField ID="hdnDrainID" runat="server" Value="0" />
</asp:Content>


