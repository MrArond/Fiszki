using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementation
{
    public class AddCardsListRepository(Datacontext _datacontext) : IAddCardsListRepository
    {

        public async Task<FlashCardsLists> AddCardsList(AddCardsListDTO addCardsListDTO, int Id)
        {
            try
            {


                var NewList = new FlashCardsLists
                {
                    UserId = Id,
                    Name = addCardsListDTO.Name,
                    Description = addCardsListDTO.Description
                };

                _datacontext.FlashCardsLists.Add(NewList);
                await _datacontext.SaveChangesAsync();
                return NewList;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
        }

        public async Task<IEnumerable<FlashCardsLists>> GetUserFlashCardsLists(int userId)
        {
            try
            {
                var flashCardsLists = await _datacontext.FlashCardsLists.Where(x => x.UserId == userId).ToListAsync();
                return flashCardsLists;
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving flash cards lists.");
            }
        }
    }
}
