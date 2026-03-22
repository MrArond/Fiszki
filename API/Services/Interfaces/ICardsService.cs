using API.DTOs;

namespace API.Services.Interfaces
{
    public interface ICardsService
    {
        Task<(bool, string)> CreateCard(AddCardDTO addCardDTO, int userId);

        Task<(bool isSuccess, string message, IEnumerable<GetCardDTO>? cards)> GetCardsByListId(int listId, int userId);
    }
}
