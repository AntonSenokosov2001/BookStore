using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using IModelBinder = System.Web.Mvc.IModelBinder;
using ModelBindingContext = System.Web.ModelBinding.ModelBindingContext;

namespace BookStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            // Создать объект Cart если он не обнаружен в сеансе
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }

            // Возвратить объект Cart
            return cart;
        }
    }
}