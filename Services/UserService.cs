using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiBlog.Api.Models;
using DigiBlog.Api.Repositories;
using System.Linq.Expressions;
using DigiBlog.Api.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace DigiBlog.Api.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> repository;
		private readonly AuthSettings authSettings;

		public UserService(IRepository<User> repository, IOptions<AuthSettings> authSettings)
		{
			this.repository = repository;
			this.authSettings = authSettings.Value;
		}

		public async Task CreateAsync(User user)
		{
			user.Id = GuidHelper.NewId();
			byte[] passwordHash, passwordSalt;
			Security.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
			user.Password = null;
			user.CreatedAt = DateTime.UtcNow;
			user.ModifiedAt = DateTime.UtcNow;
			if (string.IsNullOrWhiteSpace(user.Status))
			{
				user.Status = Status.Active;
			}
			await repository.CreateAsync(user);
		}

		public async Task UpdateAsync(User user)
		{
			var entity = await repository.ReadSingleAsync(user.Id);
			if (entity != null)
			{
				if (!string.IsNullOrWhiteSpace(user.Username))
				{
					entity.Username = user.Username;
				}
				if (!string.IsNullOrWhiteSpace(user.Email))
				{
					entity.Email = user.Email;
				}
				if (!string.IsNullOrEmpty(user.Password))
				{
					byte[] passwordHash, passwordSalt;
					Security.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
					entity.PasswordHash = passwordHash;
					entity.PasswordSalt = passwordSalt;
					entity.Password = null;
				}
				if (!string.IsNullOrWhiteSpace(user.FullName))
				{
					entity.FullName = user.FullName;
				}
				if (!string.IsNullOrWhiteSpace(user.RoleId))
				{
					entity.RoleId = user.RoleId;
				}
				if (!string.IsNullOrWhiteSpace(user.Status))
				{
					entity.Status = user.Status;
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

		public async Task<IEnumerable<User>> ReadAllAsync()
		{
			return await repository.ReadAllAsync();
		}

		public async Task<User> ReadSingleAsync(string id)
		{
			return await repository.ReadSingleNestedAsync(id);
		}

		public async Task<IEnumerable<User>> WhereAsync(Expression<Func<User, bool>> where)
		{
			return await repository.WhereAsync(where);
		}

		public async Task<IEnumerable<User>> OrderByAsync<TKey>(Expression<Func<User, TKey>> orderBy, bool isDesc)
		{
			return await repository.OrderByAsync(orderBy, isDesc);
		}

		public User Authenticate(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				return null;
			}
			var user = repository.WhereAsync(x => x.Username == username || x.Email == username).Result.SingleOrDefault();

			// check if username exists
			if (user == null)
			{
				return null;
			}
			// check if password is correct
			if (!Security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			{
				return null;
			}

			var key = Encoding.UTF8.GetBytes(authSettings.Secret);
			var symmetricKey = new SymmetricSecurityKey(key);
			var signingCredential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

			var token = new JwtSecurityToken(
				issuer: authSettings.Issuer,
				audience: authSettings.Audience,
				expires: DateTime.UtcNow.AddSeconds(authSettings.Expires),
				signingCredentials: signingCredential,
				claims: claims
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
			user.Token = tokenString;
			// authentication successful
			return user;
		}
	}
}