using System;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace loginapp.Models
{
    public partial class DataAccess : DbContext
    {
        public void RegisterUser(Users user, out String OUTPUT)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = "@ResponseMessage";
            parameter.IsNullable = true;
            parameter.SqlDbType = System.Data.SqlDbType.VarChar;
            parameter.Direction = System.Data.ParameterDirection.Output;
            parameter.Size = 50;

            Database.ExecuteSqlRaw("exec RegisterUser @FirstName, @LastName, @EmailAddress, @UserPassword",
            new SqlParameter("@FirstName", user.FirstName),
            new SqlParameter("@LastName", user.LastName),
            new SqlParameter("@EmailAddress", user.EmailAddress),
            new SqlParameter("@UserPassword", user.UserPassword), parameter);

            OUTPUT = parameter.Value.ToString();
        }

        public bool ValidateUser(Users user)
        {
            SqlParameter para = new SqlParameter("validate", System.Data.SqlDbType.Int, 128);
            para.Direction = ParameterDirection.Output;

            Database.ExecuteSqlRaw("exec @validate = ValidateUser @EmailAddress, @UserPassword", para,
            new SqlParameter("@EmailAddress", user.EmailAddress),
            new SqlParameter("@UserPassword", user.UserPassword));

            if (Convert.ToInt32(para.Value) == 1)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void UpdateUser(Users user, int User)
        {
            Database.ExecuteSqlRaw("exec UpdateUser @UserID, @FirstName, @LastName, @EmailAddress, @UserPassword",
                new SqlParameter("@UserID", user.UserId),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@EmailAddress", user.EmailAddress),
                new SqlParameter("@UserPassword", user.UserPassword));
        }

        public void DeleteUser(int User)
        {
            Database.ExecuteSqlRaw("exec DeleteUser @UserID",
                new SqlParameter("@UserID", User));
        }

        public DataAccess()
        {
        }

        public DataAccess(DbContextOptions<DataAccess> options)
            : base(options)
        {
        }

        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        //       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //       {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=socem1.uopnet.plymouth.ac.uk;Database=COMP2001_LHumphries;User Id=LHumphries;Password=LkxC210+");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Passwords>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserPassword, e.DateChanged })
                    .HasName("PK_PASSWORDSTABLE");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DateChanged).HasColumnType("date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERID");
            });

            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DateAndTime })
                    .HasName("PK_UserID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DateAndTime).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionsUserID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
