using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;

namespace DigiBlog.Api.Services
{
	public interface ICommentService
	{
		Task CreateAsync(Comment comment);
		Task UpdateAsync(Comment comment);
		Task DeleteAsync(string id);

		Task<IEnumerable<Comment>> ReadAllAsync();
		Task<Comment> ReadSingleAsync(string id);
		Task<IEnumerable<Comment>> WhereAsync(Expression<Func<Comment, bool>> where);
		Task<IEnumerable<Comment>> OrderByAsync<TKey>(Expression<Func<Comment, TKey>> orderBy, bool isDesc);
	}
}