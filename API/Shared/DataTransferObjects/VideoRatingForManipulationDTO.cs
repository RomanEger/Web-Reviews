using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record VideoRatingForManipulationDTO
    {
        [Required(ErrorMessage = "Поле Rating обязательно")]
        [Range(1,10,ErrorMessage ="Рейтинг должен быть в диапазоне от 1 до 10")]
        public int Rating { get; init; }

        [Required(ErrorMessage = "Поле UserId обязательно")]
        public Guid UserId { get; init; }

        [Required(ErrorMessage = "Поле VideoId обязательно")]
        public Guid VideoId { get; init; }
    }
}
