using LoginPassword.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Data
{
    public interface ILoginRepository
    {
        List<LoginPasswordd> GetAll();

        LoginPasswordd GetById(int id);

        void Create(LoginPasswordd loginPasswordd);

        void Update(LoginPasswordd loginPasswordd);

        void Delete(int id);
    }
}
