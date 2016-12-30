using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class DM_LoaiThutucNhiemvu_Truongdulieu
    {
        public Guid DM016201 { get; set; }
        public string DM016204 { get; set; }
        public string DM016205 { get; set; }
        public string DM016206 { get; set; }
        public string DM016207 { get; set; }
        public int DM016208 { get; set; }
        public string DM016209 { get; set; }
        public string DM016210 { get; set; }
        public string DM016211 { get; set; }
        public string DM016212 { get; set; }
        public string DM016213 { get; set; }
        public int DM016214 { get; set; }
        public char DM016215 { get; set; }
        public Guid DM016217 { get; set; }
        public DateTime DM016218 { get; set; }
        public Guid DM016219 { get; set; }
        public DateTime DM016220 { get; set; }

        public bool IsChecked { get; set; }
        public string Kieutruong { get; set; }
        public string Cachnhap { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapnhat { get; set; }
    }

    public class DM_LoaiThutucNhiemvu_Truongdulieu_Kieutruong
    {
        public string ID { get; set; }
        public string Description { get; set; }
    }
    public class DM_LoaiThutucNhiemvu_Truongdulieu_Cachnhap
    {
        public string ID { get; set; }
        public string Description { get; set; }
    }
}
