using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public abstract class RequestParameters
    {
        private const int _maxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = _maxPageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > _maxPageSize ? _maxPageSize : value; }
        }
    }
}
