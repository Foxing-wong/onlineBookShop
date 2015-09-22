using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Book/

        public ActionResult Details()
        {
            return View();
        }

    }
}
