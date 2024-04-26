using MediNet_BE.Models;
using MediNet_BE.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediNet_BE.Data
{
    public class MediNetContext : DbContext
	{
        public MediNetContext(DbContextOptions<MediNetContext> options) : base(options) { }

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CategoryChild> CategoryChilds { get; set; }
		public DbSet<Clinic> Clinics { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderService> OrderServices { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Service> Services { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder){ }

	}
}
