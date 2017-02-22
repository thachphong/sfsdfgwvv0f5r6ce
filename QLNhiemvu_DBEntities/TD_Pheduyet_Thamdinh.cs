using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class TD_Pheduyet_Thamdinh_Filter
    {
        public Guid PhongbanBophan { get; set; }
    }

    public class TD_Pheduyet_Thamdinh_Duyet
    {
        public Guid DM017501 { get; set; }
        public Guid DM017502 { get; set; }
        public int DM017503 { get; set; }
        public Guid DM017504 { get; set; }
        public string DM017505 { get; set; }
        public string DM017506 { get; set; }
        public string DM017507 { get; set; }
        public DateTime DM017508 { get; set; }
        public string DM017509 { get; set; }
        public Guid DM017510 { get; set; }
        public Guid DM017511 { get; set; }
        public Guid DM017512 { get; set; }
        public string DM017513 { get; set; }
        public string DM017514 { get; set; }
        public Guid DM017515 { get; set; }
        public DateTime DM017516 { get; set; }
        public Guid DM017517 { get; set; }
        public DateTime DM017518 { get; set; }
        public byte DM017519 { get; set; }

        public bool IsChecked { get; set; }
        public string DonviSudung { get; set; }
        public Guid MaPhanloaiNhiemvu { get; set; }
        public string PhanloaiNhiemvu { get; set; }
        public string Danhmuc { get; set; }
        public string Capbanhanh { get; set; }
        public string Nguoibanhanh { get; set; }
        public string Chucvu { get; set; }
        public Guid MaThutucNhiemvu { get; set; }
        public string ThutucNhiemvu { get; set; }
        public string NoidungChitiet { get; set; }
        public string Nguoitao { get; set; }
        public string Nguoicapnhat { get; set; }
        public List<TD_Pheduyet_Thamdinh_Duyet_Tepdinhkem> AttachedFiles { get; set; }
        public List<TD_Pheduyet_Thamdinh_Duyet_Truongdulieu> Fields { get; set; }
        public int KyhieuId { get; set; }
    }

    public class TD_Pheduyet_Thamdinh_Duyet_Tepdinhkem
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TD_Pheduyet_Thamdinh_Duyet_Truongdulieu
    {
        public Guid DM017601 { get; set; }
        public Guid DM017602 { get; set; }
        public Guid DM017603 { get; set; }
        public string DM017604 { get; set; }
        public Guid DM017605 { get; set; }

        public int Kieutruong { get; set; }
        public bool Batbuocnhap { get; set; }
        public string Maso { get; set; }
        public string Tentruong { get; set; }
        public string Tenhienthi { get; set; }
        public int Dorong { get; set; }
        public string DieukienDulieu { get; set; }
        public string CongcotDulieu { get; set; }
        public int Sapxep { get; set; }
        public List<TD_Pheduyet_Thamdinh_Duyet_Truongdulieu> Children { get; set; }
        public List<object> LookupData { get; set; }
    }

    public class TD_Pheduyet_Thamdinh_Duyet_Filter
    {
        public Guid DuyetID { get; set; }
        public string Sovanban { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Guid MaPhanloai { get; set; }
        public Guid MaDanhmuc { get; set; }
    }
}
