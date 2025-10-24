USE [SIGAPER]

------------------------------------------
-- User: Briangel.Abreu					--
-- Date: 2025-10-24						--
-- Description: Create User table		--
------------------------------------------

CREATE TABLE dbo.[User] (
    ID           CHAR(26)       PRIMARY KEY,
    Name         VARCHAR(100)   NOT NULL,
    Email        VARCHAR(100)   NOT NULL,
    PasswordHash NVARCHAR(255)  NOT NULL,
    Phone        VARCHAR(20),
    Currency     VARCHAR(10)    CONSTRAINT DF_User_Currency DEFAULT 'USD',
    Language     VARCHAR(20)    CONSTRAINT DF_User_Language DEFAULT 'English',
    Avatar       VARCHAR(100),
    IsVerified   BIT            CONSTRAINT DF_User_IsVerified DEFAULT 0 NOT NULL,
    CreatedAt    DATETIME       CONSTRAINT DF_User_CreatedAt DEFAULT GETDATE() NOT NULL,
    UpdatedAt    DATETIME       CONSTRAINT DF_User_UpdatedAt DEFAULT GETDATE()
);