using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication.DataAccess
{
    public partial class PasswordManagerContext : DbContext
    {
        public PasswordManagerContext()
        {
        }

        public PasswordManagerContext(DbContextOptions<PasswordManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=passwordmanager.database.windows.net;Initial Catalog=PasswordManager;User ID=AKAK;Password=DotnetParty1!;Trusted_Connection=False;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.SessionId).HasColumnName("sessionId");

                entity.Property(e => e.Url)
                    .HasMaxLength(50)
                    .HasColumnName("url");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Account_Category");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_User");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cvc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cvc");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NumberCard)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numberCard");

                entity.Property(e => e.SessionId).HasColumnName("sessionId");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
