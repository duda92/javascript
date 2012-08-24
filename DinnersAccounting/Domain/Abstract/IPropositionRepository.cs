using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DA.Dinners.Model;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IPropositionRepository : IDisposable
    {
        IQueryable<Proposition> All { get; }
        IQueryable<Proposition> AllIncluding(params Expression<Func<Proposition, object>>[] includeProperties);
        Proposition Find(int id);
        void InsertOrUpdate(Proposition proposition);
        void Delete(int id);
        void Save();
    }
}
