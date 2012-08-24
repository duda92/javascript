using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IOrderRepository : IDisposable
    {
        IQueryable<Order> All { get; }
        IQueryable<Order> AllIncluding(params Expression<Func<Order, object>>[] includeProperties);
        Order Find(int id);
        void InsertOrUpdateWithPerson(Order order, string domainName);
        void DeletedDetails(Order order, IEnumerable<int> deleted_detail_list);
        void InsertOrUpdate(Order order);
        void Delete(int id);
        void Save();

    }
}
