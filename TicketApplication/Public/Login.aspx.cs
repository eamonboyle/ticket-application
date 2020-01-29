using System;
using System.Web.Security;
using System.Web.UI;

namespace TicketApplication.Public
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblError.ClientVisible = false;

            Helpers.Classes.User user = new Helpers.Classes.User(txtUsername.Value);
            user.EnteredPassword = txtPassword.Value;

            if (user.Authenticate())
            {
                FormsAuthentication.RedirectFromLoginPage(user.Username, false);
            }
            else
            {
                lblError.Text = "Wrong credentials entered";
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
                lblError.ClientVisible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Page.RouteData.Values["message"] != null)
                {
                    string message = Page.RouteData.Values["message"].ToString();

                    if (message == "reset-password")
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "Password reset, please log in.";
                        lblError.Visible = true;
                    }
                    else if (message == "reset-password-expired")
                    {
                        lblError.ForeColor = System.Drawing.Color.Red;
                        lblError.Text = "Reset password link has expired, please try again.";
                        lblError.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = false;
                    }
                }
            }
        }
    }
}