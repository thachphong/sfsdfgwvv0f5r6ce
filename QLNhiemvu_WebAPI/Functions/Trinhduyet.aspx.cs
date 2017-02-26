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
    public partial class Trinhduyet : BasePage
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
                    case "getlist_kyhieuvb":
                        GetList_KyhieuVB();
                        break;
                    case "getlist_capbanhanh":
                        GetList_Capbanhanh();
                        break;
                    case "getlist_nhansu":
                        GetList_Nhansu();
                        break;
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
                    case "getlist_for_phancong":
                        GetList_For_Phancong();
                        break;
                    case "getlist_for_thamdinh":
                        GetList_For_Thamdinh();
                        break;
                    case "getlist_for_pheduyetthamdinh":
                        GetList_For_Thamdinh();
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
                    case "create_list":
                        CreateList();
                        break;
                    case "update_list":
                        UpdateList();
                        break;
                    case "delete":
                        Delete();
                        break;
                }
            }
        }

        private void UpdateList()
        {
            List<TD_ThuchienNhiemvu> list = JsonConvert.DeserializeObject<List<TD_ThuchienNhiemvu>>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_ThuchienNhiemvu_Update(list);
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

        private void CreateList()
        {
            List<TD_ThuchienNhiemvu> list = JsonConvert.DeserializeObject<List<TD_ThuchienNhiemvu>>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                int resultCode = dataTools.TD_ThuchienNhiemvu_Create(list);
                if (resultCode == 0)
                    DoResponse(new APIResponseData()
                    {
                        ErrorCode = 0,
                        Message = "Success",
                        Data = null,
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

        private void GetList_KyhieuVB()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<Kyhieuvanban> result = dataTools.KyhieuVB_Gets();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_Capbanhanh()
        {
            Guid donviID = Guid.Parse(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_Capbanhanh> result = dataTools.TD_Capbanhanh_GetList(donviID);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_For_PheduyetThamdinh()
        {
            TD_Pheduyet_Thamdinh_Filter filter = JsonConvert.DeserializeObject<TD_Pheduyet_Thamdinh_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu> result = dataTools.TD_ThuchienNhiemvu_GetListFor_PheduyetThamdinh(filter);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_For_Thamdinh()
        {
            TD_Thamdinh_Filter filter = JsonConvert.DeserializeObject<TD_Thamdinh_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu> result = dataTools.TD_ThuchienNhiemvu_GetListFor_Thamdinh(filter);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_For_Phancong()
        {
            TD_Phancong_Filter filter = JsonConvert.DeserializeObject<TD_Phancong_Filter>(currentData.Data.ToString());
            using (DataTools dataTools = new DataTools())
            {
                List<TD_ThuchienNhiemvu> result = dataTools.TD_ThuchienNhiemvu_GetListFor_Phancong(filter.Nam, filter.Phamviphancong, filter.Donviphancong, filter.Trangthai, filter.Thamquyenphancong, filter.Nguoinhanvanbanden, filter.Nguoiphancong);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_Nhansu()
        {
            List<string> ids = JsonConvert.DeserializeObject<List<string>>(currentData.Data.ToString());
            Guid ngSudungId = Guid.Parse(ids[0]);
            char phamviId = char.Parse(ids[1]);
            char thamquyenId = char.Parse(ids[2]);

            using (DataTools dataTools = new DataTools())
            {
                List<TD_Nguoiky> result = dataTools.TD_Nguoiky_GetList(ngSudungId, phamviId, thamquyenId);

                DoResponse(new APIResponseData()
                {
                    ErrorCode = 0,
                    Message = "Success",
                    Data = result == null ? null : JsonConvert.SerializeObject(result)
                });
            }
        }

        private void GetList_TrangthaiHoSo()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<TD_TrangthaiHoSo> result = dataTools.TD_Trangthai_GetList();

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
                List<TD_DonviQuanly> result = dataTools.TD_DonviQuanly_GetList();

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
                        Data = JsonConvert.SerializeObject(dataTools.TD_ThuchienNhiemvu_Get(obj.DM016701))
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
                        Data = JsonConvert.SerializeObject(dataTools.TD_ThuchienNhiemvu_Get(obj.DM016701))
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
                List<TD_ThuchienNhiemvu> result = dataTools.TD_ThuchienNhiemvu_GetList(filter);

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