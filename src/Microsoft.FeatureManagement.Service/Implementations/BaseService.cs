using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class BaseService<TModel> : IBaseService<TModel>
        where TModel : class, new()
    {
        #region Fields

        private readonly IBaseRepository<TModel> _iBaseRepository;

        #endregion

        #region Constructor

        public BaseService(IBaseRepository<TModel> iBaseRepository)
        {
            _iBaseRepository = iBaseRepository ?? throw new ArgumentNullException(nameof(iBaseRepository));
        }

        #endregion

        #region Methods

        public TModel Add(TModel modelToAdd)
        {
            if (modelToAdd == null)
            {
                throw new ArgumentNullException(nameof(modelToAdd));
            }

            return _iBaseRepository.Add(modelToAdd);
        }

        public async Task<TModel> AddAsync(TModel modelToAdd)
        {
            if (modelToAdd == null)
            {
                throw new ArgumentNullException(nameof(modelToAdd));
            }

            return await _iBaseRepository.AddAsync(modelToAdd);
        }

        public void AddRange(IList<TModel> modelsToAdd)
        {
            if (modelsToAdd == null)
            {
                throw new ArgumentNullException(nameof(modelsToAdd));
            }

            if (modelsToAdd.Count == 0)
            {
                throw new ArgumentException();
            }

            _iBaseRepository.AddRange(modelsToAdd);
        }

        public void Delete(TModel modelToDelete)
        {
            if (modelToDelete == null)
            {
                throw new ArgumentNullException(nameof(modelToDelete));
            }

            _iBaseRepository.Delete(modelToDelete);
        }

        public void DeleteById(object id)
        {
            ValidateId(id);
            _iBaseRepository.DeleteById(id);
        }

        public void DeleteRange(IList<TModel> modelsToDelete)
        {
            if (modelsToDelete == null)
            {
                throw new ArgumentNullException(nameof(modelsToDelete));
            }

            if (modelsToDelete.Count == 0)
            {
                throw new ArgumentException();
            }

            _iBaseRepository.DeleteRange(modelsToDelete);
        }

        public bool Exist(Expression<Func<object, bool>> predicate)
        {
            return _iBaseRepository.Exist(predicate);
        }

        public IList<TModel> GetAll()
        {
            return _iBaseRepository.GetAll();
        }

        public async Task<IList<TModel>> GetAllAsync()
        {
            return await _iBaseRepository.GetAllAsync();
        }

        public TModel GetById(object id)
        {
            ValidateId(id);
            return _iBaseRepository.GetById(id);
        }

        /// <inheritdoc />
        public TModel GetByIdWithNoTracking(object id)
        {
            ValidateId(id);
            return _iBaseRepository.GetByIdWithNoTracking(id);
        }

        public async Task<TModel> GetByIdAsync(object id)
        {
            ValidateId(id);
            return await _iBaseRepository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<TModel> GetByIdWithNoTrackingAsync(object id)
        {
            ValidateId(id);
            return await _iBaseRepository.GetByIdWithNoTrackingAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _iBaseRepository.SaveChangesAsync();
        }

        public TModel Update(TModel modelToUpdate)
        {
            return _iBaseRepository.Update(modelToUpdate);
        }

        public void UpdateRange(IList<TModel> modelsToUpdate)
        {
            _iBaseRepository.UpdateRange(modelsToUpdate);
        }

        private static void ValidateId(object id)
        {
            switch (id)
            {
                case Guid guid when guid == Guid.Empty:
                case int i when i == 0:
                case string s when string.IsNullOrEmpty(s):
                    throw new ArgumentException("Invalid Id");
            }
        }

        #endregion
    }
}