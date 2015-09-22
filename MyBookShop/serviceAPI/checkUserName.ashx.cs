using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookShop.serviceAPI
{
    /// <summary>
    /// checkUserName 的摘要说明
    /// </summary>
    public class checkUserName : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string sql = "select sysid from userinfo where username='" + context.Request["userName"] + "'";//sql语句
            APIclass.serviceAPI sapi = new APIclass.serviceAPI();
            int val = sapi.getRow(sql);
            if (val > 0)
            {
                context.Response.Write("N");
            }
            else
            {
                context.Response.Write("Y");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}