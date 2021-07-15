USE [master]
GO
/****** Object:  Database [AGENCIA]    Script Date: 27/06/2021 17:00:01 ******/
CREATE DATABASE [AGENCIA]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AGENCIA', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AGENCIA.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AGENCIA_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AGENCIA_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AGENCIA] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AGENCIA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AGENCIA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AGENCIA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AGENCIA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AGENCIA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AGENCIA] SET ARITHABORT OFF 
GO
ALTER DATABASE [AGENCIA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AGENCIA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AGENCIA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AGENCIA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AGENCIA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AGENCIA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AGENCIA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AGENCIA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AGENCIA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AGENCIA] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AGENCIA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AGENCIA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AGENCIA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AGENCIA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AGENCIA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AGENCIA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AGENCIA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AGENCIA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AGENCIA] SET  MULTI_USER 
GO
ALTER DATABASE [AGENCIA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AGENCIA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AGENCIA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AGENCIA] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AGENCIA] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AGENCIA] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AGENCIA] SET QUERY_STORE = OFF
GO
USE [AGENCIA]
GO
/****** Object:  Table [dbo].[ALOJAMIENTO]    Script Date: 27/06/2021 17:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ALOJAMIENTO](
	[CODIGO] [int] NOT NULL,
	[TIPO] [varchar](50) NOT NULL,
	[BARRIO] [varchar](50) NOT NULL,
	[CIUDAD] [varchar](50) NOT NULL,
	[ESTRELLAS] [int] NOT NULL,
	[CANTPERSONAS] [int] NOT NULL,
	[TV] [bit] NOT NULL,
	[PRECIODIA_CABAÑA] [int] NOT NULL,
	[PRECIOPERSONA_HOTEL] [int] NOT NULL,
	[HABITACIONES] [int] NOT NULL,
	[BAÑOS] [int] NOT NULL,
	[NOMBRE] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CABANIA]    Script Date: 27/06/2021 17:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CABANIA](
	[CODIGO] [int] NOT NULL,
	[NOMBRE] [varchar](100) NOT NULL,
	[BARRIO] [varchar](100) NOT NULL,
	[CIUDAD] [varchar](100) NOT NULL,
	[ESTRELLAS] [int] NOT NULL,
	[CANTPERSONAS] [int] NOT NULL,
	[TV] [bit] NOT NULL,
	[PRECIODIA] [float] NOT NULL,
	[HABITACIONES] [int] NOT NULL,
	[BAÑOS] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HOTEL]    Script Date: 27/06/2021 17:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HOTEL](
	[ID] [int] NOT NULL,
	[NOMBRE] [varchar](100) NOT NULL,
	[CIUDAD] [varchar](100) NOT NULL,
	[BARRIO] [varchar](100) NOT NULL,
	[ESTRELLAS] [int] NOT NULL,
	[CANTPERSONAS] [int] NOT NULL,
	[TV] [bit] NOT NULL,
	[PRECIOXPERSONA] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RESERVA]    Script Date: 27/06/2021 17:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RESERVA](
	[ID] [int] NOT NULL,
	[FDESDE] [datetime] NOT NULL,
	[FHASTA] [datetime] NOT NULL,
	[PRECIO] [int] NOT NULL,
	[ID_ALOJAMIENTO] [int] NOT NULL,
	[DNI_USUARIO] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 27/06/2021 17:00:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO](
	[DNI] [int] NOT NULL,
	[NOMBRE] [varchar](100) NOT NULL,
	[MAIL] [varchar](100) NOT NULL,
	[PASSWORD] [varchar](100) NOT NULL,
	[ESADMIN] [bit] NOT NULL,
	[BLOQUEADO] [bit] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[ALOJAMIENTO] ([CODIGO], [TIPO], [BARRIO], [CIUDAD], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIODIA_CABAÑA], [PRECIOPERSONA_HOTEL], [HABITACIONES], [BAÑOS], [NOMBRE]) VALUES (1, N'Cabaña', N'Villa Lynch', N'General San Martin', 3, 5, 1, 1526, 0, 2, 1, N'El Manquito')
GO
INSERT [dbo].[ALOJAMIENTO] ([CODIGO], [TIPO], [BARRIO], [CIUDAD], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIODIA_CABAÑA], [PRECIOPERSONA_HOTEL], [HABITACIONES], [BAÑOS], [NOMBRE]) VALUES (2, N'Hotel', N'Billinghurst', N'General San Martin', 4, 6, 1, 0, 1424, 0, 0, N'Zurdito')
GO
INSERT [dbo].[ALOJAMIENTO] ([CODIGO], [TIPO], [BARRIO], [CIUDAD], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIODIA_CABAÑA], [PRECIOPERSONA_HOTEL], [HABITACIONES], [BAÑOS], [NOMBRE]) VALUES (3, N'Hotel', N'Capital Federal', N'Nuñez', 5, 5, 1, 0, 2000, 0, 0, N'Viper')
GO
INSERT [dbo].[ALOJAMIENTO] ([CODIGO], [TIPO], [BARRIO], [CIUDAD], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIODIA_CABAÑA], [PRECIOPERSONA_HOTEL], [HABITACIONES], [BAÑOS], [NOMBRE]) VALUES (4, N'cabaña', N'prueba', N'prueba', 3, 3, 1, 1234, 1234, 4, 2, N'prueba')
GO
INSERT [dbo].[CABANIA] ([CODIGO], [NOMBRE], [BARRIO], [CIUDAD], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIODIA], [HABITACIONES], [BAÑOS]) VALUES (1, N'El Manquito', N'General San Martin', N'Villa Lynch', 3, 5, 1, 1526, 2, 1)
GO
INSERT [dbo].[HOTEL] ([ID], [NOMBRE], [CIUDAD], [BARRIO], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIOXPERSONA]) VALUES (2, N'Zurdito', N'General San Martin', N'Billinghurst', 4, 6, 1, 1424)
GO
INSERT [dbo].[HOTEL] ([ID], [NOMBRE], [CIUDAD], [BARRIO], [ESTRELLAS], [CANTPERSONAS], [TV], [PRECIOXPERSONA]) VALUES (3, N'Viper', N'Capital Federal', N'Nuñez', 5, 3, 1, 2000)
GO
INSERT [dbo].[RESERVA] ([ID], [FDESDE], [FHASTA], [PRECIO], [ID_ALOJAMIENTO], [DNI_USUARIO]) VALUES (1, CAST(N'2021-06-06T00:00:00.000' AS DateTime), CAST(N'2021-06-25T00:00:00.000' AS DateTime), 1500, 3, 39104528)
GO
INSERT [dbo].[RESERVA] ([ID], [FDESDE], [FHASTA], [PRECIO], [ID_ALOJAMIENTO], [DNI_USUARIO]) VALUES (2, CAST(N'2021-07-06T00:00:00.000' AS DateTime), CAST(N'2021-07-25T00:00:00.000' AS DateTime), 1520, 4, 39449216)
GO
INSERT [dbo].[RESERVA] ([ID], [FDESDE], [FHASTA], [PRECIO], [ID_ALOJAMIENTO], [DNI_USUARIO]) VALUES (3, CAST(N'2021-06-27T00:00:00.000' AS DateTime), CAST(N'2021-06-30T00:00:00.000' AS DateTime), 4272, 2, 123456)
GO
INSERT [dbo].[USUARIO] ([DNI], [NOMBRE], [MAIL], [PASSWORD], [ESADMIN], [BLOQUEADO]) VALUES (39449216, N'Lucas Basualdo', N'basualdo1995@gmail.com', N'123456', 1, 0)
GO
INSERT [dbo].[USUARIO] ([DNI], [NOMBRE], [MAIL], [PASSWORD], [ESADMIN], [BLOQUEADO]) VALUES (123456, N'prueba', N'@', N'12345', 0, 0)
GO
INSERT [dbo].[USUARIO] ([DNI], [NOMBRE], [MAIL], [PASSWORD], [ESADMIN], [BLOQUEADO]) VALUES (38613601, N'leo', N'leo@', N'123456', 0, 0)
GO
INSERT [dbo].[USUARIO] ([DNI], [NOMBRE], [MAIL], [PASSWORD], [ESADMIN], [BLOQUEADO]) VALUES (35957045, N'lolo', N'lolo@', N'123456', 0, 0)
GO
INSERT [dbo].[USUARIO] ([DNI], [NOMBRE], [MAIL], [PASSWORD], [ESADMIN], [BLOQUEADO]) VALUES (39104528, N'Alejo', N'lucas@', N'123456', 0, 0)
GO
USE [master]
GO
ALTER DATABASE [AGENCIA] SET  READ_WRITE 
GO