<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="PMIU.WRMIS.Web.ForgotPassword" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>WRMIS - ForgotPassword</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link rel="shortcut icon"   type="image/x-icon" href="/Design/img/favicon.ico" />

    <!--base css styles-->
    <link rel="stylesheet" href="/Design/assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="/Design/assets/font-awesome/css/font-awesome.min.css">
    <!--flaty css styles-->
    <link rel="stylesheet" href="/Design/css/flaty.css?v=0">
    <link rel="stylesheet" href="/Design/css/flaty-responsive.css">
    <!--page specific css styles-->
    <link href="/Design/css/custom.css" rel="stylesheet" />

    <link rel="shortcut icon" href="/Design/img/favicon.html">
</head>
<body class="login-page">

    <!-- BEGIN Main Content -->
    <div class="login-wrapper">

        <!-- BEGIN Forgot Password Form -->
        <form id="formforgot" runat="server"  method="get" >
            <h3>Get back your password</h3>
            <hr />
            <div class="form-group">
                <div class="controls">
                    <%--<input type="text" placeholder="Email" class="required form-control" />--%>
                    <asp:TextBox ID="txtMobileNumber" class="form-control required" runat="server" MaxLength="11"  oninvalid="setCustomValidity('Please enter valid mobile number. e.g. 03XXXXXXXXX')"
                                    onchange="try{setCustomValidity('')}catch(e){}" required="true" placeholder="Mobile Number" pattern="[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="controls">
                    <%--<button type="submit" class="btn btn-primary form-control" onclick="btnSubmit_Click">Recover</button>--%>
                   <asp:Button ID="btnrecover" runat="server" Text="Recover" CssClass="btn btn-primary form-control" Height="40px" OnClick="btnrecover_Click"/>
                    <asp:Label ID="validationMessageId" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <hr />
            <p class="clearfix">
                <a href="Login.aspx" class="goto-login pull-left">← Back to login form</a>
            </p>
        </form>
        <!-- END Forgot Password Form -->
    </div>
    <!-- END Main Content -->


    <!--basic scripts-->
    <script src="/Design/assets/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Design/assets/bootstrap/js/bootstrap.min.js"></script>


    
    
</body>
</html>
