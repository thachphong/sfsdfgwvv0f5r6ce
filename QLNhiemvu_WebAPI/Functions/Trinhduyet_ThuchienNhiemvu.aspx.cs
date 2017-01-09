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
    public partial class Trinhduyet_ThuchienNhiemvu : BasePage
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
                    case "getlist_donviquanly":
                        GetList_DonviQuanly();
                        break;
                    case "getlist_phanloainhiemvu":
                        GetList_PhanloaiNhiemvu();
                        break;
                    case "getlist_truongdulieu":
                        GetList_Truongdulieu();
                        break;
                    case "getlist_trangthaihoso":
                        GetList_TrangthaiHoSo();
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

        private void GetList_TrangthaiHoSo()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu_TrangthaiHoSo> result = dataTools.TD_ThuchienNhiemvu_Trangthai_GetList();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_Truongdulieu()
        {
            List<Guid> ids = JsonConvert.DeserializeObject<List<Guid>>(currentData.Data.ToString());
            Guid noidungId = ids[0];
            Guid tdNhiemvuId = ids[1];

            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu_Truongdulieu> result = dataTools.TD_ThuchienNhiemvu_Truongdulieu_GetList(noidungId, tdNhiemvuId);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_DonviQuanly()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu_DonviQuanly> result = dataTools.TD_ThuchienNhiemvu_DonviQuanly_GetList();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_PhanloaiNhiemvu()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu_PhanloaiNhiemvu> result = dataTools.TD_ThuchienNhiemvu_PhanloaiNhiemvu_GetList();

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
                    int resultCode = dataTools.TD_ThuchienNhiemvu_Delete(id);
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

        private void Update()
        {
            TD_ThuchienNhiemvu obj = JsonConvert.DeserializeObject<TD_ThuchienNhiemvu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_ThuchienNhiemvu_Update(obj);
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

        private void Create()
        {
            TD_ThuchienNhiemvu obj = JsonConvert.DeserializeObject<TD_ThuchienNhiemvu>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_ThuchienNhiemvu_Create(obj);
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

        private void GetList()
        {
            TD_ThuchienNhiemvu_Filter filter = JsonConvert.DeserializeObject<TD_ThuchienNhiemvu_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu> result = dataTools.TD_ThuchienNhiemvu_GetList(filter.Nam, filter.Sovanban, filter.TuNgay, filter.DenNgay, filter.MaPhanloai, filter.MaDanhmuc);

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