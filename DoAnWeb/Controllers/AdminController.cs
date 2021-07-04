using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gan gia tri nguoi dung nhap cho cac bien
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập !";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu !";
            }
            else
            {
                //Gan gia tri cho doi tuong tao moi
                tblTaiKhoan ad = db.tblTaiKhoans.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công"
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}