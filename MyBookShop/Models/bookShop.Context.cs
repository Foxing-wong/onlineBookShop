﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBookShop.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BookShopEntities4 : DbContext
    {
        public BookShopEntities4()
            : base("name=BookShopEntities4")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<buyCar> buyCar { get; set; }
        public virtual DbSet<userInfo> userInfo { get; set; }
        public virtual DbSet<toInfo> toInfo { get; set; }
        public virtual DbSet<bookInfo> bookInfo { get; set; }
        public virtual DbSet<orderInfo> orderInfo { get; set; }
        public virtual DbSet<orderDetails> orderDetails { get; set; }
        public virtual DbSet<adminRole> adminRole { get; set; }
        public virtual DbSet<adminInfo> adminInfo { get; set; }
        public virtual DbSet<newsInfo> newsInfo { get; set; }
    }
}
