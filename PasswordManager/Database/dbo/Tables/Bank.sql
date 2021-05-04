CREATE TABLE [dbo].[Bank] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [numberCard] NVARCHAR (50) NOT NULL,
    [name]       NVARCHAR (50) NOT NULL,
    [date]       NVARCHAR (50) NOT NULL,
    [cvc]        NVARCHAR (50) NOT NULL,
    [sessionId]  INT           NOT NULL,
    CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED ([id] ASC)
);



