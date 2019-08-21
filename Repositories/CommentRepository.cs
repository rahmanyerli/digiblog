using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Models;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace DigiBlog.Api.Repositories
{
	public class CommentRepository : BaseRepository, IRepository<Comment>
	{
		public CommentRepository(AppDbContext context) : base(context)
		{
		}

		public async Task CreateAsync(Comment comment)
		{
			await context.Comment.AddAsync(comment);
			await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Comment comment)
		{
			context.Comment.Update(comment);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Comment comment)
		{
			context.Comment.Remove(comment);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Comment>> ReadAllAsync()
		{
			return await context.Comment.ToListAsync();
		}

		public async Task<Comment> ReadSingleAsync(string id)
		{
			return await context.Comment
				.SingleOrDefaultAsync(comment => comment.Id == id);
		}

		public async Task<Comment> ReadSingleNestedAsync(string id)
		{
			return await context.Comment
				.Include(comment => comment.User)
				.SingleOrDefaultAsync(comment => comment.Id == id);
		}

		public async Task<IEnumerable<Comment>> WhereAsync(Expression<Func<Comment, bool>> where)
		{
			return await context.Comment
				.Where(where)
				.ToListAsync();
		}

		public async Task<IEnumerable<Comment>> OrderByAsync<TKey>(Expression<Func<Comment, TKey>> orderBy, bool isDesc)
		{
			if (isDesc)
			{
				return await context.Comment
					.OrderByDescending(orderBy)
					.ToListAsync();
			}
			else
			{
				return await context.Comment
					.OrderBy(orderBy)
					.ToListAsync();
			}
		}
	}
}