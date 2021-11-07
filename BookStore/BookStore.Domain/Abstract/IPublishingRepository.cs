using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
    interface IPublishingRepository
    {
        IEnumerable<Publishing> Publishes { get; }
    }
}
