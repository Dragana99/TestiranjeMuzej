USE [master]
GO
/****** Object:  Database [Museum]    Script Date: 3/21/2022 11:59:34 AM ******/
CREATE DATABASE [Museum]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Museum', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Museum.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Museum_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Museum_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Museum] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Museum].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Museum] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Museum] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Museum] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Museum] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Museum] SET ARITHABORT OFF 
GO
ALTER DATABASE [Museum] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Museum] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Museum] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Museum] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Museum] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Museum] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Museum] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Museum] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Museum] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Museum] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Museum] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Museum] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Museum] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Museum] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Museum] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Museum] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Museum] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Museum] SET RECOVERY FULL 
GO
ALTER DATABASE [Museum] SET  MULTI_USER 
GO
ALTER DATABASE [Museum] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Museum] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Museum] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Museum] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Museum] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Museum] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Museum', N'ON'
GO
ALTER DATABASE [Museum] SET QUERY_STORE = OFF
GO
USE [Museum]
GO
/****** Object:  Table [dbo].[Auditorium]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auditorium](
	[auditoriumId] [int] IDENTITY(1,1) NOT NULL,
	[auditoriumName] [nchar](30) NOT NULL,
	[museumId] [int] NOT NULL,
 CONSTRAINT [PK_Auditorium] PRIMARY KEY CLUSTERED 
(
	[auditoriumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exhabit]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exhabit](
	[exhabitId] [uniqueidentifier] NOT NULL,
	[exhabitName] [nchar](30) NOT NULL,
	[picturePath] [nchar](250) NOT NULL,
	[year] [int] NOT NULL,
 CONSTRAINT [PK_Exhabit] PRIMARY KEY CLUSTERED 
(
	[exhabitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exhibition]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exhibition](
	[exhibitionId] [uniqueidentifier] NOT NULL,
	[exhibitionName] [nchar](30) NOT NULL,
	[exhibitionTime] [datetime] NOT NULL,
	[description] [nchar](100) NULL,
	[imagePath] [nchar](250) NOT NULL,
	[auditoriumId] [int] NOT NULL,
	[exhabitId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Exhibition] PRIMARY KEY CLUSTERED 
(
	[exhibitionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Museum]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Museum](
	[museumId] [int] IDENTITY(1,1) NOT NULL,
	[museumName] [nchar](20) NOT NULL,
	[streetAddress] [nchar](30) NOT NULL,
	[city] [nchar](30) NOT NULL,
	[email] [nchar](30) NOT NULL,
	[phoneNumber] [nchar](30) NOT NULL,
 CONSTRAINT [PK_Museum] PRIMARY KEY CLUSTERED 
(
	[museumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[ticketId] [uniqueidentifier] NOT NULL,
	[userId] [uniqueidentifier] NOT NULL,
	[price] [float] NOT NULL,
	[exhibitionId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[ticketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/21/2022 11:59:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [uniqueidentifier] NOT NULL,
	[firstName] [nchar](30) NOT NULL,
	[lastName] [nchar](30) NOT NULL,
	[username] [nchar](30) NOT NULL,
	[role] [nchar](20) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditorium]  WITH CHECK ADD  CONSTRAINT [FK_Auditorium_Museum] FOREIGN KEY([museumId])
REFERENCES [dbo].[Museum] ([museumId])
GO
ALTER TABLE [dbo].[Auditorium] CHECK CONSTRAINT [FK_Auditorium_Museum]
GO
ALTER TABLE [dbo].[Exhibition]  WITH CHECK ADD  CONSTRAINT [FK_Exhibition_Auditorium] FOREIGN KEY([auditoriumId])
REFERENCES [dbo].[Auditorium] ([auditoriumId])
GO
ALTER TABLE [dbo].[Exhibition] CHECK CONSTRAINT [FK_Exhibition_Auditorium]
GO
ALTER TABLE [dbo].[Exhibition]  WITH CHECK ADD  CONSTRAINT [FK_Exhibition_Exhabit] FOREIGN KEY([exhabitId])
REFERENCES [dbo].[Exhabit] ([exhabitId])
GO
ALTER TABLE [dbo].[Exhibition] CHECK CONSTRAINT [FK_Exhibition_Exhabit]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Exhibition] FOREIGN KEY([exhibitionId])
REFERENCES [dbo].[Exhibition] ([exhibitionId])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_Exhibition]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_User]
GO
USE [master]
GO
ALTER DATABASE [Museum] SET  READ_WRITE 
GO
