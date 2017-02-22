using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace QLNhiemvu_WebAPI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routeCollection)
        {
            routeCollection.MapPageRoute("Home", "", "~/Functions/index.aspx");
            routeCollection.MapPageRoute("LoaiThutucNhiemvu", "loaithutuc", "~/Functions/LoaiThutuc.aspx");
            routeCollection.MapPageRoute("LoaiThutucNhiemvu_Huongdan", "loaithutuc_huongdan", "~/Functions/LoaiThutuc_Huongdan.aspx");
            routeCollection.MapPageRoute("LoaiThutucNhiemvu_Noidung", "loaithutuc_noidung", "~/Functions/LoaiThutuc_Noidung.aspx");
            routeCollection.MapPageRoute("LoaiThutucNhiemvu_Truongdulieu", "loaithutuc_truongdulieu", "~/Functions/LoaiThutuc_Truongdulieu.aspx");

            routeCollection.MapPageRoute("Trinhduyet_ThuchienNhiemvu", "trinhduyet_thuchiennhiemvu", "~/Functions/Trinhduyet.aspx");
            routeCollection.MapPageRoute("Trinhduyet_Phancong", "trinhduyet_phancong", "~/Functions/Phancong.aspx");
            routeCollection.MapPageRoute("Trinhduyet_Thamdinh", "trinhduyet_thamdinh", "~/Functions/Thamdinh.aspx");
            routeCollection.MapPageRoute("Trinhduyet_PheduyetVB", "trinhduyet_pheduyet_vb", "~/Functions/Pheduyet.aspx");

            routeCollection.MapPageRoute("LoaiThutucTrinhduyet", "loaithutuctrinhduyet", "~/Functions/LoaiThutucTrinhduyet.aspx");
            routeCollection.MapPageRoute("ThongBao", "thongbao", "~/Functions/ThongBao.aspx");
            
            routeCollection.MapPageRoute("DBUtilities", "dbutilities", "~/Functions/DBUtilities.aspx");
            routeCollection.MapPageRoute("ResourceManager", "resources", "~/Functions/ResourceManager.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}