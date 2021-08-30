using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BT05.Models
{
    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Level> Levels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNum)
                .IsFixedLength();

            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Level)
                .WillCascadeOnDelete(false);
        }
    }
}
