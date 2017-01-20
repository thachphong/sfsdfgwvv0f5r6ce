using Newtonsoft.Json;
using QLNhiemVu;
using QLNhiemvu_DBEntities;
using QLNhiemvu_WebAPI.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLNhiemvu_WebAPI.Functions
{
    public partial class LoaiThutuc : BasePage
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
                    case "get_bymaso":
                        Get_ByMaso();
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
                    int resultCode = dataTools.LoaiThutucNhiemvu_Delete(id);
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
                        Message = "Còn tồn tại một số Loại thủ tục chưa thực hiện được việc xóa!",
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
            DM_LoaiThutucNhiemvu obj = JsonConvert.DeserializeObject<DM_LoaiThutucNhiemvu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Update(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Get(obj.DM016001))
                    });
                else
                {
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = resultCode,
                        Message =
                            resultCode == -1 ? "Không tìm được Loại thủ tục nhiệm vụ này!" :
                            resultCode == 1 ? "Trùng mã số!" :
                            resultCode == 2 ? "Trùng tên!" :
                            "Unknow",
                        Data = null
                    });
                }
            }
        }

        private void Create()
        {
            DM_LoaiThutucNhiemvu obj = JsonConvert.DeserializeObject<DM_LoaiThutucNhiemvu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.LoaiThutucNhiemvu_Create(obj);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = JsonConvert.SerializeObject(dataTools.LoaiThutucNhiemvu_Get(obj.DM016001))
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
                List<DM_LoaiThutucNhiemvu> result = dataTools.LoaiThutucNhiemvu_GetList();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void Get_ByMaso()
        {
            string maso = currentData.Data.ToString();
            using (DataTools dataTools = new DataTools())
            {
                DM_LoaiThutucNhiemvu obj = dataTools.LoaiThutucNhiemvu_Get(maso);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = obj == null ? null : JsonConvert.SerializeObject(obj)
                });
            }
        }
    }
}