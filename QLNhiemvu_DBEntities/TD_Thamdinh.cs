using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class TD_Thamdinh_Duyet
    {
        public Guid DM017101 { get; set; }
        public Guid DM017102 { get; set; }
        public int DM017103 { get; set; }
        public Guid DM017104 { get; set; }
        public string DM017105 { get; set; }
        public DateTime DM017106 { get; set; }
        public string DM017107 { get; set; }
        public Guid DM017108 { get; set; }
        public Guid DM017109 { get; set; }
        public char DM017110 { get; set; }
        public string DM017111 { get; set; }
        public DateTime DM017112 { get; set; }
        public Guid DM017113 { get; set; }
        public string DM017114 { get; set; }
        public char DM017115 { get; set; }
        public char DM017116 { get; set; }
        public string DM017117 { get; set; }
        public string DM017118 { get; set; }
        public string DM017119 { get; set; }
        public Guid DM017120 { get; set; }
        public string DM017121 { get; set; }


        public bool IsChecked { get; set; }
        public string DonviSudung { get; set; }
        public Guid MaPhanloaiNhiemvu { get; set; }
        public string PhanloaiNhiemvu { get; set; }
        public string Danhmuc { get; set; }
        public Guid MaCoquannhan { get; set; }
        public string Coquannhan { get; set; }
        public string Nguoithamdinh { get; set; }
        public Guid MaThutucNhiemvu { get; set; }
        public string ThutucNhiemvu { get; set; }
        public string TrinhduyetNhiemvu { get; set; }
        public string Loai { get; set; }
        public string Nguoiky { get; set; }
        public List<TD_Thamdinh_Duyet_Truongdulieu> Fields { get; set; }
    }

    public class TD_Thamdinh_Duyet_Truongdulieu
    {
        public Guid DM017201 { get; set; }
        public Guid DM017202 { get; set; }
        public Guid DM017203 { get; set; }
        public string DM017204 { get; set; }
        public Guid DM017205 { get; set; }

        public int Kieutruong { get; set; }
        public bool Batbuocnhap { get; set; }
        public string Maso { get; set; }
        public string Tentruong { get; set; }
        public string Tenhienthi { get; set; }
        public int Dorong { get; set; }
        public string DieukienDulieu { get; set; }
        public string CongcotDulieu { get; set; }
        public int Sapxep { get; set; }
        public List<TD_Thamdinh_Duyet_Truongdulieu> Children { get; set; }
        public List<object> LookupData { get; set; }
    }

    public class TD_Thamdinh_Filter
    {
        public int Nam { get; set; }
        public Guid Donviphancong { get; set; }
        public Guid Trangthai { get; set; }
        public Guid PhanloaiNhiemvu { get; set; }
        public Guid DanhmucNhiemvu { get; set; }
        public Guid LoaiThutuc { get; set; }

        public Guid Nguoiphancong { get; set; }
    }

    public class TD_Thamdinh_Duyet_Filter
    {
        public Guid DuyetID { get; set; }
        public int Nam { get; set; }
        public string Sovanban { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Guid MaPhanloai { get; set; }
        public Guid MaDanhmuc { get; set; }
    }

    public class TD_Thamdinh_Duyet_FilterOne
    {
        public Guid DonviId { get; set; }
        public int Nam { get; set; }
        public Guid MaDanhmuc { get; set; }
        public Guid Nguoibanhanh { get; set; }
        public Guid MaNoidung { get; set; }

    }

    public class TD_Thamdinh_Duyet_Phongban
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
