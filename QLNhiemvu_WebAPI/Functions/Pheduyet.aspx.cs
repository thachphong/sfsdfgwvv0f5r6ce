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
    public partial class Pheduyet : BasePage
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
                    case "get_currentid":
                        GetCurrentID();
                        break;
                    case "getlist_truongdulieu":
                        GetList_Truongdulieu();
                        break;
                    case "getlist":
                        GetList();
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
                    case "get_currentid_thamdinh":
                        GetCurrentID_Thamdinh();
                        break;
                    case "getlist_truongdulieu_thamdinh":
                        GetList_Truongdulieu_Thamdinh();
                        break;
                    case "getlist_thamdinh":
                        GetList_Thamdinh();
                        break;
                    case "create_thamdinh":
                        Create_Thamdinh();
                        break;
                    case "update_thamdinh":
                        Update_Thamdinh();
                        break;
                    case "delete_thamdinh":
                        Delete_Thamdinh();
                        break;
                }
            }
        }

        #region Thamdinh

        private void GetCurrentID_Thamdinh()
        {
            using (DataTools dataTools = new DataTools())
            {
                byte result = dataTools.TD_Pheduyet_Thamdinh_Duyet_GetCurrentNumberID();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = result == 0 ? 1 : 0,
                    Message = result > 0 ? "Success" : "Can not fetch ID info!",
                    Data = result == 0 ? null : result.ToString()
                });
            }
        }

        private void GetList_Truongdulieu_Thamdinh()
        {
            List<Guid> ids = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            Guid noidungId = ids[0];
            Guid pheduyetId = ids[1];

            using (DataTools dataTools = new DataTools())
            {
                List<TD_Pheduyet_Thamdinh_Duyet_Truongdulieu> result = dataTools.TD_Pheduyet_Thamdinh_Duyet_Truongdulieu_GetList(noidungId, pheduyetId);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void Delete_Thamdinh()
        {
            List<Guid> list = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            int total = 0;
            using (DataTools dataTools = new DataTools())
            {
                foreach (Guid id in list)
                {
                    int resultCode = dataTools.TD_Pheduyet_Thamdinh_Duyet_Delete(id);
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
                        Message = "Còn tồn tại một số Phê duyệt Nhiệm vụ chưa thực hiện được việc xóa!",
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

        private void Update_Thamdinh()
        {
            TD_Pheduyet_Thamdinh_Duyet obj = JsonConvert.DeserializeObject<TD_Pheduyet_Thamdinh_Duyet>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Pheduyet_Thamdinh_Duyet_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_Pheduyet_VB_Get(obj.DM017501))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Phê duyệt Nhiệm vụ này!" :
                            resultCode == 1 ? "Trùng Số văn bản!" :
                            "Unknown",
                        Data = null
                    });
                }
            }
        }

        private void Create_Thamdinh()
        {
            TD_Pheduyet_Thamdinh_Duyet obj = JsonConvert.DeserializeObject<TD_Pheduyet_Thamdinh_Duyet>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Pheduyet_Thamdinh_Duyet_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_Pheduyet_Thamdinh_Duyet_Get(obj.DM017501))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == 1 ? "Trùng Mã văn bản!" :
                            "Unknow",
                        Data = null
                    });
                }
            }
        }

        private void GetList_Thamdinh()
        {
            TD_Pheduyet_Thamdinh_Duyet_Filter filter = JsonConvert.DeserializeObject<TD_Pheduyet_Thamdinh_Duyet_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_Pheduyet_Thamdinh_Duyet> result = dataTools.TD_Pheduyet_Thamdinh_Duyet_GetList(filter);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        #endregion

        private void GetCurrentID()
        {
            using (DataTools dataTools = new DataTools())
            {
                byte result = dataTools.TD_Pheduyet_VB_GetCurrentNumberID();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = result == 0 ? 1 : 0,
                    Message = result > 0 ? "Success" : "Can not fetch ID info!",
                    Data = result == 0 ? null : result.ToString()
                });
            }
        }

        private void GetList_Truongdulieu()
        {
            List<Guid> ids = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            Guid noidungId = ids[0];
            Guid pheduyetId = ids[1];

            using (DataTools dataTools = new DataTools())
            {
                List<TD_Pheduyet_VB_Truongdulieu> result = dataTools.TD_Pheduyet_VB_Truongdulieu_GetList(noidungId, pheduyetId);

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
                    int resultCode = dataTools.TD_Pheduyet_VB_Delete(id);
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
                        Message = "Còn tồn tại một số Phê duyệt Nhiệm vụ chưa thực hiện được việc xóa!",
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
            TD_Pheduyet_VB obj = JsonConvert.DeserializeObject<TD_Pheduyet_VB>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Pheduyet_VB_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_Pheduyet_VB_Get(obj.DM017301))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Phê duyệt Nhiệm vụ này!" :
                            resultCode == 1 ? "Trùng Số văn bản!" :
                            "Unknown",
                        Data = null
                    });
                }
            }
        }

        private void Create()
        {
            TD_Pheduyet_VB obj = JsonConvert.DeserializeObject<TD_Pheduyet_VB>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Pheduyet_VB_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_Pheduyet_VB_Get(obj.DM017301))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == 1 ? "Trùng Mã văn bản!" :
                            "Unknow",
                        Data = null
                    });
                }
            }
        }

        private void GetList()
        {
            TD_Pheduyet_VB_Filter filter = JsonConvert.DeserializeObject<TD_Pheduyet_VB_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_Pheduyet_VB> result = dataTools.TD_Pheduyet_VB_GetList(filter);

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