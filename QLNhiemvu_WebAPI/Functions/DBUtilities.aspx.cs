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
    public partial class DBUtilities : BasePage
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
                    case "checkdbconnection":
                        CheckDBConnection();
                        break;
                }
            }
        }

        private void CheckDBConnection()
        {
            using (DataTools dataTools = new DataTools())
            {
                bool check = dataTools.DBMaster_CheckConnection();

                DoResponse(new APIResponseData()
                {
                    ErrorCode = check ? 0 : 1,
                    Message = check ? "Success" : "Fail",
                    Data = null
                });
            }
        }
    }
}