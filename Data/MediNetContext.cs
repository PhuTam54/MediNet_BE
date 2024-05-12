using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Clinics;
using MediNet_BE.Models.Employees;
using MediNet_BE.Models.Employees.Courses;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediNet_BE.Data
{
    public class MediNetContext : DbContext
	{
		public MediNetContext(DbContextOptions<MediNetContext> options) : base(options) { }

		#region Users
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		#endregion
		#region Categories
		public DbSet<CategoryParent> CategoryParents { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CategoryChild> CategoryChilds { get; set; }
		#endregion

		#region Orders
		public DbSet<Cart> Carts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderService> OrderServices { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductDetail> ProductDetails { get; set; }
		public DbSet<Service> Services { get; set; }
		#endregion

		#region Employees
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Disease> Diseases { get; set; }
		public DbSet<Specialist> Specialists { get; set; }

		#endregion

		#region Clinics
		public DbSet<Clinic> Clinics { get; set; }
		public DbSet<Supply> Supplies { get; set; }
		#endregion

		#region Courses
		public DbSet<Course> Courses { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CategoryParent>().ToTable("CategoryParents")
				.HasIndex(cp => cp.Slug).IsUnique();
			modelBuilder.Entity<Category>().ToTable("Categories")
				.HasIndex(c => c.Slug).IsUnique();
			modelBuilder.Entity<CategoryChild>().ToTable("CategoryChilds")
			 .HasIndex(p => p.Slug).IsUnique();
			modelBuilder.Entity<Product>().ToTable("Products")
				.HasIndex(p => p.Slug).IsUnique();
			modelBuilder.Entity<Order>().ToTable("Orders")
				.HasIndex(p => p.OrderCode).IsUnique();
			modelBuilder.Entity<Clinic>().ToTable("Clinics")
				.HasIndex(p => p.Slug).IsUnique();
			modelBuilder.Entity<Course>().ToTable("Courses")
				.HasIndex(c => c.Title).IsUnique();
			modelBuilder.Entity<Course>()
				.HasOne(e => e.Employee)
				.WithMany(c => c.Courses)
				.HasForeignKey(e => e.EmployeeId)
				.OnDelete(DeleteBehavior.NoAction);
		}

	}
}
