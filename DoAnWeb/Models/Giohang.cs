using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnWeb.Models;

namespace DoAnWeb.Models
{
    public class Giohang
    {
        //Tao doi tuong data giohang
        dbQLBanGameDataContext data = new dbQLBanGameDataContext();
        public string iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sHinhAnh { set; get; }
        public Double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        //Khoi tao gio hang theo MaSP
        public Giohang(string MaSP)
        {
            iMaSP = MaSP;
            tblSanPham game = data.tblSanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = game.TenSP;
            sHinhAnh = game.HinhAnh;
            dDonGia = double.Parse(game.GiaTien.ToString());
            iSoLuong = 1;
        }
    }
}