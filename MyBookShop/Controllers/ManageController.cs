using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            if (Session["adminLoginError"] != null)
            {
                ViewBag.info = Session["adminLoginError"].ToString();
            }
            return View();
        }
        public ActionResult adminLogout()
        {
            Session["adminLogin"] = null;
            return RedirectToAction("Index", "Manage");
        }

        public ActionResult changePwd()
        {
            if (isLogin("changePwd"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult changeSkin()
        {
            if (Session["adminLogin"] != null)
            {
                if (Session["adminSkin"].ToString() == "navbar navbar-default")
                {
                    Session["adminSkin"] = "navbar navbar-inverse";
                }
                else
                {
                    Session["adminSkin"] = "navbar navbar-default";
                }
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                string uName = Session["adminLogin"].ToString();
                Models.adminInfo admini = db.adminInfo.Where(e => e.adminName == uName).FirstOrDefault();
                admini.adminSkin = Session["adminSkin"].ToString();
                db.SaveChanges();
                return RedirectToAction("MyDesktop", "Manage");
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }
        public ActionResult adminLogin(string adminName,string adminPwd)
        {
            Models.BookShopEntities4 db = new Models.BookShopEntities4();
            Models.adminInfo ain = db.adminInfo.Where(e => e.adminName == adminName && e.adminPwd == adminPwd).FirstOrDefault();
            if (ain != null)
            {
                Session["adminNick"] = ain.adminNick;
                Session["adminLogin"] = adminName;
                Session["adminLoginError"] = null;
                Session["adminSkin"] = ain.adminSkin;
                return RedirectToAction("MyDesktop", "Manage");
            }
            else
            {
                Session["adminLoginError"] = "账号密码不匹配，请重新尝试";
                return RedirectToAction("Index", "Manage");
            }
        }
        public static bool isLogin(string aName)
        {
            try
            {
                string userName = System.Web.HttpContext.Current.Session["adminLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                MyBookShop.Models.adminRole jiance = db.adminRole.Where(a => a.adminId == userName && a.actionName == aName).FirstOrDefault();
                if (jiance != null)
                {
                    if (jiance.isLogin == "允许访问")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public ActionResult UpdateRole(string keys,string type)
        {
            if (isLogin("RoleSetting"))
            {
                try
                {
                    int key = Convert.ToInt16(Models.EncryptHandler.Decrypt(keys));
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                    Models.adminRole anr = db.adminRole.Where(e => e.sysID == key).FirstOrDefault();
                    if (anr != null)
                    {
                        if (type == "allow")
                        {
                            anr.isLogin = "允许访问";
                        }
                        else
                        {
                            anr.isLogin = "拒绝访问";
                        }
                    }
                    db.SaveChanges();
                    return Content("操作成功");
                }
                catch
                {
                    return Content("系统错误");
                }
            }
            else
            {
                return Content("权限不足");
            }
        }
        public ActionResult RoleSetting(int? offset, string keyWords)
        {
            if (isLogin("RoleSetting"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var roles =db.adminRole.Where(s => s.adminId != "sysAdmin").ToList();
                if (keyWords == null||keyWords=="")
                {
                    roles = db.adminRole.Where(s => s.adminId != "sysAdmin").ToList();
                }
                else
                {
                    offset = 1;
                    roles = db.adminRole.Where(s => s.adminId != "sysAdmin"&&s.adminId.Contains(keyWords)).ToList();
                }
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (roles.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "RoleSetting", CurrentIndex = (int)offset, TotalCount = roles.Count, PageSize = 14 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = roles.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else
                {
                    return View(roles);
                }
           }
            else
                return RedirectToAction("Error", "Manage");
            
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult userAdmin(int? offset, string typeValue, string userKEY, string newAdminName, string newAdminPwd, string keyWords,string adminNick)
        {
            if (isLogin("userAdmin"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var adminInfo = db.adminInfo.Where(a => a.adminName != "sysAdmin").ToList();
                switch (typeValue)
                {
                    case "search":
                        adminInfo = db.adminInfo.Where(a => a.adminName != "sysAdmin" && a.adminName.Contains(keyWords)).ToList();
                        break;
                    case "deleteUser":
                        int keys = Convert.ToInt16(MyBookShop.Models.EncryptHandler.Decrypt(userKEY));
                        MyBookShop.Models.adminInfo ai = db.adminInfo.Where(a => a.adminID == keys).FirstOrDefault();
                        string uname = ai.adminName;
                        if (ai != null)
                        {
                            db.adminInfo.Remove(ai);
                        }
                        var allData = from a in db.adminRole
                                      where a.adminId == uname
                                      select a;
                        db.adminRole.RemoveRange(allData);
                        db.SaveChanges();
                        Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('删除用户成功',function (){location='/Manage/userAdmin'});}</script>");
                        adminInfo = db.adminInfo.Where(a => a.adminName != "sysAdmin").ToList();
                        break;
                    case "addUser":
                        MyBookShop.Models.adminInfo se = db.adminInfo.Where(a => a.adminName == newAdminName).FirstOrDefault();
                        if (se != null)
                        {
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('该用户名已经存在，请勿重复',function (){location='/Manage/userAdmin'});}</script>");
                        }
                        else if (adminNick == "" || newAdminName==""||newAdminPwd=="")
                        {
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('相应值不能为空，请输入',function (){location='/Manage/userAdmin'});}</script>");
                        }
                        else
                        {
                            Models.adminInfo newAdd = new Models.adminInfo()
                            {
                                adminNick=adminNick,
                                adminName = newAdminName,
                                adminPwd = newAdminPwd,
                                adminSkin = "navbar navbar-default"
                            };
                            db.adminInfo.Add(newAdd);
                            string[] myMenu = 
                        { "订单总览","订单审核","订单状态更新","新增动态","管理动态","商品入库","商品库存更新",
                            "商品资料修改","会员信息查看","会员信息更新","会员状态调整" ,"用户管理","用户权限设定","修改密码"};
                            string[] myMenuAction = 
                        { "AllOrder","OrderAudit","UpdateStatus","NewsEditor?type=addNews","NewsAdmin","商品入库","商品库存更新",
                            "商品资料修改","会员信息查看","会员信息更新","会员状态调整" ,"userAdmin","RoleSetting","changePwd"};
                            for (int i = 0; i < myMenu.Length; i++)
                            {
                                Models.adminRole ar = new Models.adminRole()
                                {
                                    adminId = newAdminName,
                                    functionName = myMenu[i],
                                    actionName = myMenuAction[i],
                                    isLogin = "拒绝访问"
                                };
                                db.adminRole.Add(ar);
                            }
                            db.SaveChanges();
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('新增用户成功',function (){location='/Manage/userAdmin'});}</script>");
                            adminInfo = db.adminInfo.Where(a => a.adminName != "sysAdmin").ToList();
                        }
                        break;
                    case "resetPwd":
                        int Nkey = Convert.ToInt16(MyBookShop.Models.EncryptHandler.Decrypt(userKEY));
                        MyBookShop.Models.adminInfo newai = db.adminInfo.Where(a => a.adminID == Nkey).FirstOrDefault();
                        if (newAdminPwd == "")
                        {
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('新密码不能为空',function (){location='/Manage/userAdmin'});}</script>");

                        }
                        else
                        {
                            newai.adminPwd = newAdminPwd;
                            db.SaveChanges();
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('重置用户密码成功',function (){location='/Manage/userAdmin'});}</script>");

                        }
                        break;
                }
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (adminInfo.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "userAdmin", CurrentIndex = (int)offset, TotalCount = adminInfo.Count, PageSize = 10 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = adminInfo.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                return View(adminInfo);
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult processOrder(string key)
        {
            if (isLogin("UpdateStatus"))
            {
                try
                {
                    string oid = key.Substring(1, key.Length - 1);
                    Int64 passid = Convert.ToInt64(oid);
                    string type = key.Substring(0, 1);
                    string state = "";
                    switch (type)
                    {
                        case "A":
                            state = "审核通过，等待仓库发货";
                            break;
                        case "B":
                            state = "已取消(用户主动取消)";
                            break;
                        case "C":
                            state = "已取消(后台系统取消)";
                            break;
                        case "D":
                            state = "已完成";
                            break;
                        case "E":
                            state = "状态异常，等待处理";
                            break;
                        case "F":
                            state = "订单作废";
                            break;
                        case "G":
                            state = "其他，等待处理";
                            break;
                        case "H":
                            state = "等待系统处理";
                            break;
                        case "I":
                            state = "已发货，等待用户收货";
                            break;
                        case "X":
                            state = "已退货";
                            break;

                    }
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                    Models.orderInfo oi = db.orderInfo.Where(e => e.orderId == passid).FirstOrDefault();
                    string oldState = "";
                    if (oi != null)
                    {
                        oldState = oi.orderState;
                        oi.orderState = state;
                    }
                    db.SaveChanges();
                    return Content("状态已成功从【" + oldState + "】变更为【" + state + "】");
                }
                catch
                {
                    return Content("系统错误");
                }
            }
            else if (isLogin("OrderAudit"))
            {
                try
                {
                    string oid = key.Substring(1, key.Length - 1);
                    Int64 passid = Convert.ToInt64(oid);
                    string type = key.Substring(0, 1);
                    string state = "";
                    switch (type)
                    {
                        case "A":
                            state = "审核通过，等待仓库发货";
                            break;
                        case "E":
                            state = "状态异常，等待处理";
                            break;
                    }
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                    Models.orderInfo oi = db.orderInfo.Where(e => e.orderId == passid).FirstOrDefault();
                    string oldState = "";
                    if (oi != null)
                    {
                        oldState = oi.orderState;
                        oi.orderState = state;
                    }
                    db.SaveChanges();
                    return Content("状态已成功从【" + oldState + "】变更为【" + state + "】");
                }
                catch
                {
                    return Content("系统错误");
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
            
        }
        public ActionResult NewsEditor(string type,int? newsID)
        {
            if (type == "addNews")
            {
                if (isLogin("NewsEditor?type=addNews"))
                {
                    ViewBag.type = type;
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Manage");
                }
            }
            else if (type == "updateNews")
            {
                if (isLogin("NewsAdmin"))
                {
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                    var Result = db.newsInfo.Where(e => e.newsId == newsID).FirstOrDefault();
                    ViewBag.newsTitle = Result.newsTitle;
                    ViewBag.newsContent = Result.newsContent;
                    ViewBag.NEWSID = Result.newsId;
                    ViewBag.type = type;
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult AllOrder(int? offset, string keyWords, string searchType)
        {
            if (isLogin("AllOrder"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var orderResult = db.orderInfo.OrderByDescending(e => e.orderId).ToList();
                if (keyWords != null && searchType != null)
                {
                    switch (searchType)
                    {
                        case "orderid":
                            Int64 newword;
                            try
                            {
                                newword = Convert.ToInt64(keyWords);
                            }
                            catch
                            {
                                newword = 00;
                            }
                            orderResult = db.orderInfo.Where(e => e.orderId == newword).ToList();
                            break;
                        case "userName":
                            orderResult = db.orderInfo.Where(e => e.buyUser.Contains(keyWords)).ToList();
                            break;
                        case "toName":
                            orderResult = db.orderInfo.Where(e => e.toName.Contains(keyWords)).ToList();
                            break;
                        case "toPhone":
                            orderResult = db.orderInfo.Where(e => e.toPhone.Contains(keyWords)).ToList();
                            break;
                    }
                    ViewBag.stype = searchType;
                    ViewBag.swords = keyWords;
                }
                else
                {
                    orderResult = db.orderInfo.OrderByDescending(e => e.orderId).ToList();
                }
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                ViewData["orderCount"] = orderResult.Count;
                if (orderResult.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "MyDesktop", CurrentIndex = (int)offset, TotalCount = orderResult.Count, PageSize = 10 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = orderResult.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else
                {
                    return View(orderResult);
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        [ValidateInput(false)]
        public ActionResult NewsOperation(string type,string content,string newsTitle,int? newsID)
        {
            if (type == "addNews")
            {
                if (isLogin("NewsEditor?type=addNews"))
                {
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                            Models.newsInfo ni = new Models.newsInfo()
                            {
                                submitId = Session["adminLogin"].ToString(),
                                submitor = Session["adminNick"].ToString(),
                                newsContent = content,
                                newsTitle = newsTitle,
                                sumitTime = DateTime.Now
                            };
                            db.newsInfo.Add(ni);
                            db.SaveChanges();
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('新增动态成功，页面跳转到动态管理',function (){location='/Manage/NewsAdmin'});}</script>");
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Manage");
                }

            }
            else
            {
                if (isLogin("NewsAdmin"))
                {
                    Models.BookShopEntities4 db = new Models.BookShopEntities4();
                    switch (type)
                    {
                        case "deleteNews":
                            try
                            {
                                var dele = db.newsInfo.Where(a => a.newsId == newsID).FirstOrDefault();
                                db.newsInfo.Remove(dele);
                                db.SaveChanges();
                                Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('删除动态成功',function (){location='/Manage/NewsAdmin'});}</script>");
                            }
                            catch
                            {
                                
                            }
                            break;
                        case "updateNews":
                            var up = db.newsInfo.Where(a => a.newsId == newsID).FirstOrDefault();
                            up.newsTitle = newsTitle;
                            up.newsContent = content;
                            db.SaveChanges();
                            Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('动态修改成功',function (){location='/Manage/NewsAdmin'});}</script>");
                            break;
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Manage");
                }
            }
            
        }
        public ActionResult orderDetails(string key)
        {
            if (isLogin("OrderAudit") || isLogin("AllOrder") || isLogin("UpdateStatus"))
            {
                Int64 OrderId = Convert.ToInt64(MyBookShop.Models.EncryptHandler.Decrypt(key));
                float n = OrderId;
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                Models.orderInfo exitUser = db.orderInfo.Where(b => b.orderId == OrderId).FirstOrDefault();
                if (exitUser != null)
                {
                    var orderData = from a in db.orderDetails
                                    join b in db.bookInfo
                                    on a.bookId equals b.bookID
                                    where a.orderId == OrderId
                                    select new
                                    {
                                        b.bookID,
                                        b.bookImg,
                                        b.bookName,
                                        a.bookPrice,
                                        a.buyNum,
                                    };
                    ViewData["payType"] = exitUser.payType;
                    ViewData["toAddress"] = exitUser.toAddress;
                    ViewData["toName"] = exitUser.toName;
                    ViewData["toPhone"] = exitUser.toPhone;
                    ViewData["submitTime"] = exitUser.submitTime;
                    ViewData["orderId"] = exitUser.orderId;
                    ViewData["statue"] = exitUser.orderState;
                    ViewData["orderData"] = orderData.ToList();
                    //var lst1 = db.orderDetails.Where(e => e.orderId == orderId);
                    return View();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
 
        }
        public ActionResult NewsAdmin(int? offset)
        {
            if (isLogin("NewsAdmin"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var orderResult = db.newsInfo.OrderByDescending(e => e.newsId).ToList();
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                ViewData["orderCount"] = orderResult.Count;
                if (orderResult.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "MyDesktop", CurrentIndex = (int)offset, TotalCount = orderResult.Count, PageSize = 12 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = orderResult.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else
                {
                    return View(orderResult);
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult UpdateStatus(int? offset, string keyWords,string searchType)
        {
            if (isLogin("UpdateStatus"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var orderResult = db.orderInfo.OrderByDescending(e => e.orderId).ToList();
                if (keyWords != null && searchType != null)
                {
                    switch (searchType)
                    {
                        case "orderid":
                            Int64 newword;
                            try
                            {
                                newword = Convert.ToInt64(keyWords);
                            }
                            catch
                            {
                                newword = 00;
                            }
                            orderResult = db.orderInfo.Where(e => e.orderId == newword).ToList();
                            break;
                        case "userName":
                            orderResult = db.orderInfo.Where(e => e.buyUser.Contains(keyWords)).ToList();
                            break;
                        case "toName":
                            orderResult = db.orderInfo.Where(e => e.toName.Contains(keyWords)).ToList();
                            break;
                        case "toPhone":
                            orderResult = db.orderInfo.Where(e => e.toPhone.Contains(keyWords)).ToList();
                            break;
                    }
                    ViewBag.stype = searchType;
                    ViewBag.swords = keyWords;
                }
                else
                {
                    orderResult = db.orderInfo.OrderByDescending(e => e.orderId).ToList();
                }
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                ViewData["orderCount"] = orderResult.Count;
                if (orderResult.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "MyDesktop", CurrentIndex = (int)offset, TotalCount = orderResult.Count, PageSize = 10 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = orderResult.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else
                {
                    return View(orderResult);
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult OrderAudit(int? offset)
        {
            if (isLogin("OrderAudit"))
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var orderResult = db.orderInfo.Where(e => e.orderState == "等待系统处理").OrderByDescending(e => e.orderId).ToList();
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                ViewData["orderCount"] = orderResult.Count;
                if (orderResult.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "Manage", ActionName = "MyDesktop", CurrentIndex = (int)offset, TotalCount = orderResult.Count, PageSize = 10 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = orderResult.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else
                {
                    return View(orderResult);
                }
            }
            else
            {
                return RedirectToAction("Error", "Manage");
            }
        }
        public ActionResult MyDesktop()
        {
            if (Session["adminLogin"] != null)
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var orderResult = db.orderInfo.Where(e => e.orderState == "等待系统处理").OrderByDescending(e => e.orderId).ToList();
                ViewData["orderCount"] = orderResult.Count;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }  
        }
    }
}
