# 🏆 Sports League API

API REST para gestión de liga deportiva desarrollada con .NET 8, C#, Entity Framework Core y SQL Server.

## Stack Tecnológico

- **.NET 8** | **C#** | **Entity Framework Core 8** | **SQL Server**
- **AutoMapper** | **Swagger** | **Arquitectura N-Capas**

## Estructura del Proyecto

```
SportsLeague/
├── SportsLeague.sln
├── SportsLeague.API/          ← Presentación (Controllers, DTOs, Mappings)
├── SportsLeague.Domain/       ← Negocio (Entities, Enums, Interfaces, Services)
└── SportsLeague.DataAccess/   ← Datos (DbContext, Repositories, Migrations)
```

## Requisitos Previos

- .NET 8 SDK
- SQL Server (Express, Developer o LocalDB)
- Visual Studio 2022 o VS Code

## Configuración Inicial

### 1. Restaurar paquetes NuGet
```bash
dotnet restore
```

### 2. Configurar cadena de conexión
Editar `SportsLeague.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SportsLeagueDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```
> Si usas usuario/contraseña: `Server=localhost;Database=SportsLeagueDb;User Id=sa;Password=TuPassword;TrustServerCertificate=true;`

### 3. Crear y aplicar migraciones
```bash
# Desde la raíz de la solución:
dotnet ef migrations add InitialCreate --project SportsLeague.DataAccess --startup-project SportsLeague.API
dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API

# O desde Package Manager Console en Visual Studio:
# Add-Migration InitialCreate
# Update-Database
```

### 4. Ejecutar la API
```bash
cd SportsLeague.API
dotnet run
```
Abrir: http://localhost:5011/swagger

---

## Endpoints Disponibles

### 🟢 Teams (Fase 1)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/Team | Listar todos los equipos |
| GET | /api/Team/{id} | Obtener equipo por ID |
| POST | /api/Team | Crear equipo |
| PUT | /api/Team/{id} | Actualizar equipo |
| DELETE | /api/Team/{id} | Eliminar equipo |

### 🟡 Players (Fase 2)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/Player | Listar todos los jugadores |
| GET | /api/Player/{id} | Obtener jugador por ID |
| GET | /api/Player/by-team/{teamId} | Jugadores de un equipo |
| POST | /api/Player | Crear jugador |
| PUT | /api/Player/{id} | Actualizar jugador |
| DELETE | /api/Player/{id} | Eliminar jugador |

### 🔵 Referees (Fase 3)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/Referee | Listar árbitros |
| GET | /api/Referee/{id} | Obtener árbitro por ID |
| POST | /api/Referee | Crear árbitro |
| PUT | /api/Referee/{id} | Actualizar árbitro |
| DELETE | /api/Referee/{id} | Eliminar árbitro |

### 🏆 Tournaments (Fase 3)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/Tournament | Listar torneos |
| GET | /api/Tournament/{id} | Obtener torneo (incluye equipos) |
| POST | /api/Tournament | Crear torneo |
| PUT | /api/Tournament/{id} | Actualizar torneo |
| DELETE | /api/Tournament/{id} | Eliminar torneo |
| PATCH | /api/Tournament/{id}/status | Cambiar estado |
| POST | /api/Tournament/{id}/teams | Inscribir equipo |
| GET | /api/Tournament/{id}/teams | Equipos inscritos |

---

## Datos de Prueba (Swagger)

### Crear equipo:
```json
POST /api/Team
{
  "name": "Atlético Nacional",
  "city": "Medellín",
  "stadium": "Atanasio Girardot",
  "logoUrl": "https://example.com/nacional.png",
  "foundedDate": "1947-03-07"
}
```

### Crear jugador:
```json
POST /api/Player
{
  "firstName": "Carlos",
  "lastName": "Valdés",
  "birthDate": "1984-02-17",
  "number": 5,
  "position": 1,
  "teamId": 1
}
```
> Posiciones: 0=Goalkeeper, 1=Defender, 2=Midfielder, 3=Forward

### Crear árbitro:
```json
POST /api/Referee
{
  "firstName": "Wilmar",
  "lastName": "Roldán",
  "nationality": "Colombiana"
}
```

### Crear torneo:
```json
POST /api/Tournament
{
  "name": "Liga BetPlay",
  "season": "2025-I",
  "startDate": "2025-02-01",
  "endDate": "2025-06-30"
}
```

### Cambiar estado del torneo:
```json
PATCH /api/Tournament/1/status
{ "status": 1 }
```
> Estados: 0=Pending → 1=InProgress → 2=Finished (solo en ese orden)

---

## Reglas de Negocio Implementadas

- ✅ Nombre de equipo único
- ✅ Número de camiseta único por equipo
- ✅ Jugador debe pertenecer a un equipo existente
- ✅ Torneo: fecha fin > fecha inicio
- ✅ Nombre de torneo único por temporada
- ✅ Máquina de estados: Pending → InProgress → Finished (solo hacia adelante)
- ✅ Solo se inscriben equipos en torneos con estado Pending
- ✅ No se puede editar/eliminar un torneo Finished
- ✅ Índice único compuesto (TournamentId + TeamId) evita inscripciones duplicadas
