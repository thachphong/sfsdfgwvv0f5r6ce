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
    public partial class LoaiThutuc_Huongdan : BasePage
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
                }
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
                    int resultCode = dataTools.LoaiThutucNhiemvu_Huongdan_Delete(id);
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
                        Message = "Còn tồn tại một số Hướng dẫn chưa thực hiện được việc xóa!",
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
            DM_Huongdan obj = JsonConvert.DeserializeObject<DM_Huongdan>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Huongdan_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Huongdan_Get(obj.DM016301))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Hướng dẫn này!" :
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
            DM_Huongdan obj = JsonConvert.DeserializeObject<DM_Huongdan>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Huongdan_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Huongdan_Get(obj.DM016301))
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

        private void GetList()
        {
            using (DataTools dataTools = new DataTools())
            {
                Guid id = Guid.Empty;
                if (currentData.Data != null)
                {
                    id = Guid.Parse(currentData.Data.ToString());
                }

                List<DM_Huongdan> result = dataTools.LoaiThutucNhiemvu_Huongdan_GetList(id);

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