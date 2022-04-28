using ApiProtectorDotNet;
using Microsoft.AspNetCore.Mvc;
using ThornData.Models.Bedrock;
using ThornData.Services.Bedrock;

namespace ThornAPI.Controllers.Bedrock;

[ApiController]
[Route("bedrock/[controller]")]
public class FactionsController : ControllerBase {

    private readonly FactionsService _factionsService;

    public FactionsController(FactionsService factionsService) {
        _factionsService = factionsService;
    }

    [HttpGet]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 5, TimeWindowSeconds: 10, PenaltySeconds: 10)]
    public async Task<List<Faction>> GetAll() => 
        await _factionsService.GetAll();
    
    [HttpGet]
    [Route("{arg}")]
    public async Task<ActionResult<Faction>> Get(string arg) { 
        var faction = await _factionsService.GetFaction(arg);

        if (faction == null) {
            return NotFound();
        }

        return faction;
    }

    [HttpPost]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 5, TimeWindowSeconds: 5, PenaltySeconds: 10)]
    public async Task<IActionResult> Create(Faction arg) {
        var user = await _factionsService.GetFaction(arg.id);
        
        if (user is not null) {
            return NotFound();
        }

        await _factionsService.CreateFaction(arg);
        return CreatedAtAction(nameof(Get), new { id = arg.id }, arg);
    }

    [HttpPut]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 10, TimeWindowSeconds: 15, PenaltySeconds: 10)]
    public async Task<IActionResult> Update(Faction arg) { 
        var faction = await _factionsService.GetFaction(arg.id);
        
        if (faction is null) {
            return NotFound();
        }

        await _factionsService.UpdateFaction(arg);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 5, TimeWindowSeconds:5, PenaltySeconds: 30)]
    public async Task<IActionResult> Delete(string id) {
        var faction = await _factionsService.GetFaction(id);

        if (faction is null) {
            return NotFound(); 
        }
        
        await _factionsService.DeleteFaction(id);
        return Ok();
    }
}