using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record VideoForManipulationDTO
    {
        [Required(ErrorMessage ="Поле название обязательно")]
        [MaxLength(100, ErrorMessage ="Название должно быть меньше 100 символов")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Поле VideoTypeId обязательно")]
        public Guid VideoTypeId { get; init; }

        [Required(ErrorMessage = "Поле VideoStatusId обязательно")]
        public Guid VideoStatusId { get; init; }

        [Required(ErrorMessage = "Поле VideoRestrictionId обязательно")]
        public Guid VideoRestrictionId { get; init; }

        public string? Description { get; init; }

        [Required(ErrorMessage = "Поле CurrentEpisode обязательно")]
        [Range(1, 2000, ErrorMessage = "Поле CurrentEpisode не может быть меньше 1 и больше чем 2000")]
        public int CurrentEpisode { get; init; }

        [Required(ErrorMessage = "Поле TotalEpisodes обязательно")]
        public int TotalEpisodes { get; init; }

        [Required(ErrorMessage = "Поле ReleaseDate обязательно")]
        public DateTime ReleaseDate { get; init; }

        public byte[]? Photo { get; init; }

        [Range(1,10, ErrorMessage ="Поле Rating должно быть между 1 и 10")]
        public decimal? Rating { get; init; }
    }
}
