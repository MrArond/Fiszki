using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface ICardsListRepository
    {
        Task<FlashCardsLists>  AddCardsList(AddCardsListDTO addCardsListDTO, int Id);

        Task<IEnumerable<FlashCardsLists>> GetUserFlashCardsLists(int userId);

    }
}
