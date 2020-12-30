using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GpnWebApi.Models;

#nullable disable

namespace GpnWebApi.EF
{
    public partial class GpnContext : DbContext
    {
        public GpnContext()
        {
        }

        public GpnContext(DbContextOptions<GpnContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Indicator> Indicators { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //                optionsBuilder.UseSqlServer("Name=GpnDB");
                optionsBuilder.UseSqlServer("Data Source=MICROBE\\SMPPDB1F2017;Initial Catalog=GPN;User ID=SmsMessageCenter;Password=sms_mcc@0802121;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Indicator>(entity =>
            {
                entity.ToTable("INDICATORS");

                entity.HasComment("Индикаторы силосных башен");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("Код индикатора");

                entity.Property(e => e.MaxValue)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("MAX_VALUE")
                    .HasComment("Максимальный уровень");

                entity.Property(e => e.MinValue)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("MIN_VALUE")
                    .HasComment("Минимальный уровень");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("TITLE")
                    .HasComment("Наименование");

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("VALUE")
                    .HasComment("Уровень жидкости");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Rid);

                entity.ToTable("ROLES");

                entity.HasIndex(e => e.Rname, "ROLES_NAME_UNC")
                    .IsUnique();

                entity.Property(e => e.Rid)
                    .HasColumnName("RID")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("Код записи");

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("RNAME")
                    .HasComment("Роль");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("USERS");

                entity.HasComment("Пользователи");

                entity.HasIndex(e => e.Uemail, "USERS_EMAIL_UNC")
                    .IsUnique();

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("Код записи");

                entity.Property(e => e.Uemail)
                    .IsRequired()
                    .HasMaxLength(320)
                    .HasColumnName("UEMAIL");

                entity.Property(e => e.Upassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("UPASSWORD");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Urid);

                entity.ToTable("USER_ROLES");

                entity.HasComment("Роли пользователя");

                entity.HasIndex(e => new { e.UrUid, e.UrRid }, "USER_ROLES_USER_ROLE_UNC")
                    .IsUnique();

                entity.Property(e => e.Urid)
                    .HasColumnName("URID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.UrRid)
                    .HasColumnName("UR_RID")
                    .HasComment("Код роли пользователя");

                entity.Property(e => e.UrUid)
                    .HasColumnName("UR_UID")
                    .HasComment("Код пользователя");

                entity.HasOne(d => d.UrR)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UrRid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_ROLES_ROLES");

                entity.HasOne(d => d.UrU)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UrUid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_ROLES_USERS1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
