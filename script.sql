USE [master]
GO
/****** Object:  Database [ex_pract]    Script Date: 30.06.2025 1:57:41 ******/
CREATE DATABASE [ex_pract]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ex_pract', FILENAME = N'D:\ssei\MSSQL16.SQLEXPRESS\MSSQL\DATA\ex_pract.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ex_pract_log', FILENAME = N'D:\ssei\MSSQL16.SQLEXPRESS\MSSQL\DATA\ex_pract_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ex_pract] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ex_pract].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ex_pract] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ex_pract] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ex_pract] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ex_pract] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ex_pract] SET ARITHABORT OFF 
GO
ALTER DATABASE [ex_pract] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ex_pract] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ex_pract] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ex_pract] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ex_pract] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ex_pract] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ex_pract] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ex_pract] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ex_pract] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ex_pract] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ex_pract] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ex_pract] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ex_pract] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ex_pract] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ex_pract] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ex_pract] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ex_pract] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ex_pract] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ex_pract] SET  MULTI_USER 
GO
ALTER DATABASE [ex_pract] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ex_pract] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ex_pract] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ex_pract] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ex_pract] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ex_pract] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ex_pract] SET QUERY_STORE = ON
GO
ALTER DATABASE [ex_pract] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ex_pract]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 30.06.2025 1:57:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 30.06.2025 1:57:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[roleId] [int] NOT NULL,
	[lastlogin] [datetime] NULL,
	[isBlocked] [bit] NOT NULL,
	[loginAttempts] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([id], [name]) VALUES (1, N'Администратор')
INSERT [dbo].[Roles] ([id], [name]) VALUES (2, N'Пользователь')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [login], [password], [roleId], [lastlogin], [isBlocked], [loginAttempts]) VALUES (1, N'admin', N'123', 1, CAST(N'2025-06-30T01:35:58.857' AS DateTime), 0, 0)
INSERT [dbo].[Users] ([id], [login], [password], [roleId], [lastlogin], [isBlocked], [loginAttempts]) VALUES (2, N'zonvi', N'123', 2, CAST(N'2025-06-30T01:19:36.750' AS DateTime), 0, 0)
INSERT [dbo].[Users] ([id], [login], [password], [roleId], [lastlogin], [isBlocked], [loginAttempts]) VALUES (3, N'123', N'123', 2, CAST(N'2025-06-30T01:21:07.890' AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_loginAttempts]  DEFAULT ((0)) FOR [loginAttempts]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_roles] FOREIGN KEY([roleId])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_roles]
GO
USE [master]
GO
ALTER DATABASE [ex_pract] SET  READ_WRITE 
GO
