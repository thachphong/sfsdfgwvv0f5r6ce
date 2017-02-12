using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class DM_Huongdan
    {
        public Guid DM016301 { get; set; }
        public Guid DM016302 { get; set; }
        public string DM016303 { get; set; }
        public string DM016304 { get; set; }
        public char DM016305 { get; set; }
        public Guid DM016306 { get; set; }
        public DateTime DM016307 { get; set; }
        public Guid DM016308 { get; set; }
        public DateTime DM016309 { get; set; }
        public string DM016310 { get; set; }
        public Guid DM016311 { get; set; }

        public bool IsChecked { get; set; }
        public string LoaiThutucNhiemvu { get; set; }
        public string LoaiHuongdan { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapnhat { get; set; }
    }

    public class DM_Huongdan_LoaiHuongdan
    {
        public char ID { get; set; }
        public string Description { get; set; }
    }
}
