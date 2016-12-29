using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhiemvu_DBEntities
{
    public class APIRequestData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }

    public class APIResponseData
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }
        public object Data { get; set; }
    }
}
