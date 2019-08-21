using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Repositories;
using DigiBlog.Api.Helpers;

namespace DigiBlog.Api.Services
{
	public class CommentService : ICommentService
	{
		private readonly IRepository<Comment> repository;

		public CommentService(IRepository<Comment> repository)
		{
			this.repository = repository;
		}

		public async Task CreateAsync(Comment comment)
		{
			comment.Id = GuidHelper.NewId();
			comment.CreatedAt = DateTime.UtcNow;
			comment.ModifiedAt = DateTime.UtcNow;
			comment.Text = System.Net.WebUtility.HtmlEncode(comment.Text);
			if (string.IsNullOrWhiteSpace(comment.Status))
			{
				comment.Status = Status.Active;
			}
			await repository.CreateAsync(comment);
		}

		public async Task UpdateAsync(Comment comment)
		{
			var entity = await repository.ReadSingleAsync(comment.Id);
			if (entity != null)
			{
				if (entity.UserId != comment.UserId)
				{
					throw new AccessViolationException("You are not authorized to modify this record!");
				}
				if (!string.IsNullOrWhiteSpace(comment.Text))
				{
					entity.Text = System.Net.WebUtility.HtmlEncode(comment.Text);
				}
				entity.Rating = comment.Rating;
				if (!string.IsNullOrWhiteSpace(comment.Status))
				{
					entity.Status = comment.Status;
				}
				entity.ModifiedAt = DateTime.UtcNow;
				await repository.UpdateAsync(entity);
			}
			else
			{
				throw new KeyNotFoundException("No data found!");
			}
		}

		public async Task DeleteAsync(string id)
		{
			var entity = await repository.ReadSingleAsync(id);
			if (entity != null)
			{
				await repository.DeleteAsync(entity);
			}
			else
			{
				throw new KeyNotFoundException("No data found!");
			}
		}

		public async Task<IEnumerable<Comment>> ReadAllAsync()
		{
			return await repository.ReadAllAsync();
		}

		public async Task<Comment> ReadSingleAsync(string id)
		{
			var comment = await repository.ReadSingleNestedAsync(id);
			return comment;
		}

		public async Task<IEnumerable<Comment>> WhereAsync(Expression<Func<Comment, bool>> where)
		{
			return await repository.WhereAsync(where);
		}

		public async Task<IEnumerable<Comment>> OrderByAsync<TKey>(Expression<Func<Comment, TKey>> orderBy, bool isDesc)
		{
			return await repository.OrderByAsync(orderBy, isDesc);
		}
	}
}