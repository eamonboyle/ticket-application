using Scrypt;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TicketApplication.Helpers.Classes
{
    [Serializable]
    public class User
    {
        public User()
        {
            this.Id = 0;
        }

        public User(int id)
        {
            this.PopulateUser(this.GetUser(id));
        }

        public User(string username)
        {
            this.Username = username;
            this.PopulateUser(this.GetUser(username), true);
        }

        public User(string username, string email)
        {
            this.Username = username;
            this.PopulateUser(this.GetUser(username, email), true);
        }

        public User(string email, string forgottenLink, bool forgotten)
        {
            this.PopulateUser(this.GetUser(email, forgottenLink, forgotten));
        }

        public int CompanyId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Email { get; set; }

        public bool Enabled { get; set; }

        public string EnteredPassword { get; set; }

        public bool Exists { get; set; }

        public string FirstName { get; set; }

        public string ForgottenLink { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public bool NewPassword { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public int RoleId { get; set; }

        public int RoleLevelId { get; set; }

        public string Salt { get; set; }

        public string Username { get; set; }

        public bool Authenticate()
        {
            string ipAddress = this.GetIPAddress();

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // check the users exists
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            string getCount = @"SELECT COUNT(id)
            FROM [dbo].[Users]
            WHERE [username] = @username";

            int count = Convert.ToInt32(Database.Scalar(getCount, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", this.Username}
                }));

            if (count < 1)
            {
                this.EnterLogin(ipAddress, true);
                return false;
            }

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // get the salt and hashed password
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            string getSalt = @"SELECT [salt]
            FROM [dbo].[Users]
            WHERE [username] = @username";

            string salt = (string)Database.Scalar(getSalt, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", this.Username}
                });

            string getHashed = @"SELECT [password]
                FROM [dbo].[Users]
                WHERE [username] = @username";

            string hashed = (string)Database.Scalar(getHashed, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", this.Username}
                });

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // compare the entered password with the one stored
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            ScryptEncoder encoder = new ScryptEncoder();
            bool passwordsMatch = encoder.Compare(salt + this.EnteredPassword, hashed);
            if (passwordsMatch)
            {
                // --------------------------------------------------------------------------------------------------------------------------------------------------
                // enter to log
                // --------------------------------------------------------------------------------------------------------------------------------------------------
                this.EnterLogin(ipAddress);
                //Helpers.Variables.Admin = Convert.ToBoolean(Helpers.OutputFromDB.Int(Database.WPMScalarQuery(new SqlCommand("SELECT admin FROM [dbo].[Users] WHERE [username] = '" + this.Username + "'"))));

                return true;
            }

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // failed login, insert fail to database
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            this.EnterLogin(ipAddress, true);

            return false;
        }

        public bool Create()
        {
            this.Salt = this.CreateSalt();
            ScryptEncoder encoder = new ScryptEncoder();
            this.Password = encoder.Encode(this.Salt + this.Password);

            string sql = @"INSERT INTO [dbo].[Users] ([username],
              [password],
              [salt],
              [email])
            VALUES (
              @username
             ,@password
             ,@salt
             ,@email
            )";

            return Database.Non(sql, new System.Collections.Generic.Dictionary<string, object>
            {
                {"@username", this.Username},
                {"@password", this.Password},
                {"@salt", this.Salt},
                {"@email", this.Email},
            });
        }

        public string CreateSalt()
        {
            byte[] bytSalt = new byte[16];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytSalt);

            return Convert.ToBase64String(bytSalt);
        }

        public bool Delete()
        {
            string sql = @"DELETE FROM [dbo].[Users] WHERE id = @id";

            return Database.Non(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@id", this.Id}
                });
        }

        public bool ForgottenPasswordValid()
        {
            SqlCommand command = new SqlCommand("dbo.usp_isResetPasswordLinkValid");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("forgotten_link", SqlDbType.VarChar).Value = this.ForgottenLink;
            command.Parameters.Add("email", SqlDbType.VarChar).Value = this.Email;

            int linkValidCount = Convert.ToInt32(Database.OldScalarQuery(command));

            return linkValidCount > 0 ? true : false;
        }

        public async Task<bool> SendForgottenLink()
        {
            var forgottenLink = Helpers.General.RandomStringLower(50);
            SqlCommand command = new SqlCommand("dbo.usp_SendForgottenPasswordLink");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("username", SqlDbType.VarChar).Value = this.Username;
            command.Parameters.Add("email", SqlDbType.VarChar).Value = this.Email;
            command.Parameters.Add("forgotten_link", SqlDbType.VarChar).Value = forgottenLink;

            int isPasswordLinkUpdated = Convert.ToInt32(Database.OldScalarQuery(command));

            if (isPasswordLinkUpdated == 1)
            {
                var passwordLink = Helpers.General.GetStartOfUrl() + HttpContext.Current.Request.Url.Authority + "/reset-password/" + this.Email + "/" + forgottenLink;

                StringBuilder emailContent = new StringBuilder();
                emailContent.AppendLine("<p>Hi " + this.Username + ",<p>");
                emailContent.AppendLine("<p>Please follow this <a href='" + passwordLink + "'>link</a> to reset your password.</p>");
                emailContent.AppendLine("<p>This link is only valid for 2 hours.</p>");
                emailContent.AppendLine("<br>");
                emailContent.AppendLine("<p>If the link doesn't work, please copy and paste this into your url bar:</p>");
                emailContent.AppendLine("<p>" + passwordLink + "</p>");
                emailContent.AppendLine("<p>Thanks,</p>");
                emailContent.AppendLine("<p>NewCo</p>");

                await Helpers.Emailer.SendEmail(this.Email, this.Username, "Reset your password", emailContent.ToString());

                return true;
            }

            return false;
        }

        public bool Update()
        {
            if (this.NewPassword)
            {
                this.Salt = this.CreateSalt();
                ScryptEncoder encoder = new ScryptEncoder();
                this.Password = encoder.Encode(this.Salt + this.Password);
            }

            string sql = @"UPDATE [dbo].[Users]
            SET [username] = @username
               ,[password] = @password
               ,[salt] = @salt
               ,[email] = @email
            WHERE [id] = @id";

            return Database.Non(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", this.Username},
                    {"@password", this.Password},
                    {"@salt", this.Salt},
                    {"@email", this.Email},
                    {"@id", this.Id}
                });
        }

        public bool UpdatePassword(string password)
        {
            // hash password
            var salt = this.CreateSalt();
            ScryptEncoder encoder = new ScryptEncoder();
            var hashed = encoder.Encode(salt + password);

            SqlCommand command = new SqlCommand("dbo.usp_UpdateUserPassword");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("hashed_password", SqlDbType.VarChar).Value = hashed;
            command.Parameters.Add("salt", SqlDbType.VarChar).Value = salt;
            command.Parameters.Add("forgotten_link", SqlDbType.VarChar).Value = this.ForgottenLink;
            command.Parameters.Add("email", SqlDbType.VarChar).Value = this.Email;

            bool isPasswordUpdated = Database.OldNonQuery(command);

            if (isPasswordUpdated)
            {
                // get users name
                string usernameSql = @"SELECT [username]
                FROM [dbo].[Users]
                WHERE [email] = @email
                AND [forgotten_link] = @forgottenLink";

                string username = (string)Database.Scalar(usernameSql, new System.Collections.Generic.Dictionary<string, object>
                    {
                        {"@email", this.Email},
                        {"@forgottenLink", this.ForgottenLink},
                    });

                // send confirmation email
                StringBuilder emailContent = new StringBuilder();
                emailContent.AppendLine("<p>Hi " + username + ",<p>");
                emailContent.AppendLine("<p>Your password has been reset.</p>");
                emailContent.AppendLine("<p>If you did not request this change, please contact your system administrator.</p>");
                emailContent.AppendLine("<p>Thanks,</p>");
                emailContent.AppendLine("<p>NewCo</p>");

                Helpers.Emailer.SendEmail(this.Email, username, "Your password has been reset", emailContent.ToString());
            }

            return isPasswordUpdated;
        }

        public bool UsernameTaken(string username)
        {
            string sql = @"SELECT COUNT(id)
            FROM [dbo].[Users]
            WHERE username = @username";

            int userCount = Convert.ToInt32(Database.Scalar(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", username}
                }));

            return userCount > 0 ? true : false;
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');

                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private void EnterLogin(string ipAddress, bool failed = false)
        {
            SqlCommand command = new SqlCommand("dbo.usp_InsertLogin");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("username", SqlDbType.VarChar).Value = this.Username;
            command.Parameters.Add("ip_address", SqlDbType.VarChar).Value = ipAddress;
            command.Parameters.Add("failed", SqlDbType.Bit).Value = failed ? 1 : 0;
            Database.OldNonQuery(command);
        }

        private DataRow GetUser(int id)
        {
            DataTable users = Database.OldDataQuery(new SqlCommand("SELECT * FROM [dbo].[Users] WHERE id = " + id));

            if (users.Rows.Count < 1)
            {
                return null;
            }

            return users.Rows[0];
        }

        private DataRow GetUser(string username)
        {
            string sql = @"SELECT *
            FROM [dbo].[Users]
            WHERE [username] = @username";

            return Helpers.General.ReturnFirst(Database.Data(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", username}
                }));
        }

        private DataRow GetUser(string username, string email)
        {
            string sql = @"SELECT *
            FROM [dbo].[Users]
            WHERE [username] = @username
            AND [email] = @email";

            return Helpers.General.ReturnFirst(Database.Data(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@username", username},
                    {"@email", email}
                }));
        }

        private DataRow GetUser(string email, string forgottenLink, bool forgotten)
        {
            string sql = @"SELECT *
            FROM [dbo].[Users]
            WHERE email = @email
            AND forgotten_link = @forgottenLink";

            return Helpers.General.ReturnFirst(Database.Data(sql, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"@email", email},
                    {"@forgottenLink", forgottenLink}
                }));
        }

        private void PopulateUser(DataRow user, bool gotUsername = false)
        {
            if (user != null)
            {
                this.Exists = true;
                this.Id = Helpers.OutputFromDB.Int(user["id"]);

                if (!gotUsername)
                {
                    this.Username = Helpers.OutputFromDB.StringEmpty(user["username"]);
                }

                this.Password = Helpers.OutputFromDB.StringEmpty(user["password"]);
                this.Salt = Helpers.OutputFromDB.StringEmpty(user["salt"]);
                this.Email = Helpers.OutputFromDB.StringEmpty(user["email"]);
                this.ForgottenLink = Helpers.OutputFromDB.StringEmpty(user["forgotten_link"]);
                this.ForgottenValidUntil = Helpers.OutputFromDB.Date(user["forgotten_valid_until"]);
                this.Admin = Helpers.OutputFromDB.Int(user["admin"]) == 1 ? true : false;
            }
        }
    }
}