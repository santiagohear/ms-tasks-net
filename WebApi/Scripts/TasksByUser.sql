/* ==========================================
   Consulta: tareas por usuario, filtro por estado
   y orden por fecha de creación
   ========================================== */
DECLARE @UserId INT = 1;              -- requerido
DECLARE @Status NVARCHAR(30) = NULL;  -- opcional: 'Pending', 'InProgress', 'Done'

SELECT
    t.TaskId,
    t.Title,
    t.Description,
    t.Status,
    t.AssignedToUserId,
    u.UserName AS AssignedToUserName,
    t.CreatedByUserId,
    t.CreatedAtUtc,
    t.UpdatedAtUtc,
    t.EstimatedFinishDate,
    t.AdditionalInfoJson
FROM dbo.Tasks AS t
INNER JOIN dbo.Users AS u
    ON u.UserId = t.AssignedToUserId
WHERE
    t.AssignedToUserId = @UserId
    AND (@Status IS NULL OR t.Status = @Status)
ORDER BY
    t.CreatedAtUtc DESC; -- más recientes primero
GO