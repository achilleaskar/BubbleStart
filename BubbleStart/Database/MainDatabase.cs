using BubbleStart.Model;
using MySql.Data.Entity;
using System.Data.Entity;

namespace BubbleStart.Database
{
    // Code-Based Configuration and Dependency resolution
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MainDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<District> Districts { get; set; }

        public MainDatabase() : base("BubbleDatabase")
        {
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
                .Configure(s => s.HasMaxLength(200).HasColumnType("varchar"));
            base.OnModelCreating(modelBuilder);
        }
    }
}