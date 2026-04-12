using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public PlayerController(IPlayerService playerService, IMapper mapper)
    {
        _playerService = playerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetAll()
    {
        var players = await _playerService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<PlayerResponseDTO>>(players));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerResponseDTO>> GetById(int id)
    {
        var player = await _playerService.GetByIdAsync(id);
        if (player == null)
            return NotFound(new { message = $"Jugador con ID {id} no encontrado." });

        return Ok(_mapper.Map<PlayerResponseDTO>(player));
    }

    [HttpGet("by-team/{teamId}")]
    public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByTeam(int teamId)
    {
        try
        {
            var players = await _playerService.GetByTeamIdAsync(teamId);
            return Ok(_mapper.Map<IEnumerable<PlayerResponseDTO>>(players));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost]
    public async Task<ActionResult<PlayerResponseDTO>> Create(PlayerRequestDTO dto)
    {
        try
        {
            var player = _mapper.Map<Player>(dto);
            var created = await _playerService.CreateAsync(player);
            var response = _mapper.Map<PlayerResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, PlayerRequestDTO dto)
    {
        try
        {
            var player = _mapper.Map<Player>(dto);
            await _playerService.UpdateAsync(id, player);
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
            await _playerService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
