<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TicketApplication.Public.Login" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Login | Ticket System</title>
    <link rel="stylesheet" href="/Content/bootstrap.min.css" type="text/css" />
    <link rel="stylesheet" href="/Content/Site.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableHistory="true" EnablePartialRendering="true" EnablePageMethods="true">
            <Scripts>
                <%--Site Scripts--%>
                <asp:ScriptReference Path="~/Scripts/jquery-1.10.2.js" />
                <asp:ScriptReference Path="~/Scripts/bootstrap.min.js" />
                <asp:ScriptReference Path="~/Scripts/default.js" />
            </Scripts>
        </asp:ScriptManager>

        <%-- NavBar --%>
        <div class="navbar navbar-expand-md navbar-dark bg-dark fixed-top">
            <div class="container">

                <a class="navbar-brand" href="/">Ticket System</a>

                <ul class="navbar-nav my-2 my-lg-0">
                    <dx:ASPxPanel ID="pnlLoggedIn" runat="server" Width="100%">
                        <PanelCollection>
                            <dx:PanelContent>
                                <li class="nav-item">
                                    <a class="nav-link" href="/login">Login</a>
                                </li>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                </ul>
            </div>
        </div>

        <div role="main" class="main-content container">
            <div class="row">
                <div class="col-sm-12 col-md-8 offset-2">
                    <h2 class="page-title auth-forms">Login</h2>
                    <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Text="Wrong credentials entered" Visible="false"></dx:ASPxLabel>
                    <div class="form-group">
                        <label>Username</label>
                        <input type="text" class="form-control" runat="server" id="txtUsername" autofocus="autofocus" />
                    </div>
                    <div class="form-group" style="padding-bottom: 10px;">
                        <label>Password</label>
                        <input type="password" class="form-control" runat="server" id="txtPassword" autocomplete="off" />
                    </div>
                    <div class="form-group">
                        <dx:ASPxButton ID="btnLogin" runat="server" Text="Login" CssClass="btn float-right" OnClick="btnLogin_Click"></dx:ASPxButton>
                    </div>
                    <%--<div class="form-group text-right">
                        <dx:ASPxHyperLink ID="hlForgottenPassword" runat="server" Text="Forgotten Password?" NavigateUrl="/reset-password" CssClass="forgotten-password-link float-right"></dx:ASPxHyperLink>
                    </div>--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>