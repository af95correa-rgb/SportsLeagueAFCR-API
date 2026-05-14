# 🏆 Sports League API

API REST para gestión de liga deportiva desarrollada con .NET 8, C#, Entity Framework Core y SQL Server.

## Stack Tecnológico

`.NET 8` | `C#` | `Entity Framework Core 8` | `SQL Server`  
`AutoMapper` | `Swagger` | `Arquitectura N-Capas`

---

## Estructura del Proyecto

```
SportsLeague/
├── SportsLeague.sln
├── SportsLeague.API/            ← Presentación (Controllers, DTOs, Mappings, Middlewares)
├── SportsLeague.Domain/         ← Negocio (Entities, Enums, Interfaces, Services, Helpers)
└── SportsLeague.DataAccess/     ← Datos (DbContext, Repositories, Migrations, Seeders)
```

---

## Requisitos Previos

- .NET 8 SDK
- SQL Server (Express, Developer o LocalDB)
- Visual Studio 2022 o VS Code

---

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

Si usas usuario/contraseña:
```
Server=localhost;Database=SportsLeagueDb;User Id=sa;Password=TuPassword;TrustServerCertificate=true;
```

### 3. Crear y aplicar migraciones

```bash
# Desde la raíz de la solución:
dotnet ef migrations add InitialCreate --project SportsLeague.DataAccess --startup-project SportsLeague.API
dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API
```

> **Nota:** Al levantar la aplicación por primera vez, el seeder crea automáticamente 20 equipos colombianos, 80 jugadores (4 por equipo), 4 árbitros y un torneo `Liga BetPlay 2026-I` ya en estado `InProgress` con los 20 equipos inscritos, listo para crear partidos.

### 4. Ejecutar la API

```bash
cd SportsLeague.API
dotnet run
```

Abrir: `http://localhost:5011/swagger`

---

## Endpoints Disponibles

### 🟢 Teams

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Team` | Listar todos los equipos |
| GET | `/api/Team/{id}` | Obtener equipo por ID |
| POST | `/api/Team` | Crear equipo |
| PUT | `/api/Team/{id}` | Actualizar equipo |
| DELETE | `/api/Team/{id}` | Eliminar equipo |

### 🟡 Players

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Player` | Listar todos los jugadores |
| GET | `/api/Player/{id}` | Obtener jugador por ID |
| GET | `/api/Player/by-team/{teamId}` | Jugadores de un equipo |
| POST | `/api/Player` | Crear jugador |
| PUT | `/api/Player/{id}` | Actualizar jugador |
| DELETE | `/api/Player/{id}` | Eliminar jugador |

### 🔵 Referees

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Referee` | Listar árbitros |
| GET | `/api/Referee/{id}` | Obtener árbitro por ID |
| POST | `/api/Referee` | Crear árbitro |
| PUT | `/api/Referee/{id}` | Actualizar árbitro |
| DELETE | `/api/Referee/{id}` | Eliminar árbitro |

### 🏆 Tournaments

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Tournament` | Listar torneos |
| GET | `/api/Tournament/{id}` | Obtener torneo (incluye equipos) |
| POST | `/api/Tournament` | Crear torneo |
| PUT | `/api/Tournament/{id}` | Actualizar torneo |
| DELETE | `/api/Tournament/{id}` | Eliminar torneo |
| PATCH | `/api/Tournament/{id}/status` | Cambiar estado del torneo |
| POST | `/api/Tournament/{id}/teams` | Inscribir equipo |
| GET | `/api/Tournament/{id}/teams` | Equipos inscritos |

### 💰 Sponsors

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Sponsor` | Listar patrocinadores |
| GET | `/api/Sponsor/{id}` | Obtener patrocinador por ID |
| POST | `/api/Sponsor` | Crear patrocinador |
| PUT | `/api/Sponsor/{id}` | Actualizar patrocinador |
| DELETE | `/api/Sponsor/{id}` | Eliminar patrocinador |
| POST | `/api/Sponsor/{id}/tournaments` | Vincular patrocinador a torneo |
| GET | `/api/Sponsor/{id}/tournaments` | Torneos de un patrocinador |
| DELETE | `/api/Sponsor/{id}/tournaments/{tid}` | Desvincular patrocinador de torneo |

### ⚽ Matches

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Match/tournament/{tournamentId}` | Partidos de un torneo |
| GET | `/api/Match/{id}` | Obtener partido por ID |
| POST | `/api/Match` | Crear partido |
| PUT | `/api/Match/{id}` | Actualizar partido (solo Scheduled) |
| DELETE | `/api/Match/{id}` | Eliminar partido (solo Scheduled) |
| PATCH | `/api/Match/{id}/status` | Cambiar estado del partido |

