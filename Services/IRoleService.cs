using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;

namespace DigiBlog.Api.Services
{
	public interface IRoleService
	{
		Task CreateAsync(Role role);
		Task UpdateAsync(Role role);
		Task DeleteAsync(string id);

		Task<IEnumerable<Role>> ReadAllAsync();
		Task<Role> ReadSingleAsync(string id);
		Task<IEnumerable<Role>> WhereAsync(Expression<Func<Role, bool>> where);
		Task<IEnumerable<Role>> OrderByAsync<TKey>(Expression<Func<Role, TKey>> orderBy, bool isDesc);
	}
}