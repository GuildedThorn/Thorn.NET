using ApiProtectorDotNet;
using Microsoft.AspNetCore.Mvc;
using ThornData.Models.Bedrock;
using ThornData.Services.Bedrock;

namespace ThornAPI.Controllers.Bedrock;

[ApiController]
public class FactionController : ControllerBase {

    private readonly FactionsService _factionsService;

    public FactionController(FactionsService factionsService) {
        _factionsService = factionsService;
    }

    [HttpGet("GetAllFactions")]
    [Route("bedrock/factions/all")]
    public async Task<List<Faction>> GetAll() {
        return await _factionsService.GetAll();
    }

    [HttpGet("GetFactionByIdOrName")]
    [Route("bedrock/factions/get/{arg}")]
    public async Task<Faction> Get(string arg) { 
        if (arg.All(char.IsDigit)) {
            return await _factionsService.GetFactionById(arg);
        } 
        return await _factionsService.GetFactionByName(arg);
    }

    [HttpPost("CreateFaction")]
    [Route("bedrock/factions/create")]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 5, TimeWindowSeconds: 5, PenaltySeconds: 10)]
    public async Task Create(Faction faction) =>
        await _factionsService.CreateFaction(faction);

    [HttpPut("UpdateFactionByIdOrName")]
    [Route("bedrock/factions/update/{arg}")]
    [ApiProtector(ApiProtectionType.ByIpAddress, Limit: 10, TimeWindowSeconds: 15, PenaltySeconds: 10)]
    public async Task Update(string arg, Faction faction) {
        if (arg.All(char.IsDigit)) {
            await _factionsService.UpdateFactionById(arg, faction);
        }else {
            await _factionsService.UpdateFactionByName(arg, faction);
        }
    }

    [HttpDelete("DeleteFactionByIdOrName")]
    [Route("/bedrock/factions/delete/{arg}")]
    public async Task Delete(string arg) {
        if (arg.All(char.IsDigit)) {
            await _factionsService.DeleteFactionById(arg);
        } else {
            await _factionsService.DeleteFactionByName(arg);
        }
    }
}