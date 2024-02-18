using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record VideoDTO
    {
        public Guid VideoId { get; init; }

        public string Title { get; init; }

        public Guid VideoTypeId { get; init; }

        public Guid VideoStatusId { get; init; }

        public Guid VideoRestrictionId { get; init; }

        public string? Description { get; init; }

        public int CurrentEpisode { get; init; }

        public int TotalEpisodes { get; init; }

        public DateTime ReleaseDate { get; init; }

        public byte[]? Photo { get; init; }

        public decimal? Rating { get; init; }
    }
}
