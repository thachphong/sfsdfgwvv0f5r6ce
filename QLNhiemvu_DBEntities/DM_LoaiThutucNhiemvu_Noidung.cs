using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class DM_LoaiThutucNhiemvu_Noidung
    {
        public Guid DM016101 { get; set; }
        public Guid DM016102 { get; set; }
        public string DM016103 { get; set; }
        public string DM016104 { get; set; }
        public char DM016105 { get; set; }
        public Guid DM016106 { get; set; }
        public DateTime DM016107 { get; set; }
        public Guid DM016108 { get; set; }
        public DateTime DM016109 { get; set; }
        public string DM016110 { get; set; }
        public Guid DM016111 { get; set; }

        public bool IsChecked { get; set; }
        public string LoaiThutucNhiemvu { get; set; }
        public string Cachnhap { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapnhat { get; set; }
        public List<Guid> FieldSelecteds { get; set; }
        public List<DM_LoaiThutucNhiemvu_Truongdulieu> DsTruongdulieu { get; set; }
    }

    public class DM_LoaiThutucNhiemvu_Noidung_Cachnhap
    {
        public char ID { get; set; }
        public string Description { get; set; }
    }
}
