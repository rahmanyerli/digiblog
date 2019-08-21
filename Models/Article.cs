using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiBlog.Api.Models
{
	public class Article
	{
		[Key]
		[MaxLength(32)]
		public string Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; }

		public string Text { get; set; }

		public string ImageUrl { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.Date)]
		public DateTime ModifiedAt { get; set; }

		[MaxLength(1)]
		public string Status { get; set; }

		[MaxLength(32)]
		public string CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		public Category Category { get; set; }

		[MaxLength(32)]
		[Required]
		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public IEnumerable<Comment> Comments { get; set; }
	}
}