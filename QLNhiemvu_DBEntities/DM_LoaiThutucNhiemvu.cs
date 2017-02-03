using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class DM_LoaiThutucNhiemvu
    {
        public Guid DM016001 { get; set; }
        public Guid DM016002 { get; set; }
        public string DM016003 { get; set; }
        public string DM016004 { get; set; }
        public char DM016005 { get; set; }
        public Guid DM016006 { get; set; }
        public DateTime DM016007 { get; set; }
        public Guid DM016008 { get; set; }
        public DateTime DM016009 { get; set; }
        public char DM016010 { get; set; }
        public Guid DM016011 { get; set; }
        public string DM016012 { get; set; }

        public bool IsChecked { get; set; }
        public string DonviSudung { get; set; }
        public string LoaiCapphep { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapnhat { get; set; }
        public List<Guid> FieldSelecteds { get; set; }

        public List<DM_LoaiThutucNhiemvu_Noidung> DsNoidung { get; set; }
        public List<DM_Huongdan> DsHuongdan { get; set; }
        public List<TD_ThuchienNhiemvu_Cothienthi> DsCothienthi { get; set; }
    }

    public class DM_LoaiThutucNhiemvu_LoaiCapphep
    {
        public char ID { get; set; }
        public string Description { get; set; }
    }

    public class DM_LoaiThutucNhiemvu_Filter
    {
        public string Ten { get; set; }
        public char Phamvisudung { get; set; }
        public Guid Loai { get; set; }
    }
}
