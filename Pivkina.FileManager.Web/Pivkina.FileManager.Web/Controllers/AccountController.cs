using Pivkina.FileManager.Web.Models;
using Pivkina.FileManager.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pivkina.FileManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private NHUserRepository UserRepository { get; set; }

        public AccountController()
        {
            UserRepository = new NHUserRepository();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid)
            // Если модель не прошла проверку
            // Выдаем ошибку и открываем страницу заново
            {
                ModelState.AddModelError("", "Проверьте введённые данные");
                return View(model);
            }
            //
            // Если все хорошо 
            // Отправляем запрос в бд
            // проверяем что в бд есть запись с таким логином и паролем
            var user = UserRepository
                .Find(new string[,] { { "Login", $"{model.Login}" }, { "Password", $"{model.Password}" } })
                .FirstOrDefault();
            // если записи нет
            if (user == null)
            {
                // Выдаем ошибку и открываем страницу заново
                ModelState.AddModelError("", "Ошибка авторизации");
                return View(model);
            }

            // Если все хорошо
            // Сохраняем найденного пользователя как текущего
            FormsAuthentication.SetAuthCookie(model.Login, false);
            // переходим на страницу работы с файлами
            return RedirectToAction("DocumentList", "Document");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            // Если модель не прошла проверку
            // Выдаем ошибку и открываем страницу заново
            {
                ModelState.AddModelError("", "Проверьте введённые данные");
                return View(model);
            }

            if (UserRepository.LoadByLogin(model.Login) != null)
            {
                // Выдаем ошибку и открываем страницу заново
                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                return View(model);
            }

            // Если все хорошо
            // Сохраняем найденного пользователя в БД и записываем как текущего
            var user = new User { Name = model.FullName, Login = model.Login, Password = model.Password };
            UserRepository.Create(user);

            FormsAuthentication.SetAuthCookie(model.Login, false);
            
            // переходим на страницу работы с файлами
            return RedirectToAction("DocumentList", "Document");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}