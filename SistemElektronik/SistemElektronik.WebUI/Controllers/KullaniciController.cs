using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SistemElektronik.WebUI.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        public ActionResult Index()
        {

            MembershipUserCollection users=Membership.GetAllUsers();
            return View(users);
        }
    }
}