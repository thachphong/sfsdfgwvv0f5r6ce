using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class TD_Pheduyet_VB
    {
        public Guid DM017301 { get; set; }
        public Guid DM017302 { get; set; }
        public int DM017303 { get; set; }
        public Guid DM017304 { get; set; }
        public string DM017305 { get; set; }
        public string DM017306 { get; set; }
        public string DM017307 { get; set; }
        public DateTime DM017308 { get; set; }
        public string DM017309 { get; set; }
        public Guid DM017310 { get; set; }
        public Guid DM017311 { get; set; }
        public Guid DM017312 { get; set; }
        public string DM017313 { get; set; }
        public string DM017314 { get; set; }
        public Guid DM017315 { get; set; }
        public DateTime DM017316 { get; set; }
        public Guid DM017317 { get; set; }
        public DateTime DM017318 { get; set; }
        public byte DM017319 { get; set; }

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
        public List<TD_Pheduyet_VB_Tepdinhkem> AttachedFiles { get; set; }
        public List<TD_Pheduyet_VB_Truongdulieu> Fields { get; set; }
        public int KyhieuId { get; set; }
    }

    public class TD_Pheduyet_VB_Tepdinhkem
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TD_Pheduyet_VB_Truongdulieu
    {
        public Guid DM017401 { get; set; }
        public Guid DM017402 { get; set; }
        public Guid DM017403 { get; set; }
        public string DM017404 { get; set; }
        public Guid DM017405 { get; set; }

        public int Kieutruong { get; set; }
        public bool Batbuocnhap { get; set; }
        public string Maso { get; set; }
        public string Tentruong { get; set; }
        public string Tenhienthi { get; set; }
        public int Dorong { get; set; }
        public string DieukienDulieu { get; set; }
        public string CongcotDulieu { get; set; }
        public int Sapxep { get; set; }
        public List<TD_Pheduyet_VB_Truongdulieu> Children { get; set; }
        public List<object> LookupData { get; set; }
    }

    public class TD_Pheduyet_VB_Filter
    {
        public string Sovanban { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Guid MaPhanloai { get; set; }
        public Guid MaDanhmuc { get; set; }
    }

    public class Kyhieuvanban
    {
        public int Id { get; set; }
        public string Loaivanban { get; set; }
        public string Donvibanhanh { get; set; }
        public string Kyhieu { get; set; }
    }
}
