using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Web.Configuration;

namespace TicketApplication.Helpers.Classes
{
    [Serializable]
    public class Database : IDeserializationCallback, IDisposable
    {
        [NonSerialized]
        public SqlConnection connection = null;

        private static string connectionString = WebConfigurationManager.ConnectionStrings["TicketAppConnectionString"].ConnectionString;

        ~Database()
        {
            Dispose(false);
        }

        public static DataTable Data(string sql, Dictionary<string, object> variables = null)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                if (variables != null)
                {
                    foreach (KeyValuePair<string, object> item in variables)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                DataTable dt = OldDataQuery(command);

                if (dt == null) return null;

                return dt;
            }
        }

        public static bool Non(string sql, Dictionary<string, object> variables = null)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                if (variables != null)
                {
                    foreach (KeyValuePair<string, object> item in variables)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                return OldNonQuery(command);
            }
        }

        public static DataTable OldDataQuery(SqlCommand command)
        {
            command.CommandTimeout = 0;
            DataTable dt = null;
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);

                try
                {
                    conn.Open();
                    command.Connection = conn;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    dt = new DataTable();
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    InsertErrorLog("Failed a WPMDataQuery", ex);
                    dt = null;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return dt;
        }

        public static bool OldNonQuery(SqlCommand command)
        {
            command.CommandTimeout = 0;
            bool returnBool = true;
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    InsertErrorLog("Failed WPMNonQuery", ex);
                    returnBool = false;
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return returnBool;
        }

        public static object OldScalarQuery(SqlCommand command)
        {
            command.CommandTimeout = 0;
            object returnObject = null;
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    returnObject = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    InsertErrorLog("Failed WPMScalarQuery", ex);
                    returnObject = null;
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }

            return returnObject;
        }

        public static object Scalar(string sql, Dictionary<string, object> variables = null)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                if (variables != null)
                {
                    foreach (KeyValuePair<string, object> item in variables)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                return OldScalarQuery(command);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            this.OnDeserialization();
        }

        public void OnDeserialization()
        {
            this.connection = new SqlConnection(connectionString);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }

        private static void InsertErrorLog(string message, Exception ex)
        {
            SqlCommand insertError = new SqlCommand("dbo.usp_InsertIntoErrorLog");
            insertError.CommandType = CommandType.StoredProcedure;
            //insertError.Parameters.Add("username", SqlDbType.VarChar).Value = Helpers.General.GetLoggedInUsername();
            insertError.Parameters.Add("username", SqlDbType.VarChar).Value = DBNull.Value;
            insertError.Parameters.Add("message", SqlDbType.VarChar).Value = message;
            insertError.Parameters.Add("stack_trace", SqlDbType.VarChar).Value = ex.StackTrace;
            bool entered = OldNonQuery(insertError);
        }
    }
}