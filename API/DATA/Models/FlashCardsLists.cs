using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DATA.Models
{
    public class FlashCardsLists
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int CardsListID { get; set; }

        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

    }
}
