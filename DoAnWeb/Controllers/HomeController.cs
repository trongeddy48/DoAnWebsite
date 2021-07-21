using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class HomeController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu dbQLBanGame
        dbQLBanGameDataContext data = new dbQLBanGameDataContext();

        private List<tblSanPham> Laygamemoi(int count)
        {
            //Sap xep giam dan theo ngay cap nhat
            return data.tblSanPhams.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index()
        {
            //Lay 5 san pham moi nhat
            var gamemoi = Laygamemoi(5);
            return View(gamemoi);
        }

        public ActionResult Tutorial()
        {
            return View();
        }
    }
}