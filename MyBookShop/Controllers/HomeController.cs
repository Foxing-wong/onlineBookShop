using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [ValidateInput(false)]
        public ActionResult editor(string content)
        {
            ViewBag.Content = content;
            return View();
        }
        public ActionResult Index(int? offset)
        {
            Models.BookShopEntities4 db = new Models.BookShopEntities4();
            var books = db.bookInfo.Where(a =>a.isDel!="Y").ToList();
            var newsinfo = db.newsInfo.OrderByDescending(m => m.newsId).ToList().Take(5);
            ViewData["newsData"] = newsinfo;
            if (offset == null)
                offset = 1;
            //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
            if (offset == null)
                offset = 1;
            //通过指定路径将查询结果返回给指定视图（指定不同控制器的路径中必须包括View文件的后缀cshtml）
            if (books.Count > 0)
            {
                //创建分页模型类对象
                Models.BasePageModel page =
                    new Models.BasePageModel() { ControllerName="Home",ActionName = "index", CurrentIndex = (int)offset, TotalCount = books.Count ,PageSize=12};
                //获取指定页码的图书数据集合（Ship是跳过指定数量的元素，返回剩余的元素，Take是返回指定数量的连续元素
                var bookspageList = books.Skip((page.CurrentIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                //向视图传递分页对象
                ViewData["pagemodel"] = page;
                //返回视图，传递当前页图书数据
                return View(bookspageList);
            }
            else
                return View();
        }
        [HttpGet]
        public ActionResult BookDetails(string bookId)
        {
            try
            {
                int newBookId = Convert.ToInt16(bookId);
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                var bookDetail = db.bookInfo.Where(a => a.bookID == newBookId).ToList();
                return View(bookDetail);
            }
            catch
            {
                return Content("非法访问");
            }
            
        }
        [HttpGet]
        public ActionResult getNewsContent(int newsId)
        {
             
            Models.BookShopEntities4 db = new Models.BookShopEntities4();
            var newsinfo = db.newsInfo.Where(m => m.newsId == newsId).FirstOrDefault();
            ViewData["title"]=newsinfo.newsTitle;
            ViewData["content"]=newsinfo.newsContent;
            ViewData["time"] = newsinfo.sumitTime;
            return View();
        }
        
        [HttpGet]
        public ActionResult addToBuyCar(int id)
        {
            if (Session["userLogin"] != null)
            {
                string uname=Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                Models.bookInfo bi = db.bookInfo.Where(a => a.bookID == id && a.isDel != "Y").FirstOrDefault();
                if (bi != null)
                {
                    Models.buyCar bCar = db.buyCar.Where(b => b.booksID == id && b.buyUser == uname).FirstOrDefault();
                    string output = "";
                    if (bCar == null)
                    {
                        Models.buyCar newAdd = new Models.buyCar()
                        {
                            booksID = id,
                            buyNum = 1,
                            buyUser = Session["userLogin"].ToString()
                        };
                        db.buyCar.Add(newAdd);
                        output = "加入购物车成功";
                    }
                    else
                    {
                        Models.bookInfo binfo = db.bookInfo.Where(b => b.bookID == id).FirstOrDefault();
                        if (binfo.onlyBuy != null && bCar.buyNum >= binfo.onlyBuy)
                        {
                            bCar.buyNum = binfo.onlyBuy.Value;
                            output = "该商品有限购规则，仅允许购买" + binfo.onlyBuy.Value + "个，已将购物车中该商品数量调整为最大购买数量";
                        }
                        else
                        {
                            bCar.buyNum += 1;
                            output = "购物车中已存在该商品，商品数量加1";
                        }
                    }
                    db.SaveChanges();
                    return Content(output);
                }
                else
                {
                    return Content("该商品已经下架或者商品不存在，无法加入购物车");
                }
                
            }
            else {
                return Content("尚未登录，无法加入购物车，请先登录");
            }


        }
        [HttpGet]
        public ActionResult updateCartNum(int sysID,int newNum)
        {
            if (Session["userLogin"] != null)
            {
                string uname = Session["userLogin"].ToString();
                Models.BookShopEntities4 db = new Models.BookShopEntities4();
                Models.buyCar bCar = db.buyCar.Where(b => b.sysID == sysID && b.buyUser == uname).FirstOrDefault();
                int id = bCar.booksID;
                string output = "okay";
                Models.bookInfo binfo = db.bookInfo.Where(b => b.bookID == id).FirstOrDefault();
                if (binfo.isDel != "Y")
                {
                    if (binfo.onlyBuy != null && newNum >= binfo.onlyBuy)
                    {
                        bCar.buyNum = binfo.onlyBuy.Value;
                        output = "该商品有限购规则，仅允许购买" + binfo.onlyBuy.Value + "个，已将购物车中该商品数量调整为最大购买数量";
                    }
                    else
                    {
                        bCar.buyNum = newNum;
                        output = "okay";
                    }
                }
                else
                {
                    bCar.buyNum = 0;
                    output = "该商品已经下架或者不存在，数量自动调整为0个";
                }
                db.SaveChanges();
                return Content(output);
            }
            else
            {
                return Content("会话已失效，请重新登陆后再操作");
            }


        }
        [HttpGet]
        public ActionResult Search(string searchType,string searchWords)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult getBookMenu()
        {

            return View();
        }

    }
}
