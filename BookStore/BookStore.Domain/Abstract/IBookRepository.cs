using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
    interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
    }
}
