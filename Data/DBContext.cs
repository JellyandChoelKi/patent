using Microsoft.EntityFrameworkCore;
using K2GGTT.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace K2GGTT.Data
{
	public class DBContext : DbContext
	{
		public DBContext(DbContextOptions<DBContext> options) : base(options)
		{
		}

		public virtual DbSet<Member> Member { get; set; }
		public virtual DbSet<HotTech> HotTech { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			if (!optionsBuilder.IsConfigured)
			{
				var config = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.Build();

				var connectionString = config.GetConnectionString("DefaultConnection");
				optionsBuilder.UseSqlServer(connectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Member>().ToTable("Member");
		}
	}
}
