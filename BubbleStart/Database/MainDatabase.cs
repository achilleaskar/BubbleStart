using System;
using System.Data.Entity;
using BubbleStart.Model;

namespace BubbleStart.Database
{
    // Code-Based Configuration and Dependency resolution
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    // [DbConfigurationType(typeof(ContextConfiguration))]
    public class MainDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseCategoryClass> ExpenseCategoryClasses { get; set; }
        public DbSet<WorkingRule> WorkingRules { get; set; }
        public DbSet<DayWorkingShift> DailyWorkingShifts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<GymnastHour> GymnastHours { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Apointment> Apointments { get; set; }
        public DbSet<CustomeTime> CustomeTimes { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ShowUp> ShowUps { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ItemPurchase> ItemPurchases { get; set; }
        public DbSet<ClosedHour> ClosedHours { get; set; }

        public MainDatabase() : base(normal)
        {
            // DbConfiguration.SetConfiguration(new ContextConfiguration());
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = false;

        }
        private const string normal = "Server=server19.cretaforce.gr;Database=readmore_achill2;pooling=true;user id=readmore_achill2;Pwd=986239787346;Convert Zero Datetime=True; default command timeout=3600;SslMode=none;TreatTinyAsBoolean=true;charset=utf8;";
        private const string norma2 = "Server=localhost;Database=updatedbase;pooling=true;Uid=root;Pwd=Tr6981001676!;Convert Zero Datetime=True;  default command timeout=3600;SslMode=none;TreatTinyAsBoolean=true;";
        private const string test = "Server=readmoreachill2.clq6srsguoz6.eu-west-3.rds.amazonaws.com;Database=readmore_achill2;pooling=true;Uid=readmore_achill2;Pwd=986239787346;Convert Zero Datetime=True;  default command timeout=3600;SslMode=none;TreatTinyAsBoolean=true;";

        public DateTime Limit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
            .Configure(s => s.HasMaxLength(200).HasColumnType("varchar"));
            modelBuilder.Properties().Where(x => x.PropertyType == typeof(bool))
             .Configure(x => x.HasColumnType("bit"));
            modelBuilder.Entity<Program>().HasMany(p => p.Changes)
                .WithOptional(r => r.Program)
                .HasForeignKey(t => t.Program_Id)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Program>().HasMany(p => p.ShowUpsList)
                .WithOptional(r => r.Prog)
                .HasForeignKey(t => t.Prog_Id)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Payment>().HasMany(p => p.Changes)
              .WithOptional(r => r.Payment)
              .HasForeignKey(t => t.Payment_Id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<ShowUp>().HasMany(p => p.Changes)
              .WithOptional(r => r.ShowUp)
              .HasForeignKey(t => t.ShowUp_Id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<WorkingRule>()
                .HasMany(p => p.DailyWorkingShifts)
             .WithOptional(r => r.WorkingRule)
             .HasForeignKey(t => t.WorkingRule_Id);
             modelBuilder.Entity<GymnastHour>()
                .HasOptional(p => p.Gymnast)
             .WithMany(r => r.GymnastHours)
             .HasForeignKey(t => t.Gymnast_Id);

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