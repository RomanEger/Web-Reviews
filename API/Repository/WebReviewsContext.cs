using System;
using System.Collections.Generic;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public partial class WebReviewsContext : DbContext
{
    public WebReviewsContext()
    {
    }

    public WebReviewsContext(DbContextOptions<WebReviewsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Authortype> Authortypes { get; set; }

    public virtual DbSet<Authorvideo> Authorvideos { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    public virtual DbSet<Studiovideo> Studiovideos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usercomment> Usercomments { get; set; }

    public virtual DbSet<Userrank> Userranks { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Videogenre> Videogenres { get; set; }

    public virtual DbSet<Videorating> Videoratings { get; set; }

    public virtual DbSet<Videorestriction> Videorestrictions { get; set; }

    public virtual DbSet<Videostatus> Videostatuses { get; set; }

    public virtual DbSet<Videotype> Videotypes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.AuthorId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("author_id");
            entity.Property(e => e.AuthorTypeId).HasColumnName("author_type_id");
            entity.Property(e => e.Birthday)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birthday");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .HasColumnName("middle_name");
            entity.Property(e => e.Photo).HasColumnName("photo");

            entity.HasOne(d => d.AuthorType).WithMany(p => p.Authors)
                .HasForeignKey(d => d.AuthorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_author_type_id_authors_authortypes");
        });

        modelBuilder.Entity<Authortype>(entity =>
        {
            entity.HasKey(e => e.AuthorTypeId).HasName("authortypes_pkey");

            entity.ToTable("authortypes");

            entity.Property(e => e.AuthorTypeId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("author_type_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Authorvideo>(entity =>
        {
            entity.HasKey(e => e.AuthorVideoId).HasName("authorvideo_pkey");

            entity.ToTable("authorvideo");

            entity.Property(e => e.AuthorVideoId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("author_video_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Author).WithMany(p => p.Authorvideos)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("fk_author_id_authorvideo_authors");

            entity.HasOne(d => d.Video).WithMany(p => p.Authorvideos)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video_id_authorvideo_video");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.GenreId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("genre_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.StudioId).HasName("studios_pkey");

            entity.ToTable("studios");

            entity.HasIndex(e => e.Title, "studios_title_key").IsUnique();

            entity.Property(e => e.StudioId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("studio_id");
            entity.Property(e => e.FoundationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("foundation_date");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Studiovideo>(entity =>
        {
            entity.HasKey(e => e.StudioVideoId).HasName("studiovideo_pkey");

            entity.ToTable("studiovideo");

            entity.Property(e => e.StudioVideoId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("studio_video_id");
            entity.Property(e => e.StudioId).HasColumnName("studio_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Studio).WithMany(p => p.Studiovideos)
                .HasForeignKey(d => d.StudioId)
                .HasConstraintName("fk_studio_id_studiovideo_studios");

            entity.HasOne(d => d.Video).WithMany(p => p.Studiovideos)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video_id_studiovideo_video");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Nickname, "users_nickname_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Nickname)
                .HasMaxLength(25)
                .HasColumnName("nickname");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.RefreshToken)
                .HasColumnType("character varying")
                .HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpires)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_expires");
            entity.Property(e => e.UserRankId).HasColumnName("user_rank_id");

            entity.HasOne(d => d.UserRank).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_rank_id_users_userranks");
        });

        modelBuilder.Entity<Usercomment>(entity =>
        {
            entity.HasKey(e => e.UserCommentId).HasName("usercomments_pkey");

            entity.ToTable("usercomments");

            entity.Property(e => e.UserCommentId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("user_comment_id");
            entity.Property(e => e.Advantages)
                .HasMaxLength(800)
                .HasColumnName("advantages");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("comment_date");
            entity.Property(e => e.Disadvantages)
                .HasMaxLength(800)
                .HasColumnName("disadvantages");
            entity.Property(e => e.Text)
                .HasMaxLength(800)
                .HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.User).WithMany(p => p.Usercomments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_id_usercomments_users");

            entity.HasOne(d => d.Video).WithMany(p => p.Usercomments)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video_id_usercomments_video");
        });

        modelBuilder.Entity<Userrank>(entity =>
        {
            entity.HasKey(e => e.UserRankId).HasName("userranks_pkey");

            entity.ToTable("userranks");

            entity.Property(e => e.UserRankId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("user_rank_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("video_pkey");

            entity.ToTable("video");

            entity.HasIndex(e => e.Title, "video_title_key").IsUnique();

            entity.Property(e => e.VideoId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_id");
            entity.Property(e => e.CurrentEpisode)
                .HasDefaultValue(0)
                .HasColumnName("current_episode");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Rating)
                .HasPrecision(4, 2)
                .HasDefaultValueSql("10")
                .HasColumnName("rating");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("release_date");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TotalEpisodes).HasColumnName("total_episodes");
            entity.Property(e => e.VideoRestrictionId).HasColumnName("video_restriction_id");
            entity.Property(e => e.VideoStatusId).HasColumnName("video_status_id");
            entity.Property(e => e.VideoTypeId).HasColumnName("video_type_id");

            entity.HasOne(d => d.VideoRestriction).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoRestrictionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_video_restriction_id_video_videorestrictions");

            entity.HasOne(d => d.VideoStatus).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_video_status_id_video_videostatuses");

            entity.HasOne(d => d.VideoType).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_video_type_id_video_videotypes");
        });

        modelBuilder.Entity<Videogenre>(entity =>
        {
            entity.HasKey(e => e.VideoGenreId).HasName("videogenres_pkey");

            entity.ToTable("videogenres");

            entity.Property(e => e.VideoGenreId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_genre_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.Videogenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_genre_id_videogenres_genres");

            entity.HasOne(d => d.Video).WithMany(p => p.Videogenres)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video_id_videogenres_video");
        });

        modelBuilder.Entity<Videorating>(entity =>
        {
            entity.HasKey(e => e.VideoRatingId).HasName("videoratings_pkey");

            entity.ToTable("videoratings");

            entity.Property(e => e.VideoRatingId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_rating_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.User).WithMany(p => p.Videoratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_id_videoratings_users");

            entity.HasOne(d => d.Video).WithMany(p => p.Videoratings)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video_id_videoratings_videos");
        });

        modelBuilder.Entity<Videorestriction>(entity =>
        {
            entity.HasKey(e => e.VideoRestrictionId).HasName("videorestrictions_pkey");

            entity.ToTable("videorestrictions");

            entity.Property(e => e.VideoRestrictionId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_restriction_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Videostatus>(entity =>
        {
            entity.HasKey(e => e.VideoStatusId).HasName("videostatuses_pkey");

            entity.ToTable("videostatuses");

            entity.Property(e => e.VideoStatusId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Videotype>(entity =>
        {
            entity.HasKey(e => e.VideoTypeId).HasName("videotypes_pkey");

            entity.ToTable("videotypes");

            entity.Property(e => e.VideoTypeId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("video_type_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
