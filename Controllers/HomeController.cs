using hospitalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hospitalMS.Controllers
{
    public class HomeController : Controller
    {
        HospitalDBEntities db = new HospitalDBEntities();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
        

            return View();
        }

        public ActionResult Service()
        {


            return View();
        }
    }
}