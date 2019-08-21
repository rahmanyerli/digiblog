using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System;
using Microsoft.EntityFrameworkCore;

namespace DigiBlog.Api.Models
{
	public partial class AppDbContext : DbContext
	{
		public AppDbContext()
		{
		}

		public AppDbContext(DbContextOptions<AppDbContext> options)
			 : base(options)
		{
		}

		public virtual DbSet<Category> Category { get; set; }
		public virtual DbSet<Article> Article { get; set; }
		public virtual DbSet<Role> Role { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<Comment> Comment { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

			// Eager Loading...
			// modelBuilder.Entity<Article>().HasOne(article => article.Category).WithMany(category => category.Articles);
			// modelBuilder.Entity<User>().HasOne(u => u.Role);

			modelBuilder.Entity<Category>(entity =>
			{
				entity.Property(e => e.Id)
					.HasMaxLength(32)
					.IsUnicode(false)
					.ValueGeneratedNever();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.Status)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
			});

			modelBuilder.Entity<Article>(entity =>
			{
				entity.Property(e => e.Id)
					.HasMaxLength(32)
					.IsUnicode(false)
					.ValueGeneratedNever();

				entity.Property(e => e.CategoryId)
					.IsRequired()
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.UserId)
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.Text)
					.IsRequired()
					.IsUnicode(false);

				entity.Property(e => e.ImageUrl)
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.Status)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
			});

			modelBuilder.Entity<Role>(entity =>
			{
				entity.Property(e => e.Id)
					.HasMaxLength(32)
					.IsUnicode(false)
					.ValueGeneratedNever();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.Status)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(e => e.Id)
					.HasMaxLength(32)
					.IsUnicode(false)
					.ValueGeneratedNever();

				entity.Property(e => e.Username)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.FullName)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);


				entity.Property(e => e.Email)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.Property(e => e.PasswordHash)
					.IsRequired()
					.HasMaxLength(64)
					.IsUnicode(false);

				entity.Property(e => e.PasswordSalt)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.RoleId)
					.IsRequired()
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.Status)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
			});

			modelBuilder.Entity<Comment>(entity =>
			{
				entity.Property(e => e.Id)
					.HasMaxLength(32)
					.IsUnicode(false)
					.ValueGeneratedNever();

				entity.Property(e => e.ArticleId)
					.IsRequired()
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.UserId)
					.IsRequired()
					.HasMaxLength(32)
					.IsUnicode(false);

				entity.Property(e => e.Text)
					.IsUnicode(false);

				entity.Property(e => e.Rating);

				entity.Property(e => e.Status)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");

				entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
			});
		}
	}
}
