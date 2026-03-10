using API.DATA.Models;
using API.DTOs;

namespace API.Repositories.Interfaces
{
    public interface IAddCardsListRepository
    {
        Task<FlashCardsLists>  AddCardsList(AddCardsListDTO addCardsListDTO, int Id);

    }
}
