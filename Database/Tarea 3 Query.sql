USE [ProductDB];
GO
-- Run on the existing database after backing it up. No rows are deleted/re-numbered.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.UserActions', N'U') IS NULL
        THROW 50001, 'Falta la tabla UserActions.', 1;
    IF COL_LENGTH(N'dbo.UserActions', N'CrudId') IS NULL
    BEGIN
        IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.UserActions') AND type = 'PK')
            THROW 50002, 'UserActions ya tiene otra PK; revise el esquema antes de aplicar.', 1;
        EXEC(N'ALTER TABLE dbo.UserActions ADD CrudId int IDENTITY(1,1) NOT NULL');
        EXEC(N'ALTER TABLE dbo.UserActions ADD CONSTRAINT PK_UserActions_CrudId PRIMARY KEY (CrudId)');
    END
    ELSE
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM sys.columns c
            JOIN sys.index_columns ic ON ic.object_id=c.object_id AND ic.column_id=c.column_id
            JOIN sys.indexes i ON i.object_id=ic.object_id AND i.index_id=ic.index_id
            WHERE c.object_id=OBJECT_ID(N'dbo.UserActions') AND c.name=N'CrudId'
            AND c.system_type_id=56 AND c.is_nullable=0 AND c.is_identity=1 AND i.is_primary_key=1
            AND (SELECT COUNT(*) FROM sys.index_columns kc WHERE kc.object_id=i.object_id AND kc.index_id=i.index_id AND kc.key_ordinal>0)=1)
            THROW 50003, 'CrudId en UserActions no coincide con la PK identity esperada.', 1;
    END

    IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NULL
        THROW 50001, 'Falta la tabla UserRoles.', 1;
    IF COL_LENGTH(N'dbo.UserRoles', N'CrudId') IS NULL
    BEGIN
        IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.UserRoles') AND type = 'PK')
            THROW 50002, 'UserRoles ya tiene otra PK; revise el esquema antes de aplicar.', 1;
        EXEC(N'ALTER TABLE dbo.UserRoles ADD CrudId int IDENTITY(1,1) NOT NULL');
        EXEC(N'ALTER TABLE dbo.UserRoles ADD CONSTRAINT PK_UserRoles_CrudId PRIMARY KEY (CrudId)');
    END
    ELSE
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM sys.columns c
            JOIN sys.index_columns ic ON ic.object_id=c.object_id AND ic.column_id=c.column_id
            JOIN sys.indexes i ON i.object_id=ic.object_id AND i.index_id=ic.index_id
            WHERE c.object_id=OBJECT_ID(N'dbo.UserRoles') AND c.name=N'CrudId'
            AND c.system_type_id=56 AND c.is_nullable=0 AND c.is_identity=1 AND i.is_primary_key=1
            AND (SELECT COUNT(*) FROM sys.index_columns kc WHERE kc.object_id=i.object_id AND kc.index_id=i.index_id AND kc.key_ordinal>0)=1)
            THROW 50003, 'CrudId en UserRoles no coincide con la PK identity esperada.', 1;
    END

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
SELECT TOP (10) * FROM dbo.UserActions ORDER BY CrudId;
SELECT TOP (10) * FROM dbo.UserRoles ORDER BY CrudId;
