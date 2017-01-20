using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class TD_Phancong_CotHienthi
    {
        public Guid ID { get; set; } //Tác dụng tránh trùng - xử lý bussines
        public string DisplayName { get; set; } //Tên hiển thị trên lưới
        public string PropertyName { get; set; } //Tên thuộc tính được map
        public bool IsChecked { get; set; } //Ô check trên grid
        public int Order { get; set; }
    }

    public class TD_Phancong
    {
        public Guid DM017001 { get; set; }
        public Guid DM017002 { get; set; }
        public char DM017003 { get; set; }
        public Guid DM017004 { get; set; }
        public Guid DM017005 { get; set; }
        public DateTime DM017006 { get; set; }
        public DateTime DM017007 { get; set; }
        public string DM017008 { get; set; }
        public Guid DM017009 { get; set; }
        public DateTime DM017010 { get; set; }
        public Guid DM017011 { get; set; }
        public DateTime DM017012 { get; set; }

        public bool IsChecked { get; set; }
        public string DonviNhanVB { get; set; }
        public string NguoinhanVB { get; set; }
        public string Nguoitao { get; set; }
        public string Nguoicapnhat { get; set; }
    }

    public class TD_Phancong_Phamvi
    {
        public char ID { get; set; }
        public string Description { get; set; }
    }

    public class TD_Phancong_Thamquyen
    {
        public char ID { get; set; }
        public string Description { get; set; }
    }

    public class TD_Phancong_Filter
    {
        public int Nam { get; set; }
        public char Phamviphancong { get; set; }
        public char Thamquyenphancong { get; set; }
        public Guid Donviphancong { get; set; }
        public Guid Nguoinhanvanbanden { get; set; }
        public Guid Trangthai { get; set; }
        public Guid Nguoiphancong { get; set; }
    }

    public class TD_Phancong_DoituongNhanVB
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
        public Guid MaDonvi { get; set; }

        public TD_Phancong_DoituongNhanVB(TD_DonviQuanly obj)
        {
            ID = obj.DM030101;
            Description = obj.DM030105;
            MaDonvi = Guid.Empty;
        }

        public TD_Phancong_DoituongNhanVB(TD_Nguoiky obj)
        {
            ID = obj.DM030401;
            Description = obj.DM030403;
            MaDonvi = obj.MaDonvi;
        }
    }
}
