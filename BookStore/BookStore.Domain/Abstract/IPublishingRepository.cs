using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
    public interface IPublishingRepository
    {
        IEnumerable<Publishing> Publishes { get; }
    }
}
