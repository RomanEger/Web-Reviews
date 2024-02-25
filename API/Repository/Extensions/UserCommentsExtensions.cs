using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class UserCommentsExtensions
    {
        public static IQueryable<Usercomment> FilterUserCommentByDate(this IQueryable<Usercomment> items, bool dateFiltering) =>
            dateFiltering
            ? items.OrderByDescending(x => x.CommentDate)
            : items;
    }
}
