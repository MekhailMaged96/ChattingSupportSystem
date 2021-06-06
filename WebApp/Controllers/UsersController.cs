using ApplicationCore.Services.UserService;
using Infrastructure.UnitOfWork;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private UserService userService = new UserService();
        // GET: Users
        private UnitOfWork unitOfWork = new UnitOfWork();
        public UsersController()
        {
           
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var users = unitOfWork.UserRepo.Get(e => e.Id != userId).ToList();

           return View(users);
        }
    }
}