using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookShop.serviceAPI
{
    /// <summary>
    /// updateBuyCarNum 的摘要说明
    /// </summary>
    public class updateBuyCarNum : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                string selectInfo = "select * from bookInfo where bookid=and buyUser='" + context.Session["userLogin"].ToString() + "'";
                int num = sapi.getRow(selectInfo);
                if (num < 1)
                {
                    string sql = "update buycar set buyNum='" + context.Request["newNum"] + "'where sysid='" + context.Request["sysID"] + "'and buyUser='" + context.Session["userLogin"].ToString() + "'";//sql语句
                    sapi.dataProcessor(sql);
                }
                else
                {
                    string sql = "update buycar set buyNum='" + context.Request["newNum"] + "'where sysid='" + context.Request["sysID"] + "'and buyUser='" + context.Session["userLogin"].ToString() + "'and booksid !='8'";//sql语句
                    sapi.dataProcessor(sql);
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