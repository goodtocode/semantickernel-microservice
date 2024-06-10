IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'Microservices') IS NULL EXEC(N'CREATE SCHEMA [Microservices];');
GO

CREATE TABLE [Microservices].[Associate] (
    [RowKey] uniqueidentifier NOT NULL,
    [PartitionKey] nvarchar(max) NOT NULL,
    [AssociateKey] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Associate] PRIMARY KEY ([RowKey])
);
GO

CREATE TABLE [Microservices].[Business] (
    [RowKey] uniqueidentifier NOT NULL,
    [PartitionKey] nvarchar(max) NOT NULL,
    [BusinessKey] uniqueidentifier NOT NULL,
    [BusinessName] nvarchar(200) NOT NULL,
    [TaxNumber] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY ([RowKey])
);
GO

CREATE TABLE [Microservices].[Gender] (
    [RowKey] uniqueidentifier NOT NULL,
    [PartitionKey] nvarchar(max) NOT NULL,
    [GenderKey] uniqueidentifier NOT NULL,
    [GenderName] nvarchar(50) NOT NULL,
    [GenderCode] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_Gender] PRIMARY KEY ([RowKey]),
    CONSTRAINT [CC_Gender_GenderCode] CHECK (GenderCode in ('M', 'F', 'N/A', 'U/K'))
);
GO

CREATE TABLE [Microservices].[Government] (
    [RowKey] uniqueidentifier NOT NULL,
    [PartitionKey] nvarchar(max) NOT NULL,
    [GovernmentKey] uniqueidentifier NOT NULL,
    [GovernmentName] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Government] PRIMARY KEY ([RowKey])
);
GO

CREATE TABLE [Microservices].[Person] (
    [RowKey] uniqueidentifier NOT NULL,
    [PartitionKey] nvarchar(max) NOT NULL,
    [PersonKey] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [MiddleName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [BirthDate] datetime NOT NULL,
    [GenderCode] nvarchar(3) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY ([RowKey]),
    CONSTRAINT [CC_Person_GenderCode] CHECK (GenderCode in ('M', 'F', 'N/A', 'U/K'))
);
GO

CREATE UNIQUE INDEX [IX_AssociateLocation_Associate] ON [Microservices].[Associate] ([AssociateKey]);
GO

CREATE UNIQUE INDEX [IX_Business_Key] ON [Microservices].[Business] ([BusinessKey]);
GO

CREATE UNIQUE INDEX [IX_Gender_Code] ON [Microservices].[Gender] ([GenderCode]);
GO

CREATE UNIQUE INDEX [IX_Gender_Key] ON [Microservices].[Gender] ([GenderKey]);
GO

CREATE UNIQUE INDEX [IX_Government_Associate] ON [Microservices].[Government] ([GovernmentKey]);
GO

CREATE INDEX [IX_Person_All] ON [Microservices].[Person] ([FirstName], [MiddleName], [LastName], [BirthDate]);
GO

CREATE UNIQUE INDEX [IX_Person_Associate] ON [Microservices].[Person] ([PersonKey]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220221204708_InitialCreate', N'6.0.2');
GO

COMMIT;
GO