using Lazy.EFBase;
using Lazy.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;

namespace Lazy.EF6
{
    public static class EF6Extensions
    {
        /// <summary>
        /// 获取EF上下文所有的表名称与实体类型Type的对象关系
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<EntityMetaData> GetMetaData(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            List<EntityMetaData> models = new List<EntityMetaData>();

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectItemCollection = ((ObjectItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace));
            var entitySetMappings = objectContext.MetadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace).Single().EntitySetMappings.ToList();
            var entityTypes = objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.OSpace);
            foreach (var item in entityTypes)
            {

                EntityMetaData model = new EntityMetaData();

                EntitySet table = null;

                var mapping = entitySetMappings.SingleOrDefault(x => x.EntitySet.ElementType.Name == item.Name);
                if (mapping != null)
                {
                    table = mapping.EntityTypeMappings.Single().Fragments.Single().StoreEntitySet;
                }
                else
                {
                    mapping = entitySetMappings.SingleOrDefault(x => x.EntityTypeMappings.Where(y => y.EntityType != null).Any(y => y.EntityType.Name == item.Name));

                    if (mapping != null)
                    {
                        table = mapping.EntityTypeMappings.Where(x => x.EntityType != null).Single(x => x.EntityType.Name == item.Name).Fragments.Single().StoreEntitySet;
                    }
                    else
                    {
                        var entitySetMapping = entitySetMappings.Single(x => x.EntityTypeMappings.Any(y => y.IsOfEntityTypes.Any(z => z.Name == item.Name)));
                        table = entitySetMapping.EntityTypeMappings.First(x => x.IsOfEntityTypes.Any(y => y.Name == item.Name)).Fragments.Single().StoreEntitySet;
                    }
                }
                var entityType = entityTypes.Single(r => r.Name == item.Name);
                var type = objectItemCollection.GetClrType(entityType);

                model.TableName = table.MetadataProperties["Table"]?.Value?.ToString() ?? table.Name;
                model.EntityType = type;
                model.EntityName = item.Name;
                model.Schema = table.Schema;

                model.Fields = new List<FieldMetaData>();
                foreach (var fieldMember in table.ElementType.Members)
                {
                    var field = new FieldMetaData();
                    field.ColumnName = fieldMember.Name;
                    field.FieldName = fieldMember.MetadataProperties["PreferredName"].Value.ToString();
                    field.FieldType = ((PrimitiveType)fieldMember.TypeUsage.EdmType).ClrEquivalentType;
                    var edm = ((System.Data.Entity.Core.Metadata.Edm.EdmProperty)fieldMember);

                    field.MaxLength = edm.MaxLength;
                    field.Nullable = edm.Nullable;
                    field.DefaultValue = edm.DefaultValue;
                    field.ColumnType = edm.TypeName;
                    field.IsPrimaryKey = table.ElementType.KeyMembers.Any(r => r.Name == fieldMember.Name);
                    model.Fields.Add(field);
                }
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// 获取上下文所有的已注册的实体类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllEntityTypes(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            List<Type> types = new List<Type>();

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectItemCollection = ((ObjectItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace));
            var entityTypes = objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.OSpace);
            foreach (var item in entityTypes)
            {
                var type = objectItemCollection.GetClrType(item);
                types.Add(type);
            }
            return types;
        }

        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象实体子类
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly<TEntityBase>(this DbModelBuilder modelBuilder, Assembly assembly)
            where TEntityBase : class
        {
            modelBuilder.RegisterEntitiesFromAssembly(assembly, r => !r.IsAbstract && r.IsClass && r.IsChildTypeOf<TEntityBase>());
        }

        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象实体子类
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly(this DbModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> entityTypePredicate)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            //反射得到DbModelBuilder的Entity方法
            var entityMethod = modelBuilder.GetType().GetMethod("Entity");

            //反射得到ConfigurationRegistrar的Add<TEntityType>方法
            var addMethod = typeof(ConfigurationRegistrar)
                   .GetMethods()
                   .Single(m =>
                     m.Name == "Add"
                     && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));
            //扫描所有fluent api配置类,要求父类型必须是EntityTypeConfiguration<TEntityType>
            var configTypes = assembly
                               .DefinedTypes
                               .Where(t =>
                                     !t.IsAbstract && t.BaseType != null && t.IsClass
                                     && t.BaseType.IsGenericType
                                     && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                                     )
                               .ToList();

            HashSet<Type> registedTypes = new HashSet<Type>();

            //存在fluent api配置的类,必须在Entity方法之前调用
            configTypes.ForEach(mappingType =>
            {
                var entityType = mappingType.BaseType.GetGenericArguments().Single();
                if (!entityTypePredicate(entityType))
                    return;
                var map = Activator.CreateInstance(mappingType);
                //反射调用ConfigurationRegistrar的Add方法注册fluent api配置,该方法会同时注册实体
                addMethod.MakeGenericMethod(entityType)
                     .Invoke(modelBuilder.Configurations, new object[] { map });

                registedTypes.Add(entityType);
            });

            //反射调用Entity方法 注册实体
            assembly
                .DefinedTypes
                .Where(entityTypePredicate)
                .ForEach_(r =>
                {
                    entityMethod.MakeGenericMethod(r).Invoke(modelBuilder, new object[0]);
                });
        }
    }
}