USE [GastosIngresos]

----------------------------------------------
-- User: Briangel.Abreu						--
-- Date: 2025-10-24							--
-- Description: Create RefreshToken Table	--
----------------------------------------------

CREATE TABLE dbo.[RefreshToken] (
	ID			CHAR(26)		PRIMARY KEY,
	UserId		CHAR(26)		NOT NULL,
	JwtId		VARCHAR(100)	NOT NULL,
	ExpiresAt	DATETIME,
	Revoked		BIT				CONSTRAINT DF_RefreshToken_Revoked DEFAULT 0,
	CreatedAt	DATETIME		CONSTRAINT DF_RefreshToken_CreatedAt DEFAULT GETDATE(),
	CONSTRAINT FK_RefreshToken_User FOREIGN KEY (UserId)
		REFERENCES dbo.[User](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
)