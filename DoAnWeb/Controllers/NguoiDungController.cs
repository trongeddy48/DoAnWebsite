﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, tblKhacHang kh)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (string.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["loi5"] = "Phải nhập email";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Phải nhập số điện thoại";
            }
            else
            {
                //gán giá trị cho đối tượng được tạo mới (kh)
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaKhachHang = "K" + "" + min + "" + sec;
                kh.MaKH = MaKhachHang;
                kh.TenKH = hoten;
                kh.Username = tendn;
                kh.Password = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                db.tblKhacHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            //Gán giá trị ng dùng
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng tạo mới (kh)
                tblKhacHang kh = db.tblKhacHangs.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công !";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "GameProduct");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng !";
            }
            return View();
        }
    }
}