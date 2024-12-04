<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SBEAndSDOWorkingFields.ascx.cs" Inherits="PMIU.WRMIS.Web.Modules.WaterTheft.Controls.SBEAndSDOWorkingFields" %>


<h5>SBE Working</h5>
<hr />

<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblSBECanaWireNo" runat="server" Text="Sub Engineer Canal wire #"></asp:Label>
            <div class="col-sm-8 col-lg-9 controls">
                <asp:TextBox class="form-control" ID="txtSBECanaWireNo" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblSBECanaWireDate" runat="server" Text="Sub Engineer Canal wire Date"></asp:Label>
            <div class="col-sm-8 col-lg-9 controls">
                <asp:TextBox class="form-control" ID="txtSBECanaWireDate" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblDateOfClosing" runat="server" Text="Date of closing/Repair"></asp:Label>
            <div class="col-sm-8 col-lg-9 controls">
                <asp:TextBox class="form-control" ID="txtClosingDate" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>
    </div>
</div>
<br />
<h5>SDO Working</h5>
<hr />

<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lblSDOCanalWireNo" runat="server" Text="SDO Canal wire #"></asp:Label>
            <div class="col-sm-8 col-lg-9 controls">
                <asp:TextBox class="form-control" ID="txtSDOCanalWireNo" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="form-group">
            <label class="col-sm-4 col-lg-3 control-label">SDO Canal Wire Date</label>
            <div class="col-sm-8 col-lg-9 controls">
                <div class="input-group date" data-date-viewmode="years">
                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                    <asp:TextBox ID="txtSDOCanaWireDate" runat="server" CssClass="form-control date-picker required" required="true" type="text"></asp:TextBox>
                    <span id="spanDateID" runat="server" class="input-group-addon clear" style="background-color: #fff;"><i class="fa fa-times"></i></span>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-md-6">
        <div class="form-group">
            <asp:Label class="col-sm-4 col-lg-3 control-label" ID="lbltaComments" runat="server" Text="Remarks"></asp:Label>
            <div class="col-sm-8 col-lg-9 controls">
                <textarea class="form-control" id="taComments" runat="server"></textarea>
            </div>
        </div>
    </div>
</div>







