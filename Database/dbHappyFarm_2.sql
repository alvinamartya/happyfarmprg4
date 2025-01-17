USE [master]
GO
/****** Object:  Database [HappyFarmPRG4]    Script Date: 1/5/2021 9:14:06 PM ******/
CREATE DATABASE [HappyFarmPRG4]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HappyFarmPRG4', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\HappyFarmPRG4.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HappyFarmPRG4_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\HappyFarmPRG4_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [HappyFarmPRG4] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HappyFarmPRG4].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HappyFarmPRG4] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET ARITHABORT OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HappyFarmPRG4] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HappyFarmPRG4] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HappyFarmPRG4] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HappyFarmPRG4] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HappyFarmPRG4] SET  MULTI_USER 
GO
ALTER DATABASE [HappyFarmPRG4] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HappyFarmPRG4] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HappyFarmPRG4] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HappyFarmPRG4] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HappyFarmPRG4] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HappyFarmPRG4', N'ON'
GO
ALTER DATABASE [HappyFarmPRG4] SET QUERY_STORE = OFF
GO
USE [HappyFarmPRG4]
GO
/****** Object:  Table [dbo].[Banner]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PromoId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Image] [text] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chat]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[From] [int] NOT NULL,
	[To] [int] NOT NULL,
	[Message] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserLoginId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerAddress]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAddress](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[SubDistrictId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerFeedback]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerFeedback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Rating] [float] NOT NULL,
	[Note] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserLoginId] [int] NOT NULL,
	[RegionId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Gender] [char](1) NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Goods]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodsPriceRegion]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodsPriceRegion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GoodsId] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodsStockRegion]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodsStockRegion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GoodsId] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
	[Stock] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promo]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Image] [text] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsFreeDelivery] [char](1) NOT NULL,
	[Discount] [float] NOT NULL,
	[MinTransaction] [money] NOT NULL,
	[MaxDiscount] [money] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchasing]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchasing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[FarmerName] [nvarchar](100) NOT NULL,
	[FarmerPhone] [nvarchar](20) NOT NULL,
	[FarmerAddress] [nvarchar](255) NOT NULL,
	[TotalPurchasePrice] [money] NOT NULL,
	[DateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchasingDetails]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasingDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchasingId] [int] NOT NULL,
	[GoodsId] [int] NOT NULL,
	[Qty] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Selling]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selling](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[SellingStatusId] [int] NOT NULL,
	[CustomerAddressId] [int] NOT NULL,
	[PromoId] [int] NOT NULL,
	[RecipientName] [nvarchar](100) NOT NULL,
	[RecipientPhone] [nvarchar](20) NOT NULL,
	[PaymentImage] [varbinary](max) NOT NULL,
	[ShippingCharges] [money] NOT NULL,
	[TotalSalePrice] [money] NOT NULL,
	[DateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingActivity]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellingActivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellingId] [int] NOT NULL,
	[SellingStatusid] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingDetails]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellingDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellingId] [int] NOT NULL,
	[GoodsId] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[GoodsPrice] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingStatus]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellingStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubDistrict]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubDistrict](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ShippingCharges] [money] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ModifiedAt] [datetime] NOT NULL,
	[RowStatus] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogin]    Script Date: 1/5/2021 9:14:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, N'buah', 1, 1, CAST(N'2021-01-05T20:26:15.310' AS DateTime), CAST(N'2021-01-05T20:26:15.310' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 1, NULL, N'Super Admin', N'0812', N'sa@happyfarm.id', N'Jakarta', N'M', NULL, NULL, CAST(N'2020-12-29T17:31:05.993' AS DateTime), CAST(N'2020-12-29T17:31:05.993' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, 2, 1, N'Admin Promosi', N'0812', N'marketingadmin@happyfarm.id', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-04T16:08:55.490' AS DateTime), CAST(N'2021-01-04T16:08:55.490' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, 3, 1, N'Admin Produksi', N'0812', N'productionadmin@happyfarm.id', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-04T16:31:00.343' AS DateTime), CAST(N'2021-01-04T16:31:00.343' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (4, 4, 1, N'Manager', N'0812111122', N'manager@happyfarm.id', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-04T16:39:43.223' AS DateTime), CAST(N'2021-01-04T16:39:43.223' AS DateTime), N'D')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (5, 5, 1, N'Manager', N'0812111122', N'manager2@happyfarm.id', N'Jakarta', N'F', 1, 1, CAST(N'2021-01-04T16:53:43.737' AS DateTime), CAST(N'2021-01-04T16:53:43.737' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (6, 6, 1, N'Evan', N'0812', N'manager3@happyfarm.id', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-04T16:57:55.013' AS DateTime), CAST(N'2021-01-04T17:00:00.123' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (7, 7, 1, N'Tom', N'0812111122', N'manager4@happyfarm.id', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-05T21:04:21.813' AS DateTime), CAST(N'2021-01-05T21:04:21.813' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[Goods] ON 

INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (2, 1, 1, 2, N'Jeruk', CAST(N'2021-01-05T20:57:12.723' AS DateTime), CAST(N'2021-01-05T20:59:20.680' AS DateTime), N'nipis', N'D')
SET IDENTITY_INSERT [dbo].[Goods] OFF
GO
SET IDENTITY_INSERT [dbo].[Region] ON 

INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, N'Jakarta', 1, 1, CAST(N'2020-12-29T21:24:10.853' AS DateTime), CAST(N'2021-01-04T17:24:13.320' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, N'Sayur', 1, 1, CAST(N'2021-01-05T20:32:05.743' AS DateTime), CAST(N'2021-01-05T20:52:08.333' AS DateTime), N'D')
SET IDENTITY_INSERT [dbo].[Region] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name]) VALUES (1, N'Super Admin')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (2, N'Manager')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (3, N'Admin Promosi')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (4, N'Admin Produksi')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (5, N'Customer Service')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (6, N'Admin Penjualan')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (7, N'Customer')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[SubDistrict] ON 

INSERT [dbo].[SubDistrict] ([Id], [RegionId], [Name], [ShippingCharges], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 1, N'Gading Cempaka', 10000.0000, 1, 1, CAST(N'2021-01-05T20:41:58.300' AS DateTime), CAST(N'2021-01-05T20:42:41.163' AS DateTime), N'D')
SET IDENTITY_INSERT [dbo].[SubDistrict] OFF
GO
SET IDENTITY_INSERT [dbo].[UserLogin] ON 

INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (1, 1, N'sa', N'36dd292533174299fb0c34665df468bb881756ca9eaf9757d0cfde38f9ededa1')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (2, 3, N'adminpromosi', N'2a25ba5adf3b58c014c450e071b2157fe50dfe787f7ec74ba91ac879753a66e3')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (3, 4, N'adminproduksi', N'48bee31a8390ed2d0172653e7529db2f78b10bf9c56e1f3f8dcfa63430bac123')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (4, 2, N'manager', N'db80cbc6593787b9dabd99a5c6d91cc360ab08d61581d91b76640873618bcdac')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (5, 2, N'manager2', N'5a960059c37450002beec8a70c3343d7036e0ab97e6a009efc791637228e448f')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (6, 2, N'manager3', N'08ced62fef774ff4586e7a0d59838a61169798a9ddb207906683e7bd761cca81')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (7, 2, N'manager4', N'83a01f749865ac0c493295ab3662686d8525caeb1979d0edb496d4675bb3e188')
SET IDENTITY_INSERT [dbo].[UserLogin] OFF
GO
ALTER TABLE [dbo].[Banner] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Banner] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Chat] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Goods] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Goods] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[GoodsPriceRegion] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[GoodsPriceRegion] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Promo] ADD  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [dbo].[Promo] ADD  DEFAULT (getdate()) FOR [EndDate]
GO
ALTER TABLE [dbo].[Promo] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Promo] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Purchasing] ADD  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[Region] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Region] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Selling] ADD  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[SellingActivity] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SubDistrict] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SubDistrict] ADD  DEFAULT (getdate()) FOR [ModifiedAt]
GO
ALTER TABLE [dbo].[Banner]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Banner]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Banner]  WITH CHECK ADD FOREIGN KEY([PromoId])
REFERENCES [dbo].[Promo] ([Id])
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Chat]  WITH CHECK ADD FOREIGN KEY([From])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Chat]  WITH CHECK ADD FOREIGN KEY([To])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([UserLoginId])
REFERENCES [dbo].[UserLogin] ([Id])
GO
ALTER TABLE [dbo].[CustomerAddress]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[CustomerAddress]  WITH CHECK ADD FOREIGN KEY([SubDistrictId])
REFERENCES [dbo].[SubDistrict] ([Id])
GO
ALTER TABLE [dbo].[CustomerFeedback]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Selling] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([UserLoginId])
REFERENCES [dbo].[UserLogin] ([Id])
GO
ALTER TABLE [dbo].[Goods]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Goods]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Goods]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[GoodsPriceRegion]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[GoodsPriceRegion]  WITH CHECK ADD FOREIGN KEY([GoodsId])
REFERENCES [dbo].[Goods] ([Id])
GO
ALTER TABLE [dbo].[GoodsPriceRegion]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[GoodsPriceRegion]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[GoodsStockRegion]  WITH CHECK ADD FOREIGN KEY([GoodsId])
REFERENCES [dbo].[Goods] ([Id])
GO
ALTER TABLE [dbo].[GoodsStockRegion]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Promo]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Promo]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Purchasing]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[PurchasingDetails]  WITH CHECK ADD FOREIGN KEY([GoodsId])
REFERENCES [dbo].[Goods] ([Id])
GO
ALTER TABLE [dbo].[PurchasingDetails]  WITH CHECK ADD FOREIGN KEY([PurchasingId])
REFERENCES [dbo].[Purchasing] ([Id])
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[SellingActivity]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[SellingActivity]  WITH CHECK ADD FOREIGN KEY([SellingId])
REFERENCES [dbo].[Selling] ([Id])
GO
ALTER TABLE [dbo].[SellingActivity]  WITH CHECK ADD FOREIGN KEY([SellingStatusid])
REFERENCES [dbo].[SellingStatus] ([Id])
GO
ALTER TABLE [dbo].[SellingDetails]  WITH CHECK ADD FOREIGN KEY([GoodsId])
REFERENCES [dbo].[Goods] ([Id])
GO
ALTER TABLE [dbo].[SellingDetails]  WITH CHECK ADD FOREIGN KEY([SellingId])
REFERENCES [dbo].[Selling] ([Id])
GO
ALTER TABLE [dbo].[SubDistrict]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[SubDistrict]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[SubDistrict]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[UserLogin]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
USE [master]
GO
ALTER DATABASE [HappyFarmPRG4] SET  READ_WRITE 
GO
