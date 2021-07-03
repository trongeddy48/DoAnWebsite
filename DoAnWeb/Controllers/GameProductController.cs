using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class GameProductController : Controller
    {
        // GET: GameProduct
        //Doi tuong chua CSDL
        dbQLBanGameDataContext data = new dbQLBanGameDataContext();

        private List<tblSanPham> Laygamemoi(int count)
        {
            //Sap xep
            return data.tblSanPhams.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        public ActionResult Index()
        {
            //Lay hang
            var gamemoi = Laygamemoi(30);
            return View(gamemoi);
        }

        public ActionResult Loaisp()
        {
            var loaisp = from lsp in data.tblLoaiSanPhams select lsp;
            return PartialView(loaisp);
        }

        public ActionResult SPTheotheloai(string id)
        {
            var game = from g in data.tblSanPhams where g.MaLoai==id select g;
            return View(game);
        }

        public ActionResult Details(string id)
        {
            var game = from g in data.tblSanPhams
                       where g.MaSP == id
                       select g;
            return View(game.Single());
        }
    }
}