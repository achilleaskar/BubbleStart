using BubbleStart.Migrations;
using BubbleStart.Model;
using System;
using System.Data.Entity;

namespace BubbleStart.Database
{
    // Code-Based Configuration and Dependency resolution
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
   // [DbConfigurationType(typeof(ContextConfiguration))]
    public class MainDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Apointment> Apointments { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ShowUp> ShowUps { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public MainDatabase() : base(normal)
        {
           // DbConfiguration.SetConfiguration(new ContextConfiguration());
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = false;

        }
        private const string normal = "Server=server19.cretaforce.gr;Database=readmore_achill2;pooling=true;Uid=readmore_achill2;Pwd=986239787346;Convert Zero Datetime=True; CharSet=utf8; default command timeout=3600;SslMode=none;";

        public DateTime Limit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
            .Configure(s => s.HasMaxLength(200).HasColumnType("varchar"));
            modelBuilder.Properties().Where(x => x.PropertyType == typeof(bool))
             .Configure(x => x.HasColumnType("bit"));
            //modelBuilder.Entity<ShowUp>()
            //        .Property(p => p.Arrive)
            //        .HasColumnType("bit");
            //modelBuilder.Entity<Illness>()
            //   .HasRequired(s => s.Customer)
            //   .WithRequiredPrincipal(ad => ad.Illness);
            base.OnModelCreating(modelBuilder);
        }
    }
}