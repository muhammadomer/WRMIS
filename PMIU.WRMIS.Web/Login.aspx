<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PMIU.WRMIS.Web.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>WRMIS - Login</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link rel="shortcut icon" type="image/x-icon" href="/Design/img/favicon.ico" />

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
        <!-- BEGIN Login Form -->
        <form id="frmLogin" runat="server" method="get">
            <div style="text-align: center;">
                <br />
                <img src="/Design/img/wrmis_logo.png" />
                <br />
                <br />
                <h4>WRMIS Login</h4>
                <br />
            </div>
            <div class="form-group">
                <div class="controls">
                    <div class="input-icon left">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="txtUserName" runat="server" placeholder="Username" required="true" CssClass="form-control required"></asp:TextBox>
                        <%--<input type="text" id="txtUserName" placeholder="Username" class="form-control" />--%>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="controls">
                    <div class="input-icon left">
                        <i class="fa fa-key"></i>
                        <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" required="true" CssClass="form-control required" TextMode="Password"></asp:TextBox>
                        <%--<input type="password" id="txtPassword" placeholder="Password" class="form-control" />--%>
                    </div>
                </div>
            </div>
            <div class="form-group" style="display: none;">
                <div class="controls">
                    <label class="checkbox">
                        <input type="checkbox" value="remember" />
                        Remember me
                    </label>
                </div>
            </div>
            <div class="form-group">
                <div class="controls">
                    <asp:Button ID="btnSubmit" runat="server" Text="Sign In" CssClass="btn btn-primary form-control" Height="40px" OnClick="btnSubmit_Click" />
                    <%--<button type="submit" id="btnSubmit" class="btn btn-primary form-control" onclick="btnSubmit_Click">Sign In</button>--%>
                    <asp:Label ID="validationMessageId" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <hr />
            <p class="clearfix">
                <a href="ForgotPassword.aspx" class="goto-forgot pull-left">Forgot Password?</a>
            </p>
            <div style="text-align: center;">
                <br />
                <img src="/Design/img/gop.png" />
                <br />
                Punjab Irrigation Department
        <br />
                Copyright &copy; <%= DateTime.Now.Year %> <%--<br /> <%= Environment.MachineName %>--%>
            </div>
        </form>
        <!-- END Login Form -->

        <!-- BEGIN Forgot Password Form -->
        <%--        <form id="form-forgot"  method="get" style="display: none">
            <h3>Get back your password</h3>
            <hr />
            <div class="form-group">
                <div class="controls">
                    <input type="text" placeholder="Email" class="required form-control" />
                </div>
            </div>
            <div class="form-group">
                <div class="controls">
                    <button type="submit" class="btn btn-primary form-control" onclick="btnSubmit_Click">Recover</button>
                </div>
            </div>
            <hr />
            <p class="clearfix">
                <a href="Login.aspx" class="goto-login pull-left">← Back to login form</a>
            </p>
        </form>--%>
        <!-- END Forgot Password Form -->
    </div>
    <!-- END Main Content -->


    <!--basic scripts-->
    <script src="/Design/assets/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Design/assets/bootstrap/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function goToForm(form) {
            $('.login-wrapper > form:visible').fadeOut(500, function () {
                $('#form-' + form).fadeIn(500);
            });
        }
        //$(function () {
        //    $('.goto-login').click(function () {
        //        goToForm('login');
        //    });
        //    $('.goto-forgot').click(function () {
        //        goToForm('forgot');
        //    });
        //});
    </script>



</body>
</html>
