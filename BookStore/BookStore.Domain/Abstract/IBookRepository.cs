using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
   public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
    }
}
