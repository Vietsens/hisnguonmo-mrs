using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    public class Xml3ADO
    {
        public string MaLienKet { get; set; }
        public int Stt { get; set; }
        public string LoaiDichVu { get; set; }
        public string MaDichVu { get; set; }
        public string MaVatTu { get; set; }
        public string MaNhom { get; set; }
        public string GoiVTYT { get; set; }
        public string TenVatTu { get; set; }
        public string TenDichVu { get; set; }
        public string DonViTinh { get; set; }
        public int PhamVi { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string TTThau { get; set; }
        public decimal TyLeTT { get; set; }
        public decimal ThanhTien { get; set; }
        public decimal? TongTranTT { get; set; }
        public int MucHuong { get; set; }
        public decimal TongNguonKhac { get; set; }
        public decimal TongBNTT { get; set; }
        public decimal TongBHTT { get; set; }
        public decimal TongBNCCT { get; set; }
        public decimal TongNgoaiDS { get; set; }
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public string MaGiuong { get; set; }
        public string MaBacSi { get; set; }
        public string MaBacSiChiDinh { get; set; }
        public string TenBacSiChiDinh { get; set; }
        public string MaTatCaBacSi { get; set; }
        public string TenTatCaBacSi { get; set; }
        public string MaBenh { get; set; }
        public string NgayYLenh { get; set; }
        public string NgayKetQua { get; set; }
        public int MaPTTT { get; set; }
        public string MaCha { get; set; }

        public bool IsMaterial { get; set; }
    }
}
