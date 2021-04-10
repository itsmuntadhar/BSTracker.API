using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BSTracker.Entities;
using BSTracker.Interfaces;
using BSTracker.Repositories;

namespace BSTracker.Services
{
    public abstract class Service<T> : IService<T> where T : Entity
    {
        protected readonly BaseRepository<T> Repo;

        public bool ExplicitSaveChanges { get; set; } = false;

        public Service(BaseRepository<T> repo)
            => Repo = repo;

        public Service(BaseRepository<T> repo, bool explicitSaveChanges) : this(repo)
            => ExplicitSaveChanges = explicitSaveChanges;

        public void Add(T t)
            => Operate(t, (_t) => Repo.Add(_t));

        public void Add(IEnumerable<T> ts)
            => Operate(ts, (_ts) => Repo.Add(_ts));

        public void Remove(T t)
            => Operate(t, (_t) => Repo.Remove(t));

        public void Remove(IEnumerable<T> ts)
            => Operate(ts, (_ts) => Repo.Remove(ts));

        public T Get(string id)
        {
            if (Guid.TryParse(id, out Guid _) == false)
                throw new Exception("Id has to be GUID");
            return Repo.Get(id);
        }

        public IEnumerable<T> Get(int offset)
        {
            if (offset < 0)
                throw new Exception("Offset has be greater than or equal to zero");
            return Repo.Get(offset);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate, int offset)
            => Repo.Get(predicate, offset);

        public IEnumerable<U> GetWithSelector<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> selector, int offset)
            => Repo.GetWithSelector(predicate, selector, offset);

        public int SaveChanges()
            => Repo.SaveChanges();

        public void Update(T t)
            => Operate(t, (_t) => Repo.Update(t));

        public void Update(IEnumerable<T> ts)
            => Operate(ts, (_ts) => Repo.Update(_ts));

        protected void RunInSingleTransaction(Action action)
        {
            ExplicitSaveChanges = true;
            action.Invoke();
            SaveChanges();
            ExplicitSaveChanges = false;
        }

        private void Operate(IEnumerable<T> ts, Action<IEnumerable<T>> action)
        {
            EnsureNotNull(ts);
            action.Invoke(ts);
            SaveChangesIfImplicit();
        }

        private void Operate(T t, Action<T> action)
        {
            EnsureNotNull(t);
            action.Invoke(t);
            SaveChangesIfImplicit();
        }

        private void SaveChangesIfImplicit()
        {
            if (ExplicitSaveChanges == false)
                SaveChanges();
        }

        private static void EnsureNotNull(T t)
        {
            if (t is null)
                throw new ArgumentNullException(nameof(t));
        }

        private static void EnsureNotNull(IEnumerable<T> ts)
        {
            if (ts is null || ts.Any() == false)
                throw new ArgumentNullException(nameof(ts));
            foreach (var t in ts)
                EnsureNotNull(t);
        }
    }
}
