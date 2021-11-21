using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.Controllers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void CanAddToCart()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1);
        }
    }
}
