using API.DTOs;

namespace API.Services.Interfaces
{
    public interface ICardsListService
    {
        Task<(bool, string)> AddCardsList(AddCardsListDTO addCardsListDTO, int Id);

        Task<(bool, string, IEnumerable<GetCardsListDTO>?)> GetUserCardsLists(int userId);
    }
}
    