using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DATA.Models
{
    public class FlashCardsLists
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int Id { get; set; }

        public int Name { get; set; }

        public int UserId { get; set; }
    }
}
