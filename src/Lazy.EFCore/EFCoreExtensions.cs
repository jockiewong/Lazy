using Lazy.EFBase;
using Lazy.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lazy.EFCore
{
    public static class EFCoreExtensions
    {
        /// <summary>
        /// 获取EF上下文所有的表名称与实体类型Type的对象关系
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IReadOnlyList<EntityMetaData> GetMetaData(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var entityTypes = context.Model.GetEntityTypes();

            var result = entityTypes.Select(r =>
            {
                var table = r.Relational();

                return new EntityMetaData
                {
                    EntityName = r.ClrType.Name,
                    TableName = table.TableName,
                    EntityType = r.ClrType,
                    Schema = table.Schema,
                    Fields = r
                          .GetProperties()
                          .Select(p =>
                          {
                              var filed = p.Relational();
                              return new FieldMetaData
                              {
                                  ColumnName = filed.ColumnName,
                                  FieldType = p.ClrType,
                                  FieldName = p.Name,
                                  //去掉类型中的长度,保持和ef6一致
                                  ColumnType = filed.ColumnType?.ReplaceIgnoreCase(@"\(\d+\)$", ""),
                                  DefaultValue = filed.DefaultValue,
                                  Nullable = p.IsNullable,
                                  MaxLength = p.GetMaxLength(),
                                  IsPrimaryKey = p.IsPrimaryKey()
                                  //ef6中获取不到外键数据
                                  //IsForeignKey = p.IsForeignKey()
                              };
                          })
                          .ToList()
                };
            }).ToList();

            return result.AsReadOnly();
        }

        /// <summary>
        /// 获取上下文所有的已注册的实体类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IReadOnlyList<Type> GetAllEntityTypes(this DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Model.GetEntityTypes().Select(r => r.ClrType).AsReadOnly();
        }

        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象子类为实体
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly<TEntityBase>(this ModelBuilder modelBuilder, Assembly assembly)
            where TEntityBase : class
        {
            modelBuilder.RegisterEntitiesFromAssembly(assembly, r => !r.IsAbstract && r.IsClass && r.IsChildTypeOf<TEntityBase>());
        }

        /// <summary>
        /// 注册某个程序集中所有<typeparamref name="TEntityBase"/>的非抽象子类为实体
        /// </summary>
        /// <typeparam name="TEntityBase">实体基类</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly">注册程序集</param>
        /// <param name="assembly">注册程序集</param>
        public static void RegisterEntitiesFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool> entityTypePredicate)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            //反射得到ModelBuilder的ApplyConfiguration<TEntity>(...)方法
            var applyConfigurationMethod = modelBuilder.GetType().GetMethod("ApplyConfiguration");

            //所有fluent api配置类
            var configTypes = assembly
                               .GetTypesSafely()
                               .Where(t =>
                                 !t.IsAbstract && t.BaseType != null && t.IsClass
                                 && t.IsChildTypeOfGenericType(typeof(IEntityTypeConfiguration<>))).ToList();

            HashSet<Type> registedTypes = new HashSet<Type>();
            //存在fluent api配置的类,必须在Entity方法之前调用
            configTypes.ForEach(mappingType =>
            {
                var entityType = mappingType.GetTypeInfo().ImplementedInterfaces.First().GetGenericArguments().Single();

                //如果不满足条件的实体,不注册
                if (!entityTypePredicate(entityType))
                    return;

                var map = Activator.CreateInstance(mappingType);
                applyConfigurationMethod.MakeGenericMethod(entityType)
                     .Invoke(modelBuilder, new object[] { map });

                registedTypes.Add(entityType);
            });

            assembly
                .GetTypesSafely()
                .Where(r => !registedTypes.Contains(r))
                .Where(entityTypePredicate)
                .ForEach_(r =>
                {
                    //直接调用Entity方法注册实体
                    modelBuilder.Entity(r);
                });
        }
    }
}