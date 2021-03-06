﻿using Newtonsoft.Json;
using QLNhiemVu;
using QLNhiemvu_DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace QLNhiemvu_WebAPI
{
    public class BasePage : Page
    {
        public class Defines
        {
            public class ResourcePaths
            {
                public class LoaiThutucNhiemvu
                {
                    public static string Huongdan = "~/Resources/ThutucNhiemvu/Huongdan";
                }
                public class TrinhduyetThuchiennhiemvu
                {
                    public static string Tepdinhkem = "~/Resources/Trinhduyet/ThuchienNhiemvu/Tepdinhkem";
                }
                public class TrinhduyetThamdinh
                {
                    public static string Tepdinhkem = "~/Resources/Trinhduyet/Thamdinh/Tepdinhkem";
                    public static string Tepdinhkem_Duyet = "~/Resources/Trinhduyet/Thamdinh/Duyet/Tepdinhkem";
                }
                public class TrinhduyetPheduyet
                {
                    public static string Tepdinhkem_VB = "~/Resources/Trinhduyet/Pheduyet/TuVBCosan";
                    public static string Tepdinhkem_TD = "~/Resources/Trinhduyet/Pheduyet/TuDulieuThamdinh";
                }
            }
        }
        public int ValidateRequest()
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                Log.write(ex);
                return int.MinValue;
            }
        }

        public void DoResponse(APIResponseData response)
        {
            Response.ContentType = "text/xml";
            Response.Expires = -1;
            Response.Write(JsonConvert.SerializeObject(response));
            Response.End();
        }
    }
}