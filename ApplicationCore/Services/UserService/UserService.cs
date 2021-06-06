using Infrastructure.Data.Context;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.UserService
{
    public class UserService : IUserService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
      

        public ApplicationUser GetUserById(string id)
        {
            return unitOfWork.UserRepo.GetByID(id);
        }
        public ApplicationUser GetUserByName(string name)
        {
            return unitOfWork.UserRepo.Get(e=>e.UserName == name).FirstOrDefault();
        }
    }
}
