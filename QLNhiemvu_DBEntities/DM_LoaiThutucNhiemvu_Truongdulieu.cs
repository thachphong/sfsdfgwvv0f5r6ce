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
        public string DM016213 { get; set; }
        public int DM016214 { get; set; }
        public char DM016215 { get; set; }
        public Guid DM016216 { get; set; }
        public Guid DM016217 { get; set; }
        public DateTime DM016218 { get; set; }
        public Guid DM016219 { get; set; }
        public DateTime DM016220 { get; set; }
        public Guid DM016221 { get; set; }

        public bool IsChecked { get; set; }
        public string Kieutruong { get; set; }
        public string Cachnhap { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiCapnhat { get; set; }
        public int Level { get; set; }
        public string Maso { get; set; }
        public string Tentruong { get; set; }

        public List<DM_LoaiThutucNhiemvu_Truongdulieu> DsTruongcon { get; set; }
        public Guid NoidungId { get; set; }
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

    public class DM_LoaiThutucNhiemvu_Truongdulieu_LookupData
    {
        public string Table { get; set; }
        public string ColumnSave { get; set; }
        public string ColumnDisplayID { get; set; }
        public string ColumnDisplayName { get; set; }
        public string ColumnDisplayExtend1 { get; set; }
        public string ColumnDisplayExtend2 { get; set; }
        public DM_LoaiThutucNhiemvu_Truongdulieu_LookupData_Condition Condition1 { get; set; }
        public DM_LoaiThutucNhiemvu_Truongdulieu_LookupData_Condition Condition2 { get; set; }
        public string ConditionCombination { get; set; }
    }
    public class DM_LoaiThutucNhiemvu_Truongdulieu_LookupData_Condition
    {
        public string ColumnName { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
