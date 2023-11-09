using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Models
{
    public class LoginPasswordd
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public LoginPasswordEnum Role { get; set; }

        public LoginPasswordd(DataRow row)
        {
            Id = int.Parse("" + row["id"]);
            Login = row["login"].ToString();
            Password = row["password"].ToString();
            Role = (LoginPasswordEnum)row["role"];
        }

        public LoginPasswordd()
        {

        }
    }
}
