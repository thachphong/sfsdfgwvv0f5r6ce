using Newtonsoft.Json;
using QLNhiemvu_DBEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLNhiemvu_WebAPI.Functions
{
    public partial class ResourceManager : BasePage
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

            string name = Request.QueryString["n"];
            if (string.IsNullOrEmpty(name))
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 1,
                    Message = "Missing request data!",
                    Data = null
                });

            switch (name.ToLower())
            {
                case "loaithutucnhiemvu_huongdan":
                    LoaithutucNhiemvu_Huongdan();
                    break;
                case "td_thuchiennhiemvu_tepdinhkem":
                    Trinhduyet_Thuchiennhiemvu_Tepdinhkem();
                    break;
                case "td_thamdinh_duyet_tepdinhkem":
                    Trinhduyet_Thamdinh_Duyet_Tepdinhkem();
                    break;
            }
        }

        private void Trinhduyet_Thamdinh_Duyet_Tepdinhkem()
        {
            string fn = Request.QueryString["fn"];
            if (string.IsNullOrEmpty(fn))
                fn = DateTime.Now.Ticks.ToString();

            if (Request.Files.Count == 0)
            {
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 1,
                    Message = "Missing request data!",
                    Data = null
                });
            }

            HttpPostedFile file = Request.Files[0];
            string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

            string folderPath = ResolveUrl(Defines.ResourcePaths.TrinhduyetThamdinh.Tepdinhkem_Duyet);
            if (!Directory.Exists(MapPath(folderPath)))
                Directory.CreateDirectory(MapPath(folderPath));

            string filePath = folderPath + "/" + fn + ext;
            file.SaveAs(MapPath(filePath));

            DoResponse(new APIResponseData()
            {
                ErrorCode = 0,
                Message = "Upload success!",
                Data = filePath.Replace("~", "")
            });
        }

        private void Trinhduyet_Thuchiennhiemvu_Tepdinhkem()
        {
            string fn = Request.QueryString["fn"];
            if (string.IsNullOrEmpty(fn))
                fn = DateTime.Now.Ticks.ToString();

            if (Request.Files.Count == 0)
            {
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 1,
                    Message = "Missing request data!",
                    Data = null
                });
            }

            HttpPostedFile file = Request.Files[0];
            string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

            string folderPath = ResolveUrl(Defines.ResourcePaths.TrinhduyetThuchiennhiemvu.Tepdinhkem);
            if (!Directory.Exists(MapPath(folderPath)))
                Directory.CreateDirectory(MapPath(folderPath));

            string filePath = folderPath + "/" + fn + ext;
            file.SaveAs(MapPath(filePath));

            DoResponse(new APIResponseData()
            {
                ErrorCode = 0,
                Message = "Upload success!",
                Data = filePath.Replace("~", "")
            });
        }

        private void LoaithutucNhiemvu_Huongdan()
        {
            string fn = Request.QueryString["fn"];
            if (string.IsNullOrEmpty(fn))
                fn = DateTime.Now.Ticks.ToString();

            if (Request.Files.Count == 0)
            {
                DoResponse(new APIResponseData()
                {
                    ErrorCode = 1,
                    Message = "Missing request data!",
                    Data = null
                });
            }

            HttpPostedFile file = Request.Files[0];
            string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

            string folderPath = ResolveUrl(Defines.ResourcePaths.LoaiThutucNhiemvu.Huongdan);
            if (!Directory.Exists(MapPath(folderPath)))
                Directory.CreateDirectory(MapPath(folderPath));

            string filePath = folderPath + "/" + fn + ext;
            file.SaveAs(MapPath(filePath));

            DoResponse(new APIResponseData()
            {
                ErrorCode = 0,
                Message = "Upload success!",
                Data = filePath.Replace("~", "")
            });
        }
    }
}