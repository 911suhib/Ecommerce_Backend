using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceBackend.Domain.src.Abstractions
{
	public interface IBaseRepository<TEntity> where TEntity : BaseEntity
	{
		Task<IEnumerable<TEntity>> GetAllAsync(QueryOptions queryOptions);
		Task<TEntity> AddAsync(TEntity entity);

		Task<TEntity> UpdateAsync(int entityId,TEntity entity);

		Task<bool> DeleteByIdAsync(int entityId);
		Task<TEntity> GetByIdAsync(int entityId);
		
	}
}
