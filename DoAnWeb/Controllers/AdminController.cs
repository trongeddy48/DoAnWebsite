using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

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

        public ActionResult Game(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.tblSanPhams.ToList());
            return View(db.tblSanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
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
        
        [HttpGet]
        public ActionResult ThemmoiSP()
        {
            //Dua data vao dropdownlist
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSP(tblSanPham sanPham, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            //Kiem tra duong dan file
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh sản phẩm !";
                return View();
            }
            //Them vao csdl
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan
                    var path = Path.Combine(Server.MapPath("~/Content/HinhSanPham"), fileName);
                    //Kiem tra hinh da ton tai chua
                    string min = DateTime.Now.ToString("mm");
                    string sec = DateTime.Now.ToString("ss");
                    string MaSanPham = "S" + "" + min + "" + sec;
                    sanPham.MaSP = MaSanPham;
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại !";
                    }
                    else
                    {
                        //Luu hinh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    sanPham.HinhAnh = fileName;
                    //Luu vao csdl
                    db.tblSanPhams.InsertOnSubmit(sanPham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Game");
            }
        }

        //Edit sp
        [HttpGet]
        public ActionResult SuaSP(string id)
        {
            tblSanPham sanPham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sanPham.MaNCC);
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", sanPham.MaTH);
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(tblSanPham sanPham, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            //Kiem tra duong dan
            if(fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh !";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan
                    var path = Path.Combine(Server.MapPath("~/Content/HinhSanPham"), fileName);
                    //Kiem tra hinh da ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại !";
                    }
                    else
                    {
                        //Luu hinh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanPham.HinhAnh = fileName;
                    //Luu vao csdl
                    UpdateModel(sanPham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Game");
            }
        }

        //Hien thi san pham
        public ActionResult ChitietSP(string id)
        {
            //Lay sp theo ma
            tblSanPham sanPham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);
        }

        //Xoa sp
        [HttpGet]
        public ActionResult XoaSP(string id)
        {
            //Lay ra sp can xoa theo ma~
            tblSanPham sanPham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);
        }

        [HttpPost, ActionName("XoaSP")]
        public ActionResult Xacnhanxoa(string id)
        {
            //Lay sp can xoa theo ma~
            tblSanPham sanPham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.tblSanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("Game");
        }

        //QL loai sp
        public ActionResult Loaisanpham()
        {
            return View(db.tblLoaiSanPhams.ToList());
        }

        [HttpGet]
        public ActionResult Themloaisanpham()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Themloaisanpham(FormCollection collection, tblLoaiSanPham lsp)
        {
            //Tạo biến loaisanpham và gán giá trị của người dùng nhập vào
            var loaisp = collection["TenLoai"];
            //nếu loaisanpham có giá trị == null (để trống)
            if (string.IsNullOrEmpty(loaisp))
            {
                ViewData["Loi"] = "Tên loại sản phẩm không được để trống";
            }
            else
            {
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaLoaiSanPham = "L" + "" + min + "" + sec;
                lsp.MaLoai = MaLoaiSanPham;
                lsp.TenLoai = loaisp;
                db.tblLoaiSanPhams.InsertOnSubmit(lsp);
                db.SubmitChanges();
                return RedirectToAction("Loaisanpham");
            }
            return this.Themloaisanpham();
        }

        //Sua loai san pham
        [HttpGet]
        public ActionResult Sualoaisp(string id)
        {
            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            return View(loaisp);
        }
        [HttpPost]
        public ActionResult Sualoaisp(string id, FormCollection collection)
        {
            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            var lsp = collection["TenLoai"];
            loaisp.MaLoai = id;
            if (string.IsNullOrEmpty(lsp))
            {
                ViewData["Loi"] = "Loại sản phẩm  không được để trống";
            }
            else
            {
                loaisp.TenLoai = lsp;
                UpdateModel(lsp);
                db.SubmitChanges();
                return RedirectToAction("Loaisanpham");
            }
            return this.Sualoaisp(id);
        }

        [HttpGet]
        public ActionResult Xoaloaisp(string id)
        {
            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            return View(loaisp);
        }
        [HttpPost]
        public ActionResult Xoaloaisp(string id, FormCollection collection)
        {
            var loaisp = db.tblLoaiSanPhams.Where(m => m.MaLoai == id).First();
            db.tblLoaiSanPhams.DeleteOnSubmit(loaisp);
            db.SubmitChanges();
            return RedirectToAction("Loaisanpham");
        }
    }
}