using API.DTOs;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Services.Implementations
{
    public class CardsService : ICardsService
    {
        private readonly ICardsRepository _cardsRepository;
        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }
        public async Task<(bool, string)> CreateCard(AddCardDTO addCardDTO, int userId)
        {
            try
            {
                var result = await _cardsRepository.CreateCard(addCardDTO, userId);

                if (result == null)
                {
                    return (false, "Lista nie istnieje lub nie masz do niej dostępu.");
                }

                return (true, "Dodano karte");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating card: {ex.Message}");
            }

        }
        public async Task<(bool isSuccess, string message, IEnumerable<GetCardDTO>? cards)> GetCardsByListId(int listId, int userId)
        {
            try
            {
                var result = await _cardsRepository.GetCardsByListId(listId, userId);

                var dtos = result.Select(c => new GetCardDTO
                {
                    CardName = c.CardName,
                    Translate = c.Translate,
                    FlashCardsListsCardsListID = c.FlashCardsListsCardsListID
                });

                return (true, "Karty pobrane pomyślnie", dtos);
            }
            catch (Exception ex)
            {
                if (ex.Message == "List not found or access denied.")
                {
                    return (false, "Lista nie istnieje lub nie masz do niej dostępu.", null);
                }
                throw new Exception($"Error retrieving cards: {ex.Message}");
            }
        }
        public async Task<(bool isSuccess, string message)> DeleteCard(DeleteCardDTO deleteCardDTO, int userId)
        {
            try
            {
                var result = await _cardsRepository.DeleteCard(deleteCardDTO, userId);
                if (!result)
                {
                    return (false, "Karta nie istnieje lub nie masz do niej dostępu.");
                }
                return (true, "Karta usunięta pomyślnie.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting card: {ex.Message}");
            }
        }
    }
}
