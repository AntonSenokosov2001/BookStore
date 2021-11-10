using System.Collections.Generic;
using BookStore.Domain.Entities;


namespace BookStore.Domain.Abstract
{
    interface IGenreRepository
    {
        IEnumerable<Genre> Genres { get; }
    }
}
