using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;
using Newtonsoft.Json;

namespace MyModel_CodeFirst.Controllers
{
    public class LoginController : Controller
    {
        private readonly GuestBookContext _context;

        public LoginController(GuestBookContext context)
        {
            _context = context;
        }

        //4.2.4 建立Get與Post的Login Action
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {

            //如果帳密正確,導入後台頁面

            //如果帳密不正確,回到登入頁面,並告知帳密錯誤

            //如果帳密正確
            //  導入後台頁面
            //否則
            // 回到登入頁面,並告知帳密錯誤

            if (login == null)
            {
                return View();
            }


            //select * from Login where account=@account and password=@password

            var result = await _context.Login.Where(m => m.Account == login.Account && m.Password == login.Password).FirstOrDefaultAsync();


            if (result == null)
            {
                //4.2.6 將ViewData["Error"]加入Login View
                ViewData["Error"] = "帳號或密碼錯誤!!";
                return View();
            }
            else
            {
                //Session["Manager"] = "已登入";

                HttpContext.Session.SetString("Manager", JsonConvert.SerializeObject(login));

                return RedirectToAction("Index", "Books");
            }



        }

        public IActionResult Logout()
        {


            HttpContext.Session.Remove("Manager");
            return RedirectToAction("Index", "Home");

            //return View();
        }
    }
}
