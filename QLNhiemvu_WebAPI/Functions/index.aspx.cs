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
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DataTools dataTools = new DataTools())
            {
                string path = HttpContext.Current.Server.MapPath("~");
                string folder_name = path + @"bin\log";
                bool folderExists =  Directory.Exists(folder_name);
                if (!folderExists)
                    Directory.CreateDirectory(folder_name);

                pDBConnection.InnerHtml = dataTools.DBMaster_CheckConnection() ? "OK" : "LOST";
            }
        }
    }
}