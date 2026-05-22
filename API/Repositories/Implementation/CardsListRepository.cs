using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementation
{
    public class CardsListRepository(Datacontext _datacontext) : ICardsListRepository
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

        public async Task<bool> DeleteCardsList(int listId, int userId)
        {
            var list = await _datacontext.FlashCardsLists.FirstOrDefaultAsync(l => l.CardsListID == listId && l.UserId == userId);
            if (list == null)
            {
                return false;
            }

            var cards = await _datacontext.FlashCards
                .Where(c => c.FlashCardsListsCardsListID == list.CardsListID)
                .ToListAsync();

            if (cards.Count != 0)
            {
                _datacontext.FlashCards.RemoveRange(cards);
            }

            _datacontext.FlashCardsLists.Remove(list);
            await _datacontext.SaveChangesAsync();
            return true;
        }

        public async Task<FlashCardsLists?> UpdateCardsListName(UpdateCardsListNameDTO updateCardsListNameDTO, int userId)
        {
            var list = await _datacontext.FlashCardsLists.FirstOrDefaultAsync(l => l.CardsListID == updateCardsListNameDTO.CardsListID && l.UserId == userId);
            if (list == null)
            {
                return null;
            }

            list.Name = updateCardsListNameDTO.Name;
            await _datacontext.SaveChangesAsync();
            return list;
        }
    }
}
