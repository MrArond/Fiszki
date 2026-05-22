using API.DTOs;
using API.Services.Interfaces;
using API.Repositories.Interfaces;
namespace API.Services.Implementations
{
    public class CardsListService : ICardsListService
    {
        private readonly ICardsListRepository _cardListService;

        public CardsListService(ICardsListRepository cardListService)
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
        async public Task<(bool, string, IEnumerable<GetCardsListDTO>?)> GetUserCardsLists(int userId)
        {
            try
            {
                var cardLists = await _cardListService.GetUserFlashCardsLists(userId);
                var dtoList = cardLists.Select(CardList => new GetCardsListDTO
                {
                    FlashCardsListsCardsListID = CardList.CardsListID,
                    Name = CardList.Name,
                    Description = CardList.Description
                });
                return (true, "Card lists retrieved successfully.", dtoList);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<(bool isSuccess, string message)> DeleteCardsList(int listId, int userId)
        {
            var deleted = await _cardListService.DeleteCardsList(listId, userId);
            if (!deleted)
            {
                return (false, "Lista nie istnieje lub nie masz do niej dostępu.");
            }

            return (true, "Lista została usunięta.");
        }

        public async Task<(bool isSuccess, string message)> UpdateCardsListName(UpdateCardsListNameDTO updateCardsListNameDTO, int userId)
        {
            if (string.IsNullOrWhiteSpace(updateCardsListNameDTO.Name))
            {
                return (false, "Nazwa listy jest wymagana.");
            }

            var updated = await _cardListService.UpdateCardsListName(updateCardsListNameDTO, userId);
            if (updated == null)
            {
                return (false, "Lista nie istnieje lub nie masz do niej dostępu.");
            }

            return (true, "Zaktualizowano nazwę listy.");
        }
    }
}
