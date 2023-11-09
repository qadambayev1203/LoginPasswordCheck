using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Models
{
    public class MessageUser
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Message { get; set; }

        public MessageUserEnum Status { get; set; }

        public MessageUser(DataRow row)
        {
            Id = int.Parse("" + row["id"]);
            FullName = row["fullname"].ToString();
            Message = row["message"].ToString();
            Status = (MessageUserEnum)row["status"];
        }

        public MessageUser()
        {
                
        }
    }
}
