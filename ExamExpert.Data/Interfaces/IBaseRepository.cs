using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExamExpert.Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        bool Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int entityId);
        IQueryable<TEntity> GetAll();
        TEntity GetByAnyData(Expression<Func<TEntity, bool>> expression); //(x => x.Id == "5" etc.)
    }
}
