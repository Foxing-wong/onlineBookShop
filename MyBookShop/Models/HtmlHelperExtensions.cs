using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookShop.Models
{
    public static class HtmlHelperExtensions
    {
        //创建Url扩展方法,用于在视图生成<a/>标签，注意第一个参数代表该方法为Url扩展方法，实际调用函数时，传入实参的顺序从第二个参数开始
        public static MvcHtmlString CreatePageLiTag(this UrlHelper urlHelper,
                                                    BasePageModel pageModel,
                                                     int offset,
                                                    bool isCurrentIndex = false,
                                                     bool isDisable = true,
                                                     string content = "")
        {
            //根据ActionName生成Url字符串
            string url = urlHelper.Action(pageModel.ActionName, pageModel.ControllerName, new { q = string.IsNullOrEmpty(pageModel.SearchKeyWord) ? @"" : pageModel.SearchKeyWord, offset = offset });

            //根据参数值设置待生成的html标签属性，该属性会受bootstrap样式作用
            string activeClass = !isCurrentIndex ? string.Empty : "class='active'";
            string disableClass = isDisable ? string.Empty : "class='disabled'";
            url = isDisable ? "href='" + url + "'" : string.Empty;
            string contentString = string.IsNullOrEmpty(content) ? offset.ToString() : content;
            //返回html字符串
            return new MvcHtmlString("<li " + activeClass + disableClass + "><a " + url + ">" + contentString + "</a></li>");
        }
    }
}