using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA.Dinners.Domain.Abstract;

namespace UI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public ActionResult OrdersAccounting()
        {
            return View();
        }

    }
}
