using Microsoft.EntityFrameworkCore;

namespace PersAcc
{
    public partial class ApplicationContext : DbContext
    {
        public virtual DbSet<Bucket> Buckets { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=HomeAcc.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bucket>(entity =>
            {
                entity.ToTable("Bucket");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.DateTime).HasColumnType("bigint");
                entity.Property(e => e.Description).HasColumnType("varchar");
                entity.Property(e => e.Value).HasColumnType("float");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
