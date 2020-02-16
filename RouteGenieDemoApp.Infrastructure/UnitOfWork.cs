using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteGenieDemoApp.Infrastructure.Entity;

namespace RouteGenieDemoApp.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private DbEntities _Context;
        private string _Username;

        #region -- Constructor(s) --

        public UnitOfWork(string username)
        {
            try
            {
                _Context = new DbEntities(username);
                _Username = username;
            }
            catch (Exception ex)
            {
                var ahh = "Unit of Work Foundation Error!";
            }
        }

        public DbEntities Context
        {
            get { return _Context; }
        }

        public void RefreshConnection()
        {
            _Context.Dispose();
            _Context = new DbEntities(_Username);
        }

        #endregion




        #region --  Unfiltered Repositoires  --

        public Dictionary<Type, object> _Repositories = new Dictionary<Type, object>();
        public GenericRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase
        {
            // Check to see if we have a constructor for the given type
            if (!_Repositories.ContainsKey(typeof(TEntity)))
            {
                _Repositories.Add(typeof(TEntity), new GenericRepository<TEntity>(_Context, o => o.IsDeleted == false));
            }
            return _Repositories[typeof(TEntity)] as GenericRepository<TEntity>;
        }

        #endregion

        #region -- Raw SQL --

        public List<T> GetListWithRawSql<T>(string sql, params object[] parameters)
        {
            return _Context.Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public T GetSingleWithRawSql<T>(string sql, params object[] parameters)
        {
            return _Context.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        public void ExecuteRawSql(string sql, params object[] parameters)
        {
            _Context.Database.ExecuteSqlCommand(sql, parameters);
        }

        #endregion

        public void Save()
        {
            _Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _Context.SaveChangesAsync();
        }

        private bool _Disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing && _Context != null)
                {
                    _Context.Dispose();
                }
            }
            this._Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
