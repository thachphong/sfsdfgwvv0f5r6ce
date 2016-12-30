using QLNhiemVu;
using QLNhiemvu_DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace QLNhiemvu_WebAPI.DAL
{
    public class DataTools : IDisposable
    {
        #region Constructor

        private DBDataContext db = null;
        public DataTools()
        {
            db = new DBDataContext();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        #endregion

        #region Authentication

        public Authentication GetAuth(string username, string password)
        {
            try
            {
                using (DataTools dataTool = new DataTools())
                {
                    return db.Authentications.FirstOrDefault(o =>
                        o.Username.ToLower() == username.ToLower() &&
                        o.Password == password);
                }
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        #endregion

        #region DM_LoaiThutucNhiemvu

        public List<DM_LoaiThutucNhiemvu> LoaiThutucNhiemvu_GetList()
        {
            try
            {
                var list = db.DBDM0160s.Where(o => !o.DM016011);
                if (list.Count() == 0) return null;

                List<DM_LoaiThutucNhiemvu> result = new List<DM_LoaiThutucNhiemvu>();
                foreach (DBDM0160 obj in list.OrderByDescending(o => o.DM016007))
                {
                    result.Add(new DM_LoaiThutucNhiemvu()
                    {
                        DM016001 = obj.DM016001,
                        DM016002 = obj.DM016002,
                        DM016003 = obj.DM016003,
                        DM016004 = obj.DM016004,
                        DM016005 = obj.DM016005,
                        DM016006 = obj.DM016006,
                        DM016007 = obj.DM016007,
                        DM016008 = obj.DM016008,
                        DM016009 = obj.DM016009,
                        DM016010 = obj.DM016010,
                        IsChecked = false,
                        DonviSudung = "Cơ quan X",
                        LoaiCapphep =
                            obj.DM016005 == '1' ? "Tất cả" :
                            obj.DM016005 == '2' ? "Đơn vị sử dụng chương trình cụ thể" :
                            "Đơn vị trong hệ thống đều dùng",
                        NguoiCapnhat = "Nguyễn văn XXX",
                        NguoiTao = "Nguyễn văn XXX"
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public int LoaiThutucNhiemvu_Create(DM_LoaiThutucNhiemvu obj)
        {
            try
            {
                DBDM0160 checkObj = db.DBDM0160s.FirstOrDefault(o => !o.DM016011 && o.DM016003 == obj.DM016003);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0160s.FirstOrDefault(o => !o.DM016011 && o.DM016004 == obj.DM016004);
                if (checkObj != null)
                    return 2;//Trung Ten

                db.DBDM0160s.InsertOnSubmit(new DBDM0160()
                {
                    DM016001 = obj.DM016001,
                    DM016002 = obj.DM016002,
                    DM016003 = obj.DM016003,
                    DM016004 = obj.DM016004,
                    DM016005 = obj.DM016005,
                    DM016006 = obj.DM016006,
                    DM016007 = obj.DM016007,
                    DM016008 = obj.DM016008,
                    DM016009 = obj.DM016009,
                    DM016010 = obj.DM016010,
                    DM016011 = false,
                });
                db.SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Update(DM_LoaiThutucNhiemvu obj)
        {
            try
            {
                DBDM0160 updateObj = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == obj.DM016001);
                if (updateObj == null) return -1;

                DBDM0160 checkObj = db.DBDM0160s.FirstOrDefault(o => !o.DM016011 && o.DM016001 != obj.DM016001 && o.DM016003 == obj.DM016003);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0160s.FirstOrDefault(o => !o.DM016011 && o.DM016001 != obj.DM016001 && o.DM016004 == obj.DM016004);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016002 = obj.DM016002;
                updateObj.DM016003 = obj.DM016003;
                updateObj.DM016004 = obj.DM016004;
                updateObj.DM016005 = obj.DM016005;
                updateObj.DM016008 = obj.DM016008;
                updateObj.DM016009 = obj.DM016009;
                updateObj.DM016010 = obj.DM016010;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Delete(Guid id)
        {
            try
            {
                DBDM0160 obj = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == id);
                if (obj == null) return -1;

                obj.DM016011 = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        #endregion

        #region DM_LoaiThutucNhiemvu_Huongdan

        public List<DM_Huongdan> LoaiThutucNhiemvu_Huongdan_GetList()
        {
            try
            {
                var list = db.DBDM0163s.Where(o => !o.DM016310);
                if (list.Count() == 0) return null;

                List<DM_Huongdan> result = new List<DM_Huongdan>();
                foreach (DBDM0163 obj in list.OrderByDescending(o => o.DM016307))
                {
                    //DBDM0160 loaiThutuc = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == obj.DM016302);
                    //if (loaiThutuc == null) continue;

                    result.Add(new DM_Huongdan()
                    {
                        DM016301 = obj.DM016301,
                        DM016302 = obj.DM016302,
                        DM016303 = obj.DM016303,
                        DM016304 = obj.DM016304,
                        DM016305 = obj.DM016305,
                        DM016306 = obj.DM016306,
                        DM016307 = obj.DM016307,
                        DM016308 = obj.DM016308,
                        DM016309 = obj.DM016309,
                        IsChecked = false,
                        LoaiThutucNhiemvu = obj.DBDM0160.DM016004,
                        LoaiHuongdan =
                            obj.DM016305 == '1' ? "Tệp văn bản" :
                            obj.DM016305 == '2' ? "Tệp video" :
                            "Tệp audio",
                        NguoiCapnhat = "Nguyễn văn XXX",
                        NguoiTao = "Nguyễn văn XXX"
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public int LoaiThutucNhiemvu_Huongdan_Create(DM_Huongdan obj)
        {
            try
            {
                DBDM0163 checkObj = db.DBDM0163s.FirstOrDefault(o => !o.DM016310 && o.DM016303 == obj.DM016303);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0163s.FirstOrDefault(o => !o.DM016310 && o.DM016304 == obj.DM016304);
                if (checkObj != null)
                    return 2;//Trung Ten

                db.DBDM0163s.InsertOnSubmit(new DBDM0163()
                {
                    DM016301 = obj.DM016301,
                    DM016302 = obj.DM016302,
                    DM016303 = obj.DM016303,
                    DM016304 = obj.DM016304,
                    DM016305 = obj.DM016305,
                    DM016306 = obj.DM016306,
                    DM016307 = obj.DM016307,
                    DM016308 = obj.DM016308,
                    DM016309 = obj.DM016309,
                    DM016310 = false
                });
                db.SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Huongdan_Update(DM_Huongdan obj)
        {
            try
            {
                DBDM0163 updateObj = db.DBDM0163s.FirstOrDefault(o => o.DM016301 == obj.DM016301);
                if (updateObj == null) return -1;

                DBDM0163 checkObj = db.DBDM0163s.FirstOrDefault(o => !o.DM016310 && o.DM016301 != obj.DM016301 && o.DM016303 == obj.DM016303);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0163s.FirstOrDefault(o => !o.DM016310 && o.DM016301 != obj.DM016301 && o.DM016304 == obj.DM016304);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016302 = obj.DM016302;
                updateObj.DM016303 = obj.DM016303;
                updateObj.DM016304 = obj.DM016304;
                updateObj.DM016305 = obj.DM016305;
                updateObj.DM016308 = obj.DM016308;
                updateObj.DM016309 = obj.DM016309;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Huongdan_Delete(Guid id)
        {
            try
            {
                DBDM0163 obj = db.DBDM0163s.FirstOrDefault(o => o.DM016301 == id);
                if (obj == null) return -1;

                obj.DM016310 = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        #endregion

        #region DM_LoaiThutucNhiemvu_Noidung

        public List<DM_LoaiThutucNhiemvu_Noidung> LoaiThutucNhiemvu_Noidung_GetList(Guid thutucID)
        {
            try
            {
                var list = db.DBDM0161s.Where(o => !o.DM016111 && o.DM016102 == thutucID);
                if (list.Count() == 0) return null;

                List<DM_LoaiThutucNhiemvu_Noidung> result = new List<DM_LoaiThutucNhiemvu_Noidung>();
                foreach (DBDM0161 obj in list.OrderByDescending(o => o.DM016107))
                {
                    //DBDM0160 loaiThutuc = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == obj.DM016102);
                    //if (loaiThutuc == null) continue;

                    result.Add(new DM_LoaiThutucNhiemvu_Noidung()
                    {
                        DM016101 = obj.DM016101,
                        DM016102 = obj.DM016102,
                        DM016103 = obj.DM016103,
                        DM016104 = obj.DM016104,
                        DM016105 = obj.DM016105,
                        DM016106 = obj.DM016106,
                        DM016107 = obj.DM016107,
                        DM016108 = obj.DM016108,
                        DM016109 = obj.DM016109,
                        IsChecked = false,
                        LoaiThutucNhiemvu = obj.DBDM0160.DM016004,
                        Cachnhap =
                            obj.DM016105 == '1' ? "Nhập đoạn văn" :
                            "Nhập các trường dữ liệu",
                        NguoiCapnhat = "Nguyễn văn XXX",
                        NguoiTao = "Nguyễn văn XXX"
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public int LoaiThutucNhiemvu_Noidung_Create(DM_LoaiThutucNhiemvu_Noidung obj)
        {
            try
            {
                DBDM0161 checkObj = db.DBDM0161s.FirstOrDefault(o => !o.DM016111 && o.DM016103 == obj.DM016103);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0161s.FirstOrDefault(o => !o.DM016111 && o.DM016104 == obj.DM016104);
                if (checkObj != null)
                    return 2;//Trung Ten

                db.DBDM0161s.InsertOnSubmit(new DBDM0161()
                {
                    DM016101 = obj.DM016101,
                    DM016102 = obj.DM016102,
                    DM016103 = obj.DM016103,
                    DM016104 = obj.DM016104,
                    DM016105 = obj.DM016105,
                    DM016106 = obj.DM016106,
                    DM016107 = obj.DM016107,
                    DM016108 = obj.DM016108,
                    DM016109 = obj.DM016109,
                    DM016110 = obj.DM016110,
                    DM016111 = false,
                });
                db.SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Noidung_Update(DM_LoaiThutucNhiemvu_Noidung obj)
        {
            try
            {
                DBDM0161 updateObj = db.DBDM0161s.FirstOrDefault(o => o.DM016101 == obj.DM016101);
                if (updateObj == null) return -1;

                DBDM0161 checkObj = db.DBDM0161s.FirstOrDefault(o => !o.DM016111 && o.DM016101 != obj.DM016101 && o.DM016103 == obj.DM016103);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0161s.FirstOrDefault(o => !o.DM016111 && o.DM016101 != obj.DM016101 && o.DM016104 == obj.DM016104);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016102 = obj.DM016102;
                updateObj.DM016103 = obj.DM016103;
                updateObj.DM016104 = obj.DM016104;
                updateObj.DM016105 = obj.DM016105;
                updateObj.DM016108 = obj.DM016108;
                updateObj.DM016109 = obj.DM016109;
                updateObj.DM016110 = obj.DM016110;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Noidung_Delete(Guid id)
        {
            try
            {
                DBDM0161 obj = db.DBDM0161s.FirstOrDefault(o => o.DM016101 == id);
                if (obj == null) return -1;

                obj.DM016111 = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        #endregion

        #region DM_LoaiThutucNhiemvu_Truongdulieu

        public List<DM_LoaiThutucNhiemvu_Truongdulieu> LoaiThutucNhiemvu_Truongdulieu_GetList()
        {
            try
            {
                var list = db.DBDM0162s.Where(o => !o.DM016221);
                if (list.Count() == 0) return null;

                List<DM_LoaiThutucNhiemvu_Truongdulieu> result = new List<DM_LoaiThutucNhiemvu_Truongdulieu>();
                foreach (DBDM0162 obj in list.OrderByDescending(o => o.DM016207))
                {
                    //DBDM0160 loaiThutuc = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == obj.DM016202);
                    //if (loaiThutuc == null) continue;

                    result.Add(new DM_LoaiThutucNhiemvu_Truongdulieu()
                    {
                        DM016201 = obj.DM016201,
                        DM016204 = obj.DM016204,
                        DM016205 = obj.DM016205,
                        DM016206 = obj.DM016206,
                        DM016207 = obj.DM016207,
                        DM016208 = (int)obj.DM016208,
                        DM016209 = obj.DM016209,
                        DM016210 = obj.DM016210,
                        DM016211 = obj.DM016211,
                        DM016212 = obj.DM016212,
                        DM016213 = obj.DM016213,
                        DM016214 = obj.DM016214,
                        DM016215 = obj.DM016215,
                        DM016217 = obj.DM016217,
                        DM016218 = obj.DM016218,
                        DM016219 = obj.DM016219,
                        DM016220 = obj.DM016220,
                        IsChecked = false,
                        Cachnhap = string.Empty,
                        Kieutruong =
                            obj.DM016207.Trim() == "1" ? "Text" :
                            obj.DM016207.Trim() == "2" ? "Number" :
                            obj.DM016207.Trim() == "3" ? "Yes/No" :
                            obj.DM016207.Trim() == "4" ? "Date" :
                            obj.DM016207.Trim() == "5" ? "Datetime" :
                            obj.DM016207.Trim() == "6" ? "Time" :
                            obj.DM016207.Trim() == "7" ? "Memo" :
                            obj.DM016207.Trim() == "8" ? "Lookup" :
                            obj.DM016207.Trim() == "9" ? "Tab" :
                            obj.DM016207.Trim() == "10" ? "Image" :
                            "NONE",
                        NguoiCapnhat = "Nguyễn văn XXX",
                        NguoiTao = "Nguyễn văn XXX"
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public int LoaiThutucNhiemvu_Truongdulieu_Create(DM_LoaiThutucNhiemvu_Truongdulieu obj)
        {
            try
            {
                DBDM0162 checkObj = db.DBDM0162s.FirstOrDefault(o => !o.DM016221 && o.DM016204 == obj.DM016204);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0162s.FirstOrDefault(o => !o.DM016221 && o.DM016205 == obj.DM016205);
                if (checkObj != null)
                    return 2;//Trung Ten

                db.DBDM0162s.InsertOnSubmit(new DBDM0162()
                {
                    DM016201 = obj.DM016201,
                    DM016204 = obj.DM016204,
                    DM016205 = obj.DM016205,
                    DM016206 = obj.DM016206,
                    DM016207 = obj.DM016207.Trim(),
                    DM016208 = obj.DM016208,
                    DM016209 = obj.DM016209,
                    DM016210 = obj.DM016210,
                    DM016211 = obj.DM016211,
                    DM016212 = obj.DM016212,
                    DM016213 = obj.DM016213,
                    DM016214 = obj.DM016214,
                    DM016215 = obj.DM016215,
                    DM016217 = obj.DM016217,
                    DM016218 = obj.DM016218,
                    DM016219 = obj.DM016219,
                    DM016220 = obj.DM016220,
                    DM016221 = false,
                });
                db.SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Truongdulieu_Update(DM_LoaiThutucNhiemvu_Truongdulieu obj)
        {
            try
            {
                DBDM0162 updateObj = db.DBDM0162s.FirstOrDefault(o => o.DM016201 == obj.DM016201);
                if (updateObj == null) return -1;

                DBDM0162 checkObj = db.DBDM0162s.FirstOrDefault(o => !o.DM016221 && o.DM016201 != obj.DM016201 && o.DM016204 == obj.DM016204);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0162s.FirstOrDefault(o => !o.DM016221 && o.DM016201 != obj.DM016201 && o.DM016205 == obj.DM016205);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016204 = obj.DM016204;
                updateObj.DM016205 = obj.DM016205;
                updateObj.DM016206 = obj.DM016206;
                updateObj.DM016207 = obj.DM016207;
                updateObj.DM016208 = obj.DM016208;
                updateObj.DM016209 = obj.DM016209;
                updateObj.DM016210 = obj.DM016210;
                updateObj.DM016211 = obj.DM016211;
                updateObj.DM016212 = obj.DM016212;
                updateObj.DM016213 = obj.DM016213;
                updateObj.DM016214 = obj.DM016214;
                updateObj.DM016215 = obj.DM016215;
                updateObj.DM016219 = obj.DM016219;
                updateObj.DM016220 = obj.DM016220;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Truongdulieu_Delete(Guid id)
        {
            try
            {
                DBDM0162 obj = db.DBDM0162s.FirstOrDefault(o => o.DM016201 == id);
                if (obj == null) return -1;

                obj.DM016221 = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        #endregion

        #region DB Master

        public List<string> DBMaster_GetTables(string prefix)
        {
            try
            {
                var list = db.Mapping.GetTables();
                if (list == null || list.Count() == 0) return null;

                List<string> result = new List<string>();
                foreach (MetaTable obj in list)
                {
                    if (obj.RowType.Name.IndexOf(prefix) == 0)
                        result.Add(obj.RowType.Name);
                }

                return result.Count == 0 ? null : result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public List<string> DBMaster_GetTableColumns(string tableName)
        {
            try
            {
                var list = db.Mapping.GetTables();
                if (list == null || list.Count() == 0) return null;

                MetaTable table = list.FirstOrDefault(o => o.RowType.Name.Trim() == tableName.Trim());
                if (table == null) return null;

                List<string> result = new List<string>();
                foreach (MetaDataMember column in table.RowType.DataMembers)
                {
                    if (column.Association == null)
                        result.Add(column.Name);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        #endregion
    }
}