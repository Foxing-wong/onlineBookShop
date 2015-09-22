using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookShop.serviceAPI
{
    /// <summary>
    /// buyCarDelete 的摘要说明
    /// </summary>
    public class buyCarDelete : IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["userLogin"] == null)
            {
                context.Response.Write("您的会话已经失效，请重新登陆");
            }
            else
            {
                string sql = "delete from buycar where  sysid='" + context.Request["sysID"] + "'and buyUser='" + context.Session["userLogin"].ToString() + "'";//sql语句
                APIclass.serviceAPI sapi = new APIclass.serviceAPI();
                int val = sapi.dataProcessor(sql);
                if (val > 0)
                {
                    context.Response.Write("从购物车移除成功");
                }
                else
                {
                    context.Response.Write("从购物车移除失败");
                }
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