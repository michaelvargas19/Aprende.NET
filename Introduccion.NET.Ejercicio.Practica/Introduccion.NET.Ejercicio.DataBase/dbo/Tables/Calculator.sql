CREATE TABLE [dbo].[Calculator] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Name]   VARCHAR (50)     NOT NULL,
    [Status] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

