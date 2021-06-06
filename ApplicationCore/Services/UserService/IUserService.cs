using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.UserService
{
    public interface IUserService
    {

        ApplicationUser GetUserById(string id);
    }
}
