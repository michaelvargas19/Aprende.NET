CREATE TABLE [dbo].[Operation] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [IdOperation] UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (50)     NOT NULL,
    [Status]      BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdOperation]) REFERENCES [dbo].[Calculator] ([Id])
);

