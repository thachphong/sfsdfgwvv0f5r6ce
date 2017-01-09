using Newtonsoft.Json;
using QLNhiemVu;
using QLNhiemvu_DBEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
                var list = db.DBDM0160s.Where(o => !o.IsDeleted);
                if (list.Count() == 0) return null;

                List<DM_LoaiThutucNhiemvu> result = new List<DM_LoaiThutucNhiemvu>();
                foreach (DBDM0160 obj in list.OrderByDescending(o => o.DM016007))
                {
                    List<Guid> fieldSelecteds = new List<Guid>();
                    var fields = obj.DBDM0164s.Where(o => !o.IsDeleted);
                    if (fields.Count() > 0)
                    {
                        foreach (DBDM0164 field in fields)
                            fieldSelecteds.Add(field.DM016403);
                    }

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
                        NguoiTao = "Nguyễn văn XXX",
                        FieldSelecteds = fieldSelecteds.Count == 0 ? null : fieldSelecteds
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
                DBDM0160 checkObj = db.DBDM0160s.FirstOrDefault(o => !o.IsDeleted && o.DM016003 == obj.DM016003);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0160s.FirstOrDefault(o => !o.IsDeleted && o.DM016004 == obj.DM016004);
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
                    IsDeleted = false,
                });
                db.SubmitChanges();

                if (obj.DM016010 == '1')
                {
                    LoaiThutucNhiemvu_UpdateFieldSelecteds(obj);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_UpdateFieldSelecteds(DM_LoaiThutucNhiemvu objThutuc)
        {
            try
            {
                if (objThutuc.FieldSelecteds == null || objThutuc.FieldSelecteds.Count == 0) return 1;
                DBDM0160 obj = db.DBDM0160s.FirstOrDefault(o => o.DM016001 == objThutuc.DM016001);
                if (obj == null) return 2;

                foreach (DBDM0164 child in obj.DBDM0164s)
                {
                    child.IsDeleted = true;
                    db.SubmitChanges();
                }

                foreach (Guid id in objThutuc.FieldSelecteds)
                {
                    DBDM0164 check = obj.DBDM0164s.FirstOrDefault(o => o.DM016403 == id);
                    if (check == null) //Chưa có
                    {
                        db.DBDM0164s.InsertOnSubmit(new DBDM0164()
                        {
                            DM016401 = Guid.NewGuid(),
                            DM016402 = objThutuc.DM016001,
                            DM016403 = id,
                            DM016404 = objThutuc.DM016006,
                            DM016405 = DateTime.Now,
                            DM016406 = objThutuc.DM016006,
                            DM016407 = DateTime.Now,
                            IsDeleted = false,
                        });
                        db.SubmitChanges();
                    }
                    else //Đã có
                    {
                        if (check.IsDeleted) //Đã xóa
                        {
                            check.IsDeleted = false;
                            check.DM016406 = objThutuc.DM016006;
                            check.DM016407 = DateTime.Now;
                            db.SubmitChanges();
                        }
                        else
                        {
                            check.DM016406 = objThutuc.DM016006;
                            check.DM016407 = DateTime.Now;
                            db.SubmitChanges();
                        }
                    }
                }

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

                DBDM0160 checkObj = db.DBDM0160s.FirstOrDefault(o => !o.IsDeleted && o.DM016001 != obj.DM016001 && o.DM016003 == obj.DM016003);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0160s.FirstOrDefault(o => !o.IsDeleted && o.DM016001 != obj.DM016001 && o.DM016004 == obj.DM016004);
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

                if (obj.DM016010 == '1')
                {
                    LoaiThutucNhiemvu_UpdateFieldSelecteds(obj);
                }

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

                obj.IsDeleted = true;
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

        public List<DM_Huongdan> LoaiThutucNhiemvu_Huongdan_GetList(Guid thutucId)
        {
            try
            {
                var list =
                    thutucId == Guid.Empty ? db.DBDM0163s.Where(o => !o.IsDeleted) :
                    db.DBDM0163s.Where(o => !o.IsDeleted && o.DM016302 == thutucId);

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
                        DM016310 = obj.DM016310,
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
                DBDM0163 checkObj = db.DBDM0163s.FirstOrDefault(o => !o.IsDeleted && o.DM016303 == obj.DM016303);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0163s.FirstOrDefault(o => !o.IsDeleted && o.DM016304 == obj.DM016304);
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
                    DM016310 = obj.DM016310,
                    IsDeleted = false
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

                DBDM0163 checkObj = db.DBDM0163s.FirstOrDefault(o => !o.IsDeleted && o.DM016301 != obj.DM016301 && o.DM016303 == obj.DM016303);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0163s.FirstOrDefault(o => !o.IsDeleted && o.DM016301 != obj.DM016301 && o.DM016304 == obj.DM016304);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016302 = obj.DM016302;
                updateObj.DM016303 = obj.DM016303;
                updateObj.DM016304 = obj.DM016304;
                updateObj.DM016305 = obj.DM016305;
                updateObj.DM016308 = obj.DM016308;
                updateObj.DM016309 = obj.DM016309;
                updateObj.DM016310 = obj.DM016310;
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

                obj.IsDeleted = true;
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
                var list = db.DBDM0161s.Where(o => !o.IsDeleted && o.DM016102 == thutucID);
                if (list.Count() == 0) return null;

                List<DM_LoaiThutucNhiemvu_Noidung> result = new List<DM_LoaiThutucNhiemvu_Noidung>();
                foreach (DBDM0161 obj in list.OrderByDescending(o => o.DM016107))
                {
                    List<Guid> fieldSelecteds = new List<Guid>();
                    var fields = obj.DBDM0165s.Where(o => !o.IsDeleted);
                    if (fields.Count() > 0)
                    {
                        foreach (DBDM0165 field in fields)
                            fieldSelecteds.Add(field.DM016503);
                    }

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
                        NguoiTao = "Nguyễn văn XXX",
                        FieldSelecteds = fieldSelecteds.Count == 0 ? null : fieldSelecteds
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
                DBDM0161 checkObj = db.DBDM0161s.FirstOrDefault(o => !o.IsDeleted && o.DM016103 == obj.DM016103);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0161s.FirstOrDefault(o => !o.IsDeleted && o.DM016104 == obj.DM016104);
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
                    IsDeleted = false,
                });
                db.SubmitChanges();

                if (obj.DM016105 == '2')
                    LoaiThutucNhiemvu_Noidung_UpdateFieldSelecteds(obj);

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

                DBDM0161 checkObj = db.DBDM0161s.FirstOrDefault(o => !o.IsDeleted && o.DM016101 != obj.DM016101 && o.DM016103 == obj.DM016103);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0161s.FirstOrDefault(o => !o.IsDeleted && o.DM016101 != obj.DM016101 && o.DM016104 == obj.DM016104);
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

                if (obj.DM016105 == '2')
                    LoaiThutucNhiemvu_Noidung_UpdateFieldSelecteds(obj);

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

                obj.IsDeleted = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int LoaiThutucNhiemvu_Noidung_UpdateFieldSelecteds(DM_LoaiThutucNhiemvu_Noidung objNoidung)
        {
            try
            {
                if (objNoidung.FieldSelecteds == null || objNoidung.FieldSelecteds.Count == 0) return 1;
                DBDM0161 obj = db.DBDM0161s.FirstOrDefault(o => o.DM016101 == objNoidung.DM016101);
                if (obj == null) return 2;

                foreach (DBDM0165 child in obj.DBDM0165s)
                {
                    child.IsDeleted = true;
                    db.SubmitChanges();
                }

                foreach (Guid id in objNoidung.FieldSelecteds)
                {
                    DBDM0165 check = obj.DBDM0165s.FirstOrDefault(o => o.DM016503 == id);
                    if (check == null) //Chưa có
                    {
                        db.DBDM0165s.InsertOnSubmit(new DBDM0165()
                        {
                            DM016501 = Guid.NewGuid(),
                            DM016502 = objNoidung.DM016101,
                            DM016503 = id,
                            DM016504 = objNoidung.DM016108,
                            DM016505 = DateTime.Now,
                            DM016506 = objNoidung.DM016108,
                            DM016507 = DateTime.Now,
                            IsDeleted = false,
                        });
                        db.SubmitChanges();
                    }
                    else //Đã có
                    {
                        if (check.IsDeleted) //Đã xóa
                        {
                            check.IsDeleted = false;
                            check.DM016506 = objNoidung.DM016108;
                            check.DM016507 = DateTime.Now;
                            db.SubmitChanges();
                        }
                        else
                        {
                            check.DM016506 = objNoidung.DM016108;
                            check.DM016507 = DateTime.Now;
                            db.SubmitChanges();
                        }
                    }
                }

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

        private string GenerateChildPrefix(int level)
        {
            string prefix = string.Empty;

            for (int i = 0; i < level - 1; i++)
                prefix += "--";

            return prefix;
        }

        public List<DM_LoaiThutucNhiemvu_Truongdulieu> LoaiThutucNhiemvu_Truongdulieu_GetList(Guid parentId, bool onlyCanChildren, int level = 1)
        {
            try
            {
                List<DM_LoaiThutucNhiemvu_Truongdulieu> result = new List<DM_LoaiThutucNhiemvu_Truongdulieu>();
                var list = db.DBDM0162s.Where(o => !o.IsDeleted && o.DM016216 == parentId && (onlyCanChildren ? o.DM016207.Trim() != "9" : true));

                foreach (DBDM0162 obj in list.OrderByDescending(o => o.DM016214))
                {
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
                        DM016213 = obj.DM016213,
                        DM016214 = obj.DM016214,
                        DM016215 = obj.DM016215,
                        DM016216 = obj.DM016216,
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
                        NguoiTao = "Nguyễn văn XXX",
                        Level = level,
                        Maso = GenerateChildPrefix(level) + " " + obj.DM016204,
                        Tentruong = GenerateChildPrefix(level) + " " + obj.DM016206
                    });

                    List<DM_LoaiThutucNhiemvu_Truongdulieu> children = LoaiThutucNhiemvu_Truongdulieu_GetList(obj.DM016201, onlyCanChildren, level + 1);
                    if (children != null)
                        result.AddRange(children);
                }

                return result.Count == 0 ? null : result;
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
                DBDM0162 checkObj = db.DBDM0162s.FirstOrDefault(o => !o.IsDeleted && o.DM016204 == obj.DM016204);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0162s.FirstOrDefault(o => !o.IsDeleted && o.DM016205 == obj.DM016205);
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
                    DM016213 = obj.DM016213,
                    DM016214 = obj.DM016214,
                    DM016215 = obj.DM016215,
                    DM016217 = obj.DM016217,
                    DM016218 = obj.DM016218,
                    DM016219 = obj.DM016219,
                    DM016220 = obj.DM016220,
                    DM016216 = obj.DM016216,
                    IsDeleted = false,
                });
                db.SubmitChanges();

                //if (obj.DM016207.Trim() == "9")
                //    LoaiThutucNhiemvu_Truongdulieu_UpdateChildren(obj);

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        //public bool LoaiThutucNhiemvu_Truongdulieu_UpdateChildren(DM_LoaiThutucNhiemvu_Truongdulieu obj)
        //{
        //    try
        //    {
        //        List<Guid> data = JsonConvert.DeserializeObject<List<Guid>>(obj.DM016210);
        //        if (data == null || data.Count == 0)
        //            return false;

        //        DBDM0162 parent = db.DBDM0162s.FirstOrDefault(o => o.DM016201 == obj.DM016201);
        //        if (parent == null) return false;

        //        foreach (DBDM0166 child in parent.DBDM0166s)
        //        {
        //            child.IsDeleted = true;
        //            db.SubmitChanges();
        //        }

        //        foreach (Guid id in data)
        //        {
        //            DBDM0166 check = db.DBDM0166s.FirstOrDefault(o => o.DM016602 == obj.DM016201 && o.DM016603 == id);
        //            if (check == null) //Chưa có
        //            {
        //                db.DBDM0166s.InsertOnSubmit(new DBDM0166()
        //                {
        //                    DM016601 = Guid.NewGuid(),
        //                    DM016602 = obj.DM016201,
        //                    DM016603 = id,
        //                    DM016604 = obj.DM016219,
        //                    DM016605 = DateTime.Now,
        //                    DM016606 = obj.DM016219,
        //                    DM016607 = DateTime.Now,
        //                    IsDeleted = false
        //                });
        //                db.SubmitChanges();
        //            }
        //            else //Đã có
        //            {
        //                if (check.IsDeleted) //Đã xóa
        //                {
        //                    check.IsDeleted = false;
        //                    check.DM016606 = obj.DM016219;
        //                    check.DM016607 = DateTime.Now;
        //                    db.SubmitChanges();
        //                }
        //                else
        //                {
        //                    check.DM016606 = obj.DM016219;
        //                    check.DM016607 = DateTime.Now;
        //                    db.SubmitChanges();
        //                }
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.write(ex);
        //        return false;
        //    }
        //}

        public int LoaiThutucNhiemvu_Truongdulieu_Update(DM_LoaiThutucNhiemvu_Truongdulieu obj)
        {
            try
            {
                DBDM0162 updateObj = db.DBDM0162s.FirstOrDefault(o => o.DM016201 == obj.DM016201);
                if (updateObj == null) return -1;

                DBDM0162 checkObj = db.DBDM0162s.FirstOrDefault(o => !o.IsDeleted && o.DM016201 != obj.DM016201 && o.DM016204 == obj.DM016204);
                if (checkObj != null)
                    return 1;//Trung Ma so

                checkObj = db.DBDM0162s.FirstOrDefault(o => !o.IsDeleted && o.DM016201 != obj.DM016201 && o.DM016205 == obj.DM016205);
                if (checkObj != null)
                    return 2;//Trung Ten

                updateObj.DM016204 = obj.DM016204;
                updateObj.DM016205 = obj.DM016205;
                updateObj.DM016206 = obj.DM016206;
                updateObj.DM016207 = obj.DM016207;
                updateObj.DM016208 = obj.DM016208;
                updateObj.DM016209 = obj.DM016209;
                updateObj.DM016210 = obj.DM016210;
                updateObj.DM016213 = obj.DM016213;
                updateObj.DM016214 = obj.DM016214;
                updateObj.DM016215 = obj.DM016215;
                updateObj.DM016219 = obj.DM016219;
                updateObj.DM016220 = obj.DM016220;
                updateObj.DM016216 = obj.DM016216;
                db.SubmitChanges();

                //if (obj.DM016207.Trim() == "9")
                //    LoaiThutucNhiemvu_Truongdulieu_UpdateChildren(obj);

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

                obj.IsDeleted = true;
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

        public List<string> DBMaster_GetTableColumns(string tableName, string prefix)
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
                    if (column.Association == null && column.Name.IndexOf(prefix) == 0)
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

        public bool DBMaster_CheckConnection()
        {
            try
            {
                db.Connection.Open();
                db.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return false;
            }
        }

        public class DynamicQueryHelper
        {
            public static List<object> ExcuteQuery(DM_LoaiThutucNhiemvu_Truongdulieu_LookupData condition)
            {
                try
                {
                    string sql =
                            "select " +
                                condition.ColumnDisplayID +
                                ", " + condition.ColumnDisplayName +
                                ", " + condition.ColumnSave +
                                (!string.IsNullOrEmpty(condition.ColumnDisplayExtend1) ? (", " + condition.ColumnDisplayExtend1) : string.Empty) +
                                (!string.IsNullOrEmpty(condition.ColumnDisplayExtend2) ? (", " + condition.ColumnDisplayExtend2) : string.Empty) +
                            " from " + condition.Table +
                            " where " +
                                condition.Condition1.ColumnName + " " + condition.Condition1.Condition + " " + condition.Condition1.Value +
                                (condition.Condition2 == null ? string.Empty : (
                                " " + condition.ConditionCombination + " " +
                                condition.Condition2.ColumnName + " " + condition.Condition2.Condition + " " + condition.Condition2.Value));

                    string strcnn = System.Configuration.ConfigurationManager.ConnectionStrings["QLNVConnectionString"].ConnectionString;
                    using (SqlConnection cnn = new SqlConnection(strcnn))
                    {
                        SqlCommand command = new SqlCommand(sql, cnn);
                        cnn.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            string result = "[";
                            int curRow = 0;

                            while (reader.Read())
                            {
                                int col = 0;
                                string strObj = (curRow > 0 ? ", " : string.Empty) + "{";

                                strObj += (col > 0 ? ", " : string.Empty) + condition.ColumnDisplayID + ":\"" + reader.GetValue(col) + "\""; col++;
                                strObj += (col > 0 ? ", " : string.Empty) + condition.ColumnDisplayName + ":\"" + reader.GetValue(col) + "\""; col++;
                                strObj += (col > 0 ? ", " : string.Empty) + condition.ColumnSave + ":\"" + reader.GetValue(col) + "\""; col++;
                                if (!string.IsNullOrEmpty(condition.ColumnDisplayExtend1))
                                {
                                    strObj += (col > 0 ? ", " : string.Empty) + condition.ColumnDisplayExtend1 + ":\"" + reader.GetValue(col) + "\"";
                                    col++;
                                }
                                if (!string.IsNullOrEmpty(condition.ColumnDisplayExtend2))
                                {
                                    strObj += (col > 0 ? ", " : string.Empty) + condition.ColumnDisplayExtend2 + ":\"" + reader.GetValue(col) + "\"";
                                    col++;
                                }

                                strObj += "}";

                                result += strObj;
                                curRow++;
                            }

                            result += "]";

                            return JsonConvert.DeserializeObject<List<object>>(result);
                        }
                        else return null;
                    }
                }
                catch (Exception ex)
                {
                    Log.write(ex);
                    return null;
                }
            }
        }

        #endregion

        #region TD_DanhmucNhiemvu

        public List<TD_ThuchienNhiemvu> TD_ThuchienNhiemvu_GetList(int year, string sovanban, DateTime dateStart, DateTime dateEnd, Guid phanloaiId, Guid danhmucId)
        {
            try
            {
                var list = db.DBDM0167s.Where(o => !o.IsDeleted);
                if (year != 0) list = list.Where(o => o.DM016707.Year == year);
                if (!string.IsNullOrEmpty(sovanban)) list = list.Where(o => o.DM016706.ToLower().IndexOf(sovanban.ToLower()) >= 0);
                if (dateStart != DateTime.MinValue) list = list.Where(o => o.DM016707 >= dateStart);
                if (dateEnd != DateTime.MinValue) list = list.Where(o => o.DM016707 <= dateEnd);
                if (phanloaiId != Guid.Empty) list = list.Where(o => o.DBDM0202.DBDM0140.DM014001 == phanloaiId);
                if (danhmucId != Guid.Empty) list = list.Where(o => o.DBDM0202.DM020201 == danhmucId);


                List<TD_ThuchienNhiemvu> result = new List<TD_ThuchienNhiemvu>();
                foreach (DBDM0167 obj in list.OrderByDescending(o => o.DM016707).ThenByDescending(o => o.DM016719))
                {
                    result.Add(new TD_ThuchienNhiemvu()
                    {
                        AttachedFiles = string.IsNullOrEmpty(obj.DM016714) ? null : JsonConvert.DeserializeObject<List<TD_ThuchienNhiemvu_Tepdinhkem>>(obj.DM016714),
                        Coquannhan = obj.DBDM0304.DBDM0301.DM030105,
                        Danhmuc = obj.DBDM0202.DM020204,
                        DM016701 = obj.DM016701,
                        DM016702 = obj.DM016702,
                        DM016703 = obj.DM016703,
                        DM016705 = obj.DM016705,
                        DM016706 = obj.DM016706,
                        DM016707 = obj.DM016707,
                        DM016708 = obj.DM016708,
                        DM016710 = obj.DM016710,
                        DM016711 = obj.DM016711,
                        DM016713 = obj.DM016713,
                        DM016714 = obj.DM016714,
                        DM016715 = obj.DM016715,
                        DM016716 = obj.DM016716,
                        DM016717 = obj.DM016717,
                        DM016718 = obj.DM016718,
                        DM016719 = obj.DM016719,
                        DM016720 = obj.DM016720,
                        DonviSudung = "Cơ quan X",
                        Fields = TD_ThuchienNhiemvu_Truongdulieu_GetList(obj.DBDM0161.DM016101, obj.DM016701),
                        IsChecked = false,
                        Nguoicapnhat = "Nguyễn văn XXX",
                        Nguoiky = obj.DBDM0304.DM030403,
                        Nguoitao = "Nguyễn văn XXX",
                        NoidungChitiet = obj.DBDM0161.DM016104,
                        PhanloaiNhiemvu = obj.DBDM0202.DBDM0140.DM014003,
                        ThutucNhiemvu = obj.DBDM0161.DBDM0160.DM016004,
                        Trangthai = db.DBDM0124s.FirstOrDefault(t => t.DM012401 == obj.DM016720).DM012403,
                        MaCoquannhan = obj.DBDM0304.DBDM0301.DM030101,
                        MaPhanloaiNhiemvu = obj.DBDM0202.DBDM0140.DM014001,
                        MaThutucNhiemvu = obj.DBDM0161.DBDM0160.DM016001,
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

        public List<TD_ThuchienNhiemvu_TrangthaiHoSo> TD_ThuchienNhiemvu_Trangthai_GetList()
        {
            try
            {
                var list = db.DBDM0124s;
                if (list.Count() == 0) return null;

                List<TD_ThuchienNhiemvu_TrangthaiHoSo> result = new List<TD_ThuchienNhiemvu_TrangthaiHoSo>();
                foreach (DBDM0124 obj in list.OrderBy(o => o.DM012404))
                {
                    result.Add(new TD_ThuchienNhiemvu_TrangthaiHoSo()
                    {
                        DM012401 = obj.DM012401,
                        DM012402 = obj.DM012402,
                        DM012403 = obj.DM012403
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

        private List<TD_ThuchienNhiemvu_Truongdulieu> TD_ThuchienNhiemvu_Truongdulieu_GetListByParentId(Guid noidungId, Guid parentId, Guid tdNhiemvuId)
        {
            try
            {
                var list = db.DBDM0165s.Where(o => o.DM016502 == noidungId && o.DBDM0162.DM016216 == parentId);
                if (list.Count() == 0) return null;

                List<TD_ThuchienNhiemvu_Truongdulieu> result = new List<TD_ThuchienNhiemvu_Truongdulieu>();
                foreach (DBDM0165 field in list.OrderBy(x => x.DBDM0162.DM016214))
                {
                    DBDM0168 tdnvField = field.DBDM0168s.FirstOrDefault(o => o.DM016802 == tdNhiemvuId);

                    TD_ThuchienNhiemvu_Truongdulieu obj = new TD_ThuchienNhiemvu_Truongdulieu()
                    {
                        Batbuocnhap = field.DBDM0162.DM016215 == '1',
                        Children = TD_ThuchienNhiemvu_Truongdulieu_GetListByParentId(noidungId, field.DM016503, tdNhiemvuId),
                        CongcotDulieu = field.DBDM0162.DM016213,
                        DieukienDulieu = field.DBDM0162.DM016210,
                        DM016801 = tdnvField == null ? Guid.NewGuid() : tdnvField.DM016801,
                        DM016802 = tdNhiemvuId,
                        DM016803 = field.DM016501,
                        DM016804 = tdnvField == null ? string.Empty : tdnvField.DM016804,
                        Dorong = (int)field.DBDM0162.DM016208,
                        Kieutruong = int.Parse(field.DBDM0162.DM016207),
                        Maso = field.DBDM0162.DM016204,
                        Sapxep = field.DBDM0162.DM016214,
                        Tenhienthi = field.DBDM0162.DM016206,
                        Tentruong = field.DBDM0162.DM016205,
                        LookupData = null,
                    };

                    if (obj.Kieutruong == 8)
                    {
                        DM_LoaiThutucNhiemvu_Truongdulieu_LookupData condition = JsonConvert.DeserializeObject<DM_LoaiThutucNhiemvu_Truongdulieu_LookupData>(obj.DieukienDulieu);
                        obj.LookupData = DynamicQueryHelper.ExcuteQuery(condition);
                    }

                    result.Add(obj);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public List<TD_ThuchienNhiemvu_Truongdulieu> TD_ThuchienNhiemvu_Truongdulieu_GetList(Guid noidungId, Guid tdNhiemvuId)
        {
            try
            {
                DBDM0161 noidung = db.DBDM0161s.FirstOrDefault(o => o.DM016101 == noidungId);
                if (noidung == null) return null;

                return TD_ThuchienNhiemvu_Truongdulieu_GetListByParentId(noidungId, Guid.Empty, tdNhiemvuId);
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return null;
            }
        }

        public List<TD_ThuchienNhiemvu_DonviQuanly> TD_ThuchienNhiemvu_DonviQuanly_GetList()
        {
            try
            {
                var list = db.DBDM0301s;
                if (list.Count() == 0) return null;

                List<TD_ThuchienNhiemvu_DonviQuanly> result = new List<TD_ThuchienNhiemvu_DonviQuanly>();
                foreach (DBDM0301 obj in list.OrderByDescending(o => o.DM030104))
                {
                    var listNK = obj.DBDM0304s.Count == 0 ? null : obj.DBDM0304s.OrderBy(t => t.DM030402).ToList();
                    List<TD_ThuchienNhiemvu_Nguoiky> dsnk = null;
                    if (listNK != null)
                    {
                        dsnk = new List<TD_ThuchienNhiemvu_Nguoiky>();

                        foreach (DBDM0304 nk in listNK)
                        {
                            DBDM0305 chucvu = nk.DBDM0305s.FirstOrDefault(t => t.DM030506 <= DateTime.Now && t.DM030507 >= DateTime.Now);

                            dsnk.Add(new TD_ThuchienNhiemvu_Nguoiky()
                            {
                                DM030401 = nk.DM030401,
                                DM030403 = nk.DM030403,
                                Chucvu = chucvu == null ? string.Empty : chucvu.DBDM0122.DM012203
                            });
                        }
                    }

                    result.Add(new TD_ThuchienNhiemvu_DonviQuanly()
                    {
                        DM030101 = obj.DM030101,
                        DM030105 = obj.DM030105,
                        DSNguoiky = dsnk
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

        public List<TD_ThuchienNhiemvu_PhanloaiNhiemvu> TD_ThuchienNhiemvu_PhanloaiNhiemvu_GetList()
        {
            try
            {
                var list = db.DBDM0140s;
                if (list.Count() == 0) return null;

                List<TD_ThuchienNhiemvu_PhanloaiNhiemvu> result = new List<TD_ThuchienNhiemvu_PhanloaiNhiemvu>();
                foreach (DBDM0140 obj in list.OrderByDescending(o => o.DM014002))
                {
                    var listDM = obj.DBDM0202s.Count == 0 ? null : obj.DBDM0202s.OrderBy(t => t.DM020202).ToList();
                    List<TD_ThuchienNhiemvu_DanhmucNhiemvu> dsdm = null;
                    if (listDM != null)
                    {
                        dsdm = new List<TD_ThuchienNhiemvu_DanhmucNhiemvu>();

                        foreach (DBDM0202 dm in listDM)
                        {
                            dsdm.Add(new TD_ThuchienNhiemvu_DanhmucNhiemvu()
                                {
                                    DM020201 = dm.DM020201,
                                    DM020204 = dm.DM020204
                                });
                        }
                    }

                    result.Add(new TD_ThuchienNhiemvu_PhanloaiNhiemvu()
                    {
                        DM014001 = obj.DM014001,
                        DM014003 = obj.DM014003,
                        DSDanhmuc = dsdm
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

        public int TD_ThuchienNhiemvu_Create(TD_ThuchienNhiemvu obj)
        {
            try
            {
                DBDM0167 checkObj = db.DBDM0167s.FirstOrDefault(o => !o.IsDeleted && o.DM016706 == obj.DM016706);
                if (checkObj != null)
                    return 1;//Trung Mã văn bản

                db.DBDM0167s.InsertOnSubmit(new DBDM0167()
                {
                    DM016701 = obj.DM016701,
                    DM016702 = obj.DM016702,
                    DM016703 = obj.DM016703,
                    DM016705 = obj.DM016705,
                    DM016706 = obj.DM016706,
                    DM016707 = obj.DM016707,
                    DM016708 = obj.DM016708,
                    DM016710 = obj.DM016710,
                    DM016711 = obj.DM016711,
                    DM016713 = obj.DM016713,
                    DM016714 = obj.DM016714,
                    DM016715 = obj.DM016715,
                    DM016716 = obj.DM016716,
                    DM016717 = obj.DM016717,
                    DM016718 = obj.DM016718,
                    DM016719 = obj.DM016719,
                    DM016720 = obj.DM016720,
                    IsDeleted = false,
                });
                db.SubmitChanges();

                if (obj.Fields != null)
                {
                    TD_ThuchienNhiemvu_UpdateFields(obj.DM016701, obj.Fields);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int TD_ThuchienNhiemvu_Update(TD_ThuchienNhiemvu obj)
        {
            try
            {
                DBDM0167 updateObj = db.DBDM0167s.FirstOrDefault(o => o.DM016701 == obj.DM016701);
                if (updateObj == null) return -1;

                DBDM0167 checkObj = db.DBDM0167s.FirstOrDefault(o => !o.IsDeleted && o.DM016701 != obj.DM016701 && o.DM016706 == obj.DM016706);
                if (checkObj != null)
                    return 1;//Trung Ma so

                updateObj.DM016703 = obj.DM016703;
                updateObj.DM016705 = obj.DM016705;
                updateObj.DM016706 = obj.DM016706;
                updateObj.DM016707 = obj.DM016707;
                updateObj.DM016708 = obj.DM016708;
                updateObj.DM016710 = obj.DM016710;
                updateObj.DM016711 = obj.DM016711;
                updateObj.DM016713 = obj.DM016713;
                updateObj.DM016714 = obj.DM016714;
                updateObj.DM016715 = obj.DM016715;
                updateObj.DM016718 = obj.DM016718;
                updateObj.DM016719 = obj.DM016719;
                updateObj.DM016720 = obj.DM016720;

                db.SubmitChanges();

                if (obj.Fields != null)
                {
                    TD_ThuchienNhiemvu_UpdateFields(obj.DM016701, obj.Fields);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int TD_ThuchienNhiemvu_Delete(Guid id)
        {
            try
            {
                DBDM0167 obj = db.DBDM0167s.FirstOrDefault(o => o.DM016701 == id);
                if (obj == null) return -1;

                obj.IsDeleted = true;
                db.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int TD_ThuchienNhiemvu_UpdateFields(Guid id, List<TD_ThuchienNhiemvu_Truongdulieu> fields)
        {
            try
            {
                foreach (TD_ThuchienNhiemvu_Truongdulieu field in fields)
                {
                    field.DM016802 = id;
                    TD_ThuchienNhiemvu_UpdateField(field);
                    if (field.Children != null && field.Kieutruong == 9)
                        TD_ThuchienNhiemvu_UpdateFields(id, field.Children);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public int TD_ThuchienNhiemvu_UpdateField(TD_ThuchienNhiemvu_Truongdulieu field)
        {
            try
            {
                DBDM0168 check = db.DBDM0168s.FirstOrDefault(o => o.DM016801 == field.DM016801);
                if (check == null)
                {
                    check = new DBDM0168()
                    {
                        DM016801 = field.DM016801,
                        DM016802 = field.DM016802,
                        DM016803 = field.DM016803,
                        DM016804 = field.DM016804
                    };

                    db.DBDM0168s.InsertOnSubmit(check);
                    db.SubmitChanges();
                }
                else
                {
                    check.DM016804 = field.DM016804;
                    db.SubmitChanges();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        #endregion
    }
}