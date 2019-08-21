using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigiBlog.Api.Repositories
{
	public interface IRepository<T> where T : class
	{
		Task CreateAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);

		Task<IEnumerable<T>> ReadAllAsync();
		Task<T> ReadSingleAsync(string id);
		Task<T> ReadSingleNestedAsync(string id);
		Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> where);
		Task<IEnumerable<T>> OrderByAsync<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
	}
}