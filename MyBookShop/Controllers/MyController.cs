using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Controllers
{
    public class MyController : Controller
    {
        //
        // GET: /My/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BuyCar()
        {
            if (Session["userLogin"] != null)
            {
                string uName = Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var data = from a in db.buyCar
                           join b in db.bookInfo
                           on a.booksID equals b.bookID
                           where a.buyUser == uName
                           select new
                           {
                               a.sysID,
                               b.bookID,
                               b.bookImg,
                               b.bookName,
                               b.bookPrice,
                               a.buyNum
                           };
                //var data = db.buyCar.Where(b => b.buyUser == uName);
                ViewData["pe"] = data.ToList();
                return View();

            }
            else
            {
                return View();
            }
            
        }
        //IQueryable<tb_BookDetail> FindBookByBookTypeID(int id)
        //{
        //    return db.tb_BookDetail.Where(b => b.bookTypeID == id);
        //}
        [HttpPost]
        public ActionResult OrderProcessing(string toName, string toAddress, string toPhone, string userPay)
        {
            if (Session["userLogin"] != null)
            {
                string uName = Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                Models.orderInfo oi=new Models.orderInfo ()
                {
                    submitTime=System.DateTime.Now.ToString(),
                    toAddress=toAddress,
                    toName=toName,
                    toPhone=toPhone,
                    buyUser=uName,
                    orderState="等待系统处理",
                    payType = userPay
                };
                db.orderInfo.Add(oi);
                db.SaveChanges();
                var deleteQuery = db.buyCar.Where(e => e.buyUser == uName);
               // var orderId = db.buyCar.Where(e => e.buyUser == uName);
                //Models. bCar = db.buyCar.Where(b => b.booksID == id && b.buyUser == uname).FirstOrDefault();
                foreach (var item in deleteQuery)
                {
                    Models.bookInfo binfo = db.bookInfo.Where(b => b.bookID == item.booksID).FirstOrDefault();
                    Models.orderDetails od = new Models.orderDetails()
                    {
                        orderId = oi.orderId,
                        bookId=item.booksID,
                        bookPrice=binfo.bookPrice,
                        buyNum=item.buyNum
                    };
                    db.orderDetails.Add(od);
                    db.buyCar.Remove(item);  
                }
                db.SaveChanges();
                Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('订单提交成功',function (){location='/My/MyOrder'});}</script>");
                return View();
            }
            else
            {
                return RedirectToAction("otherLogin", "Login", new { m = "My", a = "Settlement" });
            }
            

        }
      
        public ActionResult Settlement()
        {
            if (Session["userLogin"] != null)
            {
                string uName = Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var data = from a in db.buyCar
                           join b in db.bookInfo
                           on a.booksID equals b.bookID
                           where a.buyUser == uName
                           select new
                           {
                               a.sysID,
                               b.bookID,
                               b.bookImg,
                               b.bookName,
                               b.bookPrice,
                               a.buyNum
                           };
                var toData = from a in db.toInfo
                             where a.userName == uName
                           select new
                           {
                               a.toTitle,
                               a.toName,
                               a.toAddress,
                               a.toPhone
                           };
                double allSum = 0.00;
                //var data = db.buyCar.Where(b => b.buyUser == uName);
                foreach (var item in data.ToList())
                {
                    allSum += Convert.ToDouble(item.bookPrice) *Convert.ToDouble( item.buyNum);

                }
                ViewData["todata"] = toData.ToList();
                ViewData["pe"] = data.ToList();
                if (allSum<=0)
                {
                    Response.Write("<script language='javaScript' type='text/javaScript'>window.onload=function(){layer.alert('购物车是空的，无需结算',function (){location='/My/buyCar'});}</script>");
                }
                return View();
            }
            else
            {
                return RedirectToAction("otherLogin", "Login", new { m = "My", a = "Settlement" });
            }

        }
        public ActionResult newBuyCar()
        {
            if (Session["userLogin"] != null)
            {
                string uName = Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                // var data = from b in db.buyCar where b.buyUser =Session["userLogin"].ToString() select b;
                //db.buyCar;

                var data = db.buyCar.Where(b => b.buyUser == uName);
                return View(data.ToList());
            }
            else
            {
                return View();
            }

        }
        public ActionResult MyOrder(int? offset)
        {
            if (Session["userLogin"] != null)
            {
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                string uName = Session["userLogin"].ToString();
                var orderResult = db.orderInfo.Where(e => e.buyUser == uName).OrderByDescending(e=>e.orderId).ToList();
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (offset == null)
                    offset = 1;
                //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
                if (orderResult.Count > 0)
                {
                    //创建分页模型类对象
                    Models.BasePageModel page =
                        new Models.BasePageModel() { ControllerName = "My", ActionName = "MyOrder", CurrentIndex = (int)offset, TotalCount = orderResult.Count, PageSize = 3 };
                    //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                    var bookspageList = orderResult.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                    //向视图传递分页对象
                    ViewData["pagemodel"] = page;
                    //返回视图，传递当前页图书数据
                    return View(bookspageList);
                }
                else {
                    return View(orderResult);
                }
                   
                //var toData = from a in db.orderInfo
                //             join b in db.orderDetails on a.orderId equals b.orderId
                //             into temp
                //             from r in temp.DefaultIfEmpty()
                //             join s in db.bookInfo on r.bookId equals s.bookID
                //             where a.buyUser == uName
                //             select new
                //             {
                //                 a.buyUser,
                //                 a.orderId,
                //                 a.orderState,
                //                 a.toName,
                //                 a.submitTime,
                //                 a.payType,
                //                 s.bookImg
                //             };
                //var toData = from a in db.orderInfo
                //             from b in db.orderDetails
                //             from r in db.bookInfo
                //             where a.buyUser == uName &b.bookId==r.bookID &a.orderId==b.orderId
                //             orderby a.submitTime descending
                //             select new
                //             {
                //                 a.buyUser,
                //                 a.orderId,
                //                 a.orderState,
                //                 a.toName,
                //                 a.submitTime,
                //                 a.payType,
                //                 r.bookImg,
                //                 b.bookPrice,
                //                 b.buyNum,
                //                 b.bookId,
                //                 r.bookName
                //             };
                //ViewData["orderInfo"] = toData.ToList();
            }
            else
            {
                return RedirectToAction("otherLogin", "Login", new { m = "My", a = "MyOrder" });
            }
           
        }
        public ActionResult orderDetails(long orderId)
        {
            Models.BookShopEntities4 db = new Models.BookShopEntities4();
            string uName = Session["userLogin"].ToString();
            Models.orderInfo exitUser = db.orderInfo.Where(b => b.orderId == orderId && b.buyUser == uName).FirstOrDefault();
            if (exitUser != null)
            {
                var orderData = from a in db.orderDetails
                                join b in db.bookInfo
                                on a.bookId equals b.bookID
                                where a.orderId == orderId
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
        public ActionResult Account()
        {
            if (Session["userLogin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("otherLogin", "Login", new { m = "My", a = "MyOrder" });
            }
        }

    }
}
