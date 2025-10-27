USE [GastosIngresos]
----------------------------------------------
-- User: Briangel.Abreu						--
-- Date: 2025-10-24							--
-- Description: Create Transaction table	--
----------------------------------------------
CREATE TABLE dbo.[Transaction] (
	ID				INT				IDENTITY(1,1) PRIMARY KEY,
	CategoryId		INT				NOT NULL,
	UserId			CHAR(26)		NOT NULL,
	Name			VARCHAR(50)		NOT NULL,
	Description		NVARCHAR(255),
	CreatedAt		DATETIME		CONSTRAINT DF_Transaction_CreateAt DEFAULT GETDATE(),

	CONSTRAINT FK_Transaction_Category
		FOREIGN KEY (CategoryId) REFERENCES dbo.[Category](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE,

	CONSTRAINT FK_Transaction_User
		FOREIGN KEY (UserId) REFERENCES dbo.[User](ID)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
)