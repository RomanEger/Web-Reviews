using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserCommentForManipulationDTO
    {
        [Required(ErrorMessage = "Поле text обязательно")]
        [Length(minimumLength: 10, maximumLength: 1000, ErrorMessage = "Содержание должно быть между 10 и 1000 символами")]
        public string Text { get; init; }

        [Required(ErrorMessage = "User id поле обязательно")]
        public Guid UserId { get; init; }

        [Required(ErrorMessage = "Video id поле обязательно")]
        public Guid VideoId { get; init; }

        [Length(minimumLength: 10, maximumLength: 1000, ErrorMessage = "Содержание недостатков должно быть между 10 и 1000 символами")]
        public string? Disadvantages { get; init; }

        [Length(minimumLength: 10, maximumLength: 1000, ErrorMessage = "Содержание преимуществ должно быть между 10 и 1000 символами")]
        public string? Advantages { get; init; }

        [Required(ErrorMessage = "Comment date поле обязательно")]
        public DateTime CommentDate { get; init; }
    }
}
