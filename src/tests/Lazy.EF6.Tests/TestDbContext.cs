using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lazy.EF6;
using Lazy.EFBase;
using Lazy.Utilities.Extensions;

namespace Lazy.EF6.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext() : base("default")
        {
            Database.SetInitializer<TestDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
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

    public class mapping : EntityTypeConfiguration<EntityA>
    {
        public mapping()
        {
            ToTable("Entity__A");
            HasKey(r => r.Id);
        }
    }
}
