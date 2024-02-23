using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Video
{
    public Guid VideoId { get; set; }

    public string Title { get; set; } = null!;

    public Guid VideoTypeId { get; set; }

    public Guid VideoStatusId { get; set; }

    public Guid VideoRestrictionId { get; set; }

    public string? Description { get; set; }

    public int CurrentEpisode { get; set; }

    public int TotalEpisodes { get; set; }

    public DateTime ReleaseDate { get; set; }

    public byte[]? Photo { get; set; }

    public decimal? Rating { get; set; }

    public virtual ICollection<Authorvideo> Authorvideos { get; set; } = new List<Authorvideo>();

    public virtual ICollection<Studiovideo> Studiovideos { get; set; } = new List<Studiovideo>();

    public virtual ICollection<Usercomment> Usercomments { get; set; } = new List<Usercomment>();

    public virtual Videorestriction VideoRestriction { get; set; } = null!;

    public virtual Videostatus VideoStatus { get; set; } = null!;

    public virtual Videotype VideoType { get; set; } = null!;

    public virtual ICollection<Videogenre> Videogenres { get; set; } = new List<Videogenre>();

    public virtual ICollection<Videorating> Videoratings { get; set; } = new List<Videorating>();
}
