# ms-tasks-net

Microservicio de gestión de tareas construido con .NET 10 y una arquitectura en capas inspirada en Clean Architecture. Actualmente expone operaciones para usuarios y tareas, utiliza SQL Server para persistencia y organiza la lógica en proyectos separados para dominio, aplicación, infraestructura y API.

## Objetivo

Este servicio permite:

- crear usuarios
- consultar usuarios registrados
- crear tareas
- consultar tareas
- actualizar el estado de una tarea

## Arquitectura

La solución está compuesta por los siguientes proyectos:

- `Domain`
  - entidades de negocio
  - reglas de dominio
  - interfaces de repositorio y servicios
  - excepciones de dominio
- `Application`
  - comandos y consultas con MediatR
  - handlers
  - DTOs
  - perfiles de AutoMapper
- `Infrastructure`
  - Entity Framework Core
  - contexto de persistencia
  - repositorios
  - unidad de trabajo
  - filtros compartidos
  - extensiones de registro de dependencias
- `WebApi`
  - endpoints HTTP
  - validaciones con FluentValidation
  - Swagger
- `Domain.Test`
  - pruebas unitarias de dominio
- `WebApi.Integration.Test`
  - pruebas de integración/controladores

## Flujo general

1. La API recibe la solicitud HTTP.
2. FluentValidation valida el request.
3. El controlador envía un comando o query con MediatR.
4. El handler coordina el caso de uso.
5. El dominio aplica reglas de negocio.
6. Infraestructura persiste o consulta información en SQL Server.
7. La API responde con el resultado correspondiente.

## Tecnologías principales

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- MediatR
- AutoMapper
- FluentValidation
- Swagger / Swashbuckle
- xUnit
- NSubstitute

## Estructura de carpetas

```text
ms-tasks-net/
├── Application/
│   ├── Tasks/
│   └── Users/
├── Domain/
│   ├── Entities/
│   ├── Exceptions/
│   ├── Ports/
│   └── Services/
├── Infrastructure/
│   ├── Adapters/
│   ├── Configuration/
│   ├── DataSource/
│   └── Extensions/
├── WebApi/
│   ├── Base/
│   ├── Contracts/
│   ├── Controllers/
│   └── Validators/
├── Domain.Test/
└── WebApi.Integration.Test/
```

## Modelo funcional

### Usuarios

Campos principales:

- `UserName`
- `Email`

Reglas actuales:

- el nombre es obligatorio
- el correo es obligatorio
- no se permite crear usuarios con correos duplicados

### Tareas

Campos principales:

- `Title`
- `Description`
- `AssignedToUserId`
- `CreatedByUserId`
- `EstimatedFinishDate`
- `AdditionalInfoJson`
- `Status`

Estados permitidos:

- `Pending`
- `InProgress`
- `Done`

Reglas actuales:

- el título es obligatorio
- el usuario asignado debe existir
- el usuario creador debe existir
- si no se informa estado al crear, se asigna `Pending`
- no se permite pasar directamente de `Pending` a `Done`

## Endpoints disponibles

### Usuarios

#### Crear usuario

`POST /api/users`

Ejemplo de body:

```json
{
  "userName": "Santiago",
  "email": "santiago@example.com"
}
```

#### Consultar usuarios

`GET /api/users`

### Tareas

#### Crear tarea

`POST /api/tasks`

Ejemplo de body:

```json
{
  "title": "Preparar entrega",
  "description": "Validar alcance y tiempos",
  "assignedToUserId": 1,
  "createdByUserId": 1,
  "estimatedFinishDate": "2025-12-20T00:00:00Z",
  "additionalInfoJson": "{\"priority\":\"high\"}"
}
```

#### Consultar tareas

`GET /api/tasks`

#### Actualizar estado de tarea

`PUT /api/tasks/{id}/status`

Ejemplo de body:

```json
{
  "status": "InProgress"
}
```

## Validación y manejo de errores

La solución utiliza:

- `FluentValidation` para validar requests
- `ValidationFilter` para ejecutar validaciones marcadas con el atributo `Validate`
- `AppExceptionFilterAttribute` para transformar excepciones de dominio en respuestas HTTP

Comportamiento actual:

- `ValidationException` → `400 Bad Request`
- `NotFoundException` → `404 Not Found`
- excepciones no controladas → `500 Internal Server Error`

## Persistencia

La persistencia está implementada con Entity Framework Core sobre SQL Server.

Configuraciones relevantes:

- `ConnectionStrings:Base`
- `ConnectionStrings:BaseSchema`

El contexto principal es `PersistenceContext` y las configuraciones de entidades viven en:

- `Infrastructure/DataSource/Configurations/TaskItemEntityConfiguration.cs`
- `Infrastructure/DataSource/Configurations/UserEntityConfiguration.cs`

## Configuración

Archivo principal:

- `WebApi/appsettings.json`

Archivo para desarrollo:

- `WebApi/appsettings.Development.json`

Valores importantes:

- `PathBase`: prefijo base de la aplicación
- `ConnectionStrings:Base`: cadena de conexión a SQL Server
- `ConnectionStrings:BaseSchema`: esquema de base de datos

## Swagger

Swagger está habilitado en la API.

La ruta depende del `PathBase` configurado:

- sin `PathBase`: `/swagger`
- con `PathBase=/tasksapi`: `/tasksapi/swagger`

## Cómo ejecutar localmente

### 1. Restaurar paquetes

```bash
dotnet restore ms-tasks-net.slnx
```

### 2. Compilar

```bash
dotnet build ms-tasks-net.slnx
```

### 3. Ejecutar la API

```bash
dotnet run --project WebApi/WebApi.csproj
```

## Cómo ejecutar pruebas

### Todas las pruebas

```bash
dotnet test ms-tasks-net.slnx
```

### Solo pruebas de dominio

```bash
dotnet test Domain.Test/Domain.Test.csproj
```

### Solo pruebas de API

```bash
dotnet test WebApi.Integration.Test/WebApi.Integration.Test.csproj
```
