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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [VehicleVINNumber] nvarchar(17) NOT NULL,
        [VehicleMakeModel] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Customer_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE TABLE [GeoLocations] (
        [Id] int NOT NULL IDENTITY,
        [Latitude] float NOT NULL,
        [Longitude] float NOT NULL,
        [Address] nvarchar(250) NOT NULL,
        CONSTRAINT [PK_GeoLocation_Id] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE TABLE [Assistants] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(100) NOT NULL,
        [CurrentGeoLocationId] int NOT NULL,
        [IsReserved] bit NOT NULL,
        CONSTRAINT [PK_Assistant_Id] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Assistant_GeoLocation_CurrentGeoLocationId] FOREIGN KEY ([CurrentGeoLocationId]) REFERENCES [GeoLocations] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE TABLE [CustomerAssistants] (
        [Id] int NOT NULL IDENTITY,
        [CustomerId] int NOT NULL,
        [AssistantId] int NOT NULL,
        [GeoLocationId] int NOT NULL,
        [ServiceStartDate] datetimeoffset NOT NULL,
        [ServiceCompleteDate] datetimeoffset NULL,
        [Status] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_CustomerAssistant_Id] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CustomerAssistant_Assistant_AssistantId] FOREIGN KEY ([AssistantId]) REFERENCES [Assistants] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerAssistant_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerAssistant_GeoLocation_GeoLocationId] FOREIGN KEY ([GeoLocationId]) REFERENCES [GeoLocations] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE INDEX [IX_Assistants_CurrentGeoLocationId] ON [Assistants] ([CurrentGeoLocationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE INDEX [IX_CustomerAssistants_AssistantId] ON [CustomerAssistants] ([AssistantId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE INDEX [IX_CustomerAssistants_CustomerId] ON [CustomerAssistants] ([CustomerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE INDEX [IX_CustomerAssistants_GeoLocationId] ON [CustomerAssistants] ([GeoLocationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    CREATE UNIQUE INDEX [UC_GeoLocation_Longitude_Latitude] ON [GeoLocations] ([Longitude], [Latitude]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230524034201_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230524034201_init', N'7.0.5');
END;
GO

COMMIT;
GO

