using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DA.Dinners.Model;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IDayPropositionRepository : IDisposable
    {
        IQueryable<DayProposition> All { get; }
        IQueryable<DayProposition> AllIncluding(params Expression<Func<DayProposition, object>>[] includeProperties);
        DayProposition Find(int id);
        void InsertOrUpdate(DayProposition dayproposition);
        void Delete(int id);
        void Save();
    }
}
