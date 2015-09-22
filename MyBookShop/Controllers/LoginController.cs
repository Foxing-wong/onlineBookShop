using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        [HttpGet]
        public ActionResult Login(string m,string a,string goTo)
        {
            Session["m"] = m;
            Session["a"] = a;
            if (Session["userLogin"] != null)
            {
                return RedirectToAction("Account", "My");
            }
            else
            {
                if (goTo == "loginError")
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('账号或密码错误，请重新尝试',{title:'错误！！！'});}</script>");
                }
                return View();
            }
            /*else
            {
                if (m == "My")
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.open({type: 2, title: '欢迎登录蜻蜓网',shadeClose: true,shade: 0.8,area: ['380px', '200px'],content: 'login?m=" + Session["m"].ToString() + "&a=" + Session["a"].ToString() + "'});}</script>");
                }
                if (goTo == "loginError")
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('账号或密码错误，请重新尝试',{title:'错误！！！'});}</script>");
                }
                return View();
            } */
        }
        
        [HttpGet]
        public ActionResult otherLogin(string m, string a, string goTo)
        {
            Session["m"] = m;
            Session["a"] = a;
            if (Session["userLogin"] != null)
            {
                return RedirectToAction("Account", "My");
            }
            else
            {
                if (m == "My")
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.open({type: 2, title: '欢迎登录蜻蜓网',shadeClose: true,shade: 0.8,area: ['380px', '220px'],content: 'Login?m=" + Session["m"].ToString() + "&a=" + Session["a"].ToString() + "'});}</script>");
                }
                if (goTo == "loginError")
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('账号或密码错误，请重新尝试');}</script>");
                }
                return View();
            }
        }
        [HttpPost]
        public ContentResult userLogin(string userName,string userPwd)
        {
                APIclass.serviceAPI api = new APIclass.serviceAPI();
                string sql = "select sysid from userinfo where username='" + userName + "' and userpwd='" + userPwd + "'";
                int loginResult=api.getRow(sql); 
                if (loginResult>0)
                {
                    ContentResult content = new ContentResult();
                    Session["userLogin"] = userName;
                    //fanhuijieghuo
                    //return RedirectToAction(Session["a"].ToString(), Session["m"].ToString());
                    if (Session["m"] != null && Session["a"]!=null)
                    {
                         string url = "<script language='javaScript' type='text/javaScript'>window.parent.window.location.href ='/" + Session["m"].ToString() + "/" + Session["a"].ToString() + "';</script>";
                         content.Content = string.Format(url);
                    }
                    else
                    {
                    //return RedirectToAction("Index", "Home");
                        content.Content = string.Format("<script language='javaScript' type='text/javaScript'>window.parent.window.location.href = '/Home/Index';</script>");
                    }
                    return content;
                }
             else
             {
                 Session["userLogin"] = null;
                 //return RedirectToAction("Login", "Login", new { goTo = "loginError" });
                 ContentResult content = new ContentResult();
                 content.Content = string.Format("<script language='javaScript' type='text/javaScript'>window.location.href = '/Login/Login?goTo=loginError';</script>");
                 return content;
             }
        }

        public ActionResult Logout()
        {
            Session["userLogin"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Join()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult userReg(string userName, string userPwd, string trueName, string userMail)
        {
            Models.BookShopEntities4 db2 = new Models.BookShopEntities4();
            Models.userInfo newAdd2 = new Models.userInfo()
            {
                userName=userName,
                userPwd=userPwd,
                userMail=userMail,
                userTureName=trueName
            };
            db2.userInfo.Add(newAdd2);
            int result =db2.SaveChanges();
            if (result > 0)
            {
                Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.msg('注册成功，6秒之后转向登录', {icon: 1,time: 6000}, function(){window.location.href = '/My/Account';})}</script>");
            }
            else
            {
                Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.msg('注册失败，5秒之后转向首页', {icon: 1,time: 5000}, function(){window.location.href = '/Home/Index';})}</script>");
            }
            return View();
        }

    }
}
