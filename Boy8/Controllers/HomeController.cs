using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boy8.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "宝宝是怎样长大的";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "谁家宝宝惹人爱";

            return View();
        }
        
        public ActionResult Comming()
        {
            ViewBag.Message = "未完成";
            return View();
        }

    }
}