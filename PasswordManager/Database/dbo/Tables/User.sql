CREATE TABLE [dbo].[User] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [username] NVARCHAR (50) NOT NULL,
    [password] VARCHAR (100) NOT NULL,
    [email]    NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([id] ASC)
);







