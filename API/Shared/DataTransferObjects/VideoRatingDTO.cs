using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record VideoRatingDTO
    {
        public Guid VideoRatingId { get; set; }

        public int Rating { get; init; }

        public Guid UserId { get; init; }

        public Guid VideoId { get; init; }
    }
}
