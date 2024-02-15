using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Studiovideo
    {
        public Guid StudioVideoId { get; set; }

        public Guid VideoId { get; set; }

        public Guid StudioId { get; set; }

        public virtual Studio Studio { get; set; } = null!;

        public virtual Video Video { get; set; } = null!;
    }
}
