using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class VideoParameters : RequestParameters
    {
        public string? SearchTitle { get; set; }
        public bool DateFiltering { get; set; } = false;
        public bool RatingFiltering { get; set; } = false;
        public bool AlphabetFiltering { get; set; } = false;
        public List<Guid>? GenreIds { get; set; }
    }
}
