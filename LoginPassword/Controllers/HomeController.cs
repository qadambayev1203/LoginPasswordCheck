using LoginPassword.Data;
using LoginPassword.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoginPassword.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginRepository _repository;
        private readonly IMessageRepository _repositoryMessage;
        public static LoginPasswordd login = null;

        public HomeController(ILogger<HomeController> logger, ILoginRepository loginRepository, IMessageRepository messageRepository)
        {
            _logger = logger;
            _repository = loginRepository;
            _repositoryMessage = messageRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            int[] userTypes = new int[] { 1, 2 };
            string[] actionToSkip = new string[] { "Login", "Logout" };
            var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
            if (actionDesc != null && actionToSkip.Any(q => string.Compare(q, actionDesc.ActionName, true) == 0))
            {
                return;
            }
            else
            {
                int k = 0;
                for (int i = 0; i < userTypes.Length; i++)
                {
                    if (HttpContext.Session.GetInt32("user_type").HasValue &&
                   HttpContext.Session.GetInt32("user_type").Value == userTypes[i])
                    {
                        k++;
                        return;                        
                    }
                }
               
                if(k==0)
                {
                    context.Result = RedirectToAction("Login", "Home");
                }

            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginPasswordd logins)
        {
            List<LoginPasswordd> loginPasswordds = _repository.GetAll();

            foreach (var item in loginPasswordds)
            {
                if (logins != null && logins.Password == item.Password && logins.Login == item.Login)
                {
                    login = item;

                    break;
                }
            }
            if (login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin))
            {
                HttpContext.Session.SetInt32("user_type", 1);
                return (RedirectToAction(nameof(Index)));
            }
            else if (login != null && login.Role == LoginPasswordEnum.User)
            {
                HttpContext.Session.SetInt32("user_type", 2);
                return RedirectToAction(nameof(Userr));
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public IActionResult AdminUser()
        {
            if (login != null && login.Role == LoginPasswordEnum.adminUser)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Userr()
        {
            LoginPasswordd loginPasswordd = _repository.GetById(login.Id);
            HttpContext.Session.SetString("Login", login.Login);
            string Login = User.Identity.Name;
            ViewBag.Login = HttpContext.Session.GetString("Login");
            Thread.Sleep(300);
            return View(loginPasswordd);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            login = null;
            return RedirectToAction(nameof(Login));
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        ////////////////////////////Admin\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public IActionResult Index()
        {
            if (login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin))
            {
                List<LoginPasswordd> loginPasswordds = _repository.GetAll();

                loginPasswordds.Add(login);

                return View(loginPasswordds);

            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            if (login != null)
            {
                LoginPasswordd loginPasswordd = _repository.GetById(id);
                if ((login.Role == LoginPasswordEnum.adminUser && (id == login.Id || loginPasswordd.Role == LoginPasswordEnum.User)) || 
                    login.Role == LoginPasswordEnum.superAdmin)
                {
                    return View(loginPasswordd);
                }
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            if (login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin))
            {
                LoginPasswordd loginPasswordd = new()
                {
                    Role = login.Role
                };

                return View(loginPasswordd);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public IActionResult Create(LoginPasswordd loginPasswordd)
        {
            if (login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin))
            {
                if (loginPasswordd.Login != null & loginPasswordd.Password != null)
                {
                    _repository.Create(loginPasswordd);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (login != null)
            {
                LoginPasswordd loginPasswordd = _repository.GetById(id);
                if (((login.Role == LoginPasswordEnum.adminUser &&
                    (id == login.Id || loginPasswordd.Role == LoginPasswordEnum.User))
                    || login.Role == LoginPasswordEnum.superAdmin)
                    || (login.Role == LoginPasswordEnum.User && id == login.Id))
                {
                    return View(loginPasswordd);
                }
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public IActionResult Update(LoginPasswordd loginPasswordd)
        {
            if ((login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin))
                || (login.Id == loginPasswordd.Id && login.Role == LoginPasswordEnum.User))
            {
                if (loginPasswordd.Login != null & loginPasswordd.Password != null)
                {
                    _repository.Update(loginPasswordd);
                }
                if (login.Role == LoginPasswordEnum.User)
                {
                    return RedirectToAction(nameof(Userr));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (login != null)
            {
                LoginPasswordd loginPasswordd = _repository.GetById(id);
                if (((login.Role == LoginPasswordEnum.adminUser && (id == login.Id || loginPasswordd.Role == LoginPasswordEnum.User)) || login.Role == LoginPasswordEnum.superAdmin)
                    || (login.Role == LoginPasswordEnum.User && login.Id == id))
                {
                    return View(loginPasswordd);
                }
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public IActionResult Delete(LoginPasswordd loginPasswordd)
        {
            if ((login != null && (login.Role == LoginPasswordEnum.adminUser || login.Role == LoginPasswordEnum.superAdmin) && loginPasswordd.Id != 0)
                || (login.Role == LoginPasswordEnum.User && login.Id == loginPasswordd.Id))
            {
                _repository.Delete(loginPasswordd.Id);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

        }

        public IActionResult Message()
        {
            if (login.Role == LoginPasswordEnum.User)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public IActionResult Message(MessageUser messageUser)
        {
            if (messageUser != null && login.Role == LoginPasswordEnum.User)
            {
                messageUser.Status = MessageUserEnum.Admin;

                _repositoryMessage.Create(messageUser);

                return RedirectToAction(nameof(Userr));
            }
            return RedirectToAction(nameof(Userr));
        }


        [HttpGet]
        public IActionResult MessageRead()
        {
            if (login.Role == LoginPasswordEnum.adminUser)
            {
                List<MessageUser> messageUsers = _repositoryMessage.GetAll();
                return View(messageUsers);

            }
            else if (login.Role == LoginPasswordEnum.superAdmin)
            {
                return RedirectToAction(nameof(ReadMessageSA));
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Confirmation(int id)
        {
            if (login.Role == LoginPasswordEnum.adminUser)
            {
                MessageUser messageUser = _repositoryMessage.GetById(id);

                return View(messageUser);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Confirmation(MessageUser messageUser)
        {
            if (login.Role == LoginPasswordEnum.adminUser)
            {
                MessageUser messageUserr = _repositoryMessage.GetById(messageUser.Id);

                messageUserr.Status = MessageUserEnum.superAdmin;

                _repositoryMessage.Update(messageUserr);

                return RedirectToAction(nameof(MessageRead));
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ReadMessageSA()
        {
            if (login.Role == LoginPasswordEnum.superAdmin)
            {
                List<MessageUser> messageUsers = _repositoryMessage.GetAll();

                return View(messageUsers);
            }
            return RedirectToAction(nameof(Login));
        }
        public IActionResult MessageSADelete(int id)
        {
            if (id != 0)
            {
                _repositoryMessage.Delete(id);
            }
            if (login.Role == LoginPasswordEnum.adminUser)
            {
                return RedirectToAction(nameof(MessageRead));
            }
            else if (login.Role == LoginPasswordEnum.superAdmin)
            {
                return RedirectToAction(nameof(ReadMessageSA));
            }
            return RedirectToAction(nameof(Login));
        }
    }
}
