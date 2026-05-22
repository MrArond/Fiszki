using API.DTOs;

namespace API.Services.Interfaces
{
    public interface IAddCardsListService
    {
        Task<(bool, string)> AddCardsList(AddCardsListDTO addCardsListDTO, int Id);

        Task<(bool, string, IEnumerable<GetCardsListDTO>?)> GetUserCardsLists(int userId);

        Task<(bool isSuccess, string message)> DeleteCardsList(int listId, int userId);

        Task<(bool isSuccess, string message)> UpdateCardsListName(UpdateCardsListNameDTO updateCardsListNameDTO, int userId);
    }
}
    