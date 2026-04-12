using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _service;
    private readonly IMapper _mapper;

    public SponsorController(ISponsorService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // 🔹 GET ALL
    [HttpGet]
    public async Task<ActionResult> GetAll()
        => Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(await _service.GetAllAsync()));

    // 🔹 GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var sponsor = await _service.GetByIdAsync(id);
        if (sponsor == null) return NotFound();
        return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
    }

    // 🔹 CREATE
    [HttpPost]
    public async Task<ActionResult> Create(SponsorRequestDTO dto)
    {
        var entity = _mapper.Map<Sponsor>(dto);
        var created = await _service.CreateAsync(entity);

        return CreatedAtAction(nameof(GetById),
            new { id = created.Id },
            _mapper.Map<SponsorResponseDTO>(created));
    }

    // 🔹 UPDATE
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        await _service.UpdateAsync(id, _mapper.Map<Sponsor>(dto));
        return NoContent();
    }

    // 🔹 DELETE
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // 🔗 N:M → LINK SPONSOR WITH TOURNAMENT
    [HttpPost("{id}/tournaments")]
    public async Task<ActionResult> Link(int id, [FromBody] LinkSponsorDTO dto)
    {
        await _service.LinkSponsorAsync(id, dto.TournamentId, dto.ContractAmount);
        return StatusCode(201);
    }

    // 🔗 N:M → GET TOURNAMENTS BY SPONSOR (CORREGIDO)
    [HttpGet("{id}/tournaments")]
    public async Task<ActionResult> GetTournaments(int id)
    {
        var data = await _service.GetTournamentsBySponsorAsync(id);

        var result = data.Select(ts => new
        {
            ts.TournamentId,
            TournamentName = ts.Tournament.Name,
            ts.ContractAmount,
            ts.JoinedAt
        });

        return Ok(result);
    }

    // 🔗 N:M → UNLINK
    [HttpDelete("{id}/tournaments/{tid}")]
    public async Task<ActionResult> Unlink(int id, int tid)
    {
        await _service.UnlinkSponsorAsync(id, tid);
        return NoContent();
    }
}