### 📋 Match Events (Result, Goals, Cards)

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/match/{matchId}/result` | Registrar resultado |
| GET | `/api/match/{matchId}/result` | Obtener resultado |
| POST | `/api/match/{matchId}/goals` | Registrar gol |
| GET | `/api/match/{matchId}/goals` | Goles del partido |
| DELETE | `/api/match/{matchId}/goals/{goalId}` | Eliminar gol |
| POST | `/api/match/{matchId}/cards` | Registrar tarjeta |
| GET | `/api/match/{matchId}/cards` | Tarjetas del partido |
| DELETE | `/api/match/{matchId}/cards/{cardId}` | Eliminar tarjeta |

### 🧾 Match Lineup (Alineaciones)

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/match/{matchId}/lineup` | Agregar jugador a alineación |
| GET | `/api/match/{matchId}/lineup` | Ver alineación completa |
| GET | `/api/match/{matchId}/lineup/team/{teamId}` | Alineación por equipo |
| DELETE | `/api/match/{matchId}/lineup/{lineupId}` | Eliminar jugador de alineación |

---

## Datos de Prueba (Swagger)

### Crear equipo
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

### Crear jugador
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

Posiciones: `0=Goalkeeper`, `1=Defender`, `2=Midfielder`, `3=Forward`

### Crear árbitro
```json
POST /api/Referee
{
  "firstName": "Wilmar",
  "lastName": "Roldán",
  "nationality": "Colombiana"
}
```

### Crear torneo
```json
POST /api/Tournament
{
  "name": "Liga BetPlay",
  "season": "2025-I",
  "startDate": "2025-02-01",
  "endDate": "2025-06-30"
}
```

### Cambiar estado del torneo
```json
PATCH /api/Tournament/1/status
{ "status": 1 }
```
Estados torneo: `0=Pending → 1=InProgress → 2=Finished` (solo en ese orden)

### Crear partido
```json
POST /api/Match
{
  "tournamentId": 1,
  "homeTeamId": 1,
  "awayTeamId": 2,
  "refereeId": 1,
  "matchDate": "2026-03-15T15:00:00",
  "venue": "Atanasio Girardot",
  "matchday": 1
}
```

### Cambiar estado del partido
```json
PATCH /api/Match/1/status
{ "status": 1 }
```
Estados partido: `0=Scheduled`, `1=InProgress`, `2=Finished`, `3=Suspended`

Transiciones válidas:
- `Scheduled → InProgress`
- `InProgress → Finished`
- `Scheduled → Suspended`
- `InProgress → Suspended`

### Registrar gol
```json
POST /api/match/1/goals
{
  "playerId": 4,
  "minute": 35,
  "type": 0
}
```
Tipos de gol: `0=Normal`, `1=Penalty`, `2=OwnGoal`

### Registrar tarjeta
```json
POST /api/match/1/cards
{
  "playerId": 3,
  "minute": 67,
  "type": 0
}
```
Tipos de tarjeta: `0=Yellow`, `1=Red`

### Registrar resultado
```json
POST /api/match/1/result
{
  "homeGoals": 2,
  "awayGoals": 1,
  "observations": "Partido disputado"
}
```

### Agregar jugador a alineación
```json
POST /api/match/1/lineup
{
  "playerId": 1,
  "isStarter": true,
  "position": "GK"
}
```

### Crear patrocinador
```json
POST /api/Sponsor
{
  "name": "Bancolombia",
  "contactEmail": "sponsor@bancolombia.com",
  "phone": "+573001234567",
  "websiteUrl": "https://bancolombia.com",
  "category": 0
}
```
Categorías: `0=Main`, `1=Gold`, `2=Silver`, `3=Bronze`

