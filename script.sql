USE [master]
GO
/****** Object:  Database [FoodManagment]    Script Date: 4/25/2025 4:54:36 PM ******/
create DATABASE [FoodManagment]

GO

GO
USE [FoodManagment]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 4/25/2025 4:54:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[AddedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[PaymentStatus] [nvarchar](20) NULL,
	[PaidAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[ImageURL] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/25/2025 4:54:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Role] [nvarchar](10) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (16, 1, 14, 16, CAST(N'2025-03-13T13:05:26.100' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (19, 1, 18, 14, CAST(N'2025-03-13T13:10:05.633' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (27, 1, 8, 17, CAST(N'2025-03-20T13:47:20.603' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (28, 1, 22, 7, CAST(N'2025-03-20T13:47:34.213' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (29, 1, 25, 1, CAST(N'2025-03-20T13:47:35.930' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (1028, 1, 9, 4, CAST(N'2025-04-16T13:30:27.233' AS DateTime))
INSERT [dbo].[Cart] ([CartID], [UserID], [ProductID], [Quantity], [AddedAt]) VALUES (1029, 1, 16, 2, CAST(N'2025-04-23T17:21:00.717' AS DateTime))
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (2, N'Burger')
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (4, N'Desserts')
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (3, N'Drinks')
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (1, N'Pizza')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (5, 3, 8, 1, CAST(20000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [ProductID], [Quantity], [Price]) VALUES (6, 3, 16, 2, CAST(123.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [Status]) VALUES (1, 3, CAST(N'2025-03-06T16:41:35.333' AS DateTime), CAST(21.98 AS Decimal(10, 2)), N'Pending')
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [Status]) VALUES (2, 4, CAST(N'2025-03-06T16:41:35.333' AS DateTime), CAST(14.98 AS Decimal(10, 2)), N'Processing')
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [Status]) VALUES (3, 1, CAST(N'2025-04-23T17:21:01.913' AS DateTime), CAST(20246.00 AS Decimal(10, 2)), N'Pending')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Payments] ON 

INSERT [dbo].[Payments] ([PaymentID], [OrderID], [PaymentMethod], [PaymentStatus], [PaidAt]) VALUES (1, 1, N'Credit Card', N'Completed', CAST(N'2025-03-06T16:41:35.333' AS DateTime))
INSERT [dbo].[Payments] ([PaymentID], [OrderID], [PaymentMethod], [PaymentStatus], [PaidAt]) VALUES (2, 2, N'Cash', N'Pending', NULL)
SET IDENTITY_INSERT [dbo].[Payments] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (8, N'Hieu', N'hihihihihihihihihihihihihihi', CAST(200.00 AS Decimal(10, 2)), 106, 4, N'images/chocolate_cake.jpg', CAST(N'2025-03-11T22:30:18.000' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (9, N'Pepperoni Pizza123123', N'hihihihihihihihihihihihihihi', CAST(1231.00 AS Decimal(10, 2)), 1228, 2, N'images/cr71.jpg', CAST(N'2025-03-11T23:46:17.013' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (14, N'ádsad', N'123', CAST(123.00 AS Decimal(10, 2)), 107, 2, N'images/coca_cola.jpg', CAST(N'2025-03-12T16:37:12.000' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (16, N'qưe', N'qưe', CAST(123.00 AS Decimal(10, 2)), 121, 4, N'images/chocolate_cake.jpg', CAST(N'2025-03-12T16:40:18.000' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (18, N'ádsad', N'123', CAST(123.00 AS Decimal(10, 2)), 109, 2, N'images/coca_cola.jpg', CAST(N'2025-03-12T16:43:22.197' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (22, N'Matcha latte', N'ngon vcl chịu chịu', CAST(25.00 AS Decimal(10, 2)), 13, 3, N'images/coca_cola.jpg', CAST(N'2025-03-13T14:14:27.000' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (25, N'ÁD', N'QƯE', CAST(123.00 AS Decimal(10, 2)), 122, 4, N'images/pepperoni_pizza.jpg', CAST(N'2025-03-13T14:17:44.133' AS DateTime))
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Stock], [CategoryID], [ImageURL], [CreatedAt]) VALUES (26, N'Coca Cola2', N'123', CAST(123.00 AS Decimal(10, 2)), 123, 1, N'images/pepperoni_pizza.jpg', CAST(N'2025-03-13T14:18:55.587' AS DateTime))
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (1, N'admin1', N'hashedpassword123', N'Admin User', N'admin@example.com', N'0987654321', N'Admin', CAST(N'2025-03-06T16:41:35.330' AS DateTime), 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (2, N'staff1', N'hashedpassword123', N'Staff Member', N'staff@example.com', N'0987654322', N'Staff', CAST(N'2025-03-06T16:41:35.330' AS DateTime), 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (3, N'user1', N'hashedpassword123', N'Customer One', N'user1@example.com', N'0987654323', N'User', CAST(N'2025-03-06T16:41:35.330' AS DateTime), 0)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (4, N'user2', N'S', N'Customer Two', N'user2@example.com', N'09876543213', N'User', CAST(N'2025-03-06T16:41:35.330' AS DateTime), 0)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (12, N'bhieu3009@gmail.com', N'qưe', N'buihieu', N'bhieu3009@gmail.com', N'0962702002', N'Admin', CAST(N'2025-03-11T23:31:19.633' AS DateTime), 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (13, N'bhieu40@yahoo.com', N'deptrai', N'buihieu', N'bhieu40@yahoo.com', NULL, N'Staff', CAST(N'2025-03-12T16:36:38.523' AS DateTime), 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (15, N'bhieu30009@gmail.com', N'123', N'buihieudep tria', N'bhieu30009@gmail.com', N'0962702002', N'Admin', CAST(N'2025-03-13T15:51:16.267' AS DateTime), 1)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [Phone], [Role], [CreatedAt], [Status]) VALUES (16, N'hieudeptrai@ihihi.com', N'hieubui', N'buihieu', N'hieudeptrai@ihihi.com', N'0962702002', N'User', CAST(N'2025-04-16T13:33:39.290' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Categori__8517B2E085DDC998]    Script Date: 4/25/2025 4:54:37 PM ******/
ALTER TABLE [dbo].[Categories] ADD UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E431543B86]    Script Date: 4/25/2025 4:54:37 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D105347DE2AF15]    Script Date: 4/25/2025 4:54:37 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT ('Pending') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [Stock]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD CHECK  (([Status]='Cancelled' OR [Status]='Delivered' OR [Status]='Shipped' OR [Status]='Processing' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD CHECK  (([PaymentMethod]='PayPal' OR [PaymentMethod]='Credit Card' OR [PaymentMethod]='Cash'))
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD CHECK  (([PaymentStatus]='Failed' OR [PaymentStatus]='Completed' OR [PaymentStatus]='Pending'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='User' OR [Role]='Staff' OR [Role]='Admin'))
GO
USE [master]
GO
ALTER DATABASE [FoodManagment] SET  READ_WRITE 
GO
