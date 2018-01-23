using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Lazy.Utilities.Extensions;
using Lazy.EFBase;
using Lazy.EF6;

namespace Lazy.EF6.Tests
{
    public class Extensions_Tests
    {
        [Fact]
        public void tests()
        {

            TestDbContext context = new TestDbContext();
            
            var metaDatas = context.GetMetaData();
            metaDatas.Count.ShouldBe(2);
            var m = metaDatas.FirstOrDefault();
            m.ShouldNotBeNull();

            m.EntityType.ShouldNotBeOfType<EntityA>();
            m.EntityName.ShouldBe("EntityA");
            m.TableName.ShouldBe("Entity__A");
            m.Fields.Count.ShouldBe(3);
            var f = m.Fields.FirstOrDefault(r => r.FieldName == "Name");
            f.ShouldNotBeNull();
            f.ColumnName.ShouldBe("Name");
            f.ColumnType.ShouldBe("nvarchar");
            f.DefaultValue.ShouldBe(null);
            f.MaxLength.ShouldBe(100);
            f.Nullable.ShouldBe(false);
            f.IsPrimaryKey.ShouldBe(false);

            var fId = m.Fields.FirstOrDefault(r => r.FieldName == "Id");

            fId.ShouldNotBeNull();
            fId.ColumnName.ShouldBe("AId");
            fId.IsPrimaryKey.ShouldBeTrue();
        }
    }
}
