using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DATA.Models
{
    public class FlashCards
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int CardsId { get; set; }

        public string? CardName { get; set; }

        public string? Translate { get; set; }

        public int FlashCardsListsCardsListID { get; set; }


    }
}