### Vincular patrocinador a torneo
```json
POST /api/Sponsor/1/tournaments
{
  "tournamentId": 1,
  "contractAmount": 50000000
}
```

---

## Flujo de Prueba Completo

Para probar un partido de inicio a fin, seguir este orden:

1. `POST /api/Team` → Crear 2 equipos
2. `POST /api/Referee` → Crear 1 árbitro
3. `POST /api/Tournament` → Crear 1 torneo
4. `POST /api/Tournament/1/teams` → Inscribir equipo 1
5. `POST /api/Tournament/1/teams` → Inscribir equipo 2
6. `PATCH /api/Tournament/1/status` → `{ "status": 1 }` (→ InProgress)
7. `POST /api/Match` → Crear partido
8. `POST /api/match/1/lineup` → Registrar alineaciones (solo en Scheduled)
9. `PATCH /api/Match/1/status` → `{ "status": 1 }` (→ InProgress)
10. `POST /api/match/1/goals` → Registrar goles
11. `POST /api/match/1/cards` → Registrar tarjetas
12. `PATCH /api/Match/1/status` → `{ "status": 2 }` (→ Finished)
13. `POST /api/match/1/result` → Registrar resultado oficial

> **Nota:** El seeder crea automáticamente todo lo necesario en los pasos 1-6 con datos reales de la Liga BetPlay colombiana, por lo que puedes ir directo al paso 7.

---

## Reglas de Negocio Implementadas

### Equipos y Jugadores
- ✅ Nombre de equipo único
- ✅ Número de camiseta único por equipo
- ✅ Jugador debe pertenecer a un equipo existente

### Torneos
- ✅ Fecha fin > fecha inicio
- ✅ Nombre de torneo único por temporada
- ✅ Máquina de estados: `Pending → InProgress → Finished` (solo hacia adelante)
- ✅ Solo se inscriben equipos en torneos con estado `Pending`
- ✅ No se puede editar/eliminar un torneo `Finished`

### Partidos
- ✅ Solo se crean partidos en torneos `InProgress`
- ✅ Equipo local y visitante deben ser diferentes
- ✅ Ambos equipos deben estar inscritos en el torneo
- ✅ El árbitro debe existir
- ✅ Solo se editan/eliminan partidos en estado `Scheduled`
- ✅ Máquina de estados con transiciones válidas (incluye `Suspended`)
- ✅ `DeleteBehavior.Restrict` en equipos y árbitros — evita ciclos de cascada
- ✅ `DeleteBehavior.Cascade` en torneo — eliminar torneo elimina sus partidos

### Alineaciones
- ✅ Solo se registran en partidos `Scheduled`
- ✅ El jugador debe pertenecer a uno de los dos equipos del partido
- ✅ Un jugador no puede estar duplicado en la misma alineación
- ✅ Máximo 11 titulares por equipo por partido

### Eventos (Goles y Tarjetas)
- ✅ Solo se registran en partidos `InProgress` o `Finished`
- ✅ El jugador debe pertenecer a uno de los dos equipos del partido
- ✅ El minuto debe estar entre 1 y 120

### Resultado
- ✅ Solo se registra en partidos `Finished`
- ✅ Solo se puede registrar un resultado por partido
- ✅ Los goles no pueden ser negativos

### Patrocinadores
- ✅ Nombre de patrocinador único
- ✅ Email de contacto válido
- ✅ Monto de contrato mayor a 0
- ✅ No se puede duplicar la vinculación patrocinador-torneo

---

## Comportamiento de Eliminación en Cascada

| Entidad eliminada | Efecto |
|-------------------|--------|
| Torneo | Elimina en cascada: TournamentTeams, TournamentSponsors, Matches → Goals, Cards, MatchResult, Lineups |
| Equipo con partidos | ❌ Error (Restrict) — eliminar primero los partidos |
| Árbitro con partidos | ❌ Error (Restrict) — eliminar primero los partidos |
| Partido | Elimina en cascada: Goals, Cards, MatchResult, Lineups |
| Jugador con goles/tarjetas | ❌ Error (Restrict) |
