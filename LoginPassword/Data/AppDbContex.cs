using LoginPassword.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Data
{
    public class AppDbContex
    {
        private readonly NpgsqlConnection conn = new("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=1203");

        public List<LoginPasswordd> Shov()
        {
            List<LoginPasswordd> logins = new();
            DataTable table = null;

            string query = "SELECT * FROM logins;";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    LoginPasswordd login = new(row);
                    logins.Add(login);
                }
            }
            return logins;

        }

        public LoginPasswordd GetById(int id)
        {
            DataTable table = null;
            LoginPasswordd login = null;
            string query = "SELECT * FROM logins where id=@id;";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    login = new(row);                    
                }
            }

            return login;
        }

        public void Create(LoginPasswordd loginPasswordd)
        {
            if (loginPasswordd != null)
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO logins VALUES (DEFAULT,@login,@password,@role);";

                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", loginPasswordd.Login);
                        cmd.Parameters.AddWithValue("@password", loginPasswordd.Password);
                        cmd.Parameters.AddWithValue("@role", (int)loginPasswordd.Role);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void Update(LoginPasswordd loginPasswordd)
        {
            if (loginPasswordd != null)
            {
                try
                {
                    conn.Open();
                    string query = $"UPDATE logins SET login=@login,password=@password,role=@role WHERE id=@id;";

                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", loginPasswordd.Id);
                        cmd.Parameters.AddWithValue("@login", loginPasswordd.Login);
                        cmd.Parameters.AddWithValue("@password", loginPasswordd.Password);
                        cmd.Parameters.AddWithValue("@role", loginPasswordd.Role);
                        var a = cmd.ExecuteNonQuery();
                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void Remove(int id)
        {

            try
            {
                conn.Open();
                string query = $"DELETE FROM logins WHERE id=@id;";

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }

        }

        //////////Message
        ///
        public List<MessageUser> ShovMessage()
        {
            List<MessageUser> messages = new();
            DataTable table = null;

            string query = "SELECT * FROM messages";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    MessageUser message = new(row);
                    messages.Add(message);
                }
            }
            return messages;

        }

        public MessageUser GetByIdMessage(int id)
        {
            MessageUser message = null;
            DataTable table = null;

            string query = "SELECT * FROM messages where id=@id";

            try
            {
                conn.Open();

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id",id);
                    using (NpgsqlDataAdapter da = new(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
            }
            catch { }
            finally { conn.Close(); }

            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    message = new(row);
                }
            }
            return message;
        }

        public void CreateMessage(MessageUser messageUser)
        {
            if (messageUser != null)
            {
                try
                {
                    conn.Open();
                    string query = $"INSERT INTO messages VALUES (DEFAULT,@fullname,@message,@status);";
                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullname", messageUser.FullName);
                        cmd.Parameters.AddWithValue("@message", messageUser.Message);
                        cmd.Parameters.AddWithValue("@status", (int)messageUser.Status);
                        var a = cmd.ExecuteNonQuery();

                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void UpdateMessage(MessageUser message)
        {
            if (message != null)
            {   
                try
                {
                    conn.Open();
                    string query = $"UPDATE messages SET fullname=@fullname,message=@message,status=@status WHERE id=@id;";

                    using (NpgsqlCommand cmd = new(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", message.Id);
                        cmd.Parameters.AddWithValue("@fullname", message.FullName);
                        cmd.Parameters.AddWithValue("@message", message.Message);
                        cmd.Parameters.AddWithValue("@status", (int)message.Status);
                        var a = cmd.ExecuteNonQuery();
                    }
                }
                catch { }
                finally { conn.Close(); }
            }
        }

        public void RemoveMessage(int id)
        {

            try
            {
                conn.Open();
                string query = $"DELETE FROM messages WHERE id=@id;";

                using (NpgsqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var a = cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally { conn.Close(); }

        }

    }
}
