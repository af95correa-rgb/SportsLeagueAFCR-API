using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService, IMapper mapper)
    {
        _teamService = teamService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAll()
    {
        var teams = await _teamService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TeamResponseDTO>>(teams));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamResponseDTO>> GetById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);
        if (team == null)
            return NotFound(new { message = $"Equipo con ID {id} no encontrado." });

        return Ok(_mapper.Map<TeamResponseDTO>(team));
    }

    [HttpPost]
    public async Task<ActionResult<TeamResponseDTO>> Create(TeamRequestDTO dto)
    {
        try
        {
            var team = _mapper.Map<Team>(dto);
            var created = await _teamService.CreateAsync(team);
            var response = _mapper.Map<TeamResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TeamRequestDTO dto)
    {
        try
        {
            var team = _mapper.Map<Team>(dto);
            await _teamService.UpdateAsync(id, team);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _teamService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
