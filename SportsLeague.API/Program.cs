using Microsoft.EntityFrameworkCore;
using SportsLeague.API.Mappings;
using SportsLeague.API.Middlewares;
using SportsLeague.DataAccess.Context;
using SportsLeague.DataAccess.Repositories;
using SportsLeague.DataAccess.Seeders;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<LeagueDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchResultRepository, MatchResultRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentSponsorRepository, TournamentSponsorRepository>();
builder.Services.AddScoped<ITournamentTeamRepository, TournamentTeamRepository>();

// ── Services ──
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchEventService, MatchEventService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();
builder.Services.AddScoped<ISponsorService, SponsorService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IStandingsService, StandingsService>();

// ── Helpers ──
builder.Services.AddScoped<MatchValidationHelper>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// ── Controllers ──
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sports League API", Version = "v1" });
});

var app = builder.Build();

// ── Data Seeder ──
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<LeagueDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

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