using PGMenuWebservices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCDemo.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {  
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username)
        {
            var pgmenuService = new PGMenuService();
            var sysCode = ConfigurationManager.AppSettings["SysCode"].ToString();
            var data = pgmenuService.PGMenuCheckUserAuthenticationServiceNoPassword(username);
            var groups = new System.Text.StringBuilder();

            if (data == null)
            {
                ViewBag.Message = "Login Failed! Username or Password Invalid";
                return View();
            }

            foreach (var item in data.groupUserNList)
            {
                var group = item.group_code.ToUpper();
                groups.Append("," + group);
            }

            Session["pgmenudata"] = data;
            string roles = "PUBLIC" + groups.ToString();

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1
                , username
                , DateTime.Now
                , DateTime.Now.AddMinutes(20)
                , false
                , roles
                , "/");

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);
            Response.Cookies.Add(new HttpCookie("username", username));

            var userInfo = pgmenuService.GetUserInformation(data.user_login);

            Response.Cookies.Add(new HttpCookie("userId", userInfo.userId));
            Response.Cookies.Add(new HttpCookie("login_fname", userInfo.nameeng));
            Response.Cookies.Add(new HttpCookie("login_lname", userInfo.surnameeng));
            Response.Cookies.Add(new HttpCookie("login_email", userInfo.Email));

            return RedirectToAction("Index");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}