using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IBaseService<TModel> where TModel : class, new()
    {
        #region Get
        TModel GetById(object id);
        TModel GetByIdWithNoTracking(object id);
        Task<TModel> GetByIdAsync(object id);
        Task<TModel> GetByIdWithNoTrackingAsync(object id);
        IList<TModel> GetAll();
        Task<IList<TModel>> GetAllAsync();
        #endregion

        #region Add
        TModel Add(TModel modelToAdd);
        Task<TModel> AddAsync(TModel modelToAdd);
        void AddRange(IList<TModel> modelsToAdd);
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
        bool Exist(Expression<Func<object, bool>> predicate);
        #endregion
    }
}