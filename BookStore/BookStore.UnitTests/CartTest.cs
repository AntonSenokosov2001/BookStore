using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            var book1 = new Book { BookId = 1, Name = "Book1" };
            var book2 = new Book { BookId = 2, Name = "Book2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Book, book1);
            Assert.AreEqual(results[1].Book, book2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            var book1 = new Book { BookId = 1, Name = "Book1" };
            var book2 = new Book { BookId = 2, Name = "Book2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Book.BookId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            var book1 = new Book { BookId = 1, Name = "Book1" };
            var book2 = new Book { BookId = 2, Name = "Book2" };
            var book3 = new Book { BookId = 3, Name = "Book3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 4);
            cart.AddItem(book3, 2);
            cart.AddItem(book2, 1);

            // Действие
            cart.RemoveLine(book2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Book == book2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            var book1 = new Book { BookId = 1, Name = "Book1", Price = 350 };
            var book2 = new Book { BookId = 2, Name = "Book2", Price = 150 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 2250);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            var book1 = new Book { BookId = 1, Name = "Book1", Price = 350 };
            var book2 = new Book { BookId = 2, Name = "Book2", Price = 123 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
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
                new Book {BookId = 3, Name = "Book3", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 4, Name = "Book4", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 5, Name = "Book5", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
            });

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1);
        }

        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
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
                new Book {BookId = 3, Name = "Book3", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 4, Name = "Book4", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
                new Book {BookId = 5, Name = "Book5", Genre = genre.Genres.FirstOrDefault(),  Author = "HGF", Description = "MHTdsfsdfsfs", Price=234M, Publishing = publishing.Publishes.FirstOrDefault(), Year=2007},
            });

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
