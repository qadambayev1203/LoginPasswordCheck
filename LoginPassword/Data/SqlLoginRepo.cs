
using LoginPassword.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginPassword.Data
{
    public class SqlLoginRepo : ILoginRepository
    {
        private readonly AppDbContex _contex; 

        public SqlLoginRepo(AppDbContex appDbContex)
        {
            _contex = appDbContex;
        }

        public SqlLoginRepo()
        {

        }

        public List<LoginPasswordd> GetAll()
        {
            return _contex.Shov();
        }

        public LoginPasswordd GetById(int id)
        {
           LoginPasswordd loginPasswordd= _contex.GetById(id);

            return loginPasswordd;
        }

        public void Create(LoginPasswordd loginPasswordd)
        {
            _contex.Create(loginPasswordd);
        }

        public void Update(LoginPasswordd loginPasswordd)
        {
            _contex.Update(loginPasswordd);
        }

        public void Delete(int id)
        {
            _contex.Remove(id);
        }
    }
}
