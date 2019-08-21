using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace DigiBlog.Api.Models
{
	public class User
	{
		[Key]
		[MaxLength(32)]
		public string Id { get; set; }

		[MaxLength(100)]
		public string Username { get; set; }

		[MaxLength(100)]
		public string FullName { get; set; }

		[MaxLength(100)]
		public string Email { get; set; }

		[MaxLength(32)]
		[NotMapped]
		public string Password { get; set; }

		[MaxLength(64)]
		public byte[] PasswordHash { get; set; }

		[MaxLength(128)]
		public byte[] PasswordSalt { get; set; }

		public string ImageUrl { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.Date)]
		public DateTime ModifiedAt { get; set; }

		[MaxLength(1)]
		public string Status { get; set; }

		[MaxLength(32)]
		public string RoleId { get; set; }

		[ForeignKey("RoleId")]
		public Role Role { get; set; }

		[NotMapped]
		public string Token { get; set; }
	}
}