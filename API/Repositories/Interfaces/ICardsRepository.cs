using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface ICardsRepository
    {
        Task<FlashCards> CreateCard(AddCardDTO addCardDTO, int userId);

        Task<IEnumerable<FlashCards>> GetCardsByListId(int listId, int userId);

        Task<bool> DeleteCard(DeleteCardDTO deleteCardDTO, int userId);
    }
}
