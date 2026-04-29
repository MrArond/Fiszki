using API.DTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;
        public CardsController(ICardsService cardsService)
        {
            _cardsService = cardsService;
        }
        [HttpPost("AddCard")]
        [Authorize]
        public async Task<IActionResult> AddCard([FromBody] AddCardDTO addCardDTO)
        {
            var token = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });
            var claim = token.FirstOrDefault(n => n.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized(new { message = "Nieprawidłowy token użytkownika." });
            }
            var (isSuccess, message) = await _cardsService.CreateCard(addCardDTO, userId);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }
        [HttpGet("GetCards/{listId}")]
        [Authorize]
        public async Task<IActionResult> GetCards(int listId)
        {
            var token = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });
            var claim = token.FirstOrDefault(n => n.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }
            var (isSuccess, message, cards) = await _cardsService.GetCardsByListId(listId, userId);
            if (isSuccess)
            {
                return Ok(new { message, cards });
            }
            return BadRequest(new { message });

        }
        [HttpDelete("DeleteCard")]
        [Authorize]
        public async Task<IActionResult> DeleteCard([FromBody] DeleteCardDTO deleteCardDTO)
        {
            var token = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });
            var claim = token.FirstOrDefault(n => n.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }
            var (isSuccess, message) = await _cardsService.DeleteCard(deleteCardDTO, userId);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }
    }
}