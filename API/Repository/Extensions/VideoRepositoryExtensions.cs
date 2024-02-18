using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class VideoRepositoryExtensions
    {
        public static IQueryable<Video> GetVideosByName(this IQueryable<Video> items, string? searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return items;

            var lowerSeachTitle = searchTitle.ToLower().Trim();

            return items.Where(x => x.Title.ToLower().Contains(lowerSeachTitle));
        }

        public static bool IsOrdered<T>(this IQueryable<T> items)
        {
            if (items == null)
                return false;
            return items.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> SmartOrderBy<T, TKey>(this IQueryable<T> items, Expression<Func<T, TKey>> expression)
        {
            if (items.IsOrdered())
            {
                var orderedItems = items as IOrderedQueryable<T>;
                return orderedItems.ThenBy(expression);
            }
            else
                return items.OrderBy(expression);
        }

        public static IQueryable<T> SmartOrderByDescending<T, TKey>(this IQueryable<T> items, Expression<Func<T, TKey>> expression)
        {
            if (items.IsOrdered())
            {
                var orderedItems = items as IOrderedQueryable<T>;
                return orderedItems.ThenByDescending(expression);
            }
            else
                return items.OrderByDescending(expression);
        }

        public static IQueryable<Video> OrderByDateDescending(this IQueryable<Video> items, bool dateFiltering) =>
            dateFiltering
            ? SmartOrderByDescending(items, x => x.ReleaseDate)
            : items;

        public static IQueryable<Video> OrderByRatingDescending(this IQueryable<Video> items, bool ratingFiltering) =>
            ratingFiltering
            ? SmartOrderByDescending(items, x => x.Rating)
            : items;

        public static IQueryable<Video> OrderByAlphabet(this IQueryable<Video> items, bool ratingFiltering) =>
            ratingFiltering
            ? SmartOrderBy(items, x => x.Title)
            : items;
    }
}
