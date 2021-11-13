
namespace BookStore.Domain.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string Publishing { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}
