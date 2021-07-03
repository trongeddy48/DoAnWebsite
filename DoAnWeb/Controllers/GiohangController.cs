using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class GiohangController : Controller
    {
        //Tao doi tuong data
        dbQLBanGameDataContext data = new dbQLBanGameDataContext();
        //GET: Giohang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang == null)
            {
                //Neu chua co gio hang thi tao list
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Them hang vao gio
        public ActionResult ThemGiohang(string iMaSP, string strURL)
        {
            //Lay ra Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra co SP trong Session chua
            Giohang sanpham = lstGiohang.Find(n => n.iMaSP == iMaSP);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        //Tong soluong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang!= null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        //Tong tien
        private double Tongtien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongtien;
        }

        //Build trang giohang
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GameProduct");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            return View(lstGiohang);
        }

        //Tao Partial view
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            return PartialView();
        }

        //Xoa Giohang
        public ActionResult XoaGiohang (string iMaSP)
        {
            //Lay giohang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra hang co trong session
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            //Neu co cho sua soluong
            if(sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GameProduct");
            }
            return RedirectToAction("GioHang");
        }

        //Cap nhat giohang
        public ActionResult CapnhatGiohang (string iMaSP, FormCollection f)
        {
            //Lay giohang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra hang co trong session
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            //Neu ton tai thi cho sua soluong
            if(sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }

        //Xoa Gio hang
        public ActionResult XoaTatcaGiohang()
        {
            //Lay giohang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "GameProduct");
        }

        //Hien thi View DatHang de cap nhat cac thong tin cho Don hang 
        [HttpGet]
        public ActionResult DatHang()
        {
            //kiem tra dang nhap 
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            //Lay gio hang tu Session 
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();

            return View(lstGiohang);
        }
    }
}