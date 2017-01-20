using Newtonsoft.Json;
using QLNhiemVu;
using QLNhiemvu_DBEntities;
using QLNhiemvu_WebAPI.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLNhiemvu_WebAPI.Functions
{
    public partial class LoaiThutuc_Truongdulieu : BasePage
    {
        private static APIRequestData currentData = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            int validCode = ValidateRequest();
            if (validCode != 0)
            {
                DoResponse(new APIResponseData()
                {
                    ErrorCode = validCode,
                    Message = "Request denied! Authentication was not allowed!",
                    Data = null
                });
            }

            try
            {
                //byte[] bytes = Encoding.Default.GetBytes(jsonData);
                StreamReader requestSteam = new StreamReader(Request.InputStream);
                string strRequestData = requestSteam.ReadToEnd();
                currentData = JsonConvert.DeserializeObject<APIRequestData>(strRequestData);
            }
            catch (Exception ex)
            {
                Log.write(ex);
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 1,
                    Message = "Missing request data!",
                    Data = null
                });
            }

            if (!string.IsNullOrEmpty(currentData.Action))
            {
                switch (currentData.Action.ToLower())
                {
                    case "getlist_root":
                        GetList_Root();
                        break;
                    case "getlist":
                        GetList();
                        break;
                    case "getlistcanchildren":
                        GetListCanChildren();
                        break;
                    case "create":
                        Create();
                        break;
                    case "update":
                        Update();
                        break;
                    case "delete":
                        Delete();
                        break;
                    case "getlisttables":
                        GetListTables();
                        break;
                    case "getlisttablecolumns":
                        GetListTableColumns();
                        break;
                }
            }
        }

        private void GetListCanChildren()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<DM_LoaiThutucNhiemvu_Truongdulieu> result = dataTools.LoaiThutucNhiemvu_Truongdulieu_GetList(Guid.Empty, true);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void Delete()
        {
            List<Guid> list = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            int total = 0;
            using (DataTools dataTools = new DataTools())
            {
                foreach (Guid id in list)
                {
                    int resultCode = dataTools.LoaiThutucNhiemvu_Truongdulieu_Delete(id);
                    if (resultCode == 0) total++;
                }

                if (total == list.Count)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = null
                    });
                else if (total > 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 1,
                        Message = "Còn tồn tại một số Trường dữ liệu chưa thực hiện được việc xóa!",
                        Data = null
                    });
                else
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = -1,
                        Message = "Có lỗi xảy ra trong quá trình xóa dữ liệu!",
                        Data = null
                    });
            }
        }

        private void Update()
        {
            DM_LoaiThutucNhiemvu_Truongdulieu obj = JsonConvert.DeserializeObject<DM_LoaiThutucNhiemvu_Truongdulieu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Truongdulieu_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Truongdulieu_Get(obj.DM016201))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Trường dữ liệu này!" :
                            resultCode == 1 ? "Trùng mã số!" :
                            resultCode == 2 ? "Trùng tên!" :
                            "Unknown",
                        Data = null
                    });
                }
            }
        }

        private void Create()
        {
            DM_LoaiThutucNhiemvu_Truongdulieu obj = JsonConvert.DeserializeObject<DM_LoaiThutucNhiemvu_Truongdulieu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Truongdulieu_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Truongdulieu_Get(obj.DM016201))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == 1 ? "Trùng mã số!" :
                            resultCode == 2 ? "Trùng tên!" :
                            "Unknow",
                        Data = null
                    });
                }
            }
        }

        private void GetList_Root()
        {
            Guid noidungId = Guid.Parse(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<DM_LoaiThutucNhiemvu_Truongdulieu> result = dataTools.LoaiThutucNhiemvu_Truongdulieu_GetList_OneLevel(noidungId, Guid.Empty);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<DM_LoaiThutucNhiemvu_Truongdulieu> result = dataTools.LoaiThutucNhiemvu_Truongdulieu_GetList(Guid.Empty, false);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetListTables()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<string> result = dataTools.DBMaster_GetTables("DB");

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetListTableColumns()
        {
            string tableName = currentData.Data.ToString();

            using (DataTools dataTools = new DataTools())
            {
                List<string> result = dataTools.DBMaster_GetTableColumns(tableName, "DM");

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }
    }
}