using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DA.Dinners.Model;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IProductRepository : IDisposable
    {
        IQueryable<Product> All { get; }
        IQueryable<Product> AllIncluding(params Expression<Func<Product, object>>[] includeProperties);
        Product Find(int id);
        void InsertOrUpdate(Product product);
        void Delete(int id);
        void Save();
    }
}
