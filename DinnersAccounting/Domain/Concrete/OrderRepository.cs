using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DA.Dinners.Domain;
using DA.Dinners.Domain.Abstract;

namespace DA.Dinners.Domain.Concrete
{ 
    public class OrderRepository : IOrderRepository
    {
        DADinnersDomainContext context = new DADinnersDomainContext();

        public IQueryable<Order> All
        {
            get { return context.Orders; }
        }

        public IQueryable<Order> AllIncluding(params Expression<Func<Order, object>>[] includeProperties)
        {
            IQueryable<Order> query = context.Orders;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Order Find(int id)
        {
            return context.Orders.Find(id);
        }

        public void InsertOrUpdateWithPerson(Order order, string domainName)
        {
            if (order.Id == default(int))
            {
                Person person = context.People.SingleOrDefault(p => p.DomainName == domainName);
                order.Person = person;

                foreach (var detail in order.OrderDetail)
                {
                    detail.Product = context.Products.Find(detail.Product.Id);                    
                }

                order.Statuses.Add(new OrderStatus { Date = DateTime.Now, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });

                //decimal amount = order.OrderDetail.Sum(od => od.Product.Price * od.Quantity);
                //person.Operations.Add(new CreditOperation { Date = DateTime.Now, Order = order, Amount = amount, Summary = "Заказ" });
                //person.CalculateBalance();
                context.Orders.Add(order);
            }
            else
            {
                //Person person = context.People.SingleOrDefault(p => p.DomainName == domainName);
                //order.Person = person;

                foreach (var detail in order.OrderDetail)
                {
                    detail.Product = context.Products.Find(detail.Product.Id);
                    context.Entry(detail).State = EntityState.Modified;
                    if (detail.Id == default(int))
                    {
                        context.OrderDetails.Add(detail);
                    }
                }
                //CreditOperation operation = context.CreditOperations.SingleOrDefault(o => o.Order.Id == order.Id);
                //operation.Amount = order.OrderDetail.Sum(od => od.Product.Price * od.Quantity);
                //order.Person.CalculateBalance();
                //context.Entry(operation).State = EntityState.Modified;

                context.Entry(order).State = EntityState.Modified;
            }
        }
        
        public void InsertOrUpdate(Order order)
        {
            if (order.Id == default(int)) {
                // New entity
                foreach (var detail in order.OrderDetail)
                {
                    detail.Product = context.Products.Find(detail.Product.Id);
                }

                order.Statuses.Add(new OrderStatus { Date = DateTime.Now, isCurrent = true, StatusValue = (int)OrderStatusValue.Order });

                context.Orders.Add(order);
            } else {
                foreach (var detail in order.OrderDetail)
                {
                    detail.Product = context.Products.Find(detail.Product.Id);
                    context.Entry(detail).State = EntityState.Modified;
                    if (detail.Id == default(int))
                    {
                        context.OrderDetails.Add(detail);
                    }
                }
                context.Entry(order).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var order = context.Orders.Find(id);
            if (order != null)
            {
                CreditOperation oper = context.CreditOperations.SingleOrDefault(o => o.Order.Id == id);
                if (oper != null)
                    context.CreditOperations.Remove(oper);
                for (int i = 0; i < order.OrderDetail.Count; i++)
                    context.OrderDetails.Remove(order.OrderDetail[i]);

                for (int i = 0; i < order.Statuses.Count; i++)
                    context.OrderStatus.Remove(order.Statuses[i]);
            
                context.Orders.Remove(order);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
  
        public void DeletedDetails(Order order, IEnumerable<int> deleted_detail_list)
        {
            for (int i = 0; i < deleted_detail_list.Count() - 1; i++)
            {
                OrderDetail detail = context.OrderDetails.Find(deleted_detail_list.ElementAt(i));
                if (detail != null)
                    context.OrderDetails.Remove(detail);
            }
            //CreditOperation cp = context.CreditOperations.SingleOrDefault(o => o.Order.Id == order.Id);
            //if (cp != null)
            //    cp.Amount = order.OrderDetail.Sum(od => od.Product.Price * od.Quantity);
            //order.Person.CalculateBalance();
            
            if (order.OrderDetail.Count == 0)
            {
                order.Statuses.Clear();
                context.Orders.Remove(order);
            }
        }
    }

    
}