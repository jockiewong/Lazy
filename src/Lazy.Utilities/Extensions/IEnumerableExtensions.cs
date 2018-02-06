using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Lazy.Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            HashSet<T> hs = new HashSet<T>();
            source.ForEach_(r => hs.Add(r));
            return hs;
        }
        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join<T>(separator, source);
        }

        public static void ForEach_<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        public static IReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> list)
        {
            return list.ToImmutableList();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool HasRepeat<T>(this IEnumerable<T> list)
        {
            return list.GroupBy(r => r).Any(g => g.Count() > 1);
        }

        public static bool HasRepeat<T>(this IEnumerable<T> list, Func<T, object> selectProperty)
        {
            return list.GroupBy(selectProperty).Any(g => g.Count() > 1);
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
            return
                query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
        }

        public static object First_(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "First",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static object FirstOrDefault_(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "FirstOrDefault",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static List<object> ToList_(this IQueryable source)
        {
            return source.ToDynamicList();
        }

        public static object Single_(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Single",
                    new Type[] { source.ElementType },
                    source.Expression));
        }

        public static object SingleOrDefault_(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "SingleOrDefault",
                    new Type[] { source.ElementType },
                    source.Expression));
        }
    }
}