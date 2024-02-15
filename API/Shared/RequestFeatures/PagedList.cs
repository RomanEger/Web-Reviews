using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                PageSize = pageSize,
                TotalCount = count
            };
            AddRange(items);
        }
    }
}
