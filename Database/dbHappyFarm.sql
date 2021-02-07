USE [HappyFarmPRG4]
GO
/****** Object:  Table [dbo].[Banner]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PromoId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Image] [varchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[Category]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Customer]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[CustomerFeedback]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Employee]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Goods]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Image] [varchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[GoodsPriceRegion]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[GoodsStockRegion]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Promo]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[Code] [nchar](7) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Image] [varchar](100) NOT NULL,
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
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchasing]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[PurchasingDetails]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasingDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchasingId] [int] NOT NULL,
	[GoodsId] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK__Purchasi__3214EC070E51BA14] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[Selling]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selling](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[PromoId] [int] NULL,
	[RecipientName] [nvarchar](100) NOT NULL,
	[RecipientPhone] [nvarchar](20) NOT NULL,
	[RecipientAddress] [nvarchar](255) NOT NULL,
	[SubDistrictId] [int] NOT NULL,
	[PaymentImage] [varchar](100) NULL,
	[ShippingCharges] [money] NOT NULL,
	[TotalSalePrice] [money] NOT NULL,
	[DateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingActivity]    Script Date: 07/02/2021 21:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellingActivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellingId] [int] NOT NULL,
	[SellingStatusid] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingDetails]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[SellingStatus]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[SubDistrict]    Script Date: 07/02/2021 21:21:06 ******/
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
/****** Object:  Table [dbo].[UserLogin]    Script Date: 07/02/2021 21:21:06 ******/
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
SET IDENTITY_INSERT [dbo].[Banner] ON 

INSERT [dbo].[Banner] ([Id], [PromoId], [Name], [Image], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 1, N'Harbolnas 2021', N'11cef5d9-6fc5-4cef-9e7e-45a64271131d.ico', 1, 1, CAST(N'2021-01-17T13:58:26.637' AS DateTime), CAST(N'2021-01-17T13:58:26.637' AS DateTime), N'A')
INSERT [dbo].[Banner] ([Id], [PromoId], [Name], [Image], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, 1, N'Harbolnas 2021-2', N'6a9596de-d6ab-49a1-896d-e3ae9d54b254.ico', 4, 4, CAST(N'2021-01-17T14:11:03.750' AS DateTime), CAST(N'2021-01-17T14:11:03.750' AS DateTime), N'A')
INSERT [dbo].[Banner] ([Id], [PromoId], [Name], [Image], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, 1, N'Harbolnas 2021-4', N'1284eb71-8596-4ee9-8485-1628b411525e.ico', 1, 1, CAST(N'2021-01-18T14:58:05.253' AS DateTime), CAST(N'2021-01-18T14:58:46.253' AS DateTime), N'D')
SET IDENTITY_INSERT [dbo].[Banner] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, N'Buah', 3, 3, CAST(N'2021-01-16T22:30:00.330' AS DateTime), CAST(N'2021-01-16T22:30:00.330' AS DateTime), N'A')
INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, N'Sayur', 3, 3, CAST(N'2021-01-16T22:30:05.470' AS DateTime), CAST(N'2021-01-16T22:30:05.470' AS DateTime), N'D')
INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, N'Umbi-Umbian', 3, 3, CAST(N'2021-01-16T22:30:11.217' AS DateTime), CAST(N'2021-01-16T22:30:11.217' AS DateTime), N'A')
INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (4, N'Kacang', 3, 3, CAST(N'2021-01-16T22:30:19.130' AS DateTime), CAST(N'2021-01-16T22:30:19.130' AS DateTime), N'A')
INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (5, N'Beras', 3, 3, CAST(N'2021-01-16T22:30:24.657' AS DateTime), CAST(N'2021-01-16T22:30:34.153' AS DateTime), N'A')
INSERT [dbo].[Category] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (6, N'Susu', 1, 1, CAST(N'2021-01-18T15:05:33.433' AS DateTime), CAST(N'2021-01-18T15:07:00.043' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [UserLoginId], [Name], [PhoneNumber], [Email], [Gender]) VALUES (2, 1, N'Tes', N'0812', N'tes@gmail.com', N'M')
INSERT [dbo].[Customer] ([Id], [UserLoginId], [Name], [PhoneNumber], [Email], [Gender]) VALUES (3, 10, N'Syamsul', N'081211122232', N'bert@gmail.com', N'M')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerFeedback] ON 

