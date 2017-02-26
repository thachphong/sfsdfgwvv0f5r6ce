using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class TD_ThuchienNhiemvu
    {
        public Guid DM016701 { get; set; }
        public Guid DM016702 { get; set; }
        public int DM016703 { get; set; }
        public Guid DM016705 { get; set; }
        public string DM016706 { get; set; }
        public DateTime DM016707 { get; set; }
        public string DM016708 { get; set; }
        public Guid DM016710 { get; set; }
        public string DM016711 { get; set; }
        public Guid DM016713 { get; set; }
        public string DM016714 { get; set; }
        public string DM016715 { get; set; }
        public Guid DM016716 { get; set; }
        public DateTime DM016717 { get; set; }
        public Guid DM016718 { get; set; }
        public DateTime DM016719 { get; set; }
        public Guid DM016720 { get; set; }

        public bool IsChecked { get; set; }
        public string DonviSudung { get; set; }
        public Guid MaPhanloaiNhiemvu { get; set; }
        public string PhanloaiNhiemvu { get; set; }
        public string Danhmuc { get; set; }
        public Guid MaCoquannhan { get; set; }
        public string Coquannhan { get; set; }
        public string Nguoiky { get; set; }
        public Guid MaThutucNhiemvu { get; set; }
        public string ThutucNhiemvu { get; set; }
        public string NoidungChitiet { get; set; }
        public string Nguoitao { get; set; }
        public string Nguoicapnhat { get; set; }
        public string Trangthai { get; set; }
        public List<TD_ThuchienNhiemvu_Tepdinhkem> AttachedFiles { get; set; }
        public List<TD_ThuchienNhiemvu_Truongdulieu> Fields { get; set; }
        public List<TD_Phancong> DsPhancong { get; set; }
        public int SongayHoanthanh { get; set; }
        public bool Quahan { get; set; }
        public int LoaiNoidungchitiet { get; set; }
        public List<TD_ThuchienNhiemvu> ListDiffBy_Noidung { get; set; }
    }

    public class TD_ThuchienNhiemvu_Tepdinhkem
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TD_ThuchienNhiemvu_Truongdulieu
    {
        public Guid DM016801 { get; set; }
        public Guid DM016802 { get; set; }
        public Guid DM016803 { get; set; }
        public string DM016804 { get; set; }

        public int Kieutruong { get; set; }
        public bool Batbuocnhap { get; set; }
        public string Maso { get; set; }
        public string Tentruong { get; set; }
        public string Tenhienthi { get; set; }
        public int Dorong { get; set; }
        public string DieukienDulieu { get; set; }
        public string CongcotDulieu { get; set; }
        public int Sapxep { get; set; }
        public List<TD_ThuchienNhiemvu_Truongdulieu> Children { get; set; }
        public List<object> LookupData { get; set; }
    }

    public class TD_ThuchienNhiemvu_PhanloaiNhiemvu
    {
        public Guid DM014001 { get; set; }
        public string DM014003 { get; set; }
        public List<TD_ThuchienNhiemvu_DanhmucNhiemvu> DSDanhmuc { get; set; }
    }

    public class TD_ThuchienNhiemvu_DanhmucNhiemvu
    {
        public Guid DM020201 { get; set; }
        public string DM020204 { get; set; }
    }

    public class TD_DonviQuanly
    {
        public Guid DM030101 { get; set; }
        public string DM030105 { get; set; }
        public List<TD_Nguoiky> DSNguoiky { get; set; }
        public bool IsChecked { get; set; }
        public bool IsLeader { get; set; }
        public TD_DonviQuanly()
        {
            IsChecked = true;
            IsLeader = false;
        }
    }

    public class TD_Capbanhanh
    {
        public Guid DM010701 { get; set; }
        public string DM010703 { get; set; }
        public List<TD_Nguoiky> DSNguoibanhanh { get; set; }
    }

    public class TD_Nguoiky
    {
        public Guid DM030401 { get; set; }
        public string DM030403 { get; set; }
        public string Chucvu { get; set; }
        public Guid MaDonvi { get; set; }
    }

    public class TD_TrangthaiHoSo
    {
        public Guid DM012401 { get; set; }
        public string DM012402 { get; set; }
        public string DM012403 { get; set; }
    }

    public class TD_ThuchienNhiemvu_Filter
    {
        public int Nam { get; set; }
        public string Sovanban { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Guid MaPhanloai { get; set; }
        public Guid MaDanhmuc { get; set; }
        public Guid MaDMNhiemvu { get; set; }
    }

    public class TD_ThuchienNhiemvu_Cothienthi
    {
        public bool IsChecked { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
    }
}
