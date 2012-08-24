using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DA.Dinners.Domain.Abstract;
using DA.Dinners.Domain;
using UI.Models;
using System.Json;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace UI.Controllers.Api
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPersonRepository personRepository;
        private readonly IProductRepository productRepository;

        static dynamic state1 = new JsonObject();
        static dynamic state2 = new JsonObject();
        static dynamic state3 = new JsonObject();
        static dynamic state4 = new JsonObject();
        static dynamic state5 = new JsonObject();
                      
        static OrdersController()
        {
            state1.Text = "Не отправлено";
            state1.Id = (int)OrderStatusValue.Order;
            
            state2.Text = "В процессе";
            state2.Id = (int)OrderStatusValue.SentOrder;

            state3.Text = "Доставлено";
            state3.Id = (int)OrderStatusValue.DeliveredOrder;

            state4.Text = "Не доставлено";
            state4.Id = (int)OrderStatusValue.NotDeliveredOrder;

            state5.Text = "Заказов не было";
            state5.Id = 0;
         
        }
           
        public OrdersController(IOrderRepository orderRepository, IPersonRepository personRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.personRepository = personRepository;
            this.productRepository = productRepository;
        }


        [AcceptVerbs("POST")]
        public void Create(Order order)
        {
            if(order == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            
            orderRepository.InsertOrUpdateWithPerson(order, User.Identity.Name);
            orderRepository.Save();
        }

        [AcceptVerbs("POST")]
        public void RemovePreviousDetails(IEnumerable<int> deleted_detail_list)
        {
            Order order = orderRepository.Find(deleted_detail_list.Last());
            if (order == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            
            orderRepository.DeletedDetails(order, deleted_detail_list);
            orderRepository.Save();
        }

        [AcceptVerbs("POST")]
        public void CreateFromAdmin(Order order)
        {
            if (order == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            orderRepository.InsertOrUpdate(order);
            orderRepository.Save();
        }
        //-----------------------------------------------------------------------------------------------------------
        
        [AcceptVerbs("GET", "POST")]
        public string GetByDateForCurrentUser(DateTime date_)
        {
            if (date_ == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            Person person = personRepository.All.SingleOrDefault(p => p.DomainName.Equals(User.Identity.Name));

            Order order = orderRepository.All.Where(o => o.Person.Id == person.Id).SingleOrDefault(o => o.Date.Year == date_.Year && o.Date.Day == date_.Day && o.Date.Month == date_.Month);

            if (order == null)
                return null;

            return GetOrderJsonValue(order);
        }

        [AcceptVerbs("GET", "POST")]
        public string Get(int orderId)
        {
            Order order = orderRepository.Find(orderId);

            if (order == null)
                return null;

            return GetOrderJsonValue(order);
        }

        //--------------------------------------------------------------------------------
        [AcceptVerbs("GET")]
        public string GetAllOrderModelsForDates(int start_date_day, int start_date_month, int start_date_year, int end_date_day, int end_date_month, int end_date_year)
        {
            DateTime startDate = new DateTime(start_date_year, start_date_month, start_date_day);
            DateTime endDate   = new DateTime(end_date_year, end_date_month, end_date_day);

            List<Order> allOrders = orderRepository.AllIncluding(o => o.OrderDetail, o => o.Person, o => o.Statuses).Where(o => (o.Date >= startDate) && (o.Date <= endDate)).ToList();
            
            dynamic json_all_orders = new JsonArray();
            for (int i = 0; i < endDate.Subtract(startDate).Days; i++)
            {
                DateTime date= startDate.AddDays(i);
                dynamic day = new JsonObject();
                day.Date = date;
                day.possible_states = GetPosibleStatesForOrders(date); //new JsonArray();
                day.current_state = GetCurrentState(date);
                json_all_orders.Add(day);
            }
            return json_all_orders.ToString();
       
        }

        [AcceptVerbs("GET")]
        public string GetAllOrdersForDate(DateTime date)
        {
            List<Order> allOrders = orderRepository.AllIncluding(o => o.OrderDetail, o => o.Person, o => o.Statuses).Where(o => o.Date.Day == date.Day && o.Date.Month == date.Month && o.Date.Year == date.Year).ToList();
            return OrdersListByDateToJson(allOrders, date);
        }
      
        [AcceptVerbs("GET", "POST")]
        public void Delete(int orderId)
        {
            orderRepository.Delete(orderId);
            orderRepository.Save();
        }
        
        [AcceptVerbs("GET", "POST")]
        public void SendDayOrders(DateTime date)
        {
            //do something...
            List<Order> allOrders = orderRepository.AllIncluding(o => o.OrderDetail, o => o.Person, o => o.Statuses).Where(o => o.Date.Day == date.Day && o.Date.Month == date.Month && o.Date.Year == date.Year).ToList();
            foreach(var order in allOrders)
            {
                order.Statuses.SingleOrDefault(s => s.isCurrent).isCurrent = false;
                order.Statuses.Add(new OrderStatus{ StatusValue = (int)OrderStatusValue.SentOrder, Date = DateTime.Now });
            }
        }
        //---------------------------------------------------------------------
        
        private int GetCommonStatusForOrdersOnDay(DateTime date)
        {
            List<Order> allOrders = orderRepository.AllIncluding(o => o.OrderDetail, o => o.Person, o => o.Statuses).Where(o => o.Date == date.Date).ToList();

            if (allOrders.Count == 0)
                return 0;

            IEnumerable<Order> not_sent = allOrders.Where(o => o.Statuses.SingleOrDefault(s => s.isCurrent == true).StatusValue == (int)OrderStatusValue.Order);
            if (not_sent.Count() == allOrders.Count)
                return (int)OrderStatusValue.Order;

            IEnumerable<Order> sent = allOrders.Where(o => o.Statuses.SingleOrDefault(s => s.isCurrent == true).StatusValue == (int)OrderStatusValue.SentOrder);
            if (sent.Count() == allOrders.Count)
                return (int)OrderStatusValue.SentOrder;

            IEnumerable<Order> delivered = allOrders.Where(o => o.Statuses.SingleOrDefault(s => s.isCurrent == true).StatusValue == (int)OrderStatusValue.DeliveredOrder);
            if (delivered.Count() == allOrders.Count)
                return (int)OrderStatusValue.DeliveredOrder;

            IEnumerable<Order> not_delivered = allOrders.Where(o => o.Statuses.SingleOrDefault(s => s.isCurrent == true).StatusValue == (int)OrderStatusValue.NotDeliveredOrder);
            if (not_delivered.Count() == allOrders.Count)
                return (int)OrderStatusValue.NotDeliveredOrder;

            IEnumerable<Order> payed = allOrders.Where(o => o.Statuses.SingleOrDefault(s => s.isCurrent == true).StatusValue == (int)OrderStatusValue.Payed);
            if (payed.Count() == allOrders.Count)
                return (int)OrderStatusValue.Payed;

            return 0;
        }
        
        private dynamic GetCurrentState(DateTime date)
        {
            int commonStatus = GetCommonStatusForOrdersOnDay(date);
            if (commonStatus == (int)OrderStatusValue.Order)
                return state1;
            else if (commonStatus == (int)OrderStatusValue.SentOrder)
                return state2;
            else if (commonStatus == (int)OrderStatusValue.DeliveredOrder)
                return state3;
            else if (commonStatus == (int)OrderStatusValue.NotDeliveredOrder)
                return state4;
            return state5;
        }

        private dynamic GetPosibleStatesForOrders(DateTime date)
        {
            dynamic possibleStates = new JsonArray();     
            DateTime dateForCheck = DateTime.Now;//dateForCheck must be today
            
            
            

            if (date > dateForCheck)
            {
                possibleStates.Add(state1);
                possibleStates.Add(state2);
                possibleStates.Add(state3);
            }

            if (date <= dateForCheck)
            {
                possibleStates.Add(state2);
                possibleStates.Add(state3);
                possibleStates.Add(state4);
            }
            return possibleStates;
        }

        private string OrdersListByDateToJson(List<Order> allOrders, DateTime date)
        {
            dynamic json_all_orders = new JsonArray();
            foreach (var order in allOrders)
                  json_all_orders.Add(Order_(order));
            return json_all_orders.ToString();
        }

        private string OrdersListByDatesToJson(List<Order> allOrders, DateTime startDate, DateTime endDate)
        {
            dynamic json_all_orders = new JsonArray();
            for (int i = 0; i < endDate.Subtract(startDate).Days; i++)
            {
                var day_ = startDate.AddDays(i);
                dynamic day = new JsonObject();
                day.Date = day_;
                day.Orders = new JsonArray();
                foreach (var order in allOrders.Where(o => o.Date.Day == day_.Day && o.Date.Month == day_.Month && o.Date.Year == day_.Year))
                    day.Orders.Add(GetOrderJsonValue(order));
                json_all_orders.Add(day);
            }
            return json_all_orders.ToString();
        }

        private string GetOrderJsonValue(Order order)
        {
            return Order_(order).ToString();
        }

        private dynamic Order_(Order order)
        {
            dynamic dynamic_order = new JsonObject();
            dynamic_order.Id = order.Id;
            dynamic_order.Date = order.Date;
            dynamic_order.CurrentStatus = order.Statuses.SingleOrDefault(s => s.isCurrent).StatusValue;

            dynamic_order.Person = Person_(order.Person);
            
            dynamic details = new JsonArray();
            foreach (OrderDetail detail in order.OrderDetail)
            {
                dynamic json_detail = new JsonObject();
                json_detail.Id = detail.Id;
                json_detail.Quantity = detail.Quantity;
                dynamic product = new JsonObject();
                product.Id = detail.Product.Id;
                product.Price = detail.Product.Price;
                product.Summary = detail.Product.Summary;
                product.Title = detail.Product.Title;
                product.isComplex = detail.Product.isComplex;
                json_detail.Product = product;

                details.Add(json_detail);
            }
            dynamic_order.OrderDetail = details;
            return dynamic_order;
        }

        private dynamic Person_(Person person)
        {
            dynamic dynamic_person = new JsonObject();
            dynamic_person.Id = person.Id;
            dynamic_person.DomainName = person.DomainName;
            dynamic_person.Balance = person.Balance;
            dynamic_person.FullName = person.GetName();
            return dynamic_person;
        }

    }
}
