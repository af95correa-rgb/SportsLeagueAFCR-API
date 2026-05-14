using Microsoft.EntityFrameworkCore;
using SportsLeague.API.Helpers;
using SportsLeague.Domain.Helpers;
using SportsLeague.API.Mappings;
using SportsLeague.API.Middlewares;
using SportsLeague.Domain.Services;
using SportsLeague.DataAccess.Context;
using SportsLeague.DataAccess.Repositories;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Text.Json.Serialization; // 👈 IMPORTANTE (AGREGADO)

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<LeagueDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchResultRepository, MatchResultRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentSponsorRepository, TournamentSponsorRepository>();
builder.Services.AddScoped<ITournamentTeamRepository, TournamentTeamRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

// ── Services ──
builder.Services.AddScoped<IMatchService, SportsLeague.Domain.Services.MatchService>(); // ← era Application.Services
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();
builder.Services.AddScoped<ISponsorService, SponsorService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IMatchEventService, MatchEventService>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// ── Controllers ──
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 🔥 FIX CLAVE: evita el 500 por ciclos en Include()
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// ── Helper ──
builder.Services.AddScoped<SportsLeague.Domain.Helpers.MatchValidationHelper>();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sports League API", Version = "v1" });
});

var app = builder.Build();

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();