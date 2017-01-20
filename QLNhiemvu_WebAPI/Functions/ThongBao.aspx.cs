using Newtonsoft.Json;
using QLNhiemVu;
using QLNhiemvu_DBEntities;
using QLNhiemvu_WebAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace QLNhiemvu_WebAPI.Functions
{
    public partial class ThongBao : BasePage
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
                }
            }
        }
        private void GetList()
        {
            using (DataTools dataTools = new DataTools())
            {
                List<DM_ThongBao> result = dataTools.DM_ThongBao_GetList();

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