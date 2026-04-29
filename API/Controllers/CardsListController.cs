using API.DTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsListController : ControllerBase
    {
        private readonly ICardsListService _addCardsListService;

        public CardsListController(ICardsListService addCardsListService)
        {
            _addCardsListService = addCardsListService;
        }

        [HttpPost("AddCardsList")]
        [Authorize]
        public async Task<IActionResult> AddCardsList([FromBody] AddCardsListDTO addCardsList)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }

            var (isSuccess, message) = await _addCardsListService.AddCardsList(addCardsList, userId);

            if (isSuccess)
            {
                return Ok(new { message });
            }

            return BadRequest(new { message });
        }
        [HttpDelete("DeleteCardsList")]
        [Authorize]
        public async Task<IActionResult> DeleteCardsList([FromBody] DeleteCardsListDTO deleteCardsList)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }
            var (isSuccess, message) = await _addCardsListService.DeleteCardsList(deleteCardsList, userId);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            return BadRequest(new { message });
        }

        [HttpGet("GetUserCardsLists")]
        [Authorize]
        public async Task<IActionResult> GetUserCardsLists()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                return Unauthorized();
            }

            var (isSuccess, message, cardsLists) = await _addCardsListService.GetUserCardsLists(userId);

            if (isSuccess)
            {
                return Ok(cardsLists);
            }

            return BadRequest(new { message });
        }
    }
}
