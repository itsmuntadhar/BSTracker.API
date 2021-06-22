using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BSTracker.Entities;
using BSTracker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BSTracker.Repositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected const int Limit = 25;
        protected readonly IDbContext Context;

        public BaseRepository(IDbContext context)
            => Context = context;

        public virtual void Add(T t)
            => Context.GetDbSet<T>().Add(t);

        public virtual void Add(IEnumerable<T> ts)
            => Context.GetDbSet<T>().AddRange(ts);

        public virtual T Get(string id)
            => Context.GetDbSet<T>()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        public virtual IEnumerable<T> Get(int offset = 0)
            => Get(null, null, offset);

        public virtual IEnumerable<T> Get(Expression<Func<T, object>> orderBy, int offset = 0)
            => Get(orderBy, null, offset);

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate, int offset = 0)
            => Get(null, predicate, offset);

        public virtual IEnumerable<U> GetWithSelector<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> selector, int offset = 0)
            => GetQuery(null, predicate, offset)
            .Select(selector)
            .AsEnumerable();

        public virtual IEnumerable<T> Get(Expression<Func<T, object>> orderBy = null, Expression<Func<T, bool>> predicate = null, int offset = 0)
            => GetQuery(orderBy, predicate, offset)
            .AsEnumerable();

        private IQueryable<T> GetQuery(Expression<Func<T, object>> orderBy = null, Expression<Func<T, bool>> predicate = null, int offset = 0)
        {
            var query = Context.GetDbSet<T>()
                .AsQueryable();
            if (orderBy is null)
                query = query.OrderBy(x => x.Id);
            else
                query = query.OrderBy(orderBy);

            if (predicate is not null)
                query = query.Where(predicate);
            if (offset >= 0)
                query = query.Skip(offset);
            return query.Take(25);
        }

        public virtual void Update(T t)
            => Context.GetDbSet<T>().Update(t);

        public virtual void Update(IEnumerable<T> ts)
            => Context.GetDbSet<T>().UpdateRange(ts);

        public virtual void Remove(T t)
            => Context.GetDbSet<T>().Remove(t);

        public virtual void Remove(IEnumerable<T> ts)
            => Context.GetDbSet<T>().RemoveRange(ts);

        public virtual int SaveChanges()
            => Context.SaveChanges();
    }
}
