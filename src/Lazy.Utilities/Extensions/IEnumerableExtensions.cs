using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
namespace Lazy.Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 转换为HashSet集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new HashSet<T>(source);
        }

        /// <summary>
        /// 转换为HashSet集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return new HashSet<T>(source, comparer);
        }

        /// <summary>
        /// 集合拼接为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return string.Join<T>(separator, source);
        }

        /// <summary>
        /// 循环集合元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach_<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in list)
            {
                action(item);
            }
        }

        /// <summary>
        /// 转换只读集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list.ToImmutableList();
        }

        /// <summary>
        /// 判断集合是否为null或者空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        /// <summary>
        /// 判断集合是否存在重复元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="keyComparer"></param>
        /// <returns></returns>
        public static bool HasRepeat<T>(this IEnumerable<T> list, IEqualityComparer<T> keyComparer = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (keyComparer == null)
                return list.GroupBy(r => r).Any(g => g.Count() > 1);
            else
                return list.GroupBy(r => r, keyComparer).Any(g => g.Count() > 1);
        }

        /// <summary>
        /// 判断集合中是否存在属性相等的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="selectProperty"></param>
        /// <param name="keyComparer"></param>
        /// <returns></returns>
        public static bool HasRepeat<T>(this IEnumerable<T> list, Func<T, object> selectProperty, IEqualityComparer<object> keyComparer = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (selectProperty == null)
            {
                throw new ArgumentNullException(nameof(selectProperty));
            }

            if (keyComparer == null)
                return list.GroupBy(selectProperty).Any(g => g.Count() > 1);
            else
                return list.GroupBy(selectProperty, keyComparer).Any(g => g.Count() > 1);
        }


        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> list, bool condition, Func<T, bool> where)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (condition)
                return list.Where(where);
            return list;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> list, bool condition, Expression<Func<T, bool>> where)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (condition)
                return list.Where(where);
            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询源</param>
        /// <param name="pageIndex">当前页,索引从1开始</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public static IQueryable Paging(this IQueryable query, int pageIndex, int pageSize)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return
                query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
        }

        public static object First_(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "First",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static object FirstOrDefault_(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "FirstOrDefault",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static List<object> ToList_(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDynamicList();
        }

        public static object Single_(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Single",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static object SingleOrDefault_(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "SingleOrDefault",
                    new Type[] { source.ElementType },
                    source.Expression));
        }
    }
}