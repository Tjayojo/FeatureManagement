using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.FeatureManagement.Core.Attributes;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    /// <inheritdoc />
    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
    {
        #region Fields

        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;
        private readonly dynamic _iGenericRepository;

        #endregion

        #region Constructor

        public BaseRepository(IMapper iMapper, FeatureManagementDbContext dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _iGenericRepository = InitGenericRepository();
        }

        #endregion

        #region Methods

        #region Get

        public IList<TModel> GetAll()
        {
            return _iGenericRepository.GetAll();
        }

        public async Task<IList<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _iGenericRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
        }

        public TModel GetById(object id)
        {
            return _iGenericRepository.GetById(id);
        }

        public TModel GetByIdWithNoTracking(object id)
        {
            return _iGenericRepository.GetByIdWithNoTracking(id);
        }

        public async Task<TModel> GetByIdAsync(object id)
        {
            return await _iGenericRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<TModel> GetByIdWithNoTrackingAsync(object id)
        {
            return await _iGenericRepository.GetByIdWithNoTrackingAsync(id);
        }

        #endregion

        #region Add

        public TModel Add(TModel modelToAdd)
        {
            return _iGenericRepository.Add(modelToAdd);
        }

        public async Task<TModel> AddAsync(TModel modelToAdd, CancellationToken cancellationToken = default)
        {
            return await _iGenericRepository.AddAsync(modelToAdd, cancellationToken).ConfigureAwait(false);
        }

        public void AddRange(IList<TModel> modelsToAdd)
        {
            _iGenericRepository.AddRange(modelsToAdd);
        }

        public async Task AddRangeAsync(IList<TModel> modelsToAdd)
        {
            await _iGenericRepository.AddRangeAsync(modelsToAdd).ConfigureAwait(false);
        }

        #endregion

        #region Update

        public TModel Update(TModel modelToUpdate)
        {
            return _iGenericRepository.Update(modelToUpdate);
        }

        public void UpdateRange(IList<TModel> modelsToUpdate)
        {
            _iGenericRepository.UpdateRange(modelsToUpdate);
        }

        #endregion

        #region Delete

        public void DeleteById(object id)
        {
            _iGenericRepository.DeleteById(id);
        }

        public void Delete(TModel modelToDelete)
        {
            _iGenericRepository.Delete(modelToDelete);
        }

        public void DeleteRange(IList<TModel> modelsToDelete)
        {
            _iGenericRepository.DeleteRange(modelsToDelete);
        }

        #endregion

        #region Global

        public bool Exist(Expression<Func<object, bool>> predicate)
        {
            return _iGenericRepository.Exist(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _iGenericRepository.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _iGenericRepository.SaveChanges();
        }

        #endregion

        private dynamic InitGenericRepository()
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(typeof(TModel));
            var entityNameAttr =
                attrs.Where(attr => attr is EntityNameAttribute).FirstOrDefault() as EntityNameAttribute;
            string entityName = entityNameAttr?.EntityName;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type entityType = (from elem in from app in assemblies
                    select (from tip in app.GetTypes()
                        where tip.Name == entityName?.Trim()
                        select tip).FirstOrDefault()
                where elem != null
                select elem).FirstOrDefault();

            Type abstractDAOType = typeof(GenericRepository<,>).MakeGenericType(typeof(TModel), entityType);
            return Activator.CreateInstance(abstractDAOType, _iMapper, _dbContext);
        }

        #endregion
    }
}