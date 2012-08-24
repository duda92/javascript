using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DA.Dinners.Domain.Abstract
{
    public interface IDebitOperationRepository : IDisposable
    {
        IQueryable<DebitOperation> All { get; }
        IQueryable<DebitOperation> AllIncluding(params Expression<Func<DebitOperation, object>>[] includeProperties);
        DebitOperation Find(int id);
        void InsertOrUpdate(DebitOperation debitoperation);
        void Delete(int id);
        void Save();
    }
}
