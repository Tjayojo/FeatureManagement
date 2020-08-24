using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        #region Get
        TModel GetById(object id);
        TModel GetByIdWithNoTracking(object id);
        Task<TModel> GetByIdAsync(object id);
        Task<TModel> GetByIdWithNoTrackingAsync(object id);
        IList<TModel> GetAll();
        Task<IList<TModel>> GetAllAsync(CancellationToken cancellationToken = default);
        #endregion

        #region Add
        TModel Add(TModel modelToAdd);
        Task<TModel> AddAsync(TModel modelToAdd, CancellationToken cancellationToken = default);
        void AddRange(IList<TModel> modelsToAdd);
        Task AddRangeAsync(IList<TModel> modelsToAdd);
        #endregion

        #region Update
        TModel Update(TModel modelToUpdate);
        void UpdateRange(IList<TModel> modelsToUpdate);
        #endregion

        #region Delete
        void DeleteById(object id);
        void Delete(TModel modelToDelete);
        void DeleteRange(IList<TModel> modelsToDelete);
        #endregion

        #region Global
        Task<int> SaveChangesAsync();
        bool Exist(Expression<Func<object, bool>> predicate);
        #endregion
    }
}