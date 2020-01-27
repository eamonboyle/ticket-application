-- Warning: You can generate script only for two tables at a time in your Free plan 
-- 
-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[AccountToCompany]
CREATE TABLE [dbo].[AccountToCompany] (
	[account_id] INT NULL
	,[company_id] INT NULL
	);
GO

-- ************************************** [dbo].[Account]
CREATE TABLE [dbo].[Account] (
	[id] INT NOT NULL
	,[creditor] INT NULL
	,[account_ref] VARCHAR(255) NULL
	,[amount_owed] DECIMAL(18, 2) NULL
	,[status] INT NULL
	,[joint] TINYINT NULL
	,[secured] TINYINT NULL
	,[accepting_dividends] TINYINT NULL
	,CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[AlertType]
CREATE TABLE [dbo].[AlertType] (
	[id] INT NOT NULL
	,[type] VARCHAR(255) NULL
	,CONSTRAINT [PK_AlertType] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ************************************** [dbo].[Alert]
CREATE TABLE [dbo].[Alert] (
	[id] INT NOT NULL
	,[buyer_id] INT NULL
	,[sale_id] INT NULL
	,[type] INT NULL
	,[frequency] INT NULL
	,[created_by] INT NULL
	,[start_date] DATETIME NULL
	,[date_created] DATETIME NULL
	,CONSTRAINT [PK_Alert] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[Creditor]
CREATE TABLE [dbo].[Creditor] (
	[id] INT NOT NULL
	,[name] VARCHAR(255) NULL
	,[address1] VARCHAR(255) NULL
	,[city] VARCHAR(255) NULL
	,[postcode] VARCHAR(255) NULL
	,[email] VARCHAR(255) NULL
	,CONSTRAINT [PK_Creditor] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ************************************** [dbo].[Company]
CREATE TABLE [dbo].[Company] (
	[id] INT NOT NULL
	,[name] VARCHAR(255) NULL
	,[address1] VARCHAR(255) NULL
	,[city] VARCHAR(255) NULL
	,[postcode] VARCHAR(255) NULL
	,CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[RoleLevel]
CREATE TABLE [dbo].[RoleLevel] (
	[id] INT NOT NULL
	,[level] VARCHAR(255) NULL
	,CONSTRAINT [PK_RoleLevel] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ************************************** [dbo].[Role]
CREATE TABLE [dbo].[Role] (
	[id] INT NOT NULL
	,[title] VARCHAR(255) NULL
	,CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[Ticket]
CREATE TABLE [dbo].[Ticket] (
	[id] INT NOT NULL
	,[user] INT NULL
	,[company_id] INT NULL
	,[title] VARCHAR(255) NULL
	,[type] INT NULL
	,[date_created] DATETIME NULL
	,[date_closed] DATETIME NULL
	,CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ************************************** [dbo].[Sale]
CREATE TABLE [dbo].[Sale] (
	[id] INT NOT NULL
	,[account_id] INT NULL
	,[price] DECIMAL(18, 2) NULL
	,[buyer] INT NULL
	,[seller] INT NULL
	,[purchase_date] DATETIME NULL
	,CONSTRAINT [PK_Sale] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[Tranche]
CREATE TABLE [dbo].[Tranche] (
	[id] INT NOT NULL
	,[owner] INT NULL
	,[amount] DECIMAL(18, 2) NULL
	,CONSTRAINT [PK_Tranche] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ************************************** [dbo].[TicketType]
CREATE TABLE [dbo].[TicketType] (
	[id] INT NOT NULL
	,[type] VARCHAR(255) NULL
	,CONSTRAINT [PK_TicketType] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************
-- ************************************** [dbo].[User]
CREATE TABLE [dbo].[User] (
	[id] INT NOT NULL
	,[first_name] VARCHAR(255) NULL
	,[last_name] VARCHAR(255) NULL
	,[email] VARCHAR(255) NULL
	,[phone_number] VARCHAR(255) NULL
	,[mobile_number] VARCHAR(255) NULL
	,[company] INT NULL
	,[role] INT NULL
	,[role_level] INT NULL
	,[username] VARCHAR(255) NULL
	,[password] VARCHAR(255) NULL
	,[salt] VARCHAR(255) NULL
	,[enabled] TINYINT NULL
	,[date_created] DATETIME NULL
	,CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([id] ASC)
	);
GO