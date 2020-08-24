using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class GenericRepository<TModel, TEntity> : IGenericRepository<TModel, TEntity>
        where TModel : class where TEntity : class
    {
        #region Fields

        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _baseDbContext;

        #endregion

        #region Constructor

        public GenericRepository(IMapper iMapper, FeatureManagementDbContext baseDbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _baseDbContext = baseDbContext ?? throw new ArgumentNullException(nameof(baseDbContext));
        }

        #endregion

        #region Methods

        #region Get

        public IList<TModel> GetAll()
        {
            List<TEntity> result = _baseDbContext
                .Set<TEntity>()
                .ToList();
            return _iMapper.Map<IList<TModel>>(result);
        }

        public async Task<IList<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<TEntity> result = await _baseDbContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return _iMapper.Map<IList<TModel>>(result);
        }

        public TModel GetById(object id)
        {
            TEntity result = _baseDbContext
                .Set<TEntity>()
                .Find(id);
            return _iMapper.Map<TModel>(result);
        }

        /// <inheritdoc />
        public TModel GetByIdWithNoTracking(object id)
        {
            TEntity result = _baseDbContext
                .Set<TEntity>()
                .Find(id);
            _baseDbContext.Entry(result).State = EntityState.Detached;
            return _iMapper.Map<TModel>(result);
        }

        public async Task<TModel> GetByIdAsync(object id)
        {
            TEntity result = await _baseDbContext.Set<TEntity>().FindAsync(id);
            return _iMapper.Map<TModel>(result);
        }

        /// <inheritdoc />
        public async Task<TModel> GetByIdWithNoTrackingAsync(object id)
        {
            TEntity result = await _baseDbContext.Set<TEntity>().FindAsync(id);
            _baseDbContext.Entry(result).State = EntityState.Detached;
            return _iMapper.Map<TModel>(result);
        }

        #endregion

        #region Add

        public TModel Add(TModel modelToAdd)
        {
            var entityToAdd = _iMapper.Map<TEntity>(modelToAdd);
            EntityEntry<TEntity> result = _baseDbContext.Add(entityToAdd);
            SaveChanges();
            return _iMapper.Map<TModel>(result.Entity);
        }

        public async Task<TModel> AddAsync(TModel modelToAdd, CancellationToken cancellationToken = default)
        {
            var entityToAdd = _iMapper.Map<TEntity>(modelToAdd);
            EntityEntry<TEntity> result = await _baseDbContext.AddAsync(entityToAdd, cancellationToken);
            await SaveChangesAsync().ConfigureAwait(false);
            return _iMapper.Map<TModel>(result.Entity);
        }

        public void AddRange(IList<TModel> modelsToAdd)
        {
            var entitiesToAdd = _iMapper.Map<TEntity>(modelsToAdd);
            _baseDbContext.AddRange(entitiesToAdd);
            SaveChanges();
        }

        public async Task AddRangeAsync(IList<TModel> modelsToAdd)
        {
            var entitiesToAdd = _iMapper.Map<TEntity>(modelsToAdd);
            await _baseDbContext.AddRangeAsync(entitiesToAdd);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Update

        public TModel Update(TModel modelToUpdate)
        {
            var entityToUpdate = _iMapper.Map<TEntity>(modelToUpdate);
            EntityEntry<TEntity> result = _baseDbContext.Update(entityToUpdate);
            SaveChanges();
            return _iMapper.Map<TModel>(result.Entity);
        }

        public void UpdateRange(IList<TModel> modelsToUpdate)
        {
            var entitiesToUpdate = _iMapper.Map<TEntity>(modelsToUpdate);
            _baseDbContext.UpdateRange(entitiesToUpdate);
            SaveChanges();
        }

        #endregion

        #region Delete

        public void DeleteById(object id)
        {
            TModel entityToDelete = GetById(id);
            _baseDbContext.Remove(entityToDelete);
            SaveChanges();
        }

        public void Delete(TModel modelToDelete)
        {
            var entityToDelete = _iMapper.Map<TEntity>(modelToDelete);
            _baseDbContext.Remove(entityToDelete);
            SaveChanges();
        }

        public void DeleteRange(IList<TModel> modelsToDelete)
        {
            var entitiesToDelete = _iMapper.Map<TEntity>(modelsToDelete);
            _baseDbContext.RemoveRange(entitiesToDelete);
            SaveChanges();
        }

        #endregion

        #region Global

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return _baseDbContext.Set<TEntity>().Any(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _baseDbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _baseDbContext.SaveChanges();
        }

        #endregion

        #endregion
    }
}