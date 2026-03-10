using API.DTOs;
using API.Services.Interfaces;
using API.Repositories.Interfaces;
namespace API.Services.Implementations
{
    public class AddCardsListService : IAddCardsListService
    {
        private readonly IAddCardsListRepository _cardListService;

        public AddCardsListService(IAddCardsListRepository cardListService)
        {
            _cardListService = cardListService;
        }
        
        async public Task<(bool, string)> AddCardsList(AddCardsListDTO addCardsListDTO, int Id)
        {
            if (String.IsNullOrWhiteSpace(addCardsListDTO.Name))
            {
                return (false, "string");
            }
            if (String.IsNullOrWhiteSpace(addCardsListDTO.Description))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            try
            {
                var CardList = await _cardListService.AddCardsList(addCardsListDTO, Id);
                return (true, "Card list added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }
    } 
}
