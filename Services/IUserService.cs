using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;

namespace DigiBlog.Api.Services
{
	public interface IUserService
	{
		Task CreateAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(string id);

		Task<IEnumerable<User>> ReadAllAsync();
		Task<User> ReadSingleAsync(string id);
		Task<IEnumerable<User>> WhereAsync(Expression<Func<User, bool>> where);
		Task<IEnumerable<User>> OrderByAsync<TKey>(Expression<Func<User, TKey>> orderBy, bool isDesc);

		User Authenticate(string username, string password);
	}
}