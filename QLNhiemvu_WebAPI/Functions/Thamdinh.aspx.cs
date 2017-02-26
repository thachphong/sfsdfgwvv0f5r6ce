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
    public partial class Thamdinh : BasePage
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
                    case "getlist_truongdulieu":
                        GetList_Truongdulieu();
                        break;
                    case "get_duyetthamdinh":
                        Get_DuyetThamdinh();
                        break;
                    case "getlist_duyet":
                        GetList_Duyet();
                        break;
                    case "create_duyet":
                        Create_Duyet();
                        break;
                    case "update_duyet":
                        Update_Duyet();
                        break;
                    case "delete_duyet":
                        Delete_Duyet();
                        break;
                    case "create_duyet_list":
                        Create_Duyet_List();
                        break;
                    case "update_duyet_list":
                        Update_Duyet_List();
                        break;
                }
            }
        }

        private void Get_DuyetThamdinh()
        {
            TD_Thamdinh_Duyet_FilterOne filter = JsonConvert.DeserializeObject<TD_Thamdinh_Duyet_FilterOne>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                TD_Thamdinh_Duyet obj = dataTools.TD_Thamdinh_Duyet_Get(filter);
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = JsonConvert.SerializeObject(obj)
                });
            }
        }

        private void GetList_Truongdulieu()
        {
            List<Guid> ids = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            Guid noidungId = ids[0];
            Guid tdDuyetId = ids[1];
            Guid tdNhiemvuId = ids[2];

            using (DataTools dataTools = new DataTools())
            {
                List<TD_Thamdinh_Duyet_Truongdulieu> result = dataTools.TD_Thamdinh_Duyet_Truongdulieu_GetList(noidungId, tdDuyetId, tdNhiemvuId);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void Delete_Duyet()
        {
            List<Guid> list = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            int total = 0;
            using (DataTools dataTools = new DataTools())
            {
                foreach (Guid id in list)
                {
                    int resultCode = dataTools.TD_Thamdinh_Duyet_Delete(id);
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
                        Message = "Còn tồn tại một số Nhiệm vụ chưa thực hiện được việc xóa!",
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

        private void Update_Duyet()
        {
            TD_Thamdinh_Duyet obj = JsonConvert.DeserializeObject<TD_Thamdinh_Duyet>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Thamdinh_Duyet_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_ThuchienNhiemvu_Get(obj.DM017101))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Nhiệm vụ này!" :
                            resultCode == 1 ? "Trùng Số văn bản!" :
                            "Unknown",
                        Data = null
                    });
                }
            }
        }

        private void Create_Duyet()
        {
            TD_Thamdinh_Duyet obj = JsonConvert.DeserializeObject<TD_Thamdinh_Duyet>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Thamdinh_Duyet_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.TD_ThuchienNhiemvu_Get(obj.DM017101))
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

        private void GetList_Duyet()
        {
            TD_Thamdinh_Duyet_Filter filter = JsonConvert.DeserializeObject<TD_Thamdinh_Duyet_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_Thamdinh_Duyet> result = dataTools.TD_Thamdinh_Duyet_GetList(filter);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void Update_Duyet_List()
        {
            List<TD_Thamdinh_Duyet> obj = JsonConvert.DeserializeObject<List<TD_Thamdinh_Duyet>>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Thamdinh_Duyet_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = null
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Nhiệm vụ này!" :
                            resultCode == 1 ? "Trùng Số văn bản!" :
                            "Unknown",
                        Data = null
                    });
                }
            }
        }

        private void Create_Duyet_List()
        {
            List<TD_Thamdinh_Duyet> obj = JsonConvert.DeserializeObject<List<TD_Thamdinh_Duyet>>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_Thamdinh_Duyet_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = null
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
    }
}