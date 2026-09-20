/* =========================================================
   Modelo base para sistema de gestión de tareas
   Base de datos objetivo: dbTasks
   ========================================================= */

/* Crear base de datos si no existe */
IF DB_ID(N'dbTasks') IS NULL
BEGIN
    CREATE DATABASE [dbTasks];
END;
GO

USE [dbTasks];
GO

/* Limpieza opcional (solo para entornos de desarrollo/pruebas) */
IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL
    DROP TABLE dbo.Tasks;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

/* =========================
   Tabla: Users
   ========================= */
CREATE TABLE dbo.Users
(
    UserId           INT IDENTITY(1,1) NOT NULL,
    UserName         NVARCHAR(100)     NOT NULL,
    Email            NVARCHAR(255)     NOT NULL,
    IsActive         BIT               NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    CreatedAtUtc     DATETIME2(0)      NOT NULL CONSTRAINT DF_Users_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserId),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO

/* =========================
   Tabla: Tasks
   ========================= */
CREATE TABLE dbo.Tasks
(
    TaskId                 BIGINT IDENTITY(1,1) NOT NULL,
    Title                  NVARCHAR(200)        NOT NULL,
    Description            NVARCHAR(2000)       NULL,
    Status                 NVARCHAR(30)         NOT NULL CONSTRAINT DF_Tasks_Status DEFAULT (N'Pendiente'),
    AssignedToUserId       INT                  NOT NULL,
    CreatedByUserId        INT                  NOT NULL,
    CreatedAtUtc           DATETIME2(0)         NOT NULL CONSTRAINT DF_Tasks_CreatedAtUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedAtUtc           DATETIME2(0)         NULL,
    EstimatedFinishDate    DATE                 NULL,

    -- Columna solicitada para información adicional en JSON
    AdditionalInfoJson     NVARCHAR(MAX)        NULL,

    CONSTRAINT PK_Tasks PRIMARY KEY CLUSTERED (TaskId),

    CONSTRAINT FK_Tasks_AssignedToUser
        FOREIGN KEY (AssignedToUserId) REFERENCES dbo.Users (UserId),

    CONSTRAINT FK_Tasks_CreatedByUser
        FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users (UserId),

    CONSTRAINT CK_Tasks_Status
        CHECK (Status IN (N'Pending', N'InProgress', N'Done')),

    -- Si hay contenido en AdditionalInfoJson, debe ser JSON válido
    CONSTRAINT CK_Tasks_AdditionalInfoJson_IsJson
        CHECK (AdditionalInfoJson IS NULL OR ISJSON(AdditionalInfoJson) = 1)
);
GO

/* =========================
   Índices
   ========================= */

-- Índice para búsquedas de tareas por asignado y estado
CREATE NONCLUSTERED INDEX IX_Tasks_AssignedToUserId_Status
ON dbo.Tasks (AssignedToUserId, Status)
INCLUDE (EstimatedFinishDate, CreatedAtUtc);
GO

-- Índice para filtrar/ordenar tareas por fecha estimada
CREATE NONCLUSTERED INDEX IX_Tasks_EstimatedFinishDate
ON dbo.Tasks (EstimatedFinishDate);
GO