INSERT [dbo].[CustomerFeedback] ([Id], [OrderId], [Rating], [Note]) VALUES (1, 1, 4, N'Pelayanan bagus')
SET IDENTITY_INSERT [dbo].[CustomerFeedback] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 1, NULL, N'Super Admin', N'0812', N'sa@happyfarm.id', N'Jakarta', N'M', NULL, NULL, CAST(N'2020-12-29T17:31:05.993' AS DateTime), CAST(N'2020-12-29T17:31:05.993' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, 2, 1, N'Syamsul', N'0857123231231', N'syamsulramadhoni17@gmail.com', N'Jakarta', N'M', 1, 1, CAST(N'2021-01-15T07:50:48.113' AS DateTime), CAST(N'2021-01-15T07:50:48.113' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, 3, 1, N'Doni', N'081231238121', N'thekepo@gmail.com', N'Perum. BGA 2 Blok D 11/5 Tambun Selatan', N'M', 2, 2, CAST(N'2021-01-15T15:17:30.747' AS DateTime), CAST(N'2021-01-15T15:17:30.747' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (4, 4, 1, N'Doni', N'0857123231231', N'emailsyamsulramadhoni@gmail.com', N'Bekasi', N'M', 2, 2, CAST(N'2021-01-15T16:00:00.277' AS DateTime), CAST(N'2021-01-15T16:00:00.277' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (5, 5, 1, N'Ivan', N'081231238129', N'ivan@gmail.com', N'Pemalang, Jawa Tengah', N'M', 2, 2, CAST(N'2021-01-16T21:05:46.340' AS DateTime), CAST(N'2021-01-16T21:06:06.553' AS DateTime), N'D')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (6, 6, 1, N'Syamsul Ramadhoni', N'081122224444', N'syamsulramadhoni99@gmail.com', N'Griya Asri 2 Blok D 11/5 Tambun Selatan, Bekasi', N'M', 1, 1, CAST(N'2021-01-18T13:55:49.780' AS DateTime), CAST(N'2021-01-20T14:32:45.470' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (7, 7, 1, N'Customer Service', N'085712323113', N'syamsulramadhoni0@gmail.com', N'Bekasi', N'M', 1, 1, CAST(N'2021-01-20T14:33:34.270' AS DateTime), CAST(N'2021-01-20T14:33:34.270' AS DateTime), N'A')
INSERT [dbo].[Employee] ([Id], [UserLoginId], [RegionId], [Name], [PhoneNumber], [Email], [Address], [Gender], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (9, 9, 1, N'Erwin Gutawa', N'081211122232', N'apenjualan53@gmail.com', N'Jakarta', N'M', 1, 9, CAST(N'2021-01-21T21:28:08.510' AS DateTime), CAST(N'2021-01-29T20:30:20.457' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[Goods] ON 

INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (1, 1, 1, 5, N'd61b86b9-2d5e-4af1-abd5-5bd3ef01b5d1.ico', N'Beras Rojolele', CAST(N'2021-01-16T22:41:19.967' AS DateTime), CAST(N'2021-01-16T22:41:19.967' AS DateTime), N'Beras berkualitas', N'A')
INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (2, 1, 1, 5, N'b39187ef-3bf8-442a-80c3-fd498d7493ec.ico', N'Beras Super', CAST(N'2021-01-18T15:15:42.810' AS DateTime), CAST(N'2021-01-18T15:26:10.113' AS DateTime), N'Beras berkualitas', N'D')
INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (3, 3, 3, 1, N'35d1a358-3021-47b6-a488-0406f89464ba.png', N'Melon', CAST(N'2021-01-21T15:05:23.757' AS DateTime), CAST(N'2021-01-21T15:05:23.757' AS DateTime), N'Buah berkualitas', N'A')
INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (4, 1, 1, 1, N'14262745-874d-4441-b776-4d09f36939e8.jpg', N'Jeruk Bali', CAST(N'2021-02-01T11:20:02.453' AS DateTime), CAST(N'2021-02-01T11:20:02.453' AS DateTime), N'Jeruk segar asli Bali', N'A')
INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (5, 1, 1, 5, N'af80ff79-0143-4e01-ae6b-a963cb400c4e.jpg', N'Maknyuss', CAST(N'2021-02-01T11:21:24.940' AS DateTime), CAST(N'2021-02-01T11:21:24.940' AS DateTime), N'Tanpa pemutih, tanpa pengawet, tanpa pewangi', N'A')
INSERT [dbo].[Goods] ([Id], [CreatedBy], [ModifiedBy], [CategoryId], [Image], [Name], [CreatedAt], [ModifiedAt], [Description], [RowStatus]) VALUES (6, 1, 1, 5, N'2bab3718-d0f3-413f-8e44-cebb5978237d.jpg', N'Beras Pandan Wangi', CAST(N'2021-02-01T11:22:59.437' AS DateTime), CAST(N'2021-02-01T11:22:59.437' AS DateTime), N'Beras berkualitas', N'A')
SET IDENTITY_INSERT [dbo].[Goods] OFF
GO
SET IDENTITY_INSERT [dbo].[GoodsPriceRegion] ON 

INSERT [dbo].[GoodsPriceRegion] ([Id], [GoodsId], [RegionId], [CreatedBy], [Price], [CreatedAt], [ModifiedBy], [ModifiedAt], [RowStatus]) VALUES (1, 1, 1, 3, 15000.0000, CAST(N'2021-01-21T15:00:20.537' AS DateTime), 3, CAST(N'2021-01-21T15:00:20.537' AS DateTime), N'A')
INSERT [dbo].[GoodsPriceRegion] ([Id], [GoodsId], [RegionId], [CreatedBy], [Price], [CreatedAt], [ModifiedBy], [ModifiedAt], [RowStatus]) VALUES (2, 1, 3, 3, 12000.0000, CAST(N'2021-01-21T15:00:37.040' AS DateTime), 3, CAST(N'2021-01-22T14:18:28.650' AS DateTime), N'A')
INSERT [dbo].[GoodsPriceRegion] ([Id], [GoodsId], [RegionId], [CreatedBy], [Price], [CreatedAt], [ModifiedBy], [ModifiedAt], [RowStatus]) VALUES (3, 3, 1, 3, 10000.0000, CAST(N'2021-01-21T15:06:36.017' AS DateTime), 3, CAST(N'2021-01-21T15:06:36.017' AS DateTime), N'A')
INSERT [dbo].[GoodsPriceRegion] ([Id], [GoodsId], [RegionId], [CreatedBy], [Price], [CreatedAt], [ModifiedBy], [ModifiedAt], [RowStatus]) VALUES (4, 1, 5, 3, 15000.0000, CAST(N'2021-01-22T14:18:42.203' AS DateTime), 3, CAST(N'2021-01-22T14:18:42.203' AS DateTime), N'A')
INSERT [dbo].[GoodsPriceRegion] ([Id], [GoodsId], [RegionId], [CreatedBy], [Price], [CreatedAt], [ModifiedBy], [ModifiedAt], [RowStatus]) VALUES (5, 3, 3, 3, 11000.0000, CAST(N'2021-01-22T15:03:07.937' AS DateTime), 3, CAST(N'2021-01-22T15:03:07.937' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[GoodsPriceRegion] OFF
GO
SET IDENTITY_INSERT [dbo].[GoodsStockRegion] ON 

INSERT [dbo].[GoodsStockRegion] ([Id], [GoodsId], [RegionId], [Stock]) VALUES (3, 1, 1, 210)
INSERT [dbo].[GoodsStockRegion] ([Id], [GoodsId], [RegionId], [Stock]) VALUES (4, 3, 1, 100)
SET IDENTITY_INSERT [dbo].[GoodsStockRegion] OFF
GO
SET IDENTITY_INSERT [dbo].[Promo] ON 

INSERT [dbo].[Promo] ([Id], [CreatedBy], [ModifiedBy], [Code], [Name], [Image], [StartDate], [EndDate], [IsFreeDelivery], [Discount], [MinTransaction], [MaxDiscount], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 4, 4, N'BYPCHJN', N'Harbolnas 2021', N'e3c665af-ba3e-422b-87f2-0ce5a80adfa2.ico', CAST(N'2021-01-19' AS Date), CAST(N'2021-01-21' AS Date), N'Y', 10, 100000.0000, 15000.0000, CAST(N'2021-01-17T13:31:10.437' AS DateTime), CAST(N'2021-01-17T13:31:34.503' AS DateTime), N'A')
INSERT [dbo].[Promo] ([Id], [CreatedBy], [ModifiedBy], [Code], [Name], [Image], [StartDate], [EndDate], [IsFreeDelivery], [Discount], [MinTransaction], [MaxDiscount], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, 1, 1, N'GICVLCJ', N'HUT Happy Farm 2021', N'bdf7485a-0279-488b-82d6-ce5fdabcbc41.ico', CAST(N'2021-02-12' AS Date), CAST(N'2021-02-13' AS Date), N'N', 10, 150000.0000, 20000.0000, CAST(N'2021-01-18T14:17:55.957' AS DateTime), CAST(N'2021-01-18T14:19:32.417' AS DateTime), N'D')
SET IDENTITY_INSERT [dbo].[Promo] OFF
GO
SET IDENTITY_INSERT [dbo].[Purchasing] ON 

INSERT [dbo].[Purchasing] ([Id], [EmployeeId], [FarmerName], [FarmerPhone], [FarmerAddress], [TotalPurchasePrice], [DateTime]) VALUES (3, 3, N'Fadli', N'081122233344', N'Jakarta', 1000000.0000, CAST(N'2021-01-21T14:54:53.670' AS DateTime))
INSERT [dbo].[Purchasing] ([Id], [EmployeeId], [FarmerName], [FarmerPhone], [FarmerAddress], [TotalPurchasePrice], [DateTime]) VALUES (4, 3, N'Adit', N'08112223399', N'Jakarta', 900000.0000, CAST(N'2021-01-21T15:06:18.577' AS DateTime))
INSERT [dbo].[Purchasing] ([Id], [EmployeeId], [FarmerName], [FarmerPhone], [FarmerAddress], [TotalPurchasePrice], [DateTime]) VALUES (5, 3, N'Fadli', N'081122233344', N'Jakarta', 1000000.0000, CAST(N'2021-01-22T14:17:47.430' AS DateTime))
INSERT [dbo].[Purchasing] ([Id], [EmployeeId], [FarmerName], [FarmerPhone], [FarmerAddress], [TotalPurchasePrice], [DateTime]) VALUES (6, 3, N'Fadli', N'081122233344', N'Jakarta', 100000.0000, CAST(N'2021-01-22T15:01:57.787' AS DateTime))
SET IDENTITY_INSERT [dbo].[Purchasing] OFF
GO
SET IDENTITY_INSERT [dbo].[PurchasingDetails] ON 

INSERT [dbo].[PurchasingDetails] ([Id], [PurchasingId], [GoodsId], [Qty], [Price]) VALUES (1, 3, 1, 100, CAST(10000 AS Decimal(18, 0)))
INSERT [dbo].[PurchasingDetails] ([Id], [PurchasingId], [GoodsId], [Qty], [Price]) VALUES (2, 4, 3, 100, CAST(9000 AS Decimal(18, 0)))
INSERT [dbo].[PurchasingDetails] ([Id], [PurchasingId], [GoodsId], [Qty], [Price]) VALUES (3, 5, 1, 100, CAST(10000 AS Decimal(18, 0)))
INSERT [dbo].[PurchasingDetails] ([Id], [PurchasingId], [GoodsId], [Qty], [Price]) VALUES (4, 6, 1, 10, CAST(10000 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[PurchasingDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Region] ON 

INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, N'Jakarta', 1, 2, CAST(N'2020-12-29T21:24:10.853' AS DateTime), CAST(N'2021-01-15T08:02:42.180' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, N'Surabaya', 1, 2, CAST(N'2021-01-15T08:48:40.637' AS DateTime), CAST(N'2021-01-16T21:46:01.110' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, N'Bandung', 2, 2, CAST(N'2021-01-16T22:09:15.930' AS DateTime), CAST(N'2021-01-16T22:09:15.930' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (4, N'Palembang', 2, 2, CAST(N'2021-01-16T22:09:39.283' AS DateTime), CAST(N'2021-01-16T22:09:39.283' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (5, N'Makasar', 2, 2, CAST(N'2021-01-16T22:09:47.187' AS DateTime), CAST(N'2021-01-16T22:09:47.187' AS DateTime), N'A')
INSERT [dbo].[Region] ([Id], [Name], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (6, N'Lombok', 1, 1, CAST(N'2021-01-18T14:05:58.850' AS DateTime), CAST(N'2021-01-18T14:07:54.337' AS DateTime), N'D')
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
SET IDENTITY_INSERT [dbo].[Selling] ON 

INSERT [dbo].[Selling] ([Id], [CustomerId], [PromoId], [RecipientName], [RecipientPhone], [RecipientAddress], [SubDistrictId], [PaymentImage], [ShippingCharges], [TotalSalePrice], [DateTime]) VALUES (1, 3, NULL, N'tes', N'0812', N'jakarta', 1, NULL, 10000.0000, 100000.0000, CAST(N'2021-01-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Selling] ([Id], [CustomerId], [PromoId], [RecipientName], [RecipientPhone], [RecipientAddress], [SubDistrictId], [PaymentImage], [ShippingCharges], [TotalSalePrice], [DateTime]) VALUES (2, 3, NULL, N'Alvin Amartya', N'Jl. Parkit No. 61', N'Jl. Parkit No. 61', 1, NULL, 10000.0000, 200000.0000, CAST(N'2021-02-07T20:30:41.497' AS DateTime))
SET IDENTITY_INSERT [dbo].[Selling] OFF
GO
SET IDENTITY_INSERT [dbo].[SellingActivity] ON 

INSERT [dbo].[SellingActivity] ([Id], [SellingId], [SellingStatusid], [CreatedBy], [CreatedAt]) VALUES (1, 1, 5, 9, CAST(N'2021-01-27T13:28:20.583' AS DateTime))
INSERT [dbo].[SellingActivity] ([Id], [SellingId], [SellingStatusid], [CreatedBy], [CreatedAt]) VALUES (9, 1, 6, 9, CAST(N'2021-01-27T15:04:20.377' AS DateTime))
INSERT [dbo].[SellingActivity] ([Id], [SellingId], [SellingStatusid], [CreatedBy], [CreatedAt]) VALUES (10, 1, 6, 9, CAST(N'2021-01-27T15:04:30.727' AS DateTime))
INSERT [dbo].[SellingActivity] ([Id], [SellingId], [SellingStatusid], [CreatedBy], [CreatedAt]) VALUES (11, 2, 1, NULL, CAST(N'2021-02-07T20:30:41.697' AS DateTime))
SET IDENTITY_INSERT [dbo].[SellingActivity] OFF
GO
SET IDENTITY_INSERT [dbo].[SellingDetails] ON 

INSERT [dbo].[SellingDetails] ([Id], [SellingId], [GoodsId], [Qty], [GoodsPrice]) VALUES (1, 1, 1, 1, 10000.0000)
INSERT [dbo].[SellingDetails] ([Id], [SellingId], [GoodsId], [Qty], [GoodsPrice]) VALUES (2, 2, 1, 10, 15000.0000)
INSERT [dbo].[SellingDetails] ([Id], [SellingId], [GoodsId], [Qty], [GoodsPrice]) VALUES (3, 2, 3, 5, 10000.0000)
SET IDENTITY_INSERT [dbo].[SellingDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[SellingStatus] ON 

INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (1, N'Menunggu Pembayaran')
INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (2, N'Memeriksa Pembayaran')
INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (3, N'Pembayaran Berhasil')
INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (4, N'Sedang Diantar')
INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (5, N'Belum Direview')
INSERT [dbo].[SellingStatus] ([Id], [Name]) VALUES (6, N'Selesai')
SET IDENTITY_INSERT [dbo].[SellingStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[SubDistrict] ON 

INSERT [dbo].[SubDistrict] ([Id], [RegionId], [Name], [ShippingCharges], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (1, 1, N'Pulo Gadung', 10000.0000, 1, 1, CAST(N'2021-01-15T08:00:10.373' AS DateTime), CAST(N'2021-01-15T08:00:10.373' AS DateTime), N'A')
INSERT [dbo].[SubDistrict] ([Id], [RegionId], [Name], [ShippingCharges], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (2, 1, N'Cakung', 8000.0000, 2, 1, CAST(N'2021-01-15T15:15:38.273' AS DateTime), CAST(N'2021-01-18T14:36:14.790' AS DateTime), N'A')
INSERT [dbo].[SubDistrict] ([Id], [RegionId], [Name], [ShippingCharges], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (3, 1, N'Sunter', 9000.0000, 1, 1, CAST(N'2021-01-18T14:12:05.583' AS DateTime), CAST(N'2021-01-18T14:37:03.917' AS DateTime), N'A')
INSERT [dbo].[SubDistrict] ([Id], [RegionId], [Name], [ShippingCharges], [CreatedBy], [ModifiedBy], [CreatedAt], [ModifiedAt], [RowStatus]) VALUES (4, 1, N'Jatinegara', 10000.0000, 1, 1, CAST(N'2021-02-01T11:18:12.580' AS DateTime), CAST(N'2021-02-01T11:18:12.580' AS DateTime), N'A')
SET IDENTITY_INSERT [dbo].[SubDistrict] OFF
GO
SET IDENTITY_INSERT [dbo].[UserLogin] ON 

INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (1, 1, N'sa', N'36dd292533174299fb0c34665df468bb881756ca9eaf9757d0cfde38f9ededa1')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (2, 2, N'manager1', N'd73abf9991450f0862739d77866c8849e641114a811a109dbdc8134fb5fc46c4')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (3, 4, N'doni', N'ca3ad330add435f26112868b1bb2d0bf975e646aa5d32ad23a5e1190e0b30be4')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (4, 3, N'doni17', N'ddf737df491c6289b4893c5ff8b263c50f3591db91a0c58e089c96b5f8992cc2')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (5, 4, N'ivan99', N'8a966386c362ac79bbeec75a67a3dd3a5514fcf4f413729bd922e3af2b3a6884')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (6, 3, N'syamsulr17', N'494f6ff49c7bcba2554eb8337c2b34e9c26fb7a3d255776706ca936c7d75c4fa')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (7, 5, N'cs1', N'2fe8af1fd80007f20116a58798e012a37b40b9389ec1e3c4f580616cb315fe1f')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (8, 6, N'apen53', N'b7d2433afd4485a3b053925d692659eb9887db5f44919337bf195cb2e6f566e3')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (9, 6, N'adminpenjualan', N'9f2433d4ba178b2acae08f4c1926b901a10ceae18145bb39c5a314c80e81f34f')
INSERT [dbo].[UserLogin] ([Id], [RoleId], [Username], [Password]) VALUES (10, 7, N'syamsul0117', N'5c93f4212f8c476596fd7e5388576765a966974e138d6d3d9053c350258fb254')
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
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([UserLoginId])
REFERENCES [dbo].[UserLogin] ([Id])
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
ALTER TABLE [dbo].[PurchasingDetails]  WITH CHECK ADD  CONSTRAINT [FK__Purchasin__Goods__03F0984C] FOREIGN KEY([GoodsId])
REFERENCES [dbo].[Goods] ([Id])
GO
ALTER TABLE [dbo].[PurchasingDetails] CHECK CONSTRAINT [FK__Purchasin__Goods__03F0984C]
GO
ALTER TABLE [dbo].[PurchasingDetails]  WITH CHECK ADD  CONSTRAINT [FK__Purchasin__Purch__04E4BC85] FOREIGN KEY([PurchasingId])
REFERENCES [dbo].[Purchasing] ([Id])
GO
ALTER TABLE [dbo].[PurchasingDetails] CHECK CONSTRAINT [FK__Purchasin__Purch__04E4BC85]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Selling]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Selling]  WITH CHECK ADD FOREIGN KEY([PromoId])
REFERENCES [dbo].[Promo] ([Id])
GO
ALTER TABLE [dbo].[Selling]  WITH CHECK ADD FOREIGN KEY([SubDistrictId])
REFERENCES [dbo].[SubDistrict] ([Id])
GO
ALTER TABLE [dbo].[SellingActivity]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[SellingActivity]  WITH CHECK ADD FOREIGN KEY([SellingId])
REFERENCES [dbo].[Selling] ([Id])
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
