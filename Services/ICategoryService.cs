using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;

namespace DigiBlog.Api.Services
{
	public interface ICategoryService
	{
		Task CreateAsync(Category category);
		Task UpdateAsync(Category category);
		Task DeleteAsync(string id);

		Task<IEnumerable<Category>> ReadAllAsync();
		Task<Category> ReadSingleAsync(string id);
		Task<IEnumerable<Category>> WhereAsync(Expression<Func<Category, bool>> where);
		Task<IEnumerable<Category>> OrderByAsync<TKey>(Expression<Func<Category, TKey>> orderBy, bool isDesc);
	}
}