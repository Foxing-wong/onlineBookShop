USE [master]
GO
/****** Object:  Database [BookShop]    Script Date: 06/03/2015 20:47:05 ******/
CREATE DATABASE [BookShop] ON  PRIMARY 
( NAME = N'BookShop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.CHULIAM\MSSQL\DATA\BookShop.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BookShop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.CHULIAM\MSSQL\DATA\BookShop_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BookShop] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BookShop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BookShop] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [BookShop] SET ANSI_NULLS OFF
GO
ALTER DATABASE [BookShop] SET ANSI_PADDING OFF
GO
ALTER DATABASE [BookShop] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [BookShop] SET ARITHABORT OFF
GO
ALTER DATABASE [BookShop] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [BookShop] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [BookShop] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [BookShop] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [BookShop] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [BookShop] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [BookShop] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [BookShop] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [BookShop] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [BookShop] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [BookShop] SET  DISABLE_BROKER
GO
ALTER DATABASE [BookShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [BookShop] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [BookShop] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [BookShop] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [BookShop] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [BookShop] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [BookShop] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [BookShop] SET  READ_WRITE
GO
ALTER DATABASE [BookShop] SET RECOVERY FULL
GO
ALTER DATABASE [BookShop] SET  MULTI_USER
GO
ALTER DATABASE [BookShop] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [BookShop] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'BookShop', N'ON'
GO
USE [BookShop]
GO
/****** Object:  Table [dbo].[userInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userInfo](
	[sysID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](50) NOT NULL,
	[userPwd] [nvarchar](50) NOT NULL,
	[userTureName] [nvarchar](50) NOT NULL,
	[userMail] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_userInfo] PRIMARY KEY CLUSTERED 
(
	[sysID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[toInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[toInfo](
	[sysid] [int] IDENTITY(1,1) NOT NULL,
	[toTitle] [nvarchar](6) NOT NULL,
	[toName] [nvarchar](50) NOT NULL,
	[toAddress] [nvarchar](300) NOT NULL,
	[toPhone] [nvarchar](50) NOT NULL,
	[userName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_toInfo] PRIMARY KEY CLUSTERED 
(
	[sysid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orderInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orderInfo](
	[orderId] [bigint] IDENTITY(10000000000000,1) NOT NULL,
	[submitTime] [nvarchar](50) NOT NULL,
	[buyUser] [nvarchar](50) NOT NULL,
	[toName] [nvarchar](50) NOT NULL,
	[toAddress] [nvarchar](200) NOT NULL,
	[toPhone] [nvarchar](50) NOT NULL,
	[orderState] [nvarchar](20) NOT NULL,
	[payType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_orderInfo] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orderDetails]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orderDetails](
	[sysid] [bigint] IDENTITY(1,1) NOT NULL,
	[orderId] [bigint] NOT NULL,
	[bookId] [int] NOT NULL,
	[bookPrice] [money] NOT NULL,
	[buyNum] [int] NOT NULL,
 CONSTRAINT [PK_orderDetails] PRIMARY KEY CLUSTERED 
(
	[sysid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[newsInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[newsInfo](
	[newsId] [int] IDENTITY(101020,1) NOT NULL,
	[newsTitle] [nvarchar](200) NOT NULL,
	[newsContent] [text] NOT NULL,
	[sumitTime] [datetime] NOT NULL,
	[submitor] [nvarchar](50) NOT NULL,
	[submitId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_newsInfo] PRIMARY KEY CLUSTERED 
(
	[newsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[buyCar]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[buyCar](
	[sysID] [int] IDENTITY(1,1) NOT NULL,
	[booksID] [int] NOT NULL,
	[buyNum] [int] NOT NULL,
	[buyUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_buyCar] PRIMARY KEY CLUSTERED 
(
	[sysID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[bookInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bookInfo](
	[bookID] [int] IDENTITY(1,1) NOT NULL,
	[bookName] [nvarchar](50) NOT NULL,
	[bookPrice] [money] NOT NULL,
	[bookImg] [text] NOT NULL,
	[bookNotes] [text] NULL,
	[onlyBuy] [int] NULL,
	[isDel] [nvarchar](1) NULL,
 CONSTRAINT [PK_bookInfo] PRIMARY KEY CLUSTERED 
(
	[bookID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adminRole]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adminRole](
	[sysID] [int] IDENTITY(6000,1) NOT NULL,
	[adminId] [nvarchar](50) NOT NULL,
	[functionName] [nvarchar](50) NOT NULL,
	[actionName] [nvarchar](50) NOT NULL,
	[isLogin] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_adminRole] PRIMARY KEY CLUSTERED 
(
	[sysID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[adminInfo]    Script Date: 06/03/2015 20:47:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adminInfo](
	[adminID] [int] IDENTITY(8000,1) NOT NULL,
	[adminName] [nvarchar](50) NOT NULL,
	[adminNick] [nvarchar](50) NOT NULL,
	[adminPwd] [nvarchar](50) NOT NULL,
	[adminSkin] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_adminInfo] PRIMARY KEY CLUSTERED 
(
	[adminID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_adminInfo_adminSkin]    Script Date: 06/03/2015 20:47:06 ******/
ALTER TABLE [dbo].[adminInfo] ADD  CONSTRAINT [DF_adminInfo_adminSkin]  DEFAULT (N'navbar navbar-default') FOR [adminSkin]
GO
