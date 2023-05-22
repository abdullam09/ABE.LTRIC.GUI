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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    CREATE TABLE [Companies] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    CREATE TABLE [Docs] (
        [Id] int NOT NULL IDENTITY,
        [CompanyId] int NOT NULL,
        [DocNumber] nvarchar(max) NULL,
        [PaymentDate] datetime2 NOT NULL,
        [ExpectedDueDate] datetime2 NOT NULL,
        [PrincipleAmount] decimal(18,2) NOT NULL,
        [Comments] nvarchar(max) NULL,
        [IsEnded] bit NOT NULL,
        [IsODEnded] bit NOT NULL,
        [ODDueDate] datetime2 NOT NULL,
        [InsertDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Docs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Docs_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    CREATE TABLE [DocDtls] (
        [Id] int NOT NULL IDENTITY,
        [CompanyId] int NOT NULL,
        [DocId] int NOT NULL,
        [RecordDate] datetime2 NOT NULL,
        [RefundDate] datetime2 NOT NULL,
        [EarlySattleToBank] decimal(18,2) NOT NULL,
        [LTR_Period] int NOT NULL,
        [OD_Period] int NOT NULL,
        [LTR_Intrest] decimal(18,2) NOT NULL,
        [OD_Interest] decimal(18,2) NOT NULL,
        [LTR_Total] decimal(18,2) NOT NULL,
        [OD_Total] decimal(18,2) NOT NULL,
        [Total_Intrest] decimal(18,2) NOT NULL,
        [TotalChargeToBank] decimal(18,2) NOT NULL,
        [TotalChargeToCompany] decimal(18,2) NOT NULL,
        [InsertDate] datetime2 NOT NULL,
        CONSTRAINT [PK_DocDtls] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DocDtls_Docs_DocId] FOREIGN KEY ([DocId]) REFERENCES [Docs] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    CREATE INDEX [IX_DocDtls_DocId] ON [DocDtls] ([DocId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    CREATE INDEX [IX_Docs_CompanyId] ON [Docs] ([CompanyId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106151631_initialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221106151631_initialMigration', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106152616_AddSettings')
BEGIN
    CREATE TABLE [Settings] (
        [Id] int NOT NULL IDENTITY,
        [Key] nvarchar(max) NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_Settings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221106152616_AddSettings')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221106152616_AddSettings', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221129174218_AddRecoveredFromCompany')
BEGIN
    ALTER TABLE [DocDtls] ADD [RecoveredFromCompany] decimal(18,2) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221129174218_AddRecoveredFromCompany')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221129174218_AddRecoveredFromCompany', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221226184855_AddEffictiveDateToSettings')
BEGIN
    ALTER TABLE [Settings] ADD [EffectiveDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221226184855_AddEffictiveDateToSettings')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221226184855_AddEffictiveDateToSettings', N'6.0.10');
END;
GO

COMMIT;
GO

