using API.DTOs;

namespace API.Services.Interfaces
{
    public interface IAddCardsListService
    {
        Task<(bool, string)> AddCardsList(AddCardsListDTO addCardsListDTO, int Id);
    }
}
