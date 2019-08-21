using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiBlog.Api.Models
{
	public class Comment
	{
		[Key]
		[MaxLength(32)]
		public string Id { get; set; }

		public string Text { get; set; }

		[Range(1, 10)]
		[Required]
		public int Rating { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.Date)]
		public DateTime ModifiedAt { get; set; }

		[MaxLength(1)]
		public string Status { get; set; }

		[MaxLength(32)]
		public string ArticleId { get; set; }

		[MaxLength(32)]
		[Required]
		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}