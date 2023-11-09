using LoginPassword.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Data
{
    public interface IMessageRepository
    {
        List<MessageUser> GetAll();

        MessageUser GetById(int id);

        void Create(MessageUser messageUser);

        void Update(MessageUser messageUser);

        void Delete(int id);
    }
}
