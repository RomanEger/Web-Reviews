using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Authortype
{
    public Guid AuthorTypeId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
