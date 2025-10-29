USE [GastosIngresos]

--------------------------------------------------
-- User: Briangel.Abreu							--
-- Date: 2025-10-24								--
-- Description: Create UserVerification table	--
--------------------------------------------------

CREATE TABLE dbo.[UserVerification] (
	ID			CHAR(26)		PRIMARY KEY,
	UserId		CHAR(26)		NOT NULL,
	Code		CHAR(10),
	ExpiresAt	DATETIME		CONSTRAINT DF_UserVerification_EspiresAt DEFAULT GETDATE(),
	Used		BIT				CONSTRAINT DF_UserVerification_Used DEFAULT 0 NOT NULL,
	CreatedAt	DATETIME		CONSTRAINT DF_UserVerification_CreateAt DEFAULT GETDATE() NOT NULL,
	CONSTRAINT FK_UserVerification_User FOREIGN KEY (UserId)
		REFERENCES dbo.[User](ID)
		ON UPDATE CASCADE
		ON DELETE CASCADE
)