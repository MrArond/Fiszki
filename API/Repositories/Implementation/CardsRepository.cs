using API.DATA.Context;
using API.DATA.Models;
using API.DTOs;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementation
{
    public class CardsRepository(Datacontext _datacontext) : ICardsRepository
    {
        public async Task<FlashCards> CreateCard(AddCardDTO addCardDTO, int userId)
        {
            try
            {
                var listExists = await _datacontext.FlashCardsLists
                    .AnyAsync(l => l.CardsListID == addCardDTO.FlashCardsListsCardsListID && l.UserId == userId);

                if (!listExists)
                {
                    throw new Exception("List does not exist.");
                }

                var card = new FlashCards
                {
                    FlashCardsListsCardsListID = addCardDTO.FlashCardsListsCardsListID,
                    CardName = addCardDTO.CardName,
                    Translate = addCardDTO.Translate,
                };

                _datacontext.FlashCards.Add(card);
                await _datacontext.SaveChangesAsync();

                return card;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<FlashCards>> GetCardsByListId(int listId, int userId)
        {
            try
            {
                var listExists = await _datacontext.FlashCardsLists
                    .AnyAsync(l => l.CardsListID == listId && l.UserId == userId);

                if (!listExists)
                {
                    throw new Exception("List not found or access denied.");
                }

                var cards = await _datacontext.FlashCards
                    .Where(c => c.FlashCardsListsCardsListID == listId)
                    .ToListAsync();

                return cards;

            }
            catch (Exception)
            {
                throw new Exception("Error retrieving flash cards.");
            }
        }
        public async Task<bool> DeleteCard(DeleteCardDTO deleteCardDTO, int userId)
        {
            try
            {
                var card = await _datacontext.FlashCards
                    .FirstOrDefaultAsync(c => c.CardsId == deleteCardDTO.CardId && 
                        _datacontext.FlashCardsLists.Any(l => l.CardsListID == c.FlashCardsListsCardsListID && l.UserId == userId));
                if (card == null)
                {
                    return false;
                }
                _datacontext.FlashCards.Remove(card);
                await _datacontext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error deleting flash card.");
            }
        }
    }
}
