using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IAddCardsListRepository
    {
        Task<FlashCardsLists>  AddCardsList(AddCardsListDTO addCardsListDTO, int Id);

        Task<IEnumerable<FlashCardsLists>> GetUserFlashCardsLists(int userId);

        Task<bool> DeleteCardsList(int listId, int userId);

        Task<FlashCardsLists?> UpdateCardsListName(UpdateCardsListNameDTO updateCardsListNameDTO, int userId);

    }
}
