using API.DTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddCardsListController : ControllerBase
    {
        private readonly IAddCardsListService _addCardsListService;

        public AddCardsListController(IAddCardsListService addCardsListService)
        {
            _addCardsListService = addCardsListService;
        }

        [HttpPost("AddCardsList")]
        [Authorize]
        public async Task<IActionResult> AddCardsList([FromBody] AddCardsListDTO addCardsList)
        {
            var token = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });
            var claim = token.FirstOrDefault(n => n.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }
            return Ok(await _addCardsListService.AddCardsList(addCardsList, userId));
        }
    }
}
