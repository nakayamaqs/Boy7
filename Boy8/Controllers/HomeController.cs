using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boy8.BLL;

namespace Boy8.Controllers
{
    [RequireHttps]
    [AllowAnonymous]
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

        public ActionResult ShowPictures()
        {
            var thePictures = BabyStorage.GetPictures("babyimages");
            ViewBag.BabyPictures = thePictures;
            return View();
        }

        public ActionResult Events()
        {
            ViewBag.Message = "时光";
            ViewBag.EventToShow = "https://portalvhdstlvw630d4fzcc.blob.core.windows.net/asset-d1ef41e7-2df5-482b-92bd-8a41818adb5c/walkon_H264_3400kbps_AAC_und_ch2_96kbps.mp4?sv=2012-02-12&sr=c&si=ebee0ddf-d617-4a1a-ba69-35b364d26a5d&sig=mWv1wQd%2FF7Lvl4dkW3kppAU6gE%2FgtaMDVXBE1y%2BlmMA%3D&se=2014-07-31T08%3A15%3A32Z";
            return View();
        }

        public ActionResult Videos()
        {
            ViewBag.Message = "时光";
            ViewBag.EventToShow = "https://portalvhdstlvw630d4fzcc.blob.core.windows.net/asset-d1ef41e7-2df5-482b-92bd-8a41818adb5c/walkon_H264_3400kbps_AAC_und_ch2_96kbps.mp4?sv=2012-02-12&sr=c&si=ebee0ddf-d617-4a1a-ba69-35b364d26a5d&sig=mWv1wQd%2FF7Lvl4dkW3kppAU6gE%2FgtaMDVXBE1y%2BlmMA%3D&se=2014-07-31T08%3A15%3A32Z";
            return View();
        }

        public ActionResult V1(int? v)
        {
            if (Convert.ToInt16(v) <= 1) { return View(); }
            else
            {
                return View("Index");
            }
        }

        public ActionResult PicStream()
        {
            var thePictures = BabyStorage.GetPictures("babyimages").Take(12).ToList();
            ViewBag.BabyPictures = thePictures;
            return View();
        }

        public ActionResult Progress()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}