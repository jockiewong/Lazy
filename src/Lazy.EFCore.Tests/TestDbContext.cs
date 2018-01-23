using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lazy.EFBase;
using Lazy.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lazy.EFCore.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=lazytest;Integrated Security=True;Pooling=False");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterEntitiesFromAssembly<IEntity>(this.GetType().Assembly);
        }
    }

    public interface IEntity { }

    [Table("Entity_A")]
    public class EntityA : IEntity
    {
        [Key, Column("AId")]
        public Guid Id { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }

        public Guid BId { get; set; }

        [ForeignKey("BId")]
        public EntityB EntityB { get; set; }

    }

    [Table("Entity_B")]
    public class EntityB : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }
    }

    public class mapping : IEntityTypeConfiguration<EntityA>
    {

        public void Configure(EntityTypeBuilder<EntityA> builder)
        {
            builder.ToTable("Entity__A");
            builder.HasKey(r => r.Id);
        }
    }
}
