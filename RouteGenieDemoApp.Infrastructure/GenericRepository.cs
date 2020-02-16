using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure
{
    public class GenericRepository<TEntity> where TEntity : class
    {

        internal DbEntities _Context;
        internal DbSet<TEntity> _DbSet;
        internal Expression<Func<TEntity, bool>> _GlobalFilter;
        internal Func<TEntity, bool> MatchesFilter { get; private set; }

        public GenericRepository(DbEntities context)
        {
            this._Context = context;
            this._DbSet = context.Set<TEntity>();
        }

        public GenericRepository(DbEntities context, Expression<Func<TEntity, bool>> filter)
        {
            this._Context = context;
            this._DbSet = context.Set<TEntity>();
            this._GlobalFilter = filter;
            this.MatchesFilter = filter.Compile();
        }

        #region -- Public Methods --

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties);

            if (skip.HasValue && skip.Value > 0) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            return query;
        }

        public TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties);
            return query.FirstOrDefault();
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = BuildQuery(filter, null, "");
            return query.Count();
        }

        public TEntity GetByID(object id)
        {
            var entity = _DbSet.Find(id);
            ThrowIfEntityDoesNotMatchFilter(entity);
            return entity;
        }

        public void Insert(TEntity entity)
        {
            ThrowIfEntityDoesNotMatchFilter(entity);
            _DbSet.Add(entity);
        }

        public void InsertRange(IList<TEntity> entities)
        {
            foreach (var e in entities)
                ThrowIfEntityDoesNotMatchFilter(e);

            _DbSet.AddRange(entities);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _DbSet.Find(id);
            ThrowIfEntityDoesNotMatchFilter(entityToDelete);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            ThrowIfEntityDoesNotMatchFilter(entityToDelete);
            if (_Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _DbSet.Attach(entityToDelete);
            }
            _DbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {

            try
            {
                _DbSet.Attach(entityToUpdate);
                _Context.Entry(entityToUpdate).State = EntityState.Modified;
                _Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                var entry = ex.Entries.Single();
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
            }





        }

        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                    return _DbSet.SqlQuery(query).ToList();
                else return _DbSet.SqlQuery(query, parameters).ToList();
            }
            catch  
            {
                return new List<TEntity>();
            }
        }


        #region -- Async Methods --

        public async Task<List<TEntity>> AsyncGet(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties);

            if (skip.HasValue && skip.Value > 0) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<TEntity> AsyncGetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = BuildQuery(filter, orderBy, includeProperties);
            return await query.FirstOrDefaultAsync();
        }


        public async Task<int> AsyncGetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = BuildQuery(filter, null, "");
            return await query.CountAsync();
        }

        public async Task<TEntity> AsyncGetByID(object id)
        {
            var entity = await _DbSet.FindAsync(id);
            ThrowIfEntityDoesNotMatchFilter(entity);
            return entity;
        }

        public async Task<int> AsyncInsert(TEntity entity)
        {
            ThrowIfEntityDoesNotMatchFilter(entity);
            _DbSet.Add(entity);
            var result = await _Context.SaveChangesAsync();

            return result;
        }

        public async Task AsyncInsertRange(IList<TEntity> entities)
        {
            foreach (var e in entities)
                ThrowIfEntityDoesNotMatchFilter(e);

            _DbSet.AddRange(entities);
            await _Context.SaveChangesAsync();
        }

        public async Task<int> AsyncDelete(object id)
        {
            TEntity entityToDelete = _DbSet.Find(id);
            ThrowIfEntityDoesNotMatchFilter(entityToDelete);
            var result = await AsyncDelete(entityToDelete);

            return result;
        }

        public async Task<int> AsyncDelete(TEntity entityToDelete)
        {

            if (_Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _DbSet.Attach(entityToDelete);
            }
            _DbSet.Remove(entityToDelete);

            return await _Context.SaveChangesAsync();
        }

        public async Task<int> AsyncUpdate(TEntity entityToUpdate)
        {
            ThrowIfEntityDoesNotMatchFilter(entityToUpdate);
            _DbSet.Attach(entityToUpdate);
            _Context.Entry(entityToUpdate).State = EntityState.Modified;

            return await _Context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> AsyncGetWithRawSql(string query, params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                    return await _DbSet.SqlQuery(query).ToListAsync();
                else return await _DbSet.SqlQuery(query, parameters).ToListAsync();
            }
            catch  
            {
                return new List<TEntity>();
            }
        }

        #endregion

        #endregion

        #region -- Private Methods --

        private IQueryable<TEntity> BuildQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _DbSet;

            if (_GlobalFilter != null)
            {
                query = query.Where(_GlobalFilter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        private void ThrowIfEntityDoesNotMatchFilter(TEntity entity)
        {
            if (_GlobalFilter != null && !MatchesFilter(entity))
                throw new ArgumentOutOfRangeException();
        }

        #endregion
    }
}
