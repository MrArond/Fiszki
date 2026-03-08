namespace API.DTOs
{
    public class AddCardsListDTO
    {
            public int CardsListID { get; set; }

            public int UserId { get; set; }

            public string? Name { get; set; }
    
            public string? Description { get; set; }
    
    }
}
