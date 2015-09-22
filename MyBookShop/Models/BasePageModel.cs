using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookShop.Models
{
    public class BasePageModel
    {
        /// <summary>
        ///用于设置类别编号  根据具体需要而定
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        ///用于搜索功能的查询值 根据具体需要而定
        /// </summary>
        public string SearchKeyWord { get; set; }
        /// <summary>
        ///点击分页时指向 Controller 的名字 根据具体需要而定
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        ///点击分页时指向 Action 的名字 根据具体需要而定
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 总数量
        ///  </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>  
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalCount / (double)PageSize);
            }
        }

        /// <summary>
        /// 根据需要具体而定PageSize
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        ///根据需要具体而定 分页显示最大的页数 
        /// </summary>
        public virtual int DisplayMaxPages
        {
            get
            {
                return 12;
            }
        }
        /// <summary>
        /// 是否有前一页
        /// </summary>
        public bool IsHasPrePage
        {
            get
            {
                return CurrentIndex != 1;
            }
        }
        /// <summary>
        /// 是否有后一页
        /// </summary>
        public bool IsHasNextPage
        {
            get
            {
                return CurrentIndex != TotalPages;
            }
        }
    }
}