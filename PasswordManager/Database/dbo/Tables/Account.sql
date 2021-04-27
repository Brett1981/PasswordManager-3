CREATE TABLE [dbo].[Account] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [name]     NVARCHAR (50) NULL,
    [login]    NVARCHAR (50) NULL,
    [password] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([id] ASC)
);

