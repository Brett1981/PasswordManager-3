CREATE TABLE [dbo].[Account] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (50) NULL,
    [login]      NVARCHAR (50) NULL,
    [password]   NVARCHAR (50) NULL,
    [url]        NVARCHAR (50) NULL,
    [categoryId] NVARCHAR (50) NULL,
    [sessionId]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([id] ASC)
);



