using LoginPassword.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Data
{
    public class SqlMessageRepo : IMessageRepository
    {
        private readonly AppDbContex _contex;

        public SqlMessageRepo(AppDbContex appDbContex)
        {
            _contex = appDbContex;
        }
        public SqlMessageRepo()
        {

        }

        public void Create(MessageUser messageUser)
        {
            _contex.CreateMessage(messageUser);
        }

        public void Delete(int id)
        {
            _contex.RemoveMessage(id);
        }

        public List<MessageUser> GetAll()
        {
            List<MessageUser> messages = _contex.ShovMessage();

            return messages;
        }

        public MessageUser GetById(int id)
        {
            MessageUser messageUser = _contex.GetByIdMessage(id);

            return messageUser;
        }

        public void Update(MessageUser messageUser)
        {
            _contex.UpdateMessage(messageUser);
        }
    }
}
