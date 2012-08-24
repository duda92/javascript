using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IOrderDetailRepository : IDisposable
    {
        IQueryable<OrderDetail> All { get; }
        IQueryable<OrderDetail> AllIncluding(params Expression<Func<OrderDetail, object>>[] includeProperties);
        OrderDetail Find(int id);
        void InsertOrUpdate(OrderDetail orderdetail);
        void Delete(int id);
        void Save();
    }
}
