using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookShop.serviceAPI
{
    /// <summary>
    /// cancelOrder 的摘要说明
    /// </summary>
    public class cancelOrder : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["userLogin"] == null)
            {
                context.Response.Write("您的会话已经失效，请重新登陆");
            }
            else
            {
                APIclass.serviceAPI sapi = new APIclass.serviceAPI();
                string sql = "update orderInfo set orderstate='已取消' where orderId='" + context.Request["orderId"] + "'and buyUser='" + context.Session["userLogin"].ToString() + "'and orderState='等待系统处理'";
                int num = sapi.dataProcessor(sql);
                if (num < 1)
                {
                    context.Response.Write("取消失败，可能原因为订单已经开始生产，请联系客服取消");
                }
                else
                {
                    context.Response.Write("取消成功");
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