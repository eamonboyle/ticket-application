﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TicketApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Home", "home", "~/Default.aspx");
            routes.MapPageRoute("Login", "login", "~/Public/Login.aspx");
            routes.MapPageRoute("LoginMessage", "login/{message}", "~/Public/Login.aspx");
        }
    }
}