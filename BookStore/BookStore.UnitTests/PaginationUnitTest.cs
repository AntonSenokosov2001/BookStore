using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.Controllers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class PaginationUnitTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IGenreRepository> mockgenre = new Mock<IGenreRepository>();
            mockgenre.Setup(g => g.Genres).Returns(new List<Genre>
            {
                new Genre {GenreId = 1, GenreName = "Фантастика"},
                new Genre {GenreId = 2, GenreName = "Роман"},
                new Genre {GenreId = 3, GenreName = "Приключение"}
            });

            Mock<IPublishingRepository> mockpubl = new Mock<IPublishingRepository>();
            mockpubl.Setup(p => p.Publishes).Returns(new List<Publishing>
            {
                new Publishing {PublishingId = 1, City = "Kiev", PublishingName = "KievPubl"},
                new Publishing {PublishingId = 2, City = "Kharkiv", PublishingName = "KharkivPubl"},
                new Publishing {PublishingId = 1, City = "Lviv", PublishingName = "LvivPubl"}
            });

            IGenreRepository genre = mockgenre.Object;
            IPublishingRepository publishing = mockpubl.Object;

            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 2, Name = "Book2", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 3, Name = "Book2", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 4, Name = "Book3", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 5, Name = "Book4", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 6, Name = "Book5", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
            });

            BookController bookController = new BookController(mock.Object);
            bookController.pageSize = 3;

            IEnumerable<Book> result = (IEnumerable<Book>)bookController.List(2).Model;

            List<Book> books = result.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Book4");
            Assert.AreEqual(books[1].Name, "Book5");
        }
    }
}
