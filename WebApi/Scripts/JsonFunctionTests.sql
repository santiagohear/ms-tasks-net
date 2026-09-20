USE [dbTasks];
GO

/* =========================================================
   0) Datos base (usuarios de prueba)
   ========================================================= */
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'ana@empresa.local')
BEGIN
    INSERT INTO dbo.Users (UserName, Email)
    VALUES (N'Ana', N'ana@empresa.local');
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'luis@empresa.local')
BEGIN
    INSERT INTO dbo.Users (UserName, Email)
    VALUES (N'Luis', N'luis@empresa.local');
END;
GO

DECLARE @AnaId  INT = (SELECT UserId FROM dbo.Users WHERE Email = N'ana@empresa.local');
DECLARE @LuisId INT = (SELECT UserId FROM dbo.Users WHERE Email = N'luis@empresa.local');

/* =========================================================
   1) Almacenar información adicional en JSON
   (la tabla ya valida JSON con CK + ISJSON)
   ========================================================= */
INSERT INTO dbo.Tasks
(
    Title,
    Description,
    Status,
    AssignedToUserId,
    CreatedByUserId,
    EstimatedFinishDate,
    AdditionalInfoJson
)
VALUES
(
    N'Definir modelo de datos',
    N'Crear tablas iniciales del sistema',
    N'Pending',
    @AnaId,
    @LuisId,
    '2026-11-10',
    N'{
        "prioridad":"Alta",
        "fechaEstimadaFinalizacion":"2026-11-10",
        "etiquetas":["backend","sql","urgente"],
        "metadata":{"modulo":"core","origen":"manual"}
      }'
),
(
    N'Crear dashboard',
    N'Vista de seguimiento para tareas',
    N'InProgress',
    @AnaId,
    @LuisId,
    '2026-11-20',
    N'{
        "prioridad":"Media",
        "fechaEstimadaFinalizacion":"2026-11-20",
        "etiquetas":["frontend","ux"],
        "metadata":{"modulo":"ui","origen":"jira"}
      }'
);
GO

/* =========================================================
   2) Validar contenido JSON con ISJSON
   ========================================================= */
SELECT
    TaskId,
    Title,
    ISJSON(AdditionalInfoJson) AS IsValidJson
FROM dbo.Tasks
ORDER BY TaskId DESC;
GO

/* =========================================================
   3) Consultar campos JSON (JSON_VALUE / JSON_QUERY)
   ========================================================= */
SELECT
    t.TaskId,
    t.Title,
    JSON_VALUE(t.AdditionalInfoJson, '$.prioridad') AS Prioridad,
    JSON_VALUE(t.AdditionalInfoJson, '$.fechaEstimadaFinalizacion') AS FechaEstimadaFinalizacion,
    JSON_QUERY(t.AdditionalInfoJson, '$.etiquetas') AS EtiquetasJson,
    JSON_QUERY(t.AdditionalInfoJson, '$.metadata') AS MetadataJson
FROM dbo.Tasks AS t
ORDER BY t.CreatedAtUtc DESC;
GO

/* =========================================================
   4) Filtrar tareas por valor dentro del JSON
   - Ejemplo A: prioridad = 'Alta'
   ========================================================= */
SELECT
    t.TaskId,
    t.Title,
    t.Status,
    JSON_VALUE(t.AdditionalInfoJson, '$.prioridad') AS Prioridad
FROM dbo.Tasks AS t
WHERE JSON_VALUE(t.AdditionalInfoJson, '$.prioridad') = N'Alta'
ORDER BY t.CreatedAtUtc DESC;
GO

/* =========================================================
   4.b) Filtrar por etiqueta usando OPENJSON (array)
   ========================================================= */
SELECT DISTINCT
    t.TaskId,
    t.Title,
    t.Status
FROM dbo.Tasks AS t
CROSS APPLY OPENJSON(t.AdditionalInfoJson, '$.etiquetas') AS et
WHERE et.[value] = N'urgente'
ORDER BY t.TaskId DESC;
GO

DECLARE @AnaId  INT = (SELECT UserId FROM dbo.Users WHERE Email = N'ana@empresa.local');


/* =========================================================
   5) Actualizar campo específico dentro del JSON
   (JSON_MODIFY)
   ========================================================= */
UPDATE t
SET AdditionalInfoJson = JSON_MODIFY(t.AdditionalInfoJson, '$.prioridad', N'Crítica')
FROM dbo.Tasks AS t
WHERE t.TaskId = (
    SELECT TOP (1) TaskId
    FROM dbo.Tasks
    WHERE AssignedToUserId = @AnaId
    ORDER BY CreatedAtUtc DESC
);
GO

DECLARE @AnaId  INT = (SELECT UserId FROM dbo.Users WHERE Email = N'ana@empresa.local');


/* Verificación posterior a la actualización */
SELECT
    t.TaskId,
    t.Title,
    JSON_VALUE(t.AdditionalInfoJson, '$.prioridad') AS PrioridadActualizada,
    t.AdditionalInfoJson
FROM dbo.Tasks AS t
WHERE t.AssignedToUserId = @AnaId
ORDER BY t.CreatedAtUtc DESC;
GO

/* =========================================================
   6) OPENJSON con esquema tipado (WITH)
   ========================================================= */
SELECT
    t.TaskId,
    j.prioridad,
    j.fechaEstimadaFinalizacion,
    j.modulo
FROM dbo.Tasks AS t
CROSS APPLY OPENJSON(t.AdditionalInfoJson)
WITH
(
    prioridad NVARCHAR(30) '$.prioridad',
    fechaEstimadaFinalizacion DATE '$.fechaEstimadaFinalizacion',
    modulo NVARCHAR(50) '$.metadata.modulo'
) AS j
ORDER BY t.TaskId DESC;
GO