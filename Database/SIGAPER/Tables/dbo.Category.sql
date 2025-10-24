USE [SIGAPER]
-------------------------------------------
-- User: Briangel.Abreu					--
-- Date: 2025-10-24						--
-- Description: Create Category table	--
------------------------------------------
CREATE TABLE Category (
	ID			INT				IDENTITY(1,1) PRIMARY KEY,
	UserId		CHAR(26)		NOT NULL,
	Name		NVARCHAR(255)	NOT NULL,
	Type		VARCHAR(20)		NOT NULL,
	CreatedAt	DATETIME		CONSTRAINT DF_Category_CreatedAt DEFAULT GETDATE(),
	CONSTRAINT FK_Category_User FOREIGN KEY (UserId)
	REFERENCES dbo.[User](ID)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)