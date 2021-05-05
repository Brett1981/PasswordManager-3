CREATE TABLE [dbo].[Account] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (50) NULL,
    [login]      NVARCHAR (50) NULL,
    [password]   NVARCHAR (50) NULL,
    [url]        NVARCHAR (50) NULL,
    [categoryId] INT           NULL,
    [sessionId]  INT           NOT NULL,
    [CreatedAt]  DATE          CONSTRAINT [DF_Account_CreatedAt] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Account_Category] FOREIGN KEY ([categoryId]) REFERENCES [dbo].[Category] ([id]),
    CONSTRAINT [FK_Account_User] FOREIGN KEY ([sessionId]) REFERENCES [dbo].[User] ([id])
